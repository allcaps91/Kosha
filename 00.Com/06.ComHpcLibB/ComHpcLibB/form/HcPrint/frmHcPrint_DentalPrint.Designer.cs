namespace ComHpcLibB
{
    partial class frmHcPrint_DentalPrint
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
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ko-KR", false);
            FarPoint.Win.Spread.CellType.ImageCellType imageCellType1 = new FarPoint.Win.Spread.CellType.ImageCellType();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHcPrint_DentalPrint));
            FarPoint.Win.Spread.CellType.ImageCellType imageCellType2 = new FarPoint.Win.Spread.CellType.ImageCellType();
            FarPoint.Win.Spread.CellType.ImageCellType imageCellType3 = new FarPoint.Win.Spread.CellType.ImageCellType();
            this.SSPrint_Dental = new FarPoint.Win.Spread.FpSpread();
            this.SSPrint_Dental_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.SSPrint_Dental)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSPrint_Dental_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // SSPrint_Dental
            // 
            this.SSPrint_Dental.AccessibleDescription = "SSPrint_Dental, Sheet1, Row 0, Column 0, ";
            this.SSPrint_Dental.Location = new System.Drawing.Point(12, 12);
            this.SSPrint_Dental.Name = "SSPrint_Dental";
            this.SSPrint_Dental.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSPrint_Dental_Sheet1});
            this.SSPrint_Dental.Size = new System.Drawing.Size(841, 386);
            this.SSPrint_Dental.TabIndex = 0;
            this.SSPrint_Dental.SetViewportTopRow(0, 0, 6);
            // 
            // SSPrint_Dental_Sheet1
            // 
            this.SSPrint_Dental_Sheet1.Reset();
            this.SSPrint_Dental_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSPrint_Dental_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSPrint_Dental_Sheet1.ColumnCount = 15;
            this.SSPrint_Dental_Sheet1.RowCount = 54;
            this.SSPrint_Dental_Sheet1.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SSPrint_Dental_Sheet1.Cells.Get(1, 2).ColumnSpan = 12;
            this.SSPrint_Dental_Sheet1.Cells.Get(1, 2).Value = "경북 포항시 남구 대잠동길 17";
            this.SSPrint_Dental_Sheet1.Cells.Get(3, 4).ColumnSpan = 3;
            this.SSPrint_Dental_Sheet1.Cells.Get(3, 4).Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.SSPrint_Dental_Sheet1.Cells.Get(3, 4).ForeColor = System.Drawing.Color.Black;
            this.SSPrint_Dental_Sheet1.Cells.Get(3, 4).Value = "포항성모병원";
            this.SSPrint_Dental_Sheet1.Cells.Get(4, 4).ColumnSpan = 3;
            this.SSPrint_Dental_Sheet1.Cells.Get(4, 4).Value = "☎ 054-260-8188";
            this.SSPrint_Dental_Sheet1.Cells.Get(17, 1).ColumnSpan = 10;
            this.SSPrint_Dental_Sheet1.Cells.Get(17, 1).Font = new System.Drawing.Font("맑은 고딕", 16F);
            this.SSPrint_Dental_Sheet1.Cells.Get(17, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(17, 1).Value = "                   구강검진 결과통보서";
            this.SSPrint_Dental_Sheet1.Cells.Get(17, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(17, 12).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(17, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(17, 12).Value = "국민건강보험공단";
            this.SSPrint_Dental_Sheet1.Cells.Get(17, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Bottom;
            this.SSPrint_Dental_Sheet1.Cells.Get(18, 1).ColumnSpan = 3;
            this.SSPrint_Dental_Sheet1.Cells.Get(18, 1).Value = "성명 : 홍길동";
            this.SSPrint_Dental_Sheet1.Cells.Get(18, 4).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(18, 4).Value = "주민등록번호";
            this.SSPrint_Dental_Sheet1.Cells.Get(18, 6).ColumnSpan = 4;
            this.SSPrint_Dental_Sheet1.Cells.Get(18, 6).Value = "920205-1******";
            this.SSPrint_Dental_Sheet1.Cells.Get(18, 10).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(18, 10).Value = "건강점진일자";
            this.SSPrint_Dental_Sheet1.Cells.Get(18, 12).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(18, 12).Value = "□내원 □출장";
            this.SSPrint_Dental_Sheet1.Cells.Get(21, 1).ColumnSpan = 13;
            this.SSPrint_Dental_Sheet1.Cells.Get(21, 1).Value = " 나의 구강검진 종합소견은?";
            this.SSPrint_Dental_Sheet1.Cells.Get(22, 1).ColumnSpan = 13;
            this.SSPrint_Dental_Sheet1.Cells.Get(22, 1).Value = " 판정 - ■정상A";
            this.SSPrint_Dental_Sheet1.Cells.Get(23, 1).ColumnSpan = 13;
            this.SSPrint_Dental_Sheet1.Cells.Get(23, 1).Value = "● 홍길동님은 다음 사항에 대해 바로 조치가 필요합니다.";
            this.SSPrint_Dental_Sheet1.Cells.Get(24, 1).ColumnSpan = 13;
            this.SSPrint_Dental_Sheet1.Cells.Get(25, 1).ColumnSpan = 13;
            this.SSPrint_Dental_Sheet1.Cells.Get(25, 1).Value = "● 홍길동님은 다음 사항에 대해 바로 조치가 필요합니다.";
            this.SSPrint_Dental_Sheet1.Cells.Get(26, 1).ColumnSpan = 13;
            this.SSPrint_Dental_Sheet1.Cells.Get(27, 1).ColumnSpan = 13;
            this.SSPrint_Dental_Sheet1.Cells.Get(27, 1).Value = " 나의 구강검사 결과는?";
            this.SSPrint_Dental_Sheet1.Cells.Get(28, 1).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(28, 1).RowSpan = 4;
            this.SSPrint_Dental_Sheet1.Cells.Get(28, 3).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(28, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SSPrint_Dental_Sheet1.Cells.Get(28, 3).Value = " (치과)병력 문제";
            this.SSPrint_Dental_Sheet1.Cells.Get(28, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(28, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(28, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(28, 5).ColumnSpan = 9;
            this.SSPrint_Dental_Sheet1.Cells.Get(28, 5).Value = " ■ 없음  □ 있음";
            this.SSPrint_Dental_Sheet1.Cells.Get(29, 3).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(29, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SSPrint_Dental_Sheet1.Cells.Get(29, 3).Value = " 구강건강인식도 문제";
            this.SSPrint_Dental_Sheet1.Cells.Get(29, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(29, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(29, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(29, 5).ColumnSpan = 9;
            this.SSPrint_Dental_Sheet1.Cells.Get(29, 5).Value = " ■ 없음  □ 있음";
            this.SSPrint_Dental_Sheet1.Cells.Get(30, 3).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(30, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SSPrint_Dental_Sheet1.Cells.Get(30, 3).RowSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(30, 3).Value = " 구강건강 습관문제";
            this.SSPrint_Dental_Sheet1.Cells.Get(30, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(30, 5).ColumnSpan = 5;
            this.SSPrint_Dental_Sheet1.Cells.Get(30, 5).Value = " 구강위생: ■ 없음  □ 있음";
            this.SSPrint_Dental_Sheet1.Cells.Get(30, 10).ColumnSpan = 4;
            this.SSPrint_Dental_Sheet1.Cells.Get(30, 10).Value = " 불소이용: ■ 없음  □ 있음";
            this.SSPrint_Dental_Sheet1.Cells.Get(31, 5).ColumnSpan = 5;
            this.SSPrint_Dental_Sheet1.Cells.Get(31, 5).Value = " 설탕섭취: ■ 없음  □ 있음";
            this.SSPrint_Dental_Sheet1.Cells.Get(31, 10).ColumnSpan = 4;
            this.SSPrint_Dental_Sheet1.Cells.Get(31, 10).Value = " 흡     연: ■ 없음  □ 있음";
            this.SSPrint_Dental_Sheet1.Cells.Get(33, 1).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(33, 1).RowSpan = 4;
            this.SSPrint_Dental_Sheet1.Cells.Get(33, 3).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(33, 3).Value = "우식치아";
            this.SSPrint_Dental_Sheet1.Cells.Get(33, 5).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(33, 5).Value = " ■ 없음  □ 있음";
            this.SSPrint_Dental_Sheet1.Cells.Get(33, 7).ColumnSpan = 3;
            this.SSPrint_Dental_Sheet1.Cells.Get(33, 7).RowSpan = 4;
            this.SSPrint_Dental_Sheet1.Cells.Get(34, 3).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(34, 3).Value = "인접면우식의심치아";
            this.SSPrint_Dental_Sheet1.Cells.Get(34, 5).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(34, 5).Value = " ■ 없음  □ 있음";
            this.SSPrint_Dental_Sheet1.Cells.Get(34, 10).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(34, 10).Value = " 치은염증";
            this.SSPrint_Dental_Sheet1.Cells.Get(34, 12).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(34, 12).Value = " ■ 없음  □ 있음";
            this.SSPrint_Dental_Sheet1.Cells.Get(35, 3).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(35, 3).Value = "수복치아";
            this.SSPrint_Dental_Sheet1.Cells.Get(35, 5).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(35, 5).Value = " ■ 없음  □ 있음";
            this.SSPrint_Dental_Sheet1.Cells.Get(35, 10).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(35, 10).Value = " 치     석";
            this.SSPrint_Dental_Sheet1.Cells.Get(35, 12).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(35, 12).Value = " ■ 없음  □ 있음";
            this.SSPrint_Dental_Sheet1.Cells.Get(36, 3).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(36, 3).Value = "상실치아";
            this.SSPrint_Dental_Sheet1.Cells.Get(36, 5).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(36, 5).Value = " ■ 없음  □ 있음";
            this.SSPrint_Dental_Sheet1.Cells.Get(37, 1).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(37, 3).ColumnSpan = 11;
            imageCellType1.Style = FarPoint.Win.RenderStyle.Stretch;
            imageCellType1.TransparencyColor = System.Drawing.Color.Empty;
            imageCellType1.TransparencyTolerance = 0;
            this.SSPrint_Dental_Sheet1.Cells.Get(38, 1).CellType = imageCellType1;
            this.SSPrint_Dental_Sheet1.Cells.Get(38, 1).ColumnSpan = 13;
            this.SSPrint_Dental_Sheet1.Cells.Get(38, 1).Value = ((object)(resources.GetObject("resource.Value")));
            imageCellType2.Style = FarPoint.Win.RenderStyle.Stretch;
            imageCellType2.TransparencyColor = System.Drawing.Color.Empty;
            imageCellType2.TransparencyTolerance = 0;
            this.SSPrint_Dental_Sheet1.Cells.Get(40, 1).CellType = imageCellType2;
            this.SSPrint_Dental_Sheet1.Cells.Get(40, 1).ColumnSpan = 3;
            this.SSPrint_Dental_Sheet1.Cells.Get(40, 1).RowSpan = 7;
            this.SSPrint_Dental_Sheet1.Cells.Get(40, 1).Value = ((object)(resources.GetObject("resource.Value1")));
            this.SSPrint_Dental_Sheet1.Cells.Get(40, 4).ColumnSpan = 4;
            this.SSPrint_Dental_Sheet1.Cells.Get(40, 4).Value = "상악우측 제1대구치(16번) 세균막";
            this.SSPrint_Dental_Sheet1.Cells.Get(40, 9).Value = "점";
            imageCellType3.Style = FarPoint.Win.RenderStyle.Stretch;
            imageCellType3.TransparencyColor = System.Drawing.Color.Empty;
            imageCellType3.TransparencyTolerance = 0;
            this.SSPrint_Dental_Sheet1.Cells.Get(40, 10).CellType = imageCellType3;
            this.SSPrint_Dental_Sheet1.Cells.Get(40, 10).ColumnSpan = 4;
            this.SSPrint_Dental_Sheet1.Cells.Get(40, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SSPrint_Dental_Sheet1.Cells.Get(40, 10).RowSpan = 7;
            this.SSPrint_Dental_Sheet1.Cells.Get(40, 10).Value = ((object)(resources.GetObject("resource.Value2")));
            this.SSPrint_Dental_Sheet1.Cells.Get(41, 4).ColumnSpan = 4;
            this.SSPrint_Dental_Sheet1.Cells.Get(41, 4).Value = "상악우측 중절치(11번) 세균만";
            this.SSPrint_Dental_Sheet1.Cells.Get(41, 9).Value = "점";
            this.SSPrint_Dental_Sheet1.Cells.Get(42, 4).ColumnSpan = 4;
            this.SSPrint_Dental_Sheet1.Cells.Get(42, 4).Value = "상악좌측 제1대구치(26번) 세균막";
            this.SSPrint_Dental_Sheet1.Cells.Get(42, 9).Value = "점";
            this.SSPrint_Dental_Sheet1.Cells.Get(43, 4).ColumnSpan = 4;
            this.SSPrint_Dental_Sheet1.Cells.Get(43, 4).Value = "하악좌측 제1대구치(36번) 세균막";
            this.SSPrint_Dental_Sheet1.Cells.Get(43, 9).Value = "점";
            this.SSPrint_Dental_Sheet1.Cells.Get(44, 4).ColumnSpan = 4;
            this.SSPrint_Dental_Sheet1.Cells.Get(44, 4).Value = "하악좌측 중절치(31번) 세균막";
            this.SSPrint_Dental_Sheet1.Cells.Get(44, 9).Value = "점";
            this.SSPrint_Dental_Sheet1.Cells.Get(45, 4).ColumnSpan = 4;
            this.SSPrint_Dental_Sheet1.Cells.Get(45, 4).Value = "하악우측 제1대구치(46번) 세균막";
            this.SSPrint_Dental_Sheet1.Cells.Get(45, 9).Value = "점";
            this.SSPrint_Dental_Sheet1.Cells.Get(46, 4).ColumnSpan = 4;
            this.SSPrint_Dental_Sheet1.Cells.Get(46, 4).Value = "평균";
            this.SSPrint_Dental_Sheet1.Cells.Get(46, 9).Value = "점";
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 1).ColumnSpan = 3;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 1).Value = "건 강 검 진 일";
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 4).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 4).ParseFormatInfo = ((System.Globalization.DateTimeFormatInfo)(cultureInfo.DateTimeFormat.Clone()));
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 4).ParseFormatString = "yyyy-MM-dd";
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 4).Value = new System.DateTime(2020, 5, 18, 0, 0, 0, 0);
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 6).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 6).Value = "통보일자";
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 8).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 8).ParseFormatInfo = ((System.Globalization.DateTimeFormatInfo)(cultureInfo.DateTimeFormat.Clone()));
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 8).ParseFormatString = "yyyy-MM-dd";
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 8).Value = new System.DateTime(2020, 5, 18, 0, 0, 0, 0);
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 10).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 10).Value = "면호번호";
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 12).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SSPrint_Dental_Sheet1.Cells.Get(48, 12).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SSPrint_Dental_Sheet1.Cells.Get(48, 12).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 12).ParseFormatString = "n";
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 12).Value = 123456;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 13).RowSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 13).Value = "전사서명";
            this.SSPrint_Dental_Sheet1.Cells.Get(48, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 1).ColumnSpan = 3;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 1).Value = "요양기관 기호";
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 4).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SSPrint_Dental_Sheet1.Cells.Get(49, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SSPrint_Dental_Sheet1.Cells.Get(49, 4).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 4).ParseFormatString = "n";
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 4).Value = 37100068;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 6).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 6).Value = "검진기관명";
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 8).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 8).Value = "포항성모병원";
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 10).ColumnSpan = 2;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 10).Value = "치과의사명";
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 12).Value = "전산실연습";
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(49, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(51, 4).ColumnSpan = 6;
            this.SSPrint_Dental_Sheet1.Cells.Get(51, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(51, 4).Value = "이 문서는 전자서명 되었습니다.,";
            this.SSPrint_Dental_Sheet1.Cells.Get(51, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(52, 1).ColumnSpan = 13;
            this.SSPrint_Dental_Sheet1.Cells.Get(52, 1).Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.SSPrint_Dental_Sheet1.Cells.Get(52, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Cells.Get(52, 1).Value = "※ 본 건강검진결과통보서는 상급병원에서 요양급여(진료)가 필요하다는 건강검진종합소견이 있는 경우 요양급여의뢰서(진료의뢰서)로 갈음됩니다.";
            this.SSPrint_Dental_Sheet1.Cells.Get(52, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSPrint_Dental_Sheet1.Columns.Get(0).Width = 5F;
            this.SSPrint_Dental_Sheet1.Columns.Get(1).Width = 54F;
            this.SSPrint_Dental_Sheet1.Columns.Get(2).Width = 54F;
            this.SSPrint_Dental_Sheet1.Columns.Get(3).Width = 54F;
            this.SSPrint_Dental_Sheet1.Columns.Get(4).Width = 81F;
            this.SSPrint_Dental_Sheet1.Columns.Get(5).Width = 54F;
            this.SSPrint_Dental_Sheet1.Columns.Get(6).Width = 54F;
            this.SSPrint_Dental_Sheet1.Columns.Get(7).Width = 54F;
            this.SSPrint_Dental_Sheet1.Columns.Get(8).Width = 54F;
            this.SSPrint_Dental_Sheet1.Columns.Get(9).Width = 54F;
            this.SSPrint_Dental_Sheet1.Columns.Get(10).Width = 54F;
            this.SSPrint_Dental_Sheet1.Columns.Get(11).Width = 54F;
            this.SSPrint_Dental_Sheet1.Columns.Get(12).Width = 98F;
            this.SSPrint_Dental_Sheet1.Columns.Get(13).Width = 54F;
            this.SSPrint_Dental_Sheet1.Columns.Get(14).Width = 6F;
            this.SSPrint_Dental_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSPrint_Dental_Sheet1.Rows.Get(3).Height = 30F;
            this.SSPrint_Dental_Sheet1.Rows.Get(5).Height = 13F;
            this.SSPrint_Dental_Sheet1.Rows.Get(6).Height = 13F;
            this.SSPrint_Dental_Sheet1.Rows.Get(7).Height = 13F;
            this.SSPrint_Dental_Sheet1.Rows.Get(8).Height = 13F;
            this.SSPrint_Dental_Sheet1.Rows.Get(9).Height = 13F;
            this.SSPrint_Dental_Sheet1.Rows.Get(10).Height = 13F;
            this.SSPrint_Dental_Sheet1.Rows.Get(11).Height = 13F;
            this.SSPrint_Dental_Sheet1.Rows.Get(12).Height = 13F;
            this.SSPrint_Dental_Sheet1.Rows.Get(13).Height = 13F;
            this.SSPrint_Dental_Sheet1.Rows.Get(14).Height = 13F;
            this.SSPrint_Dental_Sheet1.Rows.Get(15).Height = 13F;
            this.SSPrint_Dental_Sheet1.Rows.Get(16).Height = 13F;
            this.SSPrint_Dental_Sheet1.Rows.Get(17).Height = 54F;
            this.SSPrint_Dental_Sheet1.Rows.Get(19).Height = 10F;
            this.SSPrint_Dental_Sheet1.Rows.Get(20).Height = 10F;
            this.SSPrint_Dental_Sheet1.Rows.Get(23).Height = 21F;
            this.SSPrint_Dental_Sheet1.Rows.Get(24).Height = 41F;
            this.SSPrint_Dental_Sheet1.Rows.Get(26).Height = 41F;
            this.SSPrint_Dental_Sheet1.Rows.Get(32).Height = 10F;
            this.SSPrint_Dental_Sheet1.Rows.Get(37).Height = 9F;
            this.SSPrint_Dental_Sheet1.Rows.Get(38).Height = 84F;
            this.SSPrint_Dental_Sheet1.Rows.Get(39).Height = 9F;
            this.SSPrint_Dental_Sheet1.Rows.Get(47).Height = 11F;
            this.SSPrint_Dental_Sheet1.Rows.Get(50).Height = 10F;
            this.SSPrint_Dental_Sheet1.Rows.Get(52).Height = 26F;
            this.SSPrint_Dental_Sheet1.Rows.Get(53).Height = 5F;
            this.SSPrint_Dental_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmHcPrint_DentalPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(863, 637);
            this.Controls.Add(this.SSPrint_Dental);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcPrint_DentalPrint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmHcPrint_DentalPrint";
            ((System.ComponentModel.ISupportInitialize)(this.SSPrint_Dental)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSPrint_Dental_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread SSPrint_Dental;
        private FarPoint.Win.Spread.SheetView SSPrint_Dental_Sheet1;
        private System.Windows.Forms.Timer timer1;
    }
}