namespace ComPmpaLibB
{
    partial class frmPmpaDrgCode
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
            this.expandablePanel5 = new DevComponents.DotNetBar.ExpandablePanel();
            this.panSuga = new System.Windows.Forms.Panel();
            this.SS2 = new FarPoint.Win.Spread.FpSpread();
            this.SS2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SS6 = new FarPoint.Win.Spread.FpSpread();
            this.SS6_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSplit = new System.Windows.Forms.Panel();
            this.SS5 = new FarPoint.Win.Spread.FpSpread();
            this.SS5_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.cboOgAdd = new System.Windows.Forms.ComboBox();
            this.cboTrunc = new System.Windows.Forms.ComboBox();
            this.cboNgt = new System.Windows.Forms.ComboBox();
            this.cboGbn = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SS3 = new FarPoint.Win.Spread.FpSpread();
            this.SS3_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.txtDrgName = new System.Windows.Forms.TextBox();
            this.txtDrgCode = new System.Windows.Forms.TextBox();
            this.panCodeList = new System.Windows.Forms.Panel();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.종료XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.저장SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.새로고침RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandablePanel5.SuspendLayout();
            this.panSuga.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS2_Sheet1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS6_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS5_Sheet1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS3_Sheet1)).BeginInit();
            this.panCodeList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // expandablePanel5
            // 
            this.expandablePanel5.CanvasColor = System.Drawing.SystemColors.Control;
            this.expandablePanel5.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.expandablePanel5.Controls.Add(this.panSuga);
            this.expandablePanel5.Controls.Add(this.panCodeList);
            this.expandablePanel5.Controls.Add(this.menuStrip1);
            this.expandablePanel5.DisabledBackColor = System.Drawing.Color.Empty;
            this.expandablePanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.expandablePanel5.ExpandButtonVisible = false;
            this.expandablePanel5.HideControlsWhenCollapsed = true;
            this.expandablePanel5.Location = new System.Drawing.Point(0, 0);
            this.expandablePanel5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.expandablePanel5.Name = "expandablePanel5";
            this.expandablePanel5.Size = new System.Drawing.Size(959, 901);
            this.expandablePanel5.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel5.Style.BackColor1.Color = System.Drawing.SystemColors.Window;
            this.expandablePanel5.Style.BackColor2.Color = System.Drawing.SystemColors.Window;
            this.expandablePanel5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.expandablePanel5.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.expandablePanel5.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandablePanel5.Style.GradientAngle = 90;
            this.expandablePanel5.TabIndex = 131;
            this.expandablePanel5.TitleHeight = 32;
            this.expandablePanel5.TitleStyle.BackColor1.Color = System.Drawing.Color.CornflowerBlue;
            this.expandablePanel5.TitleStyle.BackColor2.Color = System.Drawing.Color.CornflowerBlue;
            this.expandablePanel5.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.expandablePanel5.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandablePanel5.TitleStyle.ForeColor.Color = System.Drawing.Color.White;
            this.expandablePanel5.TitleStyle.GradientAngle = 90;
            this.expandablePanel5.TitleText = "   DRG 기초코드 관리";
            // 
            // panSuga
            // 
            this.panSuga.Controls.Add(this.SS2);
            this.panSuga.Controls.Add(this.groupBox2);
            this.panSuga.Controls.Add(this.groupBox1);
            this.panSuga.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSuga.Location = new System.Drawing.Point(125, 56);
            this.panSuga.Name = "panSuga";
            this.panSuga.Padding = new System.Windows.Forms.Padding(3);
            this.panSuga.Size = new System.Drawing.Size(834, 845);
            this.panSuga.TabIndex = 5;
            // 
            // SS2
            // 
            this.SS2.AccessibleDescription = "SS2, Sheet1, Row 0, Column 0, 일         수";
            this.SS2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SS2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS2.Location = new System.Drawing.Point(3, 354);
            this.SS2.Name = "SS2";
            this.SS2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS2_Sheet1});
            this.SS2.Size = new System.Drawing.Size(828, 488);
            this.SS2.TabIndex = 11;
            this.SS2.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            // 
            // SS2_Sheet1
            // 
            this.SS2_Sheet1.Reset();
            this.SS2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS2_Sheet1.ColumnCount = 6;
            this.SS2_Sheet1.RowCount = 24;
            this.SS2_Sheet1.Cells.Get(0, 0).BackColor = System.Drawing.Color.PeachPuff;
            this.SS2_Sheet1.Cells.Get(0, 0).Value = "일         수";
            this.SS2_Sheet1.Cells.Get(0, 1).Value = "1일";
            this.SS2_Sheet1.Cells.Get(0, 2).Value = "2일";
            this.SS2_Sheet1.Cells.Get(0, 3).Value = "3일";
            this.SS2_Sheet1.Cells.Get(0, 4).Value = "4일";
            this.SS2_Sheet1.Cells.Get(0, 5).Value = "5일";
            this.SS2_Sheet1.Cells.Get(1, 0).Value = "청   구   액";
            this.SS2_Sheet1.Cells.Get(1, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(1, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(1, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(1, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(1, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(1, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(1, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(1, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(1, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(1, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(1, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(1, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(1, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(1, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(1, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(1, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(1, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(1, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(1, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(1, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(1, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(1, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(1, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(1, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(1, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(1, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(1, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(1, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(1, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(1, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(2, 0).Value = "본인부담액";
            this.SS2_Sheet1.Cells.Get(2, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(2, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(2, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(2, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(2, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(2, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(2, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(2, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(2, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(2, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(2, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(2, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(2, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(2, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(2, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(2, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(2, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(2, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(2, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(2, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(2, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(2, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(2, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(2, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(2, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(2, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(2, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(2, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(2, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(2, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(3, 0).Value = "총         액";
            this.SS2_Sheet1.Cells.Get(3, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(3, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(3, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(3, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(3, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(3, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(3, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(3, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(3, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(3, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(3, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(3, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(3, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(3, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(3, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(3, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(3, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(3, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(3, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(3, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(3, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(3, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(3, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(3, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(3, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(3, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(3, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(3, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(3, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(3, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(4, 0).BackColor = System.Drawing.Color.PeachPuff;
            this.SS2_Sheet1.Cells.Get(4, 0).Value = "일         수";
            this.SS2_Sheet1.Cells.Get(4, 1).Value = "6일";
            this.SS2_Sheet1.Cells.Get(4, 2).Value = "7일";
            this.SS2_Sheet1.Cells.Get(4, 3).Value = "8일";
            this.SS2_Sheet1.Cells.Get(4, 4).Value = "9일";
            this.SS2_Sheet1.Cells.Get(4, 5).Value = "10일";
            this.SS2_Sheet1.Cells.Get(5, 0).Value = "청   구   액";
            this.SS2_Sheet1.Cells.Get(5, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(5, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(5, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(5, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(5, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(5, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(5, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(5, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(5, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(5, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(5, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(5, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(5, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(5, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(5, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(5, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(5, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(5, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(5, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(5, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(5, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(5, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(5, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(5, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(5, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(5, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(5, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(5, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(5, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(5, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(6, 0).Value = "본인부담액";
            this.SS2_Sheet1.Cells.Get(6, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(6, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(6, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(6, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(6, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(6, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(6, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(6, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(6, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(6, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(6, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(6, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(6, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(6, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(6, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(6, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(6, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(6, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(6, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(6, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(6, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(6, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(6, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(6, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(6, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(6, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(6, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(6, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(6, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(6, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(7, 0).Value = "총         액";
            this.SS2_Sheet1.Cells.Get(7, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(7, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(7, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(7, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(7, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(7, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(7, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(7, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(7, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(7, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(7, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(7, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(7, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(7, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(7, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(7, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(7, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(7, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(7, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(7, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(7, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(7, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(7, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(7, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(7, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(7, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(7, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(7, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(7, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(7, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(8, 0).BackColor = System.Drawing.Color.PeachPuff;
            this.SS2_Sheet1.Cells.Get(8, 0).Value = "일         수";
            this.SS2_Sheet1.Cells.Get(8, 1).Value = "11일";
            this.SS2_Sheet1.Cells.Get(8, 2).Value = "12일";
            this.SS2_Sheet1.Cells.Get(8, 3).Value = "13일";
            this.SS2_Sheet1.Cells.Get(8, 4).Value = "14일";
            this.SS2_Sheet1.Cells.Get(8, 5).Value = "15일";
            this.SS2_Sheet1.Cells.Get(9, 0).Value = "청   구   액";
            this.SS2_Sheet1.Cells.Get(9, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(9, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(9, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(9, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(9, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(9, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(9, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(9, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(9, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(9, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(9, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(9, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(9, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(9, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(9, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(9, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(9, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(9, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(9, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(9, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(9, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(9, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(9, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(9, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(9, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(9, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(9, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(9, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(9, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(9, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(10, 0).Value = "본인부담액";
            this.SS2_Sheet1.Cells.Get(10, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(10, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(10, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(10, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(10, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(10, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(10, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(10, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(10, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(10, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(10, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(10, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(10, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(10, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(10, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(10, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(10, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(10, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(10, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(10, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(10, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(10, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(10, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(10, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(10, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(10, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(10, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(10, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(10, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(10, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(11, 0).Value = "총         액";
            this.SS2_Sheet1.Cells.Get(11, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(11, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(11, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(11, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(11, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(11, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(11, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(11, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(11, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(11, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(11, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(11, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(11, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(11, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(11, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(11, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(11, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(11, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(11, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(11, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(11, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(11, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(11, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(11, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(11, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(11, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(11, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(11, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(11, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(11, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(12, 0).BackColor = System.Drawing.Color.PeachPuff;
            this.SS2_Sheet1.Cells.Get(12, 0).Value = "일         수";
            this.SS2_Sheet1.Cells.Get(12, 1).Value = "16일";
            this.SS2_Sheet1.Cells.Get(12, 2).Value = "17일";
            this.SS2_Sheet1.Cells.Get(12, 3).Value = "18일";
            this.SS2_Sheet1.Cells.Get(12, 4).Value = "19일";
            this.SS2_Sheet1.Cells.Get(12, 5).Value = "20일";
            this.SS2_Sheet1.Cells.Get(13, 0).Value = "청   구   액";
            this.SS2_Sheet1.Cells.Get(13, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(13, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(13, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(13, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(13, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(13, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(13, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(13, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(13, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(13, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(13, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(13, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(13, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(13, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(13, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(13, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(13, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(13, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(13, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(13, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(13, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(13, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(13, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(13, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(13, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(13, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(13, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(13, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(13, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(13, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(14, 0).Value = "본인부담액";
            this.SS2_Sheet1.Cells.Get(14, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(14, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(14, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(14, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(14, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(14, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(14, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(14, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(14, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(14, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(14, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(14, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(14, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(14, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(14, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(14, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(14, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(14, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(14, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(14, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(14, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(14, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(14, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(14, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(14, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(14, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(14, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(14, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(14, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(14, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(15, 0).Value = "총         액";
            this.SS2_Sheet1.Cells.Get(15, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(15, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(15, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(15, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(15, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(15, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(15, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(15, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(15, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(15, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(15, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(15, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(15, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(15, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(15, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(15, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(15, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(15, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(15, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(15, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(15, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(15, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(15, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(15, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(15, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(15, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(15, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(15, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(15, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(15, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(16, 0).BackColor = System.Drawing.Color.PeachPuff;
            this.SS2_Sheet1.Cells.Get(16, 0).Value = "일         수";
            this.SS2_Sheet1.Cells.Get(16, 1).Value = "21일";
            this.SS2_Sheet1.Cells.Get(16, 2).Value = "22일";
            this.SS2_Sheet1.Cells.Get(16, 3).Value = "23일";
            this.SS2_Sheet1.Cells.Get(16, 4).Value = "24일";
            this.SS2_Sheet1.Cells.Get(16, 5).Value = "25일";
            this.SS2_Sheet1.Cells.Get(17, 0).Value = "청   구   액";
            this.SS2_Sheet1.Cells.Get(17, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(17, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(17, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(17, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(17, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(17, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(17, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(17, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(17, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(17, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(17, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(17, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(17, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(17, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(17, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(17, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(17, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(17, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(17, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(17, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(17, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(17, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(17, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(17, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(17, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(17, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(17, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(17, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(17, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(17, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(18, 0).Value = "본인부담액";
            this.SS2_Sheet1.Cells.Get(18, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(18, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(18, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(18, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(18, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(18, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(18, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(18, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(18, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(18, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(18, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(18, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(18, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(18, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(18, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(18, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(18, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(18, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(18, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(18, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(18, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(18, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(18, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(18, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(18, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(18, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(18, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(18, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(18, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(18, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(19, 0).Value = "총         액";
            this.SS2_Sheet1.Cells.Get(19, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(19, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(19, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(19, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(19, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(19, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(19, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(19, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(19, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(19, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(19, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(19, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(19, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(19, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(19, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(19, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(19, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(19, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(19, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(19, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(19, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(19, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(19, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(19, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(19, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(19, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(19, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(19, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(19, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(19, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(20, 0).BackColor = System.Drawing.Color.PeachPuff;
            this.SS2_Sheet1.Cells.Get(20, 0).Value = "일         수";
            this.SS2_Sheet1.Cells.Get(20, 1).Value = "26일";
            this.SS2_Sheet1.Cells.Get(20, 2).Value = "27일";
            this.SS2_Sheet1.Cells.Get(20, 3).Value = "28일";
            this.SS2_Sheet1.Cells.Get(20, 4).Value = "29일";
            this.SS2_Sheet1.Cells.Get(20, 5).Value = "30일";
            this.SS2_Sheet1.Cells.Get(21, 0).Value = "청   구   액";
            this.SS2_Sheet1.Cells.Get(21, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(21, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(21, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(21, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(21, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(21, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(21, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(21, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(21, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(21, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(21, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(21, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(21, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(21, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(21, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(21, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(21, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(21, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(21, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(21, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(21, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(21, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(21, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(21, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(21, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(21, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(21, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(21, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(21, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(21, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(22, 0).Value = "본인부담액";
            this.SS2_Sheet1.Cells.Get(22, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(22, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(22, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(22, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(22, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(22, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(22, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(22, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(22, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(22, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(22, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(22, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(22, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(22, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(22, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(22, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(22, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(22, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(22, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(22, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(22, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(22, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(22, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(22, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(22, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(22, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(22, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(22, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(22, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(22, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(23, 0).Value = "총         액";
            this.SS2_Sheet1.Cells.Get(23, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(23, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(23, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(23, 1).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(23, 1).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(23, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(23, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(23, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(23, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(23, 2).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(23, 2).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(23, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(23, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(23, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(23, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(23, 3).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(23, 3).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(23, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(23, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(23, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(23, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(23, 4).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(23, 4).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(23, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Cells.Get(23, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS2_Sheet1.Cells.Get(23, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS2_Sheet1.Cells.Get(23, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.SS2_Sheet1.Cells.Get(23, 5).ParseFormatString = "n";
            this.SS2_Sheet1.Cells.Get(23, 5).Value = 999999999;
            this.SS2_Sheet1.Cells.Get(23, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.ColumnHeader.Visible = false;
            this.SS2_Sheet1.Columns.Get(0).BackColor = System.Drawing.Color.PeachPuff;
            this.SS2_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS2_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Columns.Get(0).Width = 110F;
            this.SS2_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS2_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Columns.Get(1).Width = 142F;
            this.SS2_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS2_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Columns.Get(2).Width = 142F;
            this.SS2_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS2_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Columns.Get(3).Width = 142F;
            this.SS2_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS2_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Columns.Get(4).Width = 142F;
            this.SS2_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS2_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2_Sheet1.Columns.Get(5).Width = 142F;
            this.SS2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS2_Sheet1.RowHeader.Visible = false;
            this.SS2_Sheet1.Rows.Get(0).BackColor = System.Drawing.Color.LightSteelBlue;
            this.SS2_Sheet1.Rows.Get(3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(207)))));
            this.SS2_Sheet1.Rows.Get(4).BackColor = System.Drawing.Color.LightSteelBlue;
            this.SS2_Sheet1.Rows.Get(7).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(207)))));
            this.SS2_Sheet1.Rows.Get(8).BackColor = System.Drawing.Color.LightSteelBlue;
            this.SS2_Sheet1.Rows.Get(11).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(207)))));
            this.SS2_Sheet1.Rows.Get(12).BackColor = System.Drawing.Color.LightSteelBlue;
            this.SS2_Sheet1.Rows.Get(15).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(207)))));
            this.SS2_Sheet1.Rows.Get(16).BackColor = System.Drawing.Color.LightSteelBlue;
            this.SS2_Sheet1.Rows.Get(19).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(207)))));
            this.SS2_Sheet1.Rows.Get(20).BackColor = System.Drawing.Color.LightSteelBlue;
            this.SS2_Sheet1.Rows.Get(23).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(207)))));
            this.SS2_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(198)))), ((int)(((byte)(198)))));
            this.SS2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.SS6);
            this.groupBox2.Controls.Add(this.panSplit);
            this.groupBox2.Controls.Add(this.SS5);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 207);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(828, 141);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "관련수가";
            // 
            // SS6
            // 
            this.SS6.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.SS6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS6.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS6.Location = new System.Drawing.Point(429, 19);
            this.SS6.Name = "SS6";
            this.SS6.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS6_Sheet1});
            this.SS6.Size = new System.Drawing.Size(396, 119);
            this.SS6.TabIndex = 2;
            this.SS6.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS6.Change += new FarPoint.Win.Spread.ChangeEventHandler(this.SS6_Change);
            // 
            // SS6_Sheet1
            // 
            this.SS6_Sheet1.Reset();
            this.SS6_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS6_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS6_Sheet1.ColumnCount = 3;
            this.SS6_Sheet1.RowCount = 10;
            this.SS6_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS6_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS6_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SS6_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS6_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "수가코드";
            this.SS6_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "수가명칭";
            this.SS6_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "Rowid";
            this.SS6_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS6_Sheet1.Columns.Get(0).Label = "수가코드";
            this.SS6_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS6_Sheet1.Columns.Get(0).Width = 93F;
            this.SS6_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS6_Sheet1.Columns.Get(1).Label = "수가명칭";
            this.SS6_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS6_Sheet1.Columns.Get(1).Width = 246F;
            this.SS6_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS6_Sheet1.Columns.Get(2).Label = "Rowid";
            this.SS6_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS6_Sheet1.Columns.Get(2).Visible = false;
            this.SS6_Sheet1.Columns.Get(2).Width = 121F;
            this.SS6_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS6_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS6_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS6_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS6_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS6_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS6_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS6_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSplit
            // 
            this.panSplit.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSplit.Location = new System.Drawing.Point(379, 19);
            this.panSplit.Name = "panSplit";
            this.panSplit.Size = new System.Drawing.Size(50, 119);
            this.panSplit.TabIndex = 1;
            // 
            // SS5
            // 
            this.SS5.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.SS5.Dock = System.Windows.Forms.DockStyle.Left;
            this.SS5.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS5.Location = new System.Drawing.Point(3, 19);
            this.SS5.Name = "SS5";
            this.SS5.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS5_Sheet1});
            this.SS5.Size = new System.Drawing.Size(376, 119);
            this.SS5.TabIndex = 0;
            this.SS5.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS5.Change += new FarPoint.Win.Spread.ChangeEventHandler(this.SS5_Change);
            // 
            // SS5_Sheet1
            // 
            this.SS5_Sheet1.Reset();
            this.SS5_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS5_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS5_Sheet1.ColumnCount = 3;
            this.SS5_Sheet1.RowCount = 10;
            this.SS5_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "수가코드";
            this.SS5_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "수가명칭";
            this.SS5_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "Rowid";
            this.SS5_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS5_Sheet1.Columns.Get(0).Label = "수가코드";
            this.SS5_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS5_Sheet1.Columns.Get(0).Width = 93F;
            this.SS5_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS5_Sheet1.Columns.Get(1).Label = "수가명칭";
            this.SS5_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS5_Sheet1.Columns.Get(1).Width = 225F;
            this.SS5_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS5_Sheet1.Columns.Get(2).Label = "Rowid";
            this.SS5_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS5_Sheet1.Columns.Get(2).Visible = false;
            this.SS5_Sheet1.Columns.Get(2).Width = 121F;
            this.SS5_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS5_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS5_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS5_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.cboOgAdd);
            this.groupBox1.Controls.Add(this.cboTrunc);
            this.groupBox1.Controls.Add(this.cboNgt);
            this.groupBox1.Controls.Add(this.cboGbn);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.SS3);
            this.groupBox1.Controls.Add(this.txtDrgName);
            this.groupBox1.Controls.Add(this.txtDrgCode);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(828, 204);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "수가정보";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(6, 51);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 29);
            this.btnSave.TabIndex = 136;
            this.btnSave.Text = "등록(&O)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cboOgAdd
            // 
            this.cboOgAdd.FormattingEnabled = true;
            this.cboOgAdd.Location = new System.Drawing.Point(631, 86);
            this.cboOgAdd.Name = "cboOgAdd";
            this.cboOgAdd.Size = new System.Drawing.Size(183, 23);
            this.cboOgAdd.TabIndex = 7;
            // 
            // cboTrunc
            // 
            this.cboTrunc.FormattingEnabled = true;
            this.cboTrunc.Location = new System.Drawing.Point(500, 86);
            this.cboTrunc.Name = "cboTrunc";
            this.cboTrunc.Size = new System.Drawing.Size(125, 23);
            this.cboTrunc.TabIndex = 6;
            // 
            // cboNgt
            // 
            this.cboNgt.FormattingEnabled = true;
            this.cboNgt.Location = new System.Drawing.Point(351, 86);
            this.cboNgt.Name = "cboNgt";
            this.cboNgt.Size = new System.Drawing.Size(143, 23);
            this.cboNgt.TabIndex = 5;
            // 
            // cboGbn
            // 
            this.cboGbn.FormattingEnabled = true;
            this.cboGbn.Location = new System.Drawing.Point(123, 86);
            this.cboGbn.Name = "cboGbn";
            this.cboGbn.Size = new System.Drawing.Size(222, 23);
            this.cboGbn.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "일수별 금액 환산표";
            // 
            // SS3
            // 
            this.SS3.AccessibleDescription = "SS3, Sheet1, Row 0, Column 0, ";
            this.SS3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SS3.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS3.Location = new System.Drawing.Point(3, 117);
            this.SS3.Name = "SS3";
            this.SS3.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS3_Sheet1});
            this.SS3.Size = new System.Drawing.Size(822, 84);
            this.SS3.TabIndex = 2;
            this.SS3.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS3.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS3_CellDoubleClick);
            // 
            // SS3_Sheet1
            // 
            this.SS3_Sheet1.Reset();
            this.SS3_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS3_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS3_Sheet1.ColumnCount = 11;
            this.SS3_Sheet1.RowCount = 5;
            this.SS3_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS3_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS3_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SS3_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "적용일자";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "상대가치점수";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "고정비율";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "입원일수(평균)";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "입원일수(하한)";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "입원일수(상한)";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "점수당단가";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "야간,공휴점수";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "나이(하한)";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "나이(상한)";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "가산점수";
            this.SS3_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS3_Sheet1.Columns.Get(0).Label = "적용일자";
            this.SS3_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS3_Sheet1.Columns.Get(0).Width = 78F;
            this.SS3_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS3_Sheet1.Columns.Get(1).Label = "상대가치점수";
            this.SS3_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS3_Sheet1.Columns.Get(1).Width = 81F;
            this.SS3_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS3_Sheet1.Columns.Get(2).Label = "고정비율";
            this.SS3_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS3_Sheet1.Columns.Get(2).Visible = false;
            this.SS3_Sheet1.Columns.Get(2).Width = 57F;
            this.SS3_Sheet1.Columns.Get(3).Label = "입원일수(평균)";
            this.SS3_Sheet1.Columns.Get(3).Width = 89F;
            this.SS3_Sheet1.Columns.Get(4).Label = "입원일수(하한)";
            this.SS3_Sheet1.Columns.Get(4).Width = 89F;
            this.SS3_Sheet1.Columns.Get(5).Label = "입원일수(상한)";
            this.SS3_Sheet1.Columns.Get(5).Width = 89F;
            this.SS3_Sheet1.Columns.Get(6).Label = "점수당단가";
            this.SS3_Sheet1.Columns.Get(6).Width = 84F;
            this.SS3_Sheet1.Columns.Get(7).Label = "야간,공휴점수";
            this.SS3_Sheet1.Columns.Get(7).Width = 86F;
            this.SS3_Sheet1.Columns.Get(8).Label = "나이(하한)";
            this.SS3_Sheet1.Columns.Get(8).Width = 65F;
            this.SS3_Sheet1.Columns.Get(9).Label = "나이(상한)";
            this.SS3_Sheet1.Columns.Get(9).Width = 65F;
            this.SS3_Sheet1.Columns.Get(10).Label = "가산점수";
            this.SS3_Sheet1.Columns.Get(10).Width = 75F;
            this.SS3_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS3_Sheet1.DefaultStyle.Locked = true;
            this.SS3_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS3_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.SS3_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS3_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS3_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS3_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS3_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS3_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS3_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS3_Sheet1.RowHeader.Visible = false;
            this.SS3_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS3_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // txtDrgName
            // 
            this.txtDrgName.Location = new System.Drawing.Point(102, 22);
            this.txtDrgName.Multiline = true;
            this.txtDrgName.Name = "txtDrgName";
            this.txtDrgName.Size = new System.Drawing.Size(712, 58);
            this.txtDrgName.TabIndex = 1;
            // 
            // txtDrgCode
            // 
            this.txtDrgCode.Location = new System.Drawing.Point(6, 22);
            this.txtDrgCode.Name = "txtDrgCode";
            this.txtDrgCode.Size = new System.Drawing.Size(90, 23);
            this.txtDrgCode.TabIndex = 0;
            this.txtDrgCode.DoubleClick += new System.EventHandler(this.txtDrgCode_DoubleClick);
            // 
            // panCodeList
            // 
            this.panCodeList.Controls.Add(this.SS1);
            this.panCodeList.Dock = System.Windows.Forms.DockStyle.Left;
            this.panCodeList.Location = new System.Drawing.Point(0, 56);
            this.panCodeList.Name = "panCodeList";
            this.panCodeList.Padding = new System.Windows.Forms.Padding(3);
            this.panCodeList.Size = new System.Drawing.Size(125, 845);
            this.panCodeList.TabIndex = 4;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS1.Location = new System.Drawing.Point(3, 3);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(119, 839);
            this.SS1.TabIndex = 6;
            this.SS1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS1_CellDoubleClick);
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 1;
            this.SS1_Sheet1.RowCount = 1;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "DRG 코드";
            this.SS1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Label = "DRG 코드";
            this.SS1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Width = 98F;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.RowHeader.Visible = false;
            this.SS1_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.종료XToolStripMenuItem,
            this.저장SToolStripMenuItem,
            this.새로고침RToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 32);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(959, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 종료XToolStripMenuItem
            // 
            this.종료XToolStripMenuItem.Name = "종료XToolStripMenuItem";
            this.종료XToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.종료XToolStripMenuItem.Text = "종료(&X)";
            this.종료XToolStripMenuItem.Click += new System.EventHandler(this.종료XToolStripMenuItem_Click);
            // 
            // 저장SToolStripMenuItem
            // 
            this.저장SToolStripMenuItem.Name = "저장SToolStripMenuItem";
            this.저장SToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.저장SToolStripMenuItem.Text = "저장(&S)";
            this.저장SToolStripMenuItem.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // 새로고침RToolStripMenuItem
            // 
            this.새로고침RToolStripMenuItem.Name = "새로고침RToolStripMenuItem";
            this.새로고침RToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.새로고침RToolStripMenuItem.Text = "새로고침(&R)";
            this.새로고침RToolStripMenuItem.Click += new System.EventHandler(this.새로고침RToolStripMenuItem_Click);
            // 
            // frmPmpaDrgCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 901);
            this.Controls.Add(this.expandablePanel5);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "frmPmpaDrgCode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmPmpaDrgCode_Load);
            this.expandablePanel5.ResumeLayout(false);
            this.expandablePanel5.PerformLayout();
            this.panSuga.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS2_Sheet1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS6_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS5_Sheet1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS3_Sheet1)).EndInit();
            this.panCodeList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ExpandablePanel expandablePanel5;
        private System.Windows.Forms.Panel panSuga;
        private FarPoint.Win.Spread.FpSpread SS2;
        private FarPoint.Win.Spread.SheetView SS2_Sheet1;
        private System.Windows.Forms.GroupBox groupBox2;
        private FarPoint.Win.Spread.FpSpread SS6;
        private FarPoint.Win.Spread.SheetView SS6_Sheet1;
        private System.Windows.Forms.Panel panSplit;
        private FarPoint.Win.Spread.FpSpread SS5;
        private FarPoint.Win.Spread.SheetView SS5_Sheet1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboOgAdd;
        private System.Windows.Forms.ComboBox cboTrunc;
        private System.Windows.Forms.ComboBox cboNgt;
        private System.Windows.Forms.ComboBox cboGbn;
        private System.Windows.Forms.Label label1;
        private FarPoint.Win.Spread.FpSpread SS3;
        private FarPoint.Win.Spread.SheetView SS3_Sheet1;
        private System.Windows.Forms.TextBox txtDrgName;
        private System.Windows.Forms.TextBox txtDrgCode;
        private System.Windows.Forms.Panel panCodeList;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 종료XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 저장SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 새로고침RToolStripMenuItem;
        private System.Windows.Forms.Button btnSave;
    }
}