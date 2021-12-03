using ComDbB; //DB연결
using System;
using System.Data;
using System.Windows.Forms;

namespace ComBase
{
    /// <summary>
    /// OCS, EMR 관련 함수
    /// </summary>
    public class clsQuery : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// BAS_BGRPCD 리스트 조회
        /// </summary>
        /// <param name="strGRPCDB">부모코드</param>
        /// <returns></returns>
        public static string SQL_BAS_BGRPCD_LIST(string strGRPCDB)
        {
            string SQL = "";
            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "   M.GRPCDB, M.GRPCD, M.APLFRDATE, M.APLENDDATE, M.GROUPNAME, M.GROUPNAME1, M.REMARK, M.REMARK1, M.USECLS, M.DISPSEQ ";
            SQL = SQL + ComNum.VBLF + "   , (SELECT COUNT(M1.GRPCD) FROM " + ComNum.DB_PMPA + "BAS_BGRPCD M1";
            SQL = SQL + ComNum.VBLF + "         WHERE M1.GRPCDB = M.GRPCD)  AS CCONT";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BGRPCD M";
            SQL = SQL + ComNum.VBLF + "WHERE M.GRPCDB = '" + strGRPCDB.Replace("GG_", "") + "'";
            //SQL = SQL + ComNum.VBLF + "     AND M.APLFRDATE <= '" + strCurDate + "'";
            //SQL = SQL + ComNum.VBLF + "     AND M.APLENDDATE >= '" + strCurDate + "'";
            //SQL = SQL + ComNum.VBLF + "     AND M.USECLS >= '1'";
            SQL = SQL + ComNum.VBLF + "ORDER BY M.DISPSEQ";

            return SQL;
        }

        /// <summary>
        /// 기초코드를 가지고 온다
        /// </summary>
        /// <param name="strGRPCDB">대분류</param>
        /// <param name="strGRPCD">소분류</param>
        /// <param name="strUSECLS">적용일자( '0' / '1' / ('0','1') </param>
        /// <param name="strAPLDATE">적용일자</param>
        /// <param name="strSubSql">서브쿼리</param>
        /// <param name="strOrderBy">정렬</param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns></returns>
        public static DataTable DT_BAS_BASCD_List(PsmhDb pDbCon, string strGRPCDB, string strGRPCD, string strUSECLS = "'1'", string strAPLDATE = "", string strSubSql = "", string strOrderBy = "")
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";

            if (strAPLDATE == "")
            {
                strAPLDATE = ComQuery.CurrentDateTime(pDbCon, "D");
            }

            if (strGRPCD != "")
            {
                SQL = SQL + ComNum.VBLF + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "   GRPCDB, GRPCD, BASCD, APLFRDATE, APLENDDATE, BASNAME, BASNAME1,  ";
                SQL = SQL + ComNum.VBLF + "   VFLAG1, VFLAG2, VFLAG3, VFLAG4, NFLAG1, NFLAG2, NFLAG3, NFLAG4, ";
                SQL = SQL + ComNum.VBLF + "   REMARK, REMARK1, INPDATE, INPTIME,  ";
                SQL = SQL + ComNum.VBLF + "   USECLS, DISPSEQ ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD ";
                SQL = SQL + ComNum.VBLF + "  WHERE GRPCDB = '" + strGRPCDB + "'";
                SQL = SQL + ComNum.VBLF + "       AND GRPCD = '" + strGRPCD + "'";

                if (strAPLDATE.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "       AND  '" + strAPLDATE + "' BETWEEN APLFRDATE AND APLENDDATE ";
                }

                if (strUSECLS != "")
                {
                    SQL = SQL + ComNum.VBLF + "       AND USECLS IN " + strUSECLS + " ";
                }

                if (strSubSql != "")
                {
                    SQL = SQL + ComNum.VBLF + strSubSql;
                }
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "   GRPCDB, GRPCD, BASCD, APLFRDATE, APLENDDATE, BASNAME, BASNAME1,  ";
                SQL = SQL + ComNum.VBLF + "   VFLAG1, VFLAG2, VFLAG3, VFLAG4, NFLAG1, NFLAG2, NFLAG3, NFLAG4, ";
                SQL = SQL + ComNum.VBLF + "   REMARK, REMARK1, INPDATE, INPTIME,  ";
                SQL = SQL + ComNum.VBLF + "   USECLS, DISPSEQ ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD ";
                SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = '" + strGRPCDB + "'";

                if (strAPLDATE.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "       AND  '" + strAPLDATE + "' BETWEEN APLFRDATE AND APLENDDATE ";
                }

                if (strUSECLS != "")
                {
                    SQL = SQL + ComNum.VBLF + "       AND USECLS IN " + strUSECLS + " ";
                }

                if (strSubSql != "")
                {
                    SQL = SQL + ComNum.VBLF + strSubSql;
                }
            }

            if (strOrderBy == "")
            {
                SQL = SQL + ComNum.VBLF + "  ORDER BY DISPSEQ ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "  ORDER BY " + strOrderBy;
            }

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("BAS_BASCD 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            }
            return dt;
        }

        /// <summary>
        /// 그룹코드로 BASCD를 조회 한다
        /// </summary>
        /// <param name="strGRPCD"></param>
        /// <param name="pTran"></param>
        /// <returns></returns>
        public static string SQL_BAS_BASCD_BY_GRPCD(PsmhDb pDbCon, string strGRPCD)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            string strGRPCDB = "";

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "   M.GRPCDB ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BGRPCD M";
            SQL = SQL + ComNum.VBLF + "WHERE M.GRPCD = '" + strGRPCD.Replace("GG_", "") + "'";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return "";
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return "";
            }
            strGRPCDB = dt.Rows[0]["GRPCDB"].ToString().Trim();
            dt.Dispose();
            dt = null;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + " GRPCDB, GRPCD, BASCD, APLFRDATE, APLENDDATE, BASNAME, BASNAME1,  ";
            SQL = SQL + ComNum.VBLF + "   VFLAG1, VFLAG2, VFLAG3, VFLAG4, NFLAG1, NFLAG2, NFLAG3, NFLAG4, ";
            SQL = SQL + ComNum.VBLF + "   REMARK, REMARK1, INPDATE, INPTIME,  ";
            SQL = SQL + ComNum.VBLF + "   USECLS, DISPSEQ ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD ";
            SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = '" + strGRPCDB.Replace("GG_", "") + "'";
            SQL = SQL + ComNum.VBLF + "     AND GRPCD = '" + strGRPCD.Replace("GG_", "") + "'";
            //SQL = SQL + ComNum.VBLF + "     AND APLFRDATE <= '" + strCurDate + "'";
            //SQL = SQL + ComNum.VBLF + "     AND APLENDDATE >= '" + strCurDate + "'";
            //SQL = SQL + ComNum.VBLF + "     AND USECLS >= '1'";
            SQL = SQL + ComNum.VBLF + "ORDER BY DISPSEQ";

            return SQL;
        }


        /// <summary>
        /// 기초코드 명을 가지고 온다
        /// </summary>
        /// <param name="strGRPCDB"></param>
        /// <param name="strGRPCD"></param>
        /// <param name="strBASCD"></param>
        /// <param name="pTran">트렌젝션</param>
        /// <returns></returns>
        public static DataTable DT_BAS_BASCD_ONE(PsmhDb pDbCon, string strGRPCDB, string strGRPCD, string strBASCD)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "   GRPCDB, GRPCD, BASCD, APLFRDATE, APLENDDATE, BASNAME, BASNAME1,  ";
            SQL = SQL + ComNum.VBLF + "   VFLAG1, VFLAG2, VFLAG3, VFLAG4, NFLAG1, NFLAG2, NFLAG3, NFLAG4, ";
            SQL = SQL + ComNum.VBLF + "   REMARK, REMARK1, INPDATE, INPTIME,  ";
            SQL = SQL + ComNum.VBLF + "   USECLS, DISPSEQ ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD ";
            SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = '" + strGRPCDB + "'";
            SQL = SQL + ComNum.VBLF + "       AND GRPCD = '" + strGRPCD + "'";
            SQL = SQL + ComNum.VBLF + "       AND BASCD = '" + strBASCD + "'";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("BAS_BASCD 조회중 문제가 발생했습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            }
            return dt;
        }

        /// <summary>
        /// 골다공증약제 365일수 체크  구분:null-전체,"1"-체크
        /// </summary>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgBDate"></param>
        /// <param name="ArgSuCode"></param>
        /// <returns></returns>
        public static long READ_BONE_ILSU_CHK(PsmhDb pDbCon, string ArgGubun, string ArgPano, string ArgBDate, string ArgSuCode, string strSysDate = "")
        {
            long rtnVal = 0;
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strSysDate == "")
            {
                strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D");
            }

            if (ArgGubun == "1")
            {
                rtnVal = 1;
                return rtnVal;
            }

            if (ArgGubun != "2")
            {
                SQL = " SELECT CODE FROM ADMIN.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='OCS_골다공증약제' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(CODE) ='" + ArgSuCode + "' ";
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
                    return rtnVal;
                }
                dt.Dispose();
                dt = null;
            }

            SQL = " SELECT a.ptno,a.bdate,a.sucode,b.sname,sum(a.qty*a.nal) qnal ";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.OCS_OORDER a, ADMIN.bas_patient b";
            SQL = SQL + ComNum.VBLF + "  Where a.PtNo = b.Pano(+)";
            SQL = SQL + ComNum.VBLF + "  and a.ptno ='" + ArgPano + "' ";
            SQL = SQL + ComNum.VBLF + "  and a.bdate>=to_date('2011-10-01','yyyy-mm-dd')";
            SQL = SQL + ComNum.VBLF + "  and a.bdate<=to_date('" + strSysDate + "','yyyy-mm-dd')"; //GstrSysDate
            SQL = SQL + ComNum.VBLF + "  AND TRIM(a.SUCODE) IN ( SELECT CODE FROM ADMIN.BAS_BCODE";
            SQL = SQL + ComNum.VBLF + "                           WHERE GUBUN ='OCS_골다공증약제')";
            SQL = SQL + ComNum.VBLF + "  and (a.res is null or a.res <> '1')";
            SQL = SQL + ComNum.VBLF + "  and (a.auto_Send is null or a.auto_Send <> '1')";
            SQL = SQL + ComNum.VBLF + "  AND a.GBSUNAP ='1'";
            SQL = SQL + ComNum.VBLF + "  AND (a.GBSELF ='0' OR a.GBSELF IS NULL ) ";
            SQL = SQL + ComNum.VBLF + "  group by a.ptno,a.bdate,a.sucode,b.sname";
            SQL = SQL + ComNum.VBLF + "  Having Sum(a.qty * a.nal) > 0";
            SQL = SQL + ComNum.VBLF + "  Union All";
            SQL = SQL + ComNum.VBLF + "  SELECT a.ptno,a.bdate,a.sucode,b.sname,sum(a.qty*a.nal) qnal ";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.OCS_iORDER a, ADMIN.bas_patient b";
            SQL = SQL + ComNum.VBLF + "  Where a.PtNo = b.Pano(+)";
            SQL = SQL + ComNum.VBLF + "  and a.ptno ='" + ArgPano + "' ";
            SQL = SQL + ComNum.VBLF + "  and a.bdate>=to_date('2011-10-01','yyyy-mm-dd')";
            SQL = SQL + ComNum.VBLF + "  and a.bdate<=to_date('" + strSysDate + "','yyyy-mm-dd')";
            SQL = SQL + ComNum.VBLF + "  AND TRIM(a.SUCODE) IN ( SELECT CODE FROM ADMIN.BAS_BCODE";
            SQL = SQL + ComNum.VBLF + "                           WHERE GUBUN ='OCS_골다공증약제')";
            SQL = SQL + ComNum.VBLF + "  AND a.GbSend =' '";
            SQL = SQL + ComNum.VBLF + "  AND a.GbPRN =' '";    //'2012-11-14
            SQL = SQL + ComNum.VBLF + "  AND a.pickupDate is not null";
            SQL = SQL + ComNum.VBLF + "  AND (a.GBSELF ='0' OR a.GBSELF IS NULL ) ";
            SQL = SQL + ComNum.VBLF + "  group by a.ptno,a.bdate,a.sucode,b.sname";
            SQL = SQL + ComNum.VBLF + "  Having Sum(a.qty * a.nal) > 0";
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
                    rtnVal = rtnVal + (long)((long)VB.Val(dt.Rows[i]["qnal"].ToString().Trim()) * READ_BONE_Sucode2_Qty(pDbCon, dt.Rows[i]["SUCODE"].ToString().Trim()));
                }
            }
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// 골다공증 코드별 일수계산 2012-11-01
        /// </summary>
        /// <param name="ArgSuCode"></param>
        /// <returns></returns>
        public static long READ_BONE_Sucode2_Qty(PsmhDb pDbCon, string ArgSuCode)
        {
            long rtnVal = 1;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SQL = " SELECT NAME ";
            SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_BCODE ";
            SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='OCS_골다공증약제' ";
            SQL = SQL + ComNum.VBLF + "   AND TRIM(CODE) ='" + ArgSuCode.Trim() + "' ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                rtnVal = (long)VB.Val(dt.Rows[0]["NAME"].ToString().Trim());
            }
            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        /// <summary>
        /// 골다공증 마스터 체크 생성
        /// </summary>
        /// <param name="ArgPano"></param>
        /// <param name="ArgSName"></param>
        /// <param name="ArgBDate"></param>
        /// <param name="ArgRemark"></param>
        /// <param name="JobSabun"></param>
        /// <returns></returns>
        public static string READ_BONE_ILSU_MST(PsmhDb pDbCon, string ArgPano, string ArgSName, string ArgBDate, string ArgRemark, string JobSabun)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수


            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = " ";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID FROM ADMIN.BAS_BONE_MST";
                SQL = SQL + ComNum.VBLF + "WHERE PANO ='" + ArgPano + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsDB.setRollbackTran(pDbCon);
                    return "";
                }

                if (dt.Rows.Count == 0)
                {
                    SQL = " INSERT INTO ADMIN.BAS_BONE_MST(PANO,SNAME,SDATE,ENTDATE,ENTDATE2,ENTSABUN,REMARK) VALUES (";
                    SQL = SQL + ComNum.VBLF + " '" + ArgPano + "','" + ArgSName + "', ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + ArgBDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " SYSDATE,SYSDATE," + JobSabun + ",'" + ArgRemark + "')  "; //GnJobSabun 
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return "";
                    }
                }
                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(pDbCon);

                return "";
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 골다공증 마스터 읽기
        /// </summary>
        /// <param name="ArgPano"></param>
        /// <returns></returns>
        public static string READ_BONE_MST(PsmhDb pDbCon, string ArgPano)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SQL = " ";
            SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE FROM ADMIN.BAS_BONE_MST";
            SQL = SQL + ComNum.VBLF + "  WHERE WHERE PANO ='" + ArgPano + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["SDate"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        /// <summary>
        /// PC 환경값을 가지고 온다
        /// </summary>
        public static void READ_PC_CONFIG(PsmhDb pDbCon)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            clsType.PC_CONFIG.IPAddress = clsCompuInfo.gstrCOMIP;
            clsType.PC_CONFIG.BuseGbn = "";
            clsType.PC_CONFIG.WardCode = "";
            clsType.PC_CONFIG.DeptCode = "";
            clsType.PC_CONFIG.DrCode = "";
            clsType.PC_CONFIG.BuCode = "";
            clsType.PC_CONFIG.PacsSW = "WEB1000";
            clsType.PC_CONFIG.CrtSize = "";
            clsType.PC_CONFIG.Job = "";
            clsType.PC_CONFIG.PcUserYN = "";
            clsType.PC_CONFIG.PacsID = "";
            clsType.PC_CONFIG.PacsPass = "";
            clsType.PC_CONFIG.Remark = "";
            clsType.PC_CONFIG.OS_Ver = "";
            clsType.PC_CONFIG.BarCode = "";
            clsType.PC_CONFIG.nx = 0;
            clsType.PC_CONFIG.nY = 0;
            clsType.PC_CONFIG.GX420D = "0";
            clsType.PC_CONFIG.GX420D_X = 0;
            clsType.PC_CONFIG.GX420D_Y = 0;

            //DB에서 PC 설정값을 READ
            SQL = "SELECT BuseGbn,WardCode,DeptCode,DrCode,BuCode,OS_Ver,PacsSW,CrtSize,";
            SQL = SQL + ComNum.VBLF + " Job,PcUserYN,PacsID,PacsPass,Remark, BarCode, nX, nY, GX420D, GX420D_X, GX420D_Y ";
            SQL = SQL + ComNum.VBLF + " FROM ADMIN.ETC_PCCONFIG ";
            SQL = SQL + ComNum.VBLF + "WHERE IpAddress='" + clsType.PC_CONFIG.IPAddress + "' ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                clsType.PC_CONFIG.BuseGbn = dt.Rows[0]["BuseGbn"].ToString().Trim();
                clsType.PC_CONFIG.WardCode = dt.Rows[0]["WardCode"].ToString().Trim();
                clsType.PC_CONFIG.DeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                clsType.PC_CONFIG.DrCode = dt.Rows[0]["DrCode"].ToString().Trim();
                clsType.PC_CONFIG.BuCode = dt.Rows[0]["BuCode"].ToString().Trim();
                clsType.PC_CONFIG.PacsSW = dt.Rows[0]["PacsSW"].ToString().Trim();
                clsType.PC_CONFIG.CrtSize = dt.Rows[0]["CrtSize"].ToString().Trim();
                clsType.PC_CONFIG.Job = dt.Rows[0]["Job"].ToString().Trim();
                clsType.PC_CONFIG.PcUserYN = dt.Rows[0]["PcUserYN"].ToString().Trim();
                clsType.PC_CONFIG.PacsID = dt.Rows[0]["PacsID"].ToString().Trim();
                clsType.PC_CONFIG.PacsPass = dt.Rows[0]["PacsPass"].ToString().Trim();
                clsType.PC_CONFIG.Remark = dt.Rows[0]["Remark"].ToString().Trim();
                clsType.PC_CONFIG.OS_Ver = dt.Rows[0]["OS_Ver"].ToString().Trim();
                clsType.PC_CONFIG.BarCode = dt.Rows[0]["BarCode"].ToString().Trim();
                clsType.PC_CONFIG.nx = (int)VB.Val(dt.Rows[0]["nx"].ToString().Trim());
                clsType.PC_CONFIG.nY = (int)VB.Val(dt.Rows[0]["nY"].ToString().Trim());
                clsType.PC_CONFIG.GX420D = dt.Rows[0]["GX420D"].ToString().Trim();
                clsType.PC_CONFIG.GX420D_X = (int)VB.Val(dt.Rows[0]["GX420D_X"].ToString().Trim());
                clsType.PC_CONFIG.GX420D_Y = (int)VB.Val(dt.Rows[0]["GX420D_Y"].ToString().Trim());
            }
            dt.Dispose();
            dt = null;

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
            SQL += ComNum.VBLF + "        SORT, PART, CNT, GUBUN2, ROWID";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1";
            SQL += ComNum.VBLF + "    AND GUBUN = '" + strGubun + "'";
            if (strCode != "")
            {
                SQL += ComNum.VBLF + "AND CODE = '" + strCode + "'";

            }
            SQL += ComNum.VBLF + "  ORDER BY CODE";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("BAS_BCODE 조회중 문제가 발생했습니다." + ComNum.VBLF + ComNum.VBLF + SQL);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        /// <summary>
        /// BAS_BCODE TABLE DATA를 가지고 온다,
        /// 조회시 컬럼 재설정, 정렬부분 재설정 
        /// <param name="strGubun"></param>
        /// <param name="strCode"></param>
        /// <param name="SelQuery"></param>
        /// <param name="Orderby"></param>
        /// <returns></returns>
        /// 2017-07-03 윤조연
        /// </summary>
        public DataTable Get_BasBcode(PsmhDb pDbCon, string strGubun, string strCode, string SelQuery = "", string AndQuery = "", string Orderby = "")
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                     \r\n";
            if (SelQuery == "")
            {
                SQL += "        GUBUN, CODE, NAME,                              \r\n";
                SQL += "        TO_CHAR(JDATE, 'YYYY-MM-DD') JDATE,             \r\n";
                SQL += "        TO_CHAR(DELDATE, 'YYYY-MM-DD') DELDATE,         \r\n";
                SQL += "        TO_CHAR(ENTDATE, 'YYYY-MM-DD') ENTDATE,         \r\n";
                SQL += "        ENTSABUN,SORT, PART, CNT, GUBUN2, GUBUN3        \r\n";
            }
            else
            {
                SQL += "  " + SelQuery + "                                      \r\n";
            }

            SQL += "        , ROWID                                             \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BCODE                     \r\n";
            SQL += "  WHERE 1 = 1                                               \r\n";
            SQL += "    AND GUBUN = '" + strGubun + "'                          \r\n";
            SQL += "    AND (DelDate IS NULL OR DelDate ='')                    \r\n"; //삭제건 제외

            if (AndQuery != "")
            {
                SQL += "  " + AndQuery + "                                      \r\n";
            }
            if (strCode != "")
            {
                SQL += "AND CODE = '" + strCode + "'                            \r\n";
            }
            if (Orderby == "")
            {
                SQL += "  ORDER BY CODE                                         \r\n";
            }
            else
            {
                SQL += "  ORDER BY " + Orderby + "                              \r\n";
            }

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("BAS_BCODE 조회중 문제가 발생했습니다." + SQL);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        public static void READ_EDI_SUGA(PsmhDb pDbCon, string ArgCode, string argSUNEXT = "")
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "      SELECT ROWID VROWID,CODE VCODE,JONG VJONG,";
                SQL = SQL + "    PNAME VPNAME,BUN VBUN,DANWI1 VDANWI1,";
                SQL = SQL + "    DANWI2 VDANWI2,SPEC VSPEC,COMPNY VCOMPNY,";
                SQL = SQL + "    EFFECT VEFFECT,GUBUN VGUBUN,DANGN VDANGN,";
                SQL = SQL + "    TO_CHAR(JDATE1,'YYYY-MM-DD') VJDATE1,PRICE1 VPRICE1,";
                SQL = SQL + "    TO_CHAR(JDATE2,'YYYY-MM-DD') VJDATE2,PRICE2 VPRICE2,";
                SQL = SQL + "    TO_CHAR(JDATE3,'YYYY-MM-DD') VJDATE3,PRICE3 VPRICE3,";
                SQL = SQL + "    TO_CHAR(JDATE4,'YYYY-MM-DD') VJDATE4,PRICE4 VPRICE4,";
                SQL = SQL + "    TO_CHAR(JDATE5,'YYYY-MM-DD') VJDATE5,PRICE5 VPRICE5 ";
                SQL = SQL + " FROM ADMIN.EDI_SUGA ";
                SQL = SQL + "WHERE CODE = '" + ArgCode.Trim() + "' ";

                //'표준코드 30050010이 산소,실구입재료 2개가 존재함

                if (ArgCode.Trim() == "30050010" || ArgCode.Trim() == "J5010001")
                {
                    if (argSUNEXT == "30050010" || argSUNEXT == "J5010001")
                    {
                        SQL = SQL + " AND JONG='8' ";
                    }
                    else
                    {
                        SQL = SQL + " AND JONG<>'8' ";
                    }
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 1)
                {
                    clsType.TES.ROWID = dt.Rows[0]["VROWID"].ToString().Trim();
                    clsType.TES.Code = dt.Rows[0]["VCODE"].ToString().Trim();
                    clsType.TES.Jong = dt.Rows[0]["VJONG"].ToString().Trim();
                    clsType.TES.PName = dt.Rows[0]["VPNAME"].ToString().Trim();
                    clsType.TES.Bun = dt.Rows[0]["VBUN"].ToString().Trim();
                    clsType.TES.Danwi1 = dt.Rows[0]["VDANWI1"].ToString().Trim();
                    clsType.TES.Danwi2 = dt.Rows[0]["VDANWI2"].ToString().Trim();
                    clsType.TES.Spec = dt.Rows[0]["VSPEC"].ToString().Trim();
                    clsType.TES.COMPNY = dt.Rows[0]["VCOMPNY"].ToString().Trim();
                    clsType.TES.Effect = dt.Rows[0]["VEFFECT"].ToString().Trim();
                    clsType.TES.Gubun = dt.Rows[0]["VGUBUN"].ToString().Trim();
                    clsType.TES.Dangn = dt.Rows[0]["VDANGN"].ToString().Trim();
                    clsType.TES.JDate1 = dt.Rows[0]["VJDATE1"].ToString().Trim();
                    clsType.TES.Price1 = VB.Val(dt.Rows[0]["VPRICE1"].ToString().Trim());
                    clsType.TES.JDate2 = dt.Rows[0]["VJDATE2"].ToString().Trim();
                    clsType.TES.Price2 = VB.Val(dt.Rows[0]["VPRICE2"].ToString().Trim());
                    clsType.TES.JDate3 = dt.Rows[0]["VJDATE3"].ToString().Trim();
                    clsType.TES.Price3 = VB.Val(dt.Rows[0]["VPRICE3"].ToString().Trim());
                    clsType.TES.JDate4 = dt.Rows[0]["VJDATE4"].ToString().Trim();
                    clsType.TES.Price4 = VB.Val(dt.Rows[0]["VPRICE4"].ToString().Trim());
                    clsType.TES.JDate5 = dt.Rows[0]["VJDATE5"].ToString().Trim();
                    clsType.TES.Price5 = VB.Val(dt.Rows[0]["VPRICE5"].ToString().Trim());
                }
                else
                {
                    clsType.TES.ROWID = "";
                    clsType.TES.Code = "";
                    clsType.TES.Jong = "";
                    clsType.TES.PName = "";
                    clsType.TES.Bun = "";
                    clsType.TES.Danwi1 = "";
                    clsType.TES.Danwi2 = "";
                    clsType.TES.Spec = "";
                    clsType.TES.COMPNY = "";
                    clsType.TES.Effect = "";
                    clsType.TES.Gubun = "";
                    clsType.TES.Dangn = "";
                    clsType.TES.JDate1 = "";
                    clsType.TES.Price1 = 0;
                    clsType.TES.JDate2 = "";
                    clsType.TES.Price2 = 0;
                    clsType.TES.JDate3 = "";
                    clsType.TES.Price3 = 0;
                    clsType.TES.JDate4 = "";
                    clsType.TES.Price4 = 0;
                    clsType.TES.JDate5 = "";
                    clsType.TES.Price5 = 0;
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
        }

        /// <summary>
        /// 진료 및 EMR 관련 사용자 정의를 가지고 온다.
        /// </summary>
        /// <param name="strUseId"></param>
        /// <param name="strOPTCD"></param>
        /// <param name="strOPTGB"></param>
        /// <returns></returns>
        public static DataTable GetEmrUserOption(PsmhDb pDbCon, string strUseId, string strOPTCD, string strOPTGB)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string strAplDate = "";

            strAplDate = ComQuery.CurrentDateTime(pDbCon,"D");

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT * FROM";
            SQL = SQL + ComNum.VBLF + "    " + ComNum.DB_EMR + "ABUSEROPTION";
            SQL = SQL + ComNum.VBLF + "  WHERE USEID = '" + strUseId + "'";
            SQL = SQL + ComNum.VBLF + "  AND OPTCD = '" + strOPTCD + "'";
            SQL = SQL + ComNum.VBLF + "  AND OPTGB = '" + strOPTGB + "'";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        /// <summary>
        /// 진료 및 EMR 관련 사용자 정의를 저장한다.
        /// </summary>
        /// <param name="strUseId"></param>
        /// <param name="strOPTCD"></param>
        /// <param name="strOPTGB"></param>
        /// <param name="strOPTVALUE"></param>
        /// <returns></returns>
        public static bool SetEmrUserOption(PsmhDb pDbCon, string strUseId, string strOPTCD, string strOPTGB, string strOPTVALUE)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strCurDateTime = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);
            try
            {
                strCurDateTime = ComQuery.CurrentDateTime(pDbCon,"A");

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "ABUSEROPTION";
                SQL = SQL + ComNum.VBLF + "  WHERE USEID = '" + strUseId + "'";
                SQL = SQL + ComNum.VBLF + "  AND OPTCD = '" + strOPTCD + "'";
                SQL = SQL + ComNum.VBLF + "  AND OPTGB = '" + strOPTGB + "'";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "ABUSEROPTION";
                SQL = SQL + ComNum.VBLF + "    (USEID, OPTCD, OPTGB, OPTVALUE, WRITEDATE, WRITETIME)";
                SQL = SQL + ComNum.VBLF + "  VALUES(";
                SQL = SQL + ComNum.VBLF + "      '" + strUseId + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTCD + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTGB + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTVALUE + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(pDbCon);
                //MessageBox.Show(new Form() { TopMost = true }, "저장하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// EMR 사용자 환경설정
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strUseId"></param>
        /// <param name="strOPTCD"></param>
        /// <param name="strOPTGB"></param>
        /// <param name="strOPTVALUE"></param>
        /// <param name="strVALUE"></param>
        /// <returns></returns>
        public static bool SetEmrUserOption(PsmhDb pDbCon, string strUseId, string strOPTCD, string strOPTGB, string strOPTVALUE, string strVALUE)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strCurDateTime = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);
            try
            {
                strCurDateTime = ComQuery.CurrentDateTime(pDbCon,"A");

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "ABUSEROPTION";
                SQL = SQL + ComNum.VBLF + "  WHERE USEID = '" + strUseId + "'";
                SQL = SQL + ComNum.VBLF + "  AND OPTCD = '" + strOPTCD + "'";
                SQL = SQL + ComNum.VBLF + "  AND OPTGB = '" + strOPTGB + "'";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "ABUSEROPTION";
                SQL = SQL + ComNum.VBLF + "    (USEID, OPTCD, OPTGB, OPTVALUE, VALUE, WRITEDATE, WRITETIME)";
                SQL = SQL + ComNum.VBLF + "  VALUES(";
                SQL = SQL + ComNum.VBLF + "      '" + strUseId + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTCD + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTGB + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTVALUE + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strVALUE + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// Description : 후불대상자자동등록
        /// Author : 박웅규
        /// Create Date : 2018.04.09
        /// GbTextEmrCon.bas NEW_TextEMR_TreatInterface
        public static bool NEW_TextEMR_TreatInterface(PsmhDb pDbCon, string ArgPatid, string ArgBDate, string ArgDeptCode, string ArgGubun, string ArgSTS, string ArgDrCode)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strJumin = "";
            string strDept = "";
            string strOK = "";

            ArgBDate = VB.Replace(ArgBDate, "-", "");

            SQL = " SELECT P.PANO, P.SNAME, P.SEX, P.JUMIN1,P.JUMIN2,P.JUMIN3, E.PATID , E.ROWID ";
            SQL = SQL + ComNum.VBLF + "   FROM ADMIN.BAS_PATIENT  P , ADMIN.EMR_PATIENTT E" ;
            SQL = SQL + ComNum.VBLF + " WHERE E.PATID (+)=P.PANO AND " ;
            SQL = SQL + ComNum.VBLF + "  P.PANO ='" + ArgPatid +  "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

            string strOutDate = ComQuery.CurrentDateTime(pDbCon, "D");

            if (ArgSTS == "취소")
            {
                dt.Dispose();
                dt = null;

                if (ArgGubun == "HR" || ArgGubun == "TO" )
                {
                    SQL = "SELECT TREATNO, ROWID  FROM ADMIN.EMR_TREATT ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PATID = '" + ArgPatid + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND INDATE  ='" + ArgBDate + "'";
                    SQL = SQL + ComNum.VBLF + "    AND CLINCODE = '" + ArgDeptCode + "'";
                    SQL = SQL + ComNum.VBLF + "    AND CLASS = 'O' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            SQL = " UPDATE ADMIN.EMR_TREATT SET ";
                            SQL = SQL + ComNum.VBLF + "  DELDATE = '" + strOutDate + "'"; // '2009-09-07 윤조연 수정
                            SQL = SQL + ComNum.VBLF + "   WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    //외래,입원
                    SQL = "SELECT TREATNO, ROWID  FROM ADMIN.EMR_TREATT ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PATID = '" + ArgPatid + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND INDATE  ='" + ArgBDate + "'";
                    if (ArgDeptCode == "MD" && (ArgDrCode == "1107" || ArgDrCode == "1125"))  //내과 오동호 과장은 RA로 2009-09-17 윤조연
                    {
                        SQL = SQL + ComNum.VBLF + "    AND CLINCODE = 'RA'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND CLINCODE = '" + ArgDeptCode + "'";
                    }
                    if (ArgGubun == "외래")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND CLASS = 'O' "; //'외래
                    }
                    else if (ArgGubun == "입원")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND CLASS = 'I' "; //'외래
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND CLASS = '' ";
                    }
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            SQL = " UPDATE ADMIN.EMR_TREATT SET ";
                            SQL = SQL + ComNum.VBLF + "  DELDATE = '" + strOutDate + "'"; // '2009-09-07 윤조연 수정
                            SQL = SQL + ComNum.VBLF + "   WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }

                return true;
            }

            if (dt.Rows[0]["PATID"].ToString().Trim() == "") //EMR_PATIENTT 테이블에 환자가 없다.
            {
                strJumin = dt.Rows[0]["Jumin1"].ToString().Trim() + VB.Left(dt.Rows[0]["Jumin2"].ToString().Trim(),1) + "******";

                SQL = "INSERT INTO ADMIN.EMR_PATIENTT(PATID, JUMINNO, NAME, SEX) " + " ";
                SQL = SQL + ComNum.VBLF + " VALUES('" + dt.Rows[0]["Pano"].ToString().Trim() + "' ,";
                SQL = SQL + ComNum.VBLF + " '" + strJumin + "', ";
                SQL = SQL + ComNum.VBLF + " '" + dt.Rows[0]["sName"].ToString().Trim() + "', ";
                SQL = SQL + ComNum.VBLF + " '" + dt.Rows[0]["Sex"].ToString().Trim() + "') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
            }
            else
            {
                strJumin = dt.Rows[0]["Jumin1"].ToString().Trim() + VB.Left(dt.Rows[0]["Jumin2"].ToString().Trim(), 1) + "******";

                SQL = "UPDATE ADMIN.EMR_PATIENTT" ;
                SQL = SQL + ComNum.VBLF + "  SET NAME ='" + dt.Rows[0]["SNAME"].ToString().Trim() + "'" ;
                SQL = SQL + ComNum.VBLF + "    , SEX  ='" + dt.Rows[0]["SEX"].ToString().Trim() + "'" ;
                SQL = SQL + ComNum.VBLF + "    , JUMINNO ='" + strJumin+ "' " ;
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
            }

            dt.Dispose();
            dt = null;

            if (ArgGubun == "외래")
            {
                //진료정보 가져오기 외래 입원 따로 가져오기
                SQL = "";
                SQL = "SELECT m.pano, TO_CHAR(M.BDATE, 'YYYYMMDD') Bdate ,m.deptcode, d.sabun, M.ROWID   " ;
                SQL = SQL + ComNum.VBLF + " from ADMIN.opd_master m, ADMIN.ocs_doctor  d " ;
                SQL = SQL + ComNum.VBLF + "  where d.drcode = m.drcode AND M.BDATE >= TO_DATE('2009-07-07', 'YYYY-MM-DD') " ;
                SQL = SQL + ComNum.VBLF + "  and  m.PANO = '" + ArgPatid + "' and  m.DeptCode = '" + ArgDeptCode + "'" ;
                SQL = SQL + ComNum.VBLF + "  AND (m.EMR ='0' OR m.EMR IS NULL ) ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

                    SQL = "";
                    SQL = " SELECT DrSabun as Sabun, DeptCode, TO_CHAR(BDATE, 'YYYYMMDD') Bdate,Pano,ROWID ";
                    SQL = SQL + ComNum.VBLF + "  From ADMIN.OPD_MASTER          ";
                    SQL = SQL + ComNum.VBLF + " Where Pano ='"+ ArgPatid + "'                 ";
                    SQL = SQL + ComNum.VBLF + "   AND DeptCode = '"+ ArgDeptCode + "'  ";
                    SQL = SQL + ComNum.VBLF + "   AND (EMR ='0' OR EMR IS NULL )              ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE >= TO_DATE('2009-07-07', 'YYYY-MM-DD')              ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (ArgDeptCode == "MD" && (dt.Rows[i]["Sabun"].ToString().Trim() == "19094" || dt.Rows[i]["Sabun"].ToString().Trim() == "30322"))
                        {
                            strDept = "RA";
                        }
                        else
                        {
                            strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                        }

                        SQL = "SELECT TREATNO, ROWID  FROM ADMIN.EMR_TREATT ";
                        SQL = SQL + ComNum.VBLF + "  WHERE PATID = '" + dt.Rows[i]["Pano"].ToString().Trim()  + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND INDATE  ='" + dt.Rows[i]["BDate"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "    AND CLINCODE = '" + strDept + "'";
                        SQL = SQL + ComNum.VBLF + "    AND CLASS = 'O' ";
                        DataTable dtSub2 = null;
                        SqlErr = clsDB.GetDataTable(ref dtSub2, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return false;
                        }
                        if (dtSub2.Rows.Count == 0)
                        {
                            SQL = "INSERT INTO ADMIN.EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE, OUTDATE, DOCCODE, ";
                            SQL = SQL + ComNum.VBLF + " ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED";
                            SQL = SQL + ComNum.VBLF + " ) values( ADMIN.SEQ_TREATNO.NEXTVAL, '" + ArgPatid + "' ,";
                            SQL = SQL + ComNum.VBLF + " 'O' ,";//  'CLASS
                            SQL = SQL + ComNum.VBLF + " '" + dt.Rows[i]["BDate"].ToString().Trim() + "' ,";// 'INDATE
                            SQL = SQL + ComNum.VBLF + " '" + strDept + "' ,";// 'CLINCODE 2009-09-17 윤조연수정
                            SQL = SQL + ComNum.VBLF + " '' ,";//                        'OUTDATE
                            SQL = SQL + ComNum.VBLF + " '" + (long)(VB.Val((dt.Rows[i]["Sabun"].ToString().Trim()))) + "',  ";// 'DOCCODE
                            SQL = SQL + ComNum.VBLF + " '0',  ";//                'ERFLAG
                            SQL = SQL + ComNum.VBLF + " '000000',  ";//         'INITTIME
                            SQL = SQL + ComNum.VBLF + " '" + ArgPatid + "',  ";//    'OLDPATID
                            SQL = SQL + ComNum.VBLF + " '2',  ";//    'FST
                            SQL = SQL + ComNum.VBLF + " '',  ";//                     'WARD
                            SQL = SQL + ComNum.VBLF + " '', ";//                       'ROOM
                            SQL = SQL + ComNum.VBLF + " '1' )";//                       'COMPLETE
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                        else
                        {
                            SQL = " UPDATE ADMIN.EMR_TREATT SET ";
                            SQL = SQL + ComNum.VBLF + "  DELDATE ='', "; // '2009-09-07 윤조연수정
                            SQL = SQL + ComNum.VBLF + "  DOCCODE = '" + (long)(VB.Val((dt.Rows[i]["Sabun"].ToString().Trim()))) + "'";
                            SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + dtSub2.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                        dtSub2.Dispose();
                        dtSub2 = null;

                        //ocs서버 업데이트. 적용시점에 새로 시작
                        SQL = " UPDATE ADMIN.opd_master SET   EMR = '1' WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }
                dt.Dispose();
                dt = null;

            }
            else if (ArgGubun == "입원")
            {
                SQL = " SELECT  S.PANO, TO_CHAR(S.INDATE, 'YYYYMMDD') INDATE,  TO_CHAR(S.OUTDATE, 'YYYYMMDD') OUTDATE, S.DeptCode, S.ROWID,  D.SABUN ";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.ipd_new_master S, ADMIN.ocs_doctor d ";
                SQL = SQL + ComNum.VBLF + "  WHERE S.DrCode = d.drcode ";
                SQL = SQL + ComNum.VBLF + "    AND S.PANO = '" + ArgPatid + "' ";
                SQL = SQL + ComNum.VBLF + "    AND S.DeptCode = '" + ArgDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "    AND (S.EMR = '0'  OR S.EMR IS NULL)";// '나중에 적용
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                //if (ArgPatid == "06743999")
                //{
                //    ArgPatid = ArgPatid;
                //}
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return false; 
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strOK = "OK";

                    if (dt.Rows[i]["Sabun"].ToString().Trim() == "") strOK = "NO";

                    if (ArgDeptCode == "MD" && (dt.Rows[i]["Sabun"].ToString().Trim() == "19094" || dt.Rows[i]["Sabun"].ToString().Trim() == "30322"))
                    {
                        strDept = "RA";
                    }
                    else
                    {
                        strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                    }

                    SQL = "SELECT TREATNO, ROWID  FROM ADMIN.EMR_TREATT ";
                    SQL = SQL + ComNum.VBLF + " WHERE PATID = '" + ArgPatid + "'";
                    SQL = SQL + ComNum.VBLF + "   AND INDATE  ='" + dt.Rows[i]["INDATE"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "   AND CLINCODE = '" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "   AND CLASS = 'I'";
                    DataTable dtSub2 = null;
                    SqlErr = clsDB.GetDataTable(ref dtSub2, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }
                    if (dtSub2.Rows.Count == 0)
                    {
                        SQL = "INSERT INTO ADMIN.EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE,  DOCCODE, ";
                        SQL = SQL + ComNum.VBLF + " ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED ) ";
                        SQL = SQL + ComNum.VBLF + " VALUES( ADMIN.SEQ_TREATNO.NEXTVAL, '" + ArgPatid + "' ,";
                        SQL = SQL + ComNum.VBLF + "'I' ,";//                   'CLASS
                        SQL = SQL + ComNum.VBLF + "'" + dt.Rows[i]["INDATE"].ToString().Trim() + "' ,";// 'INDATE
                        SQL = SQL + ComNum.VBLF + "'" + strDept + "' ,";// 'CLINCODE
                        SQL = SQL + ComNum.VBLF + "'" + (long)(VB.Val((dt.Rows[i]["Sabun"].ToString().Trim()))) + "',  ";// 'DOCCODE
                        SQL = SQL + ComNum.VBLF + "'0',  ";//                        'ERFLAG
                        SQL = SQL + ComNum.VBLF + "'000000',  ";//                 'INITTIME
                        SQL = SQL + ComNum.VBLF + "'" + ArgPatid + "',  ";//     'OLDPATID
                        SQL = SQL + ComNum.VBLF + "'2',  ";//    'FST
                        SQL = SQL + ComNum.VBLF + "'',  ";//  'WARD
                        SQL = SQL + ComNum.VBLF + "'', ";//           'ROOM
                        SQL = SQL + ComNum.VBLF + "'1') ";//                     'COMPLETE
                    }
                    else
                    {
                        SQL = " UPDATE ADMIN.EMR_TREATT SET ";
                        SQL = SQL + ComNum.VBLF + "   DELDATE ='', ";//  '2009-09-07 윤조연수정
                        SQL = SQL + ComNum.VBLF + "   DOCCODE = '" + (long)(VB.Val((dt.Rows[i]["Sabun"].ToString().Trim()))) + "' ,";
                        SQL = SQL + ComNum.VBLF + "   OUTDATE = '" + dt.Rows[i]["OutDate"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + dtSub2.Rows[0]["ROWID"].ToString().Trim() + "'";
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                    dtSub2.Dispose();
                    dtSub2 = null;

                    //ocs서버 업데이트. 사용시 풀기
                   SQL = " UPDATE ADMIN.ipd_new_master SET  EMR = '1'";
                   SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                dt.Dispose();
                dt = null;
            }
            else if (ArgGubun == "HR" || ArgGubun == "TO")
            {
                SQL = "SELECT TREATNO, ROWID  FROM ADMIN.EMR_TREATT ";
                SQL = SQL + ComNum.VBLF + "  WHERE PATID = '" + ArgPatid + "' ";
                SQL = SQL + ComNum.VBLF + "    AND INDATE  ='" + ArgBDate + "'";
                SQL = SQL + ComNum.VBLF + "    AND CLINCODE = '" + ArgDeptCode + "'";
                SQL = SQL + ComNum.VBLF + "    AND CLASS = 'O' ";
                DataTable dtSub2 = null;
                SqlErr = clsDB.GetDataTable(ref dtSub2, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (dtSub2.Rows.Count == 0)
                {
                    SQL = "INSERT INTO ADMIN.EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE, OUTDATE, DOCCODE, ";
                    SQL = SQL + ComNum.VBLF + " ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED";
                    SQL = SQL + ComNum.VBLF + " ) values( ADMIN.SEQ_TREATNO.NEXTVAL, '" + ArgPatid + "' ,";
                    SQL = SQL + ComNum.VBLF + "'O' ,";// 'CLASS
                    SQL = SQL + ComNum.VBLF + "'" + ArgBDate + "' ,";// 'INDATE
                    SQL = SQL + ComNum.VBLF + "'" + ArgDeptCode + "' ,";// 'CLINCODE 2009-09-17 윤조연수정
                    SQL = SQL + ComNum.VBLF + "'' ,";//     'OUTDATE
                    SQL = SQL + ComNum.VBLF + "'" + ArgDrCode + "',  ";// 'DOCCODE = 종검,검진은 의사사번을 받아옴
                    SQL = SQL + ComNum.VBLF + "'0',  ";//      'ERFLAG
                    SQL = SQL + ComNum.VBLF + "'000000',  ";//  'INITTIME
                    SQL = SQL + ComNum.VBLF + "'" + ArgPatid + "',  ";//  'OLDPATID
                    SQL = SQL + ComNum.VBLF + "'2',  ";// 'FST
                    SQL = SQL + ComNum.VBLF + "'',  ";//    'WARD
                    SQL = SQL + ComNum.VBLF + "'', ";//     'ROOM
                    SQL = SQL + ComNum.VBLF + "'1' )";//   'COMPLETE
                }
                else
                {
                    SQL = " UPDATE ADMIN.EMR_TREATT SET ";
                    SQL = SQL + ComNum.VBLF + "  DELDATE ='', "; // '2009-09-07 윤조연수정
                    SQL = SQL + ComNum.VBLF + "  DOCCODE = '" + ArgDrCode + "'";
                    SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + dtSub2.Rows[i]["ROWID"].ToString().Trim() + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                dtSub2.Dispose();
                dtSub2 = null;
            }
            else if (ArgGubun == "접종")
            {
                SQL = "SELECT TREATNO, ROWID  FROM ADMIN.EMR_TREATT ";
                SQL = SQL + ComNum.VBLF + "  WHERE PATID = '" + ArgPatid + "' ";
                SQL = SQL + ComNum.VBLF + "    AND INDATE  ='" + ArgBDate + "'";
                SQL = SQL + ComNum.VBLF + "    AND CLINCODE = '" + ArgDeptCode + "'";
                SQL = SQL + ComNum.VBLF + "    AND CLASS = 'O' ";
                DataTable dtSub2 = null;
                SqlErr = clsDB.GetDataTable(ref dtSub2, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (dtSub2.Rows.Count == 0)
                {
                    SQL = "INSERT INTO ADMIN.EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE, OUTDATE, DOCCODE, ";
                    SQL = SQL + ComNum.VBLF + " ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED";
                    SQL = SQL + ComNum.VBLF + " ) values( ADMIN.SEQ_TREATNO.NEXTVAL, '" + ArgPatid + "' ,";
                    SQL = SQL + ComNum.VBLF + "'O' ,";// 'CLASS
                    SQL = SQL + ComNum.VBLF + "'" + ArgBDate + "' ,";// 'INDATE
                    SQL = SQL + ComNum.VBLF + "'" + ArgDeptCode + "' ,";// 'CLINCODE 2009-09-17 윤조연수정
                    SQL = SQL + ComNum.VBLF + "'' ,";//     'OUTDATE
                    SQL = SQL + ComNum.VBLF + "'" + ArgDrCode + "',  ";// 'DOCCODE = 종검,검진은 의사사번을 받아옴
                    SQL = SQL + ComNum.VBLF + "'0',  ";//      'ERFLAG
                    SQL = SQL + ComNum.VBLF + "'000000',  ";//  'INITTIME
                    SQL = SQL + ComNum.VBLF + "'" + ArgPatid + "',  ";//  'OLDPATID
                    SQL = SQL + ComNum.VBLF + "'2',  ";// 'FST
                    SQL = SQL + ComNum.VBLF + "'',  ";//    'WARD
                    SQL = SQL + ComNum.VBLF + "'', ";//     'ROOM
                    SQL = SQL + ComNum.VBLF + "'1' )";//   'COMPLETE
                }
                else
                {
                    SQL = " UPDATE ADMIN.EMR_TREATT SET ";
                    SQL = SQL + ComNum.VBLF + "  DELDATE ='', "; // '2009-09-07 윤조연수정
                    SQL = SQL + ComNum.VBLF + "  DOCCODE = '" + ArgDrCode + "'";
                    SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + dtSub2.Rows[i]["ROWID"].ToString().Trim() + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                dtSub2.Dispose();
                dtSub2 = null;
            }

            return true;
        }
    }
}
