using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ComDbB;
using ComBase;
using System.Windows.Forms;

namespace ComMirLibB.MirEnt
{
    public class clsComMirEntSQL
    {
        ComFunc CF = new ComFunc();
        string SQL = "";

        public DataTable sel_Mir_InsDtl(PsmhDb pDbCon, int argWRTNO, string argType)
        {
            DataTable dt = null; 
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";

            if (argType == "A")
            {
                SQL = "SELECT M.WRTNO,   M.SeqNo1,  SeqNo2,  ItemGubun, M.Sunext, M.AUTODEL, M.GbSugbS , M.GBSAK, M.GBSAKDTL,   ";
                SQL += ComNum.VBLF + "       M.Bun,     M.Qty,     Nal,     Price,     GbNgt, ";
                SQL += ComNum.VBLF + "       GbGisul, GbChild, GBSELF, AMT,     UpCheck,   Memos,m.Memos2, M.GBDR,";
                SQL += ComNum.VBLF + "       B.SunameK,TO_CHAR(FrDate,'YYYY-MM-DD') FrDate, M.ROWID, ";
                SQL += ComNum.VBLF + "       EdiSeq,EdiCode,EdiPrice,EdiQty,EdiNal,EdiAmt,Remark,WonSayu,  DRUGAMT, SAMT,  EDIDRUGAMT, ";
                SQL += ComNum.VBLF + "       WRTNOS, DIV, DIVQTY, EDIDIV, EDIDIVQTY, SCODESAYU, SCODEREMARK, XrayRead, ";
                SQL += ComNum.VBLF + "  M.ILLCODE ,  M.DRBUNHO, M.DRAUTO ,m.KTASLEVL,b.SugbAA,b.sugbs, B.SUGBAC, M.GBGSADD  ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.MIR_INSDTL M, BAS_SUN B ";
                SQL += ComNum.VBLF + " WHERE WRTNO    = '" + argWRTNO + "' ";
                SQL += ComNum.VBLF + "   AND M.Sunext =  B.Sunext(+) ";
                SQL += ComNum.VBLF + "   ORDER BY SEQNO1 ";
            }
            else if (argType == "E")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT a.SuNext,a.Qty,a.Nal,a.Price,a.Amt,";
                SQL += ComNum.VBLF + "       a.EdiCode,a.EdiQty,a.EdiNal,a.EdiPrice,a.EdiAmt, a.Rowid,";
                SQL += ComNum.VBLF + "       b.SuNameK,c.Pname,c.Danwi1,c.Danwi2,c.Spec ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.MIR_INSDTL a,BAS_SUN b,EDI_SUGA c ";
                SQL += ComNum.VBLF + " WHERE a.WRTNO = " + argWRTNO + " ";
                SQL += ComNum.VBLF + "   AND a.SuNext NOT IN ('########','********')";
                SQL += ComNum.VBLF + "   AND a.SuNext = b.SuNext ";
                SQL += ComNum.VBLF + "   AND a.EdiCode = c.Code(+) ";
                SQL += ComNum.VBLF + " ORDER BY a.SeqNo1,a.SeqNo2 ";
            }
            else
            {
                SQL += "SELECT seqno1, seqno2, SuNext,Bun,GbGisul,Amt, rowid ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.MIR_INSDTL ";
                SQL += ComNum.VBLF + " WHERE WRTNO = " + argWRTNO + " ";
                SQL += ComNum.VBLF + "   AND SuNext NOT IN ('########','********') ";
                SQL += ComNum.VBLF + "   AND (EDIHANG IS NULL OR EDIHANG <>'U ' OR SUNEXT ='DRG003' )  "; // ' 100/100 제외
                SQL += ComNum.VBLF + "   AND (GbSugbs IS NULL OR GbSugbs NOT IN ('4','5','6') ) ";


            }

            //SQL += ComNum.VBLF + "   AND DTno  != '61'";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }



            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// EDI 표준 약제 상한가 차액을 읽기 위함 함수
        /// </summary>
        /// <param name="argBDate"></param>
        /// <param name="argSuNext"></param>
        /// <param name="argJINDATE"></param>
        /// <returns></returns>
        public double READ_BAS_SUGA_SAMT(PsmhDb DbCon, string argBDate, string argSuNext, string argJINDATE)
        {
            string strBDate = "";
            double rtnVal = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (argBDate.CompareTo("2012-02-01") >= 0)
                return rtnVal;

            if (argBDate == "")
            {
                strBDate = VB.Left(argJINDATE, 4) + "-" + VB.Mid(argJINDATE, 6, 2) + "-" + VB.Mid(argJINDATE, 9, 2);
            }
            else
            {
                strBDate = argBDate;
            }
            try
            {
                //약제 표준코드 , 환산계수 읽기
                SQL = " SELECT  SAMT ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_SUGA_AMT ";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT ='" + argSuNext + "'  ";
                SQL = SQL + ComNum.VBLF + "   AND SUDATE <= TO_DATE('" + strBDate + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY  SUDATE DESC  ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, DbCon);

                if (dt.Rows.Count > 0)
                {
                    rtnVal = Convert.ToInt64(dt.Rows[0]["SAMT"]);
                }
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, DbCon); //에러로그 저장
                    return rtnVal;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, DbCon); //에러로그 저장
                return rtnVal;
            }
            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        public string sel_ILL_NAME(PsmhDb pDbCon, string argILL, string argMsg, string argIpdOpd, string argIODate, string argOutDate, int argJinIlsu, string argJindate1)
        {
            DataTable dt = null;
            DataTable dt2 = null;
            string strVal = "";
            string strOutDate = "";
            string SqlErr = ""; //에러문 받는 변수
            string strILLMSG = "";
            string strGBVCODE = "";
            string strGBV252 = "";

            if (argIpdOpd == "O")
            {
                strOutDate = argOutDate;
                if (strOutDate == "")
                {
                    strOutDate = VB.Left(argJindate1, 4) + "-" + VB.Mid(argJindate1, 5, 2) + "-" + VB.Right(argJindate1, 2);
                }
            }
            else
            {

                strOutDate = VB.Mid(argIODate, 9, 8);

                if (VB.Len(strOutDate) < 10)
                {
                    strOutDate = argOutDate;

                }
                else
                {
                    strOutDate = VB.Left(strOutDate, 4) + "-" + VB.Mid(strOutDate, 5, 2) + "-" + VB.Right(strOutDate, 2);
                }
                if (VB.Len(argIODate) < 16 && strOutDate == "")
                {
                    strOutDate = CF.DATE_ADD(clsDB.DbCon, VB.Left(strOutDate, 4) + "-" + VB.Mid(strOutDate, 5, 2) + "-" + VB.Right(strOutDate, 2), argJinIlsu);
                }
            }
            strVal = "********";
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT IllNameK, NOUSE, TO_CHAR(DDATE,'YYYY-MM-DD' ) DDATE , TO_CHAR(NoUseDate,'YYYY-MM-DD' ) NoUseDate, GBV252 , GBVCODE  ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_ILLS ";
                SQL += ComNum.VBLF + "WHERE IllCode = '" + argILL + "' ";
                SQL += ComNum.VBLF + "  AND ILLCLASS = '1' ";

                //DateTime Test1 = Convert.ToDateTime("2016-03-21");
                //DateTime Test2 = Convert.ToDateTime("2016-05-20");
                //if (DateTime.Compare(Test1, Test2) > 0)
                //                    TestTime1 > TestTime2
                //if (DateTime.Compare(Test1, Test2) == 0)
                //                    TestTime1 == TestTime2
                //if (DateTime.Compare(Test1, Test2) < 0)
                //                    TestTime1 < TestTime2

                if (DateTime.Compare(Convert.ToDateTime(strOutDate), Convert.ToDateTime("2016-01-01")) < 0) { SQL += ComNum.VBLF + " AND  ( KCDOLD ='*' OR KCD6  ='*' ) "; }
                if (DateTime.Compare(Convert.ToDateTime(strOutDate), Convert.ToDateTime("2016-01-01")) >= 0) { SQL += ComNum.VBLF + " AND  ( KCDOLD ='*' OR KCD6  ='*' OR  KCD7 ='*') "; }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["DDATE"].ToString().Trim() != "" && argMsg != "1")
                    {
                        if (DateTime.Compare(Convert.ToDateTime(strOutDate), Convert.ToDateTime(dt.Rows[0]["DDATE"].ToString().Trim())) >= 0)
                        {
                            strILLMSG = "[삭제상병]" + ComNum.VBLF + ComNum.VBLF;
                            strILLMSG = strILLMSG + argILL + " 는 삭제상병입니다. " + ComNum.VBLF + ComNum.VBLF;

                            SQL = " SELECT ILLCODE, ILLNAMEK FROM kOSMOS_PMPA.BAS_ILLS ";
                            SQL += ComNum.VBLF + "WHERE ILLCODE  LIKE '" + VB.Left(argILL, VB.Len(argILL) - 1) + "%' ";
                            SQL += ComNum.VBLF + "  AND LENGTH(ILLCODE) <= 6 ";
                            SQL += ComNum.VBLF + "  AND (NOUSE <>'N' OR NOUSE IS NULL) ";
                            SQL += ComNum.VBLF + "  AND ILLCLASS ='1' ";
                            SQL += ComNum.VBLF + "  AND ( DDATE IS NULL OR DDATE < TO_DATE('" + dt.Rows[0]["DDATE"].ToString().Trim() + "','YYYY-MM-DD'))  ";
                            if (DateTime.Compare(Convert.ToDateTime(strOutDate), Convert.ToDateTime("2016-01-01")) < 0) { SQL += ComNum.VBLF + " AND  ( KCDOLD ='*' OR KCD6  ='*' ) "; }
                            if (DateTime.Compare(Convert.ToDateTime(strOutDate), Convert.ToDateTime("2016-01-01")) >= 0) { SQL += ComNum.VBLF + " AND  ( KCDOLD ='*' OR KCD6  ='*' OR  KCD7 ='*') "; }

                            SqlErr = clsDB.GetDataTable(ref dt2, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return strVal;
                            }
                            if (dt.Rows.Count > 0)
                            {
                                strILLMSG = strILLMSG + " 사용가능 상병 아래 상병참조 바랍니다." + ComNum.VBLF + ComNum.VBLF;
                                strILLMSG = strILLMSG + "====================================================" + ComNum.VBLF + ComNum.VBLF;
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    strILLMSG = strILLMSG + VB.Left(dt2.Rows[0]["illcode"].ToString().Trim() + VB.Space(12), 12) + " " + dt2.Rows[0]["IllNameK"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                }
                                dt.Dispose();
                                dt = null;
                            }
                            strILLMSG = strILLMSG + "====================================================" + ComNum.VBLF + ComNum.VBLF;
                            MessageBox.Show(strILLMSG);
                        }
                    }
                    else
                    {
                        if (dt.Rows[0]["NoUseDate"].ToString().Trim() != "")
                        {
                            if (DateTime.Compare(Convert.ToDateTime(strOutDate), Convert.ToDateTime(dt.Rows[0]["NoUseDate"].ToString().Trim())) >= 0 && dt.Rows[0]["Nouse"].ToString().Trim() == "N" && argMsg != "1")
                            {

                                strILLMSG = "[불완전상병]" + ComNum.VBLF + ComNum.VBLF;

                                strILLMSG = strILLMSG + argILL + " 는 불완전상병입니다. " + ComNum.VBLF + ComNum.VBLF;


                                SQL = " SELECT ILLCODE, ILLNAMEK FROM kOSMOS_PMPA.BAS_ILLS ";
                                SQL += ComNum.VBLF + "WHERE ILLCODE  LIKE '" + VB.Left(argILL, VB.Len(argILL) - 1) + "%' ";
                                SQL += ComNum.VBLF + "  AND LENGTH(ILLCODE) <= 6 ";
                                SQL += ComNum.VBLF + "  AND (NOUSE <>'N' OR NOUSE IS NULL) ";
                                SQL += ComNum.VBLF + "  AND ILLCLASS ='1' ";
                                SQL += ComNum.VBLF + "  AND ( DDATE IS NULL OR DDATE < TO_DATE('" + dt.Rows[0]["DDATE"].ToString().Trim() + "','YYYY-MM-DD'))  ";
                                if (DateTime.Compare(Convert.ToDateTime(strOutDate), Convert.ToDateTime("2016-01-01")) < 0) { SQL += ComNum.VBLF + " AND  ( KCDOLD ='*' OR KCD6  ='*' ) "; }
                                if (DateTime.Compare(Convert.ToDateTime(strOutDate), Convert.ToDateTime("2016-01-01")) >= 0) { SQL += ComNum.VBLF + " AND  ( KCDOLD ='*' OR KCD6  ='*' OR  KCD7 ='*') "; }

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return strVal;
                                }
                                if (dt.Rows.Count > 0)
                                {
                                    strILLMSG = strILLMSG + " 사용가능 상병 아래 상병참조 바랍니다." + ComNum.VBLF + ComNum.VBLF;
                                    strILLMSG = strILLMSG + "====================================================" + ComNum.VBLF + ComNum.VBLF;
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        strILLMSG = strILLMSG + VB.Left(dt2.Rows[0]["illcode"].ToString().Trim() + VB.Space(12), 12) + " " + dt2.Rows[0]["IllNameK"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                    }
                                    dt2.Dispose();
                                    dt2 = null;
                                }
                                strILLMSG = strILLMSG + "====================================================" + ComNum.VBLF + ComNum.VBLF;
                                MessageBox.Show(strILLMSG);
                            }
                        }
                        else
                        {
                            if (dt.Rows[0]["GBVCODE"].ToString().Trim() == "*")
                            {
                                strGBVCODE = "＠";
                            }

                            if (dt.Rows[0]["GBV252"].ToString().Trim() == "*")
                            {
                                strGBV252 = "★";
                            }

                            strVal = strGBVCODE + strGBV252 + dt.Rows[0]["IllNameK"].ToString().Trim();
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                if (dt2 != null)
                {
                    dt2.Dispose();
                    dt2 = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public double Suga_Read_Amt_NEW2(PsmhDb pDbCon, string argSunext, string argSuDate)
        {
            double retVal = 0;
            DataTable dt = null;
            DataTable dt2 = null;

            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(SUDATE,'YYYY-MM-DD') SuDate,iAmt,bAmt,tAmt,sAmt,SelAmt ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUGA_AMT ";
                SQL += ComNum.VBLF + "  WHERE Sunext ='" + argSunext + "' ";
                SQL += ComNum.VBLF + "   AND SuDate <=TO_DATE('" + VB.Left(argSuDate, 10) + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND DelDate IS NULL ";
                SQL += ComNum.VBLF + " ORDER BY SUDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return retVal;
                }

                if (dt.Rows.Count > 0)
                {
                    //TODO: 자격별로 분리해서단가를 읽어야함.
                    retVal = Convert.ToInt32(dt.Rows[0]["bAmt"].ToString().Trim());
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT a.Sucode, a.Bun,    a.Nu,    a.SugbA, a.SugbB,                          ";
                    SQL += ComNum.VBLF + "       a.SugbC,  a.SugbD,  a.SugbE, a.SugbF, a.SugbG,                          ";
                    SQL += ComNum.VBLF + "       a.SugbH,  a.SugbI,  a.SugbJ,  a.Iamt,  a.Tamt,                          ";
                    SQL += ComNum.VBLF + "       a.Bamt,   TO_CHAR(a.Sudate, 'yyyy-mm-dd') SuDay,                        ";
                    SQL += ComNum.VBLF + "       a.OldIamt,a.OldTamt,a.OldBamt,a.Sunext,                                 ";
                    SQL += ComNum.VBLF + "       TO_CHAR(a.Sudate3, 'yyyy-mm-dd') SuDay3,                                ";
                    SQL += ComNum.VBLF + "       a.Iamt3, a.Tamt3, a.Bamt3, TO_CHAR(a.Sudate4, 'yyyy-mm-dd') SuDay4,     ";
                    SQL += ComNum.VBLF + "       a.Iamt4, a.Tamt4, a.Bamt4, TO_CHAR(a.Sudate5, 'yyyy-mm-dd') SuDay5,     ";
                    SQL += ComNum.VBLF + "       a.Iamt5, a.Tamt5, a.Bamt5,                                              ";
                    SQL += ComNum.VBLF + "       a.DayMax, a.TotMax, b.SugbQ,  b.SugbR                                   ";
                    SQL += ComNum.VBLF + "  FROM BAS_SUT a , BAS_SUN b                                                   ";
                    SQL += ComNum.VBLF + " WHERE TRIM(a.Sucode) = '" + argSunext + "'                              ";
                    SQL += ComNum.VBLF + "   AND a.SuNext = b.SuNext(+) ";
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        dt2.Dispose();
                        dt2 = null;
                        return retVal;
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        if (DateTime.Compare(Convert.ToDateTime(dt2.Rows[0]["SuDay"].ToString().Trim()), Convert.ToDateTime(argSuDate)) <= 0)
                        {
                            retVal = Convert.ToInt32(dt2.Rows[0]["Bamt"].ToString().Trim());
                        }
                        else
                        {
                            if (DateTime.Compare(Convert.ToDateTime(dt2.Rows[0]["SuDay3"].ToString().Trim()), Convert.ToDateTime(argSuDate)) <= 0)
                            {
                                retVal = Convert.ToInt32(dt2.Rows[0]["OlbBamt"].ToString().Trim());
                            }
                            else
                            {
                                if (DateTime.Compare(Convert.ToDateTime(dt2.Rows[0]["SuDay4"].ToString().Trim()), Convert.ToDateTime(argSuDate)) <= 0)
                                {
                                    retVal = Convert.ToInt32(dt2.Rows[0]["Bamt3"].ToString().Trim());
                                }
                                else
                                {
                                    if (DateTime.Compare(Convert.ToDateTime(dt2.Rows[0]["SuDay5"].ToString().Trim()), Convert.ToDateTime(argSuDate)) <= 0)
                                    {
                                        retVal = Convert.ToInt32(dt2.Rows[0]["Bamt4"].ToString().Trim());
                                    }
                                    else
                                    {
                                        retVal = Convert.ToInt32(dt2.Rows[0]["Bamt5"].ToString().Trim());
                                    }
                                }
                            }
                        }
                    }
                    dt2.Dispose();
                    dt2 = null;
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                if (dt2 != null)
                {
                    dt2.Dispose();
                    dt2 = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return retVal;
        }

        public string sel_BAS_SUT_B2(PsmhDb pDbCon, string argSunext)
        {
            string strVal = "";
            DataTable dt = null;
            DataTable dt2 = null;

            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT SUGbB  ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_SUT ";
                SQL += ComNum.VBLF + " WHERE SuNext ='" + argSunext + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["SUGbB"].ToString().Trim();
                }
                else
                {
                    SQL = "SELECT SUGbB  ";
                    SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_SUH ";
                    SQL += ComNum.VBLF + " WHERE SuNext ='" + argSunext + "' ";
                    SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, pDbCon);
                    if (dt2.Rows.Count > 0)
                    {
                        strVal = dt2.Rows[0]["SUGbB"].ToString().Trim();
                    }
                    dt2.Dispose();
                    dt2 = null;
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                if (dt2 != null)
                {
                    dt2.Dispose();
                    dt2 = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public string Read_Mir_COLOR_SET(PsmhDb pDbCon, string argJobSabun, string argSunext)
        {


            string strVal = "";
            DataTable dt = null;

            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";

                SQL = " SELECT SUNEXT, RGB FROM KOSMOS_PMPA.MIR_COLOR_SET ";
                SQL += ComNum.VBLF + "  WHERE SABUN = '" + argJobSabun + "' ";
                SQL += ComNum.VBLF + "      AND SUNEXT = '" + argSunext + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["RGB"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public string sel_MIR_IPDSPEC(PsmhDb pDbCon, string argPano, string argJinDate1, double argIPDNO, string argSunext)
        {
            string strVal = "";
            DataTable dt = null;

            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT ROWID ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.MIR_IPDSPEC";
                SQL += ComNum.VBLF + " WHERE Pano ='" + argPano + "' ";
                SQL += ComNum.VBLF + "   AND (INDATE = TO_DATE('" + argJinDate1 + "','YYYYMMDD') OR IPDNO = '" + argIPDNO + "') ";
                SQL += ComNum.VBLF + "   AND TRIM(SUNEXT) = '" + argSunext + "' ";
                SQL += ComNum.VBLF + "   AND (GBSPEC ='2' OR GBSPEC IS NULL)  ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public string READ_OCS_Doctor_DrName(PsmhDb pDbCon, string strDrBunho)
        {
            string retVal = "";
            DataTable dt = null;

            string SqlErr = ""; //에러문 받는 변수

            try
            {
                //번호읽기
                SQL = " SELECT DRNAME " + ComNum.VBLF;
                SQL += "    FROM KOSMOS_OCS.OCS_DOCTOR " + ComNum.VBLF;
                SQL += "   WHERE DRBUNHO = '" + strDrBunho.Trim() + "' " + ComNum.VBLF;
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return retVal;
                }

                if (dt.Rows.Count > 0)
                {
                    retVal = dt.Rows[0]["DRNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return retVal;
        }

        public DataTable sel_Mir_Insid_Wrtno(PsmhDb pDbCon, string strPano, string strOpenYYMM, string strOpenIO, string strOpenJOB, string argDT, string argMonth = "", string argWrtno = "")
        {
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            DateTime FirstDayMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT YYMM, WRTNO, Seqno,Pano, Sname, Kiho, Gkiho, RateBon,DTno, BohoJong, ";
            SQL += ComNum.VBLF + "       TO_CHAR(OutDate, 'YYYY-MM-DD') OutDay, Upcnt1, Edimirno, Jindate1 , DEPTCODE1,StopFLAG, SCODE, JAMT, TAMT, BAMT ";
            SQL += ComNum.VBLF + "  FROM  KOSMOS_PMPA.MIR_INSID ";
            SQL += ComNum.VBLF + " WHERE 1=1  ";
            SQL += ComNum.VBLF + "   AND IpdOpd = '" + strOpenIO + "'";
            SQL += ComNum.VBLF + "   AND Johap  = '" + strOpenJOB + "'";
            SQL += ComNum.VBLF + "   AND Pano   = '" + strPano + "'";
            if (argDT == "DT")
            {
                SQL += ComNum.VBLF + "   AND DTno  = '61'";
            }

            if (argMonth == "6개월전")
            {
                SQL += ComNum.VBLF + "  AND YYMM >= '" + FirstDayMonth.AddMonths(-6).ToString("yyyyMM") + "'";
            }
            else if (argMonth == "12개월전")
            {
                SQL += ComNum.VBLF + "  AND YYMM >= '" + FirstDayMonth.AddMonths(-12).ToString("yyyyMM") + "'";
            }

            if (argWrtno != "")
            {
                SQL += ComNum.VBLF + "  AND WRTNO <> '" + argWrtno + "'";
            }
            //else
            //{
            //    SQL += ComNum.VBLF + "   AND DTno  != '61'";
            //}

            SQL += ComNum.VBLF + " ORDER BY Jindate1 DESC , YYMM, RATEBON ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_Mir_Sanid_Wrtno(PsmhDb pDbCon, string strPano, string strOpenYYMM, string strOpenIO, string strOpenJOB)
        {
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += ComNum.VBLF + "SELECT YYMM, WRTNO,  Pano, Sname, GelCode, CoprName,  RateGasan , MIRGBN , ";
            SQL += ComNum.VBLF + "       TO_CHAR(OutDate, 'YYYY-MM-DD') OutDay, Upcnt1, Edimirno, FRDATE , DEPTCODE,   JAMT  ";
            SQL += ComNum.VBLF + "  FROM  KOSMOS_PMPA.MIR_SANID ";
            SQL += ComNum.VBLF + " WHERE YYMM   = '" + strOpenYYMM + "'  ";
            SQL += ComNum.VBLF + "   AND IpdOpd = '" + strOpenIO + "'";
            //SQL += ComNum.VBLF + "   AND Johap  = '" + strOpenJOB + "'";
            SQL += ComNum.VBLF + "   AND Pano   = '" + strPano + "'";
            SQL += ComNum.VBLF + " ORDER BY YYMM ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_Mir_Taid_Wrtno(PsmhDb pDbCon, string strPano, string strOpenYYMM, string strOpenIO, string strOpenJOB)
        {
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += ComNum.VBLF + "SELECT YYMM, WRTNO, Seqno,Pano, Sname,  RateBon,DTno,  MIRGBN , ZIPCODE1, ";
            SQL += ComNum.VBLF + "       TO_CHAR(OutDate, 'YYYY-MM-DD') OutDay, Upcnt1, Edimirno,";
            SQL += ComNum.VBLF + "       TO_CHAR(FRDATE,'YYYY-MM-DD') FRDATE , DEPTCODE1,   JAMT, TAMT, BAMT  ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.MIR_TAID ";
            SQL += ComNum.VBLF + " WHERE YYMM   = '" + strOpenYYMM + "'  ";
            SQL += ComNum.VBLF + "   AND IpdOpd = '" + strOpenIO + "'";
            //SQL += ComNum.VBLF + "   AND Johap  = '" + strOpenJOB + "'";
            SQL += ComNum.VBLF + "   AND Pano   = '" + strPano + "'";
            SQL += ComNum.VBLF + " ORDER BY YYMM, RATEBON ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

    }
}
