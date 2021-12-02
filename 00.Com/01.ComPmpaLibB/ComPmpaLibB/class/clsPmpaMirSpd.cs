using ComBase;
using FarPoint.Win.Spread.CellType;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    public class clsPmpaMirSpd
    {
        public string[] sBtnJSim = { "자료체크", "퇴원+의약품", "AU코드발생", "간호간병료발생", "간호간병료발생(내소정)" };

        #region 심사처방 화면 Spread Column 
        public enum enmJSimSlip
        {// 1               2               3               4               5               6
            BDATE,          SUCODE,         SUNEXT,         SUNAMEK,        chk01,          BASEAMT,
            OPGUBUN,        QTY,            NAL,            DIV,            AMT1,           AMT2,
            //GBSELF,         SELFNAME,       GBSUGBS,        GBNGT,          NGTNAME,        GBER,
            GBSELF,         GBSUGBS,        GBNGT,          GBER,
            GBSGADD,        GBSUGBAC,       GBSUGBAB,       GBSUGBAD,       HIRISK,         GBCHILD,
            GBGISUL,        BUN,            NU,             BCODE,          ENTDATE,        PART,
            DEPTCODE,       DRCODE,         WARDCODE,       ROOMCODE,       GBSLIP,         GBHOST,
            CHANGE,         NEW,            ROWID,          CBUN,           CNU,            SUB,
            SUGBQ,          SUGBF,          SUGBP,          SUGBG,          COLOR,          S_BCODE,
            SEQNO,          GBNGT2,         POWDER,         GBSUGBAG
        }

        public string[] sSpdJSimSlip = {
         // 1               2               3               4               5               6
            "처방일자",     "수가코드",     "품목코드",     "수가명",       "삭제",         "단가",
            "수술구분",     "수량",         "날수",         "횟수",         "계산금액",     "선택진료비",
            //"급여",         "급여명칭",     "선별급여",     "야간공휴",     "야간공휴명",   "응급가산",
            "급여",         "선별급여",     "야간공휴",     "응급가산",
            "외과가산",     "마취가산",     "판독가산",     "화상가산",     "고위험산모",   "나이가산",
            "기술가산",     "분류",         "누적",         "보험코드",     "입력시간",     "입력사번",
            "진료과",       "진료의사",     "병동코드",     "병실코드",     "청구구분",     "그룹코드",
            "변경",         "추가입력",     "ROWID",        "신분류",       "신누적",       "소계",
            "Q항",          "F항",          "P항",          "수량입력구분", "색상값",       "BCODE",
            "순번",         "GBNGT2",       "POW",          "ASA"
        };

        public int[] nSpdJSimSlip = {
         // 1               2               3               4               5               6
            70,             68,             68,             200,            30,             60,
            52,             40,             28,             28,             62,             66,
            30,             84,             58,             40,
            58,             58,             30,             30,             50,             62,
            40,             40,             40,             80,             100,            60,
            48,             60,             60,             60,             44,             44,
            30,             44,             60,             44,             44,             30,
            30,             30,             30,             44,             44,             70,
            30,             30,             30,             30
        };

        #region 재원심사, 처방 Spread Combo 배열

        string[] gArrCbo_Self = {
             ""
            ,"1.비급"
            ,"2.총액"
            };

        string[] gArrCbo_Ngt = {
             ""
            ,"1.공휴"
            ,"2.야간"
            ,"D.심야"
            };

        string[] gArrCbo_S = {
             ""
            ,"1.100%"
            ,"2.20%"
            ,"3.30%"
            ,"4.80%"
            ,"5.50%"
            ,"6.80%(선별)"
            ,"7.50%(선별)"
            ,"8.90%(선별)"
            ,"9.90%"

            };

        string[] gArrCbo_ER = {
             ""
            ,"A"
            ,"1"
            ,"2"
            ,"3"
            ,"4"
            ,"5"
            };

        string[] gArrCbo_P = {
             ""
            ,"1.인정"
            ,"2.임의"
            ,"9.제외"
            };

        string[] gArrCbo_Y = {
             ""
            ,"1.외과"
            ,"2.흉부"
            };

        string[] gArrCbo_AC = {
             ""
            ,"1.개두"
            ,"2.일측"
            ,"3.개흉"
            };

        string[] gArrCbo_OP = {
             ""
            ,"1.70%"
            ,"2.50%"
            ,"D.100%"
            };
        string[] gArrCbo = {
             ""
            ,"1"
               };

        #endregion

        //재원심사 화면 Spread
        public void sSpd_enmJSimSlip(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            clsSpread cSpd = new clsSpread();
            
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;

            if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmJSimSlip)).Length; }

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

            spd.VerticalScrollBarWidth = 16;
            spd.HorizontalScrollBarHeight = 16;

            //1.헤더 및 사이즈
            cSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //Enter Key 아래로 세팅
            //clsSpread.gSpreadEnter(spd);

            //2.컬럼 스타일
            cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Text);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.chk01,    clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.BDATE,    clsSpread.enmSpdType.Date);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.SUCODE,   clsSpread.enmSpdType.Text, null, null, null, null, false);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.SUNEXT,   clsSpread.enmSpdType.Label);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.SUNAMEK,  clsSpread.enmSpdType.Label);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBGISUL,  clsSpread.enmSpdType.Label);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBSELF,   clsSpread.enmSpdType.Text, null, null, null, null, false);
            //cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBSELF,   clsSpread.enmSpdType.ComboBox, gArrCbo_Self);
            //cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBNGT,    clsSpread.enmSpdType.Text, null, null, null, null, false);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBNGT,    clsSpread.enmSpdType.ComboBox, gArrCbo_Ngt);
            //cSpd.setColStyle(spd, -1, (int)enmJSimSlip.NGTNAME,  clsSpread.enmSpdType.Label);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBER,     clsSpread.enmSpdType.ComboBox, gArrCbo_ER);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.BUN,      clsSpread.enmSpdType.Label);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.NU,       clsSpread.enmSpdType.Label);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBSUGBS,  clsSpread.enmSpdType.ComboBox, gArrCbo_S);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.CBUN,     clsSpread.enmSpdType.Label);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.CNU,      clsSpread.enmSpdType.Label);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBSGADD,  clsSpread.enmSpdType.ComboBox, gArrCbo_Y);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBSUGBAB, clsSpread.enmSpdType.ComboBox, gArrCbo);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBSUGBAC, clsSpread.enmSpdType.ComboBox, gArrCbo_AC);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBSUGBAD, clsSpread.enmSpdType.ComboBox, gArrCbo);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.OPGUBUN,  clsSpread.enmSpdType.ComboBox, gArrCbo_OP);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.HIRISK,   clsSpread.enmSpdType.ComboBox, gArrCbo);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.BCODE,    clsSpread.enmSpdType.Label);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBNGT2,   clsSpread.enmSpdType.Label);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.POWDER,   clsSpread.enmSpdType.ComboBox, gArrCbo);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBSUGBAG, clsSpread.enmSpdType.ComboBox, gArrCbo);

            //편집불가능한 ComboBox 세팅
            //clsSpread.gSpreadComboDataSetEx1(spd, 0, (int)enmJSimSlip.GBSELF, spd.ActiveSheet.RowCount - 1, (int)enmJSimSlip.GBSELF, gArrCbo_Self, false);
            clsSpread.gSpreadComboDataSetEx1(spd, 0, (int)enmJSimSlip.GBNGT, spd.ActiveSheet.RowCount - 1, (int)enmJSimSlip.GBNGT, gArrCbo_Ngt, false);
            
            //2.1 숫자형 세팅
            cSpd.setColNumberDec(spd, -1, (int)enmJSimSlip.BASEAMT, 0);
            cSpd.setColNumberDec(spd, -1, (int)enmJSimSlip.QTY, 2);
            cSpd.setColNumberDec(spd, -1, (int)enmJSimSlip.NAL, 0);
            cSpd.setColNumberDec(spd, -1, (int)enmJSimSlip.DIV, 0);
            cSpd.setColNumberDec(spd, -1, (int)enmJSimSlip.AMT1, 0);
            cSpd.setColNumberDec(spd, -1, (int)enmJSimSlip.AMT2, 0);

            //길이
            cSpd.setColLength(spd, -1, (int)enmJSimSlip.GBSLIP, 1);
            cSpd.setColLength(spd, -1, (int)enmJSimSlip.GBGISUL, 1);
            cSpd.setColLength(spd, -1, (int)enmJSimSlip.GBSELF, 1);
            //cSpd.setColLength(spd, -1, (int)enmJSimSlip.GBNGT, 1);
            
            //3.정렬
            #region 정렬
            cSpd.setColAlign(spd, -1, clsSpread.HAlign_C,    clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.chk01,    clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.BDATE,    clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.SUCODE,   clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.SUNEXT,   clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.SUNAMEK,  clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.BASEAMT,  clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.QTY,      clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.NAL,      clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.DIV,      clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.GBNGT,    clsSpread.HAlign_L, clsSpread.VAlign_C);
            //cSpd.setColAlign(spd, (int)enmJSimSlip.GBSELF,   clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.GBSELF, clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.GBGISUL,  clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.AMT1,     clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.AMT2,     clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.GBER,     clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.GBSUGBS,  clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.OPGUBUN,  clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.BUN,      clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.NU,       clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.GBSGADD,  clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.GBSUGBAB, clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.GBSUGBAC, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.GBSUGBAD, clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.DEPTCODE, clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.DRCODE,   clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.WARDCODE, clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.ROOMCODE, clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.BCODE,    clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.ENTDATE,  clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.PART,     clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.CBUN,     clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.CNU,      clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimSlip.SEQNO,    clsSpread.HAlign_R, clsSpread.VAlign_C);
            #endregion

            //4.히든
            #region 히든
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.AMT2,     clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBCHILD,  clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.DEPTCODE, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.DRCODE,   clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.WARDCODE, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.ROOMCODE, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBSLIP,   clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBHOST,   clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.ENTDATE,  clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.PART,     clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.CBUN,     clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.CNU,      clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.SUB,      clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.SUGBF,    clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.SUGBQ,    clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.SUGBP,    clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.SUGBG,    clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.S_BCODE,  clsSpread.enmSpdType.Hide);
            //cSpd.setColStyle(spd, -1, (int)enmJSimSlip.CHANGE, clsSpread.enmSpdType.Hide);
            //cSpd.setColStyle(spd, -1, (int)enmJSimSlip.NEW, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.ROWID,    clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.COLOR,    clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimSlip.SEQNO,    clsSpread.enmSpdType.Hide);
            //cSpd.setColStyle(spd, -1, (int)enmJSimSlip.GBNGT2,   clsSpread.enmSpdType.Hide);
            #endregion

            // 5. sort, filter
            cSpd.setSpdFilter(spd, (int)enmJSimSlip.SUCODE, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)enmJSimSlip.SUNEXT, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)enmJSimSlip.SUNAMEK, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)enmJSimSlip.AMT1, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)enmJSimSlip.AMT2, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 

            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(196, 227, 191);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimSlip.SUCODE, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "0", false);
            unary.BackColor = Color.FromArgb(255, 192, 192);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimSlip.GBSELF, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(254, 252, 216);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimSlip.QTY, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(254, 252, 216);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimSlip.NAL, unary);

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            //unary.BackColor = Color.FromArgb(255, 192, 192);
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimSlip.SELFNAME, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimSlip.GBGISUL, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimSlip.GBNGT, unary);

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            //unary.BackColor = Color.FromArgb(232, 203, 155);
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimSlip.NGTNAME, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimSlip.GBER, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(255, 192, 192);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimSlip.GBSUGBS, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimSlip.GBSGADD, unary);
            
            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimSlip.GBSUGBAB, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimSlip.GBSUGBAC, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimSlip.GBSUGBAD, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimSlip.OPGUBUN, unary);
        }

        #endregion
        
        #region 재원심사 청구상병 Spread
        public enum enmJSimMirILL
        {
            chk01, RANK, ILLCODE, GBILL, LONGT, ILLNAME, ROWID
        }

        public string[] sJSimMirILL = {
            "삭제", "순위", "상병코드", "구분", "장기입원(F014)", "상병명", "ROWID"
        };

        public int[] nJSimMirILL = {
            30, 30, 56, 28, 40, 250, 60
        };

        //재원심사 청구상병 Spread
        public void sSpd_enmJSimMirILL(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            clsSpread cSpd = new clsSpread();

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;

            if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmJSimMirILL)).Length; }

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

            spd.VerticalScrollBarWidth = 16;
            spd.HorizontalScrollBarHeight = 16;

            //1.헤더 및 사이즈
            cSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            cSpd.setColStyle(spd, -1, (int)enmJSimMirILL.chk01, clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)enmJSimMirILL.ILLCODE, clsSpread.enmSpdType.Text, null, null, null, null, false);
            cSpd.setColStyle(spd, -1, (int)enmJSimMirILL.RANK, clsSpread.enmSpdType.number);
            cSpd.setColStyle(spd, -1, (int)enmJSimMirILL.GBILL, clsSpread.enmSpdType.Text);
            cSpd.setColNumberDec(spd, -1, (int)enmJSimMirILL.GBILL, 0);
            cSpd.setColNumberDec(spd, -1, (int)enmJSimMirILL.RANK, 0);

            //3.정렬
            cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimMirILL.RANK, clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimMirILL.ILLCODE, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimMirILL.ILLNAME, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimMirILL.LONGT, clsSpread.HAlign_C, clsSpread.VAlign_C);

            //4.히든
            cSpd.setColStyle(spd, -1, (int)enmJSimMirILL.RANK, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimMirILL.ROWID, clsSpread.enmSpdType.Hide);

            // 6. 특정문구 색상 

            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(196, 227, 191);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimMirILL.ILLCODE, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimMirILL.LONGT, unary);
        }
        #endregion
        
        #region 재원심사 환자목록 화면 Spread Column 
        public enum enmJSimPat          {  ROUT,       MIR,    GBSTS,      ILSU,   DRG,   GBOP,       GBJIPYO,        IPDETC,         GUBUN, BOHUN,   BI,     PANO,       SNAME,  WARD,   ROOM,   DEPT,     DRCODE,     INDATE,     JSIMDATE,     PART,     JSIMSABUN,  ILLCODE,  JSIMMEMO,       GUB,        IPDNO, TRSNO , CPNOTE }
        public string[] sSpdJSimPat =   { "퇴원예고", "청구", "입원상태", "일수", "DRG", "수술여부", "예방적항생제", "장기입원예외", "구분", "장애", "자격", "등록번호", "성명", "병동", "호실", "진료과", "진료의사", "입원일자", "최종심사일", "작업자", "심사담당", "상병",   "재원심사메모", "급여", "IPDNO", "TRSNO" ,"CP" };
        public int[] nSpdJSimPat    =   {   44,         34,     70,         38,    38,     38,          74,             40,            48,     38,    64,      60,         64,     38,     38,    38,       64,          78,         74,          64,        64,        200,      280,            30,         30,     30,      30  };

        //재원심사 환자 목록 Spread
        public void sSpd_enmJSimPat(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            clsSpread cSpd = new clsSpread();

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;

            if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmJSimPat)).Length; }

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

            spd.VerticalScrollBarWidth = 16;
            spd.HorizontalScrollBarHeight = 16;

            //1.헤더 및 사이즈
            cSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            //3.정렬
            #region 정렬
            cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimPat.ROUT, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimPat.GBSTS, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimPat.ILSU, clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimPat.GUBUN, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimPat.BI, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimPat.SNAME, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimPat.ILLCODE, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimPat.JSIMMEMO, clsSpread.HAlign_L, clsSpread.VAlign_C);
            #endregion

            //4.히든
            #region 히든
            cSpd.setColStyle(spd, -1, (int)enmJSimPat.IPDNO, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimPat.TRSNO, clsSpread.enmSpdType.Hide);
            #endregion

            // 5. sort, filter
            cSpd.setSpdFilter(spd, (int)enmJSimPat.SNAME, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)enmJSimPat.PANO, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)enmJSimPat.BI, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)enmJSimPat.WARD, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)enmJSimPat.ROOM, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)enmJSimPat.DEPT, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)enmJSimPat.INDATE, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)enmJSimPat.JSIMDATE, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 

            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            // unary.BackColor = Color.FromArgb(255, 192, 255);
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimPat.ROUT, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "장애", false);
            unary.BackColor = Color.FromArgb(196, 227, 191);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimPat.BOHUN, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimPat.IPDETC, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "D", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimPat.DRG, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimPat.GUB, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimPat.GBOP, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimPat.GBJIPYO, unary);

            //입원상태 색깔표시
            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "대조리스트인쇄", false);
            unary.BackColor = Color.FromArgb(250, 228, 192);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimPat.GBSTS, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "심사완료", false);
            unary.BackColor = Color.FromArgb(192, 255, 192);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimPat.GBSTS, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "퇴원계산서인쇄", false);
            unary.BackColor = Color.FromArgb(200, 200, 255);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimPat.GBSTS, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "퇴원수납완료", false);
            unary.BackColor = Color.FromArgb(192, 192, 255);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimPat.GBSTS, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            unary.BackColor = Color.FromArgb(192, 192, 255);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimPat.CPNOTE, unary);
        }
        #endregion

        #region 사전심사 수가목록 화면 Spread Column 
        public enum enmJSimScreen
        {// 1               2               3               4               5               6
            NU,             SUCODE,         HCODE,          SUNAMEK,        BASEAMT,        OPGUBUN,    QTY,
            NAL,            AMT1,           AMT2,           GBSPC,          GBNGT,          GBGISUL,
            GBSELF,         SUGBP,          GBSUGBS,        GBER,           GBCHILD,        GBSGADD,
            GBSUGBAB,       GBSUGBAC,       GBSUGBAD,       BCODE,          ENTDATE,        PART,
            CSUCODE,        CSUNEXT,        CBUN,           SUBTOTAL,       COLOR,          SUNEXT
        }

        public string[] sSpdJSimScreen =
        {// 1               2               3               4               5               6
            "행위구분",     "수가코드",     "한글수가",     "품목명(한글)", "단가",         "수술구분",    "수량",
            "날수",         "계산금액",     "특진료",       "특진",         "야간공휴",     "기술료가산",
            "급여",         "인정비급여",   "본인일부부담", "응급가산",     "나이가산",     "외과가산",
            "판독가산",     "마취가산",     "화상가산",     "EDI수가",      "입력일자",     "입력조",
            "신수가코드",   "신품목코드",   "신분류",       "소계",         "색상",         "품목코드"
        };

        public int[] nSpdJSimScreen =
        {// 1               2               3               4               5               6
            74,             74,             74,             180,            72,             64,         44,
            44,             72,             70,             40,             60,             40,
            68,             68,             90,             40,             64,             72,
            44,             100,            44,             80,             80,             60,
            74,             74,             44,             44,             40,             40
        };

        //사전심사 화면 Spread
        public void sSpd_enmJSimScreen(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            clsSpread cSpd = new clsSpread();

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;

            if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmJSimScreen)).Length; }

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

            spd.VerticalScrollBarWidth = 16;
            spd.HorizontalScrollBarHeight = 16;

            //1.헤더 및 사이즈
            cSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.GBER,     clsSpread.enmSpdType.ComboBox, gArrCbo_ER);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.GBSUGBS,  clsSpread.enmSpdType.ComboBox, gArrCbo_S);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.SUGBP,    clsSpread.enmSpdType.ComboBox, gArrCbo_P);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.GBSGADD,  clsSpread.enmSpdType.ComboBox, gArrCbo_Y);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.GBSUGBAC, clsSpread.enmSpdType.ComboBox, gArrCbo_AC);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.OPGUBUN,  clsSpread.enmSpdType.ComboBox, gArrCbo_OP);

            //2.1 숫자형 세팅
            cSpd.setColNumberDec(spd, -1, (int)enmJSimScreen.BASEAMT, 0);
            cSpd.setColNumberDec(spd, -1, (int)enmJSimScreen.QTY, 2);
            cSpd.setColNumberDec(spd, -1, (int)enmJSimScreen.NAL, 0);
            cSpd.setColNumberDec(spd, -1, (int)enmJSimScreen.AMT1, 0);
            cSpd.setColNumberDec(spd, -1, (int)enmJSimScreen.AMT2, 0);
            
            //3.정렬
            #region 정렬
            cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            
            cSpd.setColAlign(spd, (int)enmJSimScreen.SUCODE,   clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimScreen.HCODE,    clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimScreen.SUNAMEK,  clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimScreen.BASEAMT,  clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimScreen.QTY,      clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimScreen.NAL,      clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimScreen.GBSELF,   clsSpread.HAlign_L, clsSpread.VAlign_C);
            //cSpd.setColAlign(spd, (int)enmJSimScreen.GBNGT,    clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimScreen.AMT1,     clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimScreen.AMT2,     clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimScreen.GBSGADD,  clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimScreen.GBSUGBAC, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimScreen.BCODE,    clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimScreen.SUBTOTAL, clsSpread.HAlign_R, clsSpread.VAlign_C);

            #endregion

            //4.히든
            #region 히든

            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.CBUN, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.CSUCODE, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.CSUNEXT, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.AMT2, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.GBSPC, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.GBCHILD, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.SUBTOTAL, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.ENTDATE, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.PART, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.COLOR, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)enmJSimScreen.SUNEXT, clsSpread.enmSpdType.Hide);
            #endregion

            // 5. sort, filter
            //cSpd.setSpdFilter(spd, (int)enmJSimScreen.SUCODE, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //cSpd.setSpdFilter(spd, (int)enmJSimScreen.SUNAMEK, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //cSpd.setSpdFilter(spd, (int)enmJSimScreen.NU, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //cSpd.setSpdFilter(spd, (int)enmJSimScreen.AMT2, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 

            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, " ", false);
            unary.BackColor = Color.FromArgb(196, 227, 191);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimScreen.SUCODE, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimScreen.GBSELF, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimScreen.GBER, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimScreen.GBNGT, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimScreen.GBSUGBS, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            unary.BackColor = Color.FromArgb(232, 203, 155);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimScreen.GBGISUL, unary);

        }
        #endregion

        #region 재원심사 History
        public enum enmJSimHis
        {
            GUBUN, SIMSASNAME, SIMSAYN, GBSTS, PANO, SNAME, BI, INDATE, OUTDATE
        }

        public string[] sJSimMirHis = {
            "구분", "등록자", "등록시각", "입원상태", "등록번호", "환자성명", "보험", "입원일자", "퇴원일자"
        };

        public int[] nJSimMirHis = {
            44, 48, 84, 68, 48, 48, 30, 68, 68
        };

        //재원심사 History Spread
        public void sSpd_enmJSimMirHis(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            clsSpread cSpd = new clsSpread();

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;

            if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmJSimHis)).Length; }

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

            //spd.VerticalScrollBarWidth = 4;
            //spd.HorizontalScrollBarHeight = 4;

            //1.헤더 및 사이즈
            cSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            
            //3.정렬
            cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)enmJSimHis.GBSTS, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든
            //cSpd.setColStyle(spd, -1, (int)enmJSimHis.ROWID, clsSpread.enmSpdType.Hide);

            // 6. 특정문구 색상 
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "심사중", false);
            unary.BackColor = Color.FromArgb(255, 255, 128);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmJSimHis.GUBUN, unary);

        }
        #endregion
    }
}
