using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Drawing;

namespace HC_Main
{
    public class clsHcMainSpd
    {
        /// <summary>
        /// 검진접수 Spread Default Skin Set
        /// </summary>
        /// <param name="spd"></param>
        void setDefaultSkin(FpSpread spd, bool showColHead, bool showRowHead)
        {
            Color cBack = new Color();
            cBack = Color.FromArgb(227, 241, 255);

            SheetSkin skin = new SheetSkin("HcViewSkin", Color.White, cBack, Color.Black, Color.Gray, 
                GridLines.Both, Color.FromArgb(166, 186, 214), 
                Color.Black, cBack, cBack, cBack, cBack,
                true, true, false, showColHead, showRowHead);

            skin.Apply(spd);
        }

        #region 검진접수 GROUPCODE LIST 영역
        public enum enmHcGroup      { GBDEL,  GJJONG,     CODE,       NAME,         AMT,    BURATE,   HALIN,      CHANGE, ROWID   }
        public string[] sHcGroup =  { "제외", "검진종류", "묶음코드", "묶음코드명", "금액", "부담율", "할인구분", "변경", "ROWID" };
        public int[] nHcGroup =     { 28,     60,         40,         162,          70,     32,       30,         30,     30      };

        public void sSpd_enmHcGroup(FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            try
            {
                setDefaultSkin(spd, true, false);

                clsSpread cSpd = new clsSpread();
                
                //스프레드 사이즈
                spd.ActiveSheet.RowCount = RowCnt;
                spd.ActiveSheet.ColumnCount = ColCnt;

                if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmHcGroup)).Length; }

                spd.ActiveSheet.ColumnHeader.Rows[0].Height = 36;

                spd.VerticalScrollBarWidth = 13;
                //spd.HorizontalScrollBarHeight = 16;

                //1.헤더 및 사이즈
                cSpd.setHeader(spd, colName, size, 9);
                spd.ActiveSheet.Columns.Get(-1).Visible = true;
                
                //2.컬럼 스타일
                cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
                cSpd.setColStyle(spd, -1, (int)enmHcGroup.GBDEL, clsSpread.enmSpdType.CheckBox);
                cSpd.setColStyle(spd, -1, (int)enmHcGroup.HALIN, clsSpread.enmSpdType.CheckBox);
                cSpd.setColStyle(spd, -1, (int)enmHcGroup.BURATE, clsSpread.enmSpdType.Text, null, null, null, null, false);

                cSpd.setColLength(spd, -1, (int)enmHcGroup.BURATE, 2);

                //3.정렬
                cSpd.setColAlign(spd, -1, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcGroup.AMT, clsSpread.HAlign_R, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcGroup.GBDEL, clsSpread.HAlign_C, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcGroup.HALIN, clsSpread.HAlign_C, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcGroup.BURATE, clsSpread.HAlign_C, clsSpread.VAlign_C);

                //4.히든
                cSpd.setColStyle(spd, -1, (int)enmHcGroup.ROWID, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmHcGroup.CHANGE, clsSpread.enmSpdType.Hide);

                //5.Filter
                //cSpd.setSpdFilter(spd, (int)enmHcGroup.CODE, AutoFilterMode.EnhancedContextMenu, true);
                //cSpd.setSpdFilter(spd, (int)enmHcGroup.NAME, AutoFilterMode.EnhancedContextMenu, true);

                // 6. 특정문구 색상 
                UnaryComparisonConditionalFormattingRule unary;
                
                //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
                //unary.BackColor = Color.White;
                //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcGroup.BURATE, unary);

                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
                unary.BackColor = Color.FromArgb(166, 186, 214);
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcGroup.GBDEL, unary);

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        #endregion

        #region 검진접수 검사선택 창 영역
        public enum enmHcSExam      { GBSEL,  CODE,   NAME,     GBSELF, GRPCD, HANG }
        public string[] sHcSExam =  { "선택", "코드", "코드명", "S",    "그룹CD" , "HANG" };
        public int[] nHcSExam =     { 40,     64,     320,      40,     40 , 40 };

        public void sSpd_enmHcSExam(FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            try
            {
                //setDefaultSkin(spd, true, true);

                clsSpread cSpd = new clsSpread();

                //스프레드 사이즈
                spd.ActiveSheet.RowCount = RowCnt;
                spd.ActiveSheet.ColumnCount = ColCnt;

                if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmHcSExam)).Length; }

                spd.ActiveSheet.ColumnHeader.Rows[0].Height = 36;

                spd.VerticalScrollBarWidth = 13;
                //spd.HorizontalScrollBarHeight = 16;

                //1.헤더 및 사이즈
                cSpd.setHeader(spd, colName, size, 9);
                spd.ActiveSheet.Columns.Get(-1).Visible = true;

                //2.컬럼 스타일
                cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
                cSpd.setColStyle(spd, -1, (int)enmHcSExam.GBSEL, clsSpread.enmSpdType.CheckBox);                

                //3.정렬
                cSpd.setColAlign(spd, -1, clsSpread.HAlign_L, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmHcSExam.GBSEL, clsSpread.HAlign_C, clsSpread.VAlign_C);

                //4.히든
                cSpd.setColStyle(spd, -1, (int)enmHcSExam.GBSELF, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmHcSExam.GRPCD, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmHcSExam.HANG, clsSpread.enmSpdType.Hide);

                //5.Filter
                cSpd.setSpdFilter(spd, (int)enmHcSExam.CODE, AutoFilterMode.EnhancedContextMenu, true);
                cSpd.setSpdFilter(spd, (int)enmHcSExam.NAME, AutoFilterMode.EnhancedContextMenu, true);

                // 6. 특정문구 색상 
                //UnaryComparisonConditionalFormattingRule unary;

                //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
                //unary.BackColor = Color.White;
                //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcSExam.BURATE, unary);

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        #endregion
    }
}
