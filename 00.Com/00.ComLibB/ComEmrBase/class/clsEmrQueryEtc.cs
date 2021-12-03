using ComBase; //기본 클래스
using ComDbB; //DB연결
using System;
using System.Data;
using System.Windows.Forms;


namespace ComEmrBase
{
    public class clsEmrQueryEtc
    {
        /// <summary>
        /// 재원환자의 경우 당일 임상관찰 기본 시간을 저장
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strAcpNo"></param>
        /// <param name="strWard"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strFormNo"></param>
        /// <returns></returns>
        public static bool SetSaveDefaultVitalTime(PsmhDb pDbCon, string strAcpNo, string strWard, string strChartDate, string strFormNo)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strJOBGB = "IVT";

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");
                string strCurDate = VB.Left(strCurDateTime, 8);
                string strCurTime = VB.Right(strCurDateTime, 6);

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT 1";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRBVITALTIME";
                SQL = SQL + ComNum.VBLF + "  WHERE FORMNO = " + strFormNo;
                SQL = SQL + ComNum.VBLF + "   AND ACPNO = " + strAcpNo;
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE = '" + strChartDate + "'";
                SQL = SQL + ComNum.VBLF + "   AND JOBGB = '" + strJOBGB + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    return true;
                }
                dt.Dispose();
                dt = null;

                if (strWard != "")
                {
                    switch (strWard)
                    {
                        case "ER":
                        case "OP":
                        case "ENDO":
                        case "HD":
                        case "AG":
                        case "CT":
                        case "MRI":
                        case "RI":
                        case "SONO":
                        case "외래수혈":
                            break;
                        case "33":
                        case "34":
                            for (i = 0; i < 25; i++)
                            {
                                string strTIMEVALUE = ComFunc.SetAutoZero(i.ToString().Trim(), 2) + "00";

                                SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALTIME ";
                                SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, JOBGB , CHARTDATE, TIMEVALUE, SUBGB, WRITEDATE, WRITETIME, WRITEUSEID)";
                                SQL = SQL + ComNum.VBLF + "VALUES (";
                                SQL = SQL + ComNum.VBLF + "" + strFormNo + ",";  //FORMNO
                                SQL = SQL + ComNum.VBLF + "" + strAcpNo + ",";  //ACPNO
                                SQL = SQL + ComNum.VBLF + "'" + strJOBGB + "',";    //JOBGB
                                SQL = SQL + ComNum.VBLF + "'" + strChartDate + "',";  //CHARTDATE
                                SQL = SQL + ComNum.VBLF + "'" + strTIMEVALUE + "',"; //TIMEVALUE
                                SQL = SQL + ComNum.VBLF + "'0',";   //SUBGB
                                SQL = SQL + ComNum.VBLF + "'" + strCurDate + "',";  //WRITEDATE
                                SQL = SQL + ComNum.VBLF + "'" + strCurTime + "',";  //WRITETIME
                                SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "'";    //WRITEUSEID
                                SQL = SQL + ComNum.VBLF + ")";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return false;
                                }
                            }
                            break;
                        default:
                            for (i = 1; i <= 23; i += 2)
                            {
                                string strTIMEVALUE = ComFunc.SetAutoZero(i.ToString().Trim(), 2) + "00";

                                SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALTIME ";
                                SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, JOBGB , CHARTDATE, TIMEVALUE, SUBGB, WRITEDATE, WRITETIME, WRITEUSEID)";
                                SQL = SQL + ComNum.VBLF + "VALUES (";
                                SQL = SQL + ComNum.VBLF + "" + strFormNo + ",";  //FORMNO
                                SQL = SQL + ComNum.VBLF + "" + strAcpNo + ",";  //ACPNO
                                SQL = SQL + ComNum.VBLF + "'" + strJOBGB + "',";    //JOBGB
                                SQL = SQL + ComNum.VBLF + "'" + strChartDate + "',";  //CHARTDATE
                                SQL = SQL + ComNum.VBLF + "'" + strTIMEVALUE + "',"; //TIMEVALUE
                                SQL = SQL + ComNum.VBLF + "'0',";   //SUBGB
                                SQL = SQL + ComNum.VBLF + "'" + strCurDate + "',";  //WRITEDATE
                                SQL = SQL + ComNum.VBLF + "'" + strCurTime + "',";  //WRITETIME
                                SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "'";    //WRITEUSEID
                                SQL = SQL + ComNum.VBLF + ")";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return false;
                                }
                            }
                            break;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }


        public static bool CheckHCBuse(string strBUSE)
        {
            bool rtn = false;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT NAME FROM KOSMOS_PMPA.BAS_BUSE ";
                SQL += ComNum.VBLF + " WHERE BUCODE = '" + strBUSE + "' ";
                SQL += ComNum.VBLF + "   AND DEPT_ID_UP = '44500' ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtn;
                }
                if (dt.Rows.Count > 0)
                {
                    rtn = true;
                }

                dt.Dispose();
                dt = null;

                return rtn;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtn;
            }
        }

        public static string CheckPastMedicalHistory(string strPTNO, string strMedFrDate)
        {
            //간호정보조사지의 과거병력-유 여부 체크
            string chkReturn = "";
            string rtn = "";

            int i = 0;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL += ComNum.VBLF + " SELECT ITEMCD, ITEMVALUE  ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_EMR.AEMRCHARTROW  ";
                SQL += ComNum.VBLF + "  WHERE EMRNO IN ( ";
                SQL += ComNum.VBLF + "                 SELECT EMRNO  ";
                SQL += ComNum.VBLF + "                   FROM KOSMOS_EMR.AEMRCHARTMST ";
                //SQL += ComNum.VBLF + "                  WHERE FORMNO IN (2285,2294,2295,2296,2305,2311,2356) ";
                SQL += ComNum.VBLF + "                  WHERE FORMNO = 2311";
                SQL += ComNum.VBLF + "                    AND MEDFRDATE = '" + strMedFrDate + "' ";
                SQL += ComNum.VBLF + "                    AND PTNO = '" + strPTNO + "' ";
                SQL += ComNum.VBLF + "                    AND INOUTCLS = 'I' ";
                SQL += ComNum.VBLF + "                  ) ";
                SQL += ComNum.VBLF + "    AND ITEMNO IN ('I0000034768','I0000034769','I0000034770','I0000034771','I0000034772','I0000034773','I0000034774','I0000034383', 'I0000034375') ";
                SQL += ComNum.VBLF + " ORDER BY ITEMCD ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtn;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["ITEMVALUE"].ToString().Trim() == "1")
                        {
                            if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000034375")    //과거병력-유
                            {
                                chkReturn = "OK";
                            }
                            switch (dt.Rows[i]["ITEMCD"].ToString().Trim())
                            {
                                case "I0000034768": //고혈압
                                    rtn += "고혈압 ";
                                    break;
                                case "I0000034769": //당뇨
                                    rtn += "당뇨 ";
                                    break;
                                case "I0000034770": //결핵
                                    rtn += "결핵 ";
                                    break;
                                case "I0000034771": //간염
                                    rtn += "간염 ";
                                    break;
                                case "I0000034772": //뇌졸중
                                    rtn += "뇌졸중 ";
                                    break;
                                case "I0000034773": //심장질환
                                    rtn += "심장질환 ";
                                    break;
                                case "I0000034774": //암
                                    rtn += "암 ";
                                    break;
                                case "I0000034383": //과거병력 기타
                                    rtn += "기타:" + dt.Rows[i]["ITEMCD"].ToString().Trim() + " ";
                                    break;
                            }
                        }


                        

                    }

                }

                dt.Dispose();
                dt = null;

                if (chkReturn != "OK")
                {
                    rtn = "";
                }

                return rtn;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtn;
            }
        }

        public static string ReadHCWeight(string strPTNO, string strDATE)
        {
            string rtn = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " select b.result from kosmos_pmpa.hic_jepsu a, kosmos_pmpa.hic_result b ";
                SQL += ComNum.VBLF + " where a.wrtno = b.wrtno ";
                SQL += ComNum.VBLF + " and a.deldate is null ";
                SQL += ComNum.VBLF + " and a.ptno = '" + strPTNO + "' ";
                SQL += ComNum.VBLF + " and a.jepdate = to_date('" + strDATE + "', 'yyyy-mm-dd') ";
                SQL += ComNum.VBLF + " and b.excode in('A102') ";
                SQL += ComNum.VBLF + " AND B.RESULT IS NOT NULL ";
                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + " select b.result from kosmos_pmpa.hea_jepsu a, kosmos_pmpa.hea_result b ";
                SQL += ComNum.VBLF + " where a.wrtno = b.wrtno ";
                SQL += ComNum.VBLF + " and a.deldate is null ";
                SQL += ComNum.VBLF + " and a.ptno = '" + strPTNO + "' ";
                SQL += ComNum.VBLF + " and a.sdate = to_date('" + strDATE + "', 'yyyy-mm-dd') ";
                SQL += ComNum.VBLF + " and b.excode in('A102') ";
                SQL += ComNum.VBLF + " AND B.RESULT IS NOT NULL ";
                //SQL += ComNum.VBLF + " order by excode ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtn;
                }
                if (dt.Rows.Count > 0)
                {
                    rtn = dt.Rows[0]["result"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtn;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtn;
            }
        }

        public static string ReadHCEndoGubun(string strPTNO, string strDATE, string strDEPTCODE)
        {

            //건진의 내시경 수면 여부 용... 으로 사용하다가 본관도 같이 사용.
            string rtn = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT ORDERNAME FROM KOSMOS_OCS.OCS_ORDERCODE ";
                SQL += ComNum.VBLF + " WHERE ORDERNAME LIKE '%수면%' ";
                SQL += ComNum.VBLF + "   AND ORDERCODE = (SELECT ORDERCODE FROM KOSMOS_OCS.ENDO_JUPMST  ";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + strPTNO + "' ";
                if(strDEPTCODE == "HR" || strDEPTCODE == "TO")
                {
                    SQL += ComNum.VBLF + "   AND JDATE = TO_DATE('" + strDATE + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL += ComNum.VBLF + "   AND TRUNC(RDATE) = TO_DATE('" + strDATE + "','YYYY-MM-DD') ";
                }
                SQL += ComNum.VBLF + "   AND DEPTCODE = '" + strDEPTCODE + "'";
                SQL += ComNum.VBLF + "   AND GBSUNAP IN('1', '7') ";
                SQL += ComNum.VBLF + "   AND ROWNUM = 1) ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtn;
                }
                if (dt.Rows.Count > 0)
                {
                    rtn = "수면";
                }

                dt.Dispose();
                dt = null;

                return rtn;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtn;
            }
        }


        /// <summary>
        /// 신 EMR 사용여부
        /// </summary>
        /// <returns></returns>
        public static bool NewEmrStart()
        {
            return true;
            //bool rtnVal = false;
            ////////string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

            ////////if (VB.Val(strCurDateTime) >= 20200422070000)
            ////////{
            ////////    rtnVal = true;
            ////////}

            //DataTable dt = null;
            //string SQL = "";    //Query문
            //string SqlErr = ""; //에러문 받는 변수

            //try
            //{

            //    SQL = " ";
            //    SQL = SQL + ComNum.VBLF + "SELECT ";
            //    SQL = SQL + ComNum.VBLF + "    NFLAG1 ";
            //    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_BASCD ";
            //    SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = '프로그램기초' ";
            //    SQL = SQL + ComNum.VBLF + "AND GRPCD = '프로그램' ";
            //    SQL = SQL + ComNum.VBLF + "AND BASCD = '신EMR사용' ";

            //    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            //    if (SqlErr != "")
            //    {
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        return rtnVal;
            //    }
            //    if (dt.Rows.Count == 0)
            //    {
            //        dt.Dispose();
            //        dt = null;
            //        return rtnVal;
            //    }
                
            //    if (dt.Rows[0]["NFLAG1"].ToString().Trim() == "1")
            //    {
            //        rtnVal = true;
            //    }
            //    dt.Dispose();
            //    dt = null;
                
            //    return rtnVal;
            //}
            //catch (Exception ex)
            //{
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //    ComFunc.MsgBox(ex.Message);
            //    return rtnVal;
            //}
        }

        /// <summary>
        /// 해당일자의 아이템을 만든다
        /// </summary>
        public static bool GetSetTodayItem(PsmhDb pDbCon, EmrPatient AcpEmr, string strChartDate, string strFormNo)
        {
            #region //1.일자별 등록된 것이 있는지 파악해서 있으면 세팅을 하고
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            #region //query

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    A.CHARTDATE, B.ITEMCD, B.BASNAME, B.BASEXNAME, B.ITEMGROUP, B.ITEMGROUPNM ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN ( ";
            SQL = SQL + ComNum.VBLF + "    SELECT ";
            SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
            SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
            SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('임상관찰그룹')  ";
            SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('임상관찰')  ";
            SQL = SQL + ComNum.VBLF + "    UNION ALL ";
            SQL = SQL + ComNum.VBLF + "    SELECT ";
            SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
            SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
            SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('섭취배설그룹')  ";
            SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('섭취배설') ";
            SQL = SQL + ComNum.VBLF + "    UNION ALL ";
            SQL = SQL + ComNum.VBLF + "    SELECT ";
            SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
            SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
            SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('특수치료그룹')  ";
            SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('특수치료') ";
            SQL = SQL + ComNum.VBLF + "    UNION ALL ";
            SQL = SQL + ComNum.VBLF + "    SELECT ";
            SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
            SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
            SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('기본간호그룹')  ";
            SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('기본간호') ";
            SQL = SQL + ComNum.VBLF + "    ) B ";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.ITEMCD ";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + strFormNo;
            SQL = SQL + ComNum.VBLF + "    AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + strChartDate + "'";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.SORT1, B.SORT2, B.SORT3, B.SORT4, B.SORT5, B.SORT6 ";
            #endregion //query

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }

            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                dt = null;
                return true;
            }

            dt.Dispose();
            dt = null;
            #endregion

            #region //2.없으면 이전 날짜 가지고 와서 세팅

            SQL = "SELECT B.BASCD, B.BASNAME, B.BASEXNAME, B.DISSEQNO, A.CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON B.BASCD = A.ITEMCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호')";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + strFormNo;
            SQL = SQL + ComNum.VBLF + "    AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(A1.CHARTDATE) AS CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "                                        FROM " + ComNum.DB_EMR + "AEMRBVITALSET A1";
            SQL = SQL + ComNum.VBLF + "                                        WHERE A1.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "                                            AND A1.FORMNO = " + strFormNo;
            SQL = SQL + ComNum.VBLF + "                                            AND A1.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
            SQL = SQL + ComNum.VBLF + "                                            AND A1.CHARTDATE <= '" + strChartDate + "')";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }

            if (dt.Rows.Count > 0)
            {
                try
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                    SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD, WRITEDATE, WRITETIME, WRITEUSEID)";
                    SQL = SQL + ComNum.VBLF + "SELECT ";
                    SQL = SQL + ComNum.VBLF + "     A.FORMNO, A.ACPNO, A.PTNO, ";
                    SQL = SQL + ComNum.VBLF + "     '" + strChartDate + "' AS CHARTDATE, ";
                    SQL = SQL + ComNum.VBLF + "     A.JOBGB, A.ITEMCD, A.WRITEDATE, A.WRITETIME, A.WRITEUSEID";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
                    SQL = SQL + ComNum.VBLF + "    ON B.BASCD = A.ITEMCD";
                    SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
                    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호')";
                    SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
                    SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + strFormNo;
                    SQL = SQL + ComNum.VBLF + "    AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
                    SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(A1.CHARTDATE) AS CHARTDATE ";
                    SQL = SQL + ComNum.VBLF + "                                        FROM " + ComNum.DB_EMR + "AEMRBVITALSET A1";
                    SQL = SQL + ComNum.VBLF + "                                        WHERE A1.ACPNO = " + AcpEmr.acpNo;
                    SQL = SQL + ComNum.VBLF + "                                            AND A1.FORMNO = " + strFormNo;
                    SQL = SQL + ComNum.VBLF + "                                            AND A1.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
                    SQL = SQL + ComNum.VBLF + "                                            AND A1.CHARTDATE <= '" + strChartDate + "')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }

                    dt.Dispose();
                    dt = null;
                    return true;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
            }
            else
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox(AcpEmr.ptName + "[" + AcpEmr.ptNo + "] 님의" + ComNum.VBLF + "당일 임상관찰 아이템을 찾을 수 없습니다.");
                return false;
            }
            #endregion

            //3.없으면 기본값을 세팅    
        }

        /// <summary>
        /// 해당일자에 차팅된 기록지의 아이템을 확인하여 저장한다
        /// </summary>
        /// <param name="AcpEmr"></param>
        /// <param name="strChartItems"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strCurDate"></param>
        /// <param name="strCurTime"></param>
        /// <returns></returns>
        public static bool GetSetTodayChartedItem(PsmhDb pDbCon, EmrPatient AcpEmr, string strChartItems, string strChartDate, string strFormNo, 
            string strCurDate, string strCurTime, string strJOBGB)
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;
            int i = 0;
            int j = 0;

            string[] arryITEMCD_T = VB.Split(strChartItems, ",");

            //당일 아이템을 조회해서 다시 정리 한다
            #region //Query
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    A.CHARTDATE, B.ITEMCD, B.BASNAME, B.BASEXNAME, B.ITEMGROUP, B.ITEMGROUPNM";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN ( ";
            SQL = SQL + ComNum.VBLF + "    SELECT ";
            SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
            SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
            SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('임상관찰그룹')  ";
            SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('임상관찰')  ";
            SQL = SQL + ComNum.VBLF + "    UNION ALL ";
            SQL = SQL + ComNum.VBLF + "    SELECT ";
            SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
            SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
            SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('섭취배설그룹')  ";
            SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('섭취배설') ";
            SQL = SQL + ComNum.VBLF + "    UNION ALL ";
            SQL = SQL + ComNum.VBLF + "    SELECT ";
            SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
            SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
            SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('특수치료그룹')  ";
            SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('특수치료') ";
            SQL = SQL + ComNum.VBLF + "    UNION ALL ";
            SQL = SQL + ComNum.VBLF + "    SELECT ";
            SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
            SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
            SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('기본간호그룹')  ";
            SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('기본간호') ";
            SQL = SQL + ComNum.VBLF + "    ) B ";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.ITEMCD ";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + strFormNo;
            SQL = SQL + ComNum.VBLF + "    AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + strChartDate + "'";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.SORT1, B.SORT2, B.SORT3, B.SORT4, B.SORT5, B.SORT6 ";
            #endregion //Query

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

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
                ComFunc.MsgBox(AcpEmr.ptName + "[" + AcpEmr.ptNo + "] 님의" + ComNum.VBLF + "당일 임상관찰 아이템을 찾을 수 없습니다.");
                return rtnVal;
            }

            //먼저 누락된 아이템이 있는지 확인해야 한다 ㅠ.ㅠ
            try
            {
                for (j = 0; j < arryITEMCD_T.Length; j++)
                {
                    bool IsExistsItem = false;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (arryITEMCD_T[j].Trim().Replace("'", "") == dt.Rows[i]["ITEMCD"].ToString().Trim())
                        {
                            IsExistsItem = true;
                            break;
                        }
                    }

                    if (IsExistsItem == false)
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                        SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD, WRITEDATE, WRITETIME, WRITEUSEID)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "    (";
                        SQL = SQL + ComNum.VBLF + "    " + strFormNo + ","; //FORMNO
                        SQL = SQL + ComNum.VBLF + "    " + AcpEmr.acpNo + ","; //ACPNO
                        SQL = SQL + ComNum.VBLF + "    '" + AcpEmr.ptNo + "',"; //PTNO
                        SQL = SQL + ComNum.VBLF + "    '" + strChartDate + "',"; //CHARTDATE
                        SQL = SQL + ComNum.VBLF + "    '" + strJOBGB + "',"; //JOBGB
                        SQL = SQL + ComNum.VBLF + "    '" + arryITEMCD_T[j].Trim().Replace("'", "") + "',"; //ITEMCD
                        SQL = SQL + ComNum.VBLF + "    '" + strCurDate + "',"; //WRITEDATE
                        SQL = SQL + ComNum.VBLF + "    '" + strCurTime + "',"; //WRITETIME
                        SQL = SQL + ComNum.VBLF + "    '" + clsType.User.IdNumber + "'"; //WRITEUSEID
                        SQL = SQL + ComNum.VBLF + "    )";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            ComFunc.MsgBox("아이템 저장중 오류가 발생하였습니다.");
                            return rtnVal;
                        }
                    }
                }
                rtnVal = true;
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// 차팅시간을 저장한다
        /// </summary>
        /// <param name="AcpEmr"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strCurDate"></param>
        /// <param name="strCurTime"></param>
        /// <returns></returns>
        public static bool SaveAEMRBVITALTIME(PsmhDb pDbCon, EmrPatient AcpEmr, string strChartDate, string strChartTime, string strFormNo, string strCurDate, string strCurTime)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            string strJOBGB = "IVT";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TIMEVALUE";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRBVITALTIME";
                SQL = SQL + ComNum.VBLF + " WHERE FORMNO = " + strFormNo;
                SQL = SQL + ComNum.VBLF + "   AND ACPNO = " + AcpEmr.acpNo;
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE = '" + strChartDate + "'";
                SQL = SQL + ComNum.VBLF + "   AND JOBGB = '" + strJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "   AND SUBSTR(TIMEVALUE, 1, 4) = '" + VB.Left(strChartTime, 4) + "'";
                SQL = SQL + ComNum.VBLF + "   AND SUBGB = '0'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("AEMRBVITALTIME 조회중 문제가 발생했습니다");
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    return true;
                }

                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALTIME ";
                SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, JOBGB , CHARTDATE, TIMEVALUE, SUBGB, WRITEDATE, WRITETIME, WRITEUSEID)";
                SQL = SQL + ComNum.VBLF + "VALUES (";
                SQL = SQL + ComNum.VBLF + "" + strFormNo + ",";  //FORMNO
                SQL = SQL + ComNum.VBLF + "" + AcpEmr.acpNo + ",";  //ACPNO
                SQL = SQL + ComNum.VBLF + "'" + strJOBGB + "',";    //JOBGB
                SQL = SQL + ComNum.VBLF + "'" + strChartDate + "',";  //CHARTDATE
                SQL = SQL + ComNum.VBLF + "'" + VB.Left(strChartTime, 4) + "',"; //TIMEVALUE
                SQL = SQL + ComNum.VBLF + "'0',";   //SUBGB
                SQL = SQL + ComNum.VBLF + "'" + strCurDate + "',";  //WRITEDATE
                SQL = SQL + ComNum.VBLF + "'" + strCurTime + "',";  //WRITETIME
                SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "'";    //WRITEUSEID
                SQL = SQL + ComNum.VBLF + ")";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 당일 섭취배설이 있을 경우 총섭취량, 총배설량이 있는지 확인해서 강제로 넣는다.
        /// GetSetTotInOut(p.acpNo.ToString(), p.ptNo.ToString(), mstrFormNo, "IIO", dtpFrDate.Value.ToString("yyyyMMdd"));
        /// </summary>
        public static bool GetSetTotInOut(PsmhDb pDbCon, string strACPNO, string strPTNO, string strFORMNO, string strJOBGB, string strCHARTDATE)
        {
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");
            string strITEMCD = "I0000030622";

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "     ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "    AND (B.ITEMVALUE <> '' OR B.ITEMVALUE IS NOT NULL)";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD I";
            SQL = SQL + ComNum.VBLF + "     ON  B.ITEMCD = I.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND I.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "    AND I.UNITCLS = '섭취배설' ";
            SQL = SQL + ComNum.VBLF + "    AND I.VFLAG3 = '01.섭취' ";
            SQL = SQL + ComNum.VBLF + " WHERE A.FORMNO = " + strFORMNO;
            SQL = SQL + ComNum.VBLF + "   AND A.ACPNO = " + strACPNO;
            SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE = '" + strCHARTDATE + "'";
            SQL = SQL + ComNum.VBLF + "   AND A.FORMNO = " + strFORMNO;
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            if (dt.Rows.Count > 0)
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    A.ITEMCD";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_EMR + "AEMRBVITALSET A ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.ACPNO = " + strACPNO;
                SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + strCHARTDATE + "'";
                SQL = SQL + ComNum.VBLF + "    AND A.JOBGB = '" + strJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD = '" + strITEMCD + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + strFORMNO;
                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt1.Rows.Count == 0)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                    SQL = SQL + ComNum.VBLF + "( ";
                    SQL = SQL + ComNum.VBLF + "    FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD,  ";
                    SQL = SQL + ComNum.VBLF + "    WRITEDATE, WRITETIME, WRITEUSEID ";
                    SQL = SQL + ComNum.VBLF + ") ";
                    SQL = SQL + ComNum.VBLF + "VALUES (";
                    SQL = SQL + ComNum.VBLF + "    " + strFORMNO + ", "; //FORMNO
                    SQL = SQL + ComNum.VBLF + "    " + strACPNO + ", "; //ACPNO
                    SQL = SQL + ComNum.VBLF + "    '" + strPTNO + "', "; //PTNO
                    SQL = SQL + ComNum.VBLF + "    '" + strCHARTDATE + "', "; //CHARTDATE
                    SQL = SQL + ComNum.VBLF + "    '" + strJOBGB + "', "; //JOBGB
                    SQL = SQL + ComNum.VBLF + "    '" + strITEMCD + "', "; //ITEMCD
                    SQL = SQL + ComNum.VBLF + "    '" + VB.Left(strCurDateTime, 8) + "', "; //WRITEDATE
                    SQL = SQL + ComNum.VBLF + "    '" + VB.Right(strCurDateTime, 6) + "', "; //WRITETIME
                    SQL = SQL + ComNum.VBLF + "    '" + clsType.User.IdNumber + "' "; //WRITEUSEID
                    SQL = SQL + ComNum.VBLF + ") ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }
                }
                dt1.Dispose();
                dt1 = null;
            }
            dt.Dispose();
            dt = null;

            strITEMCD = "I0000030623";

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "    AND (B.ITEMVALUE <> '' OR B.ITEMVALUE IS NOT NULL)";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD I";
            SQL = SQL + ComNum.VBLF + "    ON  B.ITEMCD = I.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND I.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "    AND I.UNITCLS = '섭취배설' ";
            SQL = SQL + ComNum.VBLF + "    AND I.VFLAG3 = '11.배설' ";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + strFORMNO;
            SQL = SQL + ComNum.VBLF + "    AND A.ACPNO = " + strACPNO;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + strCHARTDATE + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + strFORMNO;
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            if (dt.Rows.Count > 0)
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    A.ITEMCD";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A ";
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + strACPNO;
                SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + strCHARTDATE + "'";
                SQL = SQL + ComNum.VBLF + "    AND A.JOBGB = '" + strJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD = '" + strITEMCD + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + strFORMNO;
                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt1.Rows.Count == 0)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                    SQL = SQL + ComNum.VBLF + "( ";
                    SQL = SQL + ComNum.VBLF + "    FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD,  ";
                    SQL = SQL + ComNum.VBLF + "    WRITEDATE, WRITETIME, WRITEUSEID ";
                    SQL = SQL + ComNum.VBLF + ") ";
                    SQL = SQL + ComNum.VBLF + "VALUES (";
                    SQL = SQL + ComNum.VBLF + "    " + strFORMNO + ", "; //FORMNO
                    SQL = SQL + ComNum.VBLF + "    " + strACPNO + ", "; //ACPNO
                    SQL = SQL + ComNum.VBLF + "    '" + strPTNO + "', "; //PTNO
                    SQL = SQL + ComNum.VBLF + "    '" + strCHARTDATE + "', "; //CHARTDATE
                    SQL = SQL + ComNum.VBLF + "    '" + strJOBGB + "', "; //JOBGB
                    SQL = SQL + ComNum.VBLF + "    '" + strITEMCD + "', "; //ITEMCD
                    SQL = SQL + ComNum.VBLF + "    '" + VB.Left(strCurDateTime, 8) + "', "; //WRITEDATE
                    SQL = SQL + ComNum.VBLF + "    '" + VB.Right(strCurDateTime, 6) + "', "; //WRITETIME
                    SQL = SQL + ComNum.VBLF + "    '" + clsType.User.IdNumber + "' "; //WRITEUSEID
                    SQL = SQL + ComNum.VBLF + ") ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }
                }
                dt1.Dispose();
                dt1 = null;
            }
            dt.Dispose();
            dt = null;

            return true;
        }

        /// <summary>
        /// 총섭취량, 총배설량 아이템을 추가한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strACPNO"></param>
        /// <param name="strPTNO"></param>
        /// <param name="strFORMNO"></param>
        /// <param name="strJOBGB"></param>
        /// <param name="strCHARTDATE"></param>
        /// <returns></returns>
        public static bool TotInOutItem(PsmhDb pDbCon, string strACPNO, string strPTNO, string strFORMNO, string strJOBGB, string strCHARTDATE)
        {
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");
            string strITEMCD = "I0000030622";

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT  ";
            SQL = SQL + ComNum.VBLF + "    A.ITEMCD";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A ";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + strACPNO;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + strCHARTDATE + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.JOBGB = '" + strJOBGB + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD = '" + strITEMCD + "' ";
            SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + strFORMNO;
            SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }

            if (dt1.Rows.Count == 0)
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                SQL = SQL + ComNum.VBLF + "( ";
                SQL = SQL + ComNum.VBLF + "    FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD,  ";
                SQL = SQL + ComNum.VBLF + "    WRITEDATE, WRITETIME, WRITEUSEID ";
                SQL = SQL + ComNum.VBLF + ") ";
                SQL = SQL + ComNum.VBLF + "VALUES (";
                SQL = SQL + ComNum.VBLF + "    " + strFORMNO + ", "; //FORMNO
                SQL = SQL + ComNum.VBLF + "    " + strACPNO + ", "; //ACPNO
                SQL = SQL + ComNum.VBLF + "    '" + strPTNO + "', "; //PTNO
                SQL = SQL + ComNum.VBLF + "    '" + strCHARTDATE + "', "; //CHARTDATE
                SQL = SQL + ComNum.VBLF + "    '" + strJOBGB + "', "; //JOBGB
                SQL = SQL + ComNum.VBLF + "    '" + strITEMCD + "', "; //ITEMCD
                SQL = SQL + ComNum.VBLF + "    '" + VB.Left(strCurDateTime, 8) + "', "; //WRITEDATE
                SQL = SQL + ComNum.VBLF + "    '" + VB.Right(strCurDateTime, 6) + "', "; //WRITETIME
                SQL = SQL + ComNum.VBLF + "    '" + clsType.User.IdNumber + "' "; //WRITEUSEID
                SQL = SQL + ComNum.VBLF + ") ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
            }
            dt1.Dispose();
            dt1 = null;

            strITEMCD = "I0000030623";

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT  ";
            SQL = SQL + ComNum.VBLF + "    A.ITEMCD";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A ";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + strACPNO;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + strCHARTDATE + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.JOBGB = '" + strJOBGB + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD = '" + strITEMCD + "' ";
            SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + strFORMNO;
            SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }

            if (dt1.Rows.Count == 0)
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                SQL = SQL + ComNum.VBLF + "( ";
                SQL = SQL + ComNum.VBLF + "    FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD,  ";
                SQL = SQL + ComNum.VBLF + "    WRITEDATE, WRITETIME, WRITEUSEID ";
                SQL = SQL + ComNum.VBLF + ") ";
                SQL = SQL + ComNum.VBLF + "VALUES (";
                SQL = SQL + ComNum.VBLF + "    " + strFORMNO + ", "; //FORMNO
                SQL = SQL + ComNum.VBLF + "    " + strACPNO + ", "; //ACPNO
                SQL = SQL + ComNum.VBLF + "    '" + strPTNO + "', "; //PTNO
                SQL = SQL + ComNum.VBLF + "    '" + strCHARTDATE + "', "; //CHARTDATE
                SQL = SQL + ComNum.VBLF + "    '" + strJOBGB + "', "; //JOBGB
                SQL = SQL + ComNum.VBLF + "    '" + strITEMCD + "', "; //ITEMCD
                SQL = SQL + ComNum.VBLF + "    '" + VB.Left(strCurDateTime, 8) + "', "; //WRITEDATE
                SQL = SQL + ComNum.VBLF + "    '" + VB.Right(strCurDateTime, 6) + "', "; //WRITETIME
                SQL = SQL + ComNum.VBLF + "    '" + clsType.User.IdNumber + "' "; //WRITEUSEID
                SQL = SQL + ComNum.VBLF + ") ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
            }
            dt1.Dispose();
            dt1 = null;

            return true;
        }

        /// <summary>
        /// 입력값이 숫자인지 문자인지 판단
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strBSNSCLS"></param>
        /// <param name="strUNITCLS"></param>
        /// <param name="strITEMCD"></param>
        /// <returns></returns>
        public static string InputVlaueType(PsmhDb pDbCon, string strBSNSCLS, string strUNITCLS, string strITEMCD)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "C"; //C: 문자, N:숫자

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    I.BASCD ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD I";
            SQL = SQL + ComNum.VBLF + "WHERE I.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "    AND I.UNITCLS = '섭취배설' ";
            SQL = SQL + ComNum.VBLF + "    AND I.VFLAG3 = '01.섭취' ";
            SQL = SQL + ComNum.VBLF + "    AND I.BASCD = '" + strITEMCD + "' ";
            SQL = SQL + ComNum.VBLF + "UNION ALL ";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    I.BASCD ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD I";
            SQL = SQL + ComNum.VBLF + "WHERE I.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "    AND I.UNITCLS = '섭취배설' ";
            SQL = SQL + ComNum.VBLF + "    AND I.VFLAG3 = '11.섭취' ";
            SQL = SQL + ComNum.VBLF + "    AND I.BASCD = '" + strITEMCD + "' ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = "N";
            }
            else
            {
                rtnVal = "C";
            }
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// 선차팅 못하도록 막는다
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="strTime"></param>
        /// <returns></returns>
        public static bool ChkActTime(string strDate, string strTime, string strCurDate, string strCurTime, int intVal)
        {
            bool rtnVal = true;
            if (strTime == "2400")
            {
                strTime = "2359";
            }
            if (strCurDate == "")
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                strCurDate = VB.Left(strCurDateTime, 8);
                strCurTime = VB.Mid(strCurDateTime, 9, 4);
            }

            DateTime dtpSys = Convert.ToDateTime(ComFunc.FormatStrToDate(strCurDate, "D") + " " + ComFunc.FormatStrToDate(strCurTime, "M"));
            DateTime dtpNow = DateTime.ParseExact(strDate + strTime, "yyyyMMddHHmm", null);

            intVal = intVal * -1;

            if ((dtpSys - dtpNow).TotalMinutes < intVal)
            {
                rtnVal = false;
            }
            return rtnVal;
        }







    }
}
