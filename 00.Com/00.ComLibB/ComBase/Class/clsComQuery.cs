using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using ComDbB;

namespace ComBase
{
    /// <summary>
    /// 전체 공통 쿼리문
    /// </summary>
    public class ComQuery : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// PSMH MAIN 프로그램 환경 설정
        /// </summary>
        public static void SetPsmhEnv()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;

            try
            {
                clsType.SvrInfo.strClient = @"C:\PSMHEXE";

                string strAPLDATE = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                //자동로그아웃 시간//프로그램 이전 버전 읽기
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "   GRPCDB, GRPCD, BASCD, APLFRDATE, APLENDDATE, BASNAME, BASNAME1,  ";
                SQL = SQL + ComNum.VBLF + "   VFLAG1, VFLAG2, VFLAG3, VFLAG4, NFLAG1, NFLAG2, NFLAG3, NFLAG4, ";
                SQL = SQL + ComNum.VBLF + "   REMARK, REMARK1, INPDATE, INPTIME,  ";
                SQL = SQL + ComNum.VBLF + "   USECLS, DISPSEQ ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD ";
                SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = '프로그램기초'";
                SQL = SQL + ComNum.VBLF + "       AND GRPCD = '프로그램'";
                SQL = SQL + ComNum.VBLF + "       AND  '" + strAPLDATE + "' BETWEEN APLFRDATE AND APLENDDATE ";
                SQL = SQL + ComNum.VBLF + "       AND USECLS = '1'";
                SQL = SQL + ComNum.VBLF + "ORDER BY DISPSEQ ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["BASCD"].ToString().Trim() == "LOGOUTTIME")
                        {
                            clsPublic.AutoLogOutTime = (int)VB.Val(dt.Rows[i]["NFLAG1"].ToString().Trim());
                        }
                        if (dt.Rows[i]["BASCD"].ToString().Trim() == "프로그램버젼")
                        {
                            clsPublic.PSMHVERSION = (int)VB.Val(dt.Rows[i]["NFLAG1"].ToString().Trim());
                        }
                        if (dt.Rows[i]["BASCD"].ToString().Trim() == "ClientPath")
                        {
                            clsType.SvrInfo.strClient = dt.Rows[i]["VFLAG1"].ToString().Trim();
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                //컴퓨터 환경 읽기
                clsType.ClearCompEnv();

                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    B.GRPCD, B.BASCD, B.REMARK, B.REMARK1, B.VFLAG1, B.DISPSEQ   ";
                //SQL = SQL + ComNum.VBLF + "    , P.VALUEV, P.VALUEN, P.DELGB, P.ROWID";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_PMPA + "BAS_BASCD B ";
                //SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_PMPA + "BAS_PCCONFIG P ";
                //SQL = SQL + ComNum.VBLF + "     ON B.GRPCD = P.GUBUN  ";
                //SQL = SQL + ComNum.VBLF + "     AND B.BASCD = P.CODE  ";
                //SQL = SQL + ComNum.VBLF + "     AND P.IPADDRESS =  '" + clsCompuInfo.gstrCOMIP + "'";
                SQL = SQL + ComNum.VBLF + "WHERE B.GRPCDB = '프로그램PC세팅'  ";
                SQL = SQL + ComNum.VBLF + "  AND B.GRPCD = '자동로그아웃제외IP'  ";
                SQL = SQL + ComNum.VBLF + "  AND B.BASCD = '" + clsCompuInfo.gstrCOMIP + "'  ";
                SQL = SQL + ComNum.VBLF + "  AND B.APLFRDATE <= '" + strAPLDATE + "'";
                SQL = SQL + ComNum.VBLF + "  AND B.APLENDDATE >= '" + strAPLDATE + "'";
                SQL = SQL + ComNum.VBLF + "  AND B.USECLS = '1'";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                //dt = ComQuery.GetValue_BAS_PCCONFIG(clsDB.DbCon, "프로그램PC세팅", "기타PC설정", "자동로그아웃제외");
                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    clsType.CompEnv.NotAutoLogOut = true;
                }
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        /// <summary>
        /// 서버시간을 가지고 온다
        /// </summary>
        /// <param name="strFlag"></param>
        /// <returns></returns>
        public static string CurrentDateTime(PsmhDb pDbCon, string strFlag)
        {
            string SQL = "";
            string strValue = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + " SELECT TO_CHAR(SYSDATE,'YYYYMMDD') AS CURRENTDATE,      \r\n";
                SQL = SQL + "        TO_CHAR(SYSDATE,'HH24MISS') AS CURRENTTIME,      \r\n";
                SQL = SQL + "        SYSDATE                                          \r\n";
                SQL = SQL + "   FROM DUAL                                             \r\n";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strValue;
                }
                if(dt.Rows.Count == 0)
                {
                    return "";
                }
                else
                {
                    switch(strFlag)
                    {
                        case "A":
                            strValue = (dt.Rows[0]["CURRENTDATE"].ToString() + "").Trim() + (dt.Rows[0]["CURRENTTIME"].ToString() + "").Trim();
                            break;
                        case "D":
                            strValue = (dt.Rows[0]["CURRENTDATE"].ToString() + "").Trim();
                            break;
                        case "T":
                            strValue = (dt.Rows[0]["CURRENTTIME"].ToString() + "").Trim();
                            break;
                        case "S":
                            strValue = (dt.Rows[0]["SYSDATE"].ToString() + "").Trim();
                            break;
                    }
                    dt.Dispose();
                    dt = null;

                    return strValue;
                }
            }
            catch(Exception ex)
            {
                Application.ExitThread();
                Environment.Exit(0);
                //ComFunc.MsgBox(ex.Message);
                //clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strValue;
            }
        }

        public static string CurrentDateTimeClt(string strFlag)
        {
            string strValue = "";
            string strCURRENTDATETIME = VB.Now().ToString("yyyyMMddHHmmss");
            string strCURRENTDATE = VB.Left(strCURRENTDATETIME, 8);
            string strCURRENTTIME = VB.Right(strCURRENTDATETIME, 6);

            switch (strFlag)
            {
                case "A":
                    strValue = strCURRENTDATE + strCURRENTTIME;
                    break;
                case "D":
                    strValue = strCURRENTDATE;
                    break;
                case "T":
                    strValue = strCURRENTTIME;
                    break;
                case "S":
                    strValue = strCURRENTTIME;
                    break;
            }

            return strValue;
        }

        /// <summary>
        /// Sequence 가져오기 : Function 쿼리
        /// </summary>
        /// <param name="FunSeqName">USER.FUNCTION</param>
        /// <returns></returns>
        public static double GetSequencesNo(PsmhDb pDbCon, string strFunSeqName)
        {
            DataTable dt = null;
            string SQL = "";
            double rtnVal = 0;
            string SqlErr = ""; //에러문 받는 변수
            
            try
            {
                SQL = "";
                SQL = SQL + "SELECT " + strFunSeqName + "() SEQNO FROM DUAL";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return 0;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("시퀀스 조회중 문제가 발생했습니다");
                    return 0;
                }

                rtnVal = VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim());
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
        /// Sequence 가져오기 : Function 쿼리 : USER + Sequence로 
        /// </summary>
        /// <param name="strUser"></param>
        /// <param name="strSeqName"></param>
        /// <returns></returns>
        public static double GetSequencesNo(PsmhDb pDbCon, string strUser, string strSeqName)
        {
            DataTable dt = null;
            string SQL = "";
            double rtnVal = 0;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + "SELECT " + strUser + "." + strSeqName + ".NEXTVAL AS SEQNO FROM DUAL";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return 0;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("시퀀스 조회중 문제가 발생했습니다");
                    return 0;
                }

                rtnVal = VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim());
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
        /// Sequence 가져오기 : Sequences 쿼리
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strFunSeqName"></param>
        /// <returns></returns>
        public static double GetSequencesNoEx(PsmhDb pDbCon, string strFunSeqName)
        {
            DataTable dt = null;
            string SQL = "";
            double rtnVal = 0;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + "SELECT " + strFunSeqName + ".NextVal AS SEQNO FROM DUAL";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return 0;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("시퀀스 조회중 문제가 발생했습니다");
                    return 0;
                }

                rtnVal = VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim());
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
        /// 기초코드 Data 설정
        /// <param name="Gubun"></param>
        /// <param name="Code"></param>
        /// 2017-04-06 박병규
        /// </summary>
        public static DataTable Set_BaseCode_Foundation(PsmhDb pDbCon, string Gubun, string Code)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT CODE, NAME ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND GUBUN = '" + Gubun.Trim() + "' ";

                if (Code.Trim() != "")
                    SQL += ComNum.VBLF + " AND CODE = '" + Code.Trim() + "' ";

                SQL += ComNum.VBLF + "    AND DELDATE IS NULL  ";
                SQL += ComNum.VBLF + "  ORDER BY CODE ";
                SqlErr = clsDB.GetDataTableREx(ref DtQ, SQL, pDbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return DtQ;
                }

                return DtQ;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return DtQ;
            }
        }

        /// <summary>
        /// 기초코드 Data 설정(감액)
        /// <param name="Gubun">자격 또는 CASE</param>
        /// 2017-10-13 박병규
        /// </summary>
        public static DataTable Set_BaseCode_Foundation_Gamek(PsmhDb pDbCon, string Gubun)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT CODE, NAME ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND GUBUN = 'BAS_감액코드명' ";
                SQL += ComNum.VBLF + "    AND CODE NOT IN ('53','54') "; //협진,PET-CT는 진료구분에서 함.

                if (Gubun != "")
                    SQL += ComNum.VBLF + " AND GUBUN2 = '" + Gubun + "' ";

                SQL += ComNum.VBLF + "    AND DELDATE IS NULL  ";
                SQL += ComNum.VBLF + "  ORDER BY CODE ";
                SqlErr = clsDB.GetDataTableREx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return Dt;
                }

                return Dt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return Dt;
            }
        }


        /// <summary>
        /// 기초코드 Data - 지역코드 설정
        /// <param name="Gubun"></param>
        /// <param name="Code"></param>
        /// 2017-08-21 박병규
        /// </summary>
        /// <seealso cref="frmJupsuMain_New:Load_Combo_Jiyuk"/> 
        public static DataTable Set_BaseCode_Foundation_Jicode(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT * ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_AREA ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND JICODE > '0' ";
                SQL += ComNum.VBLF + "  ORDER BY JICODE ";
                SqlErr = clsDB.GetDataTableREx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return Dt;
                }

                return Dt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return Dt;
            }
        }


        /// <summary>
        /// 기초코드 Data ComboBox 설정
        /// <param name="ComboBox"></param>
        /// <param name="Gubun"></param>
        /// <param name="Code"></param>
        /// 2017-06-19 박병규
        /// </summary>
        public void Set_BaseCode_ComboBox(PsmhDb pDbCon, ComboBox o, string Gubun, string Code)
        {
            DataTable DtSet = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strCode = string.Empty;
            string strCodeName = string.Empty;

            o.Items.Clear();

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT CODE, NAME ";
                SQL += ComNum.VBLF + "   FROM BAS_BCODE ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND GUBUN = '" + Gubun.Trim() + "' ";

                if (Code.Trim() != "")
                {
                    SQL += ComNum.VBLF + " AND CODE = '" + Code.Trim() + "' ";
                }

                SQL += ComNum.VBLF + "    AND DELDATE IS NULL ";
                SQL += ComNum.VBLF + "  ORDER BY CODE ";
                SqlErr = clsDB.GetDataTableREx(ref DtSet, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    DtSet.Dispose();
                    DtSet = null;
                    return;
                }

                if (DtSet.Rows.Count > 0)
                {
                    for (int i = 0; i < DtSet.Rows.Count; i++)
                    {
                        strCode = DtSet.Rows[i]["CODE"].ToString().Trim();
                        strCodeName = DtSet.Rows[i]["NAME"].ToString().Trim();

                        o.Items.Add(strCode + "." + strCodeName);
                    }

                    o.SelectedIndex = 0;
                }

                DtSet.Dispose();
                DtSet = null;
                
                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                DtSet.Dispose();
                DtSet = null;
                return;
            }
        }

        /// <summary>
        /// 주민번호를 통해 등록번호를 가져오기
        /// <param name="strJumin1"></param>
        /// <param name="strJumin2"></param>
        /// 2017-06-15 박병규
        /// </summary>
        public string GetPtno(PsmhDb pDbCon, string strJumin1, string strJumin2)
        {
            DataTable DtPtno = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND JUMIN1 = '" + strJumin1 + "'";
            SQL += ComNum.VBLF + "    AND JUMIN3 = '" + strJumin2 + "'";
            SqlErr = clsDB.GetDataTable(ref DtPtno, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                DtPtno.Dispose();
                DtPtno = null;

                return rtnVal;
            }

            if (DtPtno.Rows.Count > 0)
            {
                rtnVal = DtPtno.Rows[0]["PANO"].ToString();
            }

            DtPtno.Dispose();
            DtPtno = null;

            return rtnVal;
        }


        /// <summary>
        /// 건물관리번호를 통해 주소를 가져오기
        /// <param name="ArgBuildNo"></param>
        /// 2017-06-15 박병규
        /// </summary>
        /// <seealso cref="vbFunction.bas:READ_ROAD_JUSO"/>
        public String Read_RoadJuso(PsmhDb pDbCon, string ArgBuildNo, string ArgZipCode = "")
        {
            DataTable DtJuso = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ZIPNAME1 || ' ' || ZIPNAME2 || ' ' || ZIPNAME3 AS HEADJUSO,";
            SQL += ComNum.VBLF + "        ROADNAME, BUILDNAME, BUN1, BUN2 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ZIPS_ROAD ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND BUILDNO = '" + ArgBuildNo + "' ";

            if (ArgZipCode != "")
                SQL += ComNum.VBLF + "    AND ZIPCODE = '" + ArgZipCode + "' ";

            SqlErr = clsDB.GetDataTableREx(ref DtJuso, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                DtJuso.Dispose();
                DtJuso = null;

                return rtnVal;
            }

            if (DtJuso.Rows.Count > 0)
            {
                rtnVal = DtJuso.Rows[0]["HEADJUSO"].ToString().Trim() + " ";
                rtnVal += DtJuso.Rows[0]["ROADNAME"].ToString().Trim() + " ";
                rtnVal += DtJuso.Rows[0]["BUN1"].ToString().Trim() + " ";

                if (VB.Val(DtJuso.Rows[0]["BUN2"].ToString().Trim()) > 0)
                {
                    rtnVal += "-" + DtJuso.Rows[0]["BUN2"].ToString().Trim() + " ";
                }

                if (DtJuso.Rows[0]["BUILDNAME"].ToString().Trim() != "")
                {
                    rtnVal += DtJuso.Rows[0]["BUILDNAME"].ToString().Trim();
                }
            }

            DtJuso.Dispose();
            DtJuso = null;

            return rtnVal;
        }

        /// <summary>
        /// 우편번호를 통해 주소를 가져오기
        /// 건물관리번호가 없을경우 사용. 6자리 우편번호이용
        /// <param name="ArgCode"></param>
        /// 2017-06-15 박병규
        /// </summary>
        /// <seealso cref="vbFunction.bas:READ_JUSO"/>

        public String Read_Juso(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtJuso = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT MAILJUSO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MAILNEW ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN = '2' ";
            SQL += ComNum.VBLF + "    AND MAILCODE = '" + ArgCode + "' ";
            SqlErr = clsDB.GetDataTableREx(ref DtJuso, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                DtJuso.Dispose();
                DtJuso = null;

                return rtnVal;
            }

            if (DtJuso.Rows.Count > 0)
            {
                rtnVal = DtJuso.Rows[0]["MAILJUSO"].ToString().Trim() + " ";
            }

            DtJuso.Dispose();
            DtJuso = null;

            return rtnVal;
        }

        /// <summary>
        /// 폼 사용 권한이 있는지 확인
        /// 조회,저장,삭제,수정,출력 권한이 하나도 없으면 폼을 로드하지 않는다.
        /// </summary>
        /// <param name="pForm">폼</param>
        /// **주의사항 : 부모폼에서 생성을 할 경우(이벤트 처리 등) 자식폼에서 this.Close() 사용금지
        ///              => 부모폼에서 폼 띄우기전에 미리 권한을 읽어서 처리하도록 한다.
        public static bool isFormAuth(PsmhDb pDbCon, Form pForm)
        {
            bool rntVal = false;
            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            try
            {
                
                SQL = "";
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    J.AUTHC, J.AUTHR, J.AUTHU, J.AUTHD, J.AUTHP  ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_USER U ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_PROJECTFORMGROUP J ";
                SQL = SQL + ComNum.VBLF + "    ON U.JOBGROUP = J.JOBGROUP ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_PROJECTFORM F ";
                SQL = SQL + ComNum.VBLF + "    ON J.FORMCD = F.FORMCD ";
                SQL = SQL + ComNum.VBLF + "    AND F.FORMNAME = '" + pForm.Name + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE U.IDNUMBER = '" + clsType.User.IdNumber + "' ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    return rntVal;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    rntVal = true;
                    return rntVal;
                }
                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    J.AUTHC, J.AUTHR, J.AUTHU, J.AUTHD, J.AUTHP  ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_PROJECTFORMGROUP J ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_PROJECTFORM F ";
                SQL = SQL + ComNum.VBLF + "    ON J.FORMCD = F.FORMCD ";
                SQL = SQL + ComNum.VBLF + "    AND F.FORMNAME = '" + pForm.Name + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE J.JOBGROUP = '" + ComNum.DEFAULT_JOBGROUP + "' ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    return rntVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rntVal;
                }
                dt.Dispose();
                dt = null;

                rntVal = true;
                return rntVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return rntVal;
            }
        }

        /// <summary>
        /// 폼 접속 로그를 저장한다.
        /// </summary>
        /// <param name="pForm"></param>
        public static void SaveFormLog(PsmhDb pDbCon, Form pForm)
        {
            //Save Form Log
        }

        /// <summary>
        /// 폼로드시에 권한별 버튼 사용유무 세팅(Enabled = false)
        /// </summary>
        /// <param name="objParent">폼</param>
        /// <param name="pOption">N: 적용안함</param>
        public static void SetFormJobAuth(PsmhDb pDbCon, Control objParent, string pOption = "Y")
        {
            if (pOption == "N") return;

            bool blnC = false;
            bool blnR = false;
            bool blnU = false;
            bool blnD = false;
            bool blnP = false;

            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            
            try
            {
                //SQL = "";
                //SQL = " SELECT  ";
                //SQL = SQL + ComNum.VBLF + "    AUTHC, AUTHR, AUTHU, AUTHD, AUTHP  ";
                //SQL = SQL + ComNum.VBLF + "FROM BAS_PROJECTFORMUSER U ";
                //SQL = SQL + ComNum.VBLF + "INNER JOIN BAS_PROJECTFORM F ";
                //SQL = SQL + ComNum.VBLF + "    ON U.FORMCD = F.FORMCD ";
                //SQL = SQL + ComNum.VBLF + "    AND F.FORMNAME = '" + objParent.Name + "' ";
                //SQL = SQL + ComNum.VBLF + "WHERE U.IDNUMBER = '" + clsType.User.IdNumber + "' ";
                SQL = "";
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    J.AUTHC, J.AUTHR, J.AUTHU, J.AUTHD, J.AUTHP  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_USER U ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_PMPA + "BAS_PROJECTFORMGROUP J ";
                SQL = SQL + ComNum.VBLF + "    ON U.JOBGROUP = J.JOBGROUP ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_PMPA + "BAS_PROJECTFORM F ";
                SQL = SQL + ComNum.VBLF + "    ON J.FORMCD = F.FORMCD ";
                SQL = SQL + ComNum.VBLF + "    AND F.FORMNAME = '" + objParent.Name + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE U.IDNUMBER = '" + clsType.User.IdNumber + "' ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                   for(int i = 0; i < dt.Rows.Count; i++ )
                    {
                        if (dt.Rows[i]["AUTHC"].ToString().Trim() == "1")
                        {
                            blnC = true;
                        }
                        if (dt.Rows[i]["AUTHR"].ToString().Trim() == "1")
                        {
                            blnR = true;
                        }
                        if (dt.Rows[i]["AUTHU"].ToString().Trim() == "1")
                        {
                            blnU = true;
                        }
                        if (dt.Rows[i]["AUTHD"].ToString().Trim() == "1")
                        {
                            blnD = true;
                        }
                        if (dt.Rows[i]["AUTHP"].ToString().Trim() == "1")
                        {
                            blnP = true;
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                Control[] controls = ComFunc.GetAllControls(objParent);
                foreach (Control ctl in controls)
                {
                    if (ctl is Button)
                    {
                        if (VB.Left(((Button)ctl).Name.Trim(), 7) == "btnSave" || VB.Left(((Button)ctl).Name.Trim(), 7) == "BtnSave")
                        {
                            if(blnC == true)
                            {
                                ((Button)ctl).Enabled = true;
                            }
                            else
                            {
                                ((Button)ctl).Enabled = false;
                            }
                        }
                        if (VB.Left(((Button)ctl).Name.Trim(), 9) == "btnSearch" || VB.Left(((Button)ctl).Name.Trim(), 9) == "BtnSearch")
                        {
                            if (blnR == true)
                            {
                                ((Button)ctl).Enabled = true;
                            }
                            else
                            {
                                ((Button)ctl).Enabled = false;
                            }
                        }
                        if (VB.Left(((Button)ctl).Name.Trim(), 9) == "btnUpdate" || VB.Left(((Button)ctl).Name.Trim(), 9) == "BtnUpdate")
                        {
                            if (blnU == true)
                            {
                                ((Button)ctl).Enabled = true;
                            }
                            else
                            {
                                ((Button)ctl).Enabled = false;
                            }
                        }
                        if (VB.Left(((Button)ctl).Name.Trim(), 9) == "btnDelete" || VB.Left(((Button)ctl).Name.Trim(), 9) == "BtnDelete")
                        {
                            if (blnD == true)
                            {
                                ((Button)ctl).Enabled = true;
                            }
                            else
                            {
                                ((Button)ctl).Enabled = false;
                            }
                        }
                        if (VB.Left(((Button)ctl).Name.Trim(), 8) == "btnPrint" || VB.Left(((Button)ctl).Name.Trim(), 8) == "BtnPrint")
                        {
                            if (blnP == true)
                            {
                                ((Button)ctl).Enabled = true;
                            }
                            else
                            {
                                ((Button)ctl).Enabled = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return;
            }
        }

        /// <summary>
        /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strJob"></param>
        /// <returns></returns>
        public static bool IsJobAuth(Form frm, string strJob, PsmhDb pDbCon) //, string strRemark = "")
        {
            bool rtnVal = false;
            DataTable Dt = new DataTable();

            if (System.Diagnostics.Debugger.IsAttached) return true;
            
            //중요업데이트가 있는지 확인한다 : 강제로 막기 어려움으로 메세지를 띄우도록 한다.??
            int intUpdate = 0;
            intUpdate = HaveToUpdate(pDbCon);

            //사용자별 권한이 있는지 확인한다.
            rtnVal = IsJobAuthCmdUser(frm, strJob, pDbCon); //, strRemark);
            return rtnVal;
        }

        /// <summary>
        /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strJob"></param>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        private static bool IsJobAuthCmdUser(Form frm, string strJob, PsmhDb pDbCon) //, string strRemark = "")
        {
            //사용자별 권한이 있는지 확인한다.
            bool rtnVal = false;
            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    J.AUTHC, J.AUTHR, J.AUTHU, J.AUTHD, J.AUTHP  ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_USER U ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_PROJECTFORMGROUP J ";
                SQL = SQL + ComNum.VBLF + "    ON U.JOBGROUP = J.JOBGROUP ";
                if (strJob == "C") SQL = SQL + ComNum.VBLF + "    AND J.AUTHC = '1' ";
                if (strJob == "R") SQL = SQL + ComNum.VBLF + "    AND J.AUTHR = '1' ";
                if (strJob == "U") SQL = SQL + ComNum.VBLF + "    AND J.AUTHU = '1' ";
                if (strJob == "D") SQL = SQL + ComNum.VBLF + "    AND J.AUTHD = '1' ";
                if (strJob == "P") SQL = SQL + ComNum.VBLF + "    AND J.AUTHP = '1' ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_PROJECTFORM F ";
                SQL = SQL + ComNum.VBLF + "    ON J.FORMCD = F.FORMCD ";
                SQL = SQL + ComNum.VBLF + "    AND F.FORMNAME = '" + frm.Name + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE U.IDNUMBER = '" + clsType.User.IdNumber + "' ";
                
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    rtnVal = true;
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                }

                if (rtnVal == false)
                {
                    SQL = "";
                    SQL = " SELECT  ";
                    SQL = SQL + ComNum.VBLF + "    J.AUTHC, J.AUTHR, J.AUTHU, J.AUTHD, J.AUTHP  ";
                    SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_PROJECTFORMGROUP J ";

                    SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_PROJECTFORM F ";
                    SQL = SQL + ComNum.VBLF + "    ON J.FORMCD = F.FORMCD ";
                    SQL = SQL + ComNum.VBLF + "    AND F.FORMNAME = '" + frm.Name + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE J.JOBGROUP = '" + ComNum.DEFAULT_JOBGROUP + "' ";
                    if (strJob == "C") SQL = SQL + ComNum.VBLF + "    AND J.AUTHC = '1' ";
                    if (strJob == "R") SQL = SQL + ComNum.VBLF + "    AND J.AUTHR = '1' ";
                    if (strJob == "U") SQL = SQL + ComNum.VBLF + "    AND J.AUTHU = '1' ";
                    if (strJob == "D") SQL = SQL + ComNum.VBLF + "    AND J.AUTHD = '1' ";
                    if (strJob == "P") SQL = SQL + ComNum.VBLF + "    AND J.AUTHP = '1' ";
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return rtnVal;
                    }
                    else
                    {
                        dt.Dispose();
                        dt = null;
                    }
                    rtnVal = true;
                }
                
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return rtnVal;
            }

            JobAuthCmdInsert(frm, strJob, pDbCon); //, strRemark);
            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strJob"></param>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        private static void JobAuthCmdInsert(Form frm, string strJob, PsmhDb pDbCon) //, string strRemark = "")
        {
            //return;

            //사용자별 권한이 있는지 확인한다.
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (frm == null) return;

            if (clsDB.strSource.Equals("PSMH_DEV"))
                return;

            try
            {
                string strTable = "";
                if (clsPublic.GstrSysDate != "")
                {
                    strTable = clsPublic.GstrSysDate.Replace("-", "");
                }
                else
                {
                    strTable = ComQuery.CurrentDateTime(pDbCon, "D");
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    FORMCD, FORMNAME, FORMNAME1 ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_PROJECTFORM ";
                SQL = SQL + ComNum.VBLF + "WHERE FORMNAME = '" + frm.Name + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    clsPublic.UserLogRemark = "";
                    return;
                }
                
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO ADMIN.ETC_USERLOG_" + strTable;
                SQL = SQL + ComNum.VBLF + "     (IDNUMBER, FORMCD, INPDATETIME, JOBGB, IP, FORMNAME, FORMNAME1, JOBREMARK)";
                SQL = SQL + ComNum.VBLF + "VALUES (";
                SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "', ";
                SQL = SQL + ComNum.VBLF + "     '" + dt.Rows[0]["FORMCD"].ToString().Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "     SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "     '" + strJob + "', ";
                SQL = SQL + ComNum.VBLF + "     '" + clsCompuInfo.gstrCOMIP + "', ";
                SQL = SQL + ComNum.VBLF + "     '" + dt.Rows[0]["FORMNAME"].ToString().Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "     '" + dt.Rows[0]["FORMNAME1"].ToString().Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "     '" + clsPublic.UserLogRemark  + "' ";
                SQL = SQL + ComNum.VBLF + "     )";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    if (SqlErr.IndexOf("ORA-00942") != -1)
                    {
                        clsPublic.GstrSysDate = Convert.ToDateTime(CurrentDateTime(pDbCon, "S")).ToShortDateString();
                    }
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("삭제중 오류가 발생하였습니다.");
                    clsPublic.UserLogRemark = "";
                    return;
                }

                dt.Dispose();
                dt = null;
                clsPublic.UserLogRemark = "";
                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                clsPublic.UserLogRemark = "";
                return;
            }
        }

        /// <summary>
        /// 중요업데이트가 있는지 확인 한다.
        /// 0: 없음, 1: 메세지만, 2: 강제 업데이트
        /// </summary>
        public static int HaveToUpdate(PsmhDb pDbCon)
        {
            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            double dblPSMHVERSION = clsPublic.PSMHVERSION;

            try
            {
                //dblPSMHVERSION = 0;
                //자동로그아웃 시간//프로그램 이전 버전 읽기
                string strAPLDATE = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D");
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "   GRPCDB, GRPCD, BASCD, APLFRDATE, APLENDDATE, BASNAME, BASNAME1,  ";
                SQL = SQL + ComNum.VBLF + "   VFLAG1, VFLAG2, VFLAG3, VFLAG4, NFLAG1, NFLAG2, NFLAG3, NFLAG4, ";
                SQL = SQL + ComNum.VBLF + "   REMARK, REMARK1, INPDATE, INPTIME,  ";
                SQL = SQL + ComNum.VBLF + "   USECLS, DISPSEQ ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD ";
                SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = '프로그램기초'";
                SQL = SQL + ComNum.VBLF + "       AND GRPCD = '프로그램'";
                SQL = SQL + ComNum.VBLF + "       AND BASCD = '프로그램버젼'";
                SQL = SQL + ComNum.VBLF + "       AND  '" + strAPLDATE + "' BETWEEN APLFRDATE AND APLENDDATE ";
                SQL = SQL + ComNum.VBLF + "       AND USECLS = '1'";
                SQL = SQL + ComNum.VBLF + "ORDER BY DISPSEQ ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return 1;
                }
                if (dt.Rows.Count > 0)
                {
                    dblPSMHVERSION = (int)VB.Val(dt.Rows[0]["NFLAG1"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                if (dblPSMHVERSION != clsPublic.PSMHVERSION)
                {
                    ComFunc.MsgBox("중요 업데이트가 있습니다." + ComNum.VBLF + "업데이트를 해 주시기 바랍니다.", "PSMH");
                }

                return 1;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return 1;
            }
        }

        /// <summary>
        /// 프로그램별로 중지할 경우: 1(중지)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pProgram"></param>
        /// <returns></returns>
        public static bool IsProgramStop(PsmhDb pDbCon, string pProgram)
        {
            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intStop = 0;
            string strRemark = "";

            bool blnPass = false;

            try
            {
                if (pProgram == "병동OCS")
                {
                    switch (clsCompuInfo.gstrCOMIP)
                    {
                        case "192.168.119.17":
                        case "192.168.14.80":
                        case "192.168.14.81":
                        case "192.168.14.82":
                        case "192.168.14.84":
                            blnPass = true;
                            break;
                    }
                }

                if (blnPass == true)
                {
                    return false;
                }

                //string strAPLDATE = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D");
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "   GRPCDB, GRPCD, BASCD, APLFRDATE, APLENDDATE, BASNAME, BASNAME1,  ";
                SQL = SQL + ComNum.VBLF + "   VFLAG1, VFLAG2, VFLAG3, VFLAG4, NFLAG1, NFLAG2, NFLAG3, NFLAG4, ";
                SQL = SQL + ComNum.VBLF + "   REMARK, REMARK1, INPDATE, INPTIME,  ";
                SQL = SQL + ComNum.VBLF + "   USECLS, DISPSEQ ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD ";
                SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = '프로그램기초'";
                SQL = SQL + ComNum.VBLF + "       AND GRPCD = '프로그램중지'";
                SQL = SQL + ComNum.VBLF + "       AND BASCD = '" + pProgram + "'";
                //SQL = SQL + ComNum.VBLF + "       AND  '" + strAPLDATE + "' BETWEEN APLFRDATE AND APLENDDATE ";
                SQL = SQL + ComNum.VBLF + "       AND USECLS = '1'";
                SQL = SQL + ComNum.VBLF + "ORDER BY DISPSEQ ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    intStop = (int)VB.Val(dt.Rows[0]["NFLAG1"].ToString().Trim());
                    strRemark = dt.Rows[0]["REMARK"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                if (intStop == 1)
                {
                    ComFunc.MsgBox(strRemark);
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return false;
            }
        }

        /// <summary>
        /// ETC_CSINFO_CODE TABLE DATA를 가지고 온다
        /// <param name="strGubun"></param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        /// 2017-05-26 박병규
        /// </summary>
        public DataTable Get_CsinfoCode(PsmhDb pDbCon, string strGubun, string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += ComNum.VBLF + " SELECT * ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_CSINFO_CODE ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1";
            SQL += ComNum.VBLF + "    AND GUBUN = '" + strGubun + "'";
            if (strCode != "")
            {
                SQL += ComNum.VBLF + "AND CODE = '" + strCode + "'";

            }
            SQL += ComNum.VBLF + "  ORDER BY CODE";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return null;
            }

            return dt;
        }

        /// <summary>
        /// BAS_BCODE TABLE DATA를 가지고 온다
        /// <param name="strGubun"></param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        /// 2017-05-26 박병규
        /// </summary>
        public DataTable Get_BasBcode(PsmhDb pDbCon, string strGubun, string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += ComNum.VBLF + " SELECT GUBUN, CODE, NAME,";
            SQL += ComNum.VBLF + "        TO_CHAR(JDATE, 'YYYY-MM-DD') JDATE,";
            SQL += ComNum.VBLF + "        TO_CHAR(DELDATE, 'YYYY-MM-DD') DELDATE,";
            SQL += ComNum.VBLF + "        ENTSABUN, ";
            SQL += ComNum.VBLF + "        TO_CHAR(ENTDATE, 'YYYY-MM-DD') ENTDATE,";
            SQL += ComNum.VBLF + "        SORT, PART, CNT, ";
            SQL += ComNum.VBLF + "        GUBUN2, GUBUN3, ROWID";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1";
            SQL += ComNum.VBLF + "    AND GUBUN = '" + strGubun + "'";
            if (strCode != "")
            {
                SQL += ComNum.VBLF + "AND CODE = '" + strCode + "'";

            }
            SQL += ComNum.VBLF + "  ORDER BY CODE";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("BAS_BCODE 조회중 문제가 발생했습니다." + ComNum.VBLF + ComNum.VBLF + SQL);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        /// <summary>
        /// Description : 후불대상자자동등록
        /// Author : 박병규
        /// Create Date : 2017.06.21
        /// <param name="ArgPtno"></param>
        /// <param name="ArgSname"></param>
        /// <param name="ArgRemark"></param>
        /// </summary>
        /// <seealso cref=""/>
        public void AUTO_MASTER_INSERT(PsmhDb pDbCon, string ArgPtno, string ArgSname, string ArgRemark)
        {
            DataTable DtMst = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            string strAutoRowid = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_AUTO_MST ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND PANO = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND GUBUN = '1' ";
            SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE = '') ";
            SqlErr = clsDB.GetDataTable(ref DtMst, SQL, pDbCon);

            if (DtMst.Rows.Count > 0)
                strAutoRowid = DtMst.Rows[0]["ROWID"].ToString().Trim();

            DtMst.Dispose();
            DtMst = null;

            

            if (strAutoRowid == "")
            {
                clsDB.setBeginTran(pDbCon);
                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_AUTO_MST";
                    SQL += ComNum.VBLF + "       (PANO, SNAME, GUBUN, ";
                    SQL += ComNum.VBLF + "        SDATE, ENTSABUN, ENTDATE, ";
                    SQL += ComNum.VBLF + "        ENTDATE2, REMARK) ";
                    SQL += ComNum.VBLF + " VALUES('" + ArgPtno + "', ";
                    SQL += ComNum.VBLF + "        '" + ArgSname + "', ";
                    SQL += ComNum.VBLF + "        '1', ";
                    SQL += ComNum.VBLF + "        TRUNC(SYSDATE), ";
                    SQL += ComNum.VBLF + "        '" + clsPublic.GstrJobPart + "', ";
                    SQL += ComNum.VBLF + "        SYSDATE, ";
                    SQL += ComNum.VBLF + "        SYSDATE, ";
                    SQL += ComNum.VBLF + "        '" + ArgRemark + "')";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                    return;
                }
            }
        }


        /// <summary>
        /// Description : 후불대상자자동삭제
        /// Author : 박병규
        /// Create Date : 2017.06.21
        /// <param name="ArgPtno"></param>
        /// <param name="ArgSname"></param>
        /// <param name="ArgRemark"></param>
        /// </summary>
        public void AUTO_MASTER_DELETE(PsmhDb pDbCon, string ArgPtno, string ArgRemark)
        {
            DataTable DtMst = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            string strAutoRowid = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_AUTO_MST ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND PANO = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND GUBUN = '1' ";
            SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE = '') ";
            SqlErr = clsDB.GetDataTable(ref DtMst, SQL, pDbCon);

            if (DtMst.Rows.Count > 0)
                strAutoRowid = DtMst.Rows[0]["ROWID"].ToString().Trim();

            DtMst.Dispose();
            DtMst = null;
            
            if (strAutoRowid != "")
            {
                clsDB.setBeginTran(pDbCon);

                try
                {
                    //이전내역 백업
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_AUTO_MST_HIS  ";
                    SQL += ComNum.VBLF + "       (PANO, SNAME, GUBUN, ";
                    SQL += ComNum.VBLF + "        DEPTCODE, SDATE, EDATE, ";
                    SQL += ComNum.VBLF + "        DELDATE, ENTDATE, ENTDATE2, ";
                    SQL += ComNum.VBLF + "        ENTSABUN, REMARK) ";
                    SQL += ComNum.VBLF + " SELECT PANO, SNAME, GUBUN, ";
                    SQL += ComNum.VBLF + "        DEPTCODE, SDATE, EDATE, ";
                    SQL += ComNum.VBLF + "        DELDATE, ENTDATE, ENTDATE2, ";
                    SQL += ComNum.VBLF + "        ENTSABUN, REMARK ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_AUTO_MST ";
                    SQL += ComNum.VBLF + "  WHERE ROWID = '" + strAutoRowid + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_AUTO_MST ";
                    SQL += ComNum.VBLF + "    SET ENTSABUN      = '" + clsPublic.GnJobSabun + "', ";
                    SQL += ComNum.VBLF + "        DELDATE       = SYSDATE, ";
                    SQL += ComNum.VBLF + "        ENTDATE2      = SYSDATE, ";
                    SQL += ComNum.VBLF + "        REMARK        = '" + ArgRemark + "' ";
                    SQL += ComNum.VBLF + "  WHERE ROWID         = '" + strAutoRowid + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                    return;
                }
            }

        }

        /// <summary>
        /// 사용자 정보를 담는다
        /// Author : 박웅규
        /// Create Date : 2017.07.12
        /// <param name="Sabun">사용자사번</param>
        /// <param name="Password">비밀번호</param>
        /// <param name="SysDate">현재날짜(서버날짜)</param>
        /// <param name="ClearToo">초기화 포함 여부</param>
        /// </summary>
        public static bool SetUser(PsmhDb pDbCon, string strIdNumber, string Password, string SysDate, bool ClearToo = true)
        {
            if(ClearToo == true) clsType.ClearUser(); //사용자 정보 초기화
            clsType.User.IdNumber = strIdNumber;
            clsType.User.Sabun = ComFunc.SetAutoZero(strIdNumber,5);
            clsType.User.PasswordChar = Password;
            clsType.User.DrCode = "";
            clsType.User.JobPart = strIdNumber; //GstrJobPart = Trim(TxtSabun.Text) '2006-01-04

            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (SysDate == "")
                {
                    SysDate = ComQuery.CurrentDateTime(pDbCon, "D");
                } 
                else
                {
                    SysDate = SysDate.Replace("-",""); 
                }

                switch (clsType.User.Sabun)
                {
                    case "00015":
                    case "00016":
                    case "00017":
                    case "00018":
                    case "4349":
                    case "04444":
                    case "04349":
                        break;
                    default:
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(TOIDAY,'YYYYMMDD') TOIDAY FROM " + ComNum.DB_ERP + "INSA_MST";
                        SQL = SQL + ComNum.VBLF + "WHERE SABUN = '" + clsType.User.Sabun + "'";
                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            clsType.ClearUser(); //사용자 정보 초기화
                            return rtnVal;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            if(dt.Rows[0]["TOIDAY"].ToString().Trim() != "" && VB.Val(dt.Rows[0]["TOIDAY"].ToString().Trim()) < VB.Val(SysDate) )
                            {
                                dt.Dispose();
                                dt = null;
                                ComFunc.MsgBox("해당하는 사번을 사용할수 없습니다. 전산실로 연락 바랍니다.");
                                clsType.ClearUser(); //사용자 정보 초기화
                                return rtnVal;
                            }
                        }
                        else
                        {
                            dt.Dispose();
                            dt = null;
                            ComFunc.MsgBox("해당하는 사번을 사용할수 없습니다. 전산실로 연락 바랍니다.");
                            clsType.ClearUser(); //사용자 정보 초기화
                            return rtnVal;
                        }
                        dt.Dispose();
                        dt = null;
                        break;
                }
                //사용자이름 가지고 오기
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT USERNAME, PASSHASH256, JOBGROUP ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_USER";
                SQL = SQL + ComNum.VBLF + "WHERE IDNUMBER =  '" + clsType.User.IdNumber  + "'" ;
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsType.ClearUser(); //사용자 정보 초기화
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    clsType.User.UserName = dt.Rows[0]["USERNAME"].ToString().Trim();
                    clsType.User.JobName = clsType.User.UserName;
                    clsType.User.JobGroup = dt.Rows[0]["JOBGROUP"].ToString().Trim();
                    clsType.User.Passhash256 = dt.Rows[0]["PASSHASH256"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                //사용자인사정보 세팅
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "     BUSE, SIL_BUSE  ";
                SQL = SQL + ComNum.VBLF + "   , ADMIN.FC_BAS_BUSENAME(BUSE) AS BUSENAME ";
                SQL = SQL + ComNum.VBLF + "   , ADMIN.FC_BAS_BUSENAME(SIL_BUSE) AS SIL_BUSENAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST";
                SQL = SQL + ComNum.VBLF + "WHERE SABUN =  '" + clsType.User.Sabun + "'";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsType.ClearUser(); //사용자 정보 초기화
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    clsType.User.BuseCode = dt.Rows[0]["BUSE"].ToString().Trim();
                    clsType.User.BuseName = dt.Rows[0]["BUSENAME"].ToString().Trim();
                    clsType.User.SilBuseCode = dt.Rows[0]["SIL_BUSE"].ToString().Trim();
                    clsType.User.Sil_BuseName = dt.Rows[0]["SIL_BUSENAME"].ToString().Trim(); 
                }
                dt.Dispose();
                dt = null;

                //의사여부 판단
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DrCode, GbOUT FROM " + ComNum.DB_MED + "OCS_DOCTOR WHERE Sabun = '" + clsType.User.Sabun + "'";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsType.ClearUser(); //사용자 정보 초기화
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    clsType.User.DrCode = dt.Rows[0]["DrCode"].ToString().Trim();
                    clsType.User.JobMan = "";
                }
                else
                {
                    //TODO
                    //If GstrJobMan = "간호사" And Gstr간호사사용 = "Y" Then
                    //    GstrDrCode = Gstr간호사_DrCode
                    //Else
                    //    Exit Sub
                    //End If
                }
                dt.Dispose();
                dt = null;

                //조직검사 결과 조회용 사번별 진료과 SET
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DeptCode FROM " + ComNum.DB_MED + "OCS_DOCTOR";
                //TODO
                //If GstrJobMan = "간호사" And Gstr간호사사용 = "Y" Then
                //    SQL = SQL & " WHERE Sabun=" & Gstr간호사_Sabun & " "
                //Else
                //    SQL = SQL & " WHERE Sabun=" & Val(TxtDrCode.Text) & " "
                //End If
                SQL = SQL + ComNum.VBLF + " WHERE Sabun= '" + clsType.User.Sabun + "' "; //SABUN
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsType.ClearUser(); //사용자 정보 초기화
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    clsType.User.AnatDeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                #region //EMR 권한조회
                SQL = "SELECT * ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AVIEWEMRUSER";
                SQL = SQL + ComNum.VBLF + "WHERE USEID = '" + clsType.User.IdNumber + "'";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsType.ClearUser(); //사용자 정보 초기화
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    clsType.User.AuAIMAGE = (VB.Val(dt.Rows[0]["AIMAGE"].ToString().Trim())).ToString();
                    clsType.User.AuAVIEW = (VB.Val(dt.Rows[0]["AVIEW"].ToString().Trim())).ToString();
                    clsType.User.AuASCAN = (VB.Val(dt.Rows[0]["ASCAN"].ToString().Trim())).ToString();
                    clsType.User.AuAVERIFY = (VB.Val(dt.Rows[0]["AVERIFY"].ToString().Trim())).ToString();
                    clsType.User.AuACOPY = (VB.Val(dt.Rows[0]["ACOPY"].ToString().Trim())).ToString();
                    clsType.User.AuAWRITE = (VB.Val(dt.Rows[0]["AWRITE"].ToString().Trim())).ToString();
                    clsType.User.AuAMANAGE = (VB.Val(dt.Rows[0]["AMANAGE"].ToString().Trim())).ToString();
                    clsType.User.AuAUSE = (VB.Val(dt.Rows[0]["AUSE"].ToString().Trim())).ToString();
                    clsType.User.AuAPRINTIN = (VB.Val(dt.Rows[0]["APRINTIN"].ToString().Trim())).ToString();
                    clsType.User.AuAPRINTOUT = (VB.Val(dt.Rows[0]["APRINTOUT"].ToString().Trim())).ToString();
                    clsType.User.AuAPRINTSIM = (VB.Val(dt.Rows[0]["APRINTSIM"].ToString().Trim())).ToString();
                    clsType.User.AuAMASK = (VB.Val(dt.Rows[0]["AMASK"].ToString().Trim())).ToString();
                }
                dt.Dispose();
                dt = null;

                #endregion

                #region //의사일 경우만 처리
                if (clsType.User.DrCode != "")
                {
                    SQL = "   SELECT DrName, DeptNameK, DeptCode, DrDept1, DrDept2, YTimeGbn,YInwon ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_DOCTOR A,     ";
                    SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_PMPA + "BAS_CLINICDEPT B  ";
                    SQL = SQL + ComNum.VBLF + "WHERE DrCode = '" + clsType.User.DrCode + "' ";  //DrCode
                    SQL = SQL + ComNum.VBLF + "  AND A.DrDept1 = B.DeptCode ";
                    SQL = SQL + ComNum.VBLF + "  AND ROWNUM    = 1 ";
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        clsType.ClearUser(); //사용자 정보 초기화
                        return rtnVal;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        //TODO
                        //SQL = " SELECT IdNumber, Name, Class, Passward FROM " + ComNum.DB_PMPA + "BAS_PASS ";
                        //SQL = SQL + ComNum.VBLF + "WHERE IdNumber  = " + clsType.User.IdNumber;
                        //SQL = SQL + ComNum.VBLF + "  AND ProgramID = ' ' ";
                        //SqlErr = clsDB.GetDataTableREx(ref dt1, SQL);

                        //if (SqlErr != "")
                        //{
                        //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        //    clsType.ClearUser(); //사용자 정보 초기화
                        //    return rtnVal;
                        //}
                        //if (dt1.Rows.Count > 0)
                        //{
                        //    clsType.User.JobMan = "간호사";
                        //}
                        //else
                        //{
                        //    clsType.User.JobMan = "";
                        //    clsType.User.DrCode = "";
                        //    clsType.User.DeptCode = "";
                        //    clsType.User.JupsuDept1 = "";
                        //    clsType.User.JupsuDept2 = "";
                        //    clsType.User.ResvGbn = 0;
                        //    clsType.User.ResvInwon = 0;
                        //}
                        //dt1.Dispose();
                        //dt1 = null;
                    }
                    else
                    {
                        //TODO
                        //If GstrJobMan = "간호사" And Gstr간호사사용 = "Y" Then
                        //    GstrJobMan = "간호사"
                        //    LabelDrName = Gstr간호사_UName
                        //Else
                        //    GstrJobMan = "의사"  '2014-08-19
                        //    LabelDrName = AdoGetString(Rs, "DrName", 0)
                        //End If
                        clsType.User.DeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                        clsType.User.JupsuDept1 = dt.Rows[0]["DrDept1"].ToString().Trim();
                        clsType.User.JupsuDept2 = dt.Rows[0]["DrDept2"].ToString().Trim();
                        clsType.User.ResvGbn = (int)VB.Val(dt.Rows[0]["YTimeGbn"].ToString().Trim());
                        clsType.User.ResvInwon = (int)VB.Val(dt.Rows[0]["YInwon"].ToString().Trim());
                    }
                    dt.Dispose();
                    dt = null;

                    //2013-09-14
                    if(clsType.User.DrCode == "1199" && clsType.User.DeptCode == "MD")
                    {
                        if (clsType.User.Sabun == "32935" || clsType.User.Sabun == "32096" || clsType.User.Sabun == "32926")
                        {
                            //MsgBox "퇴사 처리된 사번입니다...", vbCritical, "퇴사확인"
                        }
                        //진료에서 별도 처리 바람
                        //If MsgBox("인공신장 전공의 처방으로 전환하시겠습니까??", vbYesNo + vbDefaultButton2, "처방구분선택") = vbYes Then
                    }
                }
                #endregion //의사일 경우만 처리

                #region //(진료)READ_ORDER_LOGIN_CHK
                //처방에서 사용하는 것 세팅이 필요함
                clsType.User.CanOrd = ""; //str오더권한 //변경이 필요함
                //의사체크
                SQL = "SELECT DrName FROM " + ComNum.DB_MED + "OCS_DOCTOR"; 
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + clsType.User.Sabun + "' "; //SABUN
                SQL = SQL + ComNum.VBLF + " AND GBOUT='N' ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsType.ClearUser(); //사용자 정보 초기화
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    clsType.User.IsDoct = "OK";
                }
                dt.Dispose();
                dt = null;

                //간호사체크
                SQL = "SELECT KORNAME FROM ADMIN.INSA_MST";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + clsType.User.Sabun + "' "; //SABUN
                SQL = SQL + ComNum.VBLF + " AND JikJong = '41' ";
                SQL = SQL + ComNum.VBLF + " AND (TOIDay IS NULL OR TOIDay ='') ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsType.ClearUser(); //사용자 정보 초기화
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    clsType.User.IsNurse = "OK";
                }
                dt.Dispose();
                dt = null;

                //간호사조무사체크
                SQL = "SELECT KORNAME FROM ADMIN.INSA_MST";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + clsType.User.Sabun + "' "; //SABUN
                SQL = SQL + ComNum.VBLF + " AND JikJong = '42' ";
                SQL = SQL + ComNum.VBLF + " AND (TOIDay IS NULL OR TOIDay ='') ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsType.ClearUser(); //사용자 정보 초기화
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    clsType.User.IsCoNurse = "OK";
                }
                dt.Dispose();
                dt = null;
                #endregion //(진료)READ_ORDER_LOGIN_CHK

                #region 간호부 소속(간호부 스케쥴 등록대상자이면 근무시간 읽어서 로그인 가능 여부 체크)
                //2020-08-18 K.M.C
                if (Check_Nur_Schedule(clsDB.DbCon))
                {
                    if (!Check_GunTae_BunPyo(clsDB.DbCon))
                    {
                        clsType.ClearUser(); //사용자 정보 초기화
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                #endregion

                #region //로그인 카운터 0으로 변경
                int intRowAffected = 0;
                if (Password != ComNum.MNGPASSWORD)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_USER SET ";
                    SQL = SQL + ComNum.VBLF + "     LOGINCNT = 0";
                    SQL = SQL + ComNum.VBLF + "WHERE IDNUMBER = '" + clsType.User.IdNumber + "'"; //IDNUMBER
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }
                #endregion //로그인 카운터 0으로 변경

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                clsType.ClearUser(); //사용자 정보 초기화
                return rtnVal;
            }
        }

        public static bool Check_Nur_Schedule(PsmhDb pDbCon)
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strWardCode = "";
            string strJikCode = "";
            string strSchedule = "";

            try
            {
                //string strCurDate = DateTime.Now.ToShortDateString().Replace("-", "").Substring(0, 6);
                string strCurDate = VB.Left(ComQuery.CurrentDateTime(pDbCon, "D"), 6);

                #region 간호부 스케쥴표 등록대상자인지 점검
                SQL = "";
                SQL += ComNum.VBLF + "SELECT WARDCODE, SCHEDULE, JIKCODE, ROWID                  ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_SCHEDULE1        ";
                SQL += ComNum.VBLF + " WHERE Sabun = '" + clsType.User.Sabun + "'       ";
                SQL += ComNum.VBLF + "   AND YYMM = '" + VB.Left(strCurDate, 6) + "'    ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsType.ClearUser(); //사용자 정보 초기화
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    strWardCode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    strJikCode = dt.Rows[0]["JIKCODE"].ToString().Trim();
                    strSchedule = dt.Rows[0]["SCHEDULE"].ToString().Trim();

                    //스케쥴이 없으면 간호사 아니라고 판단, 해당 로직 패스
                    if (strSchedule == "")
                    {
                        rtnVal = false;
                    }
                    else
                    {
                        //당직대상인 부서는 제외
                        //AN : 마취과, OR : 수술실, ANGO : ANGIO, CSR : 중앙공급실, HD : 투석실, OPD : 외래, GAN : 간호부(PA 전담)
                        if (strWardCode == "AN" || strWardCode == "OR" || strWardCode == "ANGO" || strWardCode == "CSR" || strWardCode == "HD" || strWardCode == "OPD" || strWardCode == "GAN")
                        {
                            rtnVal = false;
                        }
                        //ER 코디네이터는 제외 2021-07-13 
                        else if (strWardCode == "ER" && strJikCode == "95")
                        {
                            rtnVal = false;
                        }
                        else
                        {
                            rtnVal = true;
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                #endregion

                #region 간호부 관리자 사번은 체크 안함
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ROWID FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL += ComNum.VBLF + " WHERE GUBUN = 'NUR_시스템관리자'                 ";
                SQL += ComNum.VBLF + "   AND CODE = '" + clsType.User.Sabun + "'        ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsType.ClearUser(); //사용자 정보 초기화
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = false;
                }
                dt.Dispose();
                dt = null;
                #endregion

                #region BAS_BCODE 에서 시행여부 확인
                SQL = "";
                SQL += ComNum.VBLF + "SELECT NAME FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL += ComNum.VBLF + " WHERE GUBUN = 'C#간호부로그인체크'                 ";
                SQL += ComNum.VBLF + "   AND CODE = '001'        ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsType.ClearUser(); //사용자 정보 초기화
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["NAME"].ToString().Trim() != "Y")
                    {
                        rtnVal = false;
                    }
                }
                dt.Dispose();
                dt = null;
                #endregion

                return rtnVal;
            }
            catch (Exception ex)
            {
                //LogOutTimer 실행시 오라클 세션이 만료되었을 경우 오류발생으로 주석처리 함 2021-01-13 KMC
                //ComFunc.MsgBox(ex.Message);
                //clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Application.ExitThread();
                Environment.Exit(0);
                return false;
            }
        }

        public static bool Check_GunTae_BunPyo(PsmhDb pDbCon, bool bMsgPop = true)
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strSchedule = "";
            string strSchedule2 = "";
            string strWardCode = "";
            string strCurDate = "";
            string strCurTime = "";
            string strDuty = "";
            string strDuty2 = "";
            string strOK = "";
            string strSTime = "",  strETime = "";
            string strSTime2 = "", strETime2 = "";
            string strMsg = "";

            //간호부 55병동 사유입력후 로그인시 계속 사용가능
            if (clsPublic.GstrNurLogInFlag_Ward == "OK") { return true; }

            try
            {
                ComFunc.ReadSysDate(pDbCon);

                strCurDate = clsPublic.GstrSysDate;
                strCurTime = clsPublic.GstrSysTime;

                int nDay = DateTime.Now.Day;                //당일
                int nDay2 = DateTime.Now.AddDays(-1).Day;   //전일

                #region 당월 스케쥴 읽기
                SQL = "";
                SQL += ComNum.VBLF + "SELECT WARDCODE, SCHEDULE, JIKCODE, ROWID                             ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_SCHEDULE1                            ";
                SQL += ComNum.VBLF + " WHERE Sabun = '" + clsType.User.Sabun + "'                           ";
                SQL += ComNum.VBLF + "   AND YYMM = '" + VB.Left(VB.Replace(strCurDate, "-", ""), 6) + "'   ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsType.ClearUser(); //사용자 정보 초기화
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    strSchedule = dt.Rows[0]["SCHEDULE"].ToString();
                    strWardCode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                #endregion

                if (nDay == 1)  //매월 초
                {
                    #region 전월 스케쥴 읽기
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT WARDCODE, SCHEDULE, JIKCODE, ROWID         ";
                    SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_SCHEDULE1        ";
                    SQL += ComNum.VBLF + " WHERE Sabun = '" + clsType.User.Sabun + "'       ";
                    SQL += ComNum.VBLF + "   AND YYMM = TO_CHAR(SYSDATE-1, 'YYYYMM')        ";
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        clsType.ClearUser(); //사용자 정보 초기화
                        return false;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strSchedule2 = dt.Rows[0]["SCHEDULE"].ToString();
                    }
                    #endregion

                    strDuty = ComFunc.MidH(strSchedule, (nDay * 4) - 3, 4).Trim();       //당일스케쥴
                    strDuty2 = ComFunc.MidH(strSchedule2, (nDay2 * 4) - 3, 4).Trim();    //전일스케쥴
                }
                else
                {
                    strDuty = ComFunc.MidH(strSchedule, (nDay * 4) - 3, 4).Trim();       //당일스케쥴
                    strDuty2 = ComFunc.MidH(strSchedule, (nDay2 * 4) - 3, 4).Trim();    //전일스케쥴
                }

                #region 당일 스케쥴 근무시간 읽기
                SQL = "";
                SQL += ComNum.VBLF + "SELECT CODE, STIME, ETIME                     ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "INSA_GUNTAECODE   ";
                SQL += ComNum.VBLF + " WHERE NURCODE = '" + strDuty + "'            ";
                SQL += ComNum.VBLF + "   AND DELDATE IS NULL                        ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsType.ClearUser(); //사용자 정보 초기화
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    strSTime = dt.Rows[0]["STIME"].ToString().Trim();
                    strETime = dt.Rows[0]["ETIME"].ToString().Trim();
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    rtnVal = true;      //근무코드가 없으면 코드작업 오류로 접속은 가능함.
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;
                #endregion

                #region 전일 스케쥴 근무시간 읽기
                SQL = "";
                SQL += ComNum.VBLF + "SELECT CODE, STIME, ETIME                     ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "INSA_GUNTAECODE   ";
                SQL += ComNum.VBLF + " WHERE NURCODE = '" + strDuty2 + "'           ";
                SQL += ComNum.VBLF + "   AND DELDATE IS NULL                        ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsType.ClearUser(); //사용자 정보 초기화
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    strSTime2 = dt.Rows[0]["STIME"].ToString().Trim();
                    strETime2 = dt.Rows[0]["ETIME"].ToString().Trim();
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    rtnVal = true;      //근무코드가 없으면 코드작업 오류로 접속은 가능함.
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;
                #endregion

                //근무시작 30분전, 근무종료 까지 로그인 가능하게 요청

                string strCurDay = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;                         //현재 시각

                string strStart = strSTime == "" ? "" : Convert.ToDateTime(strSTime).AddMinutes(-30).ToString("yyyy-MM-dd HH:mm");    //당일 시작 시각
                string strEnd   = strETime == "" ? "" : Convert.ToDateTime(strETime).AddMinutes(clsPublic.GnNurGunTime_ADD).ToString("yyyy-MM-dd HH:mm"); //당일 종료 시각
                if (string.Compare(strSTime, strETime) > 0)
                {
                    strEnd = Convert.ToDateTime(strEnd).AddDays(1).ToString("yyyy-MM-dd HH:mm");
                }

                string strStart2 = strSTime2 == "" ? "" : Convert.ToDateTime(strSTime2).AddDays(-1).AddMinutes(-30).ToString("yyyy-MM-dd HH:mm");  //전일 시작 시각
                string strEnd2   = strETime2 == "" ? "" : Convert.ToDateTime(strETime2).AddDays(-1).AddMinutes(clsPublic.GnNurGunTime_ADD).ToString("yyyy-MM-dd HH:mm");  //전일 종료 시각
                if (string.Compare(strSTime2, strETime2) > 0)
                {
                    strEnd2 = Convert.ToDateTime(strEnd2).AddDays(1).ToString("yyyy-MM-dd HH:mm");
                }

                //당일 근무번표가 있으면
                if (strStart != "" && strEnd != "")
                {
                    //당일번표 기준 비교
                    if (string.Compare(strCurDay, strStart) >= 0 && string.Compare(strCurDay, strEnd) <= 0)
                    {
                        strOK = "OK";
                    }
                    else
                    {
                        //어제 번표에 이어서 사용 가능한지 체크
                        if (strStart2 != "" && strEnd2 != "")
                        {
                            if (string.Compare(strCurDay, strStart2) >= 0 && string.Compare(strCurDay, strEnd2) <= 0)
                            {
                                strOK = "OK";
                            }
                        }
                    }
                }
                else
                {
                    strSTime = strSTime2;
                    strETime = strETime2;

                    //당일 근무번표가 없고 어제 근무번표가 있으면 (어제 근무표에 이어서 00시 넘어가는 시점)
                    if (strStart2 != "" && strEnd2 != "")
                    {
                        //어제 근무번표 비교
                        if (string.Compare(strCurDay, strStart2) >= 0 && string.Compare(strCurDay, strEnd2) <= 0)
                        {
                            strOK = "OK";
                        }
                    }
                }

                if (strOK == "OK")
                {
                    rtnVal = true;
                }
                else
                {
                    //2021-01-22 55병동은 긴급분만으로 출근하는 경우 사유를 입력하면 로그인 가능하게 요청 (간호부 김정화 팀장 요청)
                    //2021-04-06 55병동 패쇄로 83병동을 적용함
                    if (strWardCode == "83")
                    {
                        string strSayu =  Microsoft.VisualBasic.Interaction.InputBox("로그인 사유를 간략히 입력하세요.", "로그인 사유", "");

                        if (strSayu.Trim() == "")
                        {
                            ComFunc.MsgBox("로그인 사유를 입력하세요.", "로그인 불가");
                            rtnVal = false;
                        }
                        else
                        {
                            AUTO_LOGOUT_LOG("1", strSayu);
                            clsPublic.GstrNurLogInFlag_Ward = "OK";
                            rtnVal = true;
                        }
                    }
                    else
                    {
                        strMsg = "현재 스케쥴(간호부)은 근무시간이 아닙니다." + ComNum.VBLF + ComNum.VBLF;

                        if (strSTime != "" && strETime != "")
                        {
                            strMsg += "근무시작 : " + strSTime + ComNum.VBLF;
                            strMsg += "근무종료 : " + strETime + ComNum.VBLF;
                        }
                        strMsg += "근무시작 30분전, 근무종료 시각 까지만 접속가능. " + ComNum.VBLF;

                        if (bMsgPop)
                        {
                            ComFunc.MsgBox(strMsg, "접속불가");
                        }

                        rtnVal = false;
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                //LogOutTimer 실행시 오라클 세션이 만료되었을 경우 오류발생으로 주석처리 함 2021-01-13 KMC
                //ComFunc.MsgBox(ex.Message);
                //clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        private static void AUTO_LOGOUT_LOG(string ArgGubun, string argSayu)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO ADMIN.ETC_LOGOUT_LOG ";
                SQL = SQL + ComNum.VBLF + "     ( BDATE,SABUN,OUTTIME,IPADDR,GUBUN,EXENAME, SAYU )";
                SQL = SQL + ComNum.VBLF + "     VALUES ";
                SQL = SQL + ComNum.VBLF + "     ( ";
                SQL = SQL + ComNum.VBLF + "     TRUNC(SYSDATE), ";                      //BDATE
                SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "', ";     //SABUN
                SQL = SQL + ComNum.VBLF + "     SYSDATE, ";                             //OUTTIME
                SQL = SQL + ComNum.VBLF + "     '" + clsCompuInfo.gstrCOMIP + "', ";     //IPADDR
                SQL = SQL + ComNum.VBLF + "     '" + ArgGubun + "', ";      //GUBUN
                SQL = SQL + ComNum.VBLF + "     '" + "PSMHMAIN" + "', ";    //EXENAME
                SQL = SQL + ComNum.VBLF + "     '" + argSayu + "' ";        //SAYU
                SQL = SQL + ComNum.VBLF + "     ) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        /// <summary>
        /// 사용자 옵션을 불러온다
        /// Author : 박웅규
        /// Create Date : 2017.07.20
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        public static DataTable Select_BAS_USEROPTION(PsmhDb pDbCon, string strQuery)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "     IDNUMBER, OPTIONGB, ";
            SQL = SQL + ComNum.VBLF + "     VVALUE1, VVALUE2, VVALUE3, ";
            SQL = SQL + ComNum.VBLF + "     NVALUE1, NVALUE2, NVALUE3, ";
            SQL = SQL + ComNum.VBLF + "     INPDATETIME, INPIDNUMBER, INPIP, ";
            SQL = SQL + ComNum.VBLF + "     UPDATETIME, UPIDNUMBER, UPIP ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_USEROPTION ";
            SQL = SQL + strQuery ;
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        /// <summary>
        /// Query : 사용자 옵션을 삭제한다
        /// Author : 박웅규
        /// Create Date : 2017.07.20
        /// </summary>
        /// <param name="strOPTIONGB">사용자 옵션</param>
        /// <returns></returns>
        public static string Delete_BAS_USEROPTION(string strOPTIONGB)
        {
            string rtnVal = "";
            string SQL = "";    //Query문
            SQL = "";
            SQL = SQL + ComNum.VBLF + "DELETE FROM " + ComNum.DB_PMPA + "BAS_USEROPTION ";
            SQL = SQL + ComNum.VBLF + "WHERE IDNUMBER = '" + clsType.User.IdNumber + "'";
            SQL = SQL + ComNum.VBLF + "     AND OPTIONGB = '" + strOPTIONGB + "'";
            rtnVal = SQL;
            return rtnVal;
        }

        /// <summary>
        /// Query : 사용자 정보를 저장한다.
        /// Author : 박웅규
        /// Create Date : 2017.07.20
        /// </summary>
        /// <param name="strOPTIONGB">사용자 옵션</param>
        /// <param name="strVVALUE1">문자값1</param>
        /// <param name="strVVALUE2">문자값2</param>
        /// <param name="strVVALUE3">문자값3</param>
        /// <param name="strNVALUE1">숫자값1</param>
        /// <param name="strNVALUE2">숫자값2</param>
        /// <param name="strNVALUE3">숫자값3</param>
        /// <returns></returns>
        public static string Save_BAS_USEROPTION(string strOPTIONGB, string strVVALUE1, string strVVALUE2, string strVVALUE3,
                                                string strNVALUE1, string strNVALUE2, string strNVALUE3)
        {
            string rtnVal = "";
            string SQL = "";    //Query문

            SQL = "";
            SQL = SQL + ComNum.VBLF + "INSERT INTO BAS_USEROPTION ( ";
            SQL = SQL + ComNum.VBLF + "	IDNUMBER, OPTIONGB,  ";
            SQL = SQL + ComNum.VBLF + "	VVALUE1, VVALUE2, VVALUE3,  ";
            SQL = SQL + ComNum.VBLF + "	NVALUE1, NVALUE2, NVALUE3,  ";
            SQL = SQL + ComNum.VBLF + "	INPDATETIME, INPIDNUMBER, INPIP,  ";
            SQL = SQL + ComNum.VBLF + "	UPDATETIME, UPIDNUMBER, UPIP ) ";
            SQL = SQL + ComNum.VBLF + "VALUES ( ";
            SQL = SQL + ComNum.VBLF + "		'" + clsType.User.IdNumber + "',"; //IDNUMBER, 
            SQL = SQL + ComNum.VBLF + "		'" + strOPTIONGB + "',"; //OPTIONGB, 
            SQL = SQL + ComNum.VBLF + "		'" + strVVALUE1 + "',"; //VVALUE1, 
            SQL = SQL + ComNum.VBLF + "		'" + strVVALUE2 + "',"; //VVALUE2, 
            SQL = SQL + ComNum.VBLF + "		'" + strVVALUE3 + "',"; //VVALUE3, 
            SQL = SQL + ComNum.VBLF + "		'" + strNVALUE1 + "',"; //NVALUE1, 
            SQL = SQL + ComNum.VBLF + "		'" + strNVALUE2 + "',"; //NVALUE2, 
            SQL = SQL + ComNum.VBLF + "		'" + strNVALUE3 + "',"; //NVALUE3, 
            SQL = SQL + ComNum.VBLF + "         SYSDATE, ";      //INPDATETIME
            SQL = SQL + ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";      //INPIDNUMBER
            SQL = SQL + ComNum.VBLF + "         '" + clsCompuInfo.gstrCOMIP + "', ";      //INPIP
            SQL = SQL + ComNum.VBLF + "         SYSDATE, ";      //UPDATETIME
            SQL = SQL + ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";      //UPIDNUMBER
            SQL = SQL + ComNum.VBLF + "         '" + clsCompuInfo.gstrCOMIP + "' ";      //UPIP
            SQL = SQL + ComNum.VBLF + ") ";
            rtnVal = SQL;
            return rtnVal;
        }

        /// <summary>
        /// Query : 사용자 정보를 변경한다.
        /// Author : 박웅규
        /// Create Date : 2017.07.20
        /// </summary>
        /// <param name="strOPTIONGB">사용자 옵션</param>
        /// <param name="strVVALUE1">문자값1</param>
        /// <param name="strVVALUE2">문자값2</param>
        /// <param name="strVVALUE3">문자값3</param>
        /// <param name="strNVALUE1">숫자값1</param>
        /// <param name="strNVALUE2">숫자값2</param>
        /// <param name="strNVALUE3">숫자값3</param>
        /// <returns></returns>
        public static string Update_BAS_USEROPTION(string strOPTIONGB, string strVVALUE1, string strVVALUE2, string strVVALUE3,
                                                    string strNVALUE1, string strNVALUE2, string strNVALUE3)
        {
            string rtnVal = "";
            string SQL = "";    //Query문

            SQL = "";
            SQL = SQL + ComNum.VBLF + "UPDATE BAS_USEROPTION SET ";
            SQL = SQL + ComNum.VBLF + "		VVALUE1 = '" + strVVALUE1 + "',";   //VVALUE1, 
            SQL = SQL + ComNum.VBLF + "		VVALUE2 = '" + strVVALUE2 + "',";   //VVALUE2, 
            SQL = SQL + ComNum.VBLF + "		VVALUE3 = '" + strVVALUE3 + "',";   //VVALUE3, 
            SQL = SQL + ComNum.VBLF + "		NVALUE1 = '" + strNVALUE1 + "',";   //NVALUE1, 
            SQL = SQL + ComNum.VBLF + "		NVALUE2 = '" + strNVALUE2 + "',";   //NVALUE2, 
            SQL = SQL + ComNum.VBLF + "		NVALUE3 = '" + strNVALUE3 + "',";   //NVALUE3, 
            SQL = SQL + ComNum.VBLF + "		UPDATETIME = SYSDATE,"; //UPDATETIME, 
            SQL = SQL + ComNum.VBLF + "		UPIDNUMBER = '" + clsType.User.IdNumber + "',"; //UPIDNUMBER, 
            SQL = SQL + ComNum.VBLF + "		UPIP = '" + clsCompuInfo.gstrCOMIP + "'"; //UPIP
            SQL = SQL + ComNum.VBLF + "WHERE IDNUMBER = '" + clsType.User.IdNumber + "'";
            SQL = SQL + ComNum.VBLF + "     AND OPTIONGB = '" + strOPTIONGB + "'";
            rtnVal = SQL;
            return rtnVal;
        }

        /// <summary>
        /// Query : 사용자 정보를 변경한다.
        /// Author : 박웅규
        /// Create Date : 2017.07.20
        /// </summary>
        /// <param name="TRS">트랜젝션</param>
        /// <param name="strOPTIONGB">사용자 옵션</param>
        /// <param name="strVVALUE1">문자값1</param>
        /// <param name="strVVALUE2">문자값2</param>
        /// <param name="strVVALUE3">문자값3</param>
        /// <param name="strNVALUE1">숫자값1</param>
        /// <param name="strNVALUE2">숫자값2</param>
        /// <param name="strNVALUE3">숫자값3</param>
        /// <returns></returns>
        public static bool UpdateFormUserSet(PsmhDb pDbCon,
                                        string strOPTIONGB, string strVVALUE1, string strVVALUE2, string strVVALUE3,
                                                    string strNVALUE1, string strNVALUE2, string strNVALUE3)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            SQL = ComQuery.Delete_BAS_USEROPTION(strOPTIONGB);
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            SQL = ComQuery.Save_BAS_USEROPTION(strOPTIONGB, strVVALUE1, strVVALUE2, strVVALUE3, strNVALUE1, strNVALUE2, strNVALUE3);
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
            return true;
        }

        /// <summary>
        /// PC세팅 INI 파일 정보
        /// Author : 박웅규
        /// Create Date : 2017.07.27
        /// Modify Date : 2017.08.28
        /// </summary>
        /// <param name="strIP">컴퓨터 IP</param>
        /// <param name="strGRPCDB">대분류</param>
        /// <param name="strGRPCD">분류</param>
        /// <param name="strBASCD">코드</param>
        /// <param name="strCurDate">적용일자</param>
        /// <param name="strUSECLS">사용여부(사용:1, 삭제:0) : ['0', '1']</param>
        /// <returns></returns>
        public static DataTable Select_BAS_PCCONFIG(PsmhDb pDbCon, string strIP, string strGRPCDB, string strGRPCD, string strBASCD = "", string strCurDate = "", string strUSECLS = "'1'")
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (strCurDate == "")
            {
                strCurDate = CurrentDateTime(pDbCon, "D");
            }
            
            SQL = " SELECT  ";
            SQL = SQL + ComNum.VBLF + "    B.GRPCD, B.BASCD, B.REMARK, B.REMARK1, B.VFLAG1, B.DISPSEQ ,  ";
            SQL = SQL + ComNum.VBLF + "    P.VALUEV, P.VALUEN, P.DELGB, P.ROWID";
            SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_PMPA + "BAS_BASCD B ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_PMPA + "BAS_PCCONFIG P ";
            SQL = SQL + ComNum.VBLF + "     ON B.GRPCD = P.GUBUN  ";
            SQL = SQL + ComNum.VBLF + "     AND B.BASCD = P.CODE  ";
            SQL = SQL + ComNum.VBLF + "     AND P.IPADDRESS =  '" + strIP + "'";
            SQL = SQL + ComNum.VBLF + "WHERE B.GRPCDB = '" + strGRPCDB + "'  ";
            SQL = SQL + ComNum.VBLF + "  AND B.GRPCD = '" + strGRPCD + "'  ";
            if (strBASCD != "")
            {
                SQL = SQL + ComNum.VBLF + "  AND B.BASCD = '" + strBASCD + "'  ";
            }
            
            SQL = SQL + ComNum.VBLF + "  AND B.APLFRDATE <= '" + strCurDate + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.APLENDDATE >= '" + strCurDate + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.USECLS IN (" + strUSECLS + ")";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.DISPSEQ ";
            
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        /// <summary>
        /// Description : 메르스 접촉환자 등록
        /// Author : 박병규
        /// Create Date : 2017.06.28
        /// <param name="ArgPtno"></param>
        /// </summary>
        public void UPDATE_BAS_PATIENT_MERS(PsmhDb pDbCon, string ArgPtno)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "    SET GBMERS =  'Y' ";
                SQL += ComNum.VBLF + "  WHERE PANO   = '" + ArgPtno + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return;
                }

                clsDB.setCommitTran(pDbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return;
            }
        }

        /// <summary>
        /// Description : 중증 등록대상자 자동등록
        /// Author : 박병규
        /// Create Date : 2017.06.28
        /// <param name="ArgGubun">
        /// 2 : 의료급여_산정특례(희귀)등록대상자 자동등록
        /// 4 : 조산아 및 저체중아 자동등록
        /// </param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgSname"></param>
        /// <param name="ArgJumin"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgSdate"></param>
        /// <param name="ArgEdate"></param>
        /// <param name="ArgJcode"></param>
        /// </summary>
        public void AUTO_BAS_CANCER_INSERT(PsmhDb pDbCon, string ArgGubun, string ArgPtno, string ArgSname, string ArgJumin, string ArgDept, string ArgSdate, string ArgEdate, string ArgJcode, string ArgIcode, string ArgVcode, string ArgGubun2)
        {
            DataTable DtMst = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            string strAutoRowid = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_CANCER ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND GUBUN = '" + ArgGubun + "' ";
            SQL += ComNum.VBLF + "    AND trim(JBUNHO) = '" + ArgJcode.Trim() + "' ";
            SqlErr = clsDB.GetDataTable(ref DtMst, SQL, pDbCon);

            if (DtMst.Rows.Count > 0)
                strAutoRowid = DtMst.Rows[0]["ROWID"].ToString().Trim();

            DtMst.Dispose();
            DtMst = null;

            clsDB.setBeginTran(pDbCon);

            try
            {

                SQL = "";
                if (strAutoRowid == "")
                {
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_CANCER ";
                    SQL += ComNum.VBLF + "        (PANO, SNAME , SEX, AGE, JBUNHO, FDATE, TDATE,";
                    SQL += ComNum.VBLF + "         DEPT1, Memo, ENTSABUN, GUBUN,ILLCODE1,ILLVCODE1, GUBUN2, Auto_Chk ) ";
                    SQL += ComNum.VBLF + " VALUES ('" + ArgPtno + "',                                       --등록번호";
                    SQL += ComNum.VBLF + "         '" + ArgSname + "',                                      --성명";
                    SQL += ComNum.VBLF + "         '" + ComFunc.SexCheck(VB.Right(ArgJumin, 7), "2") + "',   --성별";
                    SQL += ComNum.VBLF + "         '" + ComFunc.AgeCalc(pDbCon, ArgJumin) + "' ,                    --나이";
                    SQL += ComNum.VBLF + "         '" + ArgJcode + "',                                      --중증등록번호";
                    SQL += ComNum.VBLF + "         TO_DATE('" + ArgSdate + "','YYYY-MM-DD'),                --유효시작일";
                    SQL += ComNum.VBLF + "         TO_DATE('" + ArgEdate + "','YYYY-MM-DD'),                --유효종료일";
                    SQL += ComNum.VBLF + "         '" + ArgDept + "',                                       --과1";
                    SQL += ComNum.VBLF + "         '자동등록',                                              --참고사항";
                    SQL += ComNum.VBLF + "         '" + clsType.User.Sabun + "',                            --입력자";
                    SQL += ComNum.VBLF + "         '" + ArgGubun + "',                                      --구분(1.암, 2.기타 산정특례, 3.중증화상, 4.조산아)";
                    SQL += ComNum.VBLF + "         '" + ArgIcode + "',                                      --상병1";
                    SQL += ComNum.VBLF + "         '" + ArgVcode + "',                                      --V코드(V193:암)";
                    SQL += ComNum.VBLF + "         '" + ArgGubun2 + "',                                     --구분2 (null,0 , 1:결핵,2.희귀, 3.중증난치, 4.중증치매)";
                    SQL += ComNum.VBLF + "         'Y' )                                                    --자동등록";
                }
                else
                {
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_CANCER ";
                    SQL += ComNum.VBLF + "    SET JBUNHO    = '" + ArgJcode + "', ";
                    SQL += ComNum.VBLF + "        FDATE     = TO_DATE('" + ArgSdate + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "        TDATE     = TO_DATE('" + ArgEdate + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "        ENTSABUN  = '" + clsType.User.Sabun + "' ";
                    SQL += ComNum.VBLF + "  WHERE ROWID     = '" + strAutoRowid + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return;
                }

                clsDB.setCommitTran(pDbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return;
            }
        }

        /// <summary>
        /// PC 설정값을 가지고 온다 : BAS_PCCONFIG
        /// dt = ComQuery.GetValue_BAS_PCCONFIG("프로그램PC세팅", "기타PC설정", "자동로그아웃제외");
        /// if (dt == null){}
        /// </summary>
        /// <param name="strGRPCDB">대분류</param>
        /// <param name="strGRPCD">분류</param>
        /// <param name="strBASCD">코드</param>
        /// <param name="strAPLDATE">적용일자</param>
        /// <param name="pTran">트랜젝션</param>
        /// <returns></returns>
        public static DataTable GetValue_BAS_PCCONFIG(PsmhDb pDbCon, string strGRPCDB, string strGRPCD, string strBASCD, string strAPLDATE = "")
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                if (strAPLDATE == "")
                {
                    strAPLDATE = ComQuery.CurrentDateTime(pDbCon, "D");
                }

                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    B.GRPCD, B.BASCD, B.REMARK, B.REMARK1, B.VFLAG1, B.DISPSEQ ,  ";
                SQL = SQL + ComNum.VBLF + "    P.VALUEV, P.VALUEN, P.DELGB, P.ROWID";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_PMPA + "BAS_BASCD B ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_PMPA + "BAS_PCCONFIG P ";
                SQL = SQL + ComNum.VBLF + "     ON B.GRPCD = P.GUBUN  ";
                SQL = SQL + ComNum.VBLF + "     AND B.BASCD = P.CODE  ";
                SQL = SQL + ComNum.VBLF + "     AND P.IPADDRESS =  '" + clsCompuInfo.gstrCOMIP + "'";
                SQL = SQL + ComNum.VBLF + "WHERE B.GRPCDB = '" + strGRPCDB + "'  ";
                SQL = SQL + ComNum.VBLF + "  AND B.GRPCD = '" + strGRPCD + "'  ";
                SQL = SQL + ComNum.VBLF + "  AND B.BASCD = '" + strBASCD + "'  ";
                SQL = SQL + ComNum.VBLF + "  AND B.APLFRDATE <= '" + strAPLDATE + "'";
                SQL = SQL + ComNum.VBLF + "  AND B.APLENDDATE >= '" + strAPLDATE + "'";
                SQL = SQL + ComNum.VBLF + "  AND B.USECLS = '1'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("BAS_BASCD 조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                return dt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return dt;
            }
        }

        /// <summary>
        /// PC 설정값을 변경한다 : BAS_PCCONFIG
        /// </summary>
        /// <param name="strCOMIP">컴퓨터 아이피 : clsCompuInfo.gstrCOMIP</param>
        /// <param name="strGUBUN">분류</param>
        /// <param name="strCODE">코드</param>
        /// <param name="strVALUEV">문자값</param>
        /// <param name="strVALUEN">숫자값</param>
        /// <param name="pTran">트랜젝션</param>
        /// <returns></returns>
        public static bool Update_BAS_PCCONFIG(PsmhDb pDbCon, string strCOMIP, string strGUBUN, string strCODE, string strVALUEV = "", string strVALUEN = "")
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE" + ComNum.DB_PMPA + "BAS_PCCONFIG  SET ";
                if (strVALUEV == "")
                {
                    SQL = SQL + ComNum.VBLF + "     VALUEV  =  '" + "NULL" + "',";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     VALUEV  =  '" + strVALUEV + "',";
                }
                if (strVALUEN == "")
                {
                    SQL = SQL + ComNum.VBLF + "     VALUEN  =  '" + "NULL" + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     VALUEN  =  '" + strVALUEN + "'";
                }
                SQL = SQL + ComNum.VBLF + "WHERE PADDRESS =  '" + strCOMIP + "'";
                SQL = SQL + ComNum.VBLF + "     AND GUBUN  =  '" + strGUBUN + "'";
                SQL = SQL + ComNum.VBLF + "     AND CODE  =  '" + strCODE + "'";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
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
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// 시퀀스를 가지고 온다
        /// </summary>
        /// <param name="FunSeqName"></param>
        /// <returns></returns>
        public static string gGetSequencesNo(PsmhDb pDbCon, string FunSeqName)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            SQL = "";
            SQL = SQL + "SELECT " + FunSeqName + "() FunSeqNo FROM Dual";
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
                ComFunc.MsgBox("시퀀스 조회중 문제가 발생했습니다");
                return "";
            }

            rtnVal = (dt.Rows[0]["FunSeqNo"].ToString() + "").Trim();
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// 시퀀스를 가지고 온다
        /// </summary>
        /// <param name="SeqName">시퀀스이름</param>
        /// <returns></returns>
        public static string gGetSequencesNoEx(PsmhDb pDbCon, string SeqName)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            SQL = "";
            SQL = SQL + "SELECT " + SeqName + ".NextVal AS FunSeqNo FROM Dual";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                if (SeqName == "SEQ_PANO")
                {
                    ComFunc.MsgBox("신환번호 부여오류");
                }
                else
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                }
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                if (SeqName == "SEQ_PANO")
                {
                    ComFunc.MsgBox("신환번호 부여오류");
                }
                else
                {
                    ComFunc.MsgBox("시퀀스 조회중 문제가 발생했습니다");
                }

                return "";
            }

            rtnVal = (dt.Rows[0]["FunSeqNo"].ToString() + "").Trim();
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// 병원정보 세팅
        /// </summary>
        public static void GetHosInfo(PsmhDb pDbCon)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;

            clsType.ClearHosInfo();

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT BASCD, BASNAME FROM MHEMR.AEMRBASCD";
            SQL = SQL + ComNum.VBLF + "  WHERE BSNSCLS = 'HOSPITAL'";
            SQL = SQL + ComNum.VBLF + "  AND UNITCLS = '기본설정'";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            }

            else
            {
                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    switch ((dt.Rows[i]["BASCD"].ToString() + "").Trim().ToUpper())
                    {
                        case "H_ADDR":  //병원주소(한글명)
                            clsType.HosInfo.strAddressKor = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "H_EADDR": //병원주소(영문명)
                            clsType.HosInfo.StrAddressEng = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "H_NAME":    //병원명(한글명)
                            clsType.HosInfo.strNameKor = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "H_ENAME":    //병원명(영문명)
                            clsType.HosInfo.strNameEng = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "H_Email":    //E메일
                            clsType.HosInfo.strEmail = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "H_TEL":    //전화번호
                            clsType.HosInfo.strTel = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "H_FAX":    //FAX번호
                            clsType.HosInfo.strFax = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "H_MANAGER":    //대표자
                            clsType.HosInfo.strManager = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "H_NO":    //요양기관번호
                            clsType.HosInfo.strHospitalNo = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "H_ZIPCODE":    //우편번호
                            clsType.HosInfo.strZipCode = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "H_ZIPSEQ":    //우편번호SEQ
                            clsType.HosInfo.strZipSeq = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "SHEETDAY":    //원외처방전유효기간
                            clsType.HosInfo.strSheetDay = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "H_GUBUN":    //병원구분
                            clsType.HosInfo.strHosCls = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "H_SNO":    //사업자등록번호
                            clsType.HosInfo.strSNo = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "H_CERT":    //EMR 인증사용여부
                            clsType.HosInfo.strEmrCertUseYn = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "H_PROGID":    //청구소프트웨어 업체코드(병원요양기관기호)
                            clsType.HosInfo.strPROGID = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "H_DURID":    //인증코드  IMAGEUSE
                            clsType.HosInfo.strDURID = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "IMAGEUSE":    //이미지 사용 
                            clsType.HosInfo.strIMAGEUSE = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "SPDSKIN":    //스프래드 스킨 
                            clsType.HosInfo.strIMAGEUSE = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "AUTOLOGOUT":    //자동로그아웃
                            clsType.HosInfo.strAutoLogout = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "DREMRDSP":    //진료화면설정(0:사용안함, 1:사용)
                            clsType.HosInfo.strDrEmrDsp = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "CONTYPE":
                            clsType.HosInfo.strConType = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "CERT_DN":
                            clsType.HosInfo.strCertDn = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "CERT_SVR":
                            clsType.HosInfo.strCertSvr = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                        case "CERT_SVRPT":
                            clsType.HosInfo.strCertSvrPt = (dt.Rows[i]["BASNAME"].ToString() + "").Trim();
                            break;
                    }
                }
                dt.Dispose();
                dt = null;
            }
        }


        /// <summary>
        /// INI File을 읽어서 컴퓨터 세팅값에 저장한다.
        /// 작성자 : 박웅규 2018-05-01
        /// ComFunc  CheckINIandSaveConfig 참고
        /// </summary>
        /// <param name="strCODE"></param>
        /// <param name="strVALUEV"></param>
        /// <returns></returns>
        public static bool CheckINIandSaveConfigExcute(string strCODE, string strVALUEV)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            string strGRPCD = "외래OCS진료과세팅";

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

                SQL = "";
                SQL = " SELECT *  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PCCONFIG ";
                SQL = SQL + ComNum.VBLF + "WHERE IPADDRESS = '" + clsCompuInfo.gstrCOMIP + "' ";
                SQL = SQL + ComNum.VBLF + "AND GUBUN = '" + strGRPCD + "' ";
                SQL = SQL + ComNum.VBLF + "AND CODE = '" + strCODE + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_PCCONFIG ";
                    SQL = SQL + ComNum.VBLF + "(IPADDRESS, GUBUN, CODE, VALUEV, VALUEN, DISSEQNO, INPDATE, INPTIME, DELGB)";
                    SQL = SQL + ComNum.VBLF + "VALUES (";
                    SQL = SQL + ComNum.VBLF + "'" + clsCompuInfo.gstrCOMIP + "', -- IPADDRESS";
                    SQL = SQL + ComNum.VBLF + "'" + strGRPCD + "', -- GUBUN";
                    SQL = SQL + ComNum.VBLF + "'" + strCODE + "', -- CODE";
                    SQL = SQL + ComNum.VBLF + "'" + strVALUEV + "', -- VALUEV";
                    SQL = SQL + ComNum.VBLF + "NULL, -- VALUEN";
                    SQL = SQL + ComNum.VBLF + "NULL, -- DISSEQNO";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Left(strCurDateTime, 8) + "', -- INPDATE";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Right(strCurDateTime, 6) + "', -- INPTIME";
                    SQL = SQL + ComNum.VBLF + "'" + "0" + "' -- DELGB";
                    SQL = SQL + ComNum.VBLF + ")";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        public static bool NURSE_System_Manager_Check(double ArgSabun)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;

            //'2014-02-03 주임 김현욱 추가
            //'간호부 관리자 보다 한단계 높은 권한을 주는 부분입니다.
            //'일반적으로 프로그램 변경사항, 전산실 연습 등을 미리 볼수 있는 권한입니다.(예전 고경자 과장 권한 기준)
            //'모든 수간호사가 다 봐야한다면 NUR_MANAGER_CHECK를 이용하시고
            //'좀 더 높은 권한(간호부 과장급)의 경우에 사용하시면 되겠습니다.

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT CODE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_시스템관리자'";
                SQL = SQL + ComNum.VBLF + "   AND Code=" + ArgSabun + " ";
                SQL = SQL + ComNum.VBLF + "      AND DELDATE IS NULL";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    if (VB.Val(dt.Rows[0]["code"].ToString().Trim()) > 0)
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

                    SQL = "";
                    SQL = " SELECT CODE";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_시스템관리자IP'";
                    SQL = SQL + ComNum.VBLF + "   AND Code='" + clsCompuInfo.gstrCOMIP + "' ";
                    SQL = SQL + ComNum.VBLF + "      AND DELDATE IS NULL";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (VB.Val(dt.Rows[0]["code"].ToString().Trim()) > 0)
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

                }

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

        /// <summary>
        /// 폼의 타이틀을 가지고 온다
        /// </summary>
        /// <param name="strFormName"></param>
        /// <returns></returns>
        public static string GetFormTitle(string strFormName)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";
            
            try
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    FORMNAME1 "; 
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_PROJECTFORM  ";
                SQL = SQL + ComNum.VBLF + "WHERE FORMNAME = '" + strFormName + "' ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["FORMNAME1"].ToString().Trim();
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
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

        }

        /// <summary>
        /// 의사코드로 사번을 불러 온다
        /// ** 함부로 사용하지 마시요. **
        /// ** 동일한 코딩이 있을 경우만 사용 바람 **
        /// 2018-11-11 박웅규
        /// 진료OCS에서 사용하기 위해 만듬
        /// </summary>
        /// <param name="strDrCode"></param>
        /// <returns></returns>
        public static string GetDrcodeToSabun(string strDrCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = strDrCode;

            try
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    SABUN ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_DOCTOR  ";
                SQL = SQL + ComNum.VBLF + "WHERE DRCODE = '" + strDrCode + "' ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SABUN"].ToString().Trim();
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
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// 기초코드에서 필요한 값을 불러온다
        /// 2018-11-19 윤조연 & 박웅규
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgJob"></param>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgCode"></param>
        /// <param name="ArgName"></param>
        /// <param name="ArgDelChk"></param>
        /// <returns></returns>
        public static string READ_BCODE_Name_ALL(PsmhDb pDbCon, string ArgJob, string ArgGubun, string ArgCode, string ArgName, bool ArgDelChk)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Code,Name,Gubun2 ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE Gubun='" + ArgGubun + "' ";
                if (ArgCode != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND Code='" + ArgCode + "' ";
                }
                if (ArgName != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND Name='" + ArgName + "' ";
                }
                if (ArgDelChk == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL OR DelDate ='') ";
                }
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("함수명 : " + "CHK_BOHUM_LOWDER_ADM_SONO_CHK" + ComNum.VBLF + "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "CHK_BOHUM_LOWDER_ADM_SONO_CHK" + ComNum.VBLF + SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    switch(ArgJob)
                    {
                        case "00":
                            rtnVal = dt.Rows[0]["Name"].ToString().Trim();
                            break;
                        case "01":
                            rtnVal = dt.Rows[0]["Gubun2"].ToString().Trim();
                            break;
                        default:
                            rtnVal = dt.Rows[0]["Name"].ToString().Trim();
                            break;
                    }
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
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// 자동로그아웃 제외폼을 읽어 배열에 저장한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        public static string[] GetNotLogOutForm(PsmhDb pDbCon)
        {
            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  ";
                //SQL = SQL + ComNum.VBLF + "--   GRPCDB, GRPCD, BASCD, APLFRDATE, APLENDDATE, BASNAME, BASNAME1,  ";
                //SQL = SQL + ComNum.VBLF + "--   VFLAG1, VFLAG2, VFLAG3, VFLAG4, NFLAG1, NFLAG2, NFLAG3, NFLAG4, ";
                //SQL = SQL + ComNum.VBLF + "--   REMARK, REMARK1, INPDATE, INPTIME,  ";
                //SQL = SQL + ComNum.VBLF + "--   USECLS, DISPSEQ ";
                SQL = SQL + ComNum.VBLF + "BASCD";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD ";
                SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = '프로그램기초'";
                SQL = SQL + ComNum.VBLF + "       AND GRPCD = '로그아웃제외폼'";
                SQL = SQL + ComNum.VBLF + "       AND USECLS = '1'";
                SQL = SQL + ComNum.VBLF + "ORDER BY DISPSEQ ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return null;
                }

                string[] srryForm = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    srryForm[i] = dt.Rows[i]["BASCD"].ToString().Trim();
                }
                    
                dt.Dispose();
                dt = null;

                return srryForm;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return null;
            }
        }

        /// <summary>
        /// 입원번호로 환자의 진료과를 불러온다
        /// 2019-02-12 박웅규
        /// </summary>
        /// <param name="strIpdNo"></param>
        /// <param name="strDeptcd"></param>
        /// <returns></returns>
        public static string GetAdmDeptCdFormIPDNO(string strIpdNo, string strDeptcd)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = strDeptcd;

            try
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.IPD_NEW_MASTER  ";
                SQL = SQL + ComNum.VBLF + "WHERE IPDNO = " + strIpdNo ;
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DEPTCODE"].ToString().Trim();
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
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// 프로그램 로그인 기록
        /// </summary>
        /// <returns></returns>
        public static bool EXEInfo_History(string strExeNm)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "Insert Into ADMIN.Bas_EXEInfo";
                SQL = SQL + ComNum.VBLF + "     (SAbun, ChulTime, ExeFile, IPadd)";
                SQL = SQL + ComNum.VBLF + "VALUES (";
                SQL = SQL + ComNum.VBLF + "  '" + clsType.User.IdNumber + "',";
                SQL = SQL + ComNum.VBLF + "  SYSDATE,";
                SQL = SQL + ComNum.VBLF + "  '" + strExeNm + "',";
                SQL = SQL + ComNum.VBLF + "  '" + clsCompuInfo.gstrCOMIP + "'";
                SQL = SQL + ComNum.VBLF + ")";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 사용자 로그인 정보를 저장한다
        /// 2019-02-20 박웅규
        /// </summary>
        /// <param name="strIDNUMBER"></param>
        /// <param name="intLOGINCNT"></param>
        /// <param name="strLOGINYN"></param>
        public static void InsertLoginHis(string strIDNUMBER, int intLOGINCNT, string strLOGINYN, string strLOGINREMARK)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (strIDNUMBER == "") return;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO ADMIN.ETC_USERLOGINHIS";
                SQL = SQL + ComNum.VBLF + "     (IDNUMBER, INPDATETIME, LOGINCNT, LOGINYN, LOGINIP, LOGINREMARK)";
                SQL = SQL + ComNum.VBLF + "VALUES (";
                SQL = SQL + ComNum.VBLF + "  '" + strIDNUMBER + "',";
                SQL = SQL + ComNum.VBLF + "  SYSDATE,";
                SQL = SQL + ComNum.VBLF + "  " + intLOGINCNT + ",";
                SQL = SQL + ComNum.VBLF + "  '" + strLOGINYN + "',";
                SQL = SQL + ComNum.VBLF + "  '" + clsCompuInfo.gstrCOMIP + "',";
                SQL = SQL + ComNum.VBLF + "  '" + strLOGINREMARK + "'";
                SQL = SQL + ComNum.VBLF + ")";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    //ComFunc.MsgBox("저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        /// <summary>
        /// DB변경권한 PC체크
        /// </summary>
        /// <returns></returns>
        public static bool CheckDbChangeAuth()
        {
            bool rtnVal = false;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {                                
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT NFLAG1 FROM ADMIN.BAS_BASCD ";
                SQL = SQL + ComNum.VBLF + " WHERE GRPCDB = '프로그램기초' ";
                SQL = SQL + ComNum.VBLF + "   AND GRPCD = '프로그램중지' ";
                SQL = SQL + ComNum.VBLF + "   AND BASCD = '개발DB사용여부' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return false;                    
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["NFLAG1"].ToString().Trim() != "1")
                    {
                        dt.Dispose();
                        dt = null;
                        return false;
                    }
                }
                dt.Dispose();
                dt = null;

                string strFile = @"C:\HealthSoft\HSMain\psmhDev.ini";
                FileInfo fileInfo = new FileInfo(strFile);
                if (fileInfo.Exists)
                {
                    rtnVal = true;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }
    }
}
