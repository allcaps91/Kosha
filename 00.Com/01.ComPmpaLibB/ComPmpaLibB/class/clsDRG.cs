using ComDbB;
using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    public class DRG
    {
        #region DRG 공용변수 선언부
        public static long GnDRG_Amt1           = 0;                //DRG 점수별 금액
        public static long GnDRG_Amt2           = 0;                //DRG 점수별 금액*단가
        public static long GnDRG_OGAddAmt       = 0;                //DRG OG가산 금액(총금액=GnDRG_TAMT) ... 계산없이 금액을 바로 산정(ADMIN.DRG_DAY_COST_NEW)
        public static long GnDRG_TAmt           = 0;                //DRG 총액
        public static long GnDRG_TBonAmt        = 0;                //DRG 본인부담계산을 위한 총액
        public static long GnDRG_WBonAmt        = 0;                //DRG 본인부담계산을 위한 원금액
        public static long GnDrgBonAmt          = 0;                //DRG 급여본인부담금액
        public static long GnDrgJohapAmt        = 0;                //DRG 급여조합부담금액
        public static long[] GnDrgFoodAmt       = new long[6];      //DRG 식대금액(본인, 공단, 100/100, 선택진료, 비급여)
        public static long[] GnDrgRoomAmt       = new long[6];      //DRG 병실차액(본인, 공단, 100/100, 선택진료, 비급여)
        public static long[] GnDrgSelAmt        = new long[31];     //DRG 선택진료금액
        public static long GnDrgBiTAmt          = 0;                //DRG 비급여진료총금액
        public static long GnDrgSelTAmt         = 0;                //DRG 선택진료총금액
        public static long GnDrgBiFAmt          = 0;                //DRG 비급여금액
        public static long GnGsAddAmt           = 0;                //DRG 외과가산 진료비 (DRG 총액에 합산)
        public static long GnGs100Amt           = 0;                //DRG 100/100 진료비
        public static long GnGs90Amt_T          = 0;                //DRG 100/90 진료비_총액     2018-01-01
        public static long GnGs90Amt_J          = 0;                //DRG 100/90 진료비_조합     2018-01-01
        public static long GnGs90Amt_B          = 0;                //DRG 100/90 진료비_본인     2018-01-01
        public static long GnGs80Amt_T          = 0;                //DRG 100/80 진료비_총액     2015-10-21
        public static long GnGs80Amt_J          = 0;                //DRG 100/80 진료비_조합     2015-10-21
        public static long GnGs80Amt_B          = 0;                //DRG 100/80 진료비_본인     2015-10-21
        public static long GnGs50Amt_T          = 0;                //DRG 100/50 진료비_총액     2015-10-21
        public static long GnGs50Amt_J          = 0;                //DRG 100/50 진료비_조합     2015-10-21
        public static long GnGs50Amt_B          = 0;                //DRG 100/50 진료비_본인     2015-10-21
        public static long Gn복강개복Amt        = 0;                //DRG 복강개복 수가(본인 20% 부담)
        public static long GnOTChaAmt           = 0;                //DRG  조절성인공수정체 사용시 인공수정체 제외add 2020-01-01
        public static long GnOTChaAmt_Bon = 0;                     //DRG  조절성인공수정체 사용시 인공수정체 제외add 2020-01-01
        public static long GnOTChaAmt_Jhp = 0;                      //DRG  조절성인공수정체 사용시 인공수정체 제외add 2020-01-01
        public static long GnDrgADDAmt = 0;                         //DRG  보상률 add 2020-01-01
        public static long GnDrgADDAmt_Bon = 0;                     //DRG  보상률 add 2020-01-01
        public static long GnDrgADDAmt_Jhp = 0;                     //DRG  보상률 add 2020-01-01
        public static long GnAmt1               = 0;                //행위별 급여총액
        public static long GnAmt2               = 0;                //행위별 비급여 총액
        public static long GnDrg추가입원료      = 0;                //2014-08-28
        public static long GnDrg추가입원료_Bon  = 0;                //2014-08-28
        public static long GnDrg추가입원료_Jhp  = 0;                //2014-08-28
        public static long GnDrg추가입원료_Tbed = 0;                //2018-07-01
        public static long GnDrg추가입원료_Bbed = 0;                //2018-07-01
        public static long GnDrg추가입원료_Nbed = 0;                //2018-07-01
        public static long GnDrgJinAmt          = 0;                //2015-08-31
        public static long GnDrgJinAmt_Bon      = 0;                //2015-08-31
        public static long GnDrgJinAmt_Jhp      = 0;                //2015-08-31
        public static long GnDrgJinSAmt         = 0;                //2018-01-01
        public static long GnDrgJinSAmt_Bon     = 0;                //2018-01-01
        public static long GnDrgJinSAmt_Jhp     = 0;                //2018-01-01
        public static long GnDrg급여총액        = 0;                //2014-07-22
        public static long GnDrg지원금          = 0;                //2014-11-24
        public static long GnDrg부수술총액      = 0;                //2015-01-31
        public static long GnDrg부수술총액_Bon  = 0;                //2015-01-31
        public static long GnDrg부수술총액_Jhp  = 0;                //2015-01-31
        public static long Gn행위별총액         = 0;                //2015-03-02
        public static long GnDrg열외군금액      = 0;                //2015-03-02
        public static long GnDrg열외군금액_Bon  = 0;                //2015-03-02
        public static long GnDrg열외군선별      = 0;                //2015-03-02
        
        public static long Gn응급가산수가       = 0;                //2016-07-01
        public static long Gn응급가산수가_Bon   = 0;                //2016-07-01
        public static long Gn응급가산수가_Jhp   = 0;                //2016-07-01
        public static long GnPCA                = 0;                //2018-09-20
        public static long GnPCA_Bon            = 0;                //2018-09-20
        public static long GnPCA_Jhp            = 0;                //2018-09-20
        public static long Gn재왕절개수가       = 0;                //2016-07-01
        public static long Gn재왕절개수가_Bon   = 0;                //2016-07-01
        public static long Gn재왕절개수가_Jhp   = 0;                //2016-07-01
        public static long GnDrg간호간병료      = 0;                //2016-08-01
        public static long GnDrg간호간병료_Bon  = 0;                //2016-08-01
        public static long GnDrg간호간병료_Jhp  = 0;                //2016-08-01
        public static long GnDrg간호간병료H     = 0;                //2인실간병료 추가
        public static long GnDrgSono            = 0;                //2016-10-24
        public static long GnDrgSono_Bon        = 0;                //2016-10-24
        public static long GnDrgSono_Jhp        = 0;                //2016-10-24
               
        public static int Gn재왕절개본인부담율  = 0;                //2016-07-01
               
        public static string GstrOgAdd          = string.Empty;     //산과가산여부
        public static string GstrDrgView        = string.Empty;     //DRG 금액조회 여부
        public static string GstrDRG_Info       = string.Empty;
        public static string GstrPCode          = string.Empty;     //2016-07-01

        public static long GnJSimDrgAmt         = 0;                //DRG 심사시 DRG 확인금액
        public static long GnJSimDrgBAmt        = 0;                //DRG 심사시 DRG 확인금액(본인)

        public static double FnQty              = 0;
        public static int FnNal                 = 0;
        public static int FnAge                 = 0;
        public static string FstrBun            = string.Empty;
        public static string FstrGbChild        = string.Empty;
        public static string FstrGisul          = string.Empty;
        public static string FstrSuNext         = string.Empty;
        public static string FstrNgt            = string.Empty;
        public static string FstrSugbAC         = string.Empty; 
        public static string FstrSugbAD         = string.Empty; 
        public static string FstrGbSGADD        = string.Empty; 
        #endregion

        /// <summary>
        /// DRG SETTING 시 HISTORY 관리
        /// author : 김민철
        /// Create Date : 2018-01-23
        /// <seealso cref="DRG.bas : INSERT_DRG_HISTORY"/> 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgTrsNo"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgDrgCode"></param>
        /// <param name="ArgDrgADC1"></param>
        /// <param name="ArgDrgADC2"></param>
        /// <param name="ArgDrgADC3"></param>
        /// <param name="ArgDrgADC4"></param>
        /// <param name="ArgDrgADC5"></param>
        /// <param name="ArgJob"></param>
        public void Insert_Drg_History(PsmhDb pDbCon, long ArgTrsNo, long ArgIpdNo, string ArgDrgCode, string ArgDrgADC1, string ArgDrgADC2, string ArgDrgADC3, string ArgDrgADC4, string ArgDrgADC5, string ArgJob)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0; //변경된 Row 받는 변수

            
            clsDB.setBeginTran(pDbCon);

            try
            {

                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "DRG_SET_HISTORY ";
                SQL += ComNum.VBLF + "  (TRSNO,IPDNO,PANO,SNAME,GBSTS,DRGCODE,DRGADC1,DRGADC2,DRGADC3,DRGADC4,";
                SQL += ComNum.VBLF + "  DRGADC5 , ENTDATE, ENTSABUN ) VALUES ";
                SQL += ComNum.VBLF + "  (" + ArgTrsNo + "," + ArgIpdNo + ",'" + clsPmpaType.IMST.Pano + "','" + clsPmpaType.IMST.Sname + "',";
                SQL += ComNum.VBLF + " '" + ArgJob + "','" + ArgDrgCode + "','" + ArgDrgADC1 + "','" + ArgDrgADC2 + "',";
                SQL += ComNum.VBLF + " '" + ArgDrgADC3 + "','" + ArgDrgADC4 + "','" + ArgDrgADC5 + "',SYSDATE," + clsPublic.GnJobSabun + ")";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                clsDB.setCommitTran(pDbCon);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                clsDB.setRollbackTran(pDbCon);
            }
        }

        /// <summary>
        /// 퇴원취소 당일발생 SLIP은 DELETE 하기 위한 함수
        /// <param name="ArgPano">등록번호</param>
        /// <param name="ArgIpdNo">입원번호</param>
        /// <param name="ArgTRSNO">자격번호</param>
        /// <param name="ArgActdate">회계일자</param>
        /// 2017-07-11 김민철
        /// </summary>
        /// <history> IPD_NEW_SLIP을 직접삭제 하지 않고 (-) Slip을 추가함 => 다른 로직으로 치환됨 </history>
        //public void IPD_DRG_SLIP_DELETE(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, long ArgTRSNO, string ArgActdate)
        //{
        //    DataTable Dt = new DataTable();
        //    string SQL = string.Empty;
        //    string SqlErr = string.Empty;
        //    int intRowCnt = 0;

            
        //    clsDB.setBeginTran(pDbCon);

        //    ComFunc.ReadSysDate(pDbCon);

        //    //당일건은 DRG발생건 삭제처리
        //    SQL = "";
        //    SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
        //    SQL += ComNum.VBLF + "  WHERE TRSNO = " + ArgTRSNO + " ";
        //    SQL += ComNum.VBLF + "    AND IPDNO = " + ArgIpdNo + " ";
        //    SQL += ComNum.VBLF + "    AND PANO  = '" + ArgPano + "' ";
        //    SQL += ComNum.VBLF + "    AND ActDate = TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ";
        //    SQL += ComNum.VBLF + "    AND SuNext IN ('DRG001','DRG002') ";
        //    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

        //    if (SqlErr != "")
        //    {
        //        clsDB.setRollbackTran(pDbCon);
        //        ComFunc.MsgBox(SqlErr);
        //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
        //        return;
        //    }

        //    clsDB.setCommitTran(pDbCon);

        //}

        /// <summary>
        /// author : 안정수
        /// Create Date : 2017-10-12
        /// <seealso cref="DRG.bas : Read_GbNgt_DRG"/>
        /// </summary>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTRSNO"></param>
        /// <returns></returns>
        public string Read_GbNgt_DRG(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, long ArgTRSNO)
        {
            string rtnVal = "";
            int i = 0;
            string strNgt = "";
            long nAmt = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            nAmt = 0;
            rtnVal = "0";

            //마취야간 구분 확인
            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  GbNgt,BUN,SUM(QTY+NAL) SQTY, SUM(AMT1+AMT2) AMT";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND PANO = '" + ArgPano + "'";
            SQL += ComNum.VBLF + "      AND IPDNO = " + ArgIpdNo + " ";
            SQL += ComNum.VBLF + "      AND TRSNO = " + ArgTRSNO + "";
            SQL += ComNum.VBLF + "      AND BUN IN ('22','23')";
            SQL += ComNum.VBLF + "      AND GBNGT IN ('1','2','4','5','6','7','8')";
            SQL += ComNum.VBLF + "GROUP BY GbNgt,BUN";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if(dt.Rows.Count > 0)
            {
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    strNgt = dt.Rows[i]["GbNgt"].ToString().Trim();

                    if(VB.Val(dt.Rows[i]["AMT"].ToString().Trim()) != 0)
                    {
                        if(strNgt == "1" || strNgt == "2" || strNgt == "4" || strNgt == "5" || strNgt == "7" || strNgt == "8")
                        {
                            nAmt += Convert.ToInt64(VB.Val(dt.Rows[i]["AMT"].ToString().Trim()));
                        }
                    }
                }
                if(nAmt > 0)
                {
                    rtnVal = "1";
                }
            }

            dt.Dispose();
            dt = null;

            //수술야간 구분 확인
            if(rtnVal == "0")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  GbNgt,BUN,SUM(NAL*QTY), SUM(AMT1+AMT2) AMT ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND PANO = '" + ArgPano + "' ";
                SQL += ComNum.VBLF + "      AND IPDNO = " + ArgIpdNo + "";
                SQL += ComNum.VBLF + "      AND TRSNO = " + ArgTRSNO + "";
                SQL += ComNum.VBLF + "      AND BUN ='34'";
                //SQL += ComNum.VBLF + "      AND GBNGT IN ('1','2','4','5','6','7','8','B','C')";
                SQL += ComNum.VBLF + "      AND GBNGT IN ('1','2','4','5','6','7','8','B','C','D')";    //심야추가 2018-08-30 KMC
                SQL += ComNum.VBLF + "GROUP BY GbNgt,BUN";
                SQL += ComNum.VBLF + "HAVING SUM(NAL*QTY) <> 0 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        strNgt = dt.Rows[i]["GbNgt"].ToString().Trim();

                        //if(strNgt == "1" || strNgt == "2")
                        if (strNgt == "1" || strNgt == "2" || strNgt == "D") //심야추가 2018-08-30 KMC
                        {
                            nAmt += Convert.ToInt64(VB.Val(dt.Rows[i]["AMT"].ToString().Trim()));
                        }
                    }

                    if(nAmt > 0)
                    {
                        rtnVal = "1";
                    }
                }

                dt.Dispose();
                dt = null;
            }

            return rtnVal;
        }

        /// <summary>
        /// author : 김민철
        /// Create Date : 2018-01-22
        /// <seealso cref="DRG.bas : Read_GbNgt_DRG_심야"/>
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTRSNO"></param>
        /// <returns></returns>
        public string Read_GbNgt_DRG_MidNgt(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, long ArgTRSNO)
        {
            string rtnVal = "";
            int i = 0;
            string strNgt = "";
            long nAmt = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            nAmt = 0;
            rtnVal = "0";

            //마취야간 구분 확인
            SQL = "";
            SQL += ComNum.VBLF + "SELECT GbNgt,BUN,SUM(QTY+NAL) SQTY, SUM(AMT1+AMT2) AMT ";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP";
            SQL += ComNum.VBLF + " WHERE 1 = 1";
            SQL += ComNum.VBLF + "   AND PANO = '" + ArgPano + "' ";
            SQL += ComNum.VBLF + "   AND IPDNO = " + ArgIpdNo + " ";
            SQL += ComNum.VBLF + "   AND TRSNO = " + ArgTRSNO + " ";
            SQL += ComNum.VBLF + "   AND BUN IN ('22','23') ";
            SQL += ComNum.VBLF + "   AND GBNGT = 'D' ";
            SQL += ComNum.VBLF + " GROUP BY GbNgt, BUN ";
            SQL += ComNum.VBLF + "HAVING SUM(NAL * QTY) <> 0 ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strNgt = dt.Rows[i]["GbNgt"].ToString().Trim();

                    if (VB.Val(dt.Rows[i]["AMT"].ToString().Trim()) != 0)
                    {
                        if (strNgt == "D")
                        {
                            nAmt += Convert.ToInt64(VB.Val(dt.Rows[i]["AMT"].ToString().Trim()));
                        }
                    }
                }
                if (nAmt > 0)
                {
                    rtnVal = "1";
                }
            }

            dt.Dispose();
            dt = null;

            //수술야간 구분 확인
            if (rtnVal == "0")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT GbNgt, BUN, SUM(NAL*QTY), SUM(AMT1+AMT2) AMT ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND PANO = '" + ArgPano + "' ";
                SQL += ComNum.VBLF + "    AND IPDNO = " + ArgIpdNo + " ";
                SQL += ComNum.VBLF + "    AND TRSNO = " + ArgTRSNO + " ";
                SQL += ComNum.VBLF + "    AND BUN = '34' ";
                SQL += ComNum.VBLF + "    AND GBNGT = 'D' ";
                SQL += ComNum.VBLF + "  GROUP BY GbNgt, BUN ";
                SQL += ComNum.VBLF + " HAVING SUM(NAL*QTY) <> 0 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strNgt = dt.Rows[i]["GbNgt"].ToString().Trim();

                        if (strNgt == "D")
                        {
                            nAmt += Convert.ToInt64(VB.Val(dt.Rows[i]["AMT"].ToString().Trim()));
                        }
                    }

                    if (nAmt > 0)
                    {
                        rtnVal = "1";
                    }
                }

                dt.Dispose();
                dt = null;
            }

            return rtnVal;
        }

        /// <summary>
        /// author : 김민철
        /// Create Date : 2018-01-23
        /// <seealso cref="DRG.bas : READ_DRG_AMT_MASTER"/>
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgDRGCode"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTRSNO"></param>
        /// <param name="ArgNgt"></param>
        /// <param name="ArgInDate"></param>
        /// <param name="ArgOutDate"></param>
        /// <returns></returns>
        public bool READ_DRG_AMT_MASTER(PsmhDb pDbCon, string ArgDRGCode, string ArgPano, long ArgIpdNo, long ArgTRSNO, string ArgNgt, string ArgInDate, string ArgOutDate)
        {
            #region //변수 선언부
            string strDept      = string.Empty;
            string strDRG_STS   = string.Empty;
            string strBDate     = string.Empty;
            string strSuNext    = string.Empty;
            string strGbNgt     = string.Empty;
            string strBun       = string.Empty;
            string strGisul     = string.Empty;
            string strGbChild   = string.Empty;
            string strNgt       = string.Empty;
            string strSugbAD    = string.Empty;
            string strGbSgAdd   = string.Empty;
            string strSugbAC    = string.Empty;
            string strSuGbAB    = string.Empty;
            string strER        = string.Empty;
            string strSuGbAA    = string.Empty;
            string strSugba     = string.Empty;
            string strRmCls     = string.Empty;
            string str추가입원  = string.Empty;
            string strJuminNo   = string.Empty;
            string strChild     = string.Empty;
            string strMCode     = string.Empty;
            string strOgPdBun   = string.Empty;
            string[] strTrsDate = new string[3];
            string strOGadd     = string.Empty;
            string strOpGbn     = string.Empty;

            double nDrgJumsu        = 0;    //종합병원 상대가치점수
            double nDrgOgJumsu      = 0;    //산과가산 점수(종합병원)
            double nDrgGobi         = 0;    //고정비율
            double nDrgNgt          = 0;    //종합병원 약간,공휴 점수
            double nDrgDanga        = 0;    //점수당 단가
            double nDrgIlsu         = 0;
            double nDrg_Gesan       = 0;    //DRG 군별 금액 최종산정
            double nDrg_Gesan_Bon   = 0;    //DRG 본인일부부담금을 위한 총금액
            double nQty = 0, nSQNal = 0, nMQty = 0;
            double nAgeDay = 0;
            double nGIJUMSUM = 0;           //2020-01-01 추가 기준점수

            int nNal = 0, nIlsu = 0, i = 0, nRead = 0, nCNT = 0;
            int nDrgIlsuMin = 0, nDrgIlsuMax = 0, nAge = 0;
            
            long nGsAddSuga = 0;
            long nABAmt = 0, nABAmt1 = 0, nABAmt2 = 0, nABAmt3 = 0, nABAmt4 = 0;
            
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            bool rtnVal = false;
            
            ComFunc CF          = new ComFunc();
            clsIument cIMT      = new clsIument();
            clsIuSentChk cISCHK = new clsIuSentChk();
            clsBasAcct cBACCT   = new clsBasAcct();
            clsPmpaFunc cPF     = new clsPmpaFunc();
            clsIpdAcct cIAcct   = new clsIpdAcct();
            
            clsPmpaType.cBas_Add_Arg cBArg  = null;
            clsPmpaType.Bas_Acc_Rtn cBAR    = new clsPmpaType.Bas_Acc_Rtn();
            clsPmpaType.BonRate cBON        = new clsPmpaType.BonRate();
            #endregion

            #region //변수 Clear
            GnDRG_Amt1 = 0;
            GnDRG_Amt2 = 0;
            GnDRG_TAmt = 0;
            GnDRG_TBonAmt = 0;
            GnDRG_WBonAmt = 0;
            GnGsAddAmt = 0;              //외과가산
            GnGs100Amt = 0;              //100/100
                
            GnGs80Amt_T = 0; GnGs80Amt_J = 0; GnGs80Amt_B = 0;   //2015-10-21
            GnGs90Amt_T = 0; GnGs90Amt_J = 0; GnGs90Amt_B = 0;   //2018-01-01
            GnGs50Amt_T = 0; GnGs50Amt_J = 0; GnGs50Amt_B = 0;   //2015-10-21
            
            Gn복강개복Amt = 0;           //복강개복 수가
            GnOTChaAmt = 0;           //조절성인공수정체 사용시 인공수정체 제외

            GnDrgBiFAmt = 0;             //DRG 비급여
            GnDRG_OGAddAmt = 0;          //산과가산 금액
            GstrOgAdd = "";              //산과가산여부
            GnDrg추가입원료 = 0;         //추가입원료 2014-08-28
            GnDrg추가입원료_Bon = 0;     //추가입원료 2014-08-28
            GnDrg추가입원료_Jhp = 0;     //추가입원료 2014-08-28
            GnDrg추가입원료_Tbed = 0;     //2인실추가입원료 총액 2018-07-01
            GnDrg추가입원료_Bbed = 0;     //2인실추가입원료 추가 입원료 2인실 40%본인2018-07-01
            GnDrg추가입원료_Nbed = 0;     //2인실추가입원료 기본입원료 2018-07-01
            GnDrgJinAmt = 0;             //의료질평가지원금 2015-08-31
            GnDrgJinAmt_Bon = 0;         //의료질평가지원금 2015-08-31(본인)
            GnDrgJinAmt_Jhp = 0;         //의료질평가지원금 2015-08-31(조합)

            GnDrgJinSAmt = 0;             //안전관리지원금 2018-01-01
            GnDrgJinSAmt_Bon = 0;         //안전관리지원금 2018-01-01(본인)
            GnDrgJinSAmt_Jhp = 0;         //안전관리지원금 2018-01-01(조합)
            
            GnDrg지원금 = 0;             //2014-11-24
            GnDrg부수술총액 = 0;         //2015-02-03
            GnDrg부수술총액_Bon = 0;     //2015-02-03
            GnDrg부수술총액_Jhp = 0;     //2015-02-03
            Gn행위별총액 = 0;            //2015-03-02
            GnDrg열외군금액 = 0;         //2015-03-02
            GnDrg열외군금액_Bon = 0;     //2015-03-02
            GnDrg열외군선별 = 0;         //2015-03-02
            Gn응급가산수가 = 0;          //2016-07-01
            Gn응급가산수가_Bon = 0;      //2016-07-01
            Gn응급가산수가_Jhp = 0;      //2016-07-01
            GnPCA = 0;                   //2018-09-20
            GnPCA_Bon = 0;               //2018-09-20
            GnPCA_Jhp = 0;               //2018-09-20
            Gn재왕절개수가 = 0;          //2016-07-01
            Gn재왕절개수가_Bon = 0;      //2016-07-01
            Gn재왕절개수가_Jhp = 0;      //2016-07-01
            GnDrg간호간병료 = 0;         //2016-08-01
            GnDrg간호간병료_Bon = 0;     //2016-08-01
            GnDrg간호간병료_Jhp = 0;     //2016-08-01
            GnDrg간호간병료H = 0;        //2016-08-01
            GnOTChaAmt = 0;         //2016-08-01
            GnOTChaAmt_Bon = 0;     //2016-08-01
            GnOTChaAmt_Jhp = 0;     //2016-08-01
            GnDrgADDAmt = 0;         //2016-08-01
            GnDrgADDAmt_Bon = 0;     //2016-08-01
            GnDrgADDAmt_Jhp = 0;     //2016-08-01
            GnDrgSono = 0;               //2016-10-24
            GnDrgSono_Bon = 0;           //2016-10-24
            GnDrgSono_Jhp = 0;           //2016-10-24
            Gn재왕절개본인부담율 = 0;    //2018-08-28
            clsPmpaPb.GnHRoomAmt = 0;
            clsPmpaPb.GnHRoomBonin = 0;
            clsPmpaPb.GnH1RoomAmt = 0;
            clsPmpaPb.GnHUSetAmt = 0;
            clsPmpaPb.GnHUSetBonin = 0;

            nABAmt1 = 0; nABAmt2 = 0; nABAmt3 = 0; nABAmt4 = 0;  //2014-08-28
            
            GstrDRG_Info = "";
            #endregion

            if (string.Compare(ArgInDate, "2013-07-01") < 0)
            {
                return rtnVal;
            }

            cIMT.Read_Ipd_Mst_Trans(pDbCon, ArgPano, ArgTRSNO, "");

            #region //입원 본인부담율 세팅
            //기준일자 세팅
            strBDate    = clsPmpaType.TIT.InDate;
            strJuminNo  = clsPmpaType.TIT.Jumin1 + clsPmpaType.TIT.Jumin3;

            //나이구분 체크
            
            //strMCode    = cPF.Rtn_Ipd_OgPdBunDtl(clsPmpaType.TIT.OgPdBun);
            //strOgPdBun  = clsPmpaType.TIT.OgPdBundtl;
            strMCode = cPF.Rtn_Ipd_OgPdBunDtl(clsPmpaType.TIT.OgPdBun);
            if (cPF.Rtn_Ipd_OgPdBunDtl(clsPmpaType.TIT.OgPdBun) == "")
            {
                strOgPdBun = clsPmpaType.TIT.OgPdBun;
            }
            else
            {
                strOgPdBun = clsPmpaType.TIT.OgPdBundtl;
            }
            strDept     = clsPmpaType.TIT.DeptCode;

            //부담율세팅 기초정보 
            cBON.BI      = clsPmpaType.TIT.Bi;
            cBON.IO      = "I";
            strChild = cPF.Acct_Age_Gubun(clsPmpaType.TIT.Age, strJuminNo, strBDate, cBON.IO);
            cBON.CHILD   = strChild;
            if (clsPmpaType.TIT.OgPdBun == "1")
            {
                cBON.MCODE = "E000";
                cBON.VCODE = "EV00";
            }
            else if (clsPmpaType.TIT.OgPdBun == "2")
            {
                cBON.MCODE = "F000";
                cBON.VCODE = "EV00";
            }
            else
            {
                cBON.MCODE = strMCode;
                cBON.VCODE = clsPmpaType.TIT.VCode;
            }
            //cBON.MCODE   = strMCode;
            //cBON.VCODE   = clsPmpaType.TIT.VCode;
            cBON.OGPDBUN = strOgPdBun;
            cBON.FCODE   = clsPmpaType.TIT.FCode;
            cBON.SDATE   = strBDate;
            cBON.DEPT    = strDept;

            //기본 부담율 계산
            if (cIAcct.Read_IBon_Rate(pDbCon, cBON) == false)
            {
                clsAlert cA = new clsAlert();
                cA.Alert_BonRate(cBON);
                return rtnVal;
            }

            if (clsPmpaType.TIT.Bohun == "3")   //장애인은 본인부담율 재조정 
            {
                if ((cBON.BI == "11" && cBON.MCODE == "F000") || cBON.BI == "22")
                {
                    clsPmpaType.IBR.Jin = 0;
                    clsPmpaType.IBR.Bohum = 0;
                    clsPmpaType.IBR.CTMRI = 0;
                }
            }
            #endregion

            nIlsu = CF.DATE_ILSU(pDbCon, ArgOutDate, ArgInDate) + 1;

            //2016-07-01 재왕절개 본인부담율
            

            if (ArgDRGCode == "O01600" || ArgDRGCode == "O01601" || ArgDRGCode == "O01602" || ArgDRGCode == "O01603" || ArgDRGCode == "O01700" || ArgDRGCode == "O01701" || ArgDRGCode == "O01702")
            {
                Gn재왕절개본인부담율 = 5;
            }
            //PCA 관련 본인부담율 적용 로직 추가 DRG코드에 상관없이 @V193이면 본인부담 5% 해당되도록 보완 요청(2021-07-06)
            //참고 : 케이스가 잘 없기 때문에 아래 주석달린 환자만 테스트해서 계산 완료한 상태임. 다른 환자 문제 시 해당 루틴 주석 처리 요망.
            if (clsPmpaType.TIT.VCode == "V193")
            //if (clsPmpaType.TIT.Trsno == 1507297 && clsPmpaType.TIT.Pano == "07603123")
            {
                Gn재왕절개본인부담율 = 5;
            }

            #region 산과가산 관련 세팅
            SQL = "";
            SQL += " SELECT a.Sunext                              \r\n";
            SQL += "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP A, \r\n";
            SQL += "        " + ComNum.DB_PMPA + "BAS_SUN b       \r\n";
            SQL += "  WHERE a.Sunext=b.Sunext(+)                  \r\n";
            SQL += "    AND a.TRSNO = " + ArgTRSNO + "            \r\n";
            SQL += "    AND b.DrgOGAdd ='Y'                       \r\n";
            SQL += "  Group By a.Sunext                           \r\n";
            SQL += "  Having SUM(a.Qty*a.Nal) > 0                     ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (Dt.Rows.Count > 0)
                strOGadd = "Y";

            Dt.Dispose();
            Dt = null;

            //'가산 항목이지만 DRG가산 코드가 아니면 미가산코드로 변경해준다
            if (strOGadd == "Y" && string.Compare(ArgInDate, "2018-03-01") >= 0)
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(DDATE,'YYYY-MM-DD') DDATE,                              \r\n";
                SQL += "        DCODE,DNAME,DJUMSUS,DJUMSUM, DJUMSU,DJUMSUL, DGOBI, DILSU_AV,   \r\n";
                SQL += "        DILSU_MIN, DILSU_MAX, DJUMDANGA, DJUMDANGAL, DHJUMSUS,          \r\n";
                SQL += "        DHJUMSUM, DHJUMSU, DHJUMSUL, GBOGADD,                           \r\n";
                SQL += "        DOJUMSUS, DOJUMSUM, DOJUMSU, DOJUMSUL                           \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "DRG_CODE_NEW_ADD                          \r\n";
                SQL += "  WHERE DCODE = '" + ArgDRGCode + "'                                    \r\n";
                SQL += "    AND DDATE <=TO_DATE('" + ArgInDate + "','YYYY-MM-DD')               \r\n";
                SQL += "  ORDER BY DDATE DESC                                                       ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (Dt.Rows.Count == 0)
                    strOGadd = "";

                Dt.Dispose();
                Dt = null;
            } 
            #endregion

            #region DRG CODE 별 점수정보 읽기
            strDRG_STS = "1";       //1.정상군  2.하단열외군, 3.상단열외군
            
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(DDATE,'YYYY-MM-DD') DDATE, ";
            SQL += ComNum.VBLF + "        DCODE,DNAME,DJUMSUS,DJUMSUM, DJUMSU,DJUMSUL, DGOBI, DILSU_AV, DILSU_MIN, DILSU_MAX,";
            SQL += ComNum.VBLF + "        DJUMDANGA, DJUMDANGAL, DHJUMSUS, DHJUMSUM, DHJUMSU, DHJUMSUL, GBOGADD, ";
            SQL += ComNum.VBLF + "        DOJUMSUS, DOJUMSUM, DOJUMSU, DOJUMSUL,GIJUMSUM";
            //가산 항목은 N%로 시작 예외코드 O%가 있어서 조건 추가 2018-03-15 add
            if (strOGadd == "Y" && ArgDRGCode.Substring(0, 1) == "N" && string.Compare(ArgInDate, "2018-03-01") >= 0)
                SQL += ComNum.VBLF + "        FROM " + ComNum.DB_PMPA + "DRG_CODE_NEW_ADD ";
            else
                SQL += ComNum.VBLF + "        FROM " + ComNum.DB_PMPA + "DRG_CODE_NEW ";
            SQL += ComNum.VBLF + "  WHERE DCODE ='" + ArgDRGCode + "' ";
            SQL += ComNum.VBLF + "    AND DDATE <=TO_DATE('" + ArgInDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  ORDER BY DDATE DESC   ";

            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (Dt.Rows.Count > 0)
            {
                rtnVal = true;

                nDrgJumsu = Convert.ToDouble(VB.Val(Dt.Rows[0]["DJUMSUM"].ToString()));    //점수
                nDrgOgJumsu = Convert.ToDouble(VB.Val(Dt.Rows[0]["DOJUMSUM"].ToString()));   //산과가산 점수
                nDrgGobi = Convert.ToDouble(VB.Val(Dt.Rows[0]["DGOBI"].ToString()));      //고정비율
                nDrgNgt = Convert.ToDouble(VB.Val(Dt.Rows[0]["DHJUMSUM"].ToString()));   //야간,공휴점수
                nDrgDanga = Convert.ToDouble(VB.Val(Dt.Rows[0]["DJUMDANGA"].ToString()));  //점수단가
                nDrgIlsu = Convert.ToDouble(VB.Val(Dt.Rows[0]["DILSU_AV"].ToString()));
                nDrgIlsuMin = Convert.ToInt16(VB.Val(Dt.Rows[0]["DILSU_MIN"].ToString()));
                nDrgIlsuMax = Convert.ToInt16(VB.Val(Dt.Rows[0]["DILSU_MAX"].ToString()));
                nGIJUMSUM = Convert.ToDouble(VB.Val(Dt.Rows[0]["GIJUMSUM"].ToString()));


                GstrOgAdd = Dt.Rows[0]["GBOGADD"].ToString().Trim(); //산과가산여부
                
                //정상군체크
                if (nDrgIlsuMin > nIlsu)
                { 
                    //하단열외군
                    strDRG_STS = "2";
                }
                else if (nDrgIlsuMax < nIlsu)
                { 
                    //상단열외군
                    strDRG_STS = "3";
                }
            }
            
            Dt.Dispose();
            Dt = null;
            #endregion

            #region //산과 추가가산 로직(DRG_CODE_NEW에서 OG가산 여부를 이용해야됨)
            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.Sunext                                           ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP A,              ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b                    ";
            SQL += ComNum.VBLF + "  WHERE a.Sunext = b.Sunext(+)                             ";
            SQL += ComNum.VBLF + "    AND a.TRSNO = " + ArgTRSNO + "                         ";
            SQL += ComNum.VBLF + "    AND b.DrgOGAdd ='Y'                                    ";
            SQL += ComNum.VBLF + "  Group By a.Sunext                                        ";
            SQL += ComNum.VBLF + " Having SUM(a.Qty*a.Nal) > 0                               ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (Dt.Rows.Count > 0)
            { 
                //산과가산이 있으면 금액을 계산하지 않고 테이블에서 직접 가져옴...
                //2017-01-01 이후 입원자는 산과가산 테이블을 직접 읽음(이미 계산된 점수임)

                if (GstrOgAdd == "1")
                {
                    if (string.Compare(ArgInDate, "2018-03-01") >= 0)
                    {
                        nDrgJumsu = nDrgJumsu;
                    }
                    else if (string.Compare(ArgInDate, "2017-01-01") >= 0)
                    {
                        nDrgJumsu = nDrgOgJumsu;
                    }
                    else
                    {
                        nDrgJumsu = nDrgJumsu + nDrgOgJumsu;
                    }
                }
            }

            Dt.Dispose();
            Dt = null;
            #endregion

            #region DRG 점수별 단가 계산식
            if (rtnVal == true)
            {
                #region DRG 점수별 단가 계산식  2020.1.1 개정
                if (string.Compare(ArgInDate, "2020-01-01") >= 0)
                {
                    if (strDRG_STS == "1")
                    {
                        //정상군
                        nDrg_Gesan = 0;

                        if (ArgNgt != "1")
                        {
                            // [ {질병군별 점수 상대기준} + { 가입자 입원일수 - 질병군별 평균 입원일수 } ] x 일당 상대가치점수
                            nDrg_Gesan = (nDrgJumsu + (nIlsu - nDrgIlsu) * nGIJUMSUM) * 0.2 + (nDrgJumsu * 0.8);
                            nDrg_Gesan_Bon = (nDrgJumsu + (nIlsu - nDrgIlsu) * nGIJUMSUM);
                        }
                        else
                        {
                            nDrg_Gesan = (((nIlsu - nDrgIlsu) * nGIJUMSUM + nDrgJumsu) + nDrgNgt) * 0.2 + ((nDrgJumsu + nDrgNgt) * 0.8);
                            nDrg_Gesan_Bon = (((nIlsu - nDrgIlsu) * nGIJUMSUM + nDrgJumsu) + nDrgNgt);

                        }
                    }
                    else if (strDRG_STS == "2")
                    {
                        //하단
                        nDrg_Gesan = 0;

                        if (ArgNgt != "1")
                        {
                            // [ {질병군별 점수 상대기준} + { 가입자 입원일수 - 질병군별 평균 입원일수 } ] x 일당 상대가치점수
                            nDrg_Gesan = (nDrgJumsu + (nIlsu - nDrgIlsu) * nGIJUMSUM) * 0.2;
                            nDrg_Gesan = nDrg_Gesan + (nDrgJumsu - ((nDrgIlsuMin - nIlsu) * nGIJUMSUM)) * 0.8;
                            nDrg_Gesan_Bon = ((nIlsu - nDrgIlsu) * nGIJUMSUM + nDrgJumsu);
                        }
                        else
                        {
                            nDrg_Gesan = ((nDrgJumsu + (nIlsu - nDrgIlsu) * nGIJUMSUM) + nDrgNgt) * 0.2;
                            nDrg_Gesan = nDrg_Gesan + (nDrgJumsu - (((nDrgIlsuMin - nIlsu) * nGIJUMSUM) + nDrgNgt)) * 0.8;
                            nDrg_Gesan_Bon = (nDrgJumsu + (nIlsu - nDrgIlsu) * nGIJUMSUM) + nDrgNgt;
                        }
                    }
                    else if (strDRG_STS == "3")
                    {
                        //상단
                        nDrg_Gesan = 0;
                        
                        if (ArgNgt != "1")
                        {
                            // [ {질병군별 점수 상대기준} + { 가입자 입원일수 - 질병군별 평균 입원일수 } ] x 일당 상대가치점수

                            nDrg_Gesan = (nDrgJumsu + (nIlsu - nDrgIlsu) * nGIJUMSUM) * 0.2;
                            nDrg_Gesan = nDrg_Gesan + (nDrgJumsu + ((nIlsu - nDrgIlsuMax) * nGIJUMSUM)) * 0.8;
                            nDrg_Gesan_Bon = (nDrgJumsu + (nIlsu - nDrgIlsu) * nGIJUMSUM);
                        }
                        else
                        {
                            nDrg_Gesan = ((nDrgJumsu + (nIlsu - nDrgIlsu) * nGIJUMSUM) + nDrgNgt) * 0.2;
                            nDrg_Gesan = nDrg_Gesan + (nDrgJumsu + (((nIlsu - nDrgIlsuMax) * nGIJUMSUM) + nDrgNgt)) * 0.8;
                            nDrg_Gesan_Bon = (nDrgJumsu + (nIlsu - nDrgIlsu) * nGIJUMSUM) + nDrgNgt;
                        }
                    }
                }
                else
                {
                    if (strDRG_STS == "1")
                    {
                        //정상군
                        nDrg_Gesan = 0;

                        if (ArgNgt != "1")
                        {
                            //[{질병군별 점수x고정비율} + {질병군별 점수x (1-고정비율)x가입자 입원일수/질병군별 평균 입원일수 }] x 20/100 + [ 질병군별 점수 ] x 80/100
                            nDrg_Gesan = ((nDrgJumsu * nDrgGobi) + (nDrgJumsu * (1 - nDrgGobi) * nIlsu / nDrgIlsu)) * 0.2 + (nDrgJumsu * 0.8);
                            nDrg_Gesan_Bon = (nDrgJumsu * nDrgGobi) + (nDrgJumsu * (1 - nDrgGobi) * nIlsu / nDrgIlsu);
                        }
                        else
                        {
                            nDrg_Gesan = ((nDrgJumsu * nDrgGobi) + (nDrgJumsu * (1 - nDrgGobi) * nIlsu / nDrgIlsu) + nDrgNgt) * 0.2 + (nDrgJumsu + nDrgNgt) * 0.8;
                            nDrg_Gesan_Bon = ((nDrgJumsu * nDrgGobi) + (nDrgJumsu * (1 - nDrgGobi) * nIlsu / nDrgIlsu) + nDrgNgt);
                        }
                    }
                    else if (strDRG_STS == "2")
                    {
                        //하단
                        nDrg_Gesan = 0;

                        if (ArgNgt != "1")
                        {
                            //[{질병군별 점수x고정비율} + {질병군별 점수x (1-고정비율)x가입자 입원일수/질병군별 평균 입원일수 }] x 20/100
                            // + [ {질병군별 점수 x 고정비율}+{질병군별 점수 x (1-고정비율 ) x 가입자 입원일수 /질병군별 정상군 하한 입원일수 } ] x 80/100
                            nDrg_Gesan = ((nDrgJumsu * nDrgGobi) + (nDrgJumsu * (1 - nDrgGobi) * nIlsu / nDrgIlsu)) * 0.2;
                            nDrg_Gesan = nDrg_Gesan + ((nDrgJumsu * nDrgGobi) + (nDrgJumsu * (1 - nDrgGobi) * nIlsu) / nDrgIlsuMin) * 0.8;
                            nDrg_Gesan_Bon = (nDrgJumsu * nDrgGobi) + (nDrgJumsu * (1 - nDrgGobi) * nIlsu / nDrgIlsu);
                        }
                        else
                        {
                            nDrg_Gesan = ((nDrgJumsu * nDrgGobi) + (nDrgJumsu * (1 - nDrgGobi) * nIlsu / nDrgIlsu) + nDrgNgt) * 0.2;
                            nDrg_Gesan = nDrg_Gesan + ((nDrgJumsu * nDrgGobi) + (nDrgJumsu * (1 - nDrgGobi) * nIlsu) / nDrgIlsuMin + nDrgNgt) * 0.8;
                            nDrg_Gesan_Bon = ((nDrgJumsu * nDrgGobi) + (nDrgJumsu * (1 - nDrgGobi) * nIlsu / nDrgIlsu) + nDrgNgt);
                        }
                    }
                    else if (strDRG_STS == "3")
                    {
                        //상단
                        nDrg_Gesan = 0;

                        if (ArgNgt != "1")
                        {
                            // [ {질병군별 점수x고정비율} + {질병군별 점수 x (1-고정비율) x가입자 입원일수 / 질병군별 평균 입원일수 } ] x 20/100
                            // + [ {질병군별 점수} +{질병군별 점수 x (1-고정비율)x (가입자등의 입원일수 - 질병군별 정산군 상한 입원일수) /질병군별 평균 입원일수 x 적용률 } ] x 80/100
                            nDrg_Gesan = ((nDrgJumsu * nDrgGobi) + (nDrgJumsu * (1 - nDrgGobi) * nIlsu / nDrgIlsu)) * 0.2;
                            nDrg_Gesan = nDrg_Gesan + (nDrgJumsu + (nDrgJumsu * (1 - nDrgGobi) * (nIlsu - nDrgIlsuMax) / nDrgIlsu * 1)) * 0.8;
                            nDrg_Gesan_Bon = (nDrgJumsu * nDrgGobi) + (nDrgJumsu * (1 - nDrgGobi) * nIlsu / nDrgIlsu);
                        }
                        else
                        {
                            nDrg_Gesan = ((nDrgJumsu * nDrgGobi) + (nDrgJumsu * (1 - nDrgGobi) * nIlsu / nDrgIlsu) + nDrgNgt) * 0.2;
                            nDrg_Gesan = nDrg_Gesan + (nDrgJumsu + (nDrgJumsu * (1 - nDrgGobi) * (nIlsu - nDrgIlsuMax) / nDrgIlsu * 1) + nDrgNgt) * 0.8;
                            nDrg_Gesan_Bon = ((nDrgJumsu * nDrgGobi) + (nDrgJumsu * (1 - nDrgGobi) * nIlsu / nDrgIlsu) + nDrgNgt);
                        }
                    }
                }
                

                //상대가치점수는 소수점 3째자리에서 사사오입
                nDrg_Gesan = Math.Round(nDrg_Gesan, 2);
                nDrg_Gesan_Bon = Math.Round(nDrg_Gesan_Bon, 2);

                #endregion

                #region //산과가산이 아닌경우만...
                if (GnDRG_OGAddAmt == 0)
                {
                    //10원 미만 절사
                    if (Read_GbNgt_DRG_MidNgt(pDbCon, ArgPano, ArgIpdNo, ArgTRSNO) == "1" && string.Compare(ArgInDate, "2017-01-01") >= 0)
                    {
                        GnDRG_Amt1    = (long)Math.Truncate(((nDrg_Gesan     * nDrgDanga) + (nDrgNgt * nDrgDanga)) / 10.0) * 10;                        
                        GnDRG_TBonAmt = (long)Math.Truncate(((nDrg_Gesan_Bon * nDrgDanga) + (nDrgNgt * nDrgDanga)) / 10.0) * 10;
                    }
                    else
                    {
                        GnDRG_Amt1    = (long)Math.Truncate((nDrg_Gesan     * nDrgDanga) / 10.0) * 10;
                        GnDRG_TBonAmt = (long)Math.Truncate((nDrg_Gesan_Bon * nDrgDanga) / 10.0) * 10;
                    }
                    
                    if (Gn재왕절개본인부담율 > 0)
                    { 
                        if (Read_GbNgt_DRG_MidNgt(pDbCon, ArgPano, ArgIpdNo, ArgTRSNO) == "1" && string.Compare(ArgInDate, "2017-01-01") >= 0)
                        {
                            GnDRG_WBonAmt = (long)Math.Truncate((((nDrg_Gesan_Bon * nDrgDanga) + (nDrgNgt * nDrgDanga)) * (Gn재왕절개본인부담율 / 100.0)) / 10) * 10;   //심야가산  test
                        }
                        else
                        {
                            GnDRG_WBonAmt = (long)Math.Truncate((nDrg_Gesan_Bon * nDrgDanga * (Gn재왕절개본인부담율 / 100.0)) / 10) * 10;
                        }
                    }
                    else
                    {
                        GnDRG_WBonAmt = (long)Math.Truncate((nDrg_Gesan_Bon * nDrgDanga * (clsPmpaType.IBR.Bohum / 100.0)) / 10) * 10; //2017-08-03 add
                    }
                }    
                else
                {
                    GnDRG_Amt1 = GnDRG_OGAddAmt;
                }

                GnDRG_Amt2 = GnDRG_Amt2 + GnDRG_Amt1;
                #endregion

                #region //DRG_INFO
                GstrDRG_Info =  " [DRG코드:" + ArgDRGCode + "]" + ComNum.VBLF + ComNum.VBLF;
                GstrDRG_Info += " 야간:" + ArgNgt + ComNum.VBLF + ComNum.VBLF;
                GstrDRG_Info += " 질병군별점수:" + nDrgJumsu + " 고정비율:" + nDrgGobi + ComNum.VBLF + ComNum.VBLF;
                GstrDRG_Info += " 입원일수:" + nIlsu + " 평균입원일수:" + nDrgIlsu + " 점수당 단가:" + nDrgDanga + ComNum.VBLF + ComNum.VBLF;
                GstrDRG_Info += " 질병군별 점수산정 결과 : " + VB.Format(nDrg_Gesan, "###,###,###,##0.00") + ComNum.VBLF;
                GstrDRG_Info += " 질병군별 점수산정 * 단가 결과 : " + VB.Format(nDrg_Gesan * nDrgDanga, "###,###,###,##0.00");
                #endregion

                #region //외과가산료(총액)  BAS_SUN  DrgAdd ='Y'
                //IPD_NEW_SLIP 에 주수술과 부수술 여부를 체크 (0:주수술, 1:부수술)
                nQty = 0;
                nGsAddSuga = 0;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.Sunext,a.GbNgt,SUM(a.Qty*a.Nal) NQty,   ";
                SQL += ComNum.VBLF + "        TO_CHAR(a.BDate,'YYYY-MM-DD') BDATE,      ";
                SQL += ComNum.VBLF + "        nvl(a.OPGUBUN,0)  OPGUBUN                               ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP A,     ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b           ";
                SQL += ComNum.VBLF + "  WHERE a.PANO = '" + ArgPano + "'                ";
                SQL += ComNum.VBLF + "    AND a.TRSNO = " + ArgTRSNO + "                ";
                SQL += ComNum.VBLF + "    AND a.Sunext=b.Sunext(+)                      ";
                SQL += ComNum.VBLF + "    AND b.DrgAdd ='Y'                             ";
                SQL += ComNum.VBLF + "  GROUP BY a.Sunext,a.GbNgt,a.BDate,nvl(a.OPGUBUN,0)     ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count > 0)
                {
                    nRead = Dt.Rows.Count;

                    if (nRead > 0)
                    {
                        for (i = 0; i < nRead; i++)
                        {
                            strBDate = Dt.Rows[i]["BDATE"].ToString().Trim();
                            strSuNext = Dt.Rows[i]["SUNEXT"].ToString().Trim();
                            strGbNgt = Dt.Rows[i]["GBNGT"].ToString().Trim();
                            strOpGbn = Dt.Rows[i]["OPGUBUN"].ToString().Trim();

                            nQty = Convert.ToDouble(Dt.Rows[i]["NQty"].ToString());
                            if (strSuNext == "Q275512B")
                            {
                                strSuNext = "Q2755";
                            }
                            //BAS_SUGA_DRGADD 수가에서 해당수가를 구함
                            //2014-07-24 BAS_SUGA_DRGADD 에서 제2의 수술일 경우 수가읽는것 확인해야함.
                            nGsAddSuga = 0;

                            if (nQty > 0)
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " SELECT SUCODE,SUNEXT,SUDATE,JUAMT,BUAMT,DUAMT,DELDATE,DELSABUN  ";
                                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUGA_DRGADD                    ";
                                SQL += ComNum.VBLF + "  Where SUNEXT='" + strSuNext + "'                               ";
                                SQL += ComNum.VBLF + "    AND SUDATE <= TO_DATE('" + strBDate + "','YYYY-MM-DD')       ";    //2014-07-24 적용일자 추가
                                SQL += ComNum.VBLF + "    AND DELDATE IS NULL                                          ";
                                SQL += ComNum.VBLF + "  ORDER By SUDATE DESC                                           ";
                                SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (Dt2.Rows.Count > 0)
                                {
                                    if (nQty <= 0.5 || strOpGbn == "2")       //nQTY가 0.5 이면 부수술로 인정
                                    {
                                        nGsAddSuga = Convert.ToInt64(VB.Val(Dt2.Rows[0]["BUAMT"].ToString()));       //부수술
                                    }
                                    else if ((nQty > 0.5 && nQty <= 0.7) || strOpGbn == "1")
                                    { 
                                        nGsAddSuga = Convert.ToInt64(VB.Val(Dt2.Rows[0]["DUAMT"].ToString()));       //동시수술 2014-07-24
                                    }
                                    else
                                    { 
                                        nGsAddSuga = Convert.ToInt64(VB.Val(Dt2.Rows[0]["JUAMT"].ToString()));       //주수술
                                        nGsAddSuga = (long)Math.Truncate(nGsAddSuga * nQty);
                                    }
                                }

                                Dt2.Dispose();
                                Dt2 = null;

                            }
                            GnGsAddAmt += nGsAddSuga;
                        }
                    }
                }

                GnDRG_Amt2 = GnDRG_Amt2 + GnGsAddAmt;            //DRG 총액 + 외과가산금액
                GnDRG_TBonAmt = GnDRG_TBonAmt + GnGsAddAmt;      //DRG 총액 + 외과가산금액

                Dt.Dispose();
                Dt = null;                
                #endregion

                #region //DRG 응급가산 수가 계산  2016-07-01
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.Sunext, SUM(a.Qty*a.Nal) SQNal, A.Qty SQTY, SUM(a.Nal) SNal,    ";
                SQL += ComNum.VBLF + "        b.SugbAD,a.gbsgadd,b.SugbAC,  a.Bun, a.GbGisul,a.GbChild,         ";   //SugbAC,b.SugbAD, add gbsgadd 2017-08-03  
                SQL += ComNum.VBLF + "        TO_CHAR(a.BDate,'YYYY-MM-DD') BDATE, a.GbNgt, a.GBER, a.GBSUGBAB, ";
                SQL += ComNum.VBLF + "        b.SugbAA,b.SugbAB                                                 ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,                             ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b                                   ";
                SQL += ComNum.VBLF + "  WHERE a.PANO = '" + ArgPano + "'                                        ";
                SQL += ComNum.VBLF + "    AND a.TRSNO = " + ArgTRSNO + "                                        ";
                SQL += ComNum.VBLF + "    AND a.Sunext=b.Sunext(+)                                              ";
                SQL += ComNum.VBLF + "    AND a.GbEr IN ('1','2','3')                                           ";
                SQL += ComNum.VBLF + "    AND b.SuGbAA IN ('2','3')                                             ";
                SQL += ComNum.VBLF + "  GROUP BY a.Sunext,a.BDate,a.Bun,a.GbGisul,a.GbChild, a.GbNgt ,A.Qty,    ";
                SQL += ComNum.VBLF + "           b.SugbAD,a.gbsgadd,b.SugbAC, a.GBER, a.GBSUGBAB, b.SugbAA,     ";   //b.SugbAC b.SugbAD, add gbsgadd 2017-08-03 
                SQL += ComNum.VBLF + "           b.SugbAB                                                       ";
                SQL += ComNum.VBLF + " HAVING SUM(A.Nal) <> 0                                                   ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                nRead = Dt.Rows.Count;

                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nSQNal     = Convert.ToDouble(Dt.Rows[i]["SQNal"].ToString()); 
                        nMQty      = Convert.ToDouble(Dt.Rows[i]["SQTY"].ToString());   
                        nNal       = Convert.ToInt16(Dt.Rows[i]["SNal"].ToString());
                        nAge       = clsPmpaType.TIT.Age;
                        nAgeDay    = clsPmpaType.TIT.AgeDays;
                        strBun     = Dt.Rows[i]["Bun"].ToString().Trim();         
                        strGisul   = Dt.Rows[i]["GbGisul"].ToString().Trim();     
                        strGbChild = Dt.Rows[i]["GbChild"].ToString().Trim();     
                        strNgt     = Dt.Rows[i]["GbNgt"].ToString().Trim();       
                        strSuNext  = Dt.Rows[i]["Sunext"].ToString().Trim();
                        strER      = Dt.Rows[i]["GBER"].ToString().Trim();     
                        strSugbAD  = Dt.Rows[i]["SugbAD"].ToString().Trim();      //2017-08-03 add
                        strGbSgAdd = Dt.Rows[i]["GBSGADD"].ToString().Trim();     //2017-08-03 add
                        strSugbAC  = Dt.Rows[i]["SugbAC"].ToString().Trim();      //2017-08-03 add
                        strSuGbAB  = Dt.Rows[i]["SugbAB"].ToString().Trim();
                        strSuGbAA  = Dt.Rows[i]["SugbAA"].ToString().Trim();

                        Gn응급가산수가 += READ_DRG_ER_AMT(pDbCon, strSuNext, strBun, strGbChild, strGisul, clsPmpaType.TIT.Age, nAgeDay, strNgt, nSQNal, nNal, ArgInDate, nMQty, strER, strSugbAD, strGbSgAdd, strSugbAC, strSuGbAB, strSuGbAA);
                    }
                }

                GnDRG_Amt2 = GnDRG_Amt2 + Gn응급가산수가;            //DRG 총액 + 복강개복수가
                GnDRG_TBonAmt = GnDRG_TBonAmt + Gn응급가산수가;      //DRG 총액 + 복강개복수가

                Dt.Dispose();
                Dt = null;
                #endregion

                #region //DRG 복강개복 수가 가산
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.Sunext,a.GbNgt,SUM(a.Qty*a.Nal) NQty,    ";
                SQL += ComNum.VBLF + "        SUM(a.Amt1) Amt1, SUM(a.Amt2) Amt2         ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,      ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b            ";
                SQL += ComNum.VBLF + "  WHERE a.PANO = '" + ArgPano + "'                 ";
                SQL += ComNum.VBLF + "    AND a.TRSNO = " + ArgTRSNO + "                 ";
                SQL += ComNum.VBLF + "    AND a.Sunext = b.Sunext(+)                     ";
                SQL += ComNum.VBLF + "    AND b.DRGOPEN ='Y'                             ";
                SQL += ComNum.VBLF + "  GROUP BY a.Sunext,a.GbNgt                        ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        Gn복강개복Amt += Convert.ToInt64(VB.Val(Dt.Rows[i]["AMT1"].ToString())); //Round(AdoGetNumber(rsDrg2, "AMT1", i), 0)
                    }
                }

                GnDRG_Amt2 = GnDRG_Amt2 + Gn복강개복Amt;            //DRG 총액 + 복강개복수가
                GnDRG_TBonAmt = GnDRG_TBonAmt + Gn복강개복Amt;      //DRG 총액 + 복강개복수가

                Dt.Dispose();
                Dt = null;
                #endregion

                #region //DRG 조절성인공수정체 사용시 인공수정체 제외 
                if (string.Compare(ArgInDate, "2020-01-01") >= 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT sucode    ";
                    SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a      ";
                    SQL += ComNum.VBLF + "  WHERE a.PANO = '" + ArgPano + "'                 ";
                    SQL += ComNum.VBLF + "    AND a.TRSNO = " + ArgTRSNO + "                 ";
                    SQL += ComNum.VBLF + "    AND a.Sunext in ('OTDRG1','OTDRG2','OTDRG3','OTDRG4')            ";
                    SQL += ComNum.VBLF + "     GROUP BY a.sucode HAVING SUM(QTY * NAL) > 0   ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        if (Dt.Rows[i]["sucode"].ToString().Trim() == "OTDRG1")
                        {
                            GnOTChaAmt = -129300;
                        }
                        else if (Dt.Rows[i]["sucode"].ToString().Trim() == "OTDRG2")
                        {
                            GnOTChaAmt = -258600;
                        }
                        else if (Dt.Rows[i]["sucode"].ToString().Trim() == "OTDRG3")
                        {
                            GnOTChaAmt = -47600;
                        }
                        else if (Dt.Rows[i]["sucode"].ToString().Trim() == "OTDRG4")
                        {
                            GnOTChaAmt = -95200;
                        }

                    }
                   
                    GnDRG_Amt2 = GnDRG_Amt2 + GnOTChaAmt;            //DRG 총액  + 조절성인공수정체 사용시 인공수정체 제외
                    GnDRG_TBonAmt = GnDRG_TBonAmt + GnOTChaAmt;      //DRG 총액  + 조절성인공수정체 사용시 인공수정체 제외

                    Dt.Dispose();
                    Dt = null;
                }
                #endregion
                #region //DRG 보상률 add 2020-01-01
                if (string.Compare(ArgInDate, "2020-01-01") >= 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT sum(a.amt1) * nvl(b.DRGBOSANG,0)  DrgADDAmt     ";
                    SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a , BAS_SUN b  ";
                    SQL += ComNum.VBLF + "  WHERE a.PANO = '" + ArgPano + "'                 ";
                    SQL += ComNum.VBLF + "    AND a.TRSNO = " + ArgTRSNO + "                 ";
                    SQL += ComNum.VBLF + "    AND nvl(b.DRGBOSANG,0) >0                      ";
                    SQL += ComNum.VBLF + "    AND a.Sunext = b.Sunext(+)                     ";
                    SQL += ComNum.VBLF + "  GROUP BY a.Sunext,nvl(b.DRGBOSANG,0) HAVING SUM(QTY * NAL) > 0   ";
                  
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        for (i = 0; i < Dt.Rows.Count; i++)
                        {
                            GnDrgADDAmt += Convert.ToInt64(VB.Val(Dt.Rows[i]["DrgADDAmt"].ToString())); 
                        }
                        

                    }
                    
                    GnDRG_Amt2 = GnDRG_Amt2 + GnDrgADDAmt;            //DRG 총액  + DRG 보상률 add
                    GnDRG_TBonAmt = GnDRG_TBonAmt + GnDrgADDAmt;      //DRG 총액  + DRG 보상률 add

                    Dt.Dispose();
                    Dt = null;
                }

                #endregion

                #region //DRG 재왕절개외 PCA 수가 계산 2018-09-20
                if (Gn재왕절개본인부담율 == 0 && 1== 0 )
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT a.Sunext,a.GbNgt,SUM(a.Qty*a.Nal) NQty,       ";
                    SQL += ComNum.VBLF + "        SUM(a.Amt1) Amt1, SUM(a.Amt2) Amt2            ";
                    SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP A,         ";
                    SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b               ";
                    SQL += ComNum.VBLF + "  WHERE a.PANO = '" + ArgPano + "'                    ";
                    SQL += ComNum.VBLF + "    AND a.TRSNO = " + ArgTRSNO + "                    ";
                    SQL += ComNum.VBLF + "    AND a.Sunext = b.Sunext(+)                        ";
                    SQL += ComNum.VBLF + "    AND ( a.SUCODE = 'BBI4A' OR a.SUNEXT IN ('N-FT-PC','N-FT10','N-FE-PC','N-FE10') ) ";
                    SQL += ComNum.VBLF + "  GROUP BY a.Sunext,a.GbNgt                           ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        for (i = 0; i < Dt.Rows.Count; i++)
                        {
                            GnPCA += Convert.ToInt64(VB.Val(Dt.Rows[i]["AMT1"].ToString()));
                        }
                    }

                    GnDRG_Amt2 = GnDRG_Amt2 + GnPCA;            //DRG 총액 + PCA수가
                    GnDRG_TBonAmt = GnDRG_TBonAmt + GnPCA;      //DRG 총액 + PCA수가

                    Dt.Dispose();
                    Dt = null;

                    //제왕절개 차상위 자격 본인부담없음 2016-09-23
                    if (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F" || clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2")
                    {
                        Gn재왕절개수가_Bon = (long)Math.Truncate(Gn재왕절개수가 * 0 / 100.0);
                    }
                    else
                    {
                        Gn재왕절개수가_Bon = (long)Math.Truncate(Gn재왕절개수가 * Gn재왕절개본인부담율 / 100.0);
                    }

                    Gn재왕절개수가_Jhp = Gn재왕절개수가 - Gn재왕절개수가_Bon;
                }
                #endregion

                #region //DRG 재왕절개 수가 계산 2016-07-04
                if (Gn재왕절개본인부담율 > 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT a.Sunext,a.GbNgt,SUM(a.Qty*a.Nal) NQty,       ";
                    SQL += ComNum.VBLF + "        SUM(a.Amt1) Amt1, SUM(a.Amt2) Amt2            ";
                    SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP A,         ";
                    SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b               ";
                    SQL += ComNum.VBLF + "  WHERE a.PANO = '" + ArgPano + "'                    ";
                    SQL += ComNum.VBLF + "    AND a.TRSNO = " + ArgTRSNO + "                    ";
                    SQL += ComNum.VBLF + "    AND a.Sunext = b.Sunext(+)                        ";
                    SQL += ComNum.VBLF + "    AND ( a.SUCODE = 'BBI4A' OR a.SUCODE = 'BBI5C' OR a.SUNEXT IN ('N-FT-PC','N-FT10','N-FE-PC','N-FE10') ) ";
                    SQL += ComNum.VBLF + "  GROUP BY a.Sunext,a.GbNgt                           ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        for (i = 0; i < Dt.Rows.Count; i++)
                        {
                            Gn재왕절개수가 += Convert.ToInt64(VB.Val(Dt.Rows[i]["AMT1"].ToString()));
                        }
                    }

                    GnDRG_Amt2 = GnDRG_Amt2 + Gn재왕절개수가;            //DRG 총액 + 재왕절개수가
                    GnDRG_TBonAmt = GnDRG_TBonAmt + Gn재왕절개수가;      //DRG 총액 + 재왕절개수가

                    Dt.Dispose();
                    Dt = null;

                    //제왕절개 차상위 자격 본인부담없음 2016-09-23
                    if (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F" || clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2")
                    {
                        Gn재왕절개수가_Bon = (long)Math.Truncate(Gn재왕절개수가 * 0 / 100.0);
                    }
                    else
                    {
                        Gn재왕절개수가_Bon = (long)Math.Truncate(Gn재왕절개수가 * Gn재왕절개본인부담율 / 100.0);
                    }

                    Gn재왕절개수가_Jhp = Gn재왕절개수가 - Gn재왕절개수가_Bon;
                }
                #endregion

                #region //100/100, 100/80, 100/50 단일,복합,루틴 모두 읽어서 각각 계산
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.SUCODE,a.SUNEXT, SUM(a.Amt1) Amt,SUM(a.Amt2) Amt2 ,to_char(bdate,'YYYY-MM-DD')  bdate  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A                  ";
                SQL += ComNum.VBLF + "  WHERE  a.TRSNO = " + ArgTRSNO + "                           ";
                SQL += ComNum.VBLF + "    AND  a.PANO = '" + ArgPano + "'                           ";
                SQL += ComNum.VBLF + "    AND  a.SUNEXT NOT IN ('DRG001','DRG002')                  ";
                SQL += ComNum.VBLF + "  GROUP BY a.SUCODE,a.SUNEXT ,bdate                                 ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strSugba = "";

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT c.DRG100 ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUT a, ";
                        SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b,  ";
                        SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUGA_DRGADD_NEW c  ";
                        SQL += ComNum.VBLF + "  WHERE a.Sucode = '" + Dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND a.SuNext = b.SuNext(+) ";
                        SQL += ComNum.VBLF + "    AND a.SuNext = c.SuNext(+) ";
                        SQL += ComNum.VBLF + "    AND c.SUDATE <= TO_DATE('" + Dt.Rows[i]["bdate"].ToString().Trim() + "','YYYY-MM-DD')       ";    //2014-07-24 적용일자 추가
                        SQL += ComNum.VBLF + "    AND c.DRG100 IS NOT NULL ";
                        SQL += ComNum.VBLF + "    order by c.SUDATE DESC ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }

                        if (Dt2.Rows.Count > 0)
                        {
                            strSugba = Dt2.Rows[0]["DRG100"].ToString().Trim();

                            //2016-07-04 DRG 제왕절개시 다음수가는 인정비급여로 취급안함 제욍절개 외 PCA보험 처리 시 금액이 산정되었을때도 다음수가는 인정비급여 취급안함
                            if (Gn재왕절개본인부담율 > 0 || GnPCA > 0 )
                            {
                                if (Dt.Rows[i]["SUCODE"].ToString().Trim() == "N-FT-PC" || Dt.Rows[i]["SUCODE"].ToString().Trim() == "N-FT10" || Dt.Rows[i]["SUCODE"].ToString().Trim() == "N-FE10" || Dt.Rows[i]["SUCODE"].ToString().Trim() == "N-FE-PC")
                                {
                                    strSugba = "";
                                }
                            }

                            if (strSugba == "Y")
                            {
                                GnGs100Amt += Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));   //100/100
                            }
                            else if (strSugba == "2" )
                            {   //add kyo "6" 2017-02-01
                                GnGs80Amt_T += Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());  //'80            
                                GnGs80Amt_J += (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString())) * 0.8);
                                GnGs80Amt_B += (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString())) * 0.2);
                            }
                            else if (strSugba == "3")
                            {   //add kyo "6" 2017-02-01
                                GnGs80Amt_T += Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());  //'80            
                                GnGs80Amt_J += (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString())) * 0.7);
                                GnGs80Amt_B += (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString())) * 0.3);
                            }
                            else if (strSugba == "4" || strSugba == "6")
                            {   //add kyo "6" 2017-02-01
                                GnGs80Amt_T += Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());  //'80            
                                GnGs80Amt_J += (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString())) * 0.2);     
                                GnGs80Amt_B += (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString())) * 0.8);     
                            }
                            else if (strSugba == "5" || strSugba == "7")
                            {   //add kyo "6" 2017-02-01
                                GnGs50Amt_T += Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());  //'50            
                                GnGs50Amt_J += (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString())) * 0.5);
                                GnGs50Amt_B += (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString())) * 0.5);
                            }
                            
                        }

                        Dt2.Dispose();
                        Dt2 = null;
                    }
                }
                Dt.Dispose();
                Dt = null;
                #endregion

                #region //DRG 비급여 계산
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.NU,a.Bun,a.GbSelf,b.SugbS GbSugbS,      ";
                SQL += ComNum.VBLF + "        SUM(a.Amt1) Amt,SUM(a.Amt2) Amt2          ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,     ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b           ";
                SQL += ComNum.VBLF + "  WHERE a.PANO = '" + ArgPano + "'                ";
                SQL += ComNum.VBLF + "    AND a.TRSNO = " + ArgTRSNO + "                ";
                SQL += ComNum.VBLF + "    AND a.Sunext=b.Sunext(+)                      ";
                SQL += ComNum.VBLF + "    AND b.DRGF ='Y'                               ";
                SQL += ComNum.VBLF + "    AND a.BUN NOT IN ('74','77')                  ";    //식대,실료차 비급여 제외
                SQL += ComNum.VBLF + "  GROUP BY a.Nu,a.Bun,a.GbSelf,b.SugbS            ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        GnDrgBiFAmt += Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));
                    }
                }
                Dt.Dispose();
                Dt = null;
                #endregion

                #region //2014-08-28 추가입원비용 계산로직 추가
                //RoomClass = 'O' 6인실,  RoomClass = 'M' 5인실
                //BDate 로 Group By 한 이유는 EDI_SUGA에서 적용일자별로 수가금액을 적용시키기 위함
                //적용일자별 등급 및 차액을 읽기 위해 TRANSDATE3 까지만 조회...
                //DRG 추가 입원료, GROUP BY ROOMCODE
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(a.BDate,'YYYY-MM-DD') BDate, SUM(a.Qty*a.Nal) SQty,   ";
                SQL += ComNum.VBLF + "        b.TRANSDATE1,b.RoomClass, b.OverAmt,                          ";
                SQL += ComNum.VBLF + "        b.TRANSDATE2,b.RoomClass1, b.OverAmt1,                        ";
                SQL += ComNum.VBLF + "        b.TRANSDATE3,b.RoomClass2, b.OverAmt2                         ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,                         ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_ROOM b                              ";
                SQL += ComNum.VBLF + "  WHERE a.BDATE >= TO_DATE('2014-09-01','YYYY-MM-DD')                 ";
                SQL += ComNum.VBLF + "    AND a.RoomCode = b.RoomCode(+)                                    ";
                SQL += ComNum.VBLF + "    AND a.WardCode = b.WardCode(+)                                    ";
                SQL += ComNum.VBLF + "    AND a.TRSNO = " + ArgTRSNO + "                                    ";
                SQL += ComNum.VBLF + "    AND a.PANO = '" + ArgPano + "'                                    ";
                //SQL += ComNum.VBLF + "    AND a.SUCODE NOT IN ('AU312','AU214','AU204','AU302','AP601','AV222','AV820','V7000','AC421','AC321','AI700', 'IA221','AV2221','AU403','AU413','AH013','AC460','AI120','AH011','AC321','AC302') ";
                SQL += ComNum.VBLF + "    AND a.SUCODE NOT IN ('AU312','AU214','AU204','AU302','AP601','AV222','AV820','V7000','AC421','AC321','AI700', ";
                SQL += ComNum.VBLF + "                         'IA221','AV2221','AU403','AU413','AH013','AC460','AI120','AH011','AC321','AC302','ID110','ID120','ID130') "; //2021-09-16 결핵상담료, 관리료
                SQL += ComNum.VBLF + "    AND a.BUN IN ('04','06')                                          ";   //환자관리료
                SQL += ComNum.VBLF + "  GROUP BY a.BDate, b.TRANSDATE1,b.RoomClass,b.OverAmt, b.TRANSDATE2,b.RoomClass1,b.OverAmt1, b.TRANSDATE3,b.RoomClass2,b.OverAmt2 ";
                SQL += ComNum.VBLF + "  HAVING SUM(A.QTY * A.NAL) > 0  ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                nRead = Dt.Rows.Count;

                if (nRead > 0)
                {
                    
                    for (i = 0; i < nRead; i++)
                    {
                        strRmCls = "";
                        nABAmt = 0;
                        nQty = Convert.ToDouble(Dt.Rows[i]["SQty"].ToString());
                        if (nQty < 1.0 && nQty > 0.0)
                        {
                            nQty = 1;       //체감제 수량 때문에...
                        }     
                        else
                        {
                            nQty = 1;       //체감제 수량 때문에... nQty = Convert.ToInt16(nQty);
                        }

                        strBDate = Dt.Rows[i]["BDATE"].ToString().Trim(); 
                            
                        strTrsDate[0] = Dt.Rows[i]["TRANSDATE1"].ToString().Trim();  
                        strTrsDate[1] = Dt.Rows[i]["TRANSDATE2"].ToString().Trim();  
                        strTrsDate[2] = Dt.Rows[i]["TRANSDATE3"].ToString().Trim();   
                            
                        //기준일자 적용
                        if (string.Compare(strBDate, strTrsDate[0]) >= 0)
                        {
                            strRmCls = Dt.Rows[i]["RoomClass"].ToString().Trim(); 
                        }
                        else if (string.Compare(strBDate, strTrsDate[1]) >= 0)
                        { 
                            strRmCls = Dt.Rows[i]["RoomClass1"].ToString().Trim();
                        }
                        else if (string.Compare(strBDate, strTrsDate[2]) >= 0) 
                        { 
                            strRmCls = Dt.Rows[i]["RoomClass2"].ToString().Trim();
                        }
                        
                        //RoomClass = 'M' 5인실
                        if (strRmCls == "M")
                        {
                            //5인실의 경우
                            nABAmt = cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB220", strBDate);  //5인실입원료 * 일수
                            nABAmt1 = nABAmt1 + (long)Math.Truncate(nABAmt * nQty);
                            nCNT = nCNT + (int)nQty;      //총일수
                            nABAmt3 = nABAmt3 + (long)Math.Truncate(cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB200", strBDate) * nQty);  //5인실에 대한 제외 (기본입원료 * 일수)  kyo 5인실 AB220코드변경    
                        }

                        if (strRmCls == "K")     //2017-07-10 83병동 4인실 추가
                        {
                            //4인실의 경우
                            nABAmt = cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB240", strBDate);  //4인실입원료 * 일수
                            nABAmt1 = nABAmt1 + (long)Math.Truncate(nABAmt * nQty);
                            nCNT = nCNT + (int)nQty;      //총일수
                            nABAmt3 = nABAmt3 + (long)Math.Truncate(cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB200", strBDate) * nQty);  //6인실에 대한 제외 (기본입원료 * 일수)
                        }

                        if ((strRmCls == "A" || strRmCls == "B" || strRmCls == "C") && string.Compare(strBDate, "2019-07-01") >= 0)      // 1인실입원료* 일수 0원으로 셋팅 비급여로 추가입원료에서 6인실 - 해주는 로직
                        {
                            //4인실의 경우
                            nABAmt = cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB901", strBDate);  //4인실입원료 * 일수
                            nABAmt1 = nABAmt1 + (long)Math.Truncate(nABAmt * nQty);
                            nCNT = nCNT + (int)nQty;      //총일수
                            nABAmt3 = nABAmt3 + (long)Math.Truncate(cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB200", strBDate) * nQty);  //6인실에 대한 제외 (기본입원료 * 일수)
                        }

                        if ((strRmCls == "G" || strRmCls == "H") && string.Compare(strBDate, "2018-07-01") >= 0)     //2018-07-01 2인실 추가
                        {
                            //2인실의 경우
                            nABAmt = cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB270", strBDate);  //4인실입원료 * 일수
                            nABAmt1 = nABAmt1 + (long)Math.Truncate(nABAmt * nQty);
                            nCNT = nCNT + (int)nQty;      //총일수
                            nABAmt3 = nABAmt3 + (long)Math.Truncate(cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB200", strBDate) * nQty);  //6인실에 대한 제외 (기본입원료 * 일수)
                            GnDrg추가입원료_Tbed = (GnDrg추가입원료_Tbed + (long)Math.Truncate(nABAmt * nQty)) - (long)Math.Truncate(cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB200", strBDate) * nQty);
                            GnDrg추가입원료_Bbed = (GnDrg추가입원료_Bbed + (long)Math.Truncate(cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB270", strBDate) * nQty));
                            GnDrg추가입원료_Nbed = (GnDrg추가입원료_Nbed + (long)Math.Truncate(cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB200", strBDate) * nQty));
                        }
                    }

                    //2015-09-21
                    str추가입원 = "";
                    if (cISCHK.Chk_Add_Admission_Fee(pDbCon, ArgPano, ArgIpdNo, ArgTRSNO) == true) 
                    {
                        str추가입원 = "OK";
                    }
                    
                    //2015-09-01 당일퇴원자는 퇴원당일분 차액은 산정안함
                    //2015-09-19 가퇴원환자는 계산하는 당일분은 산정안함
                    if (ArgInDate != ArgOutDate && str추가입원 != "OK")
                    { 
                        //5인실 추가입원료 당일분 포함(퇴원시 퇴원당일분 포함)
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT TRANSDATE1,RoomClass,TRANSDATE2,RoomClass1,TRANSDATE3,RoomClass2 ";
                        SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "BAS_ROOM ";
                        SQL += ComNum.VBLF + "  Where RoomCode = " + clsPmpaType.TIT.RoomCode + " ";
                        SQL += ComNum.VBLF + "    And WardCode = '" + clsPmpaType.TIT.WardCode + "' ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (Dt2.Rows.Count > 0)
                        {
                            strRmCls = "";
                            //RoomClass = //M// 5인실
                            strTrsDate[0] = Dt2.Rows[0]["TRANSDATE1"].ToString().Trim(); 
                            strTrsDate[1] = Dt2.Rows[0]["TRANSDATE2"].ToString().Trim(); 
                            strTrsDate[2] = Dt2.Rows[0]["TRANSDATE3"].ToString().Trim();

                            //기준일자 적용
                            if (string.Compare(ArgOutDate, strTrsDate[0]) >= 0)
                            {
                                strRmCls = Dt2.Rows[0]["RoomClass"].ToString().Trim();
                            }
                            else if (string.Compare(ArgOutDate, strTrsDate[1]) >= 0)
                            {
                                strRmCls = Dt2.Rows[0]["RoomClass1"].ToString().Trim();
                            }
                            else if (string.Compare(ArgOutDate, strTrsDate[2]) >= 0)
                            {
                                strRmCls = Dt2.Rows[0]["RoomClass2"].ToString().Trim();
                            }
                            
                            if (strRmCls == "M")
                            {
                                nABAmt4 = cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB220", ArgOutDate) - cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB200", ArgOutDate);
                            }
                            
                            if (strRmCls == "K")  //2017-07-10 83병동 4인실 추가
                            { 
                                nABAmt4 = cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB240", ArgOutDate) - cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB200", ArgOutDate);   //2017-07-10  4인실 ADD
                            }

                            if ((strRmCls == "A" || strRmCls == "B" || strRmCls == "C" ) && string.Compare(ArgOutDate, "2019-07-01") >= 0)  //2019-07-01 1인실 비급여로 추가
                            {
                                nABAmt4 = cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB901", ArgOutDate) - cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB200", ArgOutDate);   //  1인실 ADD
                            }
                            if (strRmCls == "G" || strRmCls == "H")  //2018-07-01 2인실 추가
                            {
                                nABAmt4 = cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB270", ArgOutDate) - cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB200", ArgOutDate);   //2018-07-01  2인실 ADD
                                GnDrg추가입원료_Tbed = GnDrg추가입원료_Tbed + cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB270", ArgOutDate) - cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB200", ArgOutDate);
                                GnDrg추가입원료_Bbed = GnDrg추가입원료_Bbed + cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB270", ArgOutDate);
                                GnDrg추가입원료_Nbed = GnDrg추가입원료_Nbed + cISCHK.Rtn_Edi_Suga_Amt(pDbCon, "AB200", ArgOutDate);
                            }
                    }
                        Dt2.Dispose();
                        Dt2 = null;
                    }
                    
                    GnDrg추가입원료 = nABAmt1 - nABAmt3 - nABAmt2 + nABAmt4;   //DRG 병실료 DRG는 6인실 기준청구라 있었던 병실(5,4인실)에서 6인실 병실료를 빼면 병실차액료가 발생된다
                    
                    GnDRG_Amt2 += GnDrg추가입원료;
                    GnDRG_TBonAmt += GnDrg추가입원료;
                }

                Dt.Dispose();
                Dt = null;

                #endregion

                #region //2015-01-31 부수술 금액 계산
                int nBi = 0;
                long nBAmt = 0;
                double nGisul = 0;
                string strSugbE = string.Empty;
                string strHiRisk = string.Empty;
                string strSugbAB = string.Empty;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT Sunext,SUM(Qty*Nal) NQty, GbGisul,BUN,GBNGT,               ";
                SQL += ComNum.VBLF + "        TO_CHAR(BDate,'YYYY-MM-DD') BDATE,nvl(OPGUBUN,0) OPGUBUN,GBER,                ";
                SQL += ComNum.VBLF + "        HIGHRISK,GBSUGBAB ,gbsgadd                                        ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP                        ";
                SQL += ComNum.VBLF + "  WHERE TRSNO = " + ArgTRSNO + "                                  ";
                SQL += ComNum.VBLF + "    AND PANO = '" + ArgPano + "'                                  ";
                SQL += ComNum.VBLF + "    AND BUN IN ('34','35')                                        ";
                SQL += ComNum.VBLF + "    AND (GbNgt IN ('5','6','7')                                   ";
                SQL += ComNum.VBLF + "        OR OPGUBUN IN ('1','2','D'))                              ";
                SQL += ComNum.VBLF + "  Group By Sunext,GbGisul,BUN,GBNGT, BDate,nvl(OPGUBUN,0),HIGHRISK,GBSUGBAB ,GBER ,gbsgadd       ";
                SQL += ComNum.VBLF + " Having SUM(Qty*Nal) <> 0                                         ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                nRead = Dt.Rows.Count;

                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strNgt = Dt.Rows[i]["GBNGT"].ToString().Trim();
                        strBun = Dt.Rows[i]["BUN"].ToString().Trim();
                        strBDate = Dt.Rows[i]["BDATE"].ToString().Trim();   
                        strSugbE = Dt.Rows[i]["GbGisul"].ToString().Trim(); 
                        strSuNext = Dt.Rows[i]["Sunext"].ToString().Trim();
                        strOpGbn = Dt.Rows[i]["OPGUBUN"].ToString().Trim();
                        strHiRisk = Dt.Rows[i]["HIGHRISK"].ToString().Trim();
                        strSugbAB = Dt.Rows[i]["GBSUGBAB"].ToString().Trim();
                        strGbSgAdd = Dt.Rows[i]["gbsgadd"].ToString().Trim();
                        strER = Dt.Rows[i]["GBER"].ToString().Trim();
                        nBi = Convert.ToInt16(VB.Left(clsPmpaType.TIT.Bi, 1));
                        
                        //기술료율 계산
                        if (string.Compare(strSugbE, "0") > 0)
                        { 
                            if (string.Compare(strBDate, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[nBi] > 0)    //기술료가산
                            {
                                nGisul = clsPmpaPb.OLD_GISUL[nBi] / 100.0;
                            }
                            else
                            {
                                nGisul = clsPmpaPb.GISUL[nBi] / 100.0;
                            }
                        }

                        nQty = Convert.ToDouble(Dt.Rows[i]["NQty"].ToString());

                        if (strOpGbn != "" && strOpGbn != "D")
                        {
                            #region 가산항목 세팅
                            cBArg = new clsPmpaType.cBas_Add_Arg();

                            cBArg.AGE       = clsPmpaType.TIT.Age;
                            cBArg.AGEILSU   = clsPmpaType.TIT.AgeDays;
                            cBArg.SUNEXT    = strSuNext;       //수가코드
                            cBArg.BUN       = strBun;          //수가분류
                            cBArg.SUGBE     = strSugbE;        //수가 E항(기술료)
                            cBArg.BDATE     = strBDate;        //처방일자            
                            cBArg.GBER      = strER;           //응급 가산
                            cBArg.NIGHT     = strNgt;          //공휴, 야간 가산
                            cBArg.AN1       = strSugbAC;       //마취 가산
                            cBArg.OP1       = strGbSgAdd;      //외과 / 흉부외과 가산
                            cBArg.OP2       = strSugbAD;       //화상 가산          
                            if (strOpGbn == "D")
                            {
                                cBArg.OP3 = "";        //수술,부수술 가산    
                                cBArg.AGE = clsPmpaType.TIT.Age;
                            }
                            else
                            {
                                cBArg.OP3 = strOpGbn;        //수술,부수술 가산     
                            }
                            cBArg.OP4       = strHiRisk;       //산모 가산     
                            cBArg.XRAY1     = strSugbAB;       //판독 가산
                            #endregion

                            //EDI 수가 금액 
                            cBAR = cBACCT.Rtn_BasAdd_EdiSuga_Amt(pDbCon, cBArg);
                            
                            nBAmt = (long)Math.Truncate(Convert.ToInt64(cBAR.BASEAMT) * nQty * nGisul);
                        }
                        else
                        {
                            //기존 계산로직
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT BAMT From " + ComNum.DB_PMPA + "BAS_SUGA_AMT ";
                            SQL += ComNum.VBLF + " Where SUNEXT = '" + strSuNext + "' ";
                            SQL += ComNum.VBLF + "   AND SUDATE <= TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "   AND DELDATE IS NULL ";
                            SQL += ComNum.VBLF + " ORDER By SuDate DESC";
                            SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            if (Dt2.Rows.Count > 0)
                            {
                                nBAmt = (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt2.Rows[0]["BAmt"].ToString())) * nQty * nGisul);
                            }

                            Dt2.Dispose();
                            Dt2 = null;
                        }
                        
                        GnDrg부수술총액 += nBAmt;
                        
                    }
                }

                GnDRG_Amt2 += GnDrg부수술총액;         //DRG 총액 + 부수술총액
                GnDRG_TBonAmt += GnDrg부수술총액;      //DRG 총액 + 부수술총액

                Dt.Dispose();
                Dt = null;
                #endregion

                #region //의료질평가지원금
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Sunext, SUM(Qty*Nal) NQty, SUM(Amt1+Amt2) SAmt    ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP                ";
                SQL += ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "'                          ";
                SQL += ComNum.VBLF + "    AND TRSNO ='" + ArgTRSNO + "'                         ";
                SQL += ComNum.VBLF + "    AND SUCODE IN ('AU204','AU302','AU403')                       ";
                SQL += ComNum.VBLF + "  Group By Sunext                                         ";
                SQL += ComNum.VBLF + "  Having SUM(Qty*Nal) <> 0                                ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        strSuNext = Dt.Rows[i]["SUNEXT"].ToString().Trim();
                        nQty = Convert.ToDouble(Dt.Rows[i]["NQty"].ToString());
                        GnDrgJinAmt += Convert.ToInt64(VB.Val(Dt.Rows[i]["SAmt"].ToString())); 
                    }
                }

                GnDRG_Amt2 += GnDrgJinAmt;            //DRG 총액 + 의료질평가지원금
                GnDRG_TBonAmt += GnDrgJinAmt;         //DRG 총액 + 의료질평가지원금

                Dt.Dispose();
                Dt = null;
                #endregion

                #region //안전관리료
                if (string.Compare(ArgInDate, "2018-01-01") >= 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT Sunext, SUM(Qty*Nal) NQty, SUM(Amt1+Amt2) SAmt    ";
                    SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP                ";
                    SQL += ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "'                          ";
                    SQL += ComNum.VBLF + "    AND TRSNO = " + ArgTRSNO + "                          ";
                    SQL += ComNum.VBLF + "    AND SUCODE in ( 'AC421','AC321')                      ";
                    SQL += ComNum.VBLF + "  Group By Sunext                                         ";
                    SQL += ComNum.VBLF + "  Having SUM(Qty*Nal) <> 0                                ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        for (i = 0; i < Dt.Rows.Count; i++)
                        {
                            strSuNext = Dt.Rows[i]["SUNEXT"].ToString().Trim();
                            nQty = Convert.ToDouble(Dt.Rows[i]["NQty"].ToString());
                            GnDrgJinSAmt += Convert.ToInt64(VB.Val(Dt.Rows[i]["SAmt"].ToString()));
                        }
                    }

                   
                    Dt.Dispose();
                    Dt = null;
                }
                #endregion

                #region //감염예방관리료
                if (string.Compare(ArgInDate, "2018-01-01") >= 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT Sunext, SUM(Qty*Nal) NQty, SUM(Amt1+Amt2) SAmt    ";
                    SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP                ";
                    SQL += ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "'                          ";
                    SQL += ComNum.VBLF + "    AND TRSNO = " + ArgTRSNO + "                          ";
                    SQL += ComNum.VBLF + "    AND SUCODE IN ('AH013','AH011')                       ";
                    SQL += ComNum.VBLF + "  Group By Sunext                                         ";
                    SQL += ComNum.VBLF + "  Having SUM(Qty*Nal) <> 0                                ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        for (i = 0; i < Dt.Rows.Count; i++)
                        {
                            strSuNext = Dt.Rows[i]["SUNEXT"].ToString().Trim();
                            nQty = Convert.ToDouble(Dt.Rows[i]["NQty"].ToString());
                            GnDrgJinSAmt += Convert.ToInt64(VB.Val(Dt.Rows[i]["SAmt"].ToString()));
                        }
                    }

                
                    Dt.Dispose();
                    Dt = null;
                }
                #endregion

                #region //야간간호관리료
                if (string.Compare(ArgInDate, "2020-01-01") >= 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT Sunext, SUM(Qty*Nal) NQty, SUM(Amt1+Amt2) SAmt    ";
                    SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP                ";
                    SQL += ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "'                          ";
                    SQL += ComNum.VBLF + "    AND TRSNO = " + ArgTRSNO + "                          ";
                    SQL += ComNum.VBLF + "    AND SUCODE = 'AI120'                                  ";
                    SQL += ComNum.VBLF + "  Group By Sunext                                         ";
                    SQL += ComNum.VBLF + "  Having SUM(Qty*Nal) <> 0                                ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        for (i = 0; i < Dt.Rows.Count; i++)
                        {
                            strSuNext = Dt.Rows[i]["SUNEXT"].ToString().Trim();
                            nQty = Convert.ToDouble(Dt.Rows[i]["NQty"].ToString());
                            GnDrgJinSAmt += Convert.ToInt64(VB.Val(Dt.Rows[i]["SAmt"].ToString()));
                        }
                    }


                    Dt.Dispose();
                    Dt = null;
                }
                #endregion


                #region //수술안전예방관리료

                SQL = "";
                SQL += ComNum.VBLF + " SELECT Sunext, SUM(Qty*Nal) NQty, SUM(Amt1+Amt2) SAmt    ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP                ";
                SQL += ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "'                          ";
                SQL += ComNum.VBLF + "    AND TRSNO = " + ArgTRSNO + "                          ";
                SQL += ComNum.VBLF + "    AND SUCODE = 'AC460'                                  ";
                SQL += ComNum.VBLF + "    AND BDATE >= TO_DATE('2019-06-01','YYYY-MM-DD')       ";
                SQL += ComNum.VBLF + "  Group By Sunext                                         ";
                SQL += ComNum.VBLF + "  Having SUM(Qty*Nal) <> 0                                ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        strSuNext = Dt.Rows[i]["SUNEXT"].ToString().Trim();
                        nQty = Convert.ToDouble(Dt.Rows[i]["NQty"].ToString());
                        GnDrgJinSAmt += Convert.ToInt64(VB.Val(Dt.Rows[i]["SAmt"].ToString()));
                    }
                }
               
                Dt.Dispose();
                Dt = null;
                
                GnDRG_Amt2 += GnDrgJinSAmt;            //DRG 총액 + 안전관리료  +  감염예방관리료 + 수술안전관리료
                GnDRG_TBonAmt += GnDrgJinSAmt;         //DRG 총액 + 안전관리료  +  감염예방관리료 + 수술안전관리료

                #endregion

                #region //간호간병료 별도계산    2016-08-01
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Sunext, SUM(Qty*Nal) NQty, SUM(Amt1+Amt2) SAmt     ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP                 ";
                SQL += ComNum.VBLF + "  WHERE PANO ='" + ArgPano + "'                            ";
                SQL += ComNum.VBLF + "    AND TRSNO = " + ArgTRSNO + "                           ";
                SQL += ComNum.VBLF + "    AND SUCODE IN ('AV222','AV2221','AV222A','AV2221A')    ";
                SQL += ComNum.VBLF + "  Group By Sunext                                          ";
                SQL += ComNum.VBLF + "  Having SUM(Qty*Nal) <> 0                                 ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        strSuNext = Dt.Rows[i]["SUNEXT"].ToString().Trim();
                        nQty = Convert.ToDouble(Dt.Rows[i]["NQty"].ToString());
                        GnDrg간호간병료 += Convert.ToInt64(VB.Val(Dt.Rows[i]["SAmt"].ToString()));
                    }
                }

                GnDRG_Amt2 += GnDrg간호간병료;            //DRG 총액 + 간호간병료총액
                GnDRG_TBonAmt += GnDrg간호간병료;         //DRG 총액 + 간호간병료총액

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT Sunext, SUM(Qty*Nal) NQty, SUM(Amt1+Amt2) SAmt     ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP                 ";
                SQL += ComNum.VBLF + "  WHERE PANO ='" + ArgPano + "'                            ";
                SQL += ComNum.VBLF + "    AND TRSNO = " + ArgTRSNO + "                           ";
                SQL += ComNum.VBLF + "    AND SUCODE IN ('AV820','AV820A')    ";
                SQL += ComNum.VBLF + "  Group By Sunext                                          ";
                SQL += ComNum.VBLF + "  Having SUM(Qty*Nal) <> 0                                 ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        strSuNext = Dt.Rows[i]["SUNEXT"].ToString().Trim();
                        nQty = Convert.ToDouble(Dt.Rows[i]["NQty"].ToString());
                        GnDrg간호간병료H += Convert.ToInt64(VB.Val(Dt.Rows[i]["SAmt"].ToString()));
                    }
                }

                GnDRG_Amt2 += GnDrg간호간병료H;            //DRG 총액 + 간호간병료2인총액
                GnDRG_TBonAmt += GnDrg간호간병료H;         //DRG 총액 + 간호간병료2인총액

                Dt.Dispose();
                Dt = null;

                #endregion

                #region //급여초음파 별도계산                
                SQL = "";
                SQL += ComNum.VBLF + " SELECT  /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1+Amt2) SAmt    ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP                    ";
                SQL += ComNum.VBLF + "  WHERE PANO ='" + ArgPano + "'                               ";
                SQL += ComNum.VBLF + "    AND TRSNO = " + ArgTRSNO + "                              ";
                SQL += ComNum.VBLF + "    AND BUN = '49'                                            ";
                SQL += ComNum.VBLF + "    AND GBSELF = '0'                                          ";
                SQL += ComNum.VBLF + "    AND BDate>=TO_DATE('2016-10-01','YYYY-MM-DD')             ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count > 0)
                {
                    GnDrgSono += Convert.ToInt64(VB.Val(Dt.Rows[0]["SAmt"].ToString()));
                }

                Dt.Dispose();
                Dt = null;

                GnDRG_Amt2 += GnDrgSono;            //DRG 총액 + 부수술총액
                GnDRG_TBonAmt += GnDrgSono;         //DRG 총액 + 부수술총액
                #endregion

                //DRG 금액 계산
                READ_DRG_AMT_GESAN(pDbCon, ArgDRGCode, ArgPano, ArgIpdNo, ArgTRSNO);

                //GnDRG_Amt1 = (long)GnDRG_Amt1;
                //GnDRG_Amt2 = (long)GnDRG_Amt2;
                //GnDRG_TAmt = (long)GnDRG_TAmt;

            }
            #endregion
            
            return rtnVal;
        }

        /// <summary>
        /// DRG 본인부담금액, 조합금액, 비급여, 급여 계산
        /// author : 김민철
        /// Create Date : 2018-01-25
        /// <seealso cref="DRG.bas : READ_DRG_AMT_GESAN"/>
        /// </summary>
        /// <param name="ArgDRGCode"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTRSNO"></param>
        public void READ_DRG_AMT_GESAN(PsmhDb pDbCon, string ArgDRGCode, string ArgPano, long ArgIpdNo, long ArgTRSNO)
        {
            int i = 0;

            DataTable Dt = new DataTable();
            DataTable Dt1 = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string strSelf = string.Empty;
            string strBun = string.Empty;

            int intRowAffected = 0;
            long nBonGubyo = 0;     // 급여   본인 부담액
            long nBonBiGubyo = 0;   // 비급여 본인 부담액
            long nBoninAmt = 0;     // 본인   총 부담액
            long nTotBiGubyo = 0;   // 비급여 총액
            long nCTMRBonin = 0;
            long nGubTot = 0;

            clsIpdAcct cIA = new clsIpdAcct();
            clsIument cIM = new clsIument(); 

            if (GnDRG_Amt2 == 0)
            {
                return;
            }

            #region //변수 Clear 및 세팅
            GnDrgBonAmt = 0;         //DRG본인부담금
            GnDrgJohapAmt = 0;       //DRG조합부담금


            GnDrgSelTAmt = 0;        //DRG선택진료총금액
            GnDrgBiTAmt = 0;         //DRG비급여진료총금액
            GnDrg급여총액 = 0;       //DRG급여총액

            for (i = 0; i < 6; i++)
            {
                GnDrgFoodAmt[i] = 0;
                GnDrgRoomAmt[i] = 0;
            }
            #endregion

            #region //식대, 병실료 차액, 선택진료 금액 조회

            cIM.Ipd_Trans_PrtAmt_Read(pDbCon, ArgTRSNO, "");

            #region ///식대본인부담율 계산       
            SQL = "";
            SQL += ComNum.VBLF + " SELECT  /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1+Amt2) SAmt    ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP                    ";
            SQL += ComNum.VBLF + "  WHERE PANO ='" + ArgPano + "'                               ";
            SQL += ComNum.VBLF + "    AND TRSNO = " + ArgTRSNO + "                              ";
            SQL += ComNum.VBLF + "    AND BUN IN ('74') ";
            SQL += ComNum.VBLF + "    AND NU  = '16' "; //급여 식대
            SQL += ComNum.VBLF + "    AND SUCODE  IN ('Y1110','T1110','Z4200','Z0100','Z0011','Z0010','Z0020') ";

            SqlErr = clsDB.GetDataTable(ref Dt1, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장

            }
            if (Dt1.Rows.Count > 0)
            {

                if (clsPmpaType.TIT.OgPdBun == "P")
                {
                    clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt1.Rows[0]["SAmt"].ToString())) * (0 / 100.0));
                }

                else if (clsPmpaType.TIT.OgPdBun == "C")             //차상위계층환자는 가산식대 없음
                {
                    clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt1.Rows[0]["SAmt"].ToString())) * (0 / 100.0));
                }
                else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && (clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2"))
                {
                    clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt1.Rows[0]["SAmt"].ToString())) * (0 / 100.0));
                }                                     //식대가산금액
                else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && clsPmpaType.TIT.OgPdBun == "E")   //차상위계층2 만성질환자는 가산식대 (전액청구) 본인0%'2009 - 04 - 01
                {
                    clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt1.Rows[0]["SAmt"].ToString())) * (0 / 100.0));
                }                              //식대가산금액
                else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && clsPmpaType.TIT.OgPdBun == "F")   //차상위계층2 장애인 만성질환자는 가산식대(전액청구) 본인0%'2009 - 04 - 01
                {
                    clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt1.Rows[0]["SAmt"].ToString())) * (0 / 100.0));
                }                                      //식대가산금액
                else
                {
                    clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt1.Rows[0]["SAmt"].ToString())) * (50 / 100.0));
                }



            }

            Dt1.Dispose();
            Dt1 = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT  /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1+Amt2) SAmt    ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP                    ";
            SQL += ComNum.VBLF + "  WHERE PANO ='" + ArgPano + "'                               ";
            SQL += ComNum.VBLF + "    AND TRSNO = " + ArgTRSNO + "                              ";
            SQL += ComNum.VBLF + "    AND BUN IN ('74') ";
            SQL += ComNum.VBLF + "    AND NU  = '16' "; //급여 식대
            SQL += ComNum.VBLF + "    AND SUCODE NOT IN ('Y1110','T1110','Z4200','Z0100','Z0011','Z0010','Z0020') ";

            SqlErr = clsDB.GetDataTable(ref Dt1, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장

            }
            if (Dt1.Rows.Count > 0)
            {
                clsPmpaType.RPG.Amt5[3] += (long)Math.Truncate(Convert.ToInt64(VB.Val(Dt1.Rows[0]["SAmt"].ToString())) * (clsPmpaType.IBR.Food / 100.0));
            }

            Dt1.Dispose();
            Dt1 = null;

            #endregion





            //clsPmpaType.RPG.Amt5[3] = (long)Math.Truncate(clsPmpaType.RPG.Amt1[3] * (clsPmpaType.IBR.Food / 100.0));
            clsPmpaType.RPG.Amt6[3] = clsPmpaType.RPG.Amt1[3] - clsPmpaType.RPG.Amt5[3];

            //식대계산 금액 변수저장
            GnDrgFoodAmt[0] = clsPmpaType.RPG.Amt5[3];  //식대 급여본인
            GnDrgFoodAmt[1] = clsPmpaType.RPG.Amt6[3];  //식대 급여공단
            GnDrgFoodAmt[2] = clsPmpaType.RPG.Amt4[3];  //식대 전액부담
            GnDrgFoodAmt[3] = clsPmpaType.RPG.Amt3[3];  //식대 선택진료
            GnDrgFoodAmt[4] = clsPmpaType.RPG.Amt2[3];  //식대 비급여
            
            //병실차액료 변수저장
            GnDrgRoomAmt[0] = clsPmpaType.RPG.Amt5[21];  //병실차액 급여본인
            GnDrgRoomAmt[1] = clsPmpaType.RPG.Amt6[21];  //병실차액 급여공단
            GnDrgRoomAmt[2] = clsPmpaType.RPG.Amt4[21];  //병실차액 전액부담
            GnDrgRoomAmt[3] = clsPmpaType.RPG.Amt3[21];  //병실차액 선택진료
            GnDrgRoomAmt[4] = clsPmpaType.RPG.Amt2[21];  //병실차액 비급여

            for (i = 0; i < 31; i++)
            {
                GnDrgSelAmt[i] = clsPmpaType.RPG.Amt3[i];  //선택진료 금액 변수저장
            }

            #endregion

            #region //본인부담율 세팅
            // 2018-07-01 add 추가입원료중 2인실은 본인부담이 달라 뺀후 마지막에 더함
            GnDrg추가입원료 = GnDrg추가입원료 - GnDrg추가입원료_Tbed;
            GnDRG_TBonAmt = GnDRG_TBonAmt - GnDrg추가입원료_Tbed;
            GnDRG_TBonAmt = GnDRG_TBonAmt - GnDrg간호간병료H;  //'add 간호간병 2인실은 본인부담이 달라 뺀후 마지막에 더함


            if (Gn재왕절개본인부담율 > 0)
            {
                if (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F" || clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2")
                {
                    //제왕절개 차상위 자격 본인부담없음
                    GnDrgBonAmt         = (long)Math.Truncate(GnDRG_TBonAmt *   0 / 100.0);
                    GnDrg추가입원료_Bon = (long)Math.Truncate(GnDrg추가입원료 * 0 / 100.0);
                    GnDrg부수술총액_Bon = (long)Math.Truncate(GnDrg부수술총액 * 0 / 100.0);
                    GnDrgJinAmt_Bon     = (long)Math.Truncate(GnDrgJinAmt *     0 / 100.0);
                    GnDrgJinSAmt_Bon    = (long)Math.Truncate(GnDrgJinSAmt *    0 / 100.0);
                    Gn응급가산수가_Bon  = (long)Math.Truncate(Gn응급가산수가 *  0 / 100.0);
                    GnDrg간호간병료_Bon = (long)Math.Truncate(GnDrg간호간병료 * 0 / 100.0);
                    GnOTChaAmt_Bon      = (long)Math.Truncate(GnOTChaAmt * 0 / 100.0);
                    GnDrgADDAmt_Bon     = (long)Math.Truncate(GnDrgADDAmt* 0 / 100.0);
                    GnDrgSono_Bon       = (long)Math.Truncate(GnDrgSono *       0 / 100.0);
                    GnDrg추가입원료_Nbed = (long)Math.Truncate(GnDrg추가입원료_Nbed * 0 / 100.0);
                }
                else
                {
                    GnDrgBonAmt         = (long)Math.Truncate(GnDRG_TBonAmt *   Gn재왕절개본인부담율 / 100.0);
                    GnDrg추가입원료_Bon = (long)Math.Truncate(GnDrg추가입원료 * Gn재왕절개본인부담율 / 100.0);
                    GnDrg부수술총액_Bon = (long)Math.Truncate(GnDrg부수술총액 * Gn재왕절개본인부담율 / 100.0);
                    GnDrgJinAmt_Bon     = (long)Math.Truncate(GnDrgJinAmt *     Gn재왕절개본인부담율 / 100.0);
                    GnDrgJinSAmt_Bon    = (long)Math.Truncate(GnDrgJinSAmt *    Gn재왕절개본인부담율 / 100.0);
                    Gn응급가산수가_Bon  = (long)Math.Truncate(Gn응급가산수가 *  Gn재왕절개본인부담율 / 100.0);
                    GnDrg간호간병료_Bon = (long)Math.Truncate(GnDrg간호간병료 * Gn재왕절개본인부담율 / 100.0);
                    GnDrgSono_Bon       = (long)Math.Truncate(GnDrgSono *       Gn재왕절개본인부담율 / 100.0);
                    GnDrg추가입원료_Nbed = (long)Math.Truncate(GnDrg추가입원료_Nbed * Gn재왕절개본인부담율 / 100.0);
                    GnOTChaAmt_Bon       = (long)Math.Truncate(GnOTChaAmt * Gn재왕절개본인부담율 / 100.0);
                    GnDrgADDAmt_Bon      = (long)Math.Truncate(GnDrgADDAmt * Gn재왕절개본인부담율 / 100.0);
                }
            }
            else
            {
                GnDrgBonAmt         = (long)Math.Truncate(GnDRG_TBonAmt *   clsPmpaType.IBR.Bohum / 100.0);
                GnDrg추가입원료_Bon = (long)Math.Truncate(GnDrg추가입원료 * clsPmpaType.IBR.Bohum / 100.0);
                GnDrg부수술총액_Bon = (long)Math.Truncate(GnDrg부수술총액 * clsPmpaType.IBR.Bohum / 100.0);
                GnDrgJinAmt_Bon     = (long)Math.Truncate(GnDrgJinAmt *     clsPmpaType.IBR.Bohum / 100.0);
                GnDrgJinSAmt_Bon    = (long)Math.Truncate(GnDrgJinSAmt *    clsPmpaType.IBR.Bohum / 100.0);
                Gn응급가산수가_Bon  = (long)Math.Truncate(Gn응급가산수가 *  clsPmpaType.IBR.Bohum / 100.0);
                GnPCA_Bon           = (long)Math.Truncate(GnPCA          *  clsPmpaType.IBR.Bohum / 100.0);
                GnDrg간호간병료_Bon = (long)Math.Truncate(GnDrg간호간병료 * clsPmpaType.IBR.Bohum / 100.0);
                GnDrgSono_Bon       = (long)Math.Truncate(GnDrgSono *       clsPmpaType.IBR.Bohum / 100.0);
                GnDrg추가입원료_Nbed = (long)Math.Truncate(GnDrg추가입원료_Nbed * clsPmpaType.IBR.Bohum / 100.0);
                GnOTChaAmt_Bon = (long)Math.Truncate(GnOTChaAmt * clsPmpaType.IBR.Bohum / 100.0);
                GnDrgADDAmt_Bon = (long)Math.Truncate(GnDrgADDAmt * clsPmpaType.IBR.Bohum / 100.0);
            }
            #endregion
            // '2인실 추가입원료 본인부담 산정 후 합산

            GnDrg추가입원료 = GnDrg추가입원료 + GnDrg추가입원료_Tbed;
            GnDRG_TBonAmt = GnDRG_TBonAmt + GnDrg추가입원료_Tbed;
            GnDrgBonAmt     = GnDrgBonAmt + ((long)Math.Truncate(GnDrg추가입원료_Bbed * 40 / 100.0) - GnDrg추가입원료_Nbed);
            GnDrg추가입원료_Bon = GnDrg추가입원료_Bon + ((long)Math.Truncate(GnDrg추가입원료_Bbed * 40 / 100.0) - GnDrg추가입원료_Nbed);

            GnDrg간호간병료 += GnDrg간호간병료H;
            GnDrg간호간병료_Bon += (long)Math.Truncate(GnDrg간호간병료H * 40 / 100.0) ;
            GnDrgBonAmt = GnDrgBonAmt + (long)Math.Truncate(GnDrg간호간병료H * 40 / 100.0);


            //GnDrgJohapAmt       = GnDRG_Amt2 - GnDrgBonAmt + GnGs80Amt_J + GnGs50Amt_J + GnGs90Amt_J;
            GnDrgJohapAmt       = GnDRG_Amt2 - GnDrgBonAmt;
            GnDrg추가입원료_Jhp = GnDrg추가입원료 - GnDrg추가입원료_Bon;
            GnDrg부수술총액_Jhp = GnDrg부수술총액 - GnDrg부수술총액_Bon;
            GnDrgJinAmt_Jhp     = GnDrgJinAmt - GnDrgJinAmt_Bon;
            GnDrgJinSAmt_Jhp    = GnDrgJinSAmt - GnDrgJinSAmt_Bon;
            Gn응급가산수가_Jhp  = Gn응급가산수가 - Gn응급가산수가_Bon;
            GnPCA_Jhp           = GnPCA - GnPCA_Bon;
            GnDrg간호간병료_Jhp = GnDrg간호간병료 - GnDrg간호간병료_Bon;
            GnDrgADDAmt_Jhp = GnDrgADDAmt - GnDrgADDAmt_Bon;
            GnOTChaAmt_Jhp = GnOTChaAmt - GnOTChaAmt_Bon;

            //선택진료비
            for (i = 0; i < 31; i++)
            {
                GnDrgSelTAmt += GnDrgSelAmt[i]; //선택금액 총액
            }
            
            GnDrgBiFAmt += GnDrgFoodAmt[4] + GnDrgRoomAmt[4];     //비급여 항목
            GnDrgBiTAmt += GnDrgSelTAmt + GnDrgBiFAmt;            //비급여 총액에 선택금액 + DRG비급여 포함

            #region DRG 총금액 산정///////////////////////////////////////////////////////////////////////////////
            GnDRG_TAmt += GnDRG_Amt2 + GnDrgBiTAmt + GnGs100Amt + GnGs80Amt_T + GnGs50Amt_T + GnGs90Amt_T;  

            for (i = 0; i < 4; i++)
            {
                GnDRG_TAmt = GnDRG_TAmt + GnDrgFoodAmt[i]; //식대총합
                GnDRG_TAmt = GnDRG_TAmt + GnDrgRoomAmt[i]; //병실료차액총합
                if (i == 3)
                {
                    GnDrgBiTAmt += GnDrgFoodAmt[i];
                    GnDrgBiTAmt += GnDrgRoomAmt[i];
                }
            }

            //GnDrg급여총액 = GnDRG_TAmt - GnDrg열외군금액_Bon - GnGs100Amt - GnDrgBiTAmt;
            GnDrg급여총액 = GnDRG_TAmt - GnDrgBiTAmt - GnGs100Amt - GnGs80Amt_B - GnGs50Amt_B - GnGs90Amt_B;

            //'2015-03-02
            nGubTot = READ_행위별진료비총액_급여(pDbCon, ArgTRSNO,clsPmpaType.TIT.InDate);
            if (string.Compare(clsPmpaType.TIT.InDate, "2019-07-01") >= 0)
            {
                DRG.GnDrg열외군금액 = nGubTot - (DRG.GnDrg급여총액 - DRG.GnGs80Amt_J - DRG.GnGs50Amt_J - DRG.GnGs90Amt_J) - 1000000;

            }
            else
            {
                DRG.GnDrg열외군금액 = nGubTot - DRG.GnDrg급여총액 - 1000000;
            }
            if (DRG.GnDrg열외군금액 < 0) { DRG.GnDrg열외군금액 = 0; }
            GnDrg열외군금액_Bon = (long)Math.Truncate(GnDrg열외군금액 * clsPmpaType.IBR.Bohum / 100.0);
            GnDRG_TAmt = GnDRG_TAmt + GnDrg열외군금액_Bon;
            #endregion

            nBoninAmt = (GnDrgBonAmt + GnDrgBiTAmt + GnDrgFoodAmt[0] + GnDrgRoomAmt[0] + GnGs100Amt + GnDrg열외군금액_Bon + GnGs80Amt_B + GnGs50Amt_B + GnGs90Amt_B);
            nBoninAmt = (long)Math.Truncate(nBoninAmt / 10.0) * 10;  //'절사
            
            //TODO : DRG 환자중 상한제 대상자일 경우 본인 급여부담액이 어디까지 포함되는지 알아볼것 ////////////////////////////////////////////////////////////////////////////
            nBonGubyo = GnDrgBonAmt + GnDrg추가입원료_Bon + GnDrg부수술총액_Bon + GnDrgJinAmt_Bon + GnDrgJinSAmt_Bon + Gn응급가산수가_Bon + GnDrg간호간병료_Bon + GnDrgSono_Bon + GnOTChaAmt_Bon + GnDrgADDAmt_Bon;
            nBonGubyo += GnDrgFoodAmt[0] + GnDrgRoomAmt[0] + GnGs100Amt + GnGs80Amt_B + GnGs50Amt_B + GnGs90Amt_B;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            
            //본인부담 상한제 계산
            if (string.Compare(clsPmpaType.TIT.Bi, "20") <= 0)
            {
                GnDrg추가입원료_Bbed = (long)Math.Truncate(GnDrg추가입원료_Bbed * 40 / 100.0) - GnDrg추가입원료_Nbed;
                cIA.Gesan_Upper_Limit(pDbCon, ref nBonGubyo ,ref GnDrg추가입원료_Bbed);
            }

            // 급여 / 비급여 본인부담액을 계산
            nBonGubyo = GnDrgBonAmt + GnDrgFoodAmt[0] + GnDrgRoomAmt[0];
            nBonBiGubyo = GnDrgBiTAmt + GnGs100Amt;
            
            #region // 총금액, 조합, 본인 금액 계산
            clsPmpaType.TIT.Amt[50] = GnDRG_TAmt;   //KYO 2017-03-29 전산팀장 요청 TIT.Amt(53) DRG환자 로직 변경
            //clsPmpaType.TIT.Amt[51] = 0;   2018-10-17 삭제 
            //clsPmpaType.TIT.Amt[52] = 0;   2018-10-17 삭제
            
            if (clsPmpaType.TIT.Amt[64] != 0)
            { 
                clsPmpaType.TIT.Amt[53] = (clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64]) - nBoninAmt; //조합부담금
            }
            else
            { 
                clsPmpaType.TIT.Amt[53] = clsPmpaType.TIT.Amt[50] - nBoninAmt;  //조합부담금
            }

            //clsPmpaType.TIT.Amt[54] = 0;            //할인금액  2018-10-17 삭제
            clsPmpaType.TIT.Amt[55] = nBoninAmt;    //본인부담금
            clsPmpaType.TIT.Amt[62] = nBonGubyo;    //급여  본인부담금
            clsPmpaType.TIT.Amt[63] = nBonBiGubyo;  //비급여 본인부담금
            #endregion

            #region //할인금액 계산
            if (string.Compare(clsPmpaType.TIT.GbGameK, "00") > 0)
            {
                if (clsPmpaType.TIT.OutDate == "")
                {
                    if (clsPmpaType.TIT.GbGameK == "55" && clsPmpaType.TIT.GelCode == "")
                    {
                        clsPublic.GstrMsgList = "";
                        clsPublic.GstrMsgList += ComNum.VBLF + "계약처 감액인데 계약처 코드가 없습니다.";
                        clsPublic.GstrMsgList += ComNum.VBLF + "등록번호 : " + clsPmpaType.TIT.Pano;
                        clsPublic.GstrMsgList += ComNum.VBLF + "  진료과 : " + clsPmpaType.TIT.DeptCode;
                        ComFunc.MsgBox(clsPublic.GstrMsgList, "확인");
                    }

                    //2013-12-23 소방처전문치료 협약관련 감액기준
                    if (clsPmpaType.TIT.GelCode == "H911")
                    {
                        if (cIA.IPD_Gamek_Account_H119(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPublic.GstrSysDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt) == false)
                            return;

                        clsPmpaType.TIT.Amt[54] = clsPmpaType.GAM.Halin_Tot;
                        clsPmpaType.TIT.DtGamek = clsPmpaType.GAM.DTHalin_Tot;
                        clsPmpaType.TIT.Amt[55] = nBoninAmt - clsPmpaType.GAM.Halin_Tot;
                    }
                    else
                    {
                        if (cIA.IPD_Gamek_Account_Main(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPublic.GstrSysDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt, nCTMRBonin) == false)
                            return;
                    }
                }
                else
                {
                    //2013-12-23 소방처전문치료 협약관련 감액기준
                    if (clsPmpaType.TIT.GelCode == "H911")
                    {
                        if (cIA.IPD_Gamek_Account_H119(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPublic.GstrSysDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt) == false)
                            return;

                        clsPmpaType.TIT.Amt[54] = clsPmpaType.GAM.Halin_Tot;
                        clsPmpaType.TIT.DtGamek = clsPmpaType.GAM.DTHalin_Tot;
                        clsPmpaType.TIT.Amt[55] = nBoninAmt - clsPmpaType.GAM.Halin_Tot;
                    }
                    else
                    {
                        if (cIA.IPD_Gamek_Account_Main(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPmpaType.TIT.OutDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt, nCTMRBonin) == false)
                            return;
                    }
                }
                clsPmpaType.TIT.Amt[54] = clsPmpaType.GAM.Halin_Tot;
                clsPmpaType.TIT.DtGamek = clsPmpaType.GAM.DTHalin_Tot;
                clsPmpaType.TIT.Amt[55] = nBoninAmt - clsPmpaType.GAM.Halin_Tot;
            }
            #endregion
            
            if (string.Compare(clsPmpaType.TIT.TGbSts, "7") < 0)
            {
                #region //DRG 계산금액 변수처리
                clsPmpaType.TIT.Amt[65] = GnAmt1;                       //DRG 급여
                clsPmpaType.TIT.Amt[66] = GnAmt2;                       //DRG 비급여
                clsPmpaType.TIT.Amt[68] = GnDrgJohapAmt;                //DRG 조합부담금액
                clsPmpaType.TIT.Amt[69] = GnDrgBonAmt;                  //DRG 본인부담금액
                clsPmpaType.TIT.Amt[70] = GnDRG_TAmt;                   //DRG 총금액
                clsPmpaType.TIT.Amt[71] = GnDRG_Amt1;                   //DRG 원금액
                clsPmpaType.TIT.Amt[72] = (GnDRG_TAmt - GnDrgBiTAmt);   //DRG 급여합계
                clsPmpaType.TIT.Amt[73] = GnDrgBiTAmt;                  //DRG 비급여합계
                clsPmpaType.TIT.Amt[74] = GnDrgBiFAmt;                  //DRG 비급여
                clsPmpaType.TIT.Amt[75] = GnDrgSelTAmt;                 //DRG 선택진료합계
                clsPmpaType.TIT.Amt[76] = GnDrgJinAmt;                  //DRG 의료질평가지원금
                clsPmpaType.TIT.Amt[77] = GnGsAddAmt;                   //DRG 외과가산금액
                clsPmpaType.TIT.Amt[78] = GnGs100Amt;                   //DRG 인정비급여
                clsPmpaType.TIT.Amt[79] = Gn복강개복Amt;                //DRG 복강개복
                clsPmpaType.TIT.Amt[80] = GnDrg열외군금액;              //DRG 열외군금액
                clsPmpaType.TIT.Amt[81] = GnDrg추가입원료;              //DRG 추가입원료
                clsPmpaType.TIT.Amt[82] = GnDrg부수술총액;              //DRG 부수술총액
                clsPmpaType.TIT.Amt[83] = GnDrg간호간병료;              //DRG 간호간병료
                clsPmpaType.TIT.Amt[84] = Gn응급가산수가;               //DRG 응급가산수가
                clsPmpaType.TIT.Amt[85] = GnDrgSono;                    //DRG 급여초음파
                clsPmpaType.TIT.Amt[86] = Gn재왕절개수가+ GnPCA;               //DRG 재왕절개수가
                clsPmpaType.TIT.Amt[87] = GnGs80Amt_T;                  //DRG 100대80
                clsPmpaType.TIT.Amt[88] = GnGs50Amt_T;                  //DRG 100대50
                clsPmpaType.TIT.Amt[89] = GnGs90Amt_T;                  //DRG 100대90
                clsPmpaType.TIT.Amt[90] = GnDrgJinSAmt;                 //DRG 안전관리료
                clsPmpaType.TIT.Amt[91] = GnDRG_Amt2;                   //DRG 계산금액
                clsPmpaType.TIT.Amt[92] = GnDRG_WBonAmt;                //DRG 원금액의 본인부담액
                clsPmpaType.TIT.Amt[93] = GnDrg추가입원료_Jhp;          //DRG 추가입원료 조합
                clsPmpaType.TIT.Amt[94] = GnDrg추가입원료_Bon;          //DRG 추가입원료 본인
                clsPmpaType.TIT.Amt[95] = Gn행위별총액;                 //DRG 행위별총액

                #endregion
                //GnDrg추가입원료_Jhp = GnDrg추가입원료 - GnDrg추가입원료_Bon;
                #region IPD_TRANS 금액 UPDATE
                SQL = "";
                SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL += ComNum.VBLF + "   SET SangAmt = " + clsPmpaType.TIT.SangAmt + " ";   //상한제 대상 금액
                SQL += ComNum.VBLF + "       ,Amt50 = " + clsPmpaType.TIT.Amt[50] + " ";     //총진료비
                SQL += ComNum.VBLF + "       ,Amt51 = " + clsPmpaType.TIT.Amt[51] + " ";     //보증금,중간납 대체액
                SQL += ComNum.VBLF + "       ,Amt52 = " + clsPmpaType.TIT.Amt[52] + " ";     //사용안함
                SQL += ComNum.VBLF + "       ,Amt53 = " + clsPmpaType.TIT.Amt[53] + " ";     //조합부담
                SQL += ComNum.VBLF + "       ,Amt54 = " + clsPmpaType.TIT.Amt[54] + " ";     //할인액
                SQL += ComNum.VBLF + "       ,Amt55 = " + clsPmpaType.TIT.Amt[55] + " ";     //차인납부
                SQL += ComNum.VBLF + "       ,Amt56 = " + clsPmpaType.TIT.Amt[56] + " ";     //개인미수
                SQL += ComNum.VBLF + "       ,Amt57 = " + clsPmpaType.TIT.Amt[57] + " ";     //퇴원금
                SQL += ComNum.VBLF + "       ,Amt58 = " + clsPmpaType.TIT.Amt[58] + " ";     //환불금
                SQL += ComNum.VBLF + "       ,Amt59 = " + clsPmpaType.TIT.Amt[59] + " ";
                SQL += ComNum.VBLF + "       ,Amt60 = " + clsPmpaType.TIT.Amt[60] + " ";
                SQL += ComNum.VBLF + "       ,Amt61 = " + GnDrg급여총액 + " ";
                //SQL += ComNum.VBLF + "       ,Amt62 = " + GnDrgBiTAmt + " ";
                SQL += ComNum.VBLF + "       ,Amt62 = " + nBonGubyo + " ";  
                SQL += ComNum.VBLF + "       ,Amt63 = " + nTotBiGubyo + " ";                 //비급여
                SQL += ComNum.VBLF + "       ,Amt65 = " + clsPmpaType.TIT.Amt[65] + " ";
                SQL += ComNum.VBLF + "       ,Amt66 = " + clsPmpaType.TIT.Amt[66] + " ";

                SQL += ComNum.VBLF + "       ,Amt68 = " + clsPmpaType.TIT.Amt[68] + " ";    //DRG 조합부담금액
                SQL += ComNum.VBLF + "       ,Amt69 = " + clsPmpaType.TIT.Amt[69] + " ";    //DRG 본인부담금액
                SQL += ComNum.VBLF + "       ,Amt70 = " + clsPmpaType.TIT.Amt[70] + " ";    //DRG 총금액
                SQL += ComNum.VBLF + "       ,Amt71 = " + clsPmpaType.TIT.Amt[71] + " ";    //DRG 원금액
                SQL += ComNum.VBLF + "       ,Amt72 = " + clsPmpaType.TIT.Amt[72] + " ";    //DRG 급여합계
                SQL += ComNum.VBLF + "       ,Amt73 = " + clsPmpaType.TIT.Amt[73] + " ";    //DRG 비급여합계
                SQL += ComNum.VBLF + "       ,Amt74 = " + clsPmpaType.TIT.Amt[74] + " ";    //DRG 비급여
                SQL += ComNum.VBLF + "       ,Amt75 = " + clsPmpaType.TIT.Amt[75] + " ";    //DRG 선택진료합계
                SQL += ComNum.VBLF + "       ,Amt76 = " + clsPmpaType.TIT.Amt[76] + " ";    //DRG 의료질평가지원금
                SQL += ComNum.VBLF + "       ,Amt77 = " + clsPmpaType.TIT.Amt[77] + " ";    //DRG 외과가산금액
                SQL += ComNum.VBLF + "       ,Amt78 = " + clsPmpaType.TIT.Amt[78] + " ";    //DRG 인정비급여
                SQL += ComNum.VBLF + "       ,Amt79 = " + clsPmpaType.TIT.Amt[79] + " ";    //DRG 복강개복
                SQL += ComNum.VBLF + "       ,Amt80 = " + clsPmpaType.TIT.Amt[80] + " ";    //DRG 열외군금액
                SQL += ComNum.VBLF + "       ,Amt81 = " + clsPmpaType.TIT.Amt[81] + " ";    //DRG 추가입원료
                SQL += ComNum.VBLF + "       ,Amt82 = " + clsPmpaType.TIT.Amt[82] + " ";    //DRG 부수술총액
                SQL += ComNum.VBLF + "       ,Amt83 = " + clsPmpaType.TIT.Amt[83] + " ";    //DRG 간호간병료
                SQL += ComNum.VBLF + "       ,Amt84 = " + clsPmpaType.TIT.Amt[84] + " ";    //DRG 응급가산수가
                SQL += ComNum.VBLF + "       ,Amt85 = " + clsPmpaType.TIT.Amt[85] + " ";    //DRG 급여초음파
                SQL += ComNum.VBLF + "       ,Amt86 = " + clsPmpaType.TIT.Amt[86] + " ";    //DRG 재왕절개수가
                SQL += ComNum.VBLF + "       ,Amt87 = " + clsPmpaType.TIT.Amt[87] + " ";    //DRG 100대80
                SQL += ComNum.VBLF + "       ,Amt88 = " + clsPmpaType.TIT.Amt[88] + " ";    //DRG 100대50
                SQL += ComNum.VBLF + "       ,Amt89 = " + clsPmpaType.TIT.Amt[89] + " ";    //DRG 100대90
                SQL += ComNum.VBLF + "       ,Amt90 = " + clsPmpaType.TIT.Amt[90] + " ";    //DRG 안전관리료
                SQL += ComNum.VBLF + "       ,Amt91 = " + clsPmpaType.TIT.Amt[91] + " ";    //DRG 계산금액
                SQL += ComNum.VBLF + "       ,Amt92 = " + clsPmpaType.TIT.Amt[92] + " ";    //DRG 원금액 본인부담액
                SQL += ComNum.VBLF + "       ,Amt93 = " + clsPmpaType.TIT.Amt[93] + " ";    //DRG 추가입원료 조합
                SQL += ComNum.VBLF + "       ,Amt94 = " + clsPmpaType.TIT.Amt[94] + " ";    //DRG 추가입원료 본인
                SQL += ComNum.VBLF + "       ,Amt95 = " + clsPmpaType.TIT.Amt[95] + " ";    //DRG 행위별
                SQL += ComNum.VBLF + "       ,DRGOG = '" + GstrOgAdd + "'              ";   //DRG 산과가산여부
                SQL += ComNum.VBLF + "       ,GBSANG = '" + clsPmpaType.TIT.GbSang + "' ";
                SQL += ComNum.VBLF + " WHERE TRSNO = " + ArgTRSNO + " ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                #endregion
            }


        }

        /// <summary>
        /// ER응급가산 수가를 읽기 위함
        /// SG 공용변수 사용함
        /// ADMIN.DRG_CODE_ER 테이블 참조함
        /// author : 김민철
        /// Create Date : 2018-01-23
        /// <seealso cref="DRG.bas : Read_DRG_ER_Amt"/>
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argSUNEXT"></param>
        /// <param name="ArgBun"></param>
        /// <param name="ArgGbChild"></param>
        /// <param name="ArgGisul"></param>
        /// <param name="ArgAge"></param>
        /// <param name="ArgNgt"></param>
        /// <param name="ArgSONAL"></param>
        /// <param name="ArgNal"></param>
        /// <param name="ArgBDate"></param>
        /// <param name="ArgQty"></param>
        /// <param name="strER"></param>
        /// <param name="strSugbAD"></param>
        /// <param name="strGbSgAdd"></param>
        /// <param name="strSugbAC"></param>
        /// <param name="strSugbAB"></param>
        /// <returns></returns>
        public long READ_DRG_ER_AMT(PsmhDb pDbCon, string argSUNEXT, string ArgBun, string ArgGbChild, string ArgGisul, int ArgAge, double ArgAgeDays, string ArgNgt, double ArgSONAL, int ArgNal, string ArgBDate, double ArgQty, string strER, string strSugbAD, string strGbSgAdd, string strSugbAC, string strSugbAB, string strSuGbAA)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            int nBi = 1;
            long nDAMTM = 0;
            long rtnVal = 0;
            string strHang = string.Empty;

            double nSuham = 0;

            clsBasAcct cBAcct = new clsBasAcct();
            clsIuSentChk cISCHK = new clsIuSentChk();
            clsPmpaType.cBas_Add_Arg cBArg = null;
            clsPmpaType.Bas_Acc_Rtn cBAR = new clsPmpaType.Bas_Acc_Rtn();
            clsPmpaFunc cPF = new clsPmpaFunc();

            strHang = cBAcct.Bas_Acct_Hang_Set(ArgBun);                         //항목코드
            GstrPCode = "";



            cPF.Suga_Read(pDbCon, argSUNEXT);
            if (clsPmpaType.RS.SuCode == "") { cPF.Suga_Read2(pDbCon, argSUNEXT); }
           
            //병원에서 사용하는 EDI 가산 수가만 적용  && clsPmpaType.RS.SugbB != "0"
            if ((ArgBun == "22" || ArgBun == "28" || ArgBun == "34") && clsPmpaType.RS.SugbB != "0")
            {
                #region 가산항목 세팅
                cBArg = new clsPmpaType.cBas_Add_Arg();

                cBArg.AGE = ArgAge;
                cBArg.AGEILSU = ArgAgeDays;
                cBArg.SUNEXT = argSUNEXT;       //수가코드
                cBArg.BUN = ArgBun;          //수가분류
                cBArg.SUGBE = ArgGisul;        //수가 E항(기술료)
                cBArg.BDATE = ArgBDate;        //처방일자            
                cBArg.GBER = strER;           //응급 가산
                cBArg.NIGHT = ArgNgt;          //공휴, 야간 가산
                //2018-11-28 add 
                if (ArgNgt == "D")
                { 
                    cBArg.MIDNIGHT = "Y";
                }
                cBArg.AN1 = strSugbAC;       //마취 가산
                cBArg.OP1 = strGbSgAdd;      //외과 / 흉부외과 가산
                cBArg.OP2 = strSugbAD;       //화상 가산          
                cBArg.OP3 = "";              //수술,부수술 가산     
                cBArg.OP4 = "";              //산모 가산     
                cBArg.XRAY1 = strSugbAB;       //판독 가산

                if (clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55")
                {
                    cBArg.Bi = 6;
                }
                else
                {
                    cBArg.Bi = Convert.ToInt16(VB.Left(clsPmpaType.TIT.Bi, 1));
                }
                #endregion

                //EDI 수가 금액 
                cBAR = cBAcct.Rtn_BasAdd_EdiSuga_Amt(pDbCon, cBArg);

                GstrPCode = cBAR.PCODE;
            }
            else
            {
                FnQty       = ArgQty;
                FnNal       = ArgNal;
                FnAge       = ArgAge;
                FstrBun     = ArgBun;
                FstrGbChild = ArgGbChild;
                FstrGisul   = ArgGisul;
                FstrSuNext  = argSUNEXT;
                FstrNgt     = ArgNgt;
                FstrSugbAC  = strSugbAC;
                FstrSugbAD  = strSugbAD;
                FstrGbSGADD = strGbSgAdd;

                //clsPmpaFunc cPF = new clsPmpaFunc();
                clsIpdAcct cIAcct = new clsIpdAcct();

                cPF.Suga_Read(pDbCon, FstrSuNext);
                if (clsPmpaType.RS.SuCode == "") { cPF.Suga_Read2(pDbCon, FstrSuNext); }
                cIAcct.Move_RS_TO_ISG();

                //예전방식대로 진행
                switch (EDI_HangMok_SET(ArgBun))
                {
                    case "01": CODE_01_Process(pDbCon); break;    //진찰료
                    case "02": CODE_02_Process(pDbCon); break;    //입원료
                    case "03": CODE_03_Process(pDbCon); break;    //투약
                    case "04": CODE_04_Process(pDbCon); break;    //주사
                    case "05": CODE_05_Process(pDbCon); break;    //마취
                    case "06": CODE_06_Process(pDbCon); break;    //물리치료
                    case "07": CODE_07_Process(pDbCon); break;    //신경정신
                    case "08": CODE_08_Process(pDbCon); break;    //처치,수술
                    case "09": CODE_09_Process(pDbCon); break;    //검사
                    case "10": CODE_10_Process(pDbCon); break;    //방사선
                    case "S":  CODE_11_Process(pDbCon); break;    //C/T, MRI 
                    default:
                        break;
                }
            }

            if (FstrSuNext == "Q275512B")
            {
                GstrPCode = "Q275512B";
            }

           
            if (GstrPCode == "")
            {
                return rtnVal;
            }

            #region DRG 응급가산 수가 테이블 조회
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(DDATE,'YYYY-MM-DD') DDATE,                        ";
            SQL += ComNum.VBLF + "        CODE,GBN,DNAME,SNAME,DJUMSUS,DAMTS,DJUMSUM,DAMTM,         ";
            SQL += ComNum.VBLF + "        DJUMSUU,DAMTU,DJUMSUL,DAMTL                               ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "DRG_CODE_ER                         ";
            SQL += ComNum.VBLF + "  WHERE CODE = '" + GstrPCode + "'                                ";
            SQL += ComNum.VBLF + "    AND DDATE <=TO_DATE('" + ArgBDate + "','YYYY-MM-DD')          ";
            SQL += ComNum.VBLF + "    AND GBN ='" + strSuGbAA + "'                                  ";
            SQL += ComNum.VBLF + "  ORDER By DDate DESC                                             ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (Dt.Rows.Count > 0)
            {
                nDAMTM = Convert.ToInt64(Dt.Rows[0]["DAMTM"].ToString());

                if (ArgBun == "34" && argSUNEXT != "")
                {
                    nSuham = cBAcct.Rtn_Bas_Sun_Standard(pDbCon, argSUNEXT);
                    if (nSuham != 1)
                    { nDAMTM = (long)Math.Truncate(nDAMTM * nSuham); }
                }
            }

            Dt.Dispose();
            Dt = null;

            if (clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55")
            {
                nBi = 6;
            }
            else
            {
                nBi = Convert.ToInt16(VB.Left(clsPmpaType.TIT.Bi, 1));
            }

            if (strHang == "04")        //마취료
            {
                rtnVal = (long)Math.Truncate(cBAcct.BAS_MACH_AMT(1, argSUNEXT, nDAMTM, ArgQty, ArgNal) * clsPmpaPb.GISUL[nBi] / 100.0);
            }
            else
            {
                rtnVal = (long)Math.Truncate(nDAMTM * clsPmpaPb.GISUL[nBi] / 100.0 * ArgSONAL); 
                #endregion
            }
            
            return rtnVal;
        }

        private void CODE_01_Process(PsmhDb pDbCon)
        {
            string strGbChild = "";

            //진찰료 소아가산 코드 SET
            //99.1.1일부 소아가산 6세이하만 Check
            strGbChild = "0";
            if (FnAge < 6) { strGbChild = "6"; }

            clsIuSentChk cISK = new clsIuSentChk();
            GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate);

            if (GstrPCode == "JJJJJJ" || GstrPCode == "AB220" || GstrPCode == "AB223" || GstrPCode == "AB221" ||
                GstrPCode == "AA222" || GstrPCode == "AY100" || GstrPCode == "AU204" || GstrPCode == "AU302" ||
                GstrPCode == "AU214" || GstrPCode == "AU312" || GstrPCode == "AP601" || GstrPCode == "V2300" ||
                GstrPCode == "V4303" || GstrPCode == "V2200" || GstrPCode == "AU403" || GstrPCode == "AU413" )
            {

            }
            else
            {
                if (strGbChild != "0") //AA220,AA221:재진환자병원관리료
                {
                    //2016-04-28
                    if (FstrSuNext == "AA256")
                    { }
                    else
                    {
                        if (GstrPCode.Length == 5) { GstrPCode += "000"; }
                        GstrPCode = VB.Left(GstrPCode, 5) + strGbChild + VB.Right(GstrPCode, 2);
                    }
                }
            }

        }

        private void CODE_02_Process(PsmhDb pDbCon)
        {
            clsIuSentChk cISK = new clsIuSentChk();
            //입원료는 환자관리료+병원관리료로 청구함
            //환자관리료는 EDI청구 제외 처리
            //99.1.1일 입원료 산정코드 변경됨

            switch (FstrSuNext)
            {
                case "AB210":
                case "AB2101":
                case "AB2102":
                     break;
                case "AB2103": 
                case "AB2103A":
                    break;
                case "AB220":   GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate); break;   //일반병실 입원료(계)
                case "AB2201":  GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate); break;   //내.소.정 입원료(계)
                case "AJ200":
                case "AJ201":   GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate); break;   //'I.C.U    입원료(계)
                case "AK200":   GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate); break;   //격리실   입원료 (계)
                case "AB2206":  GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate); break;   //응급실6시간이상(계)
                case "AB2207":  GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate); break;   //낮병동(계)
                case "AB2209":  GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate); break;   //모유수유 간호관리료
                case "AB2210":  GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate); break;   //신생아 목욕 간호관리료
                default:        GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate); break;
            }

            //체감제 구분코드 SET,수량을 1로 변경
            //00:00-06:00입원자 입원료의 50%산정 (산정코드:100):99.5.8신설
            //2000-04-01 입원료 체감제 변경
            if (FnQty == 0.9 || FnQty == 0.85 || FnQty == 0.5)
            {
                if (GstrPCode.Length == 5)
                {
                    GstrPCode += "000";
                }

                if (FnQty == 0.9)
                {
                    GstrPCode = VB.Left(GstrPCode, 5) + "8" + VB.Right(GstrPCode, 2);
                }
                else if (FnQty == 0.85)
                {
                    GstrPCode = VB.Left(GstrPCode, 5) + "9" + VB.Right(GstrPCode, 2);
                }
                else if (FnQty == 0.5)
                {
                    GstrPCode = VB.Left(GstrPCode, 5) + "1" + VB.Right(GstrPCode, 2);
                }
            }

        }

        private void CODE_03_Process(PsmhDb pDbCon)
        {
            string strPcode = "";

            clsIuSentChk cISK = new clsIuSentChk();

            GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate);

            //조제료 소아20%가산(입원도 퇴원약은 가산됨)
            if (FnAge < 6 && GstrPCode.CompareTo("J1010") >= 0 && GstrPCode.CompareTo("J1191") <= 0)
            {
                strPcode = VB.Left(strPcode, 5) + "600";
            }
        }

        private void CODE_04_Process(PsmhDb pDbCon)
        {
            clsIuSentChk cISK = new clsIuSentChk();

            GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate);

            //만8세미만 소아20%,30%가산
            if (FnAge < 8)
            {
                if (FstrBun == "17" || FstrBun == "18" || FstrBun == "29")
                {
                    switch (FstrSuNext)
                    {
                        case "KK041":
                        case "KK054":                        
                        case "KK024":
                            break;
                        default:
                            GstrPCode = VB.Left(GstrPCode, 5) + "300";
                            break;
                    }
                }

                switch (FstrSuNext)
                {
                    case "KK090":
                    case "KK100":
                    case "KK110":
                        GstrPCode = VB.Left(GstrPCode, 5) + "300";
                        break;
                }
            }
            
        }

        private void CODE_05_Process(PsmhDb pDbCon)
        {
            int nTime = 0;
            string strTime = string.Empty;
            string strOK = string.Empty;
            int nAgeYearIlsu = 0;  //2017-07-24 add

            ComFunc CF = new ComFunc();
            clsIuSentChk cISK = new clsIuSentChk();

            nAgeYearIlsu = CF.DATE_ILSU(pDbCon, clsPublic.GstrSysDate, "20" + VB.Left(clsPmpaType.TIT.Jumin1, 2) + "-" + VB.Mid(clsPmpaType.TIT.Jumin1, 3, 2) + "-" + VB.Right(clsPmpaType.TIT.Jumin1, 2));  //2017-07-24 add
                
            if (VB.Left(FstrSuNext, 5) == "L2010") { L2010_Rtn(ref strTime, ref nTime); }
            if (VB.Left(FstrSuNext, 5) == "L3010") { L2010_Rtn(ref strTime, ref nTime); }
            if (VB.Left(FstrSuNext, 5) == "L6010") { GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate); }
            if (VB.Left(FstrSuNext, 5) == "L7010") { GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate); }

            if (GstrPCode == "") { GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate); }
            if (FstrNgt.Trim() == "") { FstrNgt = "0"; }


            //마취행위료의 소아,70세,야간,공휴가산
            if ((GstrPCode.CompareTo("L0") >= 0 && GstrPCode.CompareTo("L9") <= 0) || (VB.Left(GstrPCode, 2) == "LA" || VB.Left(GstrPCode, 2) == "LB") && FstrNgt != "0")
            { 
                //야간,휴일가산
                if (VB.Left(GstrPCode, 6) == "L12119" || VB.Left(GstrPCode, 6) == "L12219" || VB.Left(GstrPCode, 6) == "L12116" || VB.Left(GstrPCode, 6) == "L12216")  //기본적으로 L1211900코드가 발생 됩니다.
                {
                    if (FstrNgt == "1" || FstrNgt == "4" || FstrNgt == "7")
                    {
                        GstrPCode = VB.Left(GstrPCode, 6) + "50";  //휴일가산
                    }
                    else if (FstrNgt == "2" || FstrNgt == "5" || FstrNgt == "8")
                    {
                        GstrPCode = VB.Left(GstrPCode, 6) + "10";  //야간가산
                    }
                    else
                    {
                        GstrPCode = VB.Left(GstrPCode, 6) + "00";
                    }
                }
                //계두술은 신생아 소아 노인가산은 없음
                else if (FstrSugbAC.CompareTo("0") > 0)
                {
                    if (FstrNgt == "1" || FstrNgt == "4" || FstrNgt == "7")
                    {
                        GstrPCode = VB.Left(GstrPCode, 6) + "50";  //휴일가산
                    }
                    else if (FstrNgt == "2" || FstrNgt == "5" || FstrNgt == "8")
                    {
                        GstrPCode = VB.Left(GstrPCode, 6) + "10";  //야간가산
                    }
                    else
                    {
                        GstrPCode = VB.Left(GstrPCode, 6) + "00";
                    }
                }
                //계두술,일측,개흉은 신생아 소아 노인가산은 없음
                else
                {
                    if (FstrNgt == "1" || FstrNgt == "4" || FstrNgt == "7")
                    {
                        GstrPCode += "050";  //휴일가산
                    }
                    else if (FstrNgt == "2" || FstrNgt == "5" || FstrNgt == "8")
                    {
                        GstrPCode += "010";  //야간가산
                    }
                    else
                    {
                        GstrPCode += "000";
                    }

                    //신생아,소아,노인가산
                    if (FstrNgt.CompareTo("6") >= 0 && FstrNgt.CompareTo("8") <= 0)
                    {
                        GstrPCode = VB.Left(GstrPCode, 5) + "1" + VB.Right(GstrPCode, 2);
                    }
                    else if (FstrNgt.CompareTo("3") >= 0 && FstrNgt.CompareTo("5") <= 0)
                    { 
                        if (FnAge >= 70)   //70세이상
                        {
                            GstrPCode = VB.Left(GstrPCode, 5) + "4" + VB.Right(GstrPCode, 2);
                        }
                        else               //만8세미만
                        {
                            GstrPCode = VB.Left(GstrPCode, 5) + "3" + VB.Right(GstrPCode, 2);
                        }
                    }
                }
            }
            
            if (clsPmpaType.ISG.SugbB != "0")
            { 
                if (FnAge > 70)
                {
                    GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                    GstrPCode = VB.Left(GstrPCode, 5) + "4" + VB.Right(GstrPCode, 2);
                }
                else if (FnAge == 0)
                { 
                    if (nAgeYearIlsu < 28)  //신생아
                    {
                        GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                        GstrPCode = VB.Left(GstrPCode, 5) + "1" + VB.Right(GstrPCode, 2);
                    }
                    else
                    {
                        GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                        GstrPCode = VB.Left(GstrPCode, 5) + "A" + VB.Right(GstrPCode, 2);
                    }
                }
                else if (FnAge < 6)
                {
                    GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                    GstrPCode = VB.Left(GstrPCode, 5) + "B" + VB.Right(GstrPCode, 2);
                }
                else
                {
                    GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                    GstrPCode = VB.Left(GstrPCode, 5) + "0" + VB.Right(GstrPCode, 2);
                }
            }
            
            if (clsPmpaType.ISG.SugbAA == "3") //응급가산 별표3----------------------------------------------------------------------
            {
                switch (clsPmpaType.TIT.KTASLEVL)
                {
                    case "1":
                    case "2":
                    case "3":
                        if (GstrPCode.Length < 8) { GstrPCode = VB.Left(GstrPCode + "00000000", 8); }
                        if (FstrNgt == "1") { GstrPCode = VB.Left(GstrPCode, 6) + "40";}  //응급공휴
                        if (FstrNgt == "2") { GstrPCode = VB.Left(GstrPCode, 6) + "30";}  //응급야간
                        if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "30";}  //응급공휴(부수술)
                        if (FstrNgt == "7") { GstrPCode = VB.Left(GstrPCode, 6) + "40";}  //응급야간(부수술)
                        if (FstrNgt == "9") { GstrPCode = VB.Left(GstrPCode, 6) + "20";}  //주간응급
                        if (FstrNgt == "0") { GstrPCode = VB.Left(GstrPCode, 6) + "20";}  //주간응급
                        if (FstrNgt == "3") { GstrPCode = VB.Left(GstrPCode, 6) + "20";}  //주간응급
                        if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "20";}  //주간응급
                        if (FstrNgt == "5") { GstrPCode = VB.Left(GstrPCode, 6) + "20"; }  //응급(부수술)
                        break;
                    default:
                        break;
                }
             }
        }

        private void L2010_Rtn(ref string strTime, ref int nTime) //경막외,척수마취
        {
            strTime = "01";
            nTime = (int)(FnQty * 60) + FnNal;
            if (nTime > 60) { strTime = VB.Format((int)(nTime / 15), "0#"); }

            switch (FstrSuNext)
            {
                case "L2010K":
                    if (nTime < 61) { GstrPCode = "L0210"; }
                    else { GstrPCode = "L2" + strTime + "0"; }      //경막외마취
                    break;
                case "L2010K0": //'15분단수
                case "L3010K":
                    GstrPCode = "L3" + strTime + "0"; break; //척수마취
                case "L3010K0": break;  //15분단수
                default: break;
            }
        }

        private void CODE_06_Process(PsmhDb pDbCon)
        {
            clsIuSentChk cISK = new clsIuSentChk();

            GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate);
        }

        private void CODE_07_Process(PsmhDb pDbCon)
        {
            clsIuSentChk cISK = new clsIuSentChk();

            GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate);
        }

        private void CODE_08_Process(PsmhDb pDbCon)
        {
            string strB = string.Empty;
            string strDtlBun = string.Empty;
            int nAgeYearIlsu = 0;

            ComFunc CF = new ComFunc();
            clsIuSentChk cISK = new clsIuSentChk();
            clsPmpaFunc cPF = new clsPmpaFunc();

            GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate);
            strDtlBun = cPF.Read_DtlBun(pDbCon, FstrSuNext);


            nAgeYearIlsu = CF.DATE_ILSU(pDbCon, clsPublic.GstrSysDate, "20" + VB.Left(clsPmpaType.TIT.Jumin1, 2) + "-" + VB.Mid(clsPmpaType.TIT.Jumin1, 3, 2) + "-" + VB.Right(clsPmpaType.TIT.Jumin1, 2));
            //주사수기료 소아가산(8세미만)
            if (FstrBun == "35" && FstrGisul == "1")
            {
                GstrPCode = VB.Left(GstrPCode, 5) + "3" + VB.Right(GstrPCode, 2);   //정상분만은 무조건 ~300 처리 함 2014-09-29 50가산
            }

            //주사수기료 소아가산(8세미만)
            if (FnAge < 8 && GstrPCode == "KK052")
            {
                GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                GstrPCode = VB.Left(GstrPCode, 5) + "3" + VB.Right(GstrPCode, 2);
            }
            else if (FnAge < 6 && FstrGisul == "1" && FstrGisul == "1" && GstrPCode != "JJJJJJ")
            {
                GstrPCode = VB.Left(GstrPCode + "00000000", 8);

                if (FstrBun == "28" || FstrBun == "34")
                {
                    if (nAgeYearIlsu < 28)   //신생아 //태어난난포함
                    {
                        if (FstrSugbAD == "1")
                            GstrPCode = VB.Left(GstrPCode, 5) + "W" + VB.Right(GstrPCode, 2);       //화상치료목적
                        else if (FstrGbSGADD == "1")
                            GstrPCode = VB.Left(GstrPCode, 5) + "U" + VB.Right(GstrPCode, 2);       //외과가산
                        else if (FstrGbSGADD == "2")
                            GstrPCode = VB.Left(GstrPCode, 5) + "V" + VB.Right(GstrPCode, 2);       //흉부외과산
                        else
                            GstrPCode = VB.Left(GstrPCode, 5) + "6" + VB.Right(GstrPCode, 2);       //신생아(만28일이전) 100% 가산
                    }
                    else if (FnAge == 0)   //0세
                    {
                        if (FstrSugbAD == "1")
                            GstrPCode = VB.Left(GstrPCode, 5) + "Q" + VB.Right(GstrPCode, 2);       //화상치료목적
                        else if (FstrGbSGADD == "1")
                            GstrPCode = VB.Left(GstrPCode, 5) + "N" + VB.Right(GstrPCode, 2);       //외과가산
                        else if (FstrGbSGADD == "2")
                            GstrPCode = VB.Left(GstrPCode, 5) + "P" + VB.Right(GstrPCode, 2);       //흉부외과산
                        else
                            GstrPCode = VB.Left(GstrPCode, 5) + "A" + VB.Right(GstrPCode, 2);       //신생아(만28일이후) 50% 가산
                    }
                    else //소아가산 //6세미만
                    {
                        if (FstrSugbAD == "1")
                            GstrPCode = VB.Left(GstrPCode, 5) + "M" + VB.Right(GstrPCode, 2);       //화상치료목적
                        else if (FstrGbSGADD == "1")
                            GstrPCode = VB.Left(GstrPCode, 5) + "K" + VB.Right(GstrPCode, 2);       //외과가산
                        else if (FstrGbSGADD == "2")
                            GstrPCode = VB.Left(GstrPCode, 5) + "L" + VB.Right(GstrPCode, 2);       //흉부외과산
                        else
                            GstrPCode = VB.Left(GstrPCode, 5) + "B" + VB.Right(GstrPCode, 2);       ////6세미만 30 %
                    }
                }
                else
                {
                    GstrPCode = VB.Left(GstrPCode, 5) + "3" + VB.Right(GstrPCode, 2);
                }

            }
            else if (FnAge >= 35 && (FstrGbChild == "1" || FstrGbChild == "Z") && FstrGisul == "1" && GstrPCode != "JJJJJJ")   //35세이상 산모
            {
                strB = cISK.Rtn_Bas_Sut_B(pDbCon, FstrSuNext);
                if (strB == "Z") { strB = "Y"; }
                if (strB == "")
                {
                    strB = cISK.Rtn_Bas_SuH_B(pDbCon, FstrSuNext);
                    if (strB == "Z") { strB = "Y"; }
                }

                if (strB == "Y")
                {
                    GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                    GstrPCode = VB.Left(GstrPCode, 5) + "5" + VB.Right(GstrPCode, 2);
                }
            }
            else
            {
                if (FstrSugbAD == "1")
                {
                    GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                    GstrPCode = VB.Left(GstrPCode, 5) + "9" + VB.Right(GstrPCode, 2);       //화상치료목적
                }
                
                if (FstrGbSGADD == "1")
                {
                    GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                    GstrPCode = VB.Left(GstrPCode, 5) + "1" + VB.Right(GstrPCode, 2);         //외과산
                }

                if (FstrGbSGADD == "2")
                {
                    GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                    GstrPCode = VB.Left(GstrPCode, 5) + "2" + VB.Right(GstrPCode, 2);         //흉부외과산
                }
            }
            
            //부수술코드 SET
            if (FstrGisul == "1" && FnQty == 0.5 && GstrPCode != "JJJJJJ")
            {
                if (VB.Left(GstrPCode, 5) == "S5117" || VB.Left(GstrPCode, 5) == "R4275")
                {
                }
                else
                {
                    if (GstrPCode.Length < 8) { GstrPCode = VB.Left(GstrPCode + "00000000", 8); }
                    GstrPCode = VB.Left(GstrPCode, 7) + "1";
                }
            }

            //수량이 0.5이고 부수술이면 *1
            if (FnQty == 0.5 && GstrPCode != "JJJJJJ" && GstrPCode.Length != 8)
            {
                if (VB.Left(GstrPCode, 5) == "S5117" || VB.Left(GstrPCode, 5) == "R4275")
                {
                }
                else
                {
                    if (FstrNgt.CompareTo("5") >= 0) { GstrPCode = VB.Left(GstrPCode + "00000000", 8); }
                    GstrPCode = VB.Left(GstrPCode + "0000000", 7) + "1";
                }
            }

            //수량이 0.7은 종병이상
            if (FnQty == 0.7 && GstrPCode != "JJJJJJ" && (FstrBun == "35" || FstrBun == "34") && FstrGisul == "1")
            {
                if (GstrPCode.Length < 8) { GstrPCode = VB.Left(GstrPCode + "00000000", 8); }
                GstrPCode = VB.Left(GstrPCode + "0000000", 7) + "4";
            }
            
            // 0.무 1.휴일 2.야간 5.부수술 6.부수술휴일 7.부수술야간 9.주간응급
            // A.제2의수술 B.제2의수술휴일 C.제2의수술야간
            
            //야간,공휴 SET
            if (GstrPCode != "JJJJJJ")
            {
                if (FstrNgt.CompareTo("0") < 0)
                {
                    FstrNgt = "0";
                }
                else if (FstrNgt == "D")
                {

                }
                else if (FstrNgt.CompareTo("9") > 0)
                {
                    FstrNgt = "0";
                }
                else if (FstrNgt == "3" || FstrNgt == "4" || FstrNgt == "5" || FstrNgt == "6" || FstrNgt == "7" || FstrNgt == "8")
                {
                    FstrNgt = "0";
                }
                
                if (FstrGisul == "1" && FstrNgt != "0")
                {
                    if (GstrPCode.Length < 8) { GstrPCode = VB.Left(GstrPCode + "00000000", 8); }
                    if (FstrNgt == "1") { GstrPCode = VB.Left(GstrPCode, 6) + "5" + VB.Right(GstrPCode, 1); } //공휴
                    if (FstrNgt == "2") { GstrPCode = VB.Left(GstrPCode, 6) + "1" + VB.Right(GstrPCode, 1); } //야간
                    if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "5" + VB.Right(GstrPCode, 1); } //공휴(부수술)
                    if (FstrNgt == "7") { GstrPCode = VB.Left(GstrPCode, 6) + "1" + VB.Right(GstrPCode, 1); } //야간(부수술)
                    if (FstrNgt == "9") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                    if (FstrNgt == "D") { GstrPCode = VB.Left(GstrPCode, 6) + "6" + VB.Right(GstrPCode, 1); } //주간응급
                }
                
                if (FstrGisul == "1")     //JJY 2016-01012 LEVEL 로직 추가
                { 
                    if (clsPmpaType.ISG.SugbAA == "1")  // 응급가산 별표1 --------------------------------------------------------------------
                    {
                        if (clsPmpaType.TIT.KTASLEVL == "A")
                        {
                            if (GstrPCode.Length < 8) { GstrPCode = VB.Left(GstrPCode + "00000000", 8); }
                            GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1);    //응급
                            if (FstrNgt == "1") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴
                            if (FstrNgt == "2") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간
                            if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴(부수술)
                            if (FstrNgt == "7") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간(부수술)
                            if (FstrNgt == "9") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                            if (FstrNgt == "0") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                        }
                        else if (clsPmpaType.TIT.KTASLEVL == "1" || clsPmpaType.TIT.KTASLEVL == "2" || clsPmpaType.TIT.KTASLEVL == "3")
                        {
                            if (GstrPCode.Length < 8) { GstrPCode = VB.Left(GstrPCode + "00000000", 8); }
                            //GstrPCode = VB.Left(GstrPCode, 5) + "7" + VB.Right(GstrPCode, 2)   //응급
                            if (FstrNgt == "1") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴
                            if (FstrNgt == "2") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간
                            if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급공휴(부수술)
                            if (FstrNgt == "7") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급야간(부수술)
                            if (FstrNgt == "9") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                            if (FstrNgt == "0") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                        }
                    }
                    else if (clsPmpaType.ISG.SugbAA == "2")  //응급가산 별표2----------------------------------------------------------------------
                    {
                        if (clsPmpaType.TIT.KTASLEVL == "1" || clsPmpaType.TIT.KTASLEVL == "2" || clsPmpaType.TIT.KTASLEVL == "3")
                        {
                            if (GstrPCode.Length < 8) { GstrPCode = VB.Left(GstrPCode + "00000000", 8); }
                            if (FstrNgt == "1") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴
                            if (FstrNgt == "2") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간
                            if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급공휴(부수술)
                            if (FstrNgt == "7") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급야간(부수술)
                            if (FstrNgt == "9") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                            if (FstrNgt == "0") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                        }
                    }
                    else if (clsPmpaType.ISG.SugbAA == "3") //응급가산 별표3----------------------------------------------------------------------
                    {
                        if (clsPmpaType.TIT.KTASLEVL == "1" || clsPmpaType.TIT.KTASLEVL == "2" || clsPmpaType.TIT.KTASLEVL == "3")
                        {
                            if (GstrPCode.Length < 8) { GstrPCode = VB.Left(GstrPCode + "00000000", 8); }
                            if (FstrNgt == "1") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴
                            if (FstrNgt == "2") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간
                            if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급공휴(부수술)
                            if (FstrNgt == "7") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급야간(부수술)
                            if (FstrNgt == "9") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                            if (FstrNgt == "0") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                            if (FstrNgt == "3") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                            if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                            if (FstrNgt == "D") { GstrPCode = VB.Left(GstrPCode, 6) + "7" + VB.Right(GstrPCode, 1); } //심야응급
                       }
                    }
                }
            }
        }

        private void CODE_09_Process(PsmhDb pDbCon)
        {
            string strOK = string.Empty;

            clsIuSentChk cISK = new clsIuSentChk();

            GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate);

            //내시경,천자,생검,순환기능검사시 소아가산 20%
            if (GstrPCode != "JJJJJJ")
            {

                //2017-01-26 보험 수면내시경일 경우 코드 치환 2017년 2월부터                
                if (VB.Left(GstrPCode, 5) == "EA001" || VB.Left(GstrPCode, 5) == "EA002" || VB.Left(GstrPCode, 5) == "EA003" || VB.Left(GstrPCode, 5) == "EA004")
                {
                    GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                    if (FnAge < 8 && FstrGbChild == "Y")      //만 8세 미만 가산(30%)
                        GstrPCode = VB.Left(GstrPCode, 5) + "3" + VB.Right(GstrPCode, 2);
                    else if (FnAge >= 70 && clsPmpaType.ISG.GbChildZ == "Y")          //만 70세 이상 가산(30%)
                        GstrPCode = VB.Left(GstrPCode, 5) + "4" + VB.Right(GstrPCode, 2);

                    if (FstrNgt == "1" || FstrNgt == "4" || FstrNgt == "7")
                    {
                        GstrPCode = VB.Left(GstrPCode, 6) + "50";  //휴일가산
                    }
                    else if (FstrNgt == "2" || FstrNgt == "5" || FstrNgt == "8")
                    {
                        GstrPCode = VB.Left(GstrPCode, 6) + "10";  //야간가산
                    }
                    else
                    {
                        GstrPCode = VB.Left(GstrPCode, 6) + "00";
                    }   
                }

                if (FstrGbChild.Trim() == "") { FstrGbChild = "0"; }
                if (FstrGisul == "1" && FstrGbChild != "0")
                { 
                    if (VB.Left(GstrPCode, 5) == "FA145" || VB.Left(GstrPCode, 5) == "FA141" || VB.Left(GstrPCode, 5) == "FA144" || VB.Left(GstrPCode, 5) == "F6101")
                    {
                        GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                        GstrPCode = VB.Left(GstrPCode, 5) + "6" + VB.Right(GstrPCode, 2);
                    }
                    else if (FstrBun != "41")
                    {
                        GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                        GstrPCode = VB.Left(GstrPCode, 5) + "3" + VB.Right(GstrPCode, 2);
                    }
                    else //핵의학검사 2000.7.1일 소아가산10% 신설
                    {
                        GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                        GstrPCode = VB.Left(GstrPCode, 5) + "3" + VB.Right(GstrPCode, 2);
                    }
                }

                if (clsPmpaType.ISG.SugbAA == "1")  // 응급가산 별표1 --------------------------------------------------------------------
                {
                    if (clsPmpaType.TIT.KTASLEVL == "A")
                    {
                        GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                        GstrPCode = VB.Left(GstrPCode, 5) + "7" + VB.Right(GstrPCode, 2);   //응급
                        if (FstrNgt == "1") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴
                        if (FstrNgt == "2") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간
                        if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴(부수술)
                        if (FstrNgt == "7") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간(부수술)
                        if (FstrNgt == "9") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                                                                                                                  //소아가산
                        if (FstrGisul == "1" && FstrGbChild == "1") { GstrPCode = VB.Left(GstrPCode, 5) + "8" + VB.Right(GstrPCode, 2); }
                    }
                    else if (clsPmpaType.TIT.KTASLEVL == "1" || clsPmpaType.TIT.KTASLEVL == "2" || clsPmpaType.TIT.KTASLEVL == "3")
                    {
                        GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                        GstrPCode = VB.Left(GstrPCode, 5) + "7" + VB.Right(GstrPCode, 2);   //응급
                        if (FstrNgt == "1") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴
                        if (FstrNgt == "2") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간
                        if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴(부수술)
                        if (FstrNgt == "7") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간(부수술)
                        if (FstrNgt == "9") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                                                                                                                  //소아가산
                        if (FstrGisul == "1" && FstrGbChild == "1") { GstrPCode = VB.Left(GstrPCode, 5) + "8" + VB.Right(GstrPCode, 2); }
                    }
                }
                else if (clsPmpaType.ISG.SugbAA == "2")  //응급가산 별표2----------------------------------------------------------------------
                {
                    if (clsPmpaType.TIT.KTASLEVL == "1" || clsPmpaType.TIT.KTASLEVL == "2" || clsPmpaType.TIT.KTASLEVL == "3")
                    {
                        GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                        GstrPCode = VB.Left(GstrPCode, 5) + "7" + VB.Right(GstrPCode, 2);   //응급
                        if (FstrNgt == "1") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴
                        if (FstrNgt == "2") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간
                        if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴(부수술)
                        if (FstrNgt == "7") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간(부수술)
                        if (FstrNgt == "9") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                    }
                    //소아가산
                    if ((clsPmpaType.TIT.KTASLEVL != "0" || clsPmpaType.TIT.KTASLEVL == "") && FstrGisul == "1" && FstrGbChild == "1") { GstrPCode = VB.Left(GstrPCode, 5) + "8" + VB.Right(GstrPCode, 2); }
                }
                else if (clsPmpaType.ISG.SugbAA == "3")  //응급가산 별표3----------------------------------------------------------------------
                {
                    if (clsPmpaType.TIT.KTASLEVL == "1" || clsPmpaType.TIT.KTASLEVL == "2" || clsPmpaType.TIT.KTASLEVL == "3")
                    {
                        GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                        GstrPCode = VB.Left(GstrPCode, 5) + "7" + VB.Right(GstrPCode, 2);   //응급
                        if (FstrNgt == "1") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴
                        if (FstrNgt == "2") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간
                        if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴(부수술)
                        if (FstrNgt == "7") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간(부수술)
                        if (FstrNgt == "9") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                    }
                    //소아가산
                    if ((clsPmpaType.TIT.KTASLEVL != "0" || clsPmpaType.TIT.KTASLEVL == "") && FstrGisul == "1" && FstrGbChild == "1") { GstrPCode = VB.Left(GstrPCode, 5) + "8" + VB.Right(GstrPCode, 2); }
                }
            }
        }

        private void CODE_10_Process(PsmhDb pDbCon)
        {            
            clsIuSentChk cISK = new clsIuSentChk();

            GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate);

            //소아 20%가산 SET
            //1999.11.15 방사선촬영 소아가산율 변경됨
            if (FstrGbChild.Trim() == "") { FstrGbChild = "0"; }
            if (GstrPCode != "JJJJJJ")
            { 
                if (FstrGbChild == "1")
                {
                    GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                    GstrPCode = VB.Left(GstrPCode, 5) + "3" + VB.Right(GstrPCode, 2);
                }

                if (clsPmpaType.ISG.SugbAA == "1") // 응급가산 별표1 --------------------------------------------------------------------
                {
                    if (clsPmpaType.TIT.KTASLEVL == "A")
                    {
                        GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                        GstrPCode = VB.Left(GstrPCode, 5) + "7" + VB.Right(GstrPCode, 2);   //응급
                        if (FstrNgt == "1") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴
                        if (FstrNgt == "2") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간
                        if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴(부수술)
                        if (FstrNgt == "7") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간(부수술)
                        if (FstrNgt == "9") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                                                                                                                    //소아가산
                        if (FstrGisul == "1" && FstrGbChild == "1") { GstrPCode = VB.Left(GstrPCode, 5) + "8" + VB.Right(GstrPCode, 2); }
                    }
                    else if (clsPmpaType.TIT.KTASLEVL == "1" || clsPmpaType.TIT.KTASLEVL == "2" || clsPmpaType.TIT.KTASLEVL == "3")
                    {
                        GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                        GstrPCode = VB.Left(GstrPCode, 5) + "7" + VB.Right(GstrPCode, 2);   //응급
                        if (FstrNgt == "1") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴
                        if (FstrNgt == "2") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간
                        if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴(부수술)
                        if (FstrNgt == "7") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간(부수술)
                        if (FstrNgt == "9") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                                                                                                                    //소아가산
                        if (FstrGisul == "1" && FstrGbChild == "1") { GstrPCode = VB.Left(GstrPCode, 5) + "8" + VB.Right(GstrPCode, 2); }
                    }
                    
                }
                else if (clsPmpaType.ISG.SugbAA == "2")  //응급가산 별표2----------------------------------------------------------------------
                {
                    if (clsPmpaType.TIT.KTASLEVL == "1" || clsPmpaType.TIT.KTASLEVL == "2" || clsPmpaType.TIT.KTASLEVL == "3")
                    {
                        GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                        GstrPCode = VB.Left(GstrPCode, 5) + "7" + VB.Right(GstrPCode, 2);   //응급
                        if (FstrNgt == "1") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴
                        if (FstrNgt == "2") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간
                        if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴(부수술)
                        if (FstrNgt == "7") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간(부수술)
                        if (FstrNgt == "9") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                                                                                                                  //소아가산
                        if (FstrGisul == "1" && FstrGbChild == "1") { GstrPCode = VB.Left(GstrPCode, 5) + "8" + VB.Right(GstrPCode, 2); }
                    }
                }
                else if (clsPmpaType.ISG.SugbAA == "3") //응급가산 별표3----------------------------------------------------------------------
                {
                    GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                    GstrPCode = VB.Left(GstrPCode, 5) + "7" + VB.Right(GstrPCode, 2);   //응급
                    if (FstrNgt == "1") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴
                    if (FstrNgt == "2") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간
                    if (FstrNgt == "6") { GstrPCode = VB.Left(GstrPCode, 6) + "4" + VB.Right(GstrPCode, 1); } //응급공휴(부수술)
                    if (FstrNgt == "7") { GstrPCode = VB.Left(GstrPCode, 6) + "3" + VB.Right(GstrPCode, 1); } //응급야간(부수술)
                    if (FstrNgt == "9") { GstrPCode = VB.Left(GstrPCode, 6) + "2" + VB.Right(GstrPCode, 1); } //주간응급
                                                                                                              //소아가산
                    if (FstrGisul == "1" && FstrGbChild == "1") { GstrPCode = VB.Left(GstrPCode, 5) + "8" + VB.Right(GstrPCode, 2); }
                }
            }

        }

        private void CODE_11_Process(PsmhDb pDbCon)
        {
            clsIuSentChk cISK = new clsIuSentChk();

            GstrPCode = cISK.Rtn_Bas_Sun_BCode(pDbCon, FstrSuNext, clsPublic.GstrSysDate);

            if (FstrGbChild.Trim() == "") { FstrGbChild = "0"; }
            if (GstrPCode != "JJJJJJ")
            {
                if (FstrGbChild == "1")
                {
                    GstrPCode = VB.Left(GstrPCode + "00000000", 8);
                    GstrPCode = VB.Left(GstrPCode, 5) + "3" + VB.Right(GstrPCode, 2);
                }
            }
        }

        public string EDI_HangMok_SET(string ArgBun)
        {
            string rtnVal = string.Empty;

            //항목번호 SET
            if (string.Compare(ArgBun, "01") >= 0 && string.Compare(ArgBun, "02") <= 0)
            {
                rtnVal = "01"; //진찰료
            }
            else if (string.Compare(ArgBun, "03") >= 0 && string.Compare(ArgBun, "10") <= 0)
            {
                rtnVal = "02"; //입원료
            }
            else if (string.Compare(ArgBun, "11") >= 0 && string.Compare(ArgBun, "15") <= 0)
            {
                rtnVal = "03"; //투약및처방전료
            }
            else if (string.Compare(ArgBun, "16") >= 0 && string.Compare(ArgBun, "21") <= 0)
            {
                rtnVal = "04"; //주사료
            }
            else if (string.Compare(ArgBun, "22") >= 0 && string.Compare(ArgBun, "23") <= 0)
            {
                rtnVal = "05"; //마취료
            }
            else if (string.Compare(ArgBun, "24") >= 0 && string.Compare(ArgBun, "25") <= 0)
            {
                rtnVal = "06"; //물리치료
            }
            else if (string.Compare(ArgBun, "26") >= 0 && string.Compare(ArgBun, "27") <= 0)
            {
                rtnVal = "07"; //정신요법료
            }
            else if (string.Compare(ArgBun, "28") >= 0 && string.Compare(ArgBun, "40") <= 0)
            {
                rtnVal = "08"; //처치및수술료
            }
            else if (string.Compare(ArgBun, "41") >= 0 && string.Compare(ArgBun, "64") <= 0)
            {
                rtnVal = "09"; //검사료
            }
            else if (string.Compare(ArgBun, "65") >= 0 && string.Compare(ArgBun, "70") <= 0)
            {
                rtnVal = "10"; //방사선
            }
            else if (ArgBun == "72")
            {
                rtnVal = "C "; //CT
            }
            else if (ArgBun == "73")
            {
                rtnVal = "M "; //MRI
            }
            else if (ArgBun == "74")
            {
                rtnVal = "02"; //보호식대 2006년06월부터 보험식대포함
            }
            else if (ArgBun == "78")
            {
                rtnVal = "S "; //PET-CT
            }
            else if (ArgBun == "80")
            {
                rtnVal = "02"; //보호안치료
            }
            else
            {
                rtnVal = "XX";
            }

            return rtnVal;

        }

        public long READ_행위별진료비총액_급여(PsmhDb pDbCon, long ArgTRSNO,string ArgInDate)
        {
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0;
            long rtnVal = 0, nAmt1 = 0, nAmt2 = 0, nAmt3 = 0;

            Gn행위별총액 = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(SUM(a.Amt1+a.Amt2), 0) TAmt               ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b       ";
            SQL += ComNum.VBLF + "  WHERE a.Sunext=b.Sunext(+)                  ";
            SQL += ComNum.VBLF + "    AND a.TRSNO = " + ArgTRSNO + "            ";
            SQL += ComNum.VBLF + "    AND a.Sunext NOT IN ('DRG001','DRG002')   ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (Dt.Rows.Count > 0)
            {
                Gn행위별총액 = Convert.ToInt64(Dt.Rows[0]["TAMT"].ToString()); 
            }
            else
            {
                Dt.Dispose();
                Dt = null;
                return rtnVal;
            }

            Dt.Dispose();
            Dt = null;

            //90만원 이하면 빠져나감
            if (Gn행위별총액 < 900000)
            {
                return rtnVal;
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(SUM(a.Amt1+a.Amt2), 0) TAmt               ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b       ";
            SQL += ComNum.VBLF + "  WHERE a.Sunext=b.Sunext(+)                  ";
            SQL += ComNum.VBLF + "    AND a.TRSNO = " + ArgTRSNO + "            ";
            SQL += ComNum.VBLF + "    AND a.Sunext NOT IN ('DRG001','DRG002')   ";
            SQL += ComNum.VBLF + "    AND GbSelf ='0'     ";
            SQL += ComNum.VBLF + "    AND GBSUGBS IN ('2','3','4','5','6','7','8','9')      ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (Dt.Rows.Count > 0)
            {
                nAmt3 = Convert.ToInt64(Dt.Rows[0]["TAMT"].ToString());
                GnDrg열외군선별 = Convert.ToInt64(Dt.Rows[0]["TAMT"].ToString());
            }
           
            Dt.Dispose();
            Dt = null;



            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.SuCode, a.BUN, SUM(a.Amt1) Amt      ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b       ";
            SQL += ComNum.VBLF + "  WHERE a.Sunext=b.Sunext(+)                  ";
            SQL += ComNum.VBLF + "    AND a.TRSNO = " + ArgTRSNO + "            ";
            SQL += ComNum.VBLF + "    AND a.Sunext NOT IN ('DRG001','DRG002')   ";
            SQL += ComNum.VBLF + "  GROUP BY a.SUCODE,a.Bun                     ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (Dt.Rows.Count > 0)
            {
                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    //전액본인부담
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SugbA ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUT a, ";
                    SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b ";
                    SQL += ComNum.VBLF + "  WHERE Sucode = '" + Dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";
                    SQL += ComNum.VBLF + "    AND a.SuNext = b.SuNext(+) ";
                    SQL += ComNum.VBLF + "    AND b.DRG100 = 'Y' ";
                    if (Gn재왕절개본인부담율 > 0)
                    {
                        SQL += ComNum.VBLF + "    AND Sucode not in ('N-FT-PC','N-FT10','N-FE10','N-FE-PC')   ";
                    }
                    SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (Dt2.Rows.Count > 0)
                    {
                        nAmt1 += Convert.ToInt64(Dt.Rows[i]["Amt"].ToString()); 
                    }
                    Dt2.Dispose();
                    Dt2 = null;


                    //DRG 비급여 계산
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SugbA ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUT a, ";
                    SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b ";
                    SQL += ComNum.VBLF + "  WHERE Sucode = '" + Dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";
                    SQL += ComNum.VBLF + "    AND a.SuNext = b.SuNext(+) ";
                    SQL += ComNum.VBLF + "    AND b.DRGF ='Y' ";
                    SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (Dt2.Rows.Count > 0)
                    { 
                        nAmt2 += Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                    }
                    Dt2.Dispose();
                    Dt2 = null;
                }
            }
            // ''전액본인 제외 및 drg선별-> 행위별선별로 변경적용
            if (string.Compare(ArgInDate, "2019-07-01") >= 0)
            {
                rtnVal = Gn행위별총액 - ( nAmt2 + GnDrgSelTAmt + nAmt3); 
            }
            else
            {
                rtnVal = Gn행위별총액 - (nAmt1 + nAmt2 + GnDrgSelTAmt + GnGs80Amt_B + GnGs50Amt_B + GnGs90Amt_B ); //kyo 수정 2017-05-14
            }
            

            Dt.Dispose();
            Dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 당일 퇴원자 DRG001, DRG002발생건 Ipd_New_Slip 수가 삭제하기
        /// Author : 김민철
        /// Create Date : 2018.02.24
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTrsNo"></param>
        /// <returns></returns>
        public bool IPD_DRG_SLIP_DELETE(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, long ArgTrsNo)
        {
            bool rtnVal = true;
            
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            ComFunc.ReadSysDate(pDbCon);

            try
            {
                //당일건은 DRG발생건 삭제처리
                SQL = "";
                SQL += " INSERT INTO " + ComNum.DB_PMPA + " IPD_NEW_SLIP (                                          \r\n";
                SQL += "        IPDNO,TRSNO,ACTDATE,PANO,BI,BDATE,ENTDATE,SUNEXT,BUN,NU,QTY,NAL,BASEAMT,            \r\n";
                SQL += "        GBSPC,GBNGT,GBGISUL,GBSELF,GBCHILD,DEPTCODE,DRCODE,WARDCODE,SUCODE,GBSLIP,GBHOST,   \r\n";
                SQL += "        PART,AMT1,AMT2,SEQNO,YYMM,DRGSELF,ORDERNO,ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,    \r\n";
                SQL += "        ORDER_DCT,EXAM_WRTNO,ROOMCODE,DIV,AMT3,WRTNO,BUILDDATE,GBSELNOT,GBSUGBS,GBOP,       \r\n";
                SQL += "        PART2,BONRATE,GBER,GBER2,GBSGADD,CBUN,CSUNEXT,CSUCODE,GBSUGBAB,GBSUGBAC,GBSUGBAD,   \r\n";
                SQL += "        BCODE )                                                                             \r\n"; 
                SQL += " SELECT IPDNO,TRSNO,TRUNC(SYSDATE),PANO,BI,BDATE,SYSDATE,SUNEXT,BUN,NU,QTY,NAL*-1,BASEAMT,  \r\n";
                SQL += "        GBSPC,GBNGT,GBGISUL,GBSELF,GBCHILD,DEPTCODE,DRCODE,WARDCODE,SUCODE,GBSLIP,GBHOST,   \r\n";
                SQL += "        '" + clsType.User.IdNumber + "', AMT1*-1, AMT2*-1, SEQNO,YYMM,DRGSELF,ORDERNO,      \r\n";
                SQL += "        ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,EXAM_WRTNO,ROOMCODE, DIV,AMT3,      \r\n";
                SQL += "        WRTNO,BUILDDATE,GBSELNOT,GBSUGBS,GBOP,PART2,BONRATE,GBER,GBER2,GBSGADD,CBUN,        \r\n";
                SQL += "        CSUNEXT,CSUCODE,GBSUGBAB,GBSUGBAC,GBSUGBAD,BCODE                                    \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                                  \r\n";
                SQL += "  WHERE TRSNO = " + ArgTrsNo + "                                                            \r\n";
                SQL += "    AND IPDNO = " + ArgIpdNo + "                                                            \r\n";
                SQL += "    AND PANO  ='" + ArgPano + "'                                                            \r\n";
                SQL += "    AND ActDate=TRUNC(SYSDATE)                                                              \r\n";
                SQL += "    AND SuNext IN ('DRG001','DRG002')                                                       \r\n";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// Description : DRG00X DRG차액 Slip 추가 
        /// Author : 김민철
        /// Create Date : 2018.02.28
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSuCode"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTRSNO"></param>
        /// <param name="ArgActdate"></param>
        /// <param name="ArgAmt"></param>
        /// <returns></returns>
        public bool IPD_DRG_SLIP_INSERT(PsmhDb pDbCon, string ArgSuCode, string ArgPano, long ArgIpdNo, long ArgTRSNO, string ArgActdate, long ArgAmt)
        {
            bool rtnVal = true;
            
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            long nAmt3 = ArgAmt;
            long nDrgAmt1 = 0, nDrgAmt2 = 0;
            long nChaAmt = 0;

            clsPmpaPb cPb = new clsPmpaPb();
            clsPmpaFunc cPF = new clsPmpaFunc();

            ComFunc.ReadSysDate(pDbCon);

            //================ 참고사항 ===================
            //ArgSuCode = DRG001 : 급여   공단부담금 차액코드
            //ArgSuCode = DRG002 : 비급여 본인부담금 차액코드
            //=============================================

            try
            {
                //당일건은 DRG발생건 삭제처리
                SQL = "";
                SQL += " INSERT INTO " + ComNum.DB_PMPA + " IPD_NEW_SLIP (                                          \r\n";
                SQL += "        IPDNO,TRSNO,ACTDATE,PANO,BI,BDATE,ENTDATE,SUNEXT,BUN,NU,QTY,NAL,BASEAMT,            \r\n";
                SQL += "        GBSPC,GBNGT,GBGISUL,GBSELF,GBCHILD,DEPTCODE,DRCODE,WARDCODE,SUCODE,GBSLIP,GBHOST,   \r\n";
                SQL += "        PART,AMT1,AMT2,SEQNO,YYMM,DRGSELF,ORDERNO,ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,    \r\n";
                SQL += "        ORDER_DCT,EXAM_WRTNO,ROOMCODE,DIV,AMT3,WRTNO,BUILDDATE,GBSELNOT,GBSUGBS,GBOP,       \r\n";
                SQL += "        PART2,BONRATE,GBER,GBER2,GBSGADD,CBUN,CSUNEXT,CSUCODE,GBSUGBAB,GBSUGBAC,GBSUGBAD,   \r\n";
                SQL += "        BCODE )                                                                             \r\n";
                SQL += " SELECT IPDNO,TRSNO,TRUNC(SYSDATE),PANO,BI,BDATE,SYSDATE,SUNEXT,BUN,NU,QTY,NAL*-1,BASEAMT,  \r\n";
                SQL += "        GBSPC,GBNGT,GBGISUL,GBSELF,GBCHILD,DEPTCODE,DRCODE,WARDCODE,SUCODE,GBSLIP,GBHOST,   \r\n";
                SQL += "        '" + clsType.User.IdNumber + "', AMT1*-1, AMT2*-1, SEQNO,YYMM,DRGSELF,ORDERNO,      \r\n";
                SQL += "        ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,EXAM_WRTNO,ROOMCODE, DIV,AMT3,      \r\n";
                SQL += "        WRTNO,BUILDDATE,GBSELNOT,GBSUGBS,GBOP,PART2,BONRATE,GBER,GBER2,GBSGADD,CBUN,        \r\n";
                SQL += "        CSUNEXT,CSUCODE,GBSUGBAB,GBSUGBAC,GBSUGBAD,BCODE                                    \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                                  \r\n";
                SQL += "  WHERE TRSNO = " + ArgTRSNO + "                                                            \r\n";
                SQL += "    AND IPDNO = " + ArgIpdNo + "                                                            \r\n";
                SQL += "    AND PANO  ='" + ArgPano + "'                                                            \r\n";
                SQL += "    AND ActDate = TO_DATE('" + ArgActdate + "','YYYY-MM-DD')                                \r\n";
                SQL += "    AND SuNext = '" + ArgSuCode + "'                                                        \r\n";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                nDrgAmt1 = GnDRG_TAmt - GnDrgBiTAmt;     //DRG 급여총액
                nDrgAmt2 = GnDrgBiTAmt;                  //DRG 비급여총액

                if (ArgSuCode == "DRG001")
                {
                    if (nDrgAmt1 != nAmt3)
                    {
                        nChaAmt = nDrgAmt1 - nAmt3;

                        #region Ipd_New_Slip Data Set
                        cPb.ArgV = new string[Enum.GetValues(typeof(clsPmpaPb.enmIpdNewSlip)).Length];
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.IPDNO] = ArgIpdNo.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.TRSNO] = ArgTRSNO.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ACTDATE] = ArgActdate;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PANO] = ArgPano;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BI] = clsPmpaType.TIT.Bi;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BDATE] = clsPmpaType.TIT.OutDate == "" ? clsPublic.GstrSysDate : clsPmpaType.TIT.OutDate;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUNEXT] = "DRG001";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BUN] = "34";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NU] = "10";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.QTY] = "1";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NAL] = nChaAmt > 0 ? "1" : "-1";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BASEAMT] = nChaAmt.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSPC] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBNGT] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBGISUL] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELF] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBCHILD] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DEPTCODE] = clsPmpaType.TIT.DeptCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRCODE] = clsPmpaType.TIT.DrCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.WARDCODE] = clsPmpaType.TIT.WardCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUCODE] = "DRG001";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSLIP] = " ";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBHOST] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PART] = clsType.User.IdNumber;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT1] = nChaAmt.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT2] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SEQNO] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.YYMM] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRGSELF] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDERNO] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ABCDATE] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DEPT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DCT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DEPT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DCT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.EXAM_WRTNO] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.RoomCode] = clsPmpaType.TIT.RoomCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DIV] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELNOT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBS] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBER] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CBUN] = "340";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUCODE] = "DRG001";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUNEXT] = "DRG001";
                        #endregion
                        SqlErr = cPF.Ins_IpdNewSlip(cPb.ArgV, pDbCon, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            return false;
                        }
                    }
                }
                else if (ArgSuCode == "DRG002")
                {
                    if (nDrgAmt2 != nAmt3)
                    {
                        nChaAmt = nDrgAmt2 - nAmt3;

                        #region Ipd_New_Slip Data Set
                        cPb.ArgV = new string[Enum.GetValues(typeof(clsPmpaPb.enmIpdNewSlip)).Length];
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.IPDNO] = ArgIpdNo.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.TRSNO] = ArgTRSNO.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ACTDATE] = ArgActdate;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PANO] = ArgPano;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BI] = clsPmpaType.TIT.Bi;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BDATE] = clsPmpaType.TIT.OutDate == "" ? clsPublic.GstrSysDate : clsPmpaType.TIT.OutDate;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUNEXT] = "DRG002";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BUN] = "34";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NU] = "28";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.QTY] = "1";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NAL] = nChaAmt > 0 ? "1" : "-1";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BASEAMT] = nChaAmt.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSPC] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBNGT] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBGISUL] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELF] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBCHILD] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DEPTCODE] = clsPmpaType.TIT.DeptCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRCODE] = clsPmpaType.TIT.DrCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.WARDCODE] = clsPmpaType.TIT.WardCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUCODE] = "DRG002";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSLIP] = " ";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBHOST] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PART] = clsType.User.IdNumber;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT1] = nChaAmt.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT2] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SEQNO] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.YYMM] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRGSELF] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDERNO] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ABCDATE] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DEPT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DCT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DEPT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DCT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.EXAM_WRTNO] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.RoomCode] = clsPmpaType.TIT.RoomCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DIV] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELNOT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBS] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBER] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CBUN] = "340";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUCODE] = "DRG002";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUNEXT] = "DRG002";

                        #endregion
                        SqlErr = cPF.Ins_IpdNewSlip(cPb.ArgV, pDbCon, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            return false;
                        }    
                    }
                }

                //TRANS AMT65, AMT66에 금액저장
                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS      ";
                SQL += "    SET AMT65 = " + nDrgAmt1 + ",            ";
                SQL += "        AMT66 = " + nDrgAmt2 + "             ";
                SQL += "  WHERE TRSNO = " + ArgTRSNO + "             ";
                SQL += "    AND IPDNO = " + ArgIpdNo + "             ";
                SQL += "    AND PANO  ='" + ArgPano + "'             ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                
                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// Description : DRG 수가중 단일, 복합, 루틴인지 판별
        /// Author : 김민철
        /// Create Date : 2018.03.23
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSuCode"></param>
        /// <param name="ArgGbn"></param>
        /// <returns></returns>
        public string READ_DRG_100(PsmhDb pDbCon, string ArgSuCode, string ArgGbn, string ArgIndate)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SugbA                             ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUT a,  ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b,   ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUGA_DRGADD_NEW c   ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1                             ";
            SQL += ComNum.VBLF + "    AND a.SUCODE = '" + ArgSuCode + "'      ";
            SQL += ComNum.VBLF + "    AND a.SuNext = b.SuNext(+)            ";
            SQL += ComNum.VBLF + "    AND a.SuNext = c.SuNext(+)            ";
            SQL += ComNum.VBLF + "    AND c.SUDATE <= TO_DATE('" + ArgIndate + "','YYYY-MM-DD')       ";    //2014-07-24 적용일자 추가
            

            if (ArgGbn == "NEW")
            {
                SQL += ComNum.VBLF + "    AND c.DRG100 IS NOT NULL          ";
            }
            else
            {
                SQL += ComNum.VBLF + "    AND c.DRG100 = 'Y'                ";
            }
            if (Gn재왕절개본인부담율 > 0)
            {
                SQL += ComNum.VBLF + "    AND Sucode not in ('N-FT-PC','N-FT10','N-FE10','N-FE-PC')          ";
            }
          
            SQL += ComNum.VBLF + "    order by c.SUDATE DESC ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["SugbA"].ToString().Trim() != "")
                {
                    rtnVal = "OK";
                }
            }

            dt.Dispose();
            dt = null;
            

            return rtnVal;
        }
        
        public string READ_DRG_CODE_NAME(PsmhDb pDbCon, string ArgCode)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DNAME                             ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "DRG_CODE_NEW   ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1                             ";
            SQL += ComNum.VBLF + "    AND DCODE = '" + ArgCode + "'      ";
            
            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["DNAME"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;


            return rtnVal;
        }

    }
}
