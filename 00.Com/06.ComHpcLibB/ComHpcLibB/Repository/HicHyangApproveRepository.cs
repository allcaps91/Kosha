namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicHyangApproveRepository : BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicHyangApproveRepository()
        {
        }

        public int Insert(HIC_HYANG_APPROVE item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_HYANG_APPROVE                              ");
            parameter.AppendSql("       (SDATE, BDATE, WRTNO, PANO, SNAME, JONG, GBSITE                 ");
            parameter.AppendSql("      , DEPTCODE, SUCODE, QTY, REALQTY, ENTQTY, NRSABUN, PTNO, JUMIN                 ");
            parameter.AppendSql("      , JUMIN2, SEX, AGE, ENTDATE, DRSABUN, AMPM, GBSLEEP, JUSO)                            ");
            parameter.AppendSql("VALUES                                                                 ");
            parameter.AppendSql("       (TO_DATE(:SDATE, 'YYYY-MM-DD'), TO_DATE(:BDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("      , :WRTNO, :PANO, :SNAME, :JONG, :GBSITE                          ");
            parameter.AppendSql("      , :DEPTCODE, :SUCODE, :QTY, :REALQTY, :ENTQTY, :NRSABUN, :PTNO, :JUMIN           ");
            parameter.AppendSql("      , :JUMIN2, :SEX, :AGE, SYSDATE, :DRSABUN, :AMPM, :GBSLEEP, :JUSO)                        ");

            parameter.Add("SDATE", item.SDATE);
            parameter.Add("BDATE", item.BDATE);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("PANO", item.PANO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("JONG", item.JONG);
            parameter.Add("GBSITE", item.GBSITE);
            parameter.Add("DEPTCODE", item.DEPTCODE);
            parameter.Add("SUCODE", item.SUCODE);
            parameter.Add("QTY", item.QTY);
            parameter.Add("REALQTY", item.REALQTY);
            parameter.Add("ENTQTY", item.ENTQTY);
            parameter.Add("NRSABUN", item.NRSABUN);
            parameter.Add("PTNO", item.PTNO);
            parameter.Add("JUMIN", item.JUMIN);
            parameter.Add("JUMIN2", item.JUMIN2);
            parameter.Add("SEX", item.SEX);
            parameter.Add("AGE", item.AGE);
            parameter.Add("DRSABUN", item.DRSABUN);
            parameter.Add("AMPM", item.AMPM);
            parameter.Add("GBSLEEP", item.GBSLEEP);
            parameter.Add("JUSO", item.JUSO);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_HYANG_APPROVE> GetItembyBDateDeptCode(string strBDate, string strJob, string strDeptCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,SNAME,AGE,SEX,SUCODE,QTY,REALQTY,PRINT,DRSABUN,GBSITE,DEPTCODE,PTNO   ");
            parameter.AppendSql("     , TO_CHAR(BDATE,'YYYY-MM-DD') BDate                                           ");
            parameter.AppendSql("     , TO_CHAR(OCSSENDTIME,'YYYY-MM-DD HH24:MI') OCSSENDTIME                       ");
            parameter.AppendSql("     , TO_CHAR(APPROVETIME,'YYYY-MM-DD HH24:MI') APPROVETIME,ROWID RID             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE                                               ");
            parameter.AppendSql(" WHERE BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                                       ");
            parameter.AppendSql("   AND DEPTCODE  = :DEPTCODE                                                       ");
            parameter.AppendSql("   AND DelDate IS NULL                                                             ");
            parameter.AppendSql("   AND SUCODE NOT IN('A-PO12GA')                                                   ");
            if (strJob == "1")
            {
                parameter.AppendSql("   AND APPROVETIME IS NULL                                                     ");
            }
            parameter.AppendSql(" ORDER BY WRTNO, BDATE, SUCODE                                                     ");

            parameter.Add("BDATE", strBDate);
            parameter.Add("DEPTCODE", strDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_HYANG_APPROVE>(parameter);
        }

        public int InsertSelectbyRowId(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_HYANG_APPROVE                                                  ");
            parameter.AppendSql("       (SDATE,BDATE,WRTNO,PANO,SNAME,JONG,GBSITE,DEPTCODE,SUCODE,QTY,REALQTY               ");
            parameter.AppendSql("     , ENTQTY,NRSABUN,PTNO,JUMIN,SEX,AGE,ENTDATE,DRSABUN,APPROVETIME,PRINT,OCSSENDTIME     ");
            parameter.AppendSql("     , ENTQTY2 , AMPM, GBSLEEP, JUMIN2, CHASU, DELDATE, CERTNO)                            ");
            parameter.AppendSql("SELECT SDATE,BDATE,WRTNO,PANO,SNAME,JONG,GBSITE,DEPTCODE,SUCODE,QTY,REALQTY                ");
            parameter.AppendSql("       ENTQTY,NRSABUN,PTNO,JUMIN,SEX,AGE,ENTDATE,DRSABUN,APPROVETIME,PRINT,OCSSENDTIME     ");
            parameter.AppendSql("       ENTQTY2,AMPM,GBSLEEP,JUMIN2,CHASU,SYSDATE,CERTNO                                    ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_HYANG_APPROVE                                                       ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                                        ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public void UpdateDelDatebySDateWrtnoSucode(string argDate, long argWrtno, string argDept, string argSuCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_HYANG_APPROVE               ");
            parameter.AppendSql("   SET DELDATE  = SYSDATE                          ");
            parameter.AppendSql(" WHERE SDATE    = TO_DATE(:SDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND WRTNO    =:WRTNO                            ");
            parameter.AppendSql("   AND DEPTCODE =:DEPTCODE                         ");
            parameter.AppendSql("   AND SUCODE   =:SUCODE                           ");

            parameter.Add("SDATE", argDate);
            parameter.Add("WRTNO", argWrtno);
            parameter.Add("DEPTCODE", argDept, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SUCODE", argSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            ExecuteNonQuery(parameter);
        }

        public void UpDateItemByRowid(string strGb, string strGbSite, int nEntQty, string strJuso, string rOWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_HYANG_APPROVE SET       ");
            if (strGb == "Y")
            {
                parameter.AppendSql("       GBSITE      = :GBSITE ,             ");
                parameter.AppendSql("     APPROVETIME = '' ,                    ");
                parameter.AppendSql("     DRSABUN     = '' ,                    ");
                parameter.AppendSql("     CERTNO      = '' ,                    ");
            }

            if (strGbSite == "1")
            {
                parameter.AppendSql("       ENTQTY = :ENTQTY,                   ");
            }

            parameter.AppendSql("     JUSO = :JUSO ,                            ");
            parameter.AppendSql("     DELDATE = ''                              ");
            parameter.AppendSql(" WHERE ROWID = :RID                            ");

            if (strGb == "Y")
            {
                parameter.Add("GBSITE", strGbSite, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (strGbSite == "1")
            {
                parameter.Add("ENTQTY", nEntQty);

            }
                
            parameter.Add("JUSO", strJuso);
            parameter.Add("RID", rOWID);

            ExecuteNonQuery(parameter);
        }

        public int UpdateAppTimebyRowId(string strGubun, string strAppTime, long nQty, string idNumber, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_HYANG_APPROVE SET           ");
            if (strGubun == "1")
            {
                parameter.AppendSql("       QTY         = :QTY                      ");
                parameter.AppendSql("     , REALQTY     = :QTY                      ");
                parameter.AppendSql("     , APPROVETIME = SYSDATE                   ");
                parameter.AppendSql("     , DRSABUN     = :DRSABUN                  ");
                parameter.AppendSql("     , CERTNO      = ''                        ");
            }
            else if (strAppTime.IsNullOrEmpty())
            {
                parameter.AppendSql("       APPROVETIME = SYSDATE                   ");
                parameter.AppendSql("     , DRSABUN     = :DRSABUN                  ");
                parameter.AppendSql("     , CERTNO      = ''                        ");
            }
            parameter.AppendSql("     , DELDATE = ''                                ");
            parameter.AppendSql(" WHERE ROWID = :RID                                ");

            if (strGubun == "1")
            {
                parameter.Add("QTY", nQty);
                parameter.Add("DRSABUN", idNumber);
            }
            if (strAppTime.IsNullOrEmpty())
            {
                parameter.Add("DRSABUN", idNumber);
            }
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }
        public int UpdateGbSiteEntQtyByBdatePtnoSucode(string strGbSite, long nQty, string strBdate, string strPtno, string strSucode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_HYANG_APPROVE SET       ");
            parameter.AppendSql("       GBSITE = :GBSITE                        ");
            parameter.AppendSql("     , ENTQTY     = :ENTQTY                    ");
            parameter.AppendSql(" WHERE BDATE = TO_DATE(:BDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql(" AND PTNO = :PTNO                              ");
            parameter.AppendSql(" AND SUCODE = :SUCODE                          ");

            parameter.Add("GBSITE", strGbSite, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTQTY", nQty);
            parameter.Add("BDATE", strBdate);
            parameter.Add("PTNO", strPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SUCODE", strSucode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        

        public int UpdateDelDatebyRowId(string argRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE HIC_HYANG_APPROVE SET       ");
            parameter.AppendSql("       DELDATE = SYSDATE           ");
            parameter.AppendSql(" WHERE ROWID = :RID                ");

            parameter.Add("RID", argRowId);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_HYANG_APPROVE> GetItembyWrtNoDeptCode(long argWrtNo, string strDeptCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PtNo,SuCode,TO_CHAR(BDate,'YYYY-MM-DD') BDate           ");
            parameter.AppendSql("     , TO_CHAR(DelDate,'YYYY-MM-DD HH24:MI') DelDate,ROWID     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE                           ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                                       ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                                    ");
            parameter.AppendSql("   AND DELDATE IS NULL                                         ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("DEPTCODE", strDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_HYANG_APPROVE>(parameter);
        }

        public List<HIC_HYANG_APPROVE> GetSucodeDrSabunQtybyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUCODE, DRSABUN, QTY            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE   ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");
            parameter.AppendSql("   AND DeptCode = 'HR'                 ");
            parameter.AppendSql("   AND DelDate IS NULL                 ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReader<HIC_HYANG_APPROVE>(parameter);
        }

        public int UpdateItembyRowId(double nQty, double nOldQty, string idNumber, string strRowId, string strApproveTime)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_HYANG_APPROVE SET   ");
            parameter.AppendSql("       DELDATE = ''                        ");
            if (nQty != nOldQty)
            {
                parameter.AppendSql("     , QTY         = :QTY              ");
                parameter.AppendSql("     , REALQTY     = :QTY              ");
                parameter.AppendSql("     , APPROVETIME = SYSDATE           ");
                parameter.AppendSql("     , DRSABUN     = :DRSABUN          ");
                parameter.AppendSql("     , CERTNO      = ''                ");
            }
            else if (strApproveTime.IsNullOrEmpty())
            {
                parameter.AppendSql("     , APPROVETIME = SYSDATE           ");
                parameter.AppendSql("     , DRSABUN     = :DRSABUN          ");
                parameter.AppendSql("     , CERTNO      = ''                ");
            }            
            parameter.AppendSql(" WHERE ROWID = :RID                        ");

            if (nQty != nOldQty)
            {
                parameter.Add("QTY", nQty);
                parameter.Add("DRSABUN", idNumber);
            }

            if (strApproveTime.IsNullOrEmpty())
            {
                parameter.Add("DRSABUN", idNumber);
            }
            parameter.Add("RID", strRowId);

            return ExecuteNonQuery(parameter);
        }

        public int InsertSelect(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_HYANG_APPROVE                                                      ");
            parameter.AppendSql("       (SDATE,BDATE,WRTNO,PANO,SNAME,JONG,GBSITE,DEPTCODE,SUCODE,QTY,REALQTY                   ");
            parameter.AppendSql("      , ENTQTY,NRSABUN,PTNO,JUMIN,SEX,AGE,ENTDATE,DRSABUN,APPROVETIME,PRINT,OCSSENDTIME        ");
            parameter.AppendSql("      , ENTQTY2 , AMPM, GBSLEEP, JUMIN2, CHASU, DELDATE, CERTNO)                               ");
            parameter.AppendSql("      , SELECT SDATE,BDATE,WRTNO,PANO,SNAME,JONG,GBSITE,DEPTCODE,SUCODE,QTY,REALQTY            ");
            parameter.AppendSql("      ,        ENTQTY,NRSABUN,PTNO,JUMIN,SEX,AGE,ENTDATE,DRSABUN,APPROVETIME,PRINT,OCSSENDTIME ");
            parameter.AppendSql("      ,        ENTQTY2,AMPM,GBSLEEP,JUMIN2,CHASU,SYSDATE,CERTNO                                ");
            parameter.AppendSql("          From KOSMOS_PMPA.HIC_HYANG_Approve                                                   ");
            parameter.AppendSql("         WHERE ROWID = :RID                                                                    ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public HIC_HYANG_APPROVE GetItembyWrtNoSucode(long argWrtNo, string strSuCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT DRSABUN,PTNO,GBSITE,QTY,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,SNAME,SEX,AGE     ");
            parameter.AppendSql("     , TO_CHAR(APPROVETIME,'YYYY-MM-DD HH24:MI') APPROVETIME,ROWID RID             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE                                               ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                                                           ");
            parameter.AppendSql("   AND DEPTCODE = 'HR'                                                             ");
            parameter.AppendSql("   AND SUCODE   = :SUCODE                                                          ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                             ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("SUCODE", strSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_HYANG_APPROVE>(parameter);
        }

        public int UpdatebyRowId(string rOWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_HYANG_APPROVE SET   ");
            parameter.AppendSql("       DELDATE = SYSDATE                   ");
            parameter.AppendSql(" WHERE ROWID = :RID                        ");

            parameter.Add("RID", rOWID);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_HYANG_APPROVE> GetItemListbyWrtNo(long argWrtNo, string argDept)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PtNo,SuCode,TO_CHAR(BDate,'YYYY-MM-DD') BDate                   ");
            parameter.AppendSql("     , TO_CHAR(DelDate,'YYYY-MM-DD HH24:MI') DelDate, ROWID RID        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE                                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                  ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                                            ");
            parameter.AppendSql("   AND DelDate IS NULL                                                 ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("DEPTCODE", argDept);

            return ExecuteReader<HIC_HYANG_APPROVE>(parameter);
        }

        public List<HIC_HYANG_APPROVE> GetItembyBdatePaNo(string strBDate, long nPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,SName,DeptCode,NrSabun,PtNo,Sex,Age,DrSabun,Ptno,GbSite   ");
            parameter.AppendSql("     , SuCode,EntQty,ApproveTime                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE                                   ");
            parameter.AppendSql(" WHERE BDATE    = TO_DATE(:BDATE, 'YYYY-MM-DD')                        ");
            parameter.AppendSql("   AND PANO     = :PANO                                                ");
            parameter.AppendSql("   AND DEPTCODE IN ('TO','HR')                                                 ");

            parameter.Add("BDATE", strBDate);
            parameter.Add("PANO", nPano);

            return ExecuteReader<HIC_HYANG_APPROVE>(parameter);
        }

        public int UpdateDrSabunbyRowId(string strDrSabun, string strApproveTime, string strCertNo, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE HIC_HYANG_APPROVE SET       ");
            parameter.AppendSql("       DRSABUN = :DRSABUN          ");
            parameter.AppendSql("     , APPROVETIME = :APPROVETIME  ");
            parameter.AppendSql("     , CERTNO = :CERTNO            ");
            parameter.AppendSql(" WHERE ROWID = :RID                ");

            parameter.Add("DRSABUN", strDrSabun);
            parameter.Add("APPROVETIME", strApproveTime);
            parameter.Add("CERTNO", strCertNo);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateDrSabunPproveTimebyRowId(string idNumber, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE HIC_HYANG_APPROVE SET       ");
            parameter.AppendSql("       DRSABUN = :DRSABUN          ");
            parameter.AppendSql("     , ApproveTime = SYSDATE       ");
            parameter.AppendSql(" WHERE ROWID = :RID                ");

            parameter.Add("DRSABUN", idNumber);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public HIC_HYANG_APPROVE GetItembyWrtnoBDate(long nWRTNO, string argBDate, string argSuCode, string argDept)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT AMPM, GBSLEEP, PTNO, JUMIN2, SEX, AGE   ");
            parameter.AppendSql("     , DEPTCODE, DRSABUN, PRINT, CHASU, GBSITE ");
            parameter.AppendSql("     ,ROWID ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE           ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND BDATE  = TO_DATE(:BDATE, 'YYYY-MM-DD')  ");
            if (argSuCode != "")
            {
                parameter.AppendSql("   AND SUCODE  = :SUCODE ");
            }

            if (argDept != "")
            {
                parameter.AppendSql("   AND DEPTCODE  = :DEPTCODE ");
            }

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("BDATE", argBDate);

            if (argSuCode != "")
            {
                parameter.Add("SUCODE", argSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (argDept != "")
            {
                parameter.Add("DEPTCODE", argDept, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReaderSingle<HIC_HYANG_APPROVE>(parameter);
        }

        public List<HIC_HYANG_APPROVE> GetItembyWrtNoBDate(long nWRTNO, string strBDate, string strDept, string strSuCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT AMPM, GBSLEEP, PTNO, JUMIN2, SEX, AGE                       ");
            parameter.AppendSql("     , DEPTCODE, DRSABUN, PRINT, CHASU, SUCODE                     ");
            parameter.AppendSql("     , TO_CHAR(APPROVETIME, 'YYYY-MM-DD') APPROVETIME, JUSO, JUMIN ");
            parameter.AppendSql("     , JONG,DEPTCODE,SUCODE,ENTQTY,ENTQTY2,NRSABUN,DRSABUN         ");
            parameter.AppendSql("     ,  TO_CHAR(BDATE, 'YYYY-MM-DD') BDATE                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE                               ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                              ");
            if (strBDate != "")
            {
                parameter.AppendSql("   AND BDATE  = TO_DATE(:BDATE, 'YYYY-MM-DD')                  ");
            }
            if (strDept != "")
            {
                parameter.AppendSql("   AND DEPTCODE  = :DEPTCODE                                   ");
            }
            if (strSuCode != "")
            {
                parameter.AppendSql("   AND SUCODE  = :SUCODE                                       ");
            }
            parameter.AppendSql("   AND DELDATE IS NULL                                             ");

            parameter.Add("WRTNO", nWRTNO);

            if (strBDate != "")
            {
                parameter.Add("BDATE", strBDate);
            }
            if (strDept != "")
            {
                parameter.Add("DEPTCODE", strDept, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (strSuCode != "")
            {
                parameter.Add("SUCODE", strSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_HYANG_APPROVE>(parameter);
        }

        public List<HIC_HYANG_APPROVE> GetItembyBDate(string fstrBDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME, WRTNO, PANO, PRINT, COUNT(*) CNT ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE           ");
            parameter.AppendSql(" WHERE BDATE  = TO_DATE(:BDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND APPROVETIME IS NOT NULL                 ");
            parameter.AppendSql("   AND GBSITE = '1'                            ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql(" GROUP BY SNAME, WRTNO, PANO, PRINT            ");
            parameter.AppendSql(" ORDER BY SNAME, WRTNO, PANO, PRINT            ");

            parameter.Add("BDATE", fstrBDate);

            return ExecuteReader<HIC_HYANG_APPROVE>(parameter);
        }

        public int GetCountbyBDate(string strBDate, string strGubun = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE               ");
            parameter.AppendSql(" WHERE BDATE  = TO_DATE(:BDATE, 'YYYY-MM-DD')      ");
            if (strGubun == "")
            {
                parameter.AppendSql("   AND GBSITE = '1'                            "); //종검내시경
            }
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            parameter.AppendSql("   AND OCSSENDTIME IS NOT NULL                     ");

            parameter.Add("BDATE", strBDate);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_HYANG_APPROVE> GetItembyWrtNo(long nWRTNO, string strBDate, long nPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT JONG, GBSITE, DEPTCODE, SUCODE, ENTQTY, NRSABUN, PTNO, JUMIN2               ");
            parameter.AppendSql("     , DRSABUN, SEX, AGE, APPROVETIME, PRINT, OCSSENDTIME, AMPM, GBSLEEP, CHASU    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE                                               ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                              ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                                       ");
            parameter.AppendSql("   AND DelDate IS NULL                                                             ");
            if (nPano != 0)
            {
                parameter.AppendSql("   AND PANO  = :PANO                                                           ");
            }
            parameter.AppendSql(" ORDER BY SUCODE                                                                   ");


            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("BDATE", strBDate);
            parameter.Add("PANO", nPano);

            return ExecuteReader<HIC_HYANG_APPROVE>(parameter);
        }

        public List<HIC_HYANG_APPROVE> GetItembyBDate(string strSDate, long nSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT SNAME, WRTNO, PANO, PTNO, DEPTCODE, COUNT(*) AS CNT    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE                           ");
            parameter.AppendSql(" WHERE BDATE = TO_DATE(:BDATE,'YYYY-MM-DD')                    ");
            if (nSabun != 36540)
            {
                parameter.AppendSql("   AND PANO <> 999                                         ");
            }
            parameter.AppendSql(" GROUP BY SNAME, WRTNO, PANO, PTNO, DEPTCODE                   ");
            parameter.AppendSql(" ORDER BY SNAME, WRTNO, PANO, PTNO, DEPTCODE                   ");

            parameter.Add("BDATE", strSDate);

            return ExecuteReader<HIC_HYANG_APPROVE>(parameter);
        }

        public List<HIC_HYANG_APPROVE> GetItembySDateDeptCodeSite(string strSDate, string strDeptCode, string strGBSite)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, PANO, SNAME, GBSITE, SUCODE, QTY, SEX, AGE,DRSABUN                   ");
            parameter.AppendSql("     , TO_CHAR(APPROVETIME,'YYYY-MM-DD HH24:MI') APPROVETIME                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE                                               ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')                                       ");
            parameter.AppendSql("   AND DEPTCODE  = :DEPTCODE                                                       ");
            parameter.AppendSql("   AND DelDate IS NULL                                                             ");
            if (strGBSite == "1")
            {
                parameter.AppendSql("   AND GBSITE = '1'                                                            ");
            }
            else if (strGBSite == "2")
            {
                parameter.AppendSql("   AND GBSITE = '2'                                                            ");
            }

            parameter.AppendSql(" ORDER BY SNAME                                                                    ");

            parameter.Add("SDATE", strSDate);
            parameter.Add("DEPTCODE", strDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_HYANG_APPROVE>(parameter);
        }

        public int UpdateEntqty2(double argEntqty, long argWrtno, string argPano, string argSucode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE HIC_HYANG_APPROVE SET               ");
            parameter.AppendSql("       ENTQTY2 = :ENTQTY                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");
            parameter.AppendSql(" AND PANO = :PANO                          ");
            parameter.AppendSql(" AND SUCODE = :SUCODE                      ");
            parameter.AppendSql(" AND DELDATE IS NULL                       ");

            parameter.Add("ENTQTY", argEntqty);
            parameter.Add("WRTNO", argWrtno);
            parameter.Add("PANO", argPano);
            parameter.Add("SUCODE", argSucode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int DeleteHicHyangApprove(string argBdate,  long argWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" DELETE KOSMOS_PMPA.HIC_HYANG_APPROVE                                      ");
            parameter.AppendSql("  WHERE BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                              ");
            parameter.AppendSql("  AND WRTNO = :WRTNO                                                       ");

            parameter.Add("BDATE", argBdate);
            parameter.Add("WRTNO", argWrtno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateOcsSendTime(string argBdate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE HIC_HYANG_APPROVE SET                   ");
            parameter.AppendSql("       OCSSENDTIME =''                         ");
            parameter.AppendSql(" WHERE BDATE = TO_DATE(:BDATE ,'YYYY-MM-DD')   ");
            parameter.AppendSql(" AND GBSITE = '1'                              ");
            parameter.AppendSql(" AND OCSSENDTIME IS NOT NULL                   ");

            parameter.Add("BDATE", argBdate);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_HYANG_APPROVE> GetItemCountByBDate(string argBDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, PANO, PTNO, SNAME, COUNT(*) CNT  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE           ");
            parameter.AppendSql(" WHERE BDATE  = TO_DATE(:BDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND GBSITE = '1'                            ");
            parameter.AppendSql("   AND DRSABUN IS NOT NULL                     ");
            parameter.AppendSql(" GROUP BY WRTNO, PANO, PTNO, SNAME             ");
            parameter.AppendSql(" ORDER BY WRTNO, PANO, PTNO, SNAME             ");

            parameter.Add("BDATE", argBDATE);

            return ExecuteReader<HIC_HYANG_APPROVE>(parameter);
        }

        public List<HIC_HYANG_APPROVE> GetRowidSucodeByItems(string argBdate, long argWrtno, long argPano, string argPtno, string argSname)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID, SUCODE, ENTQTY, ENTQTY2          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE           ");
            parameter.AppendSql(" WHERE BDATE  = TO_DATE(:BDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND GBSITE = '1'                            ");
            parameter.AppendSql("   AND WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND PANO = :PANO                            ");
            parameter.AppendSql("   AND PTNO = :PTNO                            ");
            parameter.AppendSql("   AND SNAME = :SNAME                          ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");

            parameter.Add("BDATE", argBdate);
            parameter.Add("WRTNO", argWrtno);
            parameter.Add("PANO", argPano);
            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SNAME", argSname);

            return ExecuteReader<HIC_HYANG_APPROVE>(parameter);
        }

        public int UpdateOcsSendTimeByItems(string argBdate, long argWrtno, long argPano, string argPtno, string argSname)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE HIC_HYANG_APPROVE SET                           ");
            parameter.AppendSql("       CHASU = 1                                      ");
            parameter.AppendSql("       , OCSSENDTIME = SYSDATE                         ");
            parameter.AppendSql(" WHERE BDATE = TO_DATE(:BDATE ,'YYYY-MM-DD')           ");
            parameter.AppendSql(" AND GBSITE = '1'                                      ");
            parameter.AppendSql(" AND WRTNO = :WRTNO                                    ");
            parameter.AppendSql(" AND PANO = :PANO                                      ");
            parameter.AppendSql(" AND PTNO = :PTNO                                      ");
            parameter.AppendSql(" AND SNAME = :SNAME                                    ");
            parameter.AppendSql(" AND DELDATE IS NULL                                   ");

            parameter.Add("BDATE", argBdate);
            parameter.Add("WRTNO", argWrtno);
            parameter.Add("PANO", argPano);
            parameter.Add("PTNO", argPtno);
            parameter.Add("SNAME", argSname);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_HYANG_APPROVE> GetItemByBdateSucode(string argBdate, string argSucode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID, ENTQTY, ENTQTY2                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE               ");
            parameter.AppendSql(" WHERE BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND SUCODE = :SUCODE                            ");
            parameter.AppendSql("   AND ENTQTY2 <> 0                                ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            parameter.AppendSql("   AND GBSITE = '1'                                ");

            parameter.Add("BDATE", argBdate);
            parameter.Add("SUCODE", argSucode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_HYANG_APPROVE>(parameter);
        }

        public int GetCountByPtNoBdate(string argPtno, string argSdate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE           ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                           ");
            parameter.AppendSql("   AND SDATE >= TO_DATE(:SDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND DEPTCODE = 'HR'                         ");

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SDATE", argSdate);

            return ExecuteScalar<int>(parameter);
        }
    }
}
