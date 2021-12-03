/// <summary>
/// Description     : 건진센터 공용모듈 / Spread Setting 관련
/// Author          : 김민철
/// Create Date     : 2019-07-12
/// Update History  : 
/// </summary>
/// <history>  
/// </history>

namespace ComHpcLibB
{
    using ComBase;
    using System;
    using FarPoint.Win.Spread;
    using System.Drawing;
    using FarPoint.Win.Spread.CellType;
    using ComBase.Mvc.Spread;
    using ComBase.Controls;
    using ComHpcLibB.Dto;

    public class clsHcSpd
    {
        #region 검진 기초코드 관리 영역
        public enum     enmHcCode {  chk01,  GUBUN,  CODE,   NAME,   GUBUN1,  GUBUN2,  GUBUN3,  SORT,   GBDEL,  ROWID,   CHANGE }
        public string[] sHcCode = { "삭제", "구분", "코드", "내용", "구분1", "구분2", "구분3", "정렬", "삭제", "ROWID", "변경" };
        public int[]    nHcCode = {  30,     30,     56,     240,    180,     180,     180,     30,     30,     44,      30 };

        public void sSpd_enmHcCode(FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            try
            {
                clsSpread cSpd = new clsSpread();

                //스프레드 사이즈
                spd.ActiveSheet.RowCount = RowCnt;
                spd.ActiveSheet.ColumnCount = ColCnt;

                if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmHcCode)).Length; }

                spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

                spd.VerticalScrollBarWidth = 16;
                spd.HorizontalScrollBarHeight = 16;

                //1.헤더 및 사이즈
                cSpd.setHeader(spd, colName, size);
                spd.ActiveSheet.Columns.Get(-1).Visible = true;

                //2.컬럼 스타일
                cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Text, null, null, null, null, false);
                cSpd.setColStyle(spd, -1, (int)enmHcCode.chk01, clsSpread.enmSpdType.CheckBox);
                cSpd.setColStyle(spd, -1, (int)enmHcCode.CHANGE, clsSpread.enmSpdType.Text);

                //3.정렬
                cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcCode.NAME, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcCode.GUBUN1, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcCode.GUBUN2, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcCode.GUBUN3, clsSpread.HAlign_L, clsSpread.VAlign_C);

                //4.히든
                cSpd.setColStyle(spd, -1, (int)enmHcCode.GUBUN, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmHcCode.GBDEL, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmHcCode.ROWID, clsSpread.enmSpdType.Hide);

                //5.Filter
                cSpd.setSpdFilter(spd, (int)enmHcCode.CODE, AutoFilterMode.EnhancedContextMenu, true);
                cSpd.setSpdFilter(spd, (int)enmHcCode.NAME, AutoFilterMode.EnhancedContextMenu, true);
                cSpd.setSpdFilter(spd, (int)enmHcCode.GUBUN1, AutoFilterMode.EnhancedContextMenu, true);
                cSpd.setSpdFilter(spd, (int)enmHcCode.GUBUN2, AutoFilterMode.EnhancedContextMenu, true);
                cSpd.setSpdFilter(spd, (int)enmHcCode.GUBUN3, AutoFilterMode.EnhancedContextMenu, true);

                // 6. 특정문구 색상 
                UnaryComparisonConditionalFormattingRule unary;

                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "Y", false);
                unary.BackColor = Color.DarkRed;
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcCode.GBDEL, unary);
                //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcCode.NAME, unary);
                //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcCode.GBDEL, unary);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }
        #endregion

        #region 검진 검사코드 관리 영역
        public enum     enmHcExCode {  SEQ,    ACT,        CODE,       RES,    HNAME,         ENAME,         GCODE,      SUCODE,     SEND,   SEL,    AMT1,      AMT2,       AMT3,       AMT4,        AMT5,       XNAME,    ORDERCODE,  DELDATE,   ROWID }
        public string[] sHcExCode = { "순번", "계측구분", "검사코드", "결과", "한글 코드명", "영문 코드명", "공단코드", "수가코드", "전송", "선택", "보험80%", "보험100%", "보험125%", "특+일차액", "임의수가", "촬영명", "오더코드", "삭제일자","ROWID" };
        public int[]    nHcExCode = {  42,     42,         54,         42,     220,           140,           44,         58,         30,     30,     72,        72,         72,         72,          72,         78,       78,         64,        40 };

        public void sSpd_enmHcExCode(FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            try
            {
                clsSpread cSpd = new clsSpread();

                //스프레드 사이즈
                spd.ActiveSheet.RowCount = RowCnt;
                spd.ActiveSheet.ColumnCount = ColCnt;

                if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmHcExCode)).Length; }

                spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

                spd.VerticalScrollBarWidth = 16;
                spd.HorizontalScrollBarHeight = 16;

                //1.헤더 및 사이즈
                cSpd.setHeader(spd, colName, size);
                spd.ActiveSheet.Columns.Get(-1).Visible = true;

                //2.컬럼 스타일
                cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
                
                //3.정렬
                cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcExCode.CODE, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcExCode.HNAME, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcExCode.ENAME, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcExCode.AMT1, clsSpread.HAlign_R, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcExCode.AMT2, clsSpread.HAlign_R, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcExCode.AMT3, clsSpread.HAlign_R, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcExCode.AMT4, clsSpread.HAlign_R, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcExCode.AMT5, clsSpread.HAlign_R, clsSpread.VAlign_C);

                //4.히든
                cSpd.setColStyle(spd, -1, (int)enmHcExCode.DELDATE, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmHcExCode.ROWID, clsSpread.enmSpdType.Hide);

                //5.Filter
                cSpd.setSpdFilter(spd, (int)enmHcExCode.CODE, AutoFilterMode.EnhancedContextMenu, true);
                cSpd.setSpdFilter(spd, (int)enmHcExCode.HNAME, AutoFilterMode.EnhancedContextMenu, true);
                cSpd.setSpdFilter(spd, (int)enmHcExCode.ENAME, AutoFilterMode.EnhancedContextMenu, true);

                // 6. 특정문구 색상 
                UnaryComparisonConditionalFormattingRule unary;

                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "", false);
                unary.ForeColor = Color.DarkRed;
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcExCode.DELDATE, unary);

                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "Y", false);
                unary.BackColor = Color.FromArgb(255, 224, 192);
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcExCode.SEL, unary);

                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "Y", false);
                unary.BackColor = Color.FromArgb(255, 224, 192);
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcExCode.SEND, unary);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }
        #endregion

        #region 보류대장 등록 관리 영역
        public enum     enmHcPendList { chk01,  GUBUN,   JEPDATE,    WRTNO,      SNAME,      LTDNAME,  HPHONE,       CHUL,   EXAMS,        SAYU,   END,    ENDDATE,    SPECDATE, SPECSABUN,     ENTSABUN, ENDSABUN, CHANGE, ROWID, TONGBODATE, TONGBOGBN };
        public string[] sHcPendList = { "삭제", "구분", "검진일자", "접수번호", "수검자명", "회사명", "휴대폰번호", "출장", "누락검사명", "사유", "완료", "완료일자", "검체제출일", "검체제출일", "보고자", "완료자", "변경", "ROWID","통보일자", "통보방법"};
        public int[]    nHcPendList = { 34,     44,      72,         64,         72,         160,      86,           38,     180,          180,    34,     72,         72, 72,       72,       30,     50,    78,          52 };
        public void sSpd_enmHcPendList(FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            try
            {
                clsSpread cSpd = new clsSpread();

                //스프레드 사이즈
                spd.ActiveSheet.RowCount = RowCnt;
                spd.ActiveSheet.ColumnCount = ColCnt;

                if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmHcPendList)).Length; }

                spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

                spd.VerticalScrollBarWidth = 16;
                spd.HorizontalScrollBarHeight = 16;

                //1.헤더 및 사이즈
                cSpd.setHeader(spd, colName, size);
                spd.ActiveSheet.Columns.Get(-1).Visible = true;

                //2.컬럼 스타일
                cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
                cSpd.setColStyle(spd, -1, (int)enmHcPendList.chk01,     clsSpread.enmSpdType.CheckBox);
                cSpd.setColStyle(spd, -1, (int)enmHcPendList.END,       clsSpread.enmSpdType.CheckBox);
                //cSpd.setColStyle(spd, -1, (int)enmHcPendList.JEPDATE,   clsSpread.enmSpdType.Date);
                cSpd.setColStyle(spd, -1, (int)enmHcPendList.SPECDATE,  clsSpread.enmSpdType.Date);
                cSpd.setColStyle(spd, -1, (int)enmHcPendList.EXAMS,     clsSpread.enmSpdType.Text, null, "", "", "", false);
                cSpd.setColStyle(spd, -1, (int)enmHcPendList.SAYU,      clsSpread.enmSpdType.Text, null, "", "", "", false);

                //3.정렬
                cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcPendList.LTDNAME,   clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcPendList.HPHONE,    clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcPendList.EXAMS,     clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcPendList.SAYU,      clsSpread.HAlign_L, clsSpread.VAlign_C);

                //4.히든
                cSpd.setColStyle(spd, -1, (int)enmHcPendList.ROWID, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmHcPendList.ENDDATE, clsSpread.enmSpdType.Hide); //완료일자
                cSpd.setColStyle(spd, -1, (int)enmHcPendList.SPECSABUN, clsSpread.enmSpdType.Hide); //검체제출등록자
                cSpd.setColStyle(spd, -1, (int)enmHcPendList.CHANGE, clsSpread.enmSpdType.Hide);

                //5.Filter
                //cSpd.setSpdFilter(spd, (int)enmHcPendList.SAYU, AutoFilterMode.EnhancedContextMenu, true);

                // 6. 특정문구 색상 
                UnaryComparisonConditionalFormattingRule unary;

                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "종검", false);
                unary.BackColor = Color.MistyRose;
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcPendList.GUBUN, unary);

                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
                unary.BackColor = Color.PaleGoldenrod;
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcPendList.EXAMS, unary);

                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
                unary.BackColor = Color.FromArgb(198, 223, 193);
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcPendList.SAYU, unary);

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }
        #endregion
        
        #region 판정/상담 대상 환자명단 조회 영역
        public enum     enmHcPanPatList {  WRTNO,      SNAME,  EXNAME,     LTDNAME,  JEPDATE,    GJCHASU,  DOCTOR,  XREAD,    JONGGUMYN,  SEX,    AGE,    PTNO,       GBCHUL,  GBSTSNM,     GBSTS,          GJJONG,  JUMIN,      JUMIN2,     CLASS,  BAN,  BUN,    PANJENGDRNO,    GAPANJENGNAME,   PANO,   IEMUNNO,          GWRTNO,   LTDCODE  };
        public string[] sHcPanPatList = { "접수번호", "성명", "검진종류", "회사명", "검진일자", "차수",   "의사",  "미판독", "종검수검", "성별", "나이", "외래번호", "출장",  "입력완료",  "입력완료여부", "종류",  "주민번호", "주민번호", "학년", "반", "번호", "판정의사번호", "가판정",        "PANO", "인터넷문진번호", "GWRTNO", "사업장코드" };
        public int[]    nHcPanPatList = {  64,         74,     100,        160,      72,         30,       74,      30,        42,        80,     32,     38,         30,      32,          32,             50,      70,         70,         44,     44,   44,     50,             80,              150,    50,               50,       50 };

        //public enum enmHcPanPatList_School { LTDNAME,    SNAME,  JEPDATE,    WRTNO,      JUMIN,      SEX,    LTDCODE,    CLASS,  BAN,    BUN,    AGE,    RID,     PTNO,       GWRTNO };
        //public string[] sHcPanPatList_School = { "학교", "성명", "검진일자", "접수번호", "주민번호", "성별", "회사코드", "학년", "학반", "학번", "나이", "ROWID", "외래번호", "GWRTNO" };
        //public int[] nHcPanPatList_School = { 160,       74,     72,         64,         150,        30,      72,         30,     30,    30,     34,     100,     70,         80 };
        
        public void sSpd_enmHcPanPatList(FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            try
            {
                //clsSpread cSpd = new clsSpread();

                ////스프레드 사이즈
                //spd.ActiveSheet.RowCount = RowCnt;
                //spd.ActiveSheet.ColumnCount = ColCnt;

                //if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmHcPendList)).Length; }

                //spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

                //spd.VerticalScrollBarWidth = 16;
                //spd.HorizontalScrollBarHeight = 16;

                ////1.헤더 및 사이즈
                //cSpd.setHeader(spd, colName, size);
                //spd.ActiveSheet.Columns.Get(-1).Visible = true;

                ////2.컬럼 스타일
                //cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
               
                ////3.정렬
                //cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
                //cSpd.setColAlign(spd, (int)enmHcPanPatList.LTDNAME,   clsSpread.HAlign_L, clsSpread.VAlign_C);
                //cSpd.setColAlign(spd, (int)enmHcPanPatList.EXNAME,    clsSpread.HAlign_L, clsSpread.VAlign_C);
                //cSpd.setColAlign(spd, (int)enmHcPanPatList.SNAME,     clsSpread.HAlign_L, clsSpread.VAlign_C);

                ////4.히든
                //cSpd.setColStyle(spd, -1, (int)enmHcPanPatList.GJJONG,  clsSpread.enmSpdType.Hide);
                //cSpd.setColStyle(spd, -1, (int)enmHcPanPatList.GBSTS,   clsSpread.enmSpdType.Hide);
                //cSpd.setColStyle(spd, -1, (int)enmHcPanPatList.PANDRNO, clsSpread.enmSpdType.Hide);
                //cSpd.setColStyle(spd, -1, (int)enmHcPanPatList.JUMIN, clsSpread.enmSpdType.Hide);
                ////5.Filter
                //cSpd.setSpdFilter(spd, (int)enmHcPanPatList.SNAME, AutoFilterMode.EnhancedContextMenu, true);
                //cSpd.setSpdFilter(spd, (int)enmHcPanPatList.LTDNAME, AutoFilterMode.EnhancedContextMenu, true);
                //cSpd.setSpdFilter(spd, (int)enmHcPanPatList.PTNO, AutoFilterMode.EnhancedContextMenu, true);

                //// 6. 특정문구 색상 
                //UnaryComparisonConditionalFormattingRule unary;

                //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "N", false);
                //unary.ForeColor = Color.DarkRed;
                //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcPanPatList.GBSTSNM, unary);

                //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "", false);
                //unary.BackColor = Color.Gold;
                //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcPanPatList.DOCTOR, unary);

                //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "Y", false);
                //unary.BackColor = Color.LightSalmon;
                //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcPanPatList.GBCHUL, unary);

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        public void sSpd_enmHcPanPatList_School(FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            try
            {
                //clsSpread cSpd = new clsSpread();

                ////스프레드 사이즈
                //spd.ActiveSheet.RowCount = RowCnt;
                //spd.ActiveSheet.ColumnCount = ColCnt;

                //if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmHcPendList)).Length; }

                //spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

                //spd.VerticalScrollBarWidth = 16;
                //spd.HorizontalScrollBarHeight = 16;

                ////1.헤더 및 사이즈
                //cSpd.setHeader(spd, colName, size);
                //spd.ActiveSheet.Columns.Get(-1).Visible = true;

                ////2.컬럼 스타일
                //cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

                ////3.정렬
                //cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
                //cSpd.setColAlign(spd, (int)enmHcPanPatList_School.LTDNAME, clsSpread.HAlign_L, clsSpread.VAlign_C);
                //cSpd.setColAlign(spd, (int)enmHcPanPatList_School.SNAME, clsSpread.HAlign_L, clsSpread.VAlign_C);

                ////4.히든
                //cSpd.setColStyle(spd, -1, (int)enmHcPanPatList_School.RID, clsSpread.enmSpdType.Hide);
                //cSpd.setColStyle(spd, -1, (int)enmHcPanPatList_School.LTDCODE, clsSpread.enmSpdType.Hide);
                ////5.Filter
                //cSpd.setSpdFilter(spd, (int)enmHcPanPatList_School.SNAME, AutoFilterMode.EnhancedContextMenu, true);
                //cSpd.setSpdFilter(spd, (int)enmHcPanPatList_School.LTDNAME, AutoFilterMode.EnhancedContextMenu, true);
                //cSpd.setSpdFilter(spd, (int)enmHcPanPatList_School.PTNO, AutoFilterMode.EnhancedContextMenu, true);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }
        #endregion

        #region 검진 기초코드 관리 영역

        public string[] gArrHcTax = {
             ""
            ,"종검"
            ,"일반"
            ,"측정"
            ,"대행"
            };

        public enum     enmHcTax {  chk01,  GUBUN,  BONAME,   JIK,    TEL,        HPHONE,   EMAIL,    REMARK,     ROWID,   CHANGE   }
        public string[] sHcTax = { "삭제", "구분", "담당자", "직책", "전화번호", "휴대폰", "이메일", "참고사항", "ROWID", "변경"    };
        public int[]    nHcTax = {  30,     52,     56,       56,     88,         88,       140,      154,        30,      30       };

        public void sSpd_enmHcTax(FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            try
            {
                clsSpread cSpd = new clsSpread();

                //스프레드 사이즈
                spd.ActiveSheet.RowCount = RowCnt;
                spd.ActiveSheet.ColumnCount = ColCnt;

                if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmHcTax)).Length; }

                spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

                spd.VerticalScrollBarWidth = 16;
                spd.HorizontalScrollBarHeight = 16;

                //1.헤더 및 사이즈
                cSpd.setHeader(spd, colName, size);
                spd.ActiveSheet.Columns.Get(-1).Visible = true;

                //2.컬럼 스타일
                cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Text, null, null, null, null, false);
                cSpd.setColStyle(spd, -1, (int)enmHcTax.chk01, clsSpread.enmSpdType.CheckBox);
                cSpd.setColStyle(spd, -1, (int)enmHcTax.GUBUN, clsSpread.enmSpdType.ComboBox, gArrHcTax);

                //3.정렬
                cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcTax.EMAIL, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcTax.REMARK, clsSpread.HAlign_L, clsSpread.VAlign_C);

                //3_1. 셀길이
                cSpd.setColLength(spd, -1, (int)enmHcTax.TEL, 13);
                cSpd.setColLength(spd, -1, (int)enmHcTax.HPHONE, 13);

                //4.히든
                cSpd.setColStyle(spd, -1, (int)enmHcTax.ROWID, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmHcTax.CHANGE, clsSpread.enmSpdType.Hide);

                //5.Filter
                //cSpd.setSpdFilter(spd, (int)enmHcTax.CODE, AutoFilterMode.EnhancedContextMenu, true);

                // 6. 특정문구 색상 
                //UnaryComparisonConditionalFormattingRule unary;

                //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "Y", false);
                //unary.BackColor = Color.DarkRed;
                //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcTax.GBDEL, unary);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }
        #endregion

        #region 검진수검자 메모 관리 영역
        public enum enmHcMemo { GBDEL, JOBGBN, ENTDATE, MEMO, ENTSABUN, ENTNAME, CHANGE, ROWID }
        #endregion

        #region frmHcEntryCardDaou.cs 관리영역
        public void sSpd_enmCardApprov(FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            clsSpread cSpd = new clsSpread();

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;

            if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(clsHcType.enmCardApprov)).Length; }

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

            spd.VerticalScrollBarWidth = 16;
            spd.HorizontalScrollBarHeight = 16;

            //1.헤더 및 사이즈
            cSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;
            spd.ActiveSheet.RowHeader.Visible = false;

            //2.컬럼 스타일
            cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            //3.정렬
            cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsHcType.enmCardApprov.TRADEAMT, clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsHcType.enmCardApprov.FINAME, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든
            #region 히든
            cSpd.setColStyle(spd, -1, (int)clsHcType.enmCardApprov.ROWID, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsHcType.enmCardApprov.TRANHEADER, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsHcType.enmCardApprov.ORIGINNO2, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsHcType.enmCardApprov.GUBUN1, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsHcType.enmCardApprov.DIV, clsSpread.enmSpdType.Hide);
            #endregion

            // 5. sort, filter
            //cSpd.setSpdFilter(spd, (int)enmJSimScreen.SUCODE, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            //6.특정문구 색상
            //UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "", false);
            //unary.BackColor = Color.FromArgb(196, 227, 191);
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsHcType.enmCardApprov.HPAY, unary);

        }
        #endregion

        #region 작업환경측정 결과입력 관리영역
        public enum enmHcWemRes
        {
            chkSel
                , WRTNO
                , GUBUN
                , SEQNO
                , DEPT_NM
                , PROCS_CD
                , PROCS_NM
                , btnPROCS
                , UNIT_WRKRUM_NM
                , CHMCLS_CD
                , UCODE_GROUP_CD
                , UCODE_GROUP_SEQ
                , CHMCLS_NM
                , btnCHMCLS
                , LABRR_CD
                , LABOR_CND
                , LABOR_TIME
                , UCODE_EXPSR_TIME    
                , WEM_LC              
                , LABRR_NM            
                , WEM_TIME_FROM       
                , WEM_TIME_TO         
                , WEM_CO              
                , WEM_VALUE_AVRG_ETC  
                , WEM_VALUE_AVRG_NM
                , WEM_VALUE_AVRG      
                , WEM_VALUE_PREV_ETC
                , WEM_VALUE_PREV_NM
                , WEM_VALUE_PREV      
                , WEM_VALUE_NOW_ETC
                , WEM_VALUE_NOW_NM
                , WEM_VALUE_NOW       
                , EXPSR_STDR_default
                , EXPSR_STDR_VALUE    
                , EXPSR_STDR_SE       
                , EXPSR_STDR_UNIT     
                , WEN_EVL_RESULT      
                , ANALS_MTH_CD        
                , WEM_MTH_NM          
                , ANALS_MTH_NM        
                , btnANAL
                , REMARK
                , RID
        }
        public enum enmHcWemRes_Noise
        {
            chkSel
                , WRTNO
                , GUBUN
                , SEQNO
                , DEPT_NM
                , PROCS_CD
                , PROCS_NM
                , btnPROCS
                , UNIT_WRKRUM_NM
                , CHMCLS_CD
                , UCODE_GROUP_CD
                , UCODE_GROUP_SEQ
                , CHMCLS_NM
                , LABRR_CD
                , OPERT_CN
                , LABOR_CND
                , LABOR_TIME
                , UCODE_EXPSR_CYCLE
                , UCODE_EXPSR_TIME
                , WEM_LC
                , LABRR_NM
                , WEM_TIME_FROM
                , WEM_TIME_TO
                , WEM_CO
                , WEM_VALUE_AVRG_ETC
                , WEM_VALUE_AVRG_NM
                , WEM_VALUE_AVRG
                , WEM_VALUE_PREV_ETC
                , WEM_VALUE_PREV_NM
                , WEM_VALUE_PREV
                , WEM_VALUE_NOW_ETC
                , WEM_VALUE_NOW_NM
                , WEM_VALUE_NOW
                , EXPSR_STDR_default
                , EXPSR_STDR_VALUE
                , EXPSR_STDR_SE
                , EXPSR_STDR_UNIT
                , WEN_EVL_RESULT
                , ANALS_MTH_CD
                , WEM_MTH_NM
                , ANALS_MTH_NM
                , btnANAL
                , REMARK
                , RID
        }
        public string[] sHcWemRes = {
            "선택"
                ,"일련번호"
                ,"구분"
                ,"순번"
                ,"부서명"
                ,"공정코드"
                ,"공정명"
                ,"H"
                ,"단위작업명"
                ,"유해물질코드"
                ,"그룹"
                ,"그룹순번"
                ,"유해물질명"
                ,"H"
                ,"근로자수"
                ,"근로형태"
                ,"발생시간(분)"
                ,"측정위치"
                ,"근로자명"
                ,"측정시작"
                ,"측정종료"
                ,"측정횟수"
                ,"ETC(평균)"
                ,""
                ,"측정치"
                ,"ETC(전회)"
                ,""
                ,"전회"
                ,"ETC(금회)"
                ,""
                ,"금회"
                ,"노출기준"
                ,"노출구분"
                ,"노출단위"
                ,"측정평가결과"
                ,"분석방법코드"
                ,"채취방법"
                ,"분석방법"
                ,"H"
                ,"비고"
                ,"ROWID"
        };
        public string[] dHcWemRes = {
            nameof(HIC_CHUKDTL_RESULT.IsDelete)
                , nameof(HIC_CHUKDTL_RESULT.WRTNO) 
                , nameof(HIC_CHUKDTL_RESULT.GUBUN)
                , nameof(HIC_CHUKDTL_RESULT.SEQNO)
                , nameof(HIC_CHUKDTL_RESULT.DEPT_NM)
                , nameof(HIC_CHUKDTL_RESULT.PROCS_CD)
                , nameof(HIC_CHUKDTL_RESULT.PROCS_NM)
                ,""
                , nameof(HIC_CHUKDTL_RESULT.UNIT_WRKRUM_NM)
                , nameof(HIC_CHUKDTL_RESULT.CHMCLS_CD)
                , nameof(HIC_CHUKDTL_RESULT.UCODE_GROUP_CD)
                , nameof(HIC_CHUKDTL_RESULT.UCODE_GROUP_SEQ)
                , nameof(HIC_CHUKDTL_RESULT.CHMCLS_NM)
                ,""
                , nameof(HIC_CHUKDTL_RESULT.LABRR_CD)
                , nameof(HIC_CHUKDTL_RESULT.LABOR_CND)
                , nameof(HIC_CHUKDTL_RESULT.UCODE_EXPSR_TIME)
                , nameof(HIC_CHUKDTL_RESULT.WEM_LC)
                , nameof(HIC_CHUKDTL_RESULT.LABRR_NM)
                , nameof(HIC_CHUKDTL_RESULT.WEM_TIME_FROM)
                , nameof(HIC_CHUKDTL_RESULT.WEM_TIME_TO)
                , nameof(HIC_CHUKDTL_RESULT.WEM_CO)
                , nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_AVRG_ETC)
                , ""
                , nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_AVRG)
                , nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_PREV_ETC)
                , ""
                , nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_PREV)
                , nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_NOW_ETC)
                , ""
                , nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_NOW)
                , nameof(HIC_CHUKDTL_RESULT.EXPSR_STDR_VALUE)
                , nameof(HIC_CHUKDTL_RESULT.EXPSR_STDR_SE)
                , nameof(HIC_CHUKDTL_RESULT.EXPSR_STDR_UNIT)
                , nameof(HIC_CHUKDTL_RESULT.WEN_EVL_RESULT)
                , nameof(HIC_CHUKDTL_RESULT.ANALS_MTH_CD)
                , nameof(HIC_CHUKDTL_RESULT.WEM_MTH_NM)
                , nameof(HIC_CHUKDTL_RESULT.ANALS_MTH_NM)
                ,""
                , nameof(HIC_CHUKDTL_RESULT.REMARK)
                , nameof(HIC_CHUKDTL_RESULT.RID)
        };
        public int[] nHcWemRes = {
            38,  99, 99, 38, 52, 44,
            130, 22, 84, 54, 38, 99,
            130, 22, 44, 92, 44, 44,
            84,  44, 44, 44, 44, 84,
            52,  44, 84, 52, 44, 84,
            52,  44, 72, 62, 58, 99,
            92,  92, 22, 92, 99
        };

        public void sSpd_enmHcWemRes(FpSpread spd, string[] colName, string[] dataField, int[] size, int RowCnt, int ColCnt)
        {
            try
            {
                clsSpread cSpd = new clsSpread();

                //스프레드 사이즈
                spd.ActiveSheet.RowCount = RowCnt;
                spd.ActiveSheet.ColumnCount = ColCnt;

                if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmHcWemRes)).Length; }

                spd.ActiveSheet.ColumnHeader.Rows[0].Height = 44;

                spd.VerticalScrollBarWidth = 16;
                spd.HorizontalScrollBarHeight = 16;

                //1.헤더 및 사이즈
                cSpd.setHeader(spd, colName, size, true, dataField);
                spd.ActiveSheet.Columns.Get(-1).Visible = true;

                //2.컬럼 스타일
                cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
                cSpd.setColStyle(spd, -1, (int)enmHcWemRes.chkSel, clsSpread.enmSpdType.CheckBox);
                cSpd.setColStyle(spd, -1, (int)enmHcWemRes.btnPROCS, clsSpread.enmSpdType.Button, new SpreadCellTypeOption { ButtonText = "H" });
                cSpd.setColStyle(spd, -1, (int)enmHcWemRes.btnCHMCLS, clsSpread.enmSpdType.Button, new SpreadCellTypeOption { ButtonText = "H" });
                cSpd.setColStyle(spd, -1, (int)enmHcWemRes.btnANAL, clsSpread.enmSpdType.Button, new SpreadCellTypeOption { ButtonText = "H" });
                cSpd.setColStyle(spd, -1, (int)enmHcWemRes.SEQNO, clsSpread.enmSpdType.number, new SpreadCellTypeOption { DecimalPlaces = 3 });
                cSpd.setColStyle(spd, -1, (int)enmHcWemRes.UCODE_EXPSR_TIME, clsSpread.enmSpdType.number, new SpreadCellTypeOption { });
                cSpd.setColStyle(spd, -1, (int)enmHcWemRes.WEM_CO, clsSpread.enmSpdType.number, new SpreadCellTypeOption { });

                //3.정렬
                cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcWemRes.DEPT_NM, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcWemRes.PROCS_NM, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcWemRes.UNIT_WRKRUM_NM, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcWemRes.CHMCLS_NM, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcWemRes.LABOR_CND, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcWemRes.WEM_VALUE_AVRG, clsSpread.HAlign_R, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcWemRes.WEM_VALUE_PREV, clsSpread.HAlign_R, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcWemRes.WEM_VALUE_NOW, clsSpread.HAlign_R, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcWemRes.WEM_MTH_NM, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcWemRes.ANALS_MTH_NM, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcWemRes.REMARK, clsSpread.HAlign_L, clsSpread.VAlign_C);

                //4.히든
                cSpd.setColStyle(spd, -1, (int)enmHcWemRes.WRTNO, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmHcWemRes.GUBUN, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmHcWemRes.UCODE_GROUP_SEQ, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmHcWemRes.EXPSR_STDR_SE, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmHcWemRes.ANALS_MTH_CD, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmHcWemRes.RID, clsSpread.enmSpdType.Hide);

                //5.Filter
                //cSpd.setSpdFilter(spd, (int)enmHcPendList.SAYU, AutoFilterMode.EnhancedContextMenu, true);

                // 6. 특정문구 색상 
                //UnaryComparisonConditionalFormattingRule unary;

                //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "종검", false);
                //unary.BackColor = Color.MistyRose;
                //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcPendList.GUBUN, unary);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }
        #endregion
    }

    public class GbStsCellType : TextCellType, ICustomCellType
    {
        public FpSpread fpspread { get; set; }

        public override void PaintCell(Graphics g, Rectangle r, Appearance appearance, object value, bool isSelected, bool isLocked, float zoomFactor)
        {
            if (value.IsNullOrEmpty())
            {
                return;
            }
            else
            {
                if (value.ToString().Equals("예약접수"))
                {
                    appearance.BackColor = Color.White;
                }
                else if (value.ToString().Equals("수검등록"))
                {
                    appearance.BackColor = Color.LightYellow;
                }
                else if (value.ToString().Equals("결과입력중"))
                {
                    appearance.BackColor = Color.LightGreen;
                }
                else if (value.ToString().Equals("결과입력완료"))
                {
                    appearance.BackColor = Color.LightSkyBlue;
                }
                else if (value.ToString().Equals("가판정완료"))
                {
                    appearance.BackColor = Color.LightPink;
                }
                else if (value.ToString().Equals("판정완료"))
                {
                    appearance.BackColor = Color.LightGray;
                }
                else
                {
                    appearance.BackColor = Color.White;
                }
            }
            

            base.PaintCell(g, r, appearance, value, isSelected, isLocked, zoomFactor);
        }
    }
}
