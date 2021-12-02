using System;
using System.Windows.Forms;
using ComBase;
using FarPoint.Win.Spread;
using System.Drawing;
using System.Data;
using ComDbB;

namespace ComSupLibB
{
    public class clsSupDiet
    {
        public static string GstrHelpCode;
        public static string GstrHelpName;
        public static string GstrLastSS;
        public static bool GbfrmStaff;  //Gb직원식수표시

        public struct Diet_Food_Search
        {
            public string New;
            public string Pano;
            public string sName;
            public string RoomCode;
            public string IpwonDay;
            public string Sex;
            public string Age;
            public string DIAGNOSIS;
            public string DeptCode;
            public string DietName;
            public string Height;
            public string Weight;
            public string HWeight;
            public string IBW;
            public string LabALB;
            public string LabTLC;
            public string LabHB;
            public string LabTcho;
            public string Cnt;
            public string DrCode;
            public string WardCode;
            public string sDate;  //'환자관리 등록일자.
            public string Warning;
            public string UDATE;
            public double IPDNO;
        }
        public static Diet_Food_Search dst;

        public struct Table_Diet_Jikwon
        {
            public int Sabun;
            public string Name;
            public string Buse;
            public string FoodDay;
            public int Inx;
            public string ROWID;
        }
        public static Table_Diet_Jikwon TDJ;
        
        public static void dstClear()
        {
            dst.New = "";
            dst.Pano = null;
            dst.sName = "";
            dst.RoomCode = "";
            dst.IpwonDay = "";
            dst.Sex = "";
            dst.Age = "";
            dst.DIAGNOSIS = "";
            dst.DeptCode = "";
            dst.DietName = "";
            dst.Height = "";
            dst.Weight = "";
            dst.HWeight = "";
            dst.IBW = "";
            dst.LabALB = "";
            dst.LabTLC = "";
            dst.LabHB = "";
            dst.LabTcho = "";
            dst.Cnt = "";
            dst.DrCode = "";
            dst.WardCode = "";
            dst.sDate = "";  //'환자관리 등록일자.
            dst.Warning = "";
            dst.UDATE = "";
            dst.IPDNO = 0;
        }

        public static string GET_Kcal(string argH, string argW, string ArgSex)
        {
            string strRtn = "";
            //'권장 열량 구하기(세자리에서 반올림 함)
            //'권장 열량 = 적정체중 * 30
            //'적정체중 = (남)키 * 키 * 22
            //'적정체중 = (여)키 * 키 * 21

            if (VB.IsNumeric(argH) && VB.IsNumeric(argW))
            {
            }
            else
            {
                strRtn = "Error";
                return strRtn;
            }

            if (ArgSex == "")
            {
                return strRtn;
            }

            if (ArgSex == "M")
            {
                strRtn = (Math.Round(((Convert.ToDouble(VB.Val(argH) * 0.01) * Convert.ToDouble(VB.Val(argH) * 0.01) * 22) * 30) * 0.01) * 100).ToString();
            }
            else if (ArgSex == "F")
            {
                strRtn = (Math.Round(((Convert.ToDouble(VB.Val(argH) * 0.01) * Convert.ToDouble(VB.Val(argH) * 0.01) * 21) * 30) * 0.01) * 100).ToString();
            }
            else
            {
                MessageBox.Show("성별이 없습니다");
            }

            return strRtn;
        }

        public static string DietSearch_Code(string argText, string argGubun)
        {
            string rtnVar = "";

            if (argGubun == "0")
            {
                switch (argText)
                {
                    case "H":
                        rtnVar = "고위험";
                        break;
                    case "M":
                        rtnVar = "중위험";
                        break;
                    case "L":
                        rtnVar = "저위험";
                        break;
                }
            }
            else if (argGubun == "1")
            {
                switch (argText)
                {
                    case "0":
                        rtnVar = "Adaquate";
                        break;
                    case "1":
                        rtnVar = "Marasmus-type";
                        break;
                    case "2":
                        rtnVar = "Kwashiorkor";
                        break;
                    case "3":
                        rtnVar = "Mild PCM";
                        break;
                    case "4":
                        rtnVar = "Moderate PCM";
                        break;
                    case "5":
                        rtnVar = "Severe PCM";
                        break;
                    case "6":
                        rtnVar = "Overweight";
                        break;
                    case "7":
                        rtnVar = "Obesity";
                        break;
                    default:
                        rtnVar = "";
                        break;
                }
            }

            return rtnVar;
        }

        //TODO : frmDietOrderNew 에도 같은 함수가 있으므로 정리요망
        public bool GET_DIETPRT_LOCK(PsmhDb pDbCon)
        {
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;
            string strDate = "";

            Cursor.Current = Cursors.WaitCursor;

            ComFunc CF = new ComFunc();

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(LOCKDATE, 'YYYY-MM-DD HH:MM') AS LOCKDATE FROM " + ComNum.DB_PMPA + "DIET_LOCK ";
                SQL = SQL + ComNum.VBLF + " ORDER BY LOCKDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strDate = dt.Rows[0]["LOCKDATE"].ToString().Trim();

                    if (CF.DATE_TIME(pDbCon, strDate,
                        ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "D"), "D", "-") + " " +
                        ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":")) <= 3)
                    {
                        rtnVal = true;
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return rtnVal;
            }

        }

        #region 식이일괄등록 관리영역
        public enum enmDietAll      {  ROOM,   SNAME,  DSEARCH,    MOR,    AFT,    EVE,    OUTC,       ADDC,         PSTS,       PTNO,       DEPT,    BI,      SNAME2, DRCODE,     IPDNO,   INDATE,   SEX,    ENTDATE           }
        public string[] sDietAll =  { "호실", "성명", "영양검색", "아침", "점심", "저녁", "퇴원확인", "추가상확인", "환자상태", "등록번호", "진료과", "자격", "성명", "의사코드", "IPDNO", "INDATE", "성별", "영양검색등록일"   };
        public int[] nDietAll =     {  34,     60,     30,         92,     92,     92,     30,         38,           46,         48,         38,       30,     60,     48,         48,      56,       34,     58                };

        public void sSpd_enmHcGroup(FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            try
            {
                clsSpread cSpd = new clsSpread();
                
                //스프레드 사이즈
                spd.ActiveSheet.RowCount = RowCnt;
                spd.ActiveSheet.ColumnCount = ColCnt;

                if (ColCnt == 0) { spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmDietAll)).Length; }

                spd.ActiveSheet.ColumnHeader.Rows[0].Height = 36;

                spd.VerticalScrollBarWidth = 13;
                //spd.HorizontalScrollBarHeight = 16;

                //1.헤더 및 사이즈
                cSpd.setHeader(spd, colName, size, 9);
                spd.ActiveSheet.Columns.Get(-1).Visible = true;
                
                //2.컬럼 스타일
                cSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Text);
                cSpd.setColStyle(spd, -1, (int)enmDietAll.OUTC, clsSpread.enmSpdType.CheckBox);
                cSpd.setColStyle(spd, -1, (int)enmDietAll.ADDC, clsSpread.enmSpdType.CheckBox);

                //3.정렬
                cSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
                cSpd.setColAlign(spd, (int)enmDietAll.SNAME, clsSpread.HAlign_L, clsSpread.VAlign_C);
               
                //4.히든
                cSpd.setColStyle(spd, -1, (int)enmDietAll.OUTC, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmDietAll.ADDC, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmDietAll.PSTS, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmDietAll.PTNO, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmDietAll.DEPT, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmDietAll.BI, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmDietAll.SNAME2, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmDietAll.DRCODE, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmDietAll.IPDNO, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmDietAll.INDATE, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmDietAll.SEX, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(spd, -1, (int)enmDietAll.ENTDATE, clsSpread.enmSpdType.Hide); 

                //5.Filter
                //cSpd.setSpdFilter(spd, (int)enmHcGroup.CODE, AutoFilterMode.EnhancedContextMenu, true);
                //cSpd.setSpdFilter(spd, (int)enmHcGroup.NAME, AutoFilterMode.EnhancedContextMenu, true);

                // 6. 특정문구 색상 
                UnaryComparisonConditionalFormattingRule unary;
               
                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
                unary.BackColor = Color.PaleTurquoise;
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmDietAll.MOR, unary);

                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
                unary.BackColor = Color.PaleTurquoise;
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmDietAll.AFT, unary);

                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
                unary.BackColor = Color.PaleTurquoise;
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmDietAll.EVE, unary);

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        #endregion
    }
}
