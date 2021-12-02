using System;
using System.Data;
using System.Windows.Forms;
using ComBase;
using ComDbB;

namespace ComNurLibB
{
    public class clsIpdNr
    {
        /// <summary>
        /// <seealso cref="Nrinfo00.bas"/> 
        /// </summary>
        #region 
        public string gsWard = "";
        public string GstrWardCodeCSR = "";                 //공급실 부서코드에 사용
        public string GstrWardNameCSR = "";                 //공급실 부서코드에 사용
        public string GstrDietPano = "";
        public string GstrPoint = "";                       //APACHE 점수
        public string GstrFlag = "";
        public string GstrJewon = "";
        public string GstrLock = "";

        public string[] GstrSETBis = new string[99];
        public string[] GstrSETNus = new string[60];        //누적행위명 안내 Array
        public string[] GstrSETSpcs = new string[9];        //특진구분   안내 Array
        public string[] GstrSETAmSet1s = new string[9];     //환자재원 상태

        public string GstrOrderTimeChk = "";
        public string GstrSpecialExamROWID = "";            //특수검사 ROWID
        public string GstrSpecialExamFlag = "";
        public string GstrJobGbreg = "";
        public string mstrNewChartCd = "";                  //투약기록지

        public long GnIPDNO = 0;

        public static int intLabelX = 0;
        public static int intLabelY = 0;

        #endregion

        public static string ReadCP_NameDay(PsmhDb DbCon, string strPANO, string strIPDNO, string argGbn = "0")
        {
            //이름만 리턴해줍니다. 진행상황은 ORACLE FUNCTION사용합니다.
            //KOSMOS_PMPA.READ_CP_PROGRESS_NUR

            string RtnVal = "";     //return 값이 Y이면 복사 신청 인쇄 내역 있음!
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                //SQL = " SELECT CPCODE, (SELECT BASNAME FROM KOSMOS_PMPA.BAS_BASCD WHERE GRPCDB = 'CP관리' AND GRPCD = 'CP코드관리' AND  BASCD = A.CPCODE) CPNAME  ";
                //SQL += ComNum.VBLF + "          (SELECT CPDAY FROM KOSMOS_OCS.OCS_CP_MAIN M ";
                //SQL += ComNum.VBLF + "            WHERE SDATE = (SELECT MAX(SDATE) FROM KOSMOS_OCS.OCS_CP_MAIN WHERE CPCODE = M.CPCODE) ";
                //SQL += ComNum.VBLF + "              AND CPCODE = A.CPCODE AND ROWNUM = 1) CPDAY, ";
                //SQL += ComNum.VBLF + "          CASE  ";
                //SQL += ComNum.VBLF + "          WHEN CANCERDATE IS NOT NULL THEN '중단(' || (A.CANCERDATE - A.STARTDATE) ";
                //SQL += ComNum.VBLF + "          WHEN DROPDATE IS NOT NULL THEN '제외(' || (A.DROPDATE - A.STARTDATE) ";
                //SQL += ComNum.VBLF + "          WHEN STARTDATE IS NOT NULL THEN '진행 중(' || (TO_CHAR(SYSDATE, 'YYYYMMDD') - A.STARTDATE) ";
                //SQL += ComNum.VBLF + "          ELSE '' END GUBUN ";
                //SQL += ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_CP_RECORD A ";
                //SQL += ComNum.VBLF + " WHERE IPDNO = " + strIPDNO;
                //SQL += ComNum.VBLF + "  AND PANO = '" + strPANO + "' ";

                SQL = " SELECT (SELECT BASNAME FROM KOSMOS_PMPA.BAS_BASCD WHERE GRPCDB = 'CP관리' AND GRPCD = 'CP코드관리' AND  BASCD = A.CPCODE AND ROWNUM = 1) CPNAME, ";
                SQL += ComNum.VBLF + "     CASE WHEN CANCERDATE IS NOT NULL THEN '중단(' || (A.CANCERDATE - A.STARTDATE + 1) || '/' || B.CPDAY || ')' ";
                SQL += ComNum.VBLF + "          WHEN DROPDATE IS NOT NULL THEN '제외(' || (A.DROPDATE - A.STARTDATE + 1) || '/' || B.CPDAY || ')' ";
                SQL += ComNum.VBLF + "          WHEN STARTDATE IS NOT NULL THEN '♠적용(' || (TO_CHAR(SYSDATE, 'YYYYMMDD') - A.STARTDATE + 1) || '/' || B.CPDAY || ')' ";
                SQL += ComNum.VBLF + "          ELSE '' END GUBUN ";
                SQL += ComNum.VBLF + "    FROM KOSMOS_OCS.OCS_CP_RECORD A ";
                SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_OCS.OCS_CP_MAIN B ";
                SQL += ComNum.VBLF + "      ON A.CPCODE = B.CPCODE ";
                SQL += ComNum.VBLF + "     AND B.SDATE = (SELECT MAX(SDATE) FROM KOSMOS_OCS.OCS_CP_MAIN WHERE CPCODE = A.CPCODE)                ";
                SQL += ComNum.VBLF + "   WHERE IPDNO = " + strIPDNO;
                SQL += ComNum.VBLF + "     AND CPNO = (SELECT MAX(CPNO) FROM KOSMOS_OCS.OCS_CP_RECORD WHERE IPDNO = A.IPDNO) ";
                SQL += ComNum.VBLF + "     AND ROWNUM = 1 ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (argGbn == "0")
                    {
                        RtnVal = dt.Rows[0]["CPNAME"].ToString().Trim();
                    }
                    else
                    {
                        RtnVal = dt.Rows[0]["GUBUN"].ToString().Trim() + " " + dt.Rows[0]["CPNAME"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return RtnVal;

        }

        public static void ShowNewCKnow(PsmhDb DbCon, string argSABUN)
        {
            //새로운 공지가 있을 때 띄운다. 
            //타이머?? 아니면 버튼마다??
            //일단 주요 이벤트 끝부분에~~
            //2021-04-01 이후 건 부터 표시.(이전 데이터 너무 많음)

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strNew = "";

            frmCommonKnowList frmCommonKnowListX = null;

            if (argSABUN == "")
            {
                return;
            }

            argSABUN = Convert.ToString(Convert.ToInt32(argSABUN));

            try
            {
                SQL = " SELECT SABUN FROM KOSMOS_ADM.INSA_MST ";
                SQL = SQL + ComNum.VBLF + "  WHERE SABUN = '" + argSABUN + "' ";
                SQL = SQL + ComNum.VBLF + "    AND BUSE IN ( ";
                SQL = SQL + ComNum.VBLF + "         SELECT MATCH_CODE FROM KOSMOS_PMPA.NUR_CODE ";
                SQL = SQL + ComNum.VBLF + "          WHERE GUBUN = '2' ";
                SQL = SQL + ComNum.VBLF + "            AND GBUSE = 'Y') ";
                SQL = SQL + ComNum.VBLF + "    AND TOIDAY IS NULL ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0) { }
                else
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }
                dt.Dispose();
                dt = null;



                SQL = "";
                SQL = " SELECT WRTNO";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMR_CADEX_CKNOW";
                SQL = SQL + ComNum.VBLF + " WHERE  (DELDATE >= TRUNC(SYSDATE))";
                SQL = SQL + ComNum.VBLF + "    AND BUCODE = '033100'";
                SQL = SQL + ComNum.VBLF + "    AND CDATE >= (SELECT KUNDAY FROM KOSMOS_ADM.INSA_MST WHERE SABUN = '" + argSABUN + "') ";
                SQL = SQL + ComNum.VBLF + "    AND TRUNCDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND CDATE >= TO_DATE('2021-04-01','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND WRTNO NOT IN (";
                SQL = SQL + ComNum.VBLF + "               SELECT WRTNO";
                SQL = SQL + ComNum.VBLF + "               FROM KOSMOS_EMR.EMR_CADEX_CKNOW_CLICK";
                SQL = SQL + ComNum.VBLF + "               WHERE SABUN = " + argSABUN + ")";
                SQL = SQL + ComNum.VBLF + "    AND (PHARMACY IS NULL OR (PHARMACY = '1' AND PH_SIGN1_SABUN IS NOT NULL))";
                SQL = SQL + ComNum.VBLF + "  UNION ALL";
                SQL = SQL + ComNum.VBLF + "  SELECT WRTNO";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMR_CADEX_CKNOW";
                SQL = SQL + ComNum.VBLF + " WHERE  (DELDATE IS NULL)";
                SQL = SQL + ComNum.VBLF + "    AND BUCODE = '033100'";
                SQL = SQL + ComNum.VBLF + "    AND TRUNCDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND CDATE >= TO_DATE('2021-04-01','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND CDATE >= (SELECT KUNDAY FROM KOSMOS_ADM.INSA_MST WHERE SABUN = '" + argSABUN + "') ";
                SQL = SQL + ComNum.VBLF + "    AND WRTNO NOT IN (";
                SQL = SQL + ComNum.VBLF + "               SELECT WRTNO";
                SQL = SQL + ComNum.VBLF + "               FROM KOSMOS_EMR.EMR_CADEX_CKNOW_CLICK";
                SQL = SQL + ComNum.VBLF + "               WHERE SABUN = " + argSABUN + ")";
                SQL = SQL + ComNum.VBLF + "    AND (PHARMACY IS NULL OR (PHARMACY = '1' AND PH_SIGN1_SABUN IS NOT NULL))";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strNew = "OK";
                }

                dt.Dispose();
                dt = null;

                if (strNew == "OK")
                {
                    using (frmCommonKnowListX = new frmCommonKnowList())
                    {
                        frmCommonKnowListX.StartPosition = FormStartPosition.CenterParent;
                        frmCommonKnowListX.ShowDialog();
                    }
                }

                return;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        public static void CHK_USELOG(PsmhDb DbCon, string strUseWard)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(DbCon);

            strUseWard = strUseWard + "-IPDNR";

            try
            {
                SQL = "";
                SQL = "INSERT INTO KOSMOS_PMPA.BACK_ERLOG (JOBATE, PANO, GUBUN, IPSABUN, IPADDR)";
                SQL = SQL + ComNum.VBLF + " VALUES(SYSDATE, '99999999', '" + strUseWard + "', '" + clsType.User.Sabun + "','" + clsCompuInfo.gstrCOMIP + "')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        public static string READ_PRTLOG(PsmhDb pDbCon, string argEMRNO)
        {
            string RtnVal = "";     //return 값이 Y이면 복사 신청 인쇄 내역 있음!
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT EMRNO, PRINTDATE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRPRTREQ ";
                SQL += ComNum.VBLF + " WHERE EMRNO = " + argEMRNO;
                SQL += ComNum.VBLF + "  AND PRINTYN = 'Y' ";
                SQL += ComNum.VBLF + " ORDER BY PRINTDATE DESC ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = "Y";
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return RtnVal;

        }


        public static string READ_PRTREQLOG(PsmhDb pDbCon, string argEMRNO)
        {
            string RtnVal = "";     //return 값이 Y이면 복사 신청 내역 있음!
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT EMRNO, PRINTDATE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRPRTREQ ";
                SQL += ComNum.VBLF + " WHERE EMRNO = " + argEMRNO;
                SQL += ComNum.VBLF + " ORDER BY PRINTDATE DESC ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = "Y";
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return RtnVal;

        }

        public static string READ_AST_REACTION(PsmhDb pDbCon, string ArgSuCode, string argPTNO, string ArgInDate)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (READ_AST_미해당(pDbCon, ArgSuCode, READ_AGE_GESAN(pDbCon, argPTNO)) == "미해당")
            {
                RtnVal = "미해당";
                return RtnVal;
            }

            try
            {
                SQL = " SELECT GBN ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_AST ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE = TO_DATE('" + ArgInDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ORDERCODE = '" + ArgSuCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = dt.Rows[0]["GBN"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return RtnVal;
        }

        private static string READ_AST_미해당(PsmhDb pDbCon, string argSuCode, string argAge)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Left(argSuCode, 2) == "W-" || VB.Left(argSuCode, 3) == "KT-")
            {
            }
            else
            {
                return RtnVal;
            }

            //'W-RAMI 약제는 18개월 미만일 경우 항생제 테스트 미해당임
            //'나이에 m(개월수) 또는 d(일수)가 포함된 경우는 18개월 미만환아이므로 미해당 환자
            //'변수 캐스팅 에러로 별도 처리함.

            if (argSuCode == "W-RAMI")
            {
                if (argAge.IndexOf("m") > -1 || argAge.IndexOf("d") > -1)
                {
                    RtnVal = "미해당";
                    return RtnVal;
                }
                else if (VB.Val(argAge) < 18)
                {
                    RtnVal = "미해당";
                    return RtnVal;
                }
            }

            try
            {
                SQL = " SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_AST대상항생제'";
                SQL = SQL + ComNum.VBLF + "    AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND CODE = '" + argSuCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    RtnVal = "미해당";
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return RtnVal;
        }

        /// <summary>
        /// Description : 나이 계산, 기존 로직에서 1세미만일 경우 개월수 표시
        /// Author : 안정수
        /// Create Date : 2017.12.29   
        /// </summary>
        /// <param name="argPTNO"></param>
        /// <param name="ArgAge"></param>
        /// <returns></returns>
        public static string READ_AGE_GESAN(PsmhDb pDbCon, string argPTNO, string ArgAge = "")
        {
            //간호부 1세 미만의 경우 개월수 표시, 1세 이상일 경우 기존 나이 계산 로직 사용
            //2013-12-16 주임 김현욱

            string rtnVal = "";

            string strJumin = "";
            string strMonth = "";
            string strIlsu = "";

            string SQL = "";
            string SqlErr = "";
            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D");
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  TO_DATE(TODAY,'YYYY-MM-DD') - BIRTH + 1 ILSU,";
            // 2017-12-30 안정수, 오버플로우로 인하여 MONTH 칼럼값에 floor 추가
            SQL += ComNum.VBLF + "  floor(MONTHS_BETWEEN(TO_DATE(TODAY,'YYYY-MM-DD'), BIRTH)) MONTH, JUMIN1||JUMIN2 JUMIN";
            SQL += ComNum.VBLF + "FROM (";
            SQL += ComNum.VBLF + "          SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD') TODAY, CASE TRIM(SUBSTR(JUMIN2, 1, 1))";
            SQL += ComNum.VBLF + "          WHEN '1' THEN '19'";
            SQL += ComNum.VBLF + "          WHEN '2' THEN '19'";
            SQL += ComNum.VBLF + "          WHEN '5' THEN '19'";
            SQL += ComNum.VBLF + "          WHEN '6' THEN '19'";
            SQL += ComNum.VBLF + "          WHEN '3' THEN '20'";
            SQL += ComNum.VBLF + "          WHEN '4' THEN '20'";
            SQL += ComNum.VBLF + "          WHEN '7' THEN '20'";
            SQL += ComNum.VBLF + "          WHEN '8' THEN '20'";
            SQL += ComNum.VBLF + "          WHEN '9' THEN '18'";
            SQL += ComNum.VBLF + "          WHEN '0' THEN '18'";
            SQL += ComNum.VBLF + "          END NYEAR, JUMIN1, JUMIN2, BIRTH";
            SQL += ComNum.VBLF + "          FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
            SQL += ComNum.VBLF + "          WHERE PANO = '" + argPTNO + "')";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                //2014-05-16 birth 있는것만
                if (dt.Rows[0]["MONTH"].ToString().Trim() != "")
                {
                    strMonth = VB.Fix(Convert.ToInt32(dt.Rows[0]["MONTH"].ToString().Trim())).ToString();
                    strJumin = dt.Rows[0]["JUMIN"].ToString().Trim();
                    strIlsu = VB.Fix(Convert.ToInt32(dt.Rows[0]["ILSU"].ToString().Trim())).ToString();

                    //if(String.Compare(strIlsu, "30") <= 0)
                    if (Convert.ToInt32(strIlsu) <= 30)
                    {
                        rtnVal = strIlsu + "d";
                    }

                    //else if(String.Compare(strMonth, "12") < 0 && String.Compare(strIlsu, "31") >= 0)
                    else if (Convert.ToInt32(strMonth) < 12 && Convert.ToInt32(strIlsu) >= 31)
                    {
                        rtnVal = strMonth + "m";
                    }

                    else if (ArgAge != "")
                    {
                        rtnVal = ArgAge;
                    }

                    else
                    {
                        rtnVal = ComFunc.AgeCalcEx(strJumin.Trim(), strSysDate).ToString();
                    }
                }
            }

            if (rtnVal == "" || rtnVal == "0")
            {
                SQL = "SELECT AGE FROM NUR_MASTER ";
                SQL += ComNum.VBLF + " WHERE INDATE = TRUNC(SYSDATE) ";
                SQL += ComNum.VBLF + " AND PANO = '" + argPTNO + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["AGE"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCode"></param>
        /// <seealso cref="READ_성분명"/>
        /// <returns></returns>
        public static string GetDrugInfoSnaem(PsmhDb pDbCon, string argCode)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT SNAME";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_DRUGINFO_NEW";
                SQL = SQL + ComNum.VBLF + "WHERE SUNEXT = '" + argCode + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = dt.Rows[0]["SNAME"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return RtnVal;
        }


        public static string READ_ATTENTION(PsmhDb pDbCon, string argSuCode)
        {
            string RtnVal = "";

            if (clsOrderEtc.READ_SUGA_ANTIBLOOD(pDbCon, argSuCode) == "OK")
            {
                RtnVal = RtnVal + "★";
            }

            if (clsOrderEtc.READ_SUGA_COMPONENT(pDbCon, argSuCode) == "OK")
            {
                RtnVal = RtnVal + "<!>";
            }

            if (READ_SUGA_JEPCODE7(pDbCon, argSuCode) == "OK")
            {
                RtnVal = RtnVal + "Ω";
            }

            return RtnVal;
        }



        /// <summary>
        /// 챠트에서 최근 3일이내 최신 키(height)값 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPTNO"></param>
        /// <param name="argINDATE"></param>
        /// <seealso cref="READ_IPD_HEIGHT"/>
        /// <returns> 키(height) </returns>
        public static string READ_IPD_HEIGHT(PsmhDb pDbCon, string argPTNO, string argINDATE)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT TRANSLATE (ITEMVALUE, ";
                SQL += ComNum.VBLF + "                  '0' || TRANSLATE (ITEMVALUE, 'x0123456789.', 'x'), ";
                SQL += ComNum.VBLF + "                  '0') ";
                SQL += ComNum.VBLF + "          HEIGHT ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTROW ";
                SQL += ComNum.VBLF + " WHERE EMRNO = ";
                SQL += ComNum.VBLF + "          (SELECT MAX (EMRNO) EMRNO ";
                SQL += ComNum.VBLF + "             FROM KOSMOS_EMR.AEMRCHARTMST S1 ";
                SQL += ComNum.VBLF + "            WHERE PTNO = '" + argPTNO + "' ";
                SQL += ComNum.VBLF + "                  AND (CHARTDATE = '" + argINDATE.Replace("-", "") + "' ";
                SQL += ComNum.VBLF + "                       OR (CHARTDATE >= TO_CHAR(TRUNC(SYSDATE - 3),'YYYYMMDD') AND CHARTDATE <= TO_CHAR(TRUNC(SYSDATE),'YYYYMMDD'))) ";
                SQL += ComNum.VBLF + "                  AND EXISTS ";
                SQL += ComNum.VBLF + "                         (SELECT * ";
                SQL += ComNum.VBLF + "                            FROM KOSMOS_EMR.AEMRCHARTROW S2 ";
                SQL += ComNum.VBLF + "                           WHERE S1.EMRNO = S2.EMRNO ";
                SQL += ComNum.VBLF + "                                 AND S2.ITEMCD IN ('I0000000562', 'I0000000002') ";
                SQL += ComNum.VBLF + "                                 AND ITEMVALUE IS NOT NULL)) ";
                SQL += ComNum.VBLF + "       AND ITEMCD IN ('I0000000562', 'I0000000002') ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = dt.Rows[0]["HEIGHT"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return RtnVal;
        }


        /// <summary>
        /// 챠트에서 최근 3일이내 최신 몸무게(weight)값 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPTNO"></param>
        /// <seealso cref="READ_IPD_WEIGHT"/>
        /// <returns> 몸무게 값 </returns>
        public static string READ_IPD_WEIGHT(PsmhDb pDbCon, string argPTNO, string argINDATE)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT TRANSLATE (ITEMVALUE, ";
                SQL += ComNum.VBLF + "                  '0' || TRANSLATE (ITEMVALUE, 'x0123456789.', 'x'), ";
                SQL += ComNum.VBLF + "                  '0') ";
                SQL += ComNum.VBLF + "          WEIGHT ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTROW ";
                SQL += ComNum.VBLF + " WHERE EMRNO = ";
                SQL += ComNum.VBLF + "          (SELECT MAX (EMRNO) EMRNO ";
                SQL += ComNum.VBLF + "             FROM KOSMOS_EMR.AEMRCHARTMST S1 ";
                SQL += ComNum.VBLF + "            WHERE PTNO = '" + argPTNO + "' ";
                SQL += ComNum.VBLF + "                  AND (CHARTDATE = '" + argINDATE.Replace("-", "") + "' ";
                SQL += ComNum.VBLF + "                       OR (CHARTDATE >= TO_CHAR(TRUNC(SYSDATE - 3),'YYYYMMDD') AND CHARTDATE <= TO_CHAR(TRUNC(SYSDATE),'YYYYMMDD'))) ";
                SQL += ComNum.VBLF + "                  AND EXISTS ";
                SQL += ComNum.VBLF + "                         (SELECT * ";
                SQL += ComNum.VBLF + "                            FROM KOSMOS_EMR.AEMRCHARTROW S2 ";
                SQL += ComNum.VBLF + "                           WHERE S1.EMRNO = S2.EMRNO ";
                SQL += ComNum.VBLF + "                                 AND S2.ITEMCD = 'I0000000418' ";
                SQL += ComNum.VBLF + "                                 AND ITEMVALUE IS NOT NULL)) ";
                SQL += ComNum.VBLF + "       AND ITEMCD = 'I0000000418' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = dt.Rows[0]["WEIGHT"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return RtnVal;
        }

        public static double Fixed(double arg)
        {
            string strTmp = "";

            strTmp = arg.ToString("N3");

            return double.Parse(strTmp);
        }

        public static string GetBMI(string Height, string Weight)
        {
            double nHe = 0;
            double nWt = 0;

            double nH2 = 0;
            double nWt2 = 0;
            double nWt3 = 0;
            double nWt22 = 0;
            double nWt32 = 0;

            double nTmp = 0;
            double nTmp2 = 0;

            try
            {
                nHe = Convert.ToDouble(Height);
                nWt = Convert.ToDouble(Weight);

                nH2 = Math.Pow((nHe / 100), 2);
                nWt2 = 18.5 * nH2;
                nWt3 = 22.5 * nH2;
                nWt22 = Fixed(nWt2);
                nWt32 = Fixed(nWt3);

                nTmp = nWt / nH2;
                nTmp2 = Fixed(nTmp);

                return nTmp2.ToString();
            }
            catch
            {
                return "Error";
            }
        }

        /// <summary>
        /// ComLibB로 이관 2020-12-24 KMC
        /// </summary>
        /// <param name="Height"></param>
        /// <param name="Weight"></param>
        /// <returns></returns>
        //public static string GetBSA(string Height, string Weight)
        //{

        //    double nHe = 0;
        //    double nWt = 0;

        //    double nBSA0 = 0;
        //    double nBSA1 = 0;
        //    double nBSA2 = 0;

        //    try
        //    {
        //        nHe = Convert.ToDouble(Height);
        //        nWt = Convert.ToDouble(Weight);

        //        nBSA0 = (nHe * nWt) / 3600;
        //        nBSA1 = Math.Pow(nBSA0, 0.5);
        //        nBSA2 = Fixed(nBSA1);

        //        return nBSA2.ToString();
        //    }
        //    catch
        //    {
        //        return "Error";
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argSuCode"></param>
        /// <seealso cref="READ_SUGA_면역억제제"/>
        /// <returns></returns>
        public static string READ_SUGA_JEPCODE7(PsmhDb pDbCon, string argSuCode)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT SEQNO ";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_ADM.DRUG_SPECIAL_JEPCODE  ";
                SQL = SQL + ComNum.VBLF + " WHERE SEQNO = 7 ";
                SQL = SQL + ComNum.VBLF + "    AND JEPCODE = '" + argSuCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = "OK";
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return RtnVal;
        }

        public static string SlipNo_Gubun(string argSlipNo, string argDosCode, string argBun)
        {
            string rtnVal = "";
            int intSlipNo = 0;


            if (VB.IsNumeric(argSlipNo) == true)
            {
                intSlipNo = Convert.ToInt32(argSlipNo);

                if (intSlipNo == 3 || intSlipNo == 4)
                {
                    rtnVal = "Med" + VB.Space(6) + "999";
                }
                else if (intSlipNo == 5)
                {
                    if (argDosCode.Trim() == "97" || argDosCode.Trim() == "99")
                    {
                        rtnVal = "Med" + VB.Space(6) + "23";
                    }
                    else
                    {
                        rtnVal = "Med" + VB.Space(6) + "24";
                    }
                }
                else if (intSlipNo >= 10 && intSlipNo <= 42)
                {
                    rtnVal = "Lab" + VB.Space(6) + "17";
                }
                else if (intSlipNo == 44)
                {
                    if (argBun.Trim() == "78")
                    {
                        rtnVal = "Bmd      19";
                    }
                    else
                    {
                        rtnVal = "Endo     18";
                    }
                }
                else if ((intSlipNo >= 60 && intSlipNo <= 65)
                || intSlipNo == 67
                || (intSlipNo >= 69 && intSlipNo <= 80))
                {
                    rtnVal = "Xray" + VB.Space(6) + "14";
                }
                else if (intSlipNo == 66)
                {
                    rtnVal = "RI" + VB.Space(6) + "15";
                }
                else if (intSlipNo == 68)
                {
                    rtnVal = "Sono" + VB.Space(6) + "16";
                }
            }
            else
            {
                if (argSlipNo == "TEL")
                {
                    rtnVal = "TEL" + VB.Space(6) + "21";
                }
                else if (argSlipNo == "A1")
                {
                    rtnVal = "V/S" + VB.Space(6) + "11";
                }
                else if (argSlipNo == "A2")
                {
                    rtnVal = "S/O" + VB.Space(6) + "12";
                }
                else if (argSlipNo == "A4")
                {
                    rtnVal = "S/O" + VB.Space(6) + "13";
                }
                else if (argSlipNo == "OR1" || argSlipNo == "OR2")
                {
                    rtnVal = "OR" + VB.Space(6) + "22";
                }
            }

            if (rtnVal == "")
            {
                rtnVal = "Etc" + VB.Space(6) + "20";
            }

            return rtnVal;

        }

        /// <summary>
        /// Description : DR코드 SET
        /// Author : 안정수
        /// Create Date : 2017.12.28                
        /// <seealso cref="vbfunc.bas : ComboDrCode_SET"/>
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="cbo"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgAll"></param>
        /// <param name="ArgTYPE"></param>
        /// <param name="ArgMD"></param>
        public static void ComboDrCode_SET(PsmhDb pDbCon, ComboBox cbo, string ArgDept, string ArgAll = "", string ArgTYPE = "", string ArgMD = "")
        {
            //ARGType 1: 코드 + "." + 명칭
            //2: 코드
            //3: 명칭
            //ArgAll  1: **.전체 , 2: 내과(MD)일경우 세부내과 모두 표시
            //ArgMD   MD: 내과 (MD) 일경우 세부내과로 모든의사표시

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;

            if (ArgAll == "")
            {
                ArgAll = "1";
            }

            if (ArgTYPE == "")
            {
                ArgTYPE = "1";
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  DRCODE, DRNAME";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND TOUR <> 'Y'";
            if (ArgMD == "MD")
            {
                if (ArgDept == "MD")
                {
                    SQL += ComNum.VBLF + "AND ( DRDEPT1 = '" + ArgDept + "' OR DRDEPT1 LIKE 'M%' ) ";
                }

                else
                {
                    SQL += ComNum.VBLF + "AND DRDEPT1 = '" + ArgDept + "'";
                }
            }

            else
            {
                SQL += ComNum.VBLF + "AND DRDEPT1 = '" + ArgDept + "'";
            }
            SQL += ComNum.VBLF + "ORDER BY PRINTRANKING";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            cbo.Items.Clear();

            if (ArgAll == "1")
            {
                cbo.Items.Add("****.전체");
            }
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    switch (ArgTYPE)
                    {
                        case "1":
                            cbo.Items.Add(dt.Rows[i]["DRCode"].ToString().Trim() + "." + dt.Rows[i]["DRName"].ToString().Trim());
                            break;
                        case "2":
                            cbo.Items.Add(dt.Rows[i]["DRCode"].ToString().Trim());
                            break;
                        case "3":
                            cbo.Items.Add(dt.Rows[i]["DRName"].ToString().Trim());
                            break;
                    }
                }
            }

            cbo.SelectedIndex = 0;

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// Author : 안정수
        /// Create Date : 2017.12.29 
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <returns></returns>
        public static string Get_IllName(PsmhDb pDbCon, string strPtNo)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  O.IllCode, B.IllNameE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IILLS O, " + ComNum.DB_PMPA + "BAS_ILLS B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND    O.Ptno       = '" + strPtNo + "'";
            SQL += ComNum.VBLF + "      AND    SUBSTR(O.Boowi1,1,1) = '*' ";
            SQL += ComNum.VBLF + "      AND    O.IllCode    =  B.IllCode";
            SQL += ComNum.VBLF + "ORDER BY O.EntDate DESC";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["IllNameE"].ToString().Trim();
            }

            else
            {
                rtnVal = "";
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  O.IllCode, B.IllNameE";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IILLS O, " + ComNum.DB_PMPA + "BAS_ILLS B";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND O.Ptno       = '" + strPtNo + "'";
                SQL += ComNum.VBLF + "      AND O.IllCode    =  B.IllCode";
                SQL += ComNum.VBLF + "ORDER BY O.EntDate DESC";


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["IllNameE"].ToString().Trim();
                }

                else
                {
                    rtnVal = "";
                }
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        public static string READ_GSWARD(string ArgSabun)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.CODE ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_CODE A, KOSMOS_ADM.INSA_MST B ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.GUBUN = '2' ";
                SQL = SQL + ComNum.VBLF + "    AND A.MATCH_CODE = B.BUSE ";
                SQL = SQL + ComNum.VBLF + "    AND B.SABUN = '" + clsType.User.Sabun + "' ";
                //SQL = SQL + ComNum.VBLF + "    AND A.GBUSE = 'Y'";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["Code"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public static bool NURSE_Only_Team_Reader(string ArgSabun)
        {
            //병동현황은 간호부 팀장 제외 타병동 보지 못하도록 막음
            //간호부 당직 PC 제외
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = " SELECT KORNAME ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_MST ";
            SQL += ComNum.VBLF + " WHERE (JIK = '13' OR JIK = '04') ";
            SQL += ComNum.VBLF + "   AND JIKJONG = '41' ";
            SQL += ComNum.VBLF + "   AND SABUN3 = " + ArgSabun;
            SQL += ComNum.VBLF + "   AND BUSE IN ( ";
            SQL += ComNum.VBLF + "                SELECT MATCH_CODE ";
            SQL += ComNum.VBLF + "                  FROM KOSMOS_PMPA.NUR_CODE ";
            SQL += ComNum.VBLF + "                 WHERE GUBUN = '2') ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = true;
            }
            else
            {
                rtnVal = false;
            }

            dt.Dispose();
            dt = null;

            if (rtnVal == false)
            {

                if (NURSE_Manager_Check_IP() == true)
                {
                    rtnVal = true;
                }
            }

            return rtnVal;
        }
        

        /// <summary>
        /// <seealso cref="Nrinfo.bas : NURSE_Manager_Check"/>
        /// </summary>
        /// <param name="ArgSabun"></param>
        /// <returns></returns>
        public static bool NURSE_Manager_Check(long ArgSabun)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  CODE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND GUBUN='NUR_간호부관리자사번'";
            SQL += ComNum.VBLF + "      AND CODE=" + ArgSabun + "";
            SQL += ComNum.VBLF + "      AND DELDATE IS NULL";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                if (VB.Val(dt.Rows[0]["CODE"].ToString().Trim()) > 0)
                {
                    rtnVal = true;
                }

                else
                {
                    rtnVal = false;
                }
            }

            else
            {
                rtnVal = false;
            }

            dt.Dispose();
            dt = null;

            if (rtnVal == false)
            {

                if (NURSE_Manager_Check_IP() == true)
                {
                    rtnVal = true;
                }
            }

            return rtnVal;
        }

        /// <param name="argIP"></param>
        /// <returns></returns>
        public static bool NURSE_Manager_Check_IP()
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  CODE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND GUBUN='NUR_간호부관리자사번IP'";
            SQL += ComNum.VBLF + "      AND CODE='" + clsCompuInfo.gstrCOMIP + "'";
            SQL += ComNum.VBLF + "      AND DELDATE IS NULL";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                if (VB.Val(dt.Rows[0]["CODE"].ToString().Trim()) > 0)
                {
                    rtnVal = true;
                }

                else
                {
                    rtnVal = false;
                }
            }

            else
            {
                rtnVal = false;
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// <seealso cref="Nrinfo.bas : ReadInWard"/>
        /// </summary>
        /// <param name="argWard"></param>
        /// <returns></returns>
        public static string ReadInWard(string argWard)
        {
            //과거 병동 데이터 조회 되도록 프로그램
            //쿼리 사용시 IN으로 조회해야함.

            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";
            int i = 0;
            DataTable dt1 = null;


            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  CODE ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND GUBUN = 'NUR_과거병동조회'";
            SQL += ComNum.VBLF + "      AND NAME = '" + argWard + "'";
            SQL += ComNum.VBLF + "      AND DELDATE IS NULL";

            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt1.Rows.Count > 0)
            {
                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    rtnVal = rtnVal + dt1.Rows[i]["CODE"].ToString().Trim() + "','";
                }

                rtnVal = "'" + rtnVal;
                rtnVal = VB.Mid(rtnVal, 1, (rtnVal.Length) - 2);
            }

            else
            {
                rtnVal = "'" + argWard + "'";
            }

            dt1.Dispose();
            dt1 = null;
            return rtnVal;
        }

        // EMRNO 생성
        public static double GetSequencesNo(PsmhDb pDbCon, string FunSeqName)
        {
            double rtnVal = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT " + FunSeqName + "() FunSeqNo FROM Dual";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = VB.Val(dt.Rows[0]["FunSeqNo"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        public void ComboWard_SET(ComboBox cbo)
        {
            int i = 0;
            //int j = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  WardCode, WardName";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD";
            SQL += ComNum.VBLF + "WHERE 1=1";
            //SQL += ComNum.VBLF + "      AND WARDCODE NOT IN ('IU','NP','2W','NR','DR','IQ','ER')";
            SQL += ComNum.VBLF + "      AND WARDCODE NOT IN ('IU','NP','2W','IQ','ER')";
            SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'  ";
            SQL += ComNum.VBLF + "ORDER BY WardCode";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            cbo.Items.Clear();
            cbo.Items.Add("전체");

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cbo.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }
            }

            cbo.Items.Add("SICU");
            cbo.Items.Add("MICU");

            dt.Dispose();
            dt = null;

            cbo.Text = "";

            foreach (string s in cbo.Items)
            {
                if (s == gsWard)
                {
                    cbo.Text = s;
                    cbo.Enabled = false;
                    break;
                }
            }
        }

        /// <summary>
        /// <seealso cref="NrInfo00.bas : Read_ICU_Bed_Name"/>
        /// Author : 안정수
        /// Create : 2018-02-12
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="argShort"></param>
        /// <returns></returns>
        public static string Read_ICU_Bed_Name(PsmhDb pDbCon, string arg, string argShort = "")
        {
            string rtnVal = "";
            string strTemp = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  NAME, CODE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND GUBUN = 'NUR_ICU_침상번호'";
            SQL += ComNum.VBLF + "      AND CODE = '" + arg + "'";
            SQL += ComNum.VBLF + "      AND DELDATE IS NULL";
            SQL += ComNum.VBLF + "ORDER BY CODE";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                if (argShort == "1")
                {
                    strTemp = dt.Rows[0]["NAME"].ToString().Trim();

                    if (strTemp.IndexOf("격리") > -1)
                    {
                        strTemp = strTemp.Replace("격리", "격");
                    }

                    strTemp = strTemp + ")";
                    rtnVal = strTemp;
                }

                else
                {
                    rtnVal = "-" + dt.Rows[0]["NAME"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        /// <summary>
        /// 의사명을 가지고 온다
        /// <seealso cref="NVWDDIS.bas : DrName_Get"/>        
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strDrCode"></param>
        /// <returns></returns>
        public string DrName_Get(PsmhDb pDbCon, string strDrCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT DrName FROM KOSMOS_OCS.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE Sabun = '" + strDrCode.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["DRNAME"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public string READ_SABUN(PsmhDb pDbCon, string arg)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;


            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  B.NAME, A.KORNAME";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_PMPA + "BAS_BUSE B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND A.BUSE = B.BUCODE";
            SQL += ComNum.VBLF + "      AND A.SABUN = '" + ComFunc.SetAutoZero(arg, 5) + "'";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["NAME"].ToString().Trim() + " / " + dt.Rows[0]["KORNAME"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        public string READ_FIRE(PsmhDb pDbCon, string ArgIPDNO)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  FIRE_EXIT_GUBUN";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_MASTER";
            SQL += ComNum.VBLF + "WHERE IPDNO = " + ArgIPDNO;

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                switch (dt.Rows[0]["FIRE_EXIT_GUBUN"].ToString().Trim())
                {
                    case "1":
                        rtnVal = "Bed";
                        break;

                    case "2":
                        rtnVal = "W/C";
                        break;

                    case "3":
                        rtnVal = "Walking";
                        break;
                }
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Author : 안정수
        /// Create : 2018-02-22
        /// <seealso cref="PatWarning.bas : READ_WARNING_BRADEN"/>
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPTNO"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgIPDNO"></param>
        /// <param name="ArgAge"></param>
        /// <param name="argWARD"></param>
        /// <param name="argDate2"></param>
        /// <returns></returns>
        public string READ_WARNING_BRADEN(PsmhDb pDbCon, string argPTNO, string ArgDate, string ArgIPDNO, string ArgAge, string argWARD, string ArgDate2 = "")
        {
            string rtnVal = "";
            //int iFB = 0;

            //string strFall = "";
            string strBraden = "";
            //string strOK = "";
            //string strAge = "";
            //string strWard = "";
            string strGubun = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (argPTNO == "09315922")
            {
                return rtnVal;
            }

            if (ArgIPDNO == "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  IPDNO, WARDCODE";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND PANO ='" + argPTNO + "'";
                SQL += ComNum.VBLF + "      AND JDATE =TO_DATE('1900-01-01','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND ACTDATE IS NULL";
                SQL += ComNum.VBLF + "ORDER BY INDATE DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                    argWARD = dt.Rows[0]["WARDCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }

            if (!VB.IsNumeric(ArgAge))
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  AGE";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND PANO ='" + argPTNO + "'";
                SQL += ComNum.VBLF + "      AND JDATE =TO_DATE('1900-01-01','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND ACTDATE IS NULL";
                SQL += ComNum.VBLF + "ORDER BY INDATE DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgAge = dt.Rows[0]["AGE"].ToString().Trim();
                }

                else
                {
                    ArgAge = "";
                }

                dt.Dispose();
                dt = null;
            }

            if (ArgAge == "")
            {
                return rtnVal;
            }

            if (argWARD == "NR" || argWARD == "NO" || argWARD == "IQ")
            {
                strGubun = "신생아";
            }

            else if (String.Compare(ArgAge, "5") < 0)
            {
                strGubun = "소아";
            }

            else
            {
                strGubun = "";
            }

            if (strGubun == "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  A.PANO, A.TOTAL, A.AGE";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE A";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND A.IPDNO = " + ArgIPDNO;
                SQL += ComNum.VBLF + "      AND A.PANO = '" + argPTNO + "'";

                if (ArgDate2 != "")
                {
                    SQL += ComNum.VBLF + "  AND A.ACTDATE >= TO_DATE('" + ArgDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  AND A.ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                }

                else
                {
                    SQL += ComNum.VBLF + "  AND A.ACTDATE = TO_DATE('" + ArgDate + "','YYYY-MM-DD')";
                }

                SQL += ComNum.VBLF + "      AND A.TOTAL <= 18";
                SQL += ComNum.VBLF + "      AND A.ROWID = (";
                SQL += ComNum.VBLF + "                      SELECT ROWID FROM (";
                SQL += ComNum.VBLF + "                                          SELECT * FROM KOSMOS_PMPA.NUR_BRADEN_SCALE";
                SQL += ComNum.VBLF + "                                          WHERE ACTDATE = TO_DATE('" + ArgDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "                                            AND IPDNO = " + ArgIPDNO;
                SQL += ComNum.VBLF + "                                            AND PANO = '" + argPTNO + "'";
                SQL += ComNum.VBLF + "                                          ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                SQL += ComNum.VBLF + "                      Where ROWNUM = 1)";
                SQL += ComNum.VBLF + "ORDER BY A.ActDate DESC, DECODE(A.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), A.ENTDATE) DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if ((VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) >= 60 && VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 18)
                        || (VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) < 60 && VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 18))
                    {
                        strBraden = "OK";
                    }
                }

                dt.Dispose();
                dt = null;
            }

            else if (strGubun == "소아")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  TOTAL";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE_CHILD";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND IPDNO=" + ArgIPDNO;
                SQL += ComNum.VBLF + "      AND PANO = '" + argPTNO + "'";

                if (ArgDate2 != "")
                {
                    SQL += ComNum.VBLF + "  AND ACTDATE >= TO_DATE('" + ArgDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  AND ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                }

                else
                {
                    SQL += ComNum.VBLF + "  AND ACTDATE = TO_DATE('" + ArgDate + "','YYYY-MM-DD')";
                }

                SQL += ComNum.VBLF + "ORDER BY ACTDATE DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (VB.Val(dt.Rows[0]["Total"].ToString().Trim()) <= 16)
                    {
                        //strOK = "OK";
                    }
                }

                dt.Dispose();
                dt = null;
            }

            else if (strGubun == "신생아")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  TOTAL";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE_BABY";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND IPDNO=" + ArgIPDNO;
                SQL += ComNum.VBLF + "      AND PANO = '" + argPTNO + "'";

                if (ArgDate2 != "")
                {
                    SQL += ComNum.VBLF + "  AND ACTDATE >= TO_DATE('" + ArgDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  AND ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                }

                else
                {
                    SQL += ComNum.VBLF + "  AND ACTDATE = TO_DATE('" + ArgDate + "','YYYY-MM-DD')";
                }

                SQL += ComNum.VBLF + "ORDER BY ACTDATE DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    if (VB.Val(dt.Rows[0]["Total"].ToString().Trim()) <= 20)
                    {
                        strBraden = "OK";
                    }
                }

                dt.Dispose();
                dt = null;
            }

            if (strBraden == "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT * ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_BRADEN_WARNING";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND IPDNO = " + ArgIPDNO;
                SQL += ComNum.VBLF + "      AND PANO = '" + argPTNO + "'";
                SQL += ComNum.VBLF + "      AND (";
                SQL += ComNum.VBLF + "               WARD_ICU = '1' ";
                SQL += ComNum.VBLF + "              OR GRADE_HIGH = '1' ";
                SQL += ComNum.VBLF + "              OR PARAL = '1'";
                SQL += ComNum.VBLF + "              OR COMA = '1'";
                SQL += ComNum.VBLF + "              OR NOT_MOVE = '1'";
                SQL += ComNum.VBLF + "              OR DIET_FAIL = '1'";
                SQL += ComNum.VBLF + "              OR NEED_PROTEIN = '1'";
                SQL += ComNum.VBLF + "              OR EDEMA = '1'";
                SQL += ComNum.VBLF + "              OR (BRADEN = '1' AND (BRADEN_OK = '0' OR BRADEN_OK = NULL))";
                SQL += ComNum.VBLF + "          )";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strBraden = "OK";
                }

                dt.Dispose();
                dt = null;

            }

            if (strBraden == "OK")
            {
                rtnVal = "욕창위험";
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 해당 월(MM) 계산 ex)201708 -> 201707 or 201709
        /// Author : 안정수
        /// Create Date : 2017.09.07
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argADD"></param>
        /// <seealso cref="VBFunction.bas : DATE_YYMM_ADD"/>
        public string DATE_YYMM_ADD(string ArgYYMM, int argADD)
        {
            string rtnVal = "";

            int ArgI = 0;
            int ArgJ = 0;
            int ArgYY = 0;
            int ArgMM = 0;

            if (ArgYYMM.Length != 6 || argADD == 0)
            {
                return ArgYYMM;
            }

            ArgYY = Convert.ToInt32(VB.Left(ArgYYMM, 4));
            ArgMM = Convert.ToInt32(VB.Right(ArgYYMM, 2));

            ArgJ = argADD;

            if (ArgJ < 0)
            {
                ArgJ = ArgJ * -1;
            }

            for (ArgI = 1; ArgI <= ArgJ; ArgI++)
            {
                if (argADD < 0)
                {
                    ArgMM -= 1;
                    if (ArgMM == 0)
                    {
                        ArgMM = 12;
                        ArgYY -= 1;
                    }
                }
                else
                {
                    ArgMM += 1;
                    if (ArgMM == 13)
                    {
                        ArgYY += 1;
                        ArgMM = 1;
                    }
                }

            }

            rtnVal = ComFunc.SetAutoZero(ArgYY.ToString(), 4) + ComFunc.SetAutoZero(ArgMM.ToString(), 2);
            return rtnVal;
        }


        public string READ_DRNAME_SET(string ArgDept)
        {
            DataTable dt = null;
            string SqlErr = "";
            //int i = 0;
            string rtnVal = "";
            string SQL = "";

            if (ArgDept == "")
            {
                return rtnVal;
            }
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  DRCODE, DRNAME";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND DRCODE = '" + ArgDept + "'";
                SQL += ComNum.VBLF + "      AND TOUR <>'Y'";
                SQL += ComNum.VBLF + "      AND SUBSTR(DRCODE, 3,2) <> '99'";
                SQL += ComNum.VBLF + "ORDER BY PRINTRANKING";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DRNAME"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장

                return rtnVal;
            }
        }

        public static string Read_EWard_Bed_Name(PsmhDb pDbCon, string strArg, string strShort = "", string strWard = "")
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strTemp = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT NAME, CODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1 ";
                if (strWard == "10")
                {
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN = 'NUR_10WARD_침상번호' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN = 'NUR_EWARD_침상번호' ";
                }
                SQL = SQL + ComNum.VBLF + "   AND CODE = '" + strArg + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (strShort == "1")
                    {
                        strTemp = dt.Rows[0]["NAME"].ToString().Trim();

                        if (strTemp.IndexOf("격리") > -1)
                        {
                            strTemp.Replace("격리", "격");
                        }
                        strTemp = strTemp + ")";

                        RtnVal = strTemp;
                    }
                    else
                    {
                        RtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;
        }

        public static string Read_10Ward_Bed_Name(PsmhDb pDbCon, string strArg, string strShort = "")
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strTemp = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT NAME, CODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_10WARD_침상번호' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '" + strArg + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (strShort == "1")
                    {
                        strTemp = dt.Rows[0]["NAME"].ToString().Trim();

                        if (strTemp.IndexOf("격리") > -1)
                        {
                            strTemp.Replace("격리", "격");
                        }
                        strTemp = strTemp + ")";

                        RtnVal = strTemp;
                    }
                    else
                    {
                        RtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;
        }

        /// <summary>
        /// '기록실 문제환자관리
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPano"></param>
        /// <param name="strGbn"></param>
        /// <returns></returns>
        public static string Read_Black_NurMemo_Check(PsmhDb pDbCon, string strPano, string strGbn)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT PANO,SNAME,MEMO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_OCSMEMO_MID ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO  = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DDATE IS NULL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = dt.Rows[0]["MEMO"].ToString().Trim().Replace("`", "'");
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;
        }


        /// <summary>
        /// '중환자실 젖산 수치 관리(평가대상 결과값이 3이상나왔을시 나타나게 )
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPano"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Read_Black_Lactate_Check(PsmhDb pDbCon, string strPano, string strWARD)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수


            if (strWARD == "33" || strWARD == "35")
            {
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    SQL = "";
                    SQL = " SELECT /*+RULE*/ A.SUBCODE, A.RESULT, A.UNIT, TO_CHAR(B.SENDDATE,'YY.MM.DD') OD, TO_CHAR(B.RESULTDATE,'YY.MM.DD') DD,EXAMYNAME";
                    SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.EXAM_RESULTC A, KOSMOS_OCS.EXAM_SPECMST B,KOSMOS_OCS.EXAM_MASTER D, ";
                    SQL = SQL + ComNum.VBLF + "   (SELECT /*+LEADING(A)*/ MAX(ORDERDATE) ORDERDATE, SUBCODE";
                    SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.EXAM_RESULTC SUBA, KOSMOS_OCS.EXAM_SPECMST SUBB";
                    SQL = SQL + ComNum.VBLF + " WHERE SUBA.SPECNO = SUBB.SPECNO";
                    SQL = SQL + ComNum.VBLF + "   AND SUBB.PANO = SUBA.PANO ";
                    SQL = SQL + ComNum.VBLF + "   AND SUBB.PANO  = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND SUBB.RESULTDATE >= SYSDATE - 1 ";
                    SQL = SQL + ComNum.VBLF + "   AND SUBB.RESULTDATE <= SYSDATE  ";
                    SQL = SQL + ComNum.VBLF + "   AND SUBA.SUBCODE IN ('CR63F','CR63G')";
                    SQL = SQL + ComNum.VBLF + "       GROUP BY SUBCODE) C";
                    SQL = SQL + ComNum.VBLF + " WHERE A.SPECNO = B.SPECNO";
                    SQL = SQL + ComNum.VBLF + "   AND B.PANO  = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND B.PANO = A.PANO ";
                    SQL = SQL + ComNum.VBLF + "   AND A.RESULT >= '2' ";
                    SQL = SQL + ComNum.VBLF + "   AND B.RESULTDATE >= SYSDATE - 1 ";
                    SQL = SQL + ComNum.VBLF + "   AND B.RESULTDATE <= SYSDATE  ";
                    SQL = SQL + ComNum.VBLF + "   AND B.ORDERDATE = C.ORDERDATE";
                    SQL = SQL + ComNum.VBLF + "   AND A.SUBCODE = C.SUBCODE AND A.SUBCODE = D.MASTERCODE(+)";
                    SQL = SQL + ComNum.VBLF + "   AND A.STATUS NOT IN ('N') ";
                    SQL = SQL + ComNum.VBLF + "      ORDER BY EXAMYNAME";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return RtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        RtnVal = dt.Rows[0]["RESULT"].ToString().Trim().Replace("`", "'");
                    }

                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                    }
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox(ex.Message);
                }
            }
            return RtnVal;
        }


        /// <summary>
        /// READ_환자신주소
        /// </summary>
        /// <param name="argPTNO"></param>
        /// <returns></returns>
        public static string READ_VIEW_PATIENT_JUSO(string argPTNO)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT JUSO || ' ' || JUSO2 AS JUSO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.VIEW_PATIENT_JUSO ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = dt.Rows[0]["JUSO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return RtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return RtnVal;
            }
        }


        public static string Pat_TewonName_Chk(int ArgIPDNO)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " SELECT PANO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_TEWON_NAMESEND  ";
                SQL = SQL + ComNum.VBLF + "  WHERE IPDNO = " + Convert.ToString(ArgIPDNO) + " ";
                SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE ='') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = "OK";
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return RtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return RtnVal;
            }
        }

        public static string Xray_Remark_Conv(PsmhDb pDbCon, string ArgTime, string ArgXCode, string argROOMCODE = "")
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            //MRI실이 아닐경우
            if (argROOMCODE != "L" && argROOMCODE != "M")
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "  XCODE,XNAME,REMARK1,REMARK2,REMARK3,GBDATE , DECODE(GBDATE,'1',REMARK1,'2',REMARK2,'3',REMARK3,'') REMARK";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "XRAY_CODE";
                SQL = SQL + ComNum.VBLF + "WHERE XCODE ='" + ArgXCode + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["REMARK"].ToString().Trim() != "")
                    {
                        rtnVal = ArgTime + "분 " + dt.Rows[0]["REMARK"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }

            //MRI실일 경우
            else
            {
                switch (argROOMCODE)
                {
                    case "M":
                        rtnVal = ArgTime + "분" + "에 MRI 1촬영실로 가세요";
                        break;

                    case "L":
                        rtnVal = ArgTime + "분" + "에 MRI 2촬영실로 가세요";
                        break;
                }
            }

            if (rtnVal == "")
            {
                if (String.Compare(ArgXCode.Trim(), "G27") >= 0 && String.Compare(ArgXCode.Trim(), "G2799") <= 0)
                {
                    rtnVal = "1층 컴퓨터종합건진센타로 가세요";
                }

                else if (String.Compare(ArgXCode.Trim(), "E70") >= 0 && String.Compare(ArgXCode.Trim(), "E7099") <= 0)
                {

                }

                else
                {
                    switch (ArgXCode.Trim())
                    {
                        case "G2702":
                        case "G2702B":
                            rtnVal = "1층 컴퓨터종합건진센타로 가세요";
                            break;

                        case "D4101":
                            rtnVal = "식사시 해초류(김,미역,다시마등)은 드시지" + "\r\n";
                            rtnVal += "말고" + ArgTime + "분에" + "\r\n";
                            rtnVal += "1층의 동위원소실로 가세요";
                            break;

                        case "CT66":
                        case "CT67":
                            rtnVal = "검사 6시간 전부터 금식,금물하시고 " + "\r\n";
                            rtnVal += ArgTime + "분에 영상의학과로 오십시요";
                            break;

                        default:
                            rtnVal = ArgTime + "분에 영상의학과로 오십시요";
                            break;
                    }
                }
            }
            return rtnVal;
        }

        public static string READ_ORDERSITE(string ArgOrderSite)
        {
            if (ArgOrderSite == "OPD")
            {
                return "<외래> ";
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 분만수가 여부 확인
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSuCode"></param>        
        /// <seealso cref="nrinfo02.bas : READ_분만수가"/>
        /// <returns></returns>
        public static bool READ_CHILDBIRTH_SUGA(PsmhDb pDbCon, string ArgSuCode)
        {
            bool rtnVal = false;
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string strCODE = string.Empty;

            if (string.Compare(clsPublic.GstrSysDate, "2016-11-01") < 0)
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL += " SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL += "  WHERE GUBUN = 'OCS_분만수가코드' ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        strCODE = strCODE + "'" + Dt.Rows[i]["CODE"].ToString().Trim() + "',";
                    }

                    if (strCODE.Trim() != "" && VB.Right(strCODE, 1) == ",")
                    {
                        strCODE = VB.Mid(strCODE, 1, VB.Len(strCODE) - 1);
                    }
                }

                Dt.Dispose();
                Dt = null;

                if (strCODE.Trim() != "")
                {
                    SQL = "";
                    SQL += " SELECT ORDERCODE FROM " + ComNum.DB_MED + "OCS_ORDERCODE   ";
                    SQL += "  WHERE SUCODE IN (" + strCODE + ")                         ";
                    SQL += "    AND ORDERCODE = '" + ArgSuCode + "'                     ";
                    SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (Dt.Rows.Count > 0)
                        rtnVal = true;

                    Dt.Dispose();
                    Dt = null;

                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        public void setYYYYMM(System.Windows.Forms.ComboBox argCbo, string argDate, int YYMMCNT = 20, int index = 0, int nAdd = 0, string argSTS = "+")
        {
            int i = 0;
            int nYY, nMM;

            nYY = Convert.ToInt16(VB.Left(argDate, 4));
            nMM = Convert.ToInt16(VB.Mid(argDate, 6, 2));


            for (i = 0; i < nAdd; i++)
            {
                if (argSTS == "+")
                {
                    nMM++;
                    if (nMM >= 13)
                    {
                        nYY++;
                        nMM = 1;
                    }
                }
                else
                {
                    nMM--;
                    if (nMM == 0)
                    {
                        nYY--;
                        nMM = 12;
                    }
                }
            }


            argCbo.Items.Clear();

            for (i = 1; i <= YYMMCNT; i++)
            {
                argCbo.Items.Add(ComFunc.SetAutoZero(nYY.ToString(), 4) + "년 " + ComFunc.SetAutoZero(nMM.ToString(), 2) + "월");
                nMM++;
                if (nMM >= 13)
                {
                    nYY++;
                    nMM = 1;
                }
            }

            argCbo.SelectedIndex = index;
        }

        public string[] read_huil(PsmhDb pDbCon, string argSDate, string argTDate)
        {
            DataTable dt = null;
            string[] tDay = null;

            //배열 초기화
            tDay = new string[Convert.ToInt16(VB.Right(argTDate, 2))];

            dt = sel_BasJob(pDbCon, argSDate, argTDate);

            if (dt == null) return null;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tDay[i] = "";

                    //토 , 일요일 
                    tDay[i] = clsVbfunc.GetYoIl(dt.Rows[i]["JobDate"].ToString().Trim());

                    //휴일
                    if (dt.Rows[i]["HOLYDAY"].ToString().Trim() == "*")
                    {
                        tDay[i] = "*";
                    }

                }
            }

            return tDay;

        }

        public DataTable sel_BasJob(PsmhDb pDbCon, string argSDate, string argTDate)
        {
            DataTable dt = null;

            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += " SELECT  TO_CHAR(JobDATE,'YYYY-MM-DD') JobDate, HOLYDAY , TEMPHOLYDAY                           \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_JOB                                                           \r\n";
            SQL += "  WHERE 1 = 1                                                                                   \r\n";
            SQL += "    AND JobDATE >= TO_DATE('" + argSDate + "','YYYY-MM-DD')                                     \r\n";
            SQL += "    AND JobDATE < TO_DATE('" + argTDate + "','YYYY-MM-DD')                                      \r\n";
            SQL += "   ORDER BY JobDATE                                                                             \r\n";

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