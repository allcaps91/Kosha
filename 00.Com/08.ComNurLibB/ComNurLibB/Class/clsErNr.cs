using ComBase;
using ComDbB;
using ComEmrBase;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace ComNurLibB
{
    public class clsErNr
    {
        public static string GstrHelpCode = "";
        public static string GstrHelpName = "";

        public static string GstrUMLS = "";
        public static string GstrUMLSName = "";
        public static string GstrJDate = "";
        public static string GstrInDate = "";
        public static string GstrOCSiLLCode = "";
        public static string GstrOCSiLLName = "";

        public static string[] GstrOCSGubun = new string[] { "주","부","의"};        
        public static string strSameName = "";
        //'==============================

        // 관찰구역
        public static string GstrArea1 = "'71','72','73','74','61','62','63','64','65','66','67'," +
                                         "'68','69','70','43','44','45','46','91','92','99','98','03'"; //2019-02-15 관찰구역 좌석 추가
        // 중증구역
        public static string GstrArea2 = "'81','82','83','84','85','86','75','76','77','78'";
        // 소아구역
        public static string GstrArea3 = "'52','53','54','55','56','57','A0','A1','A2','A3','A4','A5','A6','A7','A8'";
        // 격리구역
        public static string GstrArea4 = "'47','48','49','58','59'";


        public static void ComboDept_SET1(PsmhDb pDbCon, ComboBox ArgCombo)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {                
                //'진료과를 READ
                SQL = "SELECT DeptCode FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ('TO','HR','R7','II') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY DECODE(DEPTCODE, 'ER',1,'MD',2,'MG',3,'MC',4,'MP',5, ";
                SQL = SQL + ComNum.VBLF + "     'ME',6,'MN',7,'MR','8','MI','8','OS',9,'GS',10,";
                SQL = SQL + ComNum.VBLF + "     'NS',11,'OG',12,'NE',13,'CS',14,'EN',15,'UR',16,'NP',17,'RM',18)";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                ArgCombo.Items.Clear();
                ArgCombo.Items.Add("전체");
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ArgCombo.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                    }
                }
                dt.Dispose();
                dt = null;
               
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
        public static void ComboDept_SET2(PsmhDb pDbCon, ComboBox ArgCombo)
        {
            //2019-01-09
            //응급실환자관리에서 사용 
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                //'진료과를 READ
                SQL = "SELECT DeptCode FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ('TO','HR','R7','II') ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(DEPTCODE, 'ER', 1, 'MG', 2, 'MC', 3, 'MP', 4, 'MN', 5, 'ME', 6, ";
                SQL = SQL + ComNum.VBLF + " 'MI', 7, 'ME', 8, 'MR', 9, 'OS', 10, 'NS', 11, 'GS', 12, ";
                SQL = SQL + ComNum.VBLF + " 'PD', 13, 'NE', 14, 'OG', 15, 'CS', 16, 'EN', 17, 'UR', 18, ";
                SQL = SQL + ComNum.VBLF + " 'RD', 19, 'DT', 20, 'NP', 21, 'MO', 22, 'MD', 23, 'OT', 24, ";
                SQL = SQL + ComNum.VBLF + " 'DM', 25, 'RM', 26, 'AN', 27, 'FM', 28, 29) ";

                //SQL = SQL + ComNum.VBLF + " ORDER BY DECODE(DEPTCODE, 'ER',1,'MC',2,'MP',3,'MG',4,'MN',5, ";
                //SQL = SQL + ComNum.VBLF + "     'ME',6,'MI',7,'MO','8','MR','9','MD',10,'GS',11,";
                //SQL = SQL + ComNum.VBLF + "     'NS',12,'OG',13,'PD',14,'CS',15,'OS',16,'NP',17,'NE',18,'OT',19,'EN',20, ";
                //SQL = SQL + ComNum.VBLF + "     'UR',21,'DM',22,'RM',23,'DT',24,'RD',25,'AN',26,'FM',27  )";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                ArgCombo.Items.Clear();
                ArgCombo.Items.Add("전체");
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ArgCombo.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        public static string[] GetDept_ARRAY(PsmhDb pDbCon)
        {
            string[] rtnVal = null; 
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {                
                //'진료과를 READ
                SQL = "SELECT DeptCode FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ('TO','HR','R7','II') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY DECODE(DEPTCODE, 'ER',1,'MD',2,'MG',3,'MC',4,'MP',5, ";
                SQL = SQL + ComNum.VBLF + "     'ME',6,'MN',7,'MR','8','MI','8','OS',9,'GS',10,";
                SQL = SQL + ComNum.VBLF + "     'NS',11,'OG',12,'NE',13,'CS',14,'EN',15,'UR',16,'NP',17,'RM',18)";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                
                if (dt.Rows.Count > 0)
                {
                    rtnVal = new string[dt.Rows.Count];
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        rtnVal[i] = dt.Rows[i]["DeptCode"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        public static string[] GetDept_ARRAY2(PsmhDb pDbCon)
        {
            string[] rtnVal = null;
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                //'진료과를 READ
                SQL = "SELECT DeptCode FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ('TO','HR','R7','II') ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(DEPTCODE, 'ER', 1, 'MG', 2, 'MC', 3, 'MP', 4, 'MN', 5, 'ME', 6, ";
                SQL = SQL + ComNum.VBLF + " 'MI', 7, 'ME', 8, 'MR', 9, 'OS', 10, 'NS', 11, 'GS', 12, ";
                SQL = SQL + ComNum.VBLF + " 'PD', 13, 'NE', 14, 'OG', 15, 'CS', 16, 'EN', 17, 'UR', 18, ";
                SQL = SQL + ComNum.VBLF + " 'RD', 19, 'DT', 20, 'NP', 21, 'MO', 22, 'MD', 23, 'OT', 24, ";
                SQL = SQL + ComNum.VBLF + " 'DM', 25, 'RM', 26, 'AN', 27, 'FM', 28, 29) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = new string[dt.Rows.Count];
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        rtnVal[i] = dt.Rows[i]["DeptCode"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }
        /// <summary>
        /// Author : 유진호
        /// Create : 2018-04-12
        /// <seealso cref="FrmPatientEntry_New : READ_H1N1"/>
        /// </summary>
        /// <param name="strPano"></param>
        /// <param name="strJDate"></param>
        /// <returns></returns>
        public static string READ_H1N1(PsmhDb pDbCon, string strPano, string strJDate)
        {
            string rtnVal = "NO";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT PTMIINMN FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI ";
                SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + strJDate + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "YES";
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

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
        public static string READ_WARNING_BRADEN(PsmhDb pDbCon, string argPTNO, string ArgDate, string ArgIPDNO, string ArgAge, string argWARD, string ArgDate2 = "")
        {
            string rtnVal = "";
            //int iFB = 0;

            //string strFall = "";
            string strBraden = "";
            //string strOK = "";
            //string strAge = "";
            //string strWard = "";
            string strGubun = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

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

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if ((VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) >= 60 && VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 18)
                        || (VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) < 60 && VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 16))
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

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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
        /// Author : 유진호
        /// Create : 2018-04-12
        /// <seealso cref="FrmPatientEntry_New : Read_감염자격체크"/>
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtNo"></param>
        /// <param name="strBDate"></param>
        /// <param name="strRemark"></param>
        /// <returns></returns>
        public static string Read_INFECTION_NHIC_CHK_JIKA(PsmhDb pDbCon, string strPtNo, string strBDate, string strRemark)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT MESSAGE ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_NHIC";
                SQL = SQL + ComNum.VBLF + "   WHERE BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND PANO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "    AND JOB_STS ='2' ";
                SQL = SQL + ComNum.VBLF + "    AND (MESSAGE LIKE '%" + strRemark + "%' or MESSAGE LIKE '%지카%' )";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["MESSAGE"].ToString().Trim();
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
        /// Author : 유진호
        /// Create : 2018-04-12
        /// <seealso cref="FrmPatientEntry_New : READ_INFECTION"/>
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtNo"></param>
        /// <returns></returns>
        public static string READ_INFECTION(PsmhDb pDbCon, string strPtNo)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "";

            try
            {
                SQL = " SELECT CASE GUBUN ";
                SQL = SQL + ComNum.VBLF + " WHEN '01' THEN '혈액주의'";
                SQL = SQL + ComNum.VBLF + " WHEN '02' THEN '접촉주의'";
                SQL = SQL + ComNum.VBLF + " WHEN '03' THEN '공기주의'";
                SQL = SQL + ComNum.VBLF + " WHEN '04' THEN '비말주의'";
                SQL = SQL + ComNum.VBLF + " END GUBUN ";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_OCS.EXAM_INFECT_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE ODATE IS NULL";
                SQL = SQL + ComNum.VBLF + "      AND PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY GUBUN ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            rtnVal = dt.Rows[i]["GUBUN"].ToString().Trim();
                        }
                        else
                        {
                            rtnVal = rtnVal + ComNum.VBLF + dt.Rows[i]["GUBUN"].ToString().Trim();
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


        public static bool READ_NOTSEND(PsmhDb pDbCon, string strPano, string strInDate, string strInTime)
        {
            //int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            bool rtnVal = false;

            try
            {
                SQL = " SELECT PTMIIDNO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI_NOTSEND ";
                SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + strInDate.Replace("-", "") + "' ";
                SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strInTime.Replace(":", "") + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
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


        public static void Select_UMLS(PsmhDb pDbCon, string strCode, string strNo)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;            

            GstrUMLSName = "";

            try
            {
                SQL = " SELECT UMLSCODE,UMLSNO,UMLSTYPE,UMLSTERM,UMLSNAME ";
                SQL = SQL + ComNum.VBLF + " From KOSMOS_PMPA.NUR_ER_EMIHUMLS ";
                SQL = SQL + ComNum.VBLF + " WHERE UMLSCODE  = '" + strCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND UMLSNO = " + strNo + " ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    GstrUMLSName = VB.Left(dt.Rows[0]["UMLSNO"].ToString().Trim() + VB.Space(3) , 3) + dt.Rows[0]["UMLSTERM"].ToString().Trim();
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

        public static string READ_HOSPITAL(PsmhDb pDbCon, string strCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "";
            
            try
            {                
                SQL = " SELECT HOSPNAME FROM KOSMOS_PMPA.BAS_HOSPITAL ";
                SQL = SQL + ComNum.VBLF + " WHERE (HOSPCODE = '" + strCode + "' OR HOSPNUMB = '" + strCode + "') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["HOSPNAME"].ToString().Trim();
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
        /// 인사마스터 이름조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argSabun"></param>
        /// <returns></returns>
        public static string READ_INSA_NAME(PsmhDb pDbCon, string argSabun)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = SQL + " SELECT KORNAME FROM " + ComNum.DB_ERP + "INSA_MST  ";
                SQL = SQL + ComNum.VBLF + "  WHERE SABUN IN ('" + argSabun.PadLeft(5, '0') + "') ";
                SQL = SQL + ComNum.VBLF + "    AND ( TOIDAY IS NULL OR TOIDAY < TRUNC(SYSDATE) )";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["KorName"].ToString().Trim();

                }
                else
                {
                    rtnVal = "";
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
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        public static string READ_KJOB(string strJob)
        {
            string rtnVal = "";

            switch (strJob)
            {
                case "1":
                    rtnVal = "전문의";
                    break;
                case "2":
                    rtnVal = "전공의";
                    break;
                case "3":
                    rtnVal = "인턴";
                    break;
                case "4":
                    rtnVal = "일반의";
                    break;
                case "5":
                    rtnVal = "간호사";
                    break;
                case "6":
                    rtnVal = "1급 응급구조사";
                    break;
                case "8":
                    rtnVal = "기타";
                    break;
                case "9":
                    rtnVal = "미상";
                    break;
            }

            return rtnVal;
        }

        public static string READ_KTAS(PsmhDb pDbCon, string strPTMIIDNO, string strPTMIINDT, string strPTMIINTM)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                SQL = " SELECT PTMIKTID, PTMIKPR, PTMIKTS, PTMIKTDT, PTMIKTTM, ";
                SQL = SQL + ComNum.VBLF + "        PTMIKJOB, PTMIKIDN, TO_CHAR(WRITEDATE,'YYYY-MM-DD HH24:MI') WRITEDATE, KORNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_KTAS A, KOSMOS_ADM.INSA_MST C";
                SQL = SQL + ComNum.VBLF + " WHERE A.PTMIIDNO = '" + strPTMIIDNO + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.PTMIINDT = '" + strPTMIINDT + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.PTMIINTM = '" + strPTMIINTM + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.PTMIKTS = ( SELECT  MIN(B.PTMIKTS) FROM  KOSMOS_PMPA.NUR_ER_KTAS B ";
                SQL = SQL + ComNum.VBLF + "                                     WHERE A.PTMIIDNO =B.PTMIIDNO ";
                SQL = SQL + ComNum.VBLF + "                                          AND   A.PTMIINDT =B.PTMIINDT";
                SQL = SQL + ComNum.VBLF + "                                          AND   A.PTMIINTM =B.PTMIINTM ) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["PTMIKPR"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        public static string READ_KTAS_NAME(PsmhDb pDbCon, string strKTASCD)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                SQL = " SELECT KTASCD, KTASLVLCD, KTASCD1, KTASCATEGORY FROM";
                SQL = SQL + ComNum.VBLF + "   (";
                SQL = SQL + ComNum.VBLF + "         SELECT";
                SQL = SQL + ComNum.VBLF + "                 T1.*,";
                SQL = SQL + ComNum.VBLF + "                 (SELECT ktasname FROM KOSMOS_PMPA.NUR_ER_ktascd";
                SQL = SQL + ComNum.VBLF + " WHERE ktascd1 = T1.ktascd1 AND lvl ='1')";
                SQL = SQL + ComNum.VBLF + " ktasAge,";
                SQL = SQL + ComNum.VBLF + "                 (SELECT ktasname FROM KOSMOS_PMPA.NUR_ER_ktascd";
                SQL = SQL + ComNum.VBLF + " WHERE ktascd1 = T1.ktascd1 AND ktascd2 =";
                SQL = SQL + ComNum.VBLF + " T1.ktascd2 AND lvl ='2') || ' > ' ||";
                SQL = SQL + ComNum.VBLF + "                 (SELECT ktasname FROM KOSMOS_PMPA.NUR_ER_ktascd";
                SQL = SQL + ComNum.VBLF + " WHERE ktascd1 = T1.ktascd1 AND ktascd2 =";
                SQL = SQL + ComNum.VBLF + " T1.ktascd2 AND ktascd3 = T1.ktascd3 AND lvl";
                SQL = SQL + ComNum.VBLF + " ='3') || ' > ' ||";
                SQL = SQL + ComNum.VBLF + "                 (SELECT ktasname FROM KOSMOS_PMPA.NUR_ER_ktascd";
                SQL = SQL + ComNum.VBLF + " WHERE ktascd1 = T1.ktascd1 AND ktascd2 =";
                SQL = SQL + ComNum.VBLF + " T1.ktascd2 AND ktascd3 = T1.ktascd3 AND";
                SQL = SQL + ComNum.VBLF + " ktascd4 = T1.ktascd4 AND lvl ='4')";
                SQL = SQL + ComNum.VBLF + "                 ktasCategory";
                SQL = SQL + ComNum.VBLF + "         FROM (";
                SQL = SQL + ComNum.VBLF + "                 SELECT";
                SQL = SQL + ComNum.VBLF + "                         ktascd, ktascd1, ktascd2,";
                SQL = SQL + ComNum.VBLF + "                         ktascd3, ktascd4, ktaslvlcd,";
                SQL = SQL + ComNum.VBLF + "                         ktasname";
                SQL = SQL + ComNum.VBLF + "                 From KOSMOS_PMPA.NUR_ER_ktascd";
                SQL = SQL + ComNum.VBLF + "                WHERE lvl='4'";
                SQL = SQL + ComNum.VBLF + "                AND ktascd1 = '" + VB.Left(strKTASCD, 1) + "'";
                SQL = SQL + ComNum.VBLF + "                order by ktascd, ktaslvlcd";
                SQL = SQL + ComNum.VBLF + "                 ) T1";
                SQL = SQL + ComNum.VBLF + "         ) T2";
                SQL = SQL + ComNum.VBLF + " WHERE KTASCD = '" + VB.Left(strKTASCD, 5) + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY ktascd";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["KTASCATEGORY"].ToString().Trim();

                    if (VB.Left(strKTASCD, 1) == "A")
                    {
                        rtnVal += "(15세이상,";
                    }
                    else
                    {
                        rtnVal += "(15세미만,";
                    }

                    if (VB.Right(strKTASCD, 1) == "0")
                    {
                        rtnVal += "비감염)";
                    }
                    else if (VB.Right(strKTASCD, 1) == "1")
                    {
                        rtnVal += "비말/공기감염)";
                    }
                    else if (VB.Right(strKTASCD, 1) == "2")
                    {
                        rtnVal += "접촉감염)";
                    }
                    else if (VB.Right(strKTASCD, 1) == "3")
                    {
                        rtnVal += "특수상황)";
                    }
                    else if (VB.Right(strKTASCD, 1) == "9")
                    {
                        rtnVal += "미상)";
                    }
                }               
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {                
                dt.Dispose();
                dt = null;                
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }



        public static string READ_KTAS_JOBNAME(PsmhDb pDbCon, string strPTMIIDNO, string strPTMIINDT, string strPTMIINTM)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                SQL = " SELECT PTMIKTID, PTMIKPR, PTMIKTS, PTMIKTDT, PTMIKTTM, ";
                SQL = SQL + ComNum.VBLF + "        PTMIKJOB, PTMIKIDN, TO_CHAR(WRITEDATE,'YYYY-MM-DD HH24:MI') WRITEDATE, KORNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_KTAS A, KOSMOS_ADM.INSA_MST C";
                SQL = SQL + ComNum.VBLF + " WHERE A.PTMIIDNO = '" + strPTMIIDNO + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.PTMIINDT = '" + strPTMIINDT + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.PTMIINTM = '" + strPTMIINTM + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.WRITESABUN = C.SABUN ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTMIKTS = ( SELECT  MIN(B.PTMIKTS) FROM  KOSMOS_PMPA.NUR_ER_KTAS B ";
                SQL = SQL + ComNum.VBLF + "                                     WHERE A.PTMIIDNO =B.PTMIIDNO ";
                SQL = SQL + ComNum.VBLF + "                                          AND   A.PTMIINDT =B.PTMIINDT";
                SQL = SQL + ComNum.VBLF + "                                          AND   A.PTMIINTM =B.PTMIINTM ) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["KORNAME"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// READ_REPRINT_MCCERTIFI
        /// </summary>
        /// <seealso cref="modMyPublic.bas : READ_재발급내역"/>
        /// <returns></returns>        
        public static string READ_REPRINT_MCCERTIFI(PsmhDb pDbCon, string strPTNO, string strINDATE, string strDEPTCODE, string strGBN = "")
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string str = "";
            string str2 = "";
            string rtnVal = "";

            try
            {                
                SQL = " SELECT SEQDATE, MCCLASS, SINNAME, SINSAYU, BIGO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_MCCERTIFI_WONMU_REPRINT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strINDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDEPTCODE + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (dt.Rows.Count > 0)
                {

                    switch (dt.Rows[0]["MCCLASS"].ToString().Trim())
                    {
                        case "00":
                            str2 = "진료사실증명서";
                            break;
                        case "01":
                            str2 = "진단서";
                            break;
                        case "08":
                            str2 = "소견서";
                            break;
                        case "18":
                            str2 = "진료의뢰서";
                            break;
                        case "26":
                            str2 = "의료급여의뢰서";
                            break;
                        case "27":
                            str2 = "응급환자진료의뢰서";
                            break;
                        case "02":
                            str2 = "상해진단서";
                            break;
                        case "03":
                            str2 = "병사용진단서";
                            break;
                        case "05":
                            str2 = "사망진단서";
                            break;
                        case "19":
                            str2 = "장애인증명서";
                            break;
                        case "20":
                            str2 = "장애진단서";
                            break;
                    }

                    if (strGBN == "1")
                    {
                        str = "서류재발급 : " + str2 + "(" + dt.Rows[0]["SEQDATE"].ToString().Trim() + ")";
                        rtnVal = str;
                    }
                    else
                    {
                        str = "★신청자서류 : " + str2 + "(" + dt.Rows[0]["SEQDATE"].ToString().Trim() + ")" + ComNum.VBLF;
                        str = str + "★신청자명 : " + dt.Rows[0]["SINNAME"].ToString().Trim() + ComNum.VBLF;
                        str = str + "★신청사유  " + ComNum.VBLF + " => " + dt.Rows[0]["SINSAYU"].ToString().Trim();
                        if (dt.Rows[0]["BIGO"].ToString().Trim() != "")
                        {
                            str = str + ComNum.VBLF + " ★비고  " + ComNum.VBLF + " => " + dt.Rows[0]["BIGO"].ToString().Trim();
                        }

                        if (str != "")
                        {
                            ComFunc.MsgBox(str,"재발급 상세내역");
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// EMR 생성 : 간호기록지 SAVE_2049_퇴원
        /// </summary>
        /// <param name="strPTNO"></param>
        /// <param name="strInDate"></param>
        /// <param name="strDrCode"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strCHARTTIME"></param>
        /// <param name="strData"></param>
        /// <param name="strUSEID"></param>
        /// <returns></returns>
        public static bool SAVE_2049_DisChargeXML(string strPtno, string strInDate, string strDrCode, string strChartDate, string strChartTime, string strData, string strUseId)
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            
            double dblEmrNo = 0;
            string strHead = "";
            string strChartX1 = "";
            string strChartX2 = "";
            string strXML = "";
            string strXMLCert = "";
            string strTagHead = "";
            string strTagTail = "";
            string strTagVal = "";
            
            strXML = "";
            strHead = "<?xml version=" + VB.Chr(34) + "1.0" + VB.Chr(34) + " encoding=" + VB.Chr(34) + "UTF-8" + VB.Chr(34) + "?>";
            strChartX1 = "<chart>";
            strChartX2 = "</chart>";
            strXML = strHead + strChartX1;
            
            //'동반자/정보제공자
            strTagHead = @"<ta2 type=""textArea"" label=""Progress""><![CDATA[";
            strTagVal = "퇴실함";
            strTagTail = "]]></ta2>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;
            strXML = strXML + strChartX2;
            strXMLCert = strXML;
            
            try
            {                
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT KOSMOS_EMR.GetEmrXmlNo() FunSeqNo FROM Dual";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }
                if (dt.Rows.Count > 0)
                {
                    dblEmrNo = VB.Val(dt.Rows[0]["FunSeqNo"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;



                int Result = 0;
                OracleCommand cmd = new OracleCommand();
                PsmhDb pDbCon = clsDB.DbCon;

                cmd.Connection = pDbCon.Con;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandText = "KOSMOS_EMR.XMLINSRT3";
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    cmd.Parameters.Add("p_EMRNO", OracleDbType.Double, 0, dblEmrNo, ParameterDirection.Input);
                    cmd.Parameters.Add("p_FORMNO", OracleDbType.Double, 0, VB.Val("2049"), ParameterDirection.Input);
                    cmd.Parameters.Add("p_USEID", OracleDbType.Varchar2, 8, clsType.User.Sabun, ParameterDirection.Input);
                    cmd.Parameters.Add("p_CHARTDATE", OracleDbType.Varchar2, 8, Convert.ToDateTime(strChartDate).ToString("yyyyMMdd"), ParameterDirection.Input);
                    cmd.Parameters.Add("p_CHARTTIME", OracleDbType.Varchar2, 6, Convert.ToDateTime(strChartTime).ToString("HHmmss"), ParameterDirection.Input);
                    cmd.Parameters.Add("p_ACPNO", OracleDbType.Double, 0, 0, ParameterDirection.Input);
                    cmd.Parameters.Add("p_PTNO", OracleDbType.Varchar2, 9, strPtno, ParameterDirection.Input);
                    cmd.Parameters.Add("p_INOUTCLS", OracleDbType.Varchar2, 1, "O", ParameterDirection.Input);
                    cmd.Parameters.Add("p_MEDFRDATE", OracleDbType.Varchar2, 8, Convert.ToDateTime(strInDate).ToString("yyyyMMdd"), ParameterDirection.Input);
                    cmd.Parameters.Add("p_MEDFRTIME", OracleDbType.Varchar2, 6, "120000", ParameterDirection.Input);
                    cmd.Parameters.Add("p_MEDENDDATE", OracleDbType.Varchar2, 8, "", ParameterDirection.Input);
                    cmd.Parameters.Add("p_MEDENDTIME", OracleDbType.Varchar2, 6, "", ParameterDirection.Input);
                    cmd.Parameters.Add("p_MEDDEPTCD", OracleDbType.Varchar2, 4, "ER", ParameterDirection.Input);
                    cmd.Parameters.Add("p_MEDDRCD", OracleDbType.Varchar2, 6, strDrCode, ParameterDirection.Input);
                    cmd.Parameters.Add("p_MIBICHECK", OracleDbType.Varchar2, 1, "0", ParameterDirection.Input);
                    cmd.Parameters.Add("p_WRITEDATE", OracleDbType.Varchar2, 8, Convert.ToDateTime(strChartDate).ToString("yyyyMMdd"), ParameterDirection.Input);
                    cmd.Parameters.Add("p_WRITETIME", OracleDbType.Varchar2, 6, Convert.ToDateTime(strChartTime).ToString("HHmmss"), ParameterDirection.Input);
                    cmd.Parameters.Add("p_UPDATENO", OracleDbType.Int32, 0, 1, ParameterDirection.Input);
                    cmd.Parameters.Add("p_CHARTXML", OracleDbType.Clob, VB.Len(strXML), strXML, ParameterDirection.Input);

                    Result = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    ComFunc.MsgBox(e.Message + ComNum.VBLF + "경과기록지 생성 중 에러가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
                                
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {                
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        /// <summary>
        /// EMR 생성 : 간호기록지 SAVE_2049_퇴원
        /// </summary>
        /// <param name="strPTNO"></param>
        /// <param name="strInDate"></param>
        /// <param name="strDrCode"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strCHARTTIME"></param>
        /// <param name="strData"></param>
        /// <param name="strUSEID"></param>
        /// <returns></returns>
        public static bool SAVE_2049_DisChargeNEW(string strPtno, string strInDate, string strDrCode, string strChartDate, string strChartTime, string strData, string strUseId)
        {
            bool rtVal = false;

            EmrPatient pAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtno, "O", strInDate, "ER");
            EmrForm pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "2049");

            try
            {
                string strSAVEGB = "1";
                string strSAVECERT = "1";

                double dblEmrNo = clsEmrQuery.SaveNurseRecord(clsDB.DbCon, pAcp, pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(), "0", strChartDate.Replace("-", ""), strChartTime.Replace(":",""),
                                                       clsType.User.IdNumber, clsType.User.IdNumber, strSAVEGB, strSAVECERT, "0", "",
                                                       "", "", "퇴실함", "", "ER", "");
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        /// <summary>
        /// EMR 생성 : 간호기록지 SAVE_2049_신체검진
        /// </summary>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strDrCode"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strData"></param>
        /// <param name="strUseId"></param>
        /// <returns></returns>
        public static bool SAVE_2049_PhysicalExaminationXML(string strPtno, string strInDate, string strDrCode, string strChartDate, string strChartTime, string strData, string strUseId)
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            double dblEmrNo = 0;
            string strHead = "";
            string strChartX1 = "";
            string strChartX2 = "";
            string strXML = "";
            string strXMLCert = "";
            string strTagHead = "";
            string strTagTail = "";
            string strTagVal = "";        

            strXML = "";
            strHead = "<?xml version=" + VB.Chr(34) + "1.0" + VB.Chr(34) + " encoding=" + VB.Chr(34) + "UTF-8" + VB.Chr(34) + "?>";
            strChartX1 = "<chart>";
            strChartX2 = "</chart>";

            strXML = strHead + strChartX1;


            //'동반자/정보제공자
            strTagHead = @"<ta2 type=""textArea"" label=""Progress""><![CDATA[";
            strTagVal = "EM Staff " + strData + " 신체검진함";
            strTagTail = "]]></ta2>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;
            strXML = strXML + strChartX2;
            strXMLCert = strXML;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT KOSMOS_EMR.GetEmrXmlNo() FunSeqNo FROM Dual";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }
                if (dt.Rows.Count > 0)
                {
                    dblEmrNo = VB.Val(dt.Rows[0]["FunSeqNo"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;



                int Result = 0;
                OracleCommand cmd = new OracleCommand();
                PsmhDb pDbCon = clsDB.DbCon;

                cmd.Connection = pDbCon.Con;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandText = "KOSMOS_EMR.XMLINSRT3";
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    cmd.Parameters.Add("p_EMRNO", OracleDbType.Double, 0, dblEmrNo, ParameterDirection.Input);
                    cmd.Parameters.Add("p_FORMNO", OracleDbType.Double, 0, VB.Val("2049"), ParameterDirection.Input);
                    cmd.Parameters.Add("p_USEID", OracleDbType.Varchar2, 8, clsType.User.Sabun, ParameterDirection.Input);
                    cmd.Parameters.Add("p_CHARTDATE", OracleDbType.Varchar2, 8, Convert.ToDateTime(strChartDate).ToString("yyyyMMdd"), ParameterDirection.Input);
                    cmd.Parameters.Add("p_CHARTTIME", OracleDbType.Varchar2, 6, Convert.ToDateTime(strChartTime).ToString("HHmmss"), ParameterDirection.Input);
                    cmd.Parameters.Add("p_ACPNO", OracleDbType.Double, 0, 0, ParameterDirection.Input);
                    cmd.Parameters.Add("p_PTNO", OracleDbType.Varchar2, 9, strPtno, ParameterDirection.Input);
                    cmd.Parameters.Add("p_INOUTCLS", OracleDbType.Varchar2, 1, "O", ParameterDirection.Input);
                    cmd.Parameters.Add("p_MEDFRDATE", OracleDbType.Varchar2, 8, Convert.ToDateTime(strInDate).ToString("yyyyMMdd"), ParameterDirection.Input);
                    cmd.Parameters.Add("p_MEDFRTIME", OracleDbType.Varchar2, 6, "120000", ParameterDirection.Input);
                    cmd.Parameters.Add("p_MEDENDDATE", OracleDbType.Varchar2, 8, "", ParameterDirection.Input);
                    cmd.Parameters.Add("p_MEDENDTIME", OracleDbType.Varchar2, 6, "", ParameterDirection.Input);
                    cmd.Parameters.Add("p_MEDDEPTCD", OracleDbType.Varchar2, 4, "ER", ParameterDirection.Input);
                    cmd.Parameters.Add("p_MEDDRCD", OracleDbType.Varchar2, 6, strDrCode, ParameterDirection.Input);
                    cmd.Parameters.Add("p_MIBICHECK", OracleDbType.Varchar2, 1, "0", ParameterDirection.Input);
                    cmd.Parameters.Add("p_WRITEDATE", OracleDbType.Varchar2, 8, Convert.ToDateTime(strChartDate).ToString("yyyyMMdd"), ParameterDirection.Input);
                    cmd.Parameters.Add("p_WRITETIME", OracleDbType.Varchar2, 6, Convert.ToDateTime(strChartTime).ToString("HHmmss"), ParameterDirection.Input);
                    cmd.Parameters.Add("p_UPDATENO", OracleDbType.Int32, 0, 1, ParameterDirection.Input);
                    cmd.Parameters.Add("p_CHARTXML", OracleDbType.Clob, VB.Len(strXML), strXML, ParameterDirection.Input);

                    Result = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    ComFunc.MsgBox(e.Message + ComNum.VBLF + "경과기록지 생성 중 에러가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
                
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        /// <summary>
        /// EMR 생성 : 간호기록지 SAVE_2049_퇴원
        /// </summary>
        /// <param name="strPTNO"></param>
        /// <param name="strInDate"></param>
        /// <param name="strDrCode"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strCHARTTIME"></param>
        /// <param name="strData"></param>
        /// <param name="strUSEID"></param>
        /// <returns></returns>
        public static bool SAVE_2049_PhysicalExaminationNEW(string strPtno, string strInDate, string strDrCode, string strChartDate, string strChartTime, string strData, string strUseId)
        {
            bool rtVal = false;

            EmrPatient pAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtno, "O", strInDate, "ER");
            EmrForm pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "2049");

            try
            {
                string strSAVEGB = "1";
                string strSAVECERT = "1";

                double dblEmrNo = clsEmrQuery.SaveNurseRecord(clsDB.DbCon, pAcp, pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(), "0", strChartDate.Replace("-", ""), strChartTime.Replace(":", ""),
                                                       clsType.User.IdNumber, clsType.User.IdNumber, strSAVEGB, strSAVECERT, "0", "",
                                                       "", "", "EM Staff " + strData + " 신체검진함", "", "ER", "");
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }
       
        public static string READ_MAX_KTAS(string strIDNO, string strINDT, string strINTM)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            strINDT = VB.Replace(strINDT, "-", "");
            strINTM = VB.Replace(strINTM, ":", "");

            if (strINDT == "" || strINTM == "") return rtnVal;

            try
            {                                
                SQL = " SELECT MIN(PTMIKTS) KTASLEVL";
                SQL = SQL + ComNum.VBLF + "  FROM (";
                SQL = SQL + ComNum.VBLF + "      SELECT PTMIKTS";
                SQL = SQL + ComNum.VBLF + "        FROM KOSMOS_PMPA.NUR_ER_KTAS";
                SQL = SQL + ComNum.VBLF + "       WHERE PTMIIDNO = '" + strIDNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND PTMIINDT = '" + strINDT + "' ";
                SQL = SQL + ComNum.VBLF + "         AND PTMIINTM = '" + strINTM + "' ";
                SQL = SQL + ComNum.VBLF + "         AND SEQNO = 1";
                SQL = SQL + ComNum.VBLF + "  UNION ALL";
                SQL = SQL + ComNum.VBLF + "      SELECT PTMIKTS";
                SQL = SQL + ComNum.VBLF + "        FROM KOSMOS_PMPA.NUR_ER_KTAS";
                SQL = SQL + ComNum.VBLF + "       WHERE PTMIIDNO = '" + strIDNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND PTMIINDT = '" + strINDT + "' ";
                SQL = SQL + ComNum.VBLF + "         AND PTMIINTM = '" + strINTM + "' ";
                SQL = SQL + ComNum.VBLF + "         AND SEQNO > 1)";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["KTASLEVL"].ToString().Trim();                    
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        public static string READ_KTAS_SEND(string strIDNO, string strINDT, string strINTM)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strKTID = "";
            string strKPR1 = "";
            string strKTS1 = "";
            string strKTDT = "";
            string strKTTM = "";
            string strKJOB = "";
            string strKIDN = "";
            string strKPR2 = "";
            string strKTS2 = "";
            string strWRITEDATE = "";

            try
            {                
                SQL = " SELECT PTMIKTID, PTMIKPR, PTMIKTS, PTMIKTDT, PTMIKTTM, ";
                SQL = SQL + ComNum.VBLF + "        PTMIKJOB, PTMIKIDN, TO_CHAR(WRITEDATE,'YYYY-MM-DD HH24:MI') WRITEDATE ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_KTAS";
                SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + strIDNO + "'";
                SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + strINDT + "'";
                SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strINTM + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY WRITEDATE ASC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strKTID = dt.Rows[0]["PTMIKTID"].ToString().Trim();
                    strKPR1 = dt.Rows[0]["PTMIKPR"].ToString().Trim();
                    strKTS1 = dt.Rows[0]["PTMIKTS"].ToString().Trim();
                    strKTDT = dt.Rows[0]["PTMIKTDT"].ToString().Trim();
                    strKTTM = dt.Rows[0]["PTMIKTTM"].ToString().Trim();
                    strKJOB = dt.Rows[0]["PTMIKJOB"].ToString().Trim();
                    strKIDN = dt.Rows[0]["PTMIKIDN"].ToString().Trim();
                    strWRITEDATE = dt.Rows[0]["WRITEDATE"].ToString().Trim();
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    rtnVal = "";
                    return rtnVal;
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }


            try
            {
                SQL = " SELECT PTMIKPR, PTMIKTS ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_KTAS";
                SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + strIDNO + "'";
                SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + strINDT + "'";
                SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strINTM + "'";
                SQL = SQL + ComNum.VBLF + "   AND SEQNO > 1";
                SQL = SQL + ComNum.VBLF + " ORDER BY PTMIKTS ASC, WRITEDATE DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strKPR2 = dt.Rows[0]["PTMIKPR"].ToString().Trim();
                    strKTS2 = dt.Rows[0]["PTMIKTS"].ToString().Trim();                    
                }                
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            rtnVal = strKTID + "|" + strKPR1 + "|" + strKTS1 + "|" + strKTDT + "|" + strKTTM + "|" + 
                     strKJOB + "|" + strKIDN + "|" + strKPR2 + "|" + strKTS2;
            return rtnVal;
        }
        
        public static string CONV_DEPTCODE(string arg)
        {
            string rtnVal = "";
            switch (arg)
            {
                case "MI":
                    rtnVal = "AF";
                    break;
                case "MO":
                    rtnVal = "AG";
                    break;
                case "MG":
                    rtnVal = "AC";
                    break;
                case "MC":
                    rtnVal = "AA";
                    break;
                case "MP":
                    rtnVal = "AB";
                    break;
                case "ME":
                    rtnVal = "AE";
                    break;
                case "MN":
                    rtnVal = "AD";
                    break;
                case "MR":
                    rtnVal = "AI";
                    break;
                case "GS":
                    rtnVal = "BA";
                    break;
                case "NS":
                    rtnVal = "BB";
                    break;
                case "OS":
                    rtnVal = "BD";
                    break;
                case "OG":
                    rtnVal = "CA";
                    break;
                case "CS":
                    rtnVal = "BC";
                    break;
                case "AN":
                    rtnVal = "PA";
                    break;
                case "PD":
                    rtnVal = "DA";
                    break;
                case "NP":
                    rtnVal = "EA";
                    break;
                case "OT":
                    rtnVal = "GA";
                    break;
                case "EN":
                    rtnVal = "HA";
                    break;
                case "UR":
                    rtnVal = "IA";
                    break;
                case "ER":
                    rtnVal = "JA";
                    break;
                case "DM":
                    rtnVal = "LA";
                    break;
                case "RM":
                    rtnVal = "MA";
                    break;
                case "DT":
                    rtnVal = "NA";
                    break;
                case "NE":
                    rtnVal = "FA";
                    break;
                case "RD":
                    rtnVal = "OA";
                    break;
            }

            return rtnVal;            
        }


        /// <summary>
        /// 동명이인 체크
        /// </summary>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public static void getSameNameJupSu(PsmhDb pDbCon, string strFDate, string strTDate)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            strSameName = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SNAME, COUNT(SName)CNT ";
                SQL = SQL + ComNum.VBLF + "       FROM NUR_ER_PATIENT A, KOSMOS_PMPA.BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "      WHERE A.PANO = B.PANO ";
                SQL = SQL + ComNum.VBLF + "        AND A.JDate >= TO_DATE('"+ strFDate + "', 'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "        AND A.JDate <= TO_DATE('"+ strTDate + "', 'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY SNAME ";
                SQL = SQL + ComNum.VBLF + " HAVING COUNT(SName) > 1 ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strSameName += dt.Rows[i]["SName"].ToString().Trim() + ",";
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return;
            }            
        }

        // 진료구역
        public static string READ_ER_AREA(string strCode)
        {
            string strValue = "";

            if (strCode == "") return strValue;

            if (strCode == "대기" || strCode == "퇴실" || strCode == "없음" || strCode == "취소"
                || strCode == "1E2" || strCode == "3E4" || strCode == "5E6" || strCode == "7E8" )
            {
                strValue = "관찰";
            }
            else
            {
                switch (VB.Left(strCode, 1))
                {
                    case "1":
                        strValue = "관찰";
                        break;
                    case "2":
                        strValue = "중증";
                        break;
                    case "3":
                        strValue = "소아";
                        break;
                    case "4":
                        strValue = "격리";
                        break;
                }
            }

            return strValue;
        }


        public static bool WRITE_LOG_KTAS(string strPano)
        {
            bool rtVal = false;            
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " INSERT INTO KOSMOS_PMPA.BACK_ERLOG( ";
                SQL = SQL + ComNum.VBLF + " JOBATE, PANO, GUBUN, IPSABUN, IPADDR ";
                SQL = SQL + ComNum.VBLF + " ) VALUES ( ";
                SQL = SQL + ComNum.VBLF + "SYSDATE,'" + strPano + "','KTAS','" + clsType.User.Sabun + "', '" + clsCompuInfo.gstrCOMIP + "') ";
                
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);                
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        public static string READ_NEDIS_IPWON_GUBUN_OP(string argPANO, string argInDate1, string argInDate2)
        {
            //2019-01-21
            //NEDIS 입원구분 가져오기(수술실용)

            string rtnVal = "";
            string strEmrt = "";
            string strHsrt = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = " SELECT PTMIEMRT, PTMIHSRT ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.VIEW_ER_EMIHPTMI ";
                SQL += ComNum.VBLF + " WHERE PTMIIDNO = '" + argPANO + "'";
                SQL += ComNum.VBLF + "   AND PTMIINDT >= '" + argInDate1.Replace("-", "") + "' ";
                SQL += ComNum.VBLF + "   AND PTMIINDT <= '" + argInDate2.Replace("-", "") + "' ";
                SQL += ComNum.VBLF + " ORDER BY PTMIINDT DESC, PTMIINTM DESC ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strEmrt = dt.Rows[0]["PTMIEMRT"].ToString().Trim();
                    strHsrt = dt.Rows[0]["PTMIHSRT"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                switch (strEmrt)
                {
                    case "32":  //중환자실로 바로 입원
                        if (strHsrt == "21" || strHsrt == "25" || strHsrt == "26")    //EICU,격리 EICU, 음압 EICU
                        {
                            rtnVal = "EICU";
                        }
                        else
                        {
                            rtnVal = "ETC";
                        }
                        break;
                    case "33":  //수술(시술) 후 병실로 입원
                    case "34":  //수술(시술) 후 중환자실로 입원
                        rtnVal = "ER";
                        break;

                    case "31":  //병실로 입원
                    case "38":  //기타(기타 다른 곳으로 입원)
                        rtnVal = "ETC";
                        break;

                    default:
                        rtnVal = "ETC";
                        break;
                }

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
            }

            return rtnVal;

        }

        public static bool WRITE_LOG_NEDIS(string strPano)
        {
            bool rtVal = false;            
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " INSERT INTO KOSMOS_PMPA.BACK_ERLOG( ";
                SQL = SQL + ComNum.VBLF + " JOBATE, PANO, GUBUN, IPSABUN, IPADDR ";
                SQL = SQL + ComNum.VBLF + " ) VALUES ( ";
                SQL = SQL + ComNum.VBLF + "SYSDATE,'" + strPano + "','NEDIS','" + clsType.User.Sabun + "', '" + clsCompuInfo.gstrCOMIP + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);                
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }
    }
}
