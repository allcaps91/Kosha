using System.Windows.Forms;

namespace ComPmpaLibB
{
    partial class frmPmpaMisuMast1
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys key = keyData & ~(Keys.Shift | Keys.Control);

            switch (key)
            {
                case Keys.F2: //신규등록
                    eMenuClick(Menu1_1, null);
                    return true;
                case Keys.F3: //ID사항변경
                    eMenuClick(Menu1_2, null);
                    return true;
                case Keys.F4: //월별, 조합별 명단찾기
                    eMenuClick(Menu1_3, null);
                    return true;
                case Keys.F5: //미수번호별 명단찾기
                    eMenuClick(Menu1_4, null);
                    return true;

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer enhancedColumnHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer();
            FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer enhancedColumnHeaderRenderer2 = new FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer();
            FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer2 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer enhancedColumnHeaderRenderer3 = new FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer();
            FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer3 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer enhancedColumnHeaderRenderer4 = new FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer();
            FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer4 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            FarPoint.Win.Spread.CellType.FlatFilterBarHeaderRenderer flatFilterBarHeaderRenderer1 = new FarPoint.Win.Spread.CellType.FlatFilterBarHeaderRenderer();
            FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer enhancedColumnHeaderRenderer6 = new FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer();
            FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer6 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            FarPoint.Win.Spread.CellType.FlatFilterBarHeaderRenderer flatFilterBarHeaderRenderer3 = new FarPoint.Win.Spread.CellType.FlatFilterBarHeaderRenderer();
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer2 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer3 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.NamedStyle namedStyle9 = new FarPoint.Win.Spread.NamedStyle("FilterBarDefaultEnhanced");
            FarPoint.Win.Spread.CellType.FilterBarCellType filterBarCellType3 = new FarPoint.Win.Spread.CellType.FilterBarCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle10 = new FarPoint.Win.Spread.NamedStyle("ColumnHeaderDefaultEnhanced");
            FarPoint.Win.Spread.NamedStyle namedStyle11 = new FarPoint.Win.Spread.NamedStyle("RowHeaderDefaultEnhanced");
            FarPoint.Win.Spread.NamedStyle namedStyle12 = new FarPoint.Win.Spread.NamedStyle("CornerDefaultEnhanced");
            FarPoint.Win.Spread.CellType.FlatCornerHeaderRenderer flatCornerHeaderRenderer3 = new FarPoint.Win.Spread.CellType.FlatCornerHeaderRenderer();
            FarPoint.Win.Spread.NamedStyle namedStyle13 = new FarPoint.Win.Spread.NamedStyle("DataAreaDefault");
            FarPoint.Win.Spread.CellType.GeneralCellType generalCellType2 = new FarPoint.Win.Spread.CellType.GeneralCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle14 = new FarPoint.Win.Spread.NamedStyle("FilterBarGrayscale");
            FarPoint.Win.Spread.CellType.FilterBarCellType filterBarCellType4 = new FarPoint.Win.Spread.CellType.FilterBarCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle15 = new FarPoint.Win.Spread.NamedStyle("FilterBarHeaderFlatOffice2016DarkGray");
            FarPoint.Win.Spread.NamedStyle namedStyle16 = new FarPoint.Win.Spread.NamedStyle("CornerHeaderFlatOffice2016DarkGray");
            FarPoint.Win.Spread.CellType.FlatCornerHeaderRenderer flatCornerHeaderRenderer4 = new FarPoint.Win.Spread.CellType.FlatCornerHeaderRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer4 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.InputMap ssList2_InputMapWhenFocusedNormal;
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.jobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu1_1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu1_2 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu1_3 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu1_4 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu2_1 = new System.Windows.Forms.ToolStripMenuItem();
            this.미수내역조회ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.ssList1 = new FarPoint.Win.Spread.FpSpread();
            this.ssList1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.lblMukNo = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.ssList2 = new FarPoint.Win.Spread.FpSpread();
            this.ssList2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.ssList3 = new FarPoint.Win.Spread.FpSpread();
            this.ssList3_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.PanelMain = new System.Windows.Forms.Panel();
            this.TxtMirYYMM = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.TxtJepsuNo = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cboTongGbn = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cboIO = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpBDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.lblChasu = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cboBun = new System.Windows.Forms.ComboBox();
            this.btnHelp = new System.Windows.Forms.Button();
            this.lblMiaName = new System.Windows.Forms.Label();
            this.TxtGelCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblMirNo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblWRTNO = new System.Windows.Forms.Label();
            this.lblSname = new System.Windows.Forms.Label();
            this.TxtMisuID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cboClass = new System.Windows.Forms.ComboBox();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.pan = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.panTitle = new System.Windows.Forms.Panel();
            ssList2_InputMapWhenFocusedNormal = new FarPoint.Win.Spread.InputMap();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1_Sheet1)).BeginInit();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList2_Sheet1)).BeginInit();
            this.panel10.SuspendLayout();
            this.panel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList3_Sheet1)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.PanelMain.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            enhancedColumnHeaderRenderer1.ActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedColumnHeaderRenderer1.Name = "enhancedColumnHeaderRenderer1";
            enhancedColumnHeaderRenderer1.NormalGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedColumnHeaderRenderer1.PictureZoomEffect = false;
            enhancedColumnHeaderRenderer1.SelectedActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedColumnHeaderRenderer1.SelectedBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            enhancedColumnHeaderRenderer1.SelectedGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedColumnHeaderRenderer1.TextRotationAngle = 0D;
            enhancedColumnHeaderRenderer1.ZoomFactor = 1F;
            enhancedRowHeaderRenderer1.ActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedRowHeaderRenderer1.Name = "enhancedRowHeaderRenderer1";
            enhancedRowHeaderRenderer1.NormalGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedRowHeaderRenderer1.PictureZoomEffect = false;
            enhancedRowHeaderRenderer1.SelectedActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedRowHeaderRenderer1.SelectedBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            enhancedRowHeaderRenderer1.SelectedGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedRowHeaderRenderer1.TextRotationAngle = 0D;
            enhancedRowHeaderRenderer1.ZoomFactor = 1F;
            enhancedColumnHeaderRenderer2.ActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedColumnHeaderRenderer2.BackColor = System.Drawing.SystemColors.Control;
            enhancedColumnHeaderRenderer2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            enhancedColumnHeaderRenderer2.ForeColor = System.Drawing.SystemColors.ControlText;
            enhancedColumnHeaderRenderer2.Name = "enhancedColumnHeaderRenderer2";
            enhancedColumnHeaderRenderer2.NormalGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedColumnHeaderRenderer2.PictureZoomEffect = false;
            enhancedColumnHeaderRenderer2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            enhancedColumnHeaderRenderer2.SelectedActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedColumnHeaderRenderer2.SelectedBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            enhancedColumnHeaderRenderer2.SelectedGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedColumnHeaderRenderer2.TextRotationAngle = 0D;
            enhancedColumnHeaderRenderer2.ZoomFactor = 1F;
            enhancedRowHeaderRenderer2.ActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedRowHeaderRenderer2.BackColor = System.Drawing.SystemColors.Control;
            enhancedRowHeaderRenderer2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            enhancedRowHeaderRenderer2.ForeColor = System.Drawing.SystemColors.ControlText;
            enhancedRowHeaderRenderer2.Name = "enhancedRowHeaderRenderer2";
            enhancedRowHeaderRenderer2.NormalGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedRowHeaderRenderer2.PictureZoomEffect = false;
            enhancedRowHeaderRenderer2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            enhancedRowHeaderRenderer2.SelectedActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedRowHeaderRenderer2.SelectedBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            enhancedRowHeaderRenderer2.SelectedGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedRowHeaderRenderer2.TextRotationAngle = 0D;
            enhancedRowHeaderRenderer2.ZoomFactor = 1F;
            enhancedColumnHeaderRenderer3.ActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedColumnHeaderRenderer3.Name = "enhancedColumnHeaderRenderer3";
            enhancedColumnHeaderRenderer3.NormalGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedColumnHeaderRenderer3.PictureZoomEffect = false;
            enhancedColumnHeaderRenderer3.SelectedActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedColumnHeaderRenderer3.SelectedBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            enhancedColumnHeaderRenderer3.SelectedGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedColumnHeaderRenderer3.TextRotationAngle = 0D;
            enhancedColumnHeaderRenderer3.ZoomFactor = 1F;
            enhancedRowHeaderRenderer3.ActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedRowHeaderRenderer3.Name = "enhancedRowHeaderRenderer3";
            enhancedRowHeaderRenderer3.NormalGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedRowHeaderRenderer3.PictureZoomEffect = false;
            enhancedRowHeaderRenderer3.SelectedActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedRowHeaderRenderer3.SelectedBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            enhancedRowHeaderRenderer3.SelectedGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedRowHeaderRenderer3.TextRotationAngle = 0D;
            enhancedRowHeaderRenderer3.ZoomFactor = 1F;
            enhancedColumnHeaderRenderer4.ActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedColumnHeaderRenderer4.BackColor = System.Drawing.SystemColors.Control;
            enhancedColumnHeaderRenderer4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            enhancedColumnHeaderRenderer4.ForeColor = System.Drawing.SystemColors.ControlText;
            enhancedColumnHeaderRenderer4.Name = "enhancedColumnHeaderRenderer4";
            enhancedColumnHeaderRenderer4.NormalGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedColumnHeaderRenderer4.PictureZoomEffect = false;
            enhancedColumnHeaderRenderer4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            enhancedColumnHeaderRenderer4.SelectedActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedColumnHeaderRenderer4.SelectedBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            enhancedColumnHeaderRenderer4.SelectedGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedColumnHeaderRenderer4.TextRotationAngle = 0D;
            enhancedColumnHeaderRenderer4.ZoomFactor = 1F;
            enhancedRowHeaderRenderer4.ActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedRowHeaderRenderer4.BackColor = System.Drawing.SystemColors.Control;
            enhancedRowHeaderRenderer4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            enhancedRowHeaderRenderer4.ForeColor = System.Drawing.SystemColors.ControlText;
            enhancedRowHeaderRenderer4.Name = "enhancedRowHeaderRenderer4";
            enhancedRowHeaderRenderer4.NormalGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedRowHeaderRenderer4.PictureZoomEffect = false;
            enhancedRowHeaderRenderer4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            enhancedRowHeaderRenderer4.SelectedActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedRowHeaderRenderer4.SelectedBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            enhancedRowHeaderRenderer4.SelectedGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedRowHeaderRenderer4.TextRotationAngle = 0D;
            enhancedRowHeaderRenderer4.ZoomFactor = 1F;
            flatFilterBarHeaderRenderer1.BackColor = System.Drawing.SystemColors.Control;
            flatFilterBarHeaderRenderer1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            flatFilterBarHeaderRenderer1.ForeColor = System.Drawing.SystemColors.ControlText;
            flatFilterBarHeaderRenderer1.Name = "flatFilterBarHeaderRenderer1";
            flatFilterBarHeaderRenderer1.PictureZoomEffect = false;
            flatFilterBarHeaderRenderer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            flatFilterBarHeaderRenderer1.TextRotationAngle = 0D;
            flatFilterBarHeaderRenderer1.ZoomFactor = 1F;
            enhancedColumnHeaderRenderer6.ActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedColumnHeaderRenderer6.BackColor = System.Drawing.SystemColors.Control;
            enhancedColumnHeaderRenderer6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            enhancedColumnHeaderRenderer6.ForeColor = System.Drawing.SystemColors.ControlText;
            enhancedColumnHeaderRenderer6.Name = "enhancedColumnHeaderRenderer6";
            enhancedColumnHeaderRenderer6.NormalGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedColumnHeaderRenderer6.PictureZoomEffect = false;
            enhancedColumnHeaderRenderer6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            enhancedColumnHeaderRenderer6.SelectedActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedColumnHeaderRenderer6.SelectedBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            enhancedColumnHeaderRenderer6.SelectedGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedColumnHeaderRenderer6.TextRotationAngle = 0D;
            enhancedColumnHeaderRenderer6.ZoomFactor = 1F;
            enhancedRowHeaderRenderer6.ActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedRowHeaderRenderer6.BackColor = System.Drawing.SystemColors.Control;
            enhancedRowHeaderRenderer6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            enhancedRowHeaderRenderer6.ForeColor = System.Drawing.SystemColors.ControlText;
            enhancedRowHeaderRenderer6.Name = "enhancedRowHeaderRenderer6";
            enhancedRowHeaderRenderer6.NormalGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedRowHeaderRenderer6.PictureZoomEffect = false;
            enhancedRowHeaderRenderer6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            enhancedRowHeaderRenderer6.SelectedActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedRowHeaderRenderer6.SelectedBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            enhancedRowHeaderRenderer6.SelectedGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedRowHeaderRenderer6.TextRotationAngle = 0D;
            enhancedRowHeaderRenderer6.ZoomFactor = 1F;
            flatFilterBarHeaderRenderer3.BackColor = System.Drawing.SystemColors.Control;
            flatFilterBarHeaderRenderer3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            flatFilterBarHeaderRenderer3.ForeColor = System.Drawing.SystemColors.ControlText;
            flatFilterBarHeaderRenderer3.Name = "flatFilterBarHeaderRenderer3";
            flatFilterBarHeaderRenderer3.PictureZoomEffect = false;
            flatFilterBarHeaderRenderer3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            flatFilterBarHeaderRenderer3.TextRotationAngle = 0D;
            flatFilterBarHeaderRenderer3.ZoomFactor = 1F;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1279, 22);
            this.panel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jobToolStripMenuItem,
            this.Menu2_1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1279, 22);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // jobToolStripMenuItem
            // 
            this.jobToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu1_1,
            this.Menu1_2,
            this.Menu1_3,
            this.Menu1_4});
            this.jobToolStripMenuItem.Name = "jobToolStripMenuItem";
            this.jobToolStripMenuItem.Size = new System.Drawing.Size(37, 18);
            this.jobToolStripMenuItem.Text = "Job";
            // 
            // Menu1_1
            // 
            this.Menu1_1.Name = "Menu1_1";
            this.Menu1_1.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.Menu1_1.Size = new System.Drawing.Size(227, 22);
            this.Menu1_1.Text = "1. 신규등록";
            // 
            // Menu1_2
            // 
            this.Menu1_2.Name = "Menu1_2";
            this.Menu1_2.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.Menu1_2.Size = new System.Drawing.Size(227, 22);
            this.Menu1_2.Text = "2. ID사항변경";
            // 
            // Menu1_3
            // 
            this.Menu1_3.Name = "Menu1_3";
            this.Menu1_3.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.Menu1_3.Size = new System.Drawing.Size(227, 22);
            this.Menu1_3.Text = "3. 월별, 조합별 명단찾기";
            // 
            // Menu1_4
            // 
            this.Menu1_4.Name = "Menu1_4";
            this.Menu1_4.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.Menu1_4.Size = new System.Drawing.Size(227, 22);
            this.Menu1_4.Text = "4. 미수번호별 명단찾기";
            // 
            // Menu2_1
            // 
            this.Menu2_1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.미수내역조회ToolStripMenuItem});
            this.Menu2_1.Name = "Menu2_1";
            this.Menu2_1.Size = new System.Drawing.Size(45, 18);
            this.Menu2_1.Text = "View";
            // 
            // 미수내역조회ToolStripMenuItem
            // 
            this.미수내역조회ToolStripMenuItem.Name = "미수내역조회ToolStripMenuItem";
            this.미수내역조회ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.미수내역조회ToolStripMenuItem.Text = "1. 미수 내역 조회";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 21);
            this.label1.TabIndex = 81;
            this.label1.Text = "미수 원장 관리(보험)";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.ssList1);
            this.panel7.Location = new System.Drawing.Point(55, 141);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1087, 80);
            this.panel7.TabIndex = 115;
            // 
            // ssList1
            // 
            this.ssList1.AccessibleDescription = "ssList1, Sheet1, Row 0, Column 0, ";
            this.ssList1.AutoScrollWhenKeyboardShowing = false;
            this.ssList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList1.FocusRenderer = flatFocusIndicatorRenderer2;
            this.ssList1.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssList1.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer3.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            flatScrollBarRenderer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            flatScrollBarRenderer3.TrackBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.ssList1.HorizontalScrollBar.Renderer = flatScrollBarRenderer3;
            this.ssList1.HorizontalScrollBar.TabIndex = 60;
            this.ssList1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssList1.Location = new System.Drawing.Point(0, 0);
            this.ssList1.Name = "ssList1";
            namedStyle9.BackColor = System.Drawing.Color.White;
            filterBarCellType3.FormatString = "";
            namedStyle9.CellType = filterBarCellType3;
            namedStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle9.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle9.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle9.Renderer = filterBarCellType3;
            namedStyle9.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle9.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle10.BackColor = System.Drawing.Color.White;
            namedStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle10.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle10.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle10.Renderer = enhancedColumnHeaderRenderer6;
            namedStyle10.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle10.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle11.BackColor = System.Drawing.Color.White;
            namedStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle11.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle11.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle11.Renderer = enhancedRowHeaderRenderer6;
            namedStyle11.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle11.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle12.BackColor = System.Drawing.Color.White;
            namedStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle12.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle12.NoteIndicatorColor = System.Drawing.Color.Red;
            flatCornerHeaderRenderer3.ActiveForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(213)))), ((int)(((byte)(213)))));
            flatCornerHeaderRenderer3.ActiveMouseOverForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(213)))), ((int)(((byte)(213)))));
            flatCornerHeaderRenderer3.GridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            flatCornerHeaderRenderer3.NormalTriangleColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(213)))), ((int)(((byte)(213)))));
            namedStyle12.Renderer = flatCornerHeaderRenderer3;
            namedStyle12.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle12.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle13.BackColor = System.Drawing.SystemColors.Window;
            namedStyle13.CellType = generalCellType2;
            namedStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            namedStyle13.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle13.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle13.Renderer = generalCellType2;
            namedStyle13.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle13.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle14.BackColor = System.Drawing.Color.DimGray;
            filterBarCellType4.FormatString = "";
            namedStyle14.CellType = filterBarCellType4;
            namedStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle14.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle14.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle14.Renderer = filterBarCellType4;
            namedStyle14.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle14.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(106)))), ((int)(((byte)(106)))));
            namedStyle15.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle15.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle15.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle15.Renderer = flatFilterBarHeaderRenderer3;
            namedStyle15.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle15.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(106)))), ((int)(((byte)(106)))));
            namedStyle16.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle16.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle16.NoteIndicatorColor = System.Drawing.Color.Red;
            flatCornerHeaderRenderer4.ActiveForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(240)))), ((int)(((byte)(224)))));
            flatCornerHeaderRenderer4.ActiveMouseOverForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            flatCornerHeaderRenderer4.NormalTriangleColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            namedStyle16.Renderer = flatCornerHeaderRenderer4;
            namedStyle16.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle16.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle9,
            namedStyle10,
            namedStyle11,
            namedStyle12,
            namedStyle13,
            namedStyle14,
            namedStyle15,
            namedStyle16});
            this.ssList1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList1_Sheet1});
            this.ssList1.Size = new System.Drawing.Size(1087, 80);
            this.ssList1.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2016Colorful;
            this.ssList1.TabIndex = 20;
            this.ssList1.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssList1.VerticalScrollBar.Name = "";
            flatScrollBarRenderer4.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            flatScrollBarRenderer4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            flatScrollBarRenderer4.TrackBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.ssList1.VerticalScrollBar.Renderer = flatScrollBarRenderer4;
            this.ssList1.VerticalScrollBar.TabIndex = 61;
            this.ssList1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            // 
            // ssList1_Sheet1
            // 
            this.ssList1_Sheet1.Reset();
            this.ssList1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList1_Sheet1.ColumnCount = 9;
            this.ssList1_Sheet1.RowCount = 2;
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlatOffice2016Colorful";
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlatOffice2016Colorful";
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "총진료비";
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "청구금액";
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "입 금 액";
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "삭 감 액";
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "절사삭감";
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "반송액";
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "과지급금";
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "계산착오";
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "현재잔액";
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlatOffice2016Colorful";
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.Columns.Get(0).CanFocus = false;
            this.ssList1_Sheet1.Columns.Get(0).CellType = textCellType10;
            this.ssList1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList1_Sheet1.Columns.Get(0).Label = "총진료비";
            this.ssList1_Sheet1.Columns.Get(0).Resizable = false;
            this.ssList1_Sheet1.Columns.Get(0).TabStop = false;
            this.ssList1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(0).Width = 115F;
            this.ssList1_Sheet1.Columns.Get(1).CanFocus = false;
            this.ssList1_Sheet1.Columns.Get(1).CellType = textCellType11;
            this.ssList1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList1_Sheet1.Columns.Get(1).Label = "청구금액";
            this.ssList1_Sheet1.Columns.Get(1).Resizable = false;
            this.ssList1_Sheet1.Columns.Get(1).TabStop = false;
            this.ssList1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(1).Width = 115F;
            this.ssList1_Sheet1.Columns.Get(2).CanFocus = false;
            this.ssList1_Sheet1.Columns.Get(2).CellType = textCellType12;
            this.ssList1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList1_Sheet1.Columns.Get(2).Label = "입 금 액";
            this.ssList1_Sheet1.Columns.Get(2).Resizable = false;
            this.ssList1_Sheet1.Columns.Get(2).TabStop = false;
            this.ssList1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(2).Width = 115F;
            this.ssList1_Sheet1.Columns.Get(3).CanFocus = false;
            this.ssList1_Sheet1.Columns.Get(3).CellType = textCellType13;
            this.ssList1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList1_Sheet1.Columns.Get(3).Label = "삭 감 액";
            this.ssList1_Sheet1.Columns.Get(3).Resizable = false;
            this.ssList1_Sheet1.Columns.Get(3).TabStop = false;
            this.ssList1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(3).Width = 115F;
            this.ssList1_Sheet1.Columns.Get(4).CanFocus = false;
            this.ssList1_Sheet1.Columns.Get(4).CellType = textCellType14;
            this.ssList1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList1_Sheet1.Columns.Get(4).Label = "절사삭감";
            this.ssList1_Sheet1.Columns.Get(4).Resizable = false;
            this.ssList1_Sheet1.Columns.Get(4).TabStop = false;
            this.ssList1_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(4).Width = 115F;
            this.ssList1_Sheet1.Columns.Get(5).CanFocus = false;
            this.ssList1_Sheet1.Columns.Get(5).CellType = textCellType15;
            this.ssList1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList1_Sheet1.Columns.Get(5).Label = "반송액";
            this.ssList1_Sheet1.Columns.Get(5).Resizable = false;
            this.ssList1_Sheet1.Columns.Get(5).TabStop = false;
            this.ssList1_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(5).Width = 115F;
            this.ssList1_Sheet1.Columns.Get(6).CanFocus = false;
            this.ssList1_Sheet1.Columns.Get(6).CellType = textCellType16;
            this.ssList1_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList1_Sheet1.Columns.Get(6).Label = "과지급금";
            this.ssList1_Sheet1.Columns.Get(6).Resizable = false;
            this.ssList1_Sheet1.Columns.Get(6).TabStop = false;
            this.ssList1_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(6).Width = 115F;
            this.ssList1_Sheet1.Columns.Get(7).CanFocus = false;
            this.ssList1_Sheet1.Columns.Get(7).CellType = textCellType17;
            this.ssList1_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList1_Sheet1.Columns.Get(7).Label = "계산착오";
            this.ssList1_Sheet1.Columns.Get(7).Resizable = false;
            this.ssList1_Sheet1.Columns.Get(7).TabStop = false;
            this.ssList1_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(7).Width = 115F;
            this.ssList1_Sheet1.Columns.Get(8).CanFocus = false;
            this.ssList1_Sheet1.Columns.Get(8).CellType = textCellType18;
            this.ssList1_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList1_Sheet1.Columns.Get(8).Label = "현재잔액";
            this.ssList1_Sheet1.Columns.Get(8).Locked = true;
            this.ssList1_Sheet1.Columns.Get(8).Resizable = false;
            this.ssList1_Sheet1.Columns.Get(8).TabStop = false;
            this.ssList1_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(8).Width = 115F;
            this.ssList1_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssList1_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlatOffice2016Colorful";
            this.ssList1_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlatOffice2016Colorful";
            this.ssList1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.RowHeader.Cells.Get(0, 0).Value = "건수";
            this.ssList1_Sheet1.RowHeader.Cells.Get(1, 0).Value = "금액";
            this.ssList1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList1_Sheet1.RowHeader.Columns.Get(0).Width = 41F;
            this.ssList1_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlatOffice2016Colorful";
            this.ssList1_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.Rows.Get(0).CanFocus = false;
            this.ssList1_Sheet1.Rows.Get(0).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(0).Label = "건수";
            this.ssList1_Sheet1.Rows.Get(0).LockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ssList1_Sheet1.Rows.Get(0).Locked = true;
            this.ssList1_Sheet1.Rows.Get(0).Resizable = false;
            this.ssList1_Sheet1.Rows.Get(0).TabStop = false;
            this.ssList1_Sheet1.Rows.Get(1).CanFocus = false;
            this.ssList1_Sheet1.Rows.Get(1).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(1).Label = "금액";
            this.ssList1_Sheet1.Rows.Get(1).LockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ssList1_Sheet1.Rows.Get(1).Locked = true;
            this.ssList1_Sheet1.Rows.Get(1).Resizable = false;
            this.ssList1_Sheet1.Rows.Get(1).TabStop = false;
            this.ssList1_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlatOffice2016Colorful";
            this.ssList1_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // lblMukNo
            // 
            this.lblMukNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMukNo.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMukNo.ForeColor = System.Drawing.Color.Black;
            this.lblMukNo.Location = new System.Drawing.Point(922, 70);
            this.lblMukNo.Name = "lblMukNo";
            this.lblMukNo.Size = new System.Drawing.Size(107, 25);
            this.lblMukNo.TabIndex = 114;
            this.lblMukNo.Text = "lblMukNo";
            this.lblMukNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(852, 73);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(69, 20);
            this.label14.TabIndex = 113;
            this.label14.Text = "묶음번호";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.panel9);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 241);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1279, 317);
            this.panel8.TabIndex = 1;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.ssList2);
            this.panel9.Location = new System.Drawing.Point(38, 5);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1204, 306);
            this.panel9.TabIndex = 0;
            // 
            // ssList2
            // 
            this.ssList2.AccessibleDescription = "";
            this.ssList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssList2.Location = new System.Drawing.Point(0, 0);
            this.ssList2.Name = "ssList2";
            this.ssList2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList2_Sheet1});
            this.ssList2.Size = new System.Drawing.Size(1204, 306);
            this.ssList2.TabIndex = 1;
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Back, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.StartEditing);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke('='), FarPoint.Win.Spread.SpreadActions.StartEditingFormula);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.C, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCopy);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.V, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardPasteAll);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.X, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCut);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Insert, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCopy);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Delete, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCut);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Insert, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardPasteAll);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.F4, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ShowSubEditor);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Space, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.SelectRow);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Z, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.Undo);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Y, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.Redo);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToPreviousCellThenControl);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextCellThenControl);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Up, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToPreviousRow);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Down, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextRow);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.None);
            this.ssList2.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused, FarPoint.Win.Spread.OperationMode.Normal, ssList2_InputMapWhenFocusedNormal);
            // 
            // ssList2_Sheet1
            // 
            this.ssList2_Sheet1.Reset();
            this.ssList2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList2_Sheet1.ColumnCount = 1;
            this.ssList2_Sheet1.RowCount = 1;
            this.ssList2_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssList2_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssList2_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssList2_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssList2_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList2_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList2_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList2_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssList2_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.panel11);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(0, 558);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(1279, 173);
            this.panel10.TabIndex = 2;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.ssList3);
            this.panel11.Location = new System.Drawing.Point(38, 5);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(1204, 164);
            this.panel11.TabIndex = 0;
            // 
            // ssList3
            // 
            this.ssList3.AccessibleDescription = "";
            this.ssList3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList3.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssList3.Location = new System.Drawing.Point(0, 0);
            this.ssList3.Name = "ssList3";
            this.ssList3.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList3_Sheet1});
            this.ssList3.Size = new System.Drawing.Size(1204, 164);
            this.ssList3.TabIndex = 0;
            this.ssList3.SetActiveViewport(0, -1, -1);
            // 
            // ssList3_Sheet1
            // 
            this.ssList3_Sheet1.Reset();
            this.ssList3_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList3_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList3_Sheet1.ColumnCount = 0;
            this.ssList3_Sheet1.RowCount = 0;
            this.ssList3_Sheet1.ActiveColumnIndex = -1;
            this.ssList3_Sheet1.ActiveRowIndex = -1;
            this.ssList3_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssList3_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssList3_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssList3_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssList3_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssList3_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList3_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList3_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList3_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssList3_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel10);
            this.panel4.Controls.Add(this.panel8);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 129);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1279, 731);
            this.panel4.TabIndex = 166;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.PanelMain);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1279, 241);
            this.panel5.TabIndex = 0;
            // 
            // PanelMain
            // 
            this.PanelMain.BackColor = System.Drawing.SystemColors.Menu;
            this.PanelMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PanelMain.Controls.Add(this.panel7);
            this.PanelMain.Controls.Add(this.lblMukNo);
            this.PanelMain.Controls.Add(this.label14);
            this.PanelMain.Controls.Add(this.TxtMirYYMM);
            this.PanelMain.Controls.Add(this.label12);
            this.PanelMain.Controls.Add(this.TxtJepsuNo);
            this.PanelMain.Controls.Add(this.label11);
            this.PanelMain.Controls.Add(this.cboTongGbn);
            this.PanelMain.Controls.Add(this.label10);
            this.PanelMain.Controls.Add(this.cboIO);
            this.PanelMain.Controls.Add(this.label4);
            this.PanelMain.Controls.Add(this.dtpBDate);
            this.PanelMain.Controls.Add(this.label3);
            this.PanelMain.Controls.Add(this.lblChasu);
            this.PanelMain.Controls.Add(this.label9);
            this.PanelMain.Controls.Add(this.cboBun);
            this.PanelMain.Controls.Add(this.btnHelp);
            this.PanelMain.Controls.Add(this.lblMiaName);
            this.PanelMain.Controls.Add(this.TxtGelCode);
            this.PanelMain.Controls.Add(this.label5);
            this.PanelMain.Controls.Add(this.label8);
            this.PanelMain.Controls.Add(this.lblMirNo);
            this.PanelMain.Controls.Add(this.label2);
            this.PanelMain.Controls.Add(this.lblWRTNO);
            this.PanelMain.Controls.Add(this.lblSname);
            this.PanelMain.Controls.Add(this.TxtMisuID);
            this.PanelMain.Controls.Add(this.label6);
            this.PanelMain.Controls.Add(this.label7);
            this.PanelMain.Location = new System.Drawing.Point(38, 5);
            this.PanelMain.Name = "PanelMain";
            this.PanelMain.Size = new System.Drawing.Size(1204, 230);
            this.PanelMain.TabIndex = 11;
            // 
            // TxtMirYYMM
            // 
            this.TxtMirYYMM.BackColor = System.Drawing.Color.White;
            this.TxtMirYYMM.Location = new System.Drawing.Point(638, 96);
            this.TxtMirYYMM.Name = "TxtMirYYMM";
            this.TxtMirYYMM.Size = new System.Drawing.Size(139, 25);
            this.TxtMirYYMM.TabIndex = 8;
            this.TxtMirYYMM.Text = "TxtMirYYMM";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(568, 98);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(69, 20);
            this.label12.TabIndex = 28;
            this.label12.Text = "통계년월";
            // 
            // TxtJepsuNo
            // 
            this.TxtJepsuNo.BackColor = System.Drawing.Color.White;
            this.TxtJepsuNo.Location = new System.Drawing.Point(638, 68);
            this.TxtJepsuNo.Name = "TxtJepsuNo";
            this.TxtJepsuNo.Size = new System.Drawing.Size(139, 25);
            this.TxtJepsuNo.TabIndex = 7;
            this.TxtJepsuNo.Text = "TxtJepsuNo";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(568, 70);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 20);
            this.label11.TabIndex = 21;
            this.label11.Text = "접수번호";
            // 
            // cboTongGbn
            // 
            this.cboTongGbn.BackColor = System.Drawing.Color.White;
            this.cboTongGbn.FormattingEnabled = true;
            this.cboTongGbn.Location = new System.Drawing.Point(383, 96);
            this.cboTongGbn.Name = "cboTongGbn";
            this.cboTongGbn.Size = new System.Drawing.Size(118, 25);
            this.cboTongGbn.TabIndex = 5;
            this.cboTongGbn.Tag = "";
            this.cboTongGbn.Text = "cboTong";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(308, 98);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 20);
            this.label10.TabIndex = 30;
            this.label10.Text = "통계종류";
            // 
            // cboIO
            // 
            this.cboIO.BackColor = System.Drawing.Color.White;
            this.cboIO.FormattingEnabled = true;
            this.cboIO.Location = new System.Drawing.Point(383, 67);
            this.cboIO.Name = "cboIO";
            this.cboIO.Size = new System.Drawing.Size(118, 25);
            this.cboIO.TabIndex = 4;
            this.cboIO.Text = "cboIO";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(308, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 20);
            this.label4.TabIndex = 17;
            this.label4.Text = "외래입원";
            // 
            // dtpBDate
            // 
            this.dtpBDate.CustomFormat = "yyyy-MM-dd";
            this.dtpBDate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpBDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBDate.Location = new System.Drawing.Point(169, 67);
            this.dtpBDate.Name = "dtpBDate";
            this.dtpBDate.Size = new System.Drawing.Size(119, 25);
            this.dtpBDate.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(97, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 20);
            this.label3.TabIndex = 18;
            this.label3.Text = "청구일자";
            // 
            // lblChasu
            // 
            this.lblChasu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblChasu.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblChasu.ForeColor = System.Drawing.Color.Black;
            this.lblChasu.Location = new System.Drawing.Point(922, 41);
            this.lblChasu.Name = "lblChasu";
            this.lblChasu.Size = new System.Drawing.Size(107, 25);
            this.lblChasu.TabIndex = 102;
            this.lblChasu.Text = "lblChasu";
            this.lblChasu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(852, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 20);
            this.label9.TabIndex = 101;
            this.label9.Text = "심사차수";
            // 
            // cboBun
            // 
            this.cboBun.BackColor = System.Drawing.Color.White;
            this.cboBun.FormattingEnabled = true;
            this.cboBun.Location = new System.Drawing.Point(638, 40);
            this.cboBun.Name = "cboBun";
            this.cboBun.Size = new System.Drawing.Size(139, 25);
            this.cboBun.TabIndex = 6;
            this.cboBun.Text = "cboBun";
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = System.Drawing.Color.Transparent;
            this.btnHelp.Location = new System.Drawing.Point(275, 38);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(47, 27);
            this.btnHelp.TabIndex = 14;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = false;
            // 
            // lblMiaName
            // 
            this.lblMiaName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMiaName.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMiaName.ForeColor = System.Drawing.Color.Black;
            this.lblMiaName.Location = new System.Drawing.Point(325, 39);
            this.lblMiaName.Name = "lblMiaName";
            this.lblMiaName.Size = new System.Drawing.Size(176, 25);
            this.lblMiaName.TabIndex = 15;
            this.lblMiaName.Text = "lblMiaName";
            this.lblMiaName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtGelCode
            // 
            this.TxtGelCode.BackColor = System.Drawing.Color.White;
            this.TxtGelCode.Location = new System.Drawing.Point(169, 38);
            this.TxtGelCode.Name = "TxtGelCode";
            this.TxtGelCode.Size = new System.Drawing.Size(100, 25);
            this.TxtGelCode.TabIndex = 2;
            this.TxtGelCode.Text = "TxtGelCode";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(567, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "청구분야";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(97, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 20);
            this.label8.TabIndex = 12;
            this.label8.Text = "계 약 처";
            // 
            // lblMirNo
            // 
            this.lblMirNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMirNo.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMirNo.ForeColor = System.Drawing.Color.Black;
            this.lblMirNo.Location = new System.Drawing.Point(922, 10);
            this.lblMirNo.Name = "lblMirNo";
            this.lblMirNo.Size = new System.Drawing.Size(107, 25);
            this.lblMirNo.TabIndex = 94;
            this.lblMirNo.Text = "lblMirNo";
            this.lblMirNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(852, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 20);
            this.label2.TabIndex = 93;
            this.label2.Text = "청구번호";
            // 
            // lblWRTNO
            // 
            this.lblWRTNO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWRTNO.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWRTNO.ForeColor = System.Drawing.Color.Black;
            this.lblWRTNO.Location = new System.Drawing.Point(638, 10);
            this.lblWRTNO.Name = "lblWRTNO";
            this.lblWRTNO.Size = new System.Drawing.Size(139, 25);
            this.lblWRTNO.TabIndex = 8;
            this.lblWRTNO.Text = "lblWRNTO";
            this.lblWRTNO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSname
            // 
            this.lblSname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSname.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSname.ForeColor = System.Drawing.Color.Black;
            this.lblSname.Location = new System.Drawing.Point(275, 10);
            this.lblSname.Name = "lblSname";
            this.lblSname.Size = new System.Drawing.Size(226, 25);
            this.lblSname.TabIndex = 16;
            this.lblSname.Text = "lblSname";
            this.lblSname.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtMisuID
            // 
            this.TxtMisuID.BackColor = System.Drawing.Color.White;
            this.TxtMisuID.Location = new System.Drawing.Point(169, 10);
            this.TxtMisuID.Name = "TxtMisuID";
            this.TxtMisuID.Size = new System.Drawing.Size(100, 25);
            this.TxtMisuID.TabIndex = 1;
            this.TxtMisuID.Text = "TxtMisuID";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(569, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 20);
            this.label6.TabIndex = 9;
            this.label6.Text = "WRNTO";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(95, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 20);
            this.label7.TabIndex = 13;
            this.label7.Text = "미수종류";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cboClass);
            this.panel3.Controls.Add(this.lblItem0);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(272, 54);
            this.panel3.TabIndex = 30;
            // 
            // cboClass
            // 
            this.cboClass.FormattingEnabled = true;
            this.cboClass.Location = new System.Drawing.Point(120, 16);
            this.cboClass.Name = "cboClass";
            this.cboClass.Size = new System.Drawing.Size(109, 25);
            this.cboClass.TabIndex = 0;
            this.cboClass.Text = "cboJong";
            // 
            // lblItem0
            // 
            this.lblItem0.AutoSize = true;
            this.lblItem0.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblItem0.Location = new System.Drawing.Point(44, 17);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(69, 20);
            this.lblItem0.TabIndex = 25;
            this.lblItem0.Text = "미수종류";
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.White;
            this.panel12.Controls.Add(this.panel2);
            this.panel12.Controls.Add(this.panel6);
            this.panel12.Controls.Add(this.panel3);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel12.Location = new System.Drawing.Point(0, 75);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(1279, 54);
            this.panel12.TabIndex = 165;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnDel);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(936, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(306, 54);
            this.panel2.TabIndex = 32;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOK.Location = new System.Drawing.Point(8, 11);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(92, 33);
            this.btnOK.TabIndex = 31;
            this.btnOK.Text = "확인(&O)";
            this.btnOK.UseVisualStyleBackColor = false;
            // 
            // btnDel
            // 
            this.btnDel.BackColor = System.Drawing.Color.Transparent;
            this.btnDel.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDel.Location = new System.Drawing.Point(206, 11);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(92, 33);
            this.btnDel.TabIndex = 29;
            this.btnDel.Text = "삭제(&D)";
            this.btnDel.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.Location = new System.Drawing.Point(107, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 33);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "취소(&C)";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(272, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(664, 54);
            this.panel6.TabIndex = 31;
            // 
            // pan
            // 
            this.pan.BackColor = System.Drawing.Color.RoyalBlue;
            this.pan.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan.Location = new System.Drawing.Point(0, 59);
            this.pan.Name = "pan";
            this.pan.Size = new System.Drawing.Size(1279, 16);
            this.pan.TabIndex = 164;
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(1203, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 33);
            this.btnExit.TabIndex = 82;
            this.btnExit.Text = "닫기(&X)";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.label1);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 22);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1279, 37);
            this.panTitle.TabIndex = 163;
            // 
            // frmPmpaMisuMast1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1279, 860);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel12);
            this.Controls.Add(this.pan);
            this.Controls.Add(this.panTitle);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmPmpaMisuMast1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "미수 원장 관리(보험)";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1_Sheet1)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList2_Sheet1)).EndInit();
            this.panel10.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList3_Sheet1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.PanelMain.ResumeLayout(false);
            this.PanelMain.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem jobToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Menu2_1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel7;
        private FarPoint.Win.Spread.FpSpread ssList1;
        private FarPoint.Win.Spread.SheetView ssList1_Sheet1;
        private System.Windows.Forms.Label lblMukNo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel11;
        private FarPoint.Win.Spread.FpSpread ssList3;
        private FarPoint.Win.Spread.SheetView ssList3_Sheet1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel PanelMain;
        private System.Windows.Forms.TextBox TxtMirYYMM;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox TxtJepsuNo;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboTongGbn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboIO;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpBDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblChasu;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboBun;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Label lblMiaName;
        private System.Windows.Forms.TextBox TxtGelCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblMirNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblWRTNO;
        private System.Windows.Forms.Label lblSname;
        private System.Windows.Forms.TextBox TxtMisuID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cboClass;
        private System.Windows.Forms.Label lblItem0;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel pan;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.ToolStripMenuItem Menu1_1;
        private System.Windows.Forms.ToolStripMenuItem Menu1_2;
        private System.Windows.Forms.ToolStripMenuItem Menu1_3;
        private System.Windows.Forms.ToolStripMenuItem Menu1_4;
        private System.Windows.Forms.ToolStripMenuItem 미수내역조회ToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel6;
        private FarPoint.Win.Spread.FpSpread ssList2;
        private FarPoint.Win.Spread.SheetView ssList2_Sheet1;
    }
}