namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class EtcJupmstRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public EtcJupmstRepository()
        {
        }

        public int Insert_Etc_JupMst(ETC_JUPMST item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.ETC_JUPMST                                                                                                                  ");
            parameter.AppendSql("       ( BDATE, PTNO,     SNAME,  SEX,    AGE,   ORDERCODE, ORDERNO, GBIO                                                                          ");
            parameter.AppendSql("       , BUN,   DEPTCODE, DRCODE, REMARK, RDATE, AMT, GBJOB, ENTDATE, GBER, GUBUN                                                                  ");
            parameter.AppendSql("       , ORDERDATE, SENDDATE )                                                                                                                     ");
            parameter.AppendSql("VALUES                                                                                                                                             ");
            parameter.AppendSql("       ( TO_DATE(:BDATE, 'YYYY-MM-DD'), :PTNO, :SNAME, :SEX, :AGE, :ORDERCODE, :ORDERNO, :GBIO                                                     ");
            parameter.AppendSql("                                                                                                                                                   ");
            parameter.AppendSql("       , :BUN, :DEPTCODE, :DRCODE, :REMARK, TO_DATE(:RDATE, 'YYYY-MM-DD HH24:MI'), :AMT, :GBJOB, SYSDATE, :GBER, :GUBUN                            ");
            parameter.AppendSql("       , SYSDATE, SYSDATE )                                                                                                                        ");

            parameter.Add("BDATE", item.BDATE);
            parameter.Add("PTNO", item.PTNO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX);
            parameter.Add("AGE", item.AGE);
            parameter.Add("ORDERCODE", item.ORDERCODE);
            parameter.Add("ORDERNO", item.ORDERNO);
            parameter.Add("GBIO", item.GBIO);
            parameter.Add("BUN", item.BUN);
            parameter.Add("DEPTCODE", item.DEPTCODE);
            parameter.Add("DRCODE", item.DRCODE);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("RDATE", item.RDATE);
            parameter.Add("AMT", item.AMT);
            parameter.Add("GBJOB", item.GBJOB);
            parameter.Add("GBER", item.GBER);
            parameter.Add("GUBUN", item.GUBUN);

            return ExecuteNonQuery(parameter);
        }

        public ETC_JUPMST GetImagebyRowId(string argRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT IMAGE                       ");
            parameter.AppendSql("  FROM ADMIN.ETC_JUPMST       ");
            parameter.AppendSql(" WHERE ROWID = :RID                ");
            parameter.AppendSql("   AND IMAGE IS NOT NULL           ");

            parameter.Add("RID", argRowId);

            return ExecuteReaderSingle<ETC_JUPMST>(parameter);
        }

        public int UpDate_Etc_JupMst(ETC_JUPMST dJUPMST)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.ETC_JUPMST                               ");
            parameter.AppendSql("   SET SNAME     =:SNAME                                   ");
            parameter.AppendSql("      ,SEX       =:SEX                                     ");
            parameter.AppendSql("      ,AGE       =:AGE                                     ");
            parameter.AppendSql("      ,ORDERCODE =:ORDERCODE                               ");
            parameter.AppendSql("      ,GBIO      =:GBIO                                    ");
            parameter.AppendSql("      ,BUN       =:BUN                                     ");
            parameter.AppendSql("      ,DEPTCODE  =:DEPTCODE                                ");
            parameter.AppendSql("      ,DRCODE    =:DRCODE                                  ");
            parameter.AppendSql("      ,BDATE     =TO_DATE(:BDATE, 'YYYY-MM-DD')            ");
            parameter.AppendSql("      ,RDATE     =TO_DATE(:RDATE, 'YYYY-MM-DD HH24:MI')    ");
            parameter.AppendSql("      ,ENTDATE   =SYSDATE                                  ");
            parameter.AppendSql("      ,GUBUN     =:GUBUN                                   ");
            parameter.AppendSql(" WHERE ROWID =:RID                                         ");

            parameter.Add("SNAME", dJUPMST.SNAME);
            parameter.Add("SEX", dJUPMST.SEX);
            parameter.Add("AGE", dJUPMST.AGE);
            parameter.Add("ORDERCODE", dJUPMST.ORDERCODE);
            parameter.Add("GBIO", dJUPMST.GBIO);	
            parameter.Add("BUN", dJUPMST.BUN);
            parameter.Add("DEPTCODE", dJUPMST.DEPTCODE);	
            parameter.Add("DRCODE", dJUPMST.DRCODE);
            parameter.Add("BDATE", dJUPMST.BDATE);
            parameter.Add("RDATE", dJUPMST.RDATE);
            parameter.Add("GUBUN", dJUPMST.GUBUN);
            parameter.Add("RID", dJUPMST.ROWID);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyPtNoBDate(string fstrPano, string fstrJepDate, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT FROM ADMIN.ETC_JUPMST               ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                            ");
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')                                 ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                   "); //index 태우기위해사용
            parameter.AppendSql("   AND GUBUN = :GUBUN                                          "); 
            parameter.AppendSql(" ORDER BY RDate DESC                                           ");

            parameter.Add("PTNO", fstrPano, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", fstrJepDate);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);            

            return ExecuteScalar<int>(parameter);
        }

        public List<ETC_JUPMST> GetRowIdbyPtNoBDateList(string pTNO, string jEPDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID FROM ADMIN.ETC_JUPMST                        ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                            ");
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')                                 ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                   "); //index 태우기위해사용
            parameter.AppendSql("   AND GUBUN = '4'                                             "); //PFT
            parameter.AppendSql(" ORDER BY RDate DESC                                           ");

            parameter.Add("PTNO", pTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", jEPDATE);

            return ExecuteReader<ETC_JUPMST>(parameter);
        }

        public ETC_JUPMST GetRowIdbyPtNoBDateOrerCode(string pTNO, string jEPDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID FROM ADMIN.ETC_JUPMST                        ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                            ");
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')                                 ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                   "); //index 태우기위해사용
            parameter.AppendSql("   AND ORDERCODE = 'E6941'                                     ");
            parameter.AppendSql("   AND GUBUN = '6'                                             "); //청력
            parameter.AppendSql("   AND GBJOB <> '9'                                            "); //삭제제외
            parameter.AppendSql(" ORDER BY ENTDATE DESC                                         ");

            parameter.Add("PTNO", pTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", jEPDATE);

            return ExecuteReaderSingle<ETC_JUPMST>(parameter);
        }

        public ETC_JUPMST GetRowIdbyPtNoBDate(string pTNO, string jEPDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID FROM ADMIN.ETC_JUPMST                        ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                            ");
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')                                 ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                   "); //index 태우기위해사용
            parameter.AppendSql("   AND GUBUN = '6'                                             "); //청력
            parameter.AppendSql(" ORDER BY RDate DESC                                           ");

            parameter.Add("PTNO", pTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", jEPDATE);

            return ExecuteReaderSingle<ETC_JUPMST>(parameter);
        }

        public List<ETC_JUPMST> GetRowIdbyPtNoBDateOnlyDeptCode(string fstrPtno, string fstrJepDate, string strOrderCodeYN, string strGubun, string strDeptCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                                                   ");
            parameter.AppendSql("  FROM ADMIN.ETC_JUPMST                                   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                            ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                                    ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                   "); //index 태우기위해사용
            parameter.AppendSql("   AND GUBUN = :GUBUN                                          ");
            if (strOrderCodeYN == "Y")
            {
                parameter.AppendSql("   AND ORDERCODE = 'E6941'                                 ");
            }
            parameter.AppendSql(" ORDER BY RDate DESC                                           ");

            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", fstrJepDate);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DEPTCODE", strDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteReader<ETC_JUPMST>(parameter);
        }

        public int UpdateGbJobbyPtNoRDate(string strPtNo, string strRDate, string strEDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.ETC_JUPMST SET                           ");
            parameter.AppendSql("       GBJOB = '3'                                         ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                        ");
            parameter.AppendSql("   AND RDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')             ");
            parameter.AppendSql("   AND RDATE <  TO_DATE(:TODATE,'YYYY-MM-DD')              ");
            parameter.AppendSql("   AND GBJOB = '1'                                         ");
            parameter.AppendSql("   AND ORDERCODE = 'ABI'                                   ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FRDATE", strRDate);
            parameter.Add("TODATE", strEDATE);

            return ExecuteNonQuery(parameter);
        }

        public string GetRowIdbyPtNoBDateDeptCode(string fstrPtno, string fstrJepDate, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                                                   ");
            parameter.AppendSql("  FROM ADMIN.ETC_JUPMST                                   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                            ");
            parameter.AppendSql("   AND DEPTCODE = 'HR'                                         ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                   "); //index 태우기위해사용
            parameter.AppendSql("   AND GUBUN = :GUBUN                                          ");
            parameter.AppendSql(" ORDER BY RDate DESC                                           ");

            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", fstrJepDate);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public ETC_JUPMST GetRowIdbyPtNoBDateDeptcode(string fstrPano, string fstrBDate, string fstrDeptCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                                   ");
            parameter.AppendSql("  FROM ADMIN.ETC_JUPMST                   ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                           ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')   "); 
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                    ");
            parameter.AppendSql("   AND ORDERCODE = 'STRESS'                    ");
            parameter.AppendSql("   AND GUBUN = '18'                            ");  

            parameter.Add("PTNO", fstrPano, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", fstrBDate);
            parameter.Add("DEPTCODE", fstrDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<ETC_JUPMST>(parameter);
        }

        public int UpdateImageGbJobSogenbyRowId(string strGbFtp, string strFilePath, string fstrROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.ETC_JUPMST   ");
            parameter.AppendSql("   SET GBFTP    = :GBFTP       ");
            parameter.AppendSql("     , FILEPATH = :FILEPATH    ");
            parameter.AppendSql(" WHERE ROWID    = :RID         ");

            parameter.Add("GBFTP", strGbFtp);
            parameter.Add("FILEPATH", strFilePath);
            parameter.Add("RID", fstrROWID);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateImageGbJobSogenbyRowId(string strImage_Gbn, string strGbJob, string strSogen, string fstrROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.ETC_JUPMST           ");
            parameter.AppendSql("   SET IMAGE_GBN    = :IMAGE_GBN       ");
            parameter.AppendSql("     , GBJOB        = :GBJOB           ");
            parameter.AppendSql("     , STRESS_SOGEN = :STRESS_SOGEN    ");
            parameter.AppendSql(" WHERE ROWID =:RID                     ");

            parameter.Add("IMAGE_GBN", strImage_Gbn);
            parameter.Add("GBJOB", strGbJob, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("STRESS_SOGEN", strSogen);
            parameter.Add("RID", fstrROWID);

            return ExecuteNonQuery(parameter);
        }

        public int UpDate_Etc_JupMst_Del(string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.ETC_JUPMST   ");
            parameter.AppendSql("   SET GBJOB = '9'             ");
            parameter.AppendSql("      ,ENTDATE = SYSDATE       ");
            parameter.AppendSql(" WHERE ROWID =:RID             ");

            parameter.Add("RID", argRowid);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyPtNo(string fstrPtno, string fstrJepDate, string strGubun, string strTeamPanoYN)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM ADMIN.ETC_JUPMST                   ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                           ");
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')                 ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')   "); //index 태우기위해사용
            parameter.AppendSql("   AND GUBUN = :GUBUN                          ");  //청력
            if (strTeamPanoYN == "Y")
            {
                parameter.AppendSql("   AND ORDERCODE = 'E6941'                 ");
            }
            parameter.AppendSql(" ORDER BY RDate DESC                           ");

            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", fstrJepDate);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public string GetRowidByPtNoBDateOrderCode(string argPtno, string argOrderCode, string argDate, string argDept)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID RID                               ");
            parameter.AppendSql("  FROM ADMIN.ETC_JUPMST                   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                            ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND ORDERCODE = :ORDERCODE                  ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                    ");
            parameter.AppendSql("   AND GBJOB != '9'                            ");

            if (argOrderCode == "01030110" && argDept =="HR")
            {
                parameter.AppendSql("   AND GBFTP IS NULL                            ");
            }

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", argDate);
            parameter.Add("ORDERCODE", argOrderCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DEPTCODE", argDept, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<ETC_JUPMST> GetRowIdbyPtNoBDate(string fstrPtno, string fstrJepDate, string strOrderCodeYN, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                                                   ");
            parameter.AppendSql("  FROM ADMIN.ETC_JUPMST                                   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                            ");
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')                                 ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                   "); //index 태우기위해사용
            parameter.AppendSql("   AND GUBUN = :GUBUN                                          ");
            if (strOrderCodeYN == "Y")
            {
                parameter.AppendSql("   AND ORDERCODE = 'E6941'                                 ");
            }
            parameter.AppendSql(" ORDER BY RDate DESC                                           ");

            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", fstrJepDate);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteReader<ETC_JUPMST>(parameter);
        }

        public ETC_JUPMST GetIetmbyRowId(string argRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID,TO_CHAR(RDATE,'YYYYMMDD') RDATE, FILEPATH         ");
            parameter.AppendSql("  FROM ADMIN.ETC_JUPMST                                   ");
            parameter.AppendSql(" WHERE ROWID = :RID                                            ");
            parameter.AppendSql("   AND GbFTP = 'Y'                                             ");

            parameter.Add("RID", argRowId);

            return ExecuteReaderSingle<ETC_JUPMST>(parameter);
        }

        public List<ETC_JUPMST> GetItembyPtNoJepDate(string fstrPtno, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(RDate,'YYYY-MM-DD') RDate,Ptno,SName,OrderCode  ");
            parameter.AppendSql("  FROM ADMIN.ETC_JUPMST                                   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                            ");
            parameter.AppendSql("   AND TRUNC(RDATE) = TO_DATE(:RDATE, 'YYYY-MM-DD')            ");
            parameter.AppendSql("   AND DeptCode IN ('HR','TO')                                 ");
            parameter.AppendSql("   AND ORDERCODE = 'USTCD'                                     ");
            parameter.AppendSql(" ORDER BY RDate DESC,Ptno                                      ");

            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RDATE", fstrJepDate);

            return ExecuteReader<ETC_JUPMST>(parameter);
        }

        public List<ETC_JUPMST> GetStress_SogenbyPtNo(string fstrPtno, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PTno,Stress_Sogen                                   ");
            parameter.AppendSql("  FROM ADMIN.ETC_JUPMST                               ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                        ");
            parameter.AppendSql("   AND TRUNC(RDATE) = TO_DATE(:RDATE,'YYYY-MM-DD')         ");
            parameter.AppendSql("   AND DeptCode IN ('HR','TO')                             ");
            parameter.AppendSql("   AND Gubun = '18'                                        ");

            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RDATE", fstrJepDate);

            return ExecuteReader<ETC_JUPMST>(parameter);
        }

        public int GetCountbyRowId(string strRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT          ");
            parameter.AppendSql("  FROM ADMIN.ETC_JUPMST   ");
            parameter.AppendSql(" WHERE ROWID = :RID            ");
            parameter.AppendSql("   AND GbFTP = 'Y'             ");

            parameter.Add("RID", strRowId);

            return ExecuteScalar<int>(parameter);
        }

        public ETC_JUPMST GetImageGbnbyRowId(string strRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID, IMAGE_GBN        ");
            parameter.AppendSql("  FROM ADMIN.ETC_JUPMST   ");
            parameter.AppendSql(" WHERE ROWID = :RID            ");
            parameter.AppendSql("   AND GbFTP = 'Y'             ");

            parameter.Add("RID", strRowId);

            return ExecuteReaderSingle<ETC_JUPMST>(parameter);
        }

        public List<ETC_JUPMST> GetItembyRdate()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(RDate,'YYYY-MM-DD') RDATE, PTNO, SNAME, ORDERCODE, DEPTCODE     ");
            parameter.AppendSql("  FROM ADMIN.ETC_JUPMST                                                   ");
            parameter.AppendSql(" WHERE RDate >= TRUNC(SYSDATE-7)                                               ");
            parameter.AppendSql("   AND RDate <= TRUNC(SYSDATE)                                                 ");
            parameter.AppendSql("   AND ORDERCODE = 'USTCD'                                                     ");
            parameter.AppendSql("   AND DeptCode IN ('TO', 'HR')                                                ");
            parameter.AppendSql(" ORDER BY RDATE DESC, PTNO                                                     ");

            return ExecuteReader<ETC_JUPMST>(parameter);
        }

        public List<ETC_JUPMST> GetItemStress()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PTNO, STRESS_SOGEN      ");
            parameter.AppendSql(" FROM ADMIN.ETC_JUPMST    ");
            parameter.AppendSql("WHERE RDate>=TRUNC(SYSDATE-3)  ");
            parameter.AppendSql("  AND RDate<=TRUNC(SYSDATE+1)  ");
            parameter.AppendSql("  AND DeptCode='TO'            ");
            parameter.AppendSql("  AND Gubun='18'               ");

            return ExecuteReader<ETC_JUPMST>(parameter);
        }

        public List<ETC_JUPMST> GetItemEtcJupMst()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT FILEPATH                                    ");
            parameter.AppendSql("  FROM ADMIN.ETC_JUPMST                       ");
            parameter.AppendSql(" WHERE RDate >= TO_DATE('2015-08-01','YYYY-MM-DD') ");
            parameter.AppendSql("   AND DeptCode = 'TO'                             ");
            parameter.AppendSql("   AND Gubun = '18'                                ");
            parameter.AppendSql("   AND GbFTP = 'Y'                                 ");

            return ExecuteReader<ETC_JUPMST>(parameter);
        }

        public string GetRowIdbyPaNo(string fstrPano, string fstrBDate, string fstrDeptCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID RID                               ");
            parameter.AppendSql("  FROM ADMIN.ETC_JUPMST                   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                            ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                    ");
            parameter.AppendSql("   AND ORDERCODE = 'STRESS'                    ");
            parameter.AppendSql("   AND GUBUN = '18'                            ");

            parameter.Add("BDATE", fstrBDate);
            parameter.Add("PTNO", fstrPano, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DEPTCODE", fstrDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }
    }
}
