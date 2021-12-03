using System;
using System.Drawing;
using ComBase;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;

namespace ComPmpaLibB
{
    public class clsComPmpaSpd
    {
        clsSpread methodSpd = new clsSpread();
        clsPrint cp = new clsPrint();

        #region frmMisu.cs 관리 영역        
        /// <summary> Misu 관리 메인 스프레드 표시 </summary>
        public void sSpd_enmPmpaMisu(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            clsSpread cSpd = new clsSpread();

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(clsPmpaPb.enmPmpaMisu)).Length;
            
            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            cSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaMisu.chk01, clsSpread.enmSpdType.CheckBox);
            
            //3.정렬
            cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmPmpaMisu.chk01, clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmPmpaMisu.BalAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmPmpaMisu.IpGumAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmPmpaMisu.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmPmpaMisu.TotAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);

            //4.히든
            // cSpd.setColStyle(spd, -1, (int)enmSupFnExMain.GbIO, clsSpread.enmSpdType.Hide);

            // 5. sort, filter
            //cSpd.setSpdFilter(spd, (int)enmPmpaMisu.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            
            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "동", false);
            //unary.BackColor = Color.FromArgb(255, 60, 60); ;
            //unary.ForeColor = Color.FromArgb(255, 250, 250);
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmPmpaMisu.JobName , unary); 
            
        }
        #endregion

        #region frmPmpaBasAcctAdd.cs  관리영역

        string[] gArrCombo_Child = {
             "0.성인"
            ,"1.신생아"
            ,"2.만1세미만"
            ,"3.만1세이상-만6세미만"
            ,"4.만6세미만"
            ,"5.만70세이상"
            ,"6.만35세이상(분만)"
            ,"7.신생아0세"
            };

        /// <summary> Bas_Add 관리 메인 스프레드 표시 </summary>
        public void sSpd_enmPmpaADD(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt, bool bDel, string strGbn)
        {
            clsSpread cSpd = new clsSpread();
            
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;

            if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(clsPmpaPb.enmPmpaAdd)).Length; }

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

            spd.VerticalScrollBarWidth = 16;
            spd.HorizontalScrollBarHeight = 16;

            //1.헤더 및 사이즈
            cSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Text);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.chk01,   clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.PCODE,   clsSpread.enmSpdType.Text, null, null, null, null, false);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.GBCHILD, clsSpread.enmSpdType.ComboBox, this.gArrCombo_Child);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.NIGHT,   clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.MIDNIGHT, clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.GBER,    clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.HOLIDAY, clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD1,    clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD2,    clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD3,    clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD4,    clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD5,    clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD6,    clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD7,    clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD8,    clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD9,    clsSpread.enmSpdType.CheckBox);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.SDATE,   clsSpread.enmSpdType.Date);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.EDATE,   clsSpread.enmSpdType.Date);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.DELDATE, clsSpread.enmSpdType.Date);

            cSpd.setColLength(spd, -1, (int)clsPmpaPb.enmPmpaAdd.PCODE,   3);

            //3.정렬
            cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmPmpaAdd.GBCHILD,  clsSpread.HAlign_L, clsSpread.VAlign_C);
    

            //4.히든
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ENTDATE,  clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ENTSABUN, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.Change,   clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ROWID,    clsSpread.enmSpdType.Hide);
            if (bDel == false)
            {
                cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.DELDATE, clsSpread.enmSpdType.Hide);
            }

            //4.조건별 히든
            #region 조건부 히든
            switch (strGbn)
            {
                case "01":  //진찰료
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.GBER,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.MIDNIGHT, clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD1,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD2,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD3,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD4,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD5,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD6,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD7,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD8, clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD9, clsSpread.enmSpdType.Hide);
                    break;
                case "02":  //약제조제료
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.NIGHT,  clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.MIDNIGHT, clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.GBER,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.HOLIDAY, clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD2,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD3,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD4,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD5,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD6,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD7,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD8, clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD9, clsSpread.enmSpdType.Hide);
                    break;
                case "03":  //주사수기료
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.NIGHT,  clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.MIDNIGHT, clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.GBER,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.HOLIDAY, clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD1,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD2,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD3,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD4,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD5,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD6,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD7,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD8, clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD9, clsSpread.enmSpdType.Hide);
                    break;
                case "04":  //마취료
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.MIDNIGHT, clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD4,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD5,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD6,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD7,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD8, clsSpread.enmSpdType.Hide);
                  
                    break;
                case "05":  //처치수술료
                    //cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD6, clsSpread.enmSpdType.Hide);
                    break;
                case "06":  //검사료
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.NIGHT,  clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.MIDNIGHT, clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.HOLIDAY, clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD6,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD7,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD8, clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD9, clsSpread.enmSpdType.Hide);
                    break;
                case "07":  //영상진단료
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.NIGHT,  clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.MIDNIGHT, clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.HOLIDAY, clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD2,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD3,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD4,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD5,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD6,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD7,   clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD8, clsSpread.enmSpdType.Hide);
                    cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmPmpaAdd.ADD9, clsSpread.enmSpdType.Hide);
                    break;
                default:
                    break;
            }
            #endregion

            // 5. sort, filter
            cSpd.setSpdFilter(spd, (int)clsPmpaPb.enmPmpaAdd.EDATE, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)clsPmpaPb.enmPmpaAdd.SDATE, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.ForeColor = Color.FromArgb(255, 60, 60);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmPmpaAdd.DELDATE, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.Wheat;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmPmpaAdd.PCODE, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "True", false);
            unary.BackColor = Color.PeachPuff;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmPmpaAdd.GBCHILD, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "True", false);
            unary.BackColor = Color.PeachPuff;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmPmpaAdd.GBER, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "True", false);
            unary.BackColor = Color.PeachPuff;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmPmpaAdd.HOLIDAY, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "True", false);
            unary.BackColor = Color.PeachPuff;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmPmpaAdd.NIGHT, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "True", false);
            unary.BackColor = Color.PeachPuff;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmPmpaAdd.MIDNIGHT, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "True", false);
            unary.BackColor = Color.PeachPuff;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmPmpaAdd.ADD1, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "True", false);
            unary.BackColor = Color.PeachPuff;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmPmpaAdd.ADD2, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "True", false);
            unary.BackColor = Color.PeachPuff;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmPmpaAdd.ADD3, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "True", false);
            unary.BackColor = Color.PeachPuff;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmPmpaAdd.ADD4, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "True", false);
            unary.BackColor = Color.PeachPuff;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmPmpaAdd.ADD5, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "True", false);
            unary.BackColor = Color.PeachPuff;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmPmpaAdd.ADD6, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "True", false);
            unary.BackColor = Color.PeachPuff;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmPmpaAdd.ADD7, unary);

        }

        #endregion

        #region frmPmpaBasAccountBon.cs  관리영역   

        public string[] gArrCombo_Child2 = {
             "0.성인"
            ,"1.신생아"
            ,"2.6세미만"
            ,"3.6세~15세"
            ,"4.65세이상"
            };

        public void sSpd_enmBasAcctBon(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt, string[] gArrCboHC, string[] gArrCboF)
        {
            clsSpread cSpd = new clsSpread();

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;

            if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(clsPmpaPb.enmBasAcctBon)).Length; }

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

            spd.VerticalScrollBarWidth = 16;
            spd.HorizontalScrollBarHeight = 16;

            //1.헤더 및 사이즈
            cSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmBasAcctBon.chk01, clsSpread.enmSpdType.CheckBox);
            //cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmBasAcctBon.GBCHILD, clsSpread.enmSpdType.ComboBox, this.gArrCombo_Child2);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmBasAcctBon.HC, clsSpread.enmSpdType.ComboBox, gArrCboHC);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmBasAcctBon.FCODE, clsSpread.enmSpdType.ComboBox, gArrCboF);

            //Multi Line => false
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmBasAcctBon.FAMT1, clsSpread.enmSpdType.Text, null, null, null, null, false);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmBasAcctBon.FAMT2, clsSpread.enmSpdType.Text, null, null, null, null, false);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmBasAcctBon.MCODE, clsSpread.enmSpdType.Text, null, null, null, null, false);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmBasAcctBon.VCODE, clsSpread.enmSpdType.Text, null, null, null, null, false);

            cSpd.setColLength(spd, -1, (int)clsPmpaPb.enmBasAcctBon.DEPT, 2);
            cSpd.setColLength(spd, -1, (int)clsPmpaPb.enmBasAcctBon.GBCHILD, 1);
            cSpd.setColLength(spd, -1, (int)clsPmpaPb.enmBasAcctBon.MCODE, 4);
            cSpd.setColLength(spd, -1, (int)clsPmpaPb.enmBasAcctBon.VCODE, 4);
            cSpd.setColLength(spd, -1, (int)clsPmpaPb.enmBasAcctBon.JIN, 3);
            cSpd.setColLength(spd, -1, (int)clsPmpaPb.enmBasAcctBon.BOHUM, 3);
            cSpd.setColLength(spd, -1, (int)clsPmpaPb.enmBasAcctBon.FOOD, 3);
            cSpd.setColLength(spd, -1, (int)clsPmpaPb.enmBasAcctBon.CTMRI, 3);
            cSpd.setColLength(spd, -1, (int)clsPmpaPb.enmBasAcctBon.DT1, 3);
            cSpd.setColLength(spd, -1, (int)clsPmpaPb.enmBasAcctBon.DT2, 3);
           
            //3.정렬
            cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);

            cSpd.setColAlign(spd, (int)clsPmpaPb.enmBasAcctBon.CHILDNAME, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmBasAcctBon.MCODE_NAME, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmBasAcctBon.VCODE_NAME, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmBasAcctBon.HC, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmBasAcctBon.FCODE, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmBasAcctBon.JIN, clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmBasAcctBon.BOHUM, clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmBasAcctBon.CTMRI, clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmBasAcctBon.FOOD, clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmBasAcctBon.DT1, clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmBasAcctBon.DT2, clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmBasAcctBon.FAMT1, clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmBasAcctBon.FAMT2, clsSpread.HAlign_R, clsSpread.VAlign_C);

            //4.히든
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmBasAcctBon.GBIO, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmBasAcctBon.ROWID, clsSpread.enmSpdType.Hide);

            // 5. sort, filter
            cSpd.setSpdFilter(spd, (int)clsPmpaPb.enmBasAcctBon.GBIO, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)clsPmpaPb.enmBasAcctBon.BI, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)clsPmpaPb.enmBasAcctBon.SDATE, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)clsPmpaPb.enmBasAcctBon.EDATE, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)clsPmpaPb.enmBasAcctBon.MCODE, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)clsPmpaPb.enmBasAcctBon.GBCHILD, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)clsPmpaPb.enmBasAcctBon.VCODE, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)clsPmpaPb.enmBasAcctBon.HC, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            cSpd.setSpdFilter(spd, (int)clsPmpaPb.enmBasAcctBon.FCODE, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, " ", false);
            unary.ForeColor = Color.FromArgb(238, 206, 206);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmBasAcctBon.chk01, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(226, 238, 210);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmBasAcctBon.JIN, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(226, 238, 210);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmBasAcctBon.BOHUM, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(226, 238, 210);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmBasAcctBon.CTMRI, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(226, 238, 210);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmBasAcctBon.FOOD, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(226, 238, 210);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmBasAcctBon.DT1, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(226, 238, 210);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(226, 238, 210);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmBasAcctBon.DT2, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(226, 238, 210);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(226, 238, 210);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmBasAcctBon.FAMT1, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(226, 238, 210);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmBasAcctBon.FAMT2, unary);

        }
        #endregion

        #region frmPmpaViewExitReceiptList.cs 관리영역
        
        public void sSpd_enmRcptTrsList(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt, string[] gArrCboHC)
        {
            clsSpread cSpd = new clsSpread();

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;

            if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(clsPmpaPb.enmRcptTrsList)).Length; }

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

            //4.히든
            #region 히든
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmRcptTrsList.IPDNO, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmRcptTrsList.TRSNO, clsSpread.enmSpdType.Hide);
            //cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmRcptTrsList.DRGCODE, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmRcptTrsList.TEMP, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmRcptTrsList.ROWID, clsSpread.enmSpdType.Hide);
            #endregion

            // 5. sort, filter
            //cSpd.setSpdFilter(spd, (int)enmJSimScreen.SUCODE, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            //6.특정문구 색상
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "재원", false);
            //unary.BackColor = Color.FromArgb(192, 255, 192);
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmRcptTrsList.GBSTS, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(196, 227, 191);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmRcptTrsList.SECRET, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "D", false);
            unary.BackColor = Color.FromArgb(196, 227, 191);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmRcptTrsList.DRG, unary);

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(250, 0, 0);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmRcptTrsList.FCODE, unary);

        }
        #endregion

        #region frmPmpaEntryCardDaou.cs 관리영역
        public void sSpd_enmCardApprov(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            clsSpread cSpd = new clsSpread();

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;

            if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(clsPmpaPb.enmCardApprov)).Length; }

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
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmCardApprov.TRADEAMT, clsSpread.HAlign_R, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmCardApprov.FINAME, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든
            #region 히든
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmCardApprov.ROWID, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmCardApprov.TRANHEADER, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmCardApprov.ORIGINNO2, clsSpread.enmSpdType.Hide);
            //cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmCardApprov.PTGUBUN, clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmCardApprov.DIV, clsSpread.enmSpdType.Hide);
            #endregion

            // 5. sort, filter
            //cSpd.setSpdFilter(spd, (int)enmJSimScreen.SUCODE, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            //6.특정문구 색상
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(196, 227, 191);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmCardApprov.HPAY, unary);
            
        }
        #endregion

        #region frmPmpaTransChange.cs 관리영역
        public void sSpd_enmIpdTrsChg(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            clsSpread cSpd = new clsSpread();

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;

            if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(clsPmpaPb.enmIpdTrsChg)).Length; }

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;

            spd.VerticalScrollBarWidth = 16;
            spd.HorizontalScrollBarHeight = 16;

            //1.헤더 및 사이즈
            cSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;
            spd.ActiveSheet.RowHeader.Visible = false;

            //2.컬럼 스타일
            cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmIpdTrsChg.chk01, clsSpread.enmSpdType.CheckBox);

            cSpd.setColLength(spd, -1, (int)clsPmpaPb.enmIpdTrsChg.GKIHO, 18);

            //3.정렬
            cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmIpdTrsChg.KIHO, clsSpread.HAlign_L, clsSpread.VAlign_C);
            cSpd.setColAlign(spd, (int)clsPmpaPb.enmIpdTrsChg.GKIHO, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든
            #region 히든
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmIpdTrsChg.TRSNO,    clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmIpdTrsChg.OUTDATE,  clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmIpdTrsChg.GBSTS,    clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmIpdTrsChg.GELCODE,  clsSpread.enmSpdType.Hide);
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmIpdTrsChg.DRCODE,   clsSpread.enmSpdType.Hide);
            #endregion

            // 5. sort, filter
            cSpd.setSpdFilter(spd, (int)clsPmpaPb.enmIpdTrsChg.INDATE, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            //6.특정문구 색상
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "Y", false);
            unary.BackColor = Color.FromArgb(255, 255, 196);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmIpdTrsChg.GBILBAN2, unary);

        }
        #endregion

        #region frmPmpaViewWardOutList.cs 관리영역
        public void sSpd_enmROutList(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            clsSpread cSpd = new clsSpread();

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;

            if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(clsPmpaPb.enmROutList)).Length; }

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
            
            //4.히든
            cSpd.setColStyle(spd, -1, (int)clsPmpaPb.enmROutList.Last, clsSpread.enmSpdType.Hide);

            //5.sort, filter
            cSpd.setSpdSort(spd, (int)clsPmpaPb.enmROutList.ROUTENTTIME, true);
            //cSpd.setSpdFilter(spd, (int)clsPmpaPb.enmROutList.ROUTENTTIME, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
           
            // 6. 특정문구 색상 

            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "Y", false);
            unary.BackColor = Color.FromArgb(255, 192, 192);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsPmpaPb.enmROutList.Last, unary);
            
        }
        #endregion

        #region frmPmpaDailyCheck.cs 관리영역-김해수
        public enum enmDailyCheck_sel { Pano, SName, OUTDATE, bigo }

        public string[] senmDailyCheck_sel = { "등록번호", "성명", "입원일자", "비  고" };

        public int[] nenmDailyCheck_sel = { 70, 50, 70, 300 };

        public void sSpd_enmDailyCheck_sel(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            ////스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmDailyCheck_sel)).Length;

            ////spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            ////spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)enmDailyCheck_bunnu.nToiAmt + 1;//칼럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 20;
            spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 40;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, 6, clsSpread.enmSpdType.Text,null,null,null,null,false);
            //methodSpd.setColStyle(spd, -1, (int)enmDailyCheck_bunnu.Remark, clsSpread.enmSpdType.Text, null, null, null, null, false);
            //sup.setColStyle_Text(spd, -1, (int)enmIpdMirCheckUpdate.MirAmt, false, false, false, 1); //txt재정의
            //sup.setColStyle_Text(spd, -1, (int)enmDailyCheck_bunnu.Remark, false, false, false, 100);
            //methodSpd.setColStyle(spd, -1, (int)enmSupXrayRCP01Copy2.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmDailyCheck_sel.Pano, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmDailyCheck_sel.SName, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmDailyCheck_sel.OUTDATE, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmDailyCheck_sel.bigo, clsSpread.HAlign_L, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmDailyCheck_bunnu.Bi, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmDailyCheck_bunnu.nToiAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmDailyCheck_bunnu.MirAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmDailyCheck_bunnu.ChaAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmDailyCheck_bunnu.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든         
            //methodSpd.setColStyle(spd, -1, (int)enmDailyCheck_bunnu.ROWID, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmDailyCheck_bunnu.OldMirAmt, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmDailyCheck_bunnu.OldReMark, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmDailyCheck_bunnu.YYMM, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmDailyCheck_bunnu.InDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMagammisubs35Spd.Chang, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMagammisubs35Spd.RoomCode, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMagammisubs35Spd.ROWID, clsSpread.enmSpdType.Hide);

            //6.특정문구 색상
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(192, 255, 192);
            // spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmDailyCheck_bunnu.MirAmt, unary); //결과

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(192, 255, 192);
            // spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmDailyCheck_bunnu.Remark, unary);

        }
        #endregion

        #region PmpaMagamIpdMirCheckUpdate2.cs 관리영역-김해수
        public enum enmIpdMirCheckUpdate
        {
            Pano, SName, Bi, nToiAmt, BDate
            , MirAmt, ChaAmt, Remark, ROWID, OldMirAmt
            , OldReMark, YYMM, InDate
        }

        public string[] senmIpdMirCheckUpdate = { "등록번호","성명","종류","조합미수액"," "
                                                    ,"청구액","청구차액","참고사항","ROWID","OLD청구금액"
                                                    ,"OLD참고사항","YYMM","M"};



        public int[] nenmIpdMirCheckUpdate = { 75,55,20,100,5
                                                 ,100,110,500,60,60
                                                 ,60,60,60};

        public void sSpd_enmIpdMirCheckUpdate(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            ////스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmIpdMirCheckUpdate)).Length;

            ////spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            ////spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.FrozenColumnCount = (int)enmIpdMirCheckUpdate.nToiAmt + 1;//칼럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
            spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 45;
            //스크롤바 사이즈 
            spd.VerticalScrollBarWidth = 15;
            spd.HorizontalScrollBarHeight = 15;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;
            

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, 6, clsSpread.enmSpdType.Text,null,null,null,null,false);
            methodSpd.setColStyle(spd, -1, (int)enmIpdMirCheckUpdate.Remark, clsSpread.enmSpdType.Text, null, null, null, null, false);
            //sup.setColStyle_Text(spd, -1, (int)enmIpdMirCheckUpdate.MirAmt, false, false, false, 1); //txt재정의
            //sup.setColStyle_Text(spd, -1, (int)enmIpdMirCheckUpdate.Remark, false, false, false, 100);//최대길이 100까지 순환참조로 사용불가능
            //methodSpd.setColStyle(spd, -1, (int)enmSupXrayRCP01Copy2.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmIpdMirCheckUpdate.Pano, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmIpdMirCheckUpdate.SName, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmIpdMirCheckUpdate.Bi, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmIpdMirCheckUpdate.nToiAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmIpdMirCheckUpdate.MirAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmIpdMirCheckUpdate.ChaAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmIpdMirCheckUpdate.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든         
            methodSpd.setColStyle(spd, -1, (int)enmIpdMirCheckUpdate.ROWID, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmIpdMirCheckUpdate.OldMirAmt, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmIpdMirCheckUpdate.OldReMark, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmIpdMirCheckUpdate.YYMM, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmIpdMirCheckUpdate.InDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMagammisubs35Spd.Chang, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMagammisubs35Spd.RoomCode, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMagammisubs35Spd.ROWID, clsSpread.enmSpdType.Hide);

            //6.특정문구 색상
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(192, 255, 192);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmIpdMirCheckUpdate.MirAmt, unary); //결과

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(192, 255, 192);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmIpdMirCheckUpdate.Remark, unary);

        }
        #endregion

        #region frmPmpaViewSmokeMisuInput.cs 관리영역-김해수
        public enum PmpaViewSmokeMisuInput
        {
            Chk, Pano, SName, BDate, GUBUN1,
            GUBUN2, OPDIPD, DEPTCODE, MisuDTL22, AMT,
            MisuDTL, GUBUN_1, GUBUN_2, PoBun,ROWID, Remark
        }

        public string[] senmPmpaViewSmokeMisuInput = { "A", "등록번호", "성명", "발생일자", "구분1"
                                                    ,"구분2", "IO", "과", "미수상세", "발생금액"
                                                    ,"MISUDTL", "구분1", "구분2", "PoBun", "ROWID","적요"};



        public int[] nenmPmpaViewSmokeMisuInput = { 25,90,60,80,60
                                                 ,60,30,50,40,70
                                                 ,50,20,20,20,50,70};

        public void sSpd_PmpaViewSmokeMisuInput(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            ////스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmIpdMirCheckUpdate)).Length;

            ////spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            ////spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)PmpaViewSmokeMisuInput.nToiAmt + 1;//칼럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 40;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, 0, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, 6, clsSpread.enmSpdType.Text,null,null,null,null,false);
            //methodSpd.setColStyle(spd, -1, (int)enmIpdMirCheckUpdate.Remark, clsSpread.enmSpdType.Text, null, null, null, null, false);
            //sup.setColStyle_Text(spd, -1, (int)enmIpdMirCheckUpdate.MirAmt, false, false, false, 1); //txt재정의
            //sup.setColStyle_Text(spd, -1, (int)enmIpdMirCheckUpdate.Remark, false, false, false, 100);//최대길이 100까지 순환참조로 사용불가능
            //methodSpd.setColStyle(spd, -1, (int)enmSupXrayRCP01Copy2.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)PmpaViewSmokeMisuInput.Chk, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)PmpaViewSmokeMisuInput.Pano, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)PmpaViewSmokeMisuInput.SName, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)PmpaViewSmokeMisuInput.BDate, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)PmpaViewSmokeMisuInput.GUBUN1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)PmpaViewSmokeMisuInput.GUBUN2, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)PmpaViewSmokeMisuInput.OPDIPD, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)PmpaViewSmokeMisuInput.DEPTCODE, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)PmpaViewSmokeMisuInput.MisuDTL22, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)PmpaViewSmokeMisuInput.AMT, clsSpread.HAlign_R, clsSpread.VAlign_C);


            //4.히든          
            methodSpd.setColStyle(spd, -1, (int)PmpaViewSmokeMisuInput.MisuDTL, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)PmpaViewSmokeMisuInput.GUBUN_1, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)PmpaViewSmokeMisuInput.GUBUN_2, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)PmpaViewSmokeMisuInput.PoBun, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)PmpaViewSmokeMisuInput.ROWID, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMagammisubs35Spd.Chang, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMagammisubs35Spd.RoomCode, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmMagammisubs35Spd.ROWID, clsSpread.enmSpdType.Hide);

            //6.특정문구 색상
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            //unary.BackColor = Color.FromArgb(192, 255, 192);
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmIpdMirCheckUpdate.MirAmt, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            //unary.BackColor = Color.FromArgb(192, 255, 192);
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmIpdMirCheckUpdate.Remark, unary);

        }
        #endregion

        #region frmPmpaMisuMast1_2.cs 관리영역-김해수
        public enum enmPmpaMisuMast1_2
        {
            A, Bdate, Gubun, GubunName, Qty,
            TAmt, Amt, Remark, Chasu, ROWID,
            OldBdate, OldGubun, OldQty, OldTAmt, OldAmt,
            OldRemark, OldChasu, EntDate, EntPart
        }

        public string[] senmPmpaMisuMast1_2 = { "A", "입력일자", "구분", "구분명", "건   수"
                                                    ,"총진료비", "금   액", "적      요", "심사차수", "ROWID"
                                                    ,"변경전 발생일자", "변경전 구분", "변경전 건수", "변경전 총 진료비", "변경전 금액"
                                                    ,"변경전 적요","변경전 심사차수","Old EntDate","Old EntPart"};



        public int[] nenmPmpaMisuMast1_2 = { 20,100,80,70,60
                                             ,120,120,330,100,60
                                             ,50,50,50,50,50
                                             ,50,50,50,50};

        public void sSpd_PmpaMisuMast1_2(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            ////스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmIpdMirCheckUpdate)).Length;

            ////spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            ////spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)enmIpdMirCheckUpdate.nToiAmt + 1;//칼럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 27;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 40;

            spd.VerticalScrollBarWidth = 13;
            spd.HorizontalScrollBarHeight = 13;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.A, clsSpread.enmSpdType.CheckBox,null,null,null,null,false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.GubunName, clsSpread.enmSpdType.Label, null, null, null, null, false);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.Bdate, clsSpread.enmSpdType.Text, null, null, null, null, false);
            setColStyle_Date(spd, -1, (int)enmPmpaMisuMast1_2.Bdate, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.Gubun, clsSpread.enmSpdType.Text, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.Qty, clsSpread.enmSpdType.Text, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.TAmt, clsSpread.enmSpdType.Text, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.Amt, clsSpread.enmSpdType.Text, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.Remark, clsSpread.enmSpdType.Text, null, null, null, null, true);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.Chasu, clsSpread.enmSpdType.Text, null, null, null, null, true);
            //methodSpd.setColStyle(spd, -1, 6, clsSpread.enmSpdType.Text,null,null,null,null,false);
            //methodSpd.setColStyle(spd, -1, (int)enmIpdMirCheckUpdate.Remark, clsSpread.enmSpdType.Text, null, null, null, null, false);
            //sup.setColStyle_Text(spd, -1, (int)enmIpdMirCheckUpdate.MirAmt, false, false, false, 1); //txt재정의
            //sup.setColStyle_Text(spd, -1, (int)enmIpdMirCheckUpdate.Remark, false, false, false, 100);//최대길이 100까지 순환참조로 사용불가능
            //methodSpd.setColStyle(spd, -1, (int)enmSupXrayRCP01Copy2.Infect, clsSpread.enmSpdType.IMAGE);

            TextCellType spdObj = new TextCellType();
            spdObj.Multiline = true;
            spdObj.WordWrap = true;
            spdObj.MaxLength = 9999;

            spd.ActiveSheet.Columns.Get((int)enmPmpaMisuMast1_2.Remark).CellType = spdObj;


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_2.Bdate, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_2.Gubun, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_2.GubunName, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_2.Qty, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_2.TAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_2.Amt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_2.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_2.Chasu, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColLength(spd, -1, (int)enmPmpaMisuMast1_2.Remark, 39);

            //4.히든          
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.ROWID, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldBdate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldGubun, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldQty, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldTAmt, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldAmt, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldRemark, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldChasu, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.EntDate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.EntPart, clsSpread.enmSpdType.Hide);

            //6.특정문구 색상
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(250, 244, 192);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmPmpaMisuMast1_2.GubunName, unary); //결과

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(255, 217, 236);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmPmpaMisuMast1_2.Chasu, unary);

        }
        #endregion

        #region frmPmpaMisuMast1_3.cs 관리영역-김해수
        public enum enmPmpaMisuMast1_3
        {
            YYMM, IwolAmt, MisuAmt, IpgumAmt, SakAmt,
            SakAmt2, BanAmt, EtcAmt, JanAmt
        }

        public string[] senmPmpaMisuMast1_3 = {
                                                    "월별", "이월금액", "당월미수", "당월입금", "당월삭감"
                                                    ,"당월절사삭감", "당월반송", "당월기타", "월말잔액"
                                              };



        public int[] nenmPmpaMisuMast1_3 = {
                                                70,115,115,115,115
                                                ,115,115,115,115         
                                           };

        public void sSpd_PmpaMisuMast1_3(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            ////스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmIpdMirCheckUpdate)).Length;

            ////spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            ////spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)enmIpdMirCheckUpdate.nToiAmt + 1;//칼럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 27;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 40;

            spd.VerticalScrollBarWidth = 13;
            spd.HorizontalScrollBarHeight = 13;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, 0, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, 6, clsSpread.enmSpdType.Text,null,null,null,null,false);
            //methodSpd.setColStyle(spd, -1, (int)enmIpdMirCheckUpdate.Remark, clsSpread.enmSpdType.Text, null, null, null, null, false);
            //sup.setColStyle_Text(spd, -1, (int)enmIpdMirCheckUpdate.MirAmt, false, false, false, 1); //txt재정의
            //sup.setColStyle_Text(spd, -1, (int)enmIpdMirCheckUpdate.Remark, false, false, false, 100);//최대길이 100까지 순환참조로 사용불가능
            //methodSpd.setColStyle(spd, -1, (int)enmSupXrayRCP01Copy2.Infect, clsSpread.enmSpdType.IMAGE);

            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_3.YYMM, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_3.IwolAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_3.MisuAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_3.IpgumAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_3.SakAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_3.SakAmt2, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_3.BanAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_3.EtcAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast1_3.JanAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);


            //4.히든          
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.ROWID, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldBdate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldGubun, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldQty, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldTAmt, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldAmt, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldRemark, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldChasu, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.EntDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.EntPart, clsSpread.enmSpdType.Hide);

            //6.특정문구 색상
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            //unary.BackColor = Color.FromArgb(250, 244, 192);
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmPmpaMisuMast1_2.GubunName, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            //unary.BackColor = Color.FromArgb(255, 217, 236);
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmPmpaMisuMast1_2.Chasu, unary);

        }
        #endregion

        #region frmPmpaMisuMast2.cs 관리영역-김해수
        public enum enmPmpaMisuMast2
        {
            A, Bdate, Gubun, GubunName, Qty 
            , Amt, Remark, ROWID, OldBdate, OldGubun
            , OldQty, OldAmt, OldRemark, EntDate, EntPart
        }

        public string[] senmPmpaMisuMast2 = {
                                                    "A", "입력일자", "구분", "구분명", "건    수"
                                                    ,"금      액", "적    요", "ROWID", "Old Bdate", "Old Gubun"
                                                    , "Old Qty", "Old Amt", "Old Remark", "EntDate", "EntPart"
                                              };



        public int[] nenmPmpaMisuMast2 = {
                                                20,100,50,90,60
                                                ,110,390,10,10,10
                                                ,10,10,10,10,10
                                           };

        public void sSpd_PmpaMisuMast2(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            ////스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmIpdMirCheckUpdate)).Length;

            ////spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            ////spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)enmIpdMirCheckUpdate.nToiAmt + 1;//칼럼고정

            //칼럼명 사이즈
            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 40;

            spd.VerticalScrollBarWidth = 13;
            spd.HorizontalScrollBarHeight = 13;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2.A, clsSpread.enmSpdType.CheckBox, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2.GubunName, clsSpread.enmSpdType.Label, null, null, null, null, false);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2.Bdate, clsSpread.enmSpdType.Date, null, null, null, null, false);
            setColStyle_Date(spd, -1, (int)enmPmpaMisuMast2.Bdate, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2.Gubun, clsSpread.enmSpdType.Text, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2.Qty, clsSpread.enmSpdType.Text, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2.Amt, clsSpread.enmSpdType.Text, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2.Remark, clsSpread.enmSpdType.Text, null, null, null, null, true);

            //methodSpd.setColStyle(spd, -1, 6, clsSpread.enmSpdType.Text,null,null,null,null,false);
            //methodSpd.setColStyle(spd, -1, (int)enmIpdMirCheckUpdate.Remark, clsSpread.enmSpdType.Text, null, null, null, null, false);
            //sup.setColStyle_Text(spd, -1, (int)enmIpdMirCheckUpdate.MirAmt, false, false, false, 1); //txt재정의
            //sup.setColStyle_Text(spd, -1, (int)enmIpdMirCheckUpdate.Remark, false, false, false, 100);//최대길이 100까지 순환참조로 사용불가능
            //methodSpd.setColStyle(spd, -1, (int)enmSupXrayRCP01Copy2.Infect, clsSpread.enmSpdType.IMAGE);

            TextCellType spdObj = new TextCellType();
            spdObj.Multiline = true;
            spdObj.WordWrap = true;

            spd.ActiveSheet.Columns.Get((int)enmPmpaMisuMast2.Remark).CellType = spdObj;

            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast2.A, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast2.Bdate, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast2.Gubun, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast2.GubunName, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast2.Qty, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast2.Amt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast2.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);
            //methodSpd.setColLength(spd, -1, (int)enmPmpaMisuMast2.Remark, 39);

            //4.히든          
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2.ROWID, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2.OldBdate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2.OldGubun, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2.OldQty, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2.OldAmt, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2.OldRemark, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2.EntDate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2.EntPart, clsSpread.enmSpdType.Hide);

            //6.특정문구 색상
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(250, 244, 192);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmPmpaMisuMast2.GubunName, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            //unary.BackColor = Color.FromArgb(255, 217, 236);
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmPmpaMisuMast1_2.Chasu, unary);

        }
        #endregion

        #region frmPmpaMisuMast2TA.cs 관리영역-김해수
        public enum enmPmpaMisuMast2TA
        {
            A, Bdate, Gubun, GubunName, Qty
            , Amt, Remark, ROWID, OldBdate, OldGubun
            , OldQty, OldAmt, OldRemark, EntDate, EntPart
        }

        public string[] senmPmpaMisuMast2TA = {
                                                    "A", "입력일자", "구분", "구분명", "건    수"
                                                    ,"금      액", "적    요", "ROWID", "Old Bdate", "Old Gubun"
                                                    , "Old Qty", "Old Amt", "Old Remark", "EntDate", "EntPart"
                                              };



        public int[] nenmPmpaMisuMast2TA = {
                                                20,100,50,90,60
                                                ,110,390,10,10,10
                                                ,10,10,10,10,10
                                           };

        public void sSpd_PmpaMisuMast2TA(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            ////스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmIpdMirCheckUpdate)).Length;

            ////spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            ////spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)enmIpdMirCheckUpdate.nToiAmt + 1;//칼럼고정

            //칼럼명 사이즈
            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 40;

            spd.VerticalScrollBarWidth = 13;
            spd.HorizontalScrollBarHeight = 13;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.A, clsSpread.enmSpdType.CheckBox, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.GubunName, clsSpread.enmSpdType.Label, null, null, null, null, false);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.Bdate, clsSpread.enmSpdType.Text, null, null, null, null, false);
            setColStyle_Date(spd, -1, (int)enmPmpaMisuMast2TA.Bdate, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.Gubun, clsSpread.enmSpdType.Text, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.Qty, clsSpread.enmSpdType.Text, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.Amt, clsSpread.enmSpdType.Text, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.Remark, clsSpread.enmSpdType.Text, null, null, null, null, true);

            //methodSpd.setColStyle(spd, -1, 6, clsSpread.enmSpdType.Text,null,null,null,null,false);
            //methodSpd.setColStyle(spd, -1, (int)enmIpdMirCheckUpdate.Remark, clsSpread.enmSpdType.Text, null, null, null, null, false);
            //sup.setColStyle_Text(spd, -1, (int)enmIpdMirCheckUpdate.MirAmt, false, false, false, 1); //txt재정의
            //sup.setColStyle_Text(spd, -1, (int)enmIpdMirCheckUpdate.Remark, false, false, false, 100);//최대길이 100까지 순환참조로 사용불가능
            //methodSpd.setColStyle(spd, -1, (int)enmSupXrayRCP01Copy2.Infect, clsSpread.enmSpdType.IMAGE);

            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast2TA.A, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast2TA.Bdate, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast2TA.Gubun, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast2TA.GubunName, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast2TA.Qty, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast2TA.Amt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast2TA.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);
            //methodSpd.setColLength(spd, -1, (int)enmPmpaMisuMast2.Remark, 39);

            //4.히든          
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.ROWID, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.OldBdate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.OldGubun, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.OldQty, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.OldAmt, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.OldRemark, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.EntDate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.EntPart, clsSpread.enmSpdType.Hide);

            //6.특정문구 색상
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(250, 244, 192);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmPmpaMisuMast2TA.GubunName, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            //unary.BackColor = Color.FromArgb(255, 217, 236);
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmPmpaMisuMast1_2.Chasu, unary);

        }
        #endregion

        #region frmPmpaMisuMast_1.cs 관리영역-김해수
        public enum enmPmpaMisuMast_1
        {
            A, Bdate, Gubun, GubunName, Qty
            , Amt, Remark, ROWID, OldBdate, OldGubun
            , OldQty, OldAmt, OldRemark, EntDate, EntPart
        }

        public string[] senmPmpaMisuMast_1 = {
                                                    "A", "입력일자", "구분", "구분명", "건    수"
                                                    ,"금      액", "적    요", "ROWID", "Old Bdate", "Old Gubun"
                                                    , "Old Qty", "Old Amt", "Old Remark", "EntDate", "EntPart"
                                              };



        public int[] nenmPmpaMisuMast_1 = {
                                                20,100,50,90,60
                                                ,110,390,10,10,10
                                                ,10,10,10,10,10
                                           };

        public void sSpd_PmpaMisuMast_1(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            ////스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmIpdMirCheckUpdate)).Length;

            ////spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            ////spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)enmIpdMirCheckUpdate.nToiAmt + 1;//칼럼고정

            //칼럼명 사이즈
            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 40;

            spd.VerticalScrollBarWidth = 13;
            spd.HorizontalScrollBarHeight = 13;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_1.A, clsSpread.enmSpdType.CheckBox, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_1.GubunName, clsSpread.enmSpdType.Label, null, null, null, null, false);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast2TA.Bdate, clsSpread.enmSpdType.Text, null, null, null, null, false);
            setColStyle_Date(spd, -1, (int)enmPmpaMisuMast_1.Bdate, CellHorizontalAlignment.Center, CellVerticalAlignment.Center);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_1.Gubun, clsSpread.enmSpdType.Text, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_1.Qty, clsSpread.enmSpdType.Text, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_1.Amt, clsSpread.enmSpdType.Text, null, null, null, null, false);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_1.Remark, clsSpread.enmSpdType.Text, null, null, null, null, true);

            TextCellType spdObj = new TextCellType();
            spdObj.Multiline = true;
            spdObj.WordWrap = true;

            spd.ActiveSheet.Columns.Get((int)enmPmpaMisuMast_1.Remark).CellType = spdObj;
           


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_1.A, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_1.Bdate, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_1.Gubun, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_1.GubunName, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_1.Qty, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_1.Amt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_1.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든          
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_1.ROWID, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_1.OldBdate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_1.OldGubun, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_1.OldQty, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_1.OldAmt, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_1.OldRemark, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_1.EntDate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_1.EntPart, clsSpread.enmSpdType.Hide);

            //6.특정문구 색상
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(250, 244, 192);
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmPmpaMisuMast_1.GubunName, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            //unary.BackColor = Color.FromArgb(255, 217, 236);
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmPmpaMisuMast1_2.Chasu, unary);

        }
        #endregion

        #region frmPmpaMisuMast_2.cs 관리영역-김해수
        public enum enmPmpaMisuMast_2
        {
            YYMM, IwolAmt, MisuAmt, IpgumAmt, SakAmt,
            SakAmt2, BanAmt, EtcAmt, JanAmt
        }

        public string[] senmPmpaMisuMast_2 = {
                                                    "월별", "이월금액", "당월미수", "당월입금", "당월삭감"
                                                    ,"당월절사삭감", "당월반송", "당월기타", "월말잔액"
                                              };



        public int[] nenmPmpaMisuMast_2 = {
                                                65,95,95,95,95
                                                ,95,95,95,95
                                           };

        public void sSpd_PmpaMisuMast_2(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            ////스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmIpdMirCheckUpdate)).Length;

            ////spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            ////spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)enmIpdMirCheckUpdate.nToiAmt + 1;//칼럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 27;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 40;

            spd.VerticalScrollBarWidth = 13;
            spd.HorizontalScrollBarHeight = 13;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_2.YYMM, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_2.IwolAmt, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_2.MisuAmt, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_2.IpgumAmt, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_2.SakAmt, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_2.SakAmt, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_2.BanAmt, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_2.EtcAmt, clsSpread.enmSpdType.Label); 
            methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast_2.JanAmt, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, 0, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, 6, clsSpread.enmSpdType.Text,null,null,null,null,false);
            //methodSpd.setColStyle(spd, -1, (int)enmIpdMirCheckUpdate.Remark, clsSpread.enmSpdType.Text, null, null, null, null, false);
            //sup.setColStyle_Text(spd, -1, (int)enmIpdMirCheckUpdate.MirAmt, false, false, false, 1); //txt재정의
            //sup.setColStyle_Text(spd, -1, (int)enmIpdMirCheckUpdate.Remark, false, false, false, 100);//최대길이 100까지 순환참조로 사용불가능
            //methodSpd.setColStyle(spd, -1, (int)enmSupXrayRCP01Copy2.Infect, clsSpread.enmSpdType.IMAGE);

            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_2.YYMM, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_2.IwolAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_2.MisuAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_2.IpgumAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_2.SakAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_2.SakAmt2, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_2.BanAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_2.EtcAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmPmpaMisuMast_2.JanAmt, clsSpread.HAlign_R, clsSpread.VAlign_C);


            //4.히든          
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.ROWID, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldBdate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldGubun, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldQty, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldTAmt, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldAmt, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldRemark, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.OldChasu, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.EntDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmPmpaMisuMast1_2.EntPart, clsSpread.enmSpdType.Hide);

            //6.특정문구 색상
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            //unary.BackColor = Color.FromArgb(250, 244, 192);
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmPmpaMisuMast1_2.GubunName, unary); //결과

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "*", false);
            //unary.BackColor = Color.FromArgb(255, 217, 236);
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmPmpaMisuMast1_2.Chasu, unary);

        }
        #endregion

        /// <summary>
        /// Spread Row 추가 현재셀아래에 추가
        /// </summary>
        /// <param name="o"></param>
        /// 

        public void setDel_Ins_Under(FpSpread o)
        {
            if (o.ActiveSheet.Rows.Count == 0)
            {
                o.ActiveSheet.Rows.Count = 1;
            }
            else if (o.ActiveSheet.Rows.Count - 1 == o.ActiveSheet.ActiveCell.Row.Index)
            {
                o.ActiveSheet.Rows.Count = o.ActiveSheet.Rows.Count + 1;
            }
            else
            {
                o.ActiveSheet.AddRows(o.ActiveSheet.ActiveCell.Row.Index + 1, 1);
            }
        }

        public void setColStyle_Date(FpSpread o, int nRow, int nCol, CellHorizontalAlignment HA, CellVerticalAlignment VA)
        {
            o.ActiveSheet.Columns.Get(nCol).Locked = false;
            DateTimeCellType spdObj = new DateTimeCellType();
            spdObj.DateTimeFormat = DateTimeFormat.ShortDate;

            o.ActiveSheet.Columns[nCol].HorizontalAlignment = HA;// CellHorizontalAlignment.Left;
            o.ActiveSheet.Columns[nCol].VerticalAlignment = VA;

            if (nRow == -1)
            {
                o.ActiveSheet.Columns.Get(nCol).CellType = spdObj;
            }
            else
            {
                o.ActiveSheet.Cells[nRow, nCol].CellType = spdObj;
            }


        }
    }
}
