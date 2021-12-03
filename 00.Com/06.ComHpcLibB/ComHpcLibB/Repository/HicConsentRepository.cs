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
    public class HicConsentRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicConsentRepository()
        {
        }

        public int UpdateASAbyPtNoSDate(string strASA, long fnDrno, string fstrPtno, string fstrExDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_CONSENT                     ");
            parameter.AppendSql("   SET ASA      = :ASA                             ");
            parameter.AppendSql("     , ASASABUN = :ASASABUN                        ");
            parameter.AppendSql("     , ASATIME  = SYSDATE                          ");
            parameter.AppendSql(" WHERE PTNO     = :PTNO                            ");
            parameter.AppendSql("   AND SDATE   >= TO_DATE(:SDATE, 'YYYY-MM-DD')    ");

            parameter.Add("ASA", strASA, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ASASABUN", fnDrno);
            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SDATE", fstrExDate);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateASASabunbyPtNoSDate(string strASA, long fnDrno, string fstrPtno, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_CONSENT SET                 ");
            parameter.AppendSql("       ASA      = :ASA                             ");
            parameter.AppendSql("     , ASASABUN = :ASASABUN                        ");
            parameter.AppendSql("     , ASATIME  = SYSDATE                          ");
            parameter.AppendSql("WHERE PTNO      = :PTNO                            ");
            parameter.AppendSql("  AND SDATE    >= TO_DATE(:SDATE,'YYYY-MM-DD')     ");

            parameter.Add("ASA", strASA, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ASASABUN", fnDrno);
            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SDATE", fstrJepDate);

            return ExecuteNonQuery(parameter);
        }

        public void Insert(HIC_CONSENT nHC)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO HIC_CONSENT (BDATE,SDATE,PTNO,WRTNO,PANO,SNAME,DEPTCODE,FORMCODE,PAGECNT,ENTDATE,ENTSABUN  ");
            parameter.AppendSql(" ) VALUES (                                                                                            ");
            parameter.AppendSql("      TO_DATE(:BDATE,'YYYY-MM-DD'),TO_DATE(:SDATE,'YYYY-MM-DD'),:PTNO,:WRTNO,:PANO,:SNAME,:DEPTCODE                 ");
            parameter.AppendSql("     ,:FORMCODE,:PAGECNT,SYSDATE,:ENTSABUN )                                                           ");

            parameter.Add("BDATE", nHC.BDATE);
            parameter.Add("SDATE", nHC.SDATE);
            parameter.Add("PTNO", nHC.PTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", nHC.WRTNO);
            parameter.Add("PANO", nHC.PANO);
            parameter.Add("SNAME", nHC.SNAME);
            parameter.Add("DEPTCODE", nHC.DEPTCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FORMCODE", nHC.FORMCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PAGECNT", nHC.PAGECNT);
            parameter.Add("ENTSABUN", nHC.ENTSABUN);

            ExecuteNonQuery(parameter);
        }

        public void UpDateDelDateByRowid(string strRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_CONSENT        ");
            parameter.AppendSql("   SET DELDATE = SYSDATE              ");
            parameter.AppendSql(" WHERE ROWID   = :RID               ");

            parameter.Add("RID", strRowid);

            ExecuteNonQuery(parameter);
        }

        public int GetCountbyPtNoSDateFormCode(string pTNO, string strJEPDATE, string strFormCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM ADMIN.HIC_CONSENT                     ");
            parameter.AppendSql(" WHERE PTNO     = :PTNO                            ");
            parameter.AppendSql("   AND SDATE    = TO_DATE(:SDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND FORMCODE = :FORMCODE                        ");
            parameter.AppendSql("   AND DOCTSIGN IS NOT NULL                        ");

            parameter.Add("PTNO", pTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SDATE", strJEPDATE);
            parameter.Add("FORMCODE", strFormCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateItem2(HIC_CONSENT item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_CONSENT SET                 ");
            parameter.AppendSql("       DRSABUN      = :DRSABUN                     ");
            parameter.AppendSql("     , DOCTSIGN     = SYSDATE                      ");
            parameter.AppendSql("     , CONSENT_TIME = SYSDATE                      ");
            parameter.AppendSql("     , EMRNO        = :EMRNO                       ");
            parameter.AppendSql(" WHERE PTNO         = :PTNO                        ");
            parameter.AppendSql("   AND FORMCODE     = :FORMCODE                    ");
            parameter.AppendSql("   AND SDATE        = TO_DATE(:SDATE, 'YYYY-MM-DD')");
            parameter.AppendSql("   AND DEPTCODE     = :DEPTCODE                    ");

            parameter.Add("DRSABUN", item.DRSABUN);
            parameter.Add("EMRNO", item.EMRNO);
            parameter.Add("PTNO", item.PTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FORMCODE", item.FORMCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SDATE", item.SDATE);
            parameter.Add("DEPTCODE", item.DEPTCODE);

            return ExecuteNonQuery(parameter);
        }

        public string GetFormCDByFormNo(long fmFORMNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT FORMCODE                          ");
            parameter.AppendSql("  FROM ADMIN.HIC_CONSENT_FORM      ");
            parameter.AppendSql(" WHERE FORMNO      = :FORMNO             ");
            parameter.AppendSql("   AND DELDATE IS NULL                   ");

            parameter.Add("FORMNO", fmFORMNO);

            return ExecuteScalar<string>(parameter);
        }

        public long GetListByFormNoPtnoDate(long fORMNO, string strPtno, string strDate, string strDept)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(C.ID) AS CNT                      ");
            parameter.AppendSql("  FROM ADMIN.AEMRFORM F                   ");
            parameter.AppendSql(" INNER JOIN ADMIN.AEASFORMCONTENT B       ");
            parameter.AppendSql("    ON F.FORMNO = B.FORMNO                     ");
            parameter.AppendSql("   AND F.UPDATENO = B.UPDATENO                 ");
            parameter.AppendSql(" INNER JOIN ADMIN.AEASFORMDATA C          ");
            parameter.AppendSql("    ON B.ID = C.EASFORMCONTENT                 ");
            parameter.AppendSql(" WHERE C.PTNO      = :PTNO                     ");
            parameter.AppendSql("   AND F.FORMNO    = :FORMNO                   ");
            parameter.AppendSql("   AND C.MEDFRDATE = :MEDFRDATE                ");
            parameter.AppendSql("   AND C.ISDELETED = 'N'                       ");
            parameter.AppendSql("   AND C.INOUTCLS  = 'O'                       ");
            parameter.AppendSql("   AND C.MEDDEPTCD = :MEDDEPTCD                ");

            parameter.Add("PTNO", strPtno);
            parameter.Add("FORMNO", fORMNO);
            parameter.Add("MEDFRDATE", strDate);
            parameter.Add("MEDDEPTCD", strDept);

            return ExecuteScalar<int>(parameter);
        }

        public int UpDateD10PageSetByWrtno(HIC_CONSENT item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_CONSENT SET     ");
            parameter.AppendSql("       PAGECNT = 3                     ");
            parameter.AppendSql("WHERE PTNO     = :PTNO                 ");
            parameter.AppendSql("  AND FORMCODE = 'D10'                 ");
            parameter.AppendSql("  AND PAGECNT  = 1                     ");
            parameter.AppendSql("  AND WRTNO    =:WRTNO                 ");

            parameter.Add("PTNO", item.PTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateItem(HIC_CONSENT item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_CONSENT SET             ");
            parameter.AppendSql("       DRSABUN      = :DRSABUN                 ");
            parameter.AppendSql("     , DOCTSIGN     = SYSDATE                  ");
            parameter.AppendSql("     , CONSENT_TIME = SYSDATE                  ");
            parameter.AppendSql("WHERE PTNO          = :PTNO                    ");
            parameter.AppendSql("  AND FORMCODE = :FORMCODE                     ");
            parameter.AppendSql("  AND WRTNO         =:WRTNO                    ");

            parameter.Add("DRSABUN", item.DRSABUN);
            parameter.Add("PTNO", item.PTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FORMCODE", item.FORMCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_CONSENT> GetListByItems(string argFDate, string argTDate, string argSName, string argDept, string argJob, long argWRTNO, List<string> lstFrmCD, string argForm)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT b.WRTNO, b.SNAME, b.FORMCODE, b.PAGECNT, b.DRSABUN, b.PTNO, b.DEPTCODE      ");
            parameter.AppendSql("     , b.ROWID AS RID , TO_CHAR(b.BDATE,'YYYY-MM-DD') BDATE, b.ENTDATE, b.DOCTSIGN ");
            parameter.AppendSql("     , b.DRSABUN, ADMIN.FC_INSA_MST_KORNAME(b.DRSABUN) DRNAME, b.ASA, b.EMRNO ");
            parameter.AppendSql("     , (SELECT FORMNAME FROM ADMIN.HIC_CONSENT_FORM                          ");
            parameter.AppendSql("       WHERE FORMCODE = b.FORMCODE) FORMNAME, TO_CHAR(b.CONSENT_TIME, 'YYYY-MM-DD HH24:MI') CONSENT_TIME ");
            if (argDept == "HEA")
            {
                if (argJob == "NEW")
                {
                    parameter.AppendSql("     , TO_CHAR(a.SDATE, 'YYYY-MM-DD') SDATE                        ");
                }
                else
                {
                    parameter.AppendSql("     , TO_CHAR(b.SDATE, 'YYYY-MM-DD') SDATE                        ");
                }

                parameter.AppendSql("  FROM ADMIN.HEA_JEPSU a, ADMIN.HIC_CONSENT b              ");
                parameter.AppendSql(" WHERE b.SDate >= TO_DATE(:FDATE, 'YYYY-MM-DD')                        ");
                parameter.AppendSql("   AND b.SDate <= TO_DATE(:TDATE, 'YYYY-MM-DD')                        ");
            }
            else
            {
                if (argJob == "NEW")
                {
                    parameter.AppendSql("     , TO_CHAR(a.JEPDATE, 'YYYY-MM-DD') SDATE                      ");
                }
                else
                {
                    parameter.AppendSql("     , TO_CHAR(b.SDATE, 'YYYY-MM-DD') SDATE                        ");
                }

                parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_CONSENT b              ");
                parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FDATE, 'YYYY-MM-DD')                      ");
                parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TDATE, 'YYYY-MM-DD')                      ");
            }

            if (argJob == "NEW")
            {
                parameter.AppendSql("   AND b.CONSENT_TIME IS NULL                          ");
            }
            else
            {
                parameter.AppendSql("   AND b.CONSENT_TIME IS NOT NULL                      ");
                parameter.AppendSql("   AND b.DELDATE IS NULL                               ");
            }

            parameter.AppendSql("   AND b.FORMCODE IS NOT NULL                                              ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                ");

            if (lstFrmCD.Count > 0)
            {
                parameter.AppendSql("   AND b.FORMCODE IN (:FORMCODE)                                       ");
            }

            if (argWRTNO > 0)
            {
                parameter.AppendSql("   AND b.WRTNO =:WRTNO                                 ");
            }

            if (!argSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME LIKE (:SNAME)                           ");
            }

            if (!argForm.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND b.FORMCODE = :FORM                              ");
            }

            parameter.AppendSql(" ORDER BY a.SNAME                            ");

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);

            if (lstFrmCD.Count > 0)
            {
                parameter.AddInStatement("FORMCODE", lstFrmCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (argWRTNO > 0)
            {
                parameter.Add("WRTNO", argWRTNO);
            }

            if (!argSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", argSName);
            }
            if (!argForm.IsNullOrEmpty())
            {
                parameter.Add("FORM", argForm);
            }

            return ExecuteReader<HIC_CONSENT>(parameter);
        }

        public void UpDateItem(HIC_CONSENT nHC, string strRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_CONSENT                 ");
            parameter.AppendSql("   SET DELDATE = ''                            ");
            parameter.AppendSql("      ,WRTNO =:WRTNO                           ");
            parameter.AppendSql("      ,SNAME =:SNAME                           ");
            parameter.AppendSql("      ,SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql(" WHERE ROWID =:RID                         ");

            parameter.Add("WRTNO", nHC.WRTNO);
            parameter.Add("SNAME", nHC.SNAME);
            parameter.Add("SDATE", nHC.SDATE);
            parameter.Add("RID", strRowid);

            ExecuteNonQuery(parameter);
        }

        public string GetRowidByPtnoDeptFormCD(string argPtno, string argDept, string argForm)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                           ");
            parameter.AppendSql("  FROM ADMIN.HIC_CONSENT         ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                    ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE            ");
            parameter.AppendSql("   AND FORMCODE=:FORMCODE              ");
            parameter.AppendSql("   AND SDATE >= TRUNC(SYSDATE-30)      ");
            parameter.AppendSql("   AND DOCTSIGN IS NULL                ");

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DEPTCODE", argDept, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FORMCODE", argForm, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetRowidByPtnoDeptFormCDWrtno(string argPtno, string argDept, string argForm, long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                           ");
            parameter.AppendSql("  FROM ADMIN.HIC_CONSENT         ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                    ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE            ");
            parameter.AppendSql("   AND FORMCODE=:FORMCODE              ");
            parameter.AppendSql("   AND DOCTSIGN IS NOT NULL                ");

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DEPTCODE", argDept, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FORMCODE", argForm, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", argWrtno);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_CONSENT> GetItembyWrtNo(long nWRTNO, int nCol)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT DEPTCODE, FORMCODE, PAGECNT                 ");
            parameter.AppendSql("  FROM ADMIN.HIC_CONSENT                     ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                           ");
            if (nCol== 4)
            {
                parameter.AppendSql("   AND FormCode = 'D10'                        ");
            }
            else if (nCol == 5)
            {
                parameter.AppendSql("   AND FormCode = 'D20'                        ");
            }
            else if (nCol == 6)
            {
                parameter.AppendSql("   AND FormCode = 'D30'                        ");
            }
            else if (nCol == 7)
            {
                parameter.AppendSql("   AND FormCode = 'D40'                        ");
            }
            parameter.AppendSql("   AND FormCode NOT IN('D50','D51','D52','D53')    ");
            parameter.AppendSql("   AND Consent_Time IS NOT NULL                    ");
            parameter.AppendSql(" GROUP BY DEPTCODE, FORMCODE, PAGECNT              ");
            parameter.AppendSql(" ORDER BY FormCode                                 ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_CONSENT>(parameter);
        }

        public int GetCountbyWrtNoSDate(long fnWRTNO, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM ADMIN.HIC_CONSENT                     ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                           ");
            parameter.AppendSql("   AND SDATE    = TO_DATE(:SDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE = 'HR'                             ");
            parameter.AppendSql("   AND FORMCODE IN ('D10','D20','D54')              ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("SDATE", fstrJepDate);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateDocSignbyRowId(string strDocSign, string strRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_CONSENT                             ");
            parameter.AppendSql("   SET DOCTSIGN = TO_DATE(:DOCTSIGN, 'YYYY-MM-DD HH24:MI') ");
            parameter.AppendSql(" WHERE ROWID    = :RID                                     ");

            parameter.Add("DOCTSIGN", strDocSign);
            parameter.Add("RID", strRowId);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_CONSENT> GetItembySabun(string idNumber, string strFrDate, string strToDate, string strJob)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,SName,FormCode,PageCnt,DrSabun,Ptno,DeptCode,ROWID AS RID     ");
            parameter.AppendSql("     , TO_CHAR(BDate,'YYYY-MM-DD') BDate                                   ");
            parameter.AppendSql("     , TO_CHAR(EntDate,'YYYY-MM-DD') EntDate                               ");
            parameter.AppendSql("     , TO_CHAR(DoctSign,'YYYY-MM-DD HH24:MI') DoctSign                     ");
            parameter.AppendSql("     , ADMIN.FC_INSA_MST_KORNAME(a.DRSABUN) DRNAME                    ");
            parameter.AppendSql("     , ROWID                                                               ");
            parameter.AppendSql("     , (SELECT FORMNAME                                                    ");
            parameter.AppendSql("          FROM ADMIN.HIC_CONSENT_FORM                                ");
            parameter.AppendSql("         WHERE FORMCODE = a.FORMCODE) FORMNAME                             ");
            parameter.AppendSql("  FROM ADMIN.HIC_CONSENT a                                           ");
            parameter.AppendSql(" WHERE 1=1                                                                 ");
            if (!idNumber.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND DRSABUN = :DRSABUN                                              ");
            }
            parameter.AppendSql("   AND ENTDATE >= TO_DATE(:FRDATE, 'yyyy-mm-dd')                           ");
            parameter.AppendSql("   AND ENTDATE <  TO_DATE(:TODATE, 'yyyy-mm-dd')                           ");


            //주석풀기----------------------
            //if (strJob == "1")
            //{
            //    parameter.AppendSql("   AND DoctSign IS NULL                                                ");
            //}
            //D10 접수용 임시사용을 위해 추가
            parameter.AppendSql("  AND FORMCODE = 'D10'                                                     ");
            parameter.AppendSql("   AND DoctSign IS NOT NULL                                                ");


            parameter.AppendSql(" ORDER BY SName,WRTNO,FormCode ");

            if (!idNumber.IsNullOrEmpty())
            {
                parameter.Add("DRSABUN", idNumber);
            }
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReader<HIC_CONSENT>(parameter);
        }

        public HIC_CONSENT GetIetmByPtnoSdateDeptForm(string argPTNO, string argSDATE, string[] argDEPTCODE, string argFORMCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DOCTSIGN                                ");
            parameter.AppendSql("  FROM ADMIN.HIC_CONSENT                 ");
            parameter.AppendSql(" WHERE 1 =1                                    ");
            parameter.AppendSql(" AND PTNO = :PTNO                              ");
            parameter.AppendSql(" AND SDATE = TO_DATE(:SDATE,'YYYY-MM-DD')      ");
            parameter.AppendSql(" AND DEPTCODE IN (:DEPTCODE)                  ");
            parameter.AppendSql(" AND FORMCODE = :FROMCODE                      ");
            parameter.AppendSql(" AND DRSABUN IS NOT NULL                       ");

            parameter.Add("PTNO", argPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SDATE", argSDATE);
            parameter.AddInStatement("DEPTCODE", argDEPTCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FROMCODE", argFORMCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_CONSENT>(parameter);
        }

        public HIC_CONSENT GetIetmByPtnoForm(string argPTNO, string argFORMCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT FILENAME1                               ");
            parameter.AppendSql("  FROM ADMIN.HIC_CONSENT                 ");
            parameter.AppendSql(" WHERE 1 =1                                    ");
            parameter.AppendSql(" AND PTNO = :PTNO                              ");
            parameter.AppendSql(" AND FORMCODE = :FROMCODE                      ");
            parameter.AppendSql(" AND SENDTIME IS NOT NULL                      ");
            parameter.AppendSql(" ORDER BY SDATE                                ");

            parameter.Add("PTNO", argPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FROMCODE", argFORMCODE);

            return ExecuteReaderSingle<HIC_CONSENT>(parameter);
        }

        public int UpdateItemByWrtno(string argPtno, long argWrtno, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_CONSENT                                 ");
            if (argGubun =="OK")
            {
                parameter.AppendSql("   SET DRSABUN ='111'                                      ");
                parameter.AppendSql(" , DOCTSIGN = SYSDATE                                      ");
            }
            else
            {
                parameter.AppendSql("   SET DRSABUN =''                                         ");
                parameter.AppendSql(" , DOCTSIGN = ''                                           ");
                parameter.AppendSql(" , CONSENT_TIME = ''                                       ");
            }
            parameter.AppendSql(" WHERE PTNO = :PTNO                                            ");
            parameter.AppendSql(" AND WRTNO = :WRTNO                                            ");

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", argWrtno);

            return ExecuteNonQuery(parameter);
        }
        public int UpdateTimeByWrtnoForm(string argSDate, long argWrtno, string argPtno, string argForm)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_CONSENT                                 ");
            parameter.AppendSql("   SET SDATE =TO_DATE(:SDATE, 'YYYY-MM-DD')                    ");
            parameter.AppendSql(" , CONSENT_TIME = SYSDATE                                      ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                            ");
            parameter.AppendSql(" AND FORMCODE = :FORM                                          ");
            parameter.AppendSql(" AND WRTNO = :WRTNO                                            ");

            parameter.Add("SDATE", argSDate);
            parameter.Add("WRTNO", argWrtno);
            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FORM", argForm, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

    }
}
