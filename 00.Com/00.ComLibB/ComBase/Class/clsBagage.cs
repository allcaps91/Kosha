using System;
using System.Windows.Forms;
using System.Data;
using ComDbB; //DB연결
using System.Drawing;

namespace ComBase
{
    /// <summary>
    /// TODO : bagage.bas
    /// </summary>
    public class clsBagage
    {
        /// <summary>
        /// TODO : bagage.bas : READ_조영제수가
        /// </summary>
        /// <param name="pDbCon">커넥션 객체</param>
        /// <param name="strArg"></param>
        /// <returns></returns>
        public static string READ_CONTRAST_MEDIUM_SUGA(PsmhDb pDbCon, string strArg)
        {
            string SQL = "";    //Query문
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;

            string rtnVal = "";

            //arg 값이 있을 경우 조영제 여부 체크
            //arg 값이 없을 경우 조영제 목록 전달

            try
            {
                SQL = "";
                SQL = "SELECT A.SUNEXT";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.BAS_SUN A, ADMIN.EDI_SUGA B, ADMIN.OCS_DRUGINFO_NEW C";
                SQL = SQL + ComNum.VBLF + "    WHERE A.BCODE = B.CODE";
                SQL = SQL + ComNum.VBLF + "       AND B.EFFECT = '721'";
                SQL = SQL + ComNum.VBLF + "       AND A.SUNEXT = C.SUNEXT";

                if (strArg != "")
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.SUNEXT = '" + strArg + "' ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    if (strArg == "")
                    {
                        rtnVal = "OK";
                    }
                    else
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            rtnVal = rtnVal + dt.Rows[i]["SUNEXT"].ToString().Trim() + "', '";
                        }

                        rtnVal = "'" + rtnVal;

                        rtnVal = VB.Mid(rtnVal, 1, VB.Len(rtnVal) - 2);
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : bagage.bas : PrtAgreeBefore
        /// </summary>
        /// <param name="strArg"></param>
        /// <returns></returns>
        public static string PrtAgreeBefore(PsmhDb pDbCon, string strArg)
        {
            string SQL = "";    //Query문
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "NO";

            //조영제사용하는 오더 또는 수혈오더가 발생할 경우 내원시 최초 1회 자동 인쇄함
            //내원 중 인쇄한 내역 있는지 확인 함.

            string[] strDate = null;

            strDate = VB.Split(strArg, "|");

            try
            {
                SQL = "";
                SQL = "SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMR_AGREE_PRINT ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strDate[0].Trim() + "'";
                SQL = SQL + ComNum.VBLF + "    AND MEDFRDATE = '" + strDate[3].Replace("-", "").Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "    AND FORMNO = '" + strDate[11] + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "YES";
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : bagage.bas : PrtAgree
        /// </summary>
        /// <param name="strArg"></param>
        public static void PrtAgree(PsmhDb pDbCon, string strArg)
        {
            string SQL = "";    //Query문
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = ""; //에러문 받는 변수

            string[] strData = null;
            string strOK = "";

            clsOrdFunction.GstrAgreePrt = "";

            strData = VB.Split(strArg, "|");

            try
            {
                SQL = "";
                SQL = "SELECT PANO ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_HDMST";
                SQL = SQL + ComNum.VBLF + " WHERE PANO= '" + strData[0] + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = "SELECT A.PATID";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMR_TREATT A, ADMIN.EMR_CHARTPAGET B, ADMIN.EMR_PAGET C";
                    SQL = SQL + ComNum.VBLF + " WHERE a.TREATNO = b.TREATNO";
                    SQL = SQL + ComNum.VBLF + "    AND B.PAGENO = C.PAGENO";
                    SQL = SQL + ComNum.VBLF + "    AND B.FORMCODE IN ('146','147')";
                    SQL = SQL + ComNum.VBLF + "    AND A.INDATE >= '" + strData[3] + "'";
                    SQL = SQL + ComNum.VBLF + "    AND A.PATID = '" + strData[0] + "'";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        strOK = "NO";
                    }

                    dt1.Dispose();
                    dt1 = null;
                }

                dt.Dispose();
                dt = null;

                if (strOK == "NO")
                {
                    return;
                }

                clsPublic.GstrHelpCode = strArg;

                //TODO : EMR 동의서 출력
                clsOrdFunction.GstrAgreePrt = "OK";
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// TODO : bagage.bas : readAllergyInfo
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <returns></returns>
        public static string readAllergyInfo(PsmhDb pDbCon, string strPtNo)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;

            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT CODE, REMARK, ENTDATE, NVL(SABUN,'4349') ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.ETC_ALLERGY_MST";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODE";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

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
                        if (dt.Rows[i]["CODE"].ToString().Trim() == "004")
                        {
                            rtnVal = rtnVal + ComNum.VBLF + "※" + dt.Rows[i]["REMARK"].ToString().Trim() + " => 등록 (" + dt.Rows[i]["ENTDATE"].ToString().Trim() + "/약물이상반응모니터링";
                        }
                        else
                        {
                            rtnVal = rtnVal + ComNum.VBLF + "※" + dt.Rows[i]["REMARK"].ToString().Trim() + " => 등록 (" + dt.Rows[i]["ENTDATE"].ToString().Trim();
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : bagage.bas : ZipCodeToJuso
        /// </summary>
        /// <param name="strArg1"></param>
        /// <param name="strArg2"></param>
        /// <returns></returns>
        public static string ZipCodeToJuso(PsmhDb pDbCon, string strArg1, string strArg2)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT ZIPNAME1 || ' ' || ZIPNAME2 || ' ' || ZIPNAME3 || ' ' || RI || ' ' || DLGB || ' ' || BUNJI juso";
                SQL = SQL + ComNum.VBLF + "  From ADMIN.BAS_ZIPSNEW";
                SQL = SQL + ComNum.VBLF + "  WHERE ZIPCODE1 = '" + strArg1 + "' ";
                SQL = SQL + ComNum.VBLF + "     AND ZIPCODE2 = '" + strArg2 + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["JUSO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : bagage.bas : GET_Kcal
        /// </summary>
        /// <param name="strH"></param>
        /// <param name="strW"></param>
        /// <param name="strSex"></param>
        /// <returns></returns>
        public static string GET_Kcal(string strH, string strW, string strSex)
        {
            string rtnVal = "";

            //권장 열량 구하기(세자리에서 반올림 함)
            //권장 열량 = 적정체중 * 30
            //적정체중 = (남)키 * 키 * 22
            //적정체중 = (여)키 * 키 * 21

            if (VB.IsNumeric(strH) && VB.IsNumeric(strW))
            {
            }
            else
            {
                rtnVal = "Error";
                return rtnVal;
            }

            if (strH == "")
            {
                return rtnVal;
            }

            if (strSex == "M")
            {
                rtnVal = (Math.Round((((VB.Val(strH) * 0.01) * (VB.Val(strH) * 0.01) * 22) * 30) * 0.01) * 100).ToString();
            }
            else if (strSex == "F")
            {
                rtnVal = (Math.Round((((VB.Val(strH) * 0.01) * (VB.Val(strH) * 0.01) * 21) * 30) * 0.01) * 100).ToString();
            }
            else
            {
                ComFunc.MsgBox("성별이 없습니다.", "확인");
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : bagage.bas : ReadICUSelect
        /// </summary>
        /// <param name="strWARD"></param>
        /// <returns></returns>
        public static bool ReadICUSelect(string strWARD)
        {
            bool rtnVal = false;

            switch (strWARD)
            {
                case "ICU":
                case "SICU":
                case "MICU":
                case "CCU":
                case "32":
                case "33":
                case "35":
                    rtnVal = true;
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : bagage.bas : LoadCalc
        /// </summary>
        /// <param name="strCode"></param>
        /// <param name="strPtNo"></param>
        public static void LoadCalc(PsmhDb pDbCon, string strCode, string strPtNo)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            clsPublic.GstrHelpCode = "";

            try
            {
                SQL = "";
                SQL = "SELECT ROWID FROM ADMIN.OCS_ORDERCODE";
                SQL = SQL + ComNum.VBLF + " WHERE BUN IN ('72','73')";
                SQL = SQL + ComNum.VBLF + " AND ORDERCODE = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    clsPublic.GstrHelpCode = strPtNo;

                    //TODO : DRUG/DRINFO - 포함된 vbp 가 없음
                    //FrmMediCalc.Show
                }

                dt.Dispose();
                dt = null;

                clsPublic.GstrHelpCode = "";
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// TODO : bagage.bas : ForeWard
        /// </summary>
        /// <param name="strArg"></param>
        /// <returns></returns>
        public static string ForeWard(string strArg)
        {
            string rtnVal = "";

            switch (strArg)
            {
                case "41":
                    rtnVal = "8W";
                    break;
                case "51":
                    rtnVal = "5W";
                    break;
                case "52":
                    rtnVal = "4W";
                    break;
                case "53":
                    rtnVal = "6A";
                    break;
                case "62":
                    rtnVal = "6W";
                    break;
                case "63":
                    rtnVal = "4A";
                    break;
                case "72":
                    rtnVal = "3W";
                    break;
                case "73":
                    rtnVal = "7W";
                    break;
                default:
                    rtnVal = strArg;
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// TODO : bagage.bas : ReadDrugProgress
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static string ReadDrugProgress(PsmhDb pDbCon, string strPtNo, string strDate)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            string strTewon = "";
            string strAirsht = "";
            string strTemp = "";

            try
            {
                SQL = "";
                SQL = "SELECT ROWNUM SEQ, OUTSEQ, ENTTIME, GBATC, WRITESABUN, GBTFLAG, AIRSHT";
                SQL = SQL + ComNum.VBLF + "    FROM (";
                SQL = SQL + ComNum.VBLF + " SELECT MAX(A.OUTSEQ) OUTSEQ, A.ENTTIME, A.GBATC, B.WRITESABUN,  A.GBTFLAG, '' AIRSHT";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.OCS_PHARMACY A, ADMIN.DRUG_PROGRESS B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.ENTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "       AND PTNO = '" + strPtNo + "'";
                SQL = SQL + ComNum.VBLF + "       AND A.GBTFLAG = 'T'";
                SQL = SQL + ComNum.VBLF + "       AND A.ENTDATE = B.ENTDATE(+)";
                SQL = SQL + ComNum.VBLF + "       AND A.OUTSEQ = B.OUTSEQ(+)";
                SQL = SQL + ComNum.VBLF + "  GROUP BY  A.ENTTIME, A.GBATC, B.WRITESABUN, A.GBTFLAG, ''";
                SQL = SQL + ComNum.VBLF + "  Union All";
                SQL = SQL + ComNum.VBLF + "   SELECT MAX(A.OUTSEQ) OUTSEQ, A.ENTTIME, A.GBATC, B.WRITESABUN,  '' GBTFLAG, A.AIRSHT";
                SQL = SQL + ComNum.VBLF + "     FROM ADMIN.OCS_PHARMACY A, ADMIN.DRUG_PROGRESS B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.ENTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "       AND PTNO = '" + strPtNo + "'";
                SQL = SQL + ComNum.VBLF + "       AND A.AIRSHT = '1'";
                SQL = SQL + ComNum.VBLF + "       AND A.ENTDATE = B.ENTDATE(+)";
                SQL = SQL + ComNum.VBLF + "       AND A.OUTSEQ = B.OUTSEQ(+)";
                SQL = SQL + ComNum.VBLF + "       AND (B.WRITEDATE IS NULL OR B.WRITEDATE >= SYSDATE + INTERVAL '-1' hour)";
                SQL = SQL + ComNum.VBLF + "  GROUP BY  A.ENTTIME, A.GBATC, B.WRITESABUN, '', A.AIRSHT )";
                SQL = SQL + ComNum.VBLF + " ORDER BY OUTSEQ DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["GBTFLAG"].ToString().Trim() == "T")
                    {
                        if (dt.Rows[0]["SEQ"].ToString().Trim() != "1")
                        {
                            strTewon = "추가퇴원약)";
                        }
                        else
                        {
                            strTewon = "퇴원)";
                        }
                    }

                    if (dt.Rows[0]["AIRSHT"].ToString().Trim() == "1")
                    {
                        strAirsht = "기송)";
                    }

                    if (dt.Rows[0]["WRITESABUN"].ToString().Trim() != "")
                    {
                        strTemp = "완료";
                    }
                    else if (dt.Rows[0]["GBATC"].ToString().Trim() == "Y")
                    {
                        strTemp = "조제중";
                    }
                    else
                    {
                        strTemp = "접수";
                    }

                    rtnVal = strTewon + strAirsht + strTemp + ComNum.VBLF;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : bagage.bas : Not_AirSht
        /// </summary>
        /// <param name="strArg"></param>
        /// <param name="strGBN"></param>
        /// <returns></returns>
        public static bool Not_AirSht(PsmhDb pDbCon, string[] strArg, string strGBN = "")
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;

            bool rtnVal = false;

            string strTemp = "";
            string strQuery = "";

            for (i = 0; i < strArg.Length; i++)
            {
                strQuery = strQuery + strArg[i] + "', '";
            }

            strQuery = "'" + VB.Mid(strQuery, 1, strQuery.Length - 3);

            try
            {
                SQL = "";
                SQL = "SELECT A.JEPCODE, B.SUNAMEK";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.VIEW_NOT_AIRSHT A, ADMIN.BAS_SUN B";
                SQL = SQL + ComNum.VBLF + " WHERE JEPCODE IN (" + strQuery + ")";
                SQL = SQL + ComNum.VBLF + " AND A.JEPCODE = B.SUNEXT";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

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
                        strTemp = strTemp + "  ⊙ 약코드 : " + VB.Left(dt.Rows[i]["JEPCODE"].ToString().Trim() + VB.Space(10), 13) + dt.Rows[i]["SUNAMEK"].ToString().Trim() + ComNum.VBLF;
                    }

                    if (strGBN == "IORDER")
                    {
                        strTemp = "★ 전송하시려는 긴급약처방 중 아래 약제는 기송관 전송이 불가능한 약제입니다. 약제과로 방문하여 수령하시기 바랍니다." + ComNum.VBLF + strTemp;
                    }
                    else
                    {
                        strTemp = "★ 픽업하시려는 긴급약처방 중 아래 약제는 기송관 전송이 불가능한 약제입니다. 약제과로 방문하여 수령하시기 바랍니다." + ComNum.VBLF + strTemp;
                    }

                    ComFunc.MsgBox(strTemp, "확인");
                }

                dt.Dispose();
                dt = null;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : bagage.bas : RES_SMS_NOTSEND_PROCESS
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strBDate"></param>
        /// <param name="strDeptCode"></param>
        /// <param name="strRDate"></param>
        /// <param name="strRTime"></param>
        /// <param name="strDrCode"></param>
        /// <param name="strGubun"></param>
        /// <param name="strGubun2"></param>
        /// <param name="strOk"></param>
        /// <returns></returns>
        public static string RES_SMS_NOTSEND_PROCESS(PsmhDb pDbCon, string strPtNo, string strBDate, string strDeptCode, string strRDate, string strRTime,
                                                    string strDrCode, string strGubun, string strGubun2, string strOk)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string rtnVal = strOk;

            Cursor.Current = Cursors.WaitCursor;

            //argGUBUN  : D/B저장 구분
            //argGUBUN2 : 문자 전송 안함 구분 (값이 'Y' 이면 문자 전송 안함)

            if (strOk == "NO")
            {
                rtnVal = "NO";
                return rtnVal;
            }

            //신규 입력이면서 문자보내지 않음이 아니면 빠져나감
            if (strGubun == "INSERT" && strGubun2 != "Y")
            {
                return rtnVal;
            }

            //예약 변경이면서 문자보내지 않음이 아니면 기존 저장된 내역 삭제
            if (strGubun == "UPDATE" && strGubun2 != "Y")
            {
                strGubun = "DELETE";
            }

            //clsDB.setBeginTran(pDbCon);

            try
            {
                switch (strGubun)
                {
                    case "INSERT":
                        SQL = "";
                        SQL = "INSERT INTO ADMIN.ETC_SMS_RESNOTSEND";
                        SQL = SQL + ComNum.VBLF + "     (PTNO, BDATE, DEPTCODE, DRCODE, RDATE, WRITEDATE, WRITESABUN)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         '" + strPtNo + "', ";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strBDate + "','YYYY-MM-DD'), ";
                        SQL = SQL + ComNum.VBLF + "         '" + strDeptCode + "',";
                        SQL = SQL + ComNum.VBLF + "         '" + strDrCode + "', ";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strRDate + " " + strRTime + "','YYYY-MM-DD HH24:MI'),";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         " + clsPublic.GnJobSabun;
                        SQL = SQL + ComNum.VBLF + "     ) ";
                        break;
                    case "UPDATE":
                        SQL = "";
                        SQL = "UPDATE ADMIN.ETC_SMS_RESNOTSEND";
                        SQL = SQL + ComNum.VBLF + "     SET ";
                        SQL = SQL + ComNum.VBLF + "         DRCODE = '" + strDrCode + "', ";
                        SQL = SQL + ComNum.VBLF + "         RDATE = TO_DATE('" + strRDate + " " + strRTime + "','YYYY-MM-DD HH24:MI'), ";
                        SQL = SQL + ComNum.VBLF + "         WRITEDATE = SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         WRITESABUN = " + clsPublic.GnJobSabun;
                        SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "' ";
                        break;
                    case "DELETE":
                        SQL = "";
                        SQL = "DELETE ADMIN.ETC_SMS_RESNOTSEND ";
                        SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "' ";
                        break;
                }

                if (strGubun == "UPDATE")
                {
                    SQL = "";
                    SQL = "INSERT INTO ADMIN.ETC_SMS_RESNOTSEND";
                    SQL = SQL + ComNum.VBLF + "     (PTNO, BDATE, DEPTCODE, DRCODE, RDATE, WRITEDATE, WRITESABUN)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         '" + strPtNo + "', ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strBDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + strDeptCode + "',";
                    SQL = SQL + ComNum.VBLF + "         '" + strDrCode + "', ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strRDate + " " + strRTime + "','YYYY-MM-DD HH24:MI'), ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         " + clsPublic.GnJobSabun;
                    SQL = SQL + ComNum.VBLF + "     ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                //clsDB.setCommitTran(pDbCon);

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                rtnVal = "NO";
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : bagage.bas : CHECK_RES_SMSNOTSEND1
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strRDate"></param>
        /// <param name="strDrCode"></param>
        /// <returns></returns>
        public static string CHECK_RES_SMSNOTSEND1(PsmhDb pDbCon, string strPtNo, string strRDate, string strDrCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "0";

            try
            {
                SQL = "";
                SQL = "SELECT PTNO ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.ETC_SMS_RESNOTSEND";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND RDATE = TO_DATE('" + strRDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DRCODE = '" + strDrCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "Y";
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// TODO : bagage.bas : CHECK_RES_SMSNOTSEND2
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strBDate"></param>
        /// <param name="strDeptCode"></param>
        /// <returns></returns>
        public static string CHECK_RES_SMSNOTSEND2(PsmhDb pDbCon, string strPtNo, string strBDate, string strDeptCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "0";

            try
            {
                SQL = "";
                SQL = "SELECT PTNO ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.ETC_SMS_RESNOTSEND";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "Y";
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// bagage.bas : READ_ORDER_SUBUL_BUSE
        /// </summary>
        /// <param name="strArg"></param>
        /// <param name="strGBN"></param>
        /// <returns></returns>
        public static string READ_ORDER_SUBUL_BUSE(PsmhDb pDbCon, string strArg, string strGBN)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            if (strArg.Trim() == "")
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL = "SELECT NAME, CODE";
                SQL = SQL + ComNum.VBLF + "  From ADMIN.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'OCS_불출부서_코드'";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '" + strArg + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    if (strGBN == "OCS")
                    {
                        rtnVal = dt.Rows[0]["NAME"].ToString().Trim() + "." + dt.Rows[0]["CODE"].ToString().Trim();
                    }
                    else
                    {
                        rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// bagage.bas : CHECK_SUBUL_ORDERCODE
        /// </summary>
        /// <param name="strArg"></param>
        /// <returns></returns>
        public static bool CHECK_SUBUL_ORDERCODE(PsmhDb pDbCon, string strArg)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "SELECT ORDERCODE";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.OCS_ORDERCODE";
                SQL = SQL + ComNum.VBLF + " WHERE ORDERCODE = '" + strArg + "'";
                SQL = SQL + ComNum.VBLF + "   AND (SLIPNO = 'A6' OR BUN IN ('11','12','20','23'))";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// bagage.bas : CHECK_GS_ADD_GBN (외과 가산 구분 (외과, 흉부외과))
        /// </summary>
        /// <param name="strArg"></param>
        /// <returns></returns>
        public static string CHECK_GS_ADD_GBN(PsmhDb pDbCon, string strOrderCode, string strBDate)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "0";

            string strY = "0";
            string strZ = "0";

            try
            {
                strOrderCode = strOrderCode.Trim();
                if (strBDate == null)
                {
                    strBDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                }
                else
                {
                    if (strBDate == "")
                    {
                        strBDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                    }
                    else
                    {
                        strBDate = (Convert.ToDateTime(strBDate)).ToString("yyyy-MM-dd");
                    }
                }

                if (string.IsNullOrWhiteSpace(clsPublic.GstrZoneEmergencyStartDate))
                {
                    clsPublic.GstrZoneEmergencyStartDate = "2017-07-01";
                }

                if (string.Compare(strBDate, clsPublic.GstrZoneEmergencyStartDate) < 0)
                {
                    return rtnVal;
                }

                SQL = "";
                SQL += " SELECT /*+ INDEX(BAS_SUN INDEX_BASSUN20) */ nvl(SUGBY, 0) SUGBY    \r";
                SQL += "      , nvl(SUGBZ, 0) SUGBZ                                         \r";
                SQL += "   From ADMIN.BAS_SUN                                         \r";
                SQL += "  WHERE SUNEXT = '" + strOrderCode + "'                             \r";
                SQL += "  Union                                                             \r";
                SQL += " SELECT /*+ INDEX(BAS_SUN INDEX_BASSUN20) */ nvl(SUGBY, 0)SUGBY     \r";
                SQL += "      , nvl(SUGBZ, 0) SUGBZ                                         \r";
                SQL += "   From ADMIN.BAS_SUN                                         \r";
                SQL += "  WHERE SUNEXT IN  (                                                \r";
                SQL += "                     SELECT SUCODE                                  \r";
                SQL += "                       From ADMIN.OCS_ORDERCODE                \r";
                SQL += "                      WHERE ORDERCODE = '" + strOrderCode + "')     \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    if (int.Parse(dt.Rows[0]["SUGBY"].ToString().Trim()) >= 1)
                    {
                        strY = "1";
                    }

                    if (int.Parse(dt.Rows[0]["SUGBZ"].ToString().Trim()) >= 1)
                    {
                        strZ = "1";
                    }

                    if (strY == "1" && strZ == "1")
                    {
                        rtnVal = "3";
                    }
                    else if (strY == "1" && strZ == "0")
                    {
                        rtnVal = "1";
                    }
                    else if (strY == "0" && strZ == "1")
                    {
                        rtnVal = "2";
                    }
                    else
                    {
                        rtnVal = "0";
                    }
                }
                else
                {
                    rtnVal = "0";
                }

                //2019-06-03 "O1510A" 외과가산 안물리도록
                if (strOrderCode.Trim() == "O1510A") rtnVal = "0";

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        public static void SET_COMBO_ORDER_SUBUL_DEPT(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread spdNm, int nCol, int nRow)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            string strCode = "";
            string[] sComboList = new string[5];

            if (spdNm.ActiveSheet.RowCount == 0)
            {
                return;
            }

            FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            try
            {
                SQL = "";
                SQL += " SELECT NAME, CODE                      \r";
                SQL += "   FROM ADMIN.BAS_BCODE           \r";
                SQL += "  WHERE GUBUN = 'OCS_불출부서_코드'     \r";
                SQL += "  ORDER BY SORT                         \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    Array.Resize(ref sComboList, dt.Rows.Count);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sComboList[i] = dt.Rows[i]["NAME"].ToString().Trim() + "." + dt.Rows[i]["CODE"].ToString().Trim();
                    }




                    combo.Items = sComboList;
                    combo.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
                    //combo.MaxDrop = dt.Rows.Count;
                    combo.MaxDrop = 20;
                    combo.MaxLength = 100;
                    combo.ListWidth = 200;
                    combo.Editable = false;

                    if (nRow == -1)
                    {
                        spdNm.ActiveSheet.Cells[0, nCol, spdNm.ActiveSheet.RowCount - 1, nCol].Text = "";
                        spdNm.ActiveSheet.Columns[nCol].CellType = combo;
                    }
                    else
                    {
                        spdNm.ActiveSheet.Cells[nRow, nCol].Text = "";
                        spdNm.ActiveSheet.Cells[nRow, nCol].CellType = combo;
                    }

                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        public static void SET_RETURN_LABEL(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread spdNm, int nCol, int nRow)
        {
            string sPreValue = "";
            FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();

            if (nRow == -1)
            {
                for (int i = 0; i < spdNm.ActiveSheet.RowCount; i++)
                {
                    sPreValue = spdNm.ActiveSheet.Cells[i, nCol].Text;
                    spdNm.ActiveSheet.Cells[i, nCol].CellType = txt;
                    spdNm.ActiveSheet.Cells[i, nCol].Text = "";
                    spdNm.ActiveSheet.Cells[i, nCol].Text = sPreValue;
                }
            }
            else
            {
                sPreValue = spdNm.ActiveSheet.Cells[nRow, nCol].Text;
                spdNm.ActiveSheet.Cells[nRow, nCol].CellType = txt;
                spdNm.ActiveSheet.Cells[nRow, nCol].Text = "";
                spdNm.ActiveSheet.Cells[nRow, nCol].Text = sPreValue;
            }
        }

        public static void SET_RETURN_CHECKBOX(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread spdNm, int nCol, int nRow, string strValue)
        {
            FarPoint.Win.Spread.CellType.CheckBoxCellType chk = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            spdNm.ActiveSheet.Cells[nRow, nCol].CellType = chk;
            if (strValue == "1")
            {
                spdNm.ActiveSheet.Cells[nRow, nCol].Text = "True";
            }
            else
            {
                spdNm.ActiveSheet.Cells[nRow, nCol].Text = "False";
            }
        }

        public static void SET_COMBO_GS_ADD(FarPoint.Win.Spread.FpSpread spdNm, int nCol, int nRow, string sGbn)
        {
            string[] sComboGsAddList = new string[3];

            FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            switch (sGbn)
            {
                case "1":
                    sComboGsAddList[0] = "0.기타";
                    sComboGsAddList[1] = "1.GS";
                    break;
                case "2":
                    sComboGsAddList[0] = "0.기타";
                    sComboGsAddList[1] = "2.CS";
                    break;
                case "3":
                    sComboGsAddList[0] = "0.기타";
                    sComboGsAddList[1] = "1.GS";
                    sComboGsAddList[2] = "2.CS";
                    break;
                default:
                    clsBagage.SET_RETURN_LABEL(clsDB.DbCon, spdNm, nCol, nRow);
                    return;
            }

            //spdNm.ActiveSheet.Cells[nRow, nCol].Text = "";
            combo.Items = sComboGsAddList;
            combo.AutoSearch = FarPoint.Win.AutoSearch.MultipleCharacter;
            combo.MaxDrop = 3;
            combo.MaxLength = 150;
            combo.ListWidth = 100;
            combo.Editable = false;
            spdNm.ActiveSheet.Cells[nRow, nCol].CellType = combo;
            spdNm.ActiveSheet.Cells[nRow, nCol].Locked = false;
        }

        /// <summary>
        /// CHECK_후불가능조건
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="spdNm"></param>
        /// <param name="nCol"></param>
        /// <param name="nRow"></param>
        public static string CHECK_DEFERRED_STAT(PsmhDb pDbCon, string strDeptCode, string strOrderCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            string strRtn = "NO";

            strOrderCode = strOrderCode.Trim();


            if (strOrderCode == "C3710" || strOrderCode == "A26" || strOrderCode == "A27")
            {
                strRtn = "OK";
                return strRtn;
            }

            switch (strDeptCode)
            {
                case "OS":
                case "MP":
                case "MC":
                case "ME":
                    strRtn = "OK";
                    break;
                default:
                    strRtn = "NO";
                    break;
            }

            if (strRtn == "" || strRtn == "NO")
            {
                strRtn = "NO";
                return strRtn;
            }

            strRtn = "NO";

            switch (strDeptCode)
            {
                case "OS":
                    try
                    {
                        SQL = "";
                        SQL += " SELECT ORDERCODE                           \r";
                        SQL += "   FROM ADMIN.OCS_ORDERCODE            \r";
                        SQL += "  WHERE SLIPNO = '0060'                     \r";
                        SQL += "    AND ORDERCODE = '" + strOrderCode + "'  \r";
                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return strRtn;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            strRtn = "OK";
                        }
                        dt.Dispose();
                        dt = null;
                        return strRtn;
                    }
                    catch (Exception ex)
                    {
                        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox(ex.Message);
                        Cursor.Current = Cursors.Default;
                        return strRtn;
                    }
                case "MC":
                    if (strOrderCode == "01030110" || strOrderCode == "00600410")
                    {
                        strRtn = "OK";
                    }
                    break;
                case "MP":
                    if (strOrderCode == "00600440" || strOrderCode == "00602830" || strOrderCode == "F6001A" || strOrderCode == "01030110")
                    {
                        strRtn = "OK";
                    }
                    break;
                case "ME":          //2019-10-26 내분비 추가
                    if (strOrderCode == "C3710" || strOrderCode == "A26" || strOrderCode == "A27")
                    {
                        strRtn = "OK";
                    }
                    break;
                default:
                    break;
            }
            return strRtn;
        }

        public static void SET_CHECKBOX_ORDER_XRAY(PsmhDb pDbCon, string sOrdCode, string sPano, string sBDate, string sOrdNo, string sDC, FarPoint.Win.Spread.FpSpread spdNm, int nCol, int nRow)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            string strCode = "";
            string strOK = "";

            FarPoint.Win.Spread.CellType.CheckBoxCellType chk = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

            if (clsOrdFunction.GstrGbJob != "OPD")
                return;

            if (sDC == "True")
            {
                clsBagage.SET_RETURN_LABEL(pDbCon, spdNm, nCol, nRow);
                return;
            }
            if (sOrdCode.Trim() == "")
            {
                clsBagage.SET_RETURN_LABEL(pDbCon, spdNm, nCol, nRow);
                return;
            }

            //코드가 없으면 라벨로 세팅 후 빠져나감
            try
            {
                SQL = "";
                SQL += " SELECT GBAUTOSEND2                                         \r";
                SQL += "   FROM ADMIN.OCS_OORDER                               \r";
                SQL += "  WHERE PTNO = '" + sPano + "'                              \r";
                SQL += "    AND BDATE = TO_DATE('" + sBDate + "','YYYY-MM-DD')      \r";
                SQL += "    AND ORDERNO = " + VB.Val(sOrdNo) + "                    \r";
                if (sDC == "True")
                {
                    SQL += "    AND NAL < 0                                         \r";
                }
                else
                {
                    SQL += "    AND NAL > 0                                         \r";
                }
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["GBAUTOSEND2"].ToString().Trim())
                    {
                        case "2":
                            SET_LABEL_BLUE(pDbCon, spdNm, nCol, nRow);
                            //break;
                            return;
                        default:
                            //SET_RETURN_LABEL(pDbCon, spdNm, nCol, nRow);
                            SET_RETURN_CHECKBOX(pDbCon, spdNm, nCol, nRow, "");
                            //break;
                            return;
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

            //spdNm.ActiveSheet.Cells[nRow, nCol].Text = "";
            spdNm.ActiveSheet.Cells[nRow, nCol].CellType = chk;
            spdNm.ActiveSheet.SetActiveCell(nRow, nCol);
        }

        public static void SET_LABEL_BLUE(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread spdNm, int nCol, int nRow)
        {
            spdNm.ActiveSheet.Cells[nRow, nCol].BackColor = Color.Blue;
        }

        public static bool ReadERWardSelect(string argWARD, string argRoom = "")
        {
            bool RtnVal = false;

            switch (argWARD)
            {
                case "65":
                case "83":
                    RtnVal = true;
                    break;
            }

            return RtnVal;
        }

        public static bool Read10WardSelect(string argWARD, string argRoom = "")
        {
            bool RtnVal = false;

            switch (argWARD)
            {
                case "10":
                    RtnVal = true;
                    break;
            }

            return RtnVal;
        }

        public static bool USE_DIF()
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = " SELECT NAME";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'DRUG_DIF_교체'";
                SQL = SQL + ComNum.VBLF + "   AND CODE = 'USE'";
                SQL = SQL + ComNum.VBLF + "   AND NAME = 'Y'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtVal = true;
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

            return rtVal;
        }
    }
}
