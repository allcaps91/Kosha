namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class EndoJupmstRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public EndoJupmstRepository()
        {
        }

        public List<ENDO_JUPMST> GetItembyPtNo(List<string> strList, string strChkRsltInput)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SNAME,PTNO,DEPTCODE,COUNT(*) CNT                                ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                                          ");
            parameter.AppendSql(" WHERE RDate >= TRUNC(SYSDATE)                                          ");
            parameter.AppendSql("   AND RDate <  TRUNC(SYSDATE+1)                                       ");
            if (strChkRsltInput == "N")
            {
                parameter.AppendSql("   AND (DeptCode = 'TO' OR (DeptCode = 'HR' AND BUSE = '044500'))  ");
            }
            else
            {
                parameter.AppendSql("   AND ResultDate IS NULL                                          ");
                parameter.AppendSql("   AND (DeptCode = 'TO' OR (DeptCode = 'HR' AND BUSE = '044500'))  ");
            }
            parameter.AppendSql("   AND GbSunap <> '*'                                                  ");//취소가 아닌것만
            if (strList.Count > 0)
            {
                parameter.AppendSql("   AND Ptno NOT IN (:LIST)                                         ");
            }
            parameter.AppendSql(" GROUP BY SName,Ptno,DeptCode                                          ");
            parameter.AppendSql(" ORDER BY SName,Ptno,DeptCode                                          ");

            if (strList.Count > 0)
            {
                parameter.AddInStatement("LIST", strList, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<ENDO_JUPMST>(parameter);
        }

        public int GetCountbyPtNoJDate(string argPtno, string argJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                  ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                            ");
            parameter.AppendSql("   AND JDATE = TO_DATE(:JDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND GBSUNAP <> '*'                          ");

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JDATE", argJepDate);

            return ExecuteScalar<int>(parameter);
        }

        public long GetResultDrCodebyPtNoBDateGroup(string fstrPano, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RESULTDRCODE                                                                    ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                                                          ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                                                    ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                                           ");
            parameter.AppendSql("   AND ORDERCODE IN ('OO440916','00440110','00440165','Q7703800','Q7701A', 'E7660GB')  ");
            parameter.AppendSql("   AND RESULTDATE IS NOT NULL                                                          ");
            parameter.AppendSql("   AND PACSNO IS NOT NULL                                                              ");
            parameter.AppendSql(" GROUP BY RESULTDRCODE                                                                 ");

            parameter.Add("PTNO", fstrPano, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", fstrJepDate);

            return ExecuteScalar<long>(parameter);
        }

        public List<ENDO_JUPMST> GetItembySName(string strSName, string sGubun)
        {
            MParameter parameter = CreateParameter();

            if (sGubun == "1")
            {
                parameter.AppendSql("SELECT *                                   ");
                parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST              ");
                parameter.AppendSql(" WHERE RDATE >= TRUNC(SYSDATE)             ");
                parameter.AppendSql("   AND RDATE <  TRUNC(SYSDATE) + 1         ");
                parameter.AppendSql("   AND DEPTCODE IN('TO','HR')              ");
                if (!strSName.IsNullOrEmpty())
                {
                    parameter.AppendSql(" AND SNAME = :SNAME                    ");
                }
                parameter.AppendSql(" AND GBSUNAP <> '*'                        ");
                parameter.AppendSql(" ORDER BY SNAME                            ");
            }
            else if (sGubun == "2")
            {
                parameter.AppendSql("SELECT SNAME, PTNO, DEPTCODE, BUSE         ");
                parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST              ");
                parameter.AppendSql(" WHERE RDATE >= TRUNC(SYSDATE)             ");
                parameter.AppendSql("   AND RDATE <  TRUNC(SYSDATE) + 1         ");
                parameter.AppendSql("   AND DEPTCODE IN('TO','HR')              ");
                if (!strSName.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND SNAME = :SNAME                  ");
                }
                parameter.AppendSql("   AND GBSUNAP <> '*'                      ");
                parameter.AppendSql(" GROUP BY SNAME, PTNO, DEPTCODE, BUSE      ");
                parameter.AppendSql(" ORDER BY SNAME                            ");
            }
            else if (sGubun == "3")
            {
                parameter.AppendSql("SELECT *                                   ");
                parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST              ");
                parameter.AppendSql(" WHERE RDATE >= TRUNC(SYSDATE)             ");
                parameter.AppendSql("   AND RDATE <  TRUNC(SYSDATE) + 1         ");
                parameter.AppendSql("   AND DEPTCODE IN('HR')                   ");
                if (!strSName.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND SNAME = :SNAME                  ");
                }
                parameter.AppendSql("   AND GBSUNAP <> '*'                      ");
                parameter.AppendSql(" ORDER BY SNAME                            ");
            }
            else if (sGubun == "4")
            {
                parameter.AppendSql("SELECT SNAME, PTNO, DEPTCODE, BUSE         ");
                parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST              ");
                parameter.AppendSql(" WHERE RDATE >= TRUNC(SYSDATE)             ");
                parameter.AppendSql("   AND RDATE <  TRUNC(SYSDATE) + 1         ");
                parameter.AppendSql("   AND DEPTCODE IN('HR')                   ");
                if (!strSName.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND SNAME = :SNAME                  ");
                }
                parameter.AppendSql("   AND GBSUNAP <> '*'                      ");
                parameter.AppendSql(" GROUP BY SNAME, PTNO, DEPTCODE, BUSE      ");
                parameter.AppendSql(" ORDER BY SNAME                            ");
            }
            
            if (!strSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strSName);
            }

            return ExecuteReader<ENDO_JUPMST>(parameter);
        }

        public void UpDateOrderCodeByPtnoRDate(string strOrderCode, string pTNO, string strExamDate, string strGbJob)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.ENDO_JUPMST SET                      ");
            parameter.AppendSql("       ORDERCODE  = :ORDERCODE                         ");
            parameter.AppendSql("      ,SENDDATE   = SYSDATE                            ");
            parameter.AppendSql("      ,PACSSEND   = '*'                                ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                                   ");
            parameter.AppendSql("   AND TRUNC(RDATE) = TO_DATE(:RDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE IN ('TO','HR')                         ");
            parameter.AppendSql("   AND GBSUNAP IN ('1','7')                            ");
            parameter.AppendSql("   AND GBJOB = :GBJOB                                  ");
            parameter.AppendSql("   AND ORDERCODE != :NORDERCODE                        ");

            parameter.Add("ORDERCODE", strOrderCode);
            parameter.Add("PTNO", pTNO);
            parameter.Add("RDATE", strExamDate.Substring(0, 10));
            parameter.Add("GBJOB", strGbJob);
            parameter.Add("NORDERCODE", strOrderCode);

            ExecuteNonQuery(parameter);
        }

        public List<ENDO_JUPMST> GetItembyPtNoBDate(string argPtno, string argSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUCODE,TO_CHAR(RESULTDATE,'MM/DD HH24:MI') RESULTDATE, ROWID AS RID ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                                              ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND PTNO = :PTNO                                                        ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                               ");
            parameter.AppendSql("   AND GBJOB IN ('2', '3')                                                 ");
            parameter.AppendSql("   AND GBSUNAP = '7'                                                       ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("BDATE", argSDate);

            return ExecuteReader<ENDO_JUPMST>(parameter);
        }

        public ENDO_JUPMST GetSeqNoRDateByPtnoJDateGbJob(COMHPC cHPC, string argDept, string argGbJob)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SEQNO,TO_CHAR(RDATE,'YYYY-MM-DD') RDATE             ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                              ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                        ");
            parameter.AppendSql("   AND JDATE >= TO_DATE(:JDATE, 'YYYY-MM-DD')              ");
            parameter.AppendSql("   AND RESULTDATE IS NOT NULL                              ");
            parameter.AppendSql("   AND GBJOB = :GBJOB                                      ");
            parameter.AppendSql("   AND GBSUNAP != '*'                                      ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                                ");
            parameter.AppendSql("   AND TRUNC(RDATE)!=TO_DATE('" + DateTime.Now.Year.ToString() + "-12-25','YYYY-MM-DD')    ");
            parameter.AppendSql(" ORDER BY RDATE DESC                                       ");

            parameter.Add("PTNO", cHPC.PTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JDATE", cHPC.JEPDATE);
            parameter.Add("GBJOB", argGbJob);
            parameter.Add("DEPTCODE", argDept, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<ENDO_JUPMST>(parameter);
        }

        public void UpDateOrderCodeBuseByRowid(string strOrderCode, string argBuse, string rID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.ENDO_JUPMST SET                      ");
            parameter.AppendSql("       ORDERCODE  = :ORDERCODE                         ");
            parameter.AppendSql("      ,SENDDATE   = SYSDATE                            ");
            parameter.AppendSql("      ,BUSE       = :BUSE                              ");
            parameter.AppendSql("      ,PACSSEND   = '*'                                ");
            parameter.AppendSql(" WHERE ROWID  = :RID                                   ");

            parameter.Add("ORDERCODE", strOrderCode);
            parameter.Add("BUSE", argBuse);
            parameter.Add("RID", rID);

            ExecuteNonQuery(parameter);
        }

        public ENDO_JUPMST GetItemByPtnoRDateGbJob(string pTNO, string strExamDate, string strGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID AS RID, ORDERCODE, BUSE, GBSUNAP          ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                          ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                    ");
            parameter.AppendSql("   AND TRUNC(RDATE) = TO_DATE(:RDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE IN ('TO', 'HR')                        ");
            parameter.AppendSql("   AND GBSUNAP IN ('1', '7')                           ");
            parameter.AppendSql("   AND GBJOB = :GBJOB                                  ");

            parameter.Add("PTNO", pTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RDATE", strExamDate.Substring(0, 10));
            parameter.Add("GBJOB", strGbn);

            return ExecuteReaderSingle<ENDO_JUPMST>(parameter);
        }

        public void InsertData(ENDO_JUPMST rEJ)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.ENDO_JUPMST (                           ");
            parameter.AppendSql("   PTNO,     JDATE,     ORDERCODE, ORDERNO,  GBJOB, RDATE      ");
            parameter.AppendSql("  ,DEPTCODE, DRCODE,    WARDCODE,  ROOMCODE, GBIO,  GBSUNAP    ");
            parameter.AppendSql("  ,AMT,      SEQNO,     JUPSUNAME, ENTDATE,  VDATE, SNAME      ");
            parameter.AppendSql("  ,SEX,      BIRTHDATE, PACSSEND,  SEQNUM,   BDATE, BUSE       ");
            parameter.AppendSql(" ) VALUES (                                                    ");
            parameter.AppendSql("  :PTNO, TO_DATE(:JDATE, 'YYYY-MM-DD'),:ORDERCODE,:ORDERNO, :GBJOB,:RDATE      ");
            parameter.AppendSql(" ,:DEPTCODE,:DRCODE,   :WARDCODE, :ROOMCODE,:GBIO, :GBSUNAP    ");
            parameter.AppendSql(" ,:AMT,ADMIN.ENDO_SEQNO.NEXTVAL,:JUPSUNAME,SYSDATE, TO_DATE(:VDATE, 'YYYY-MM-DD'),:SNAME      ");
            parameter.AppendSql(" ,:SEX, TO_DATE(:BIRTHDATE, 'YYYY-MM-DD'),:PACSSEND, :SEQNUM,  TO_DATE(:BDATE, 'YYYY-MM-DD')             ");
            parameter.AppendSql(" ,:BUSE)                                                             ");

            parameter.Add("PTNO", rEJ.PTNO);
            parameter.Add("JDATE", rEJ.JDATE.Substring(0, 10));
            parameter.Add("ORDERCODE", rEJ.ORDERCODE);
            parameter.Add("ORDERNO", rEJ.ORDERNO);
            parameter.Add("GBJOB", rEJ.GBJOB);
            parameter.Add("RDATE", rEJ.RDATE);
            parameter.Add("DEPTCODE", rEJ.DEPTCODE);
            parameter.Add("DRCODE", rEJ.DRCODE);
            parameter.Add("WARDCODE", rEJ.WARDCODE);
            parameter.Add("ROOMCODE", rEJ.ROOMCODE);
            parameter.Add("GBIO", rEJ.GBIO);
            parameter.Add("GBSUNAP", rEJ.GBSUNAP);
            parameter.Add("AMT", rEJ.AMT);
            parameter.Add("JUPSUNAME", rEJ.JUPSUNAME);
            parameter.Add("VDATE", rEJ.VDATE.Substring(0, 10));
            parameter.Add("SNAME", rEJ.SNAME);
            parameter.Add("SEX", rEJ.SEX);
            parameter.Add("BIRTHDATE", rEJ.BIRTHDATE);
            parameter.Add("PACSSEND", rEJ.PACSSEND);
            parameter.Add("SEQNUM", rEJ.SEQNUM);
            parameter.Add("BDATE", rEJ.BDATE.Substring(0, 10));
            parameter.Add("BUSE", rEJ.BUSE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            ExecuteNonQuery(parameter);
        }

        public string GetMaxSeqNum(string strFDate, string strTDate, string strJob)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MAX(SEQNUM)                                     ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                          ");
            parameter.AppendSql(" WHERE JDATE >= TO_DATE(:FJDATE, 'YYYY-MM-DD')         ");
            parameter.AppendSql("   AND JDATE <= TO_DATE(:TJDATE, 'YYYY-MM-DD')         ");
            parameter.AppendSql("   AND GBJOB = :GBJOB                                  ");

            parameter.Add("FJDATE", strFDate);
            parameter.Add("TJDATE", strTDate);
            parameter.Add("GBJOB", strJob);
            
            return ExecuteScalar<string>(parameter);
        }

        public string GetOrderCodeByPtnoRDate(string pTNO, string strExamDate, string strGbJob)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ORDERCODE                                       ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                          ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                    ");
            parameter.AppendSql("   AND TRUNC(RDATE) = TO_DATE(:RDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE IN ('TO','HR')                         ");
            parameter.AppendSql("   AND GBSUNAP IN ('1','7')                            ");
            parameter.AppendSql("   AND GBJOB = :GBJOB                                  ");

            parameter.Add("PTNO", pTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RDATE", strExamDate.Substring(0, 10));
            parameter.Add("GBJOB", strGbJob);

            return ExecuteScalar<string>(parameter);
        }

        public void UpDateDeptDrCodeByRowid(string argDeptCode, string argDrCode, string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.ENDO_JUPMST SET                  ");
            parameter.AppendSql("       DEPTCODE   = :DEPTCODE                      ");
            parameter.AppendSql("      ,DRCODE     = :DRCODE                        ");
            parameter.AppendSql("      ,PACSSEND   = '*'                            ");
            parameter.AppendSql(" WHERE ROWID  = :RID                               ");

            parameter.Add("DEPTCODE", argDeptCode);
            parameter.Add("DRCODE", argDrCode);
            parameter.Add("RID", argRowid);

            ExecuteNonQuery(parameter);
        }

        public List<ENDO_JUPMST> GetHcListByPtnoBDate(string strPtNo, string strBDATE, string strGbEndo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID AS RID, DEPTCODE, DRCODE, BUSE     ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                   ");
            parameter.AppendSql(" WHERE 1 = 1                                    ");
            parameter.AppendSql("   AND PTNO = :PTNO                             ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE IN ('TO', 'HR')                 ");
            if (strGbEndo == "1")
            {
                parameter.AppendSql("   AND ORDERCODE IN ('00440110','00440120') "); //위
            }
            else if (strGbEndo == "2")
            {
                parameter.AppendSql("   AND ORDERCODE IN ('00440165','OO440916','E7660GA','E7660GB') "); //대장
            }
            else
            {
                parameter.AppendSql("   AND ORDERCODE IN ('00440110','00440120','00440165','OO440916','E7660GA','E7660GB') "); //위+대장
            }

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strBDATE);
            
            return ExecuteReader<ENDO_JUPMST>(parameter);
        }

        public ENDO_JUPMST GetResultDrCodebyPtNoBDate(string fstrPano, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RESULTDRCODE                                                                    ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                                                          ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                                                    ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                                           ");
            parameter.AppendSql("   AND ORDERCODE IN ('00440110','00440120','00440120','Q7611800','Q7612800','Q7620B')  ");
            parameter.AppendSql("   AND RESULTDATE IS NOT NULL                                                          ");
            parameter.AppendSql("   AND GBSUNAP = '7'                                                                   ");
            parameter.Add("PTNO", fstrPano, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", fstrJepDate);

            return ExecuteReaderSingle<ENDO_JUPMST>(parameter);
        }

        public int GetCountbyPtNoRDate(string strFrDate, string strToDate, string strPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                  ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                            ");
            parameter.AppendSql("   AND ORDERCODE IN('00440110', '00440120')    ");
            parameter.AppendSql("   AND RDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND RDATE <  TO_DATE(:TODATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND BUSE = '044500'                         ");
            parameter.AppendSql("   AND GBSUNAP <> '*'                          ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteScalar<int>(parameter);
        }

        public string GetASAbyPtNoJepDate(string fstrPtno, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ASA                                     ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                  ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                            ");
            parameter.AppendSql("   AND RDATE >= TO_DATE(:RDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND DeptCode IN ('HR','TO')                 ");

            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RDATE", fstrJepDate);

            return ExecuteScalar<string>(parameter);
        }

        public int UPdateASAbyPtNoRDate(string strASA, string fstrPtno, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.ENDO_JUPMST SET                  ");
            parameter.AppendSql("       ASA   = :ASA                                ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                               ");
            parameter.AppendSql("   AND DeptCode IN('HR', 'TO')                     ");
            parameter.AppendSql("   AND RDATE >= TO_DATE(:RDATE,'YYYY-MM-DD')       ");

            parameter.Add("ASA", strASA, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RDATE", fstrJepDate);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateASAbyRowId(string strASA, string fstrROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.ENDO_JUPMST SET  ");
            parameter.AppendSql("       ASA   = :ASA                ");
            parameter.AppendSql(" WHERE ROWID = :RID                ");

            parameter.Add("ASA", strASA);
            parameter.Add("RID", fstrROWID);

            return ExecuteNonQuery(parameter);
        }

        public ENDO_JUPMST GetItembyPtNoGbSunap(string strPtNo, string strBDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GbSunap,ASA,ROWID AS RID FROM ADMIN.ENDO_JUPMST   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                    ");
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')                         ");
            parameter.AppendSql("   AND BDATE >= TO_DATE(:BDATE, 'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND GBSUNAP <> '*'                                  ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strBDate);

            return ExecuteReaderSingle<ENDO_JUPMST>(parameter);
        }

        public ENDO_JUPMST GetGbConbyPtno(string strPtNo, string strBDate, string strDept)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBCON_21, GBCON_31                          ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                      ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                               ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND GBSUNAP <> '*'                              ");
            parameter.AppendSql("   AND GBJOB = '2'                                 ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                        ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strBDate);
            parameter.Add("DEPTCODE", strDept, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<ENDO_JUPMST>(parameter);
        }

        public ENDO_JUPMST GetResultDatebyPtNo(string strPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(RESULTDATE,'YYYY-MM-DD hh24:mi:ss') RESULTDATE  ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                                  ");
            parameter.AppendSql(" WHERE RDATE >= TRUNC(SYSDATE)                                 ");
            parameter.AppendSql("   AND RDATE <  TRUNC(SYSDATE + 1)                             ");
            parameter.AppendSql("   AND DEPTCODE IN ('TO','HR')                                 ");
            parameter.AppendSql("   AND GbSunap <> '*'                                          ");
            parameter.AppendSql("   AND PTNO = :PTNO                                            ");
            parameter.AppendSql("   AND RESULTDATE IS NOT NULL                                  ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<ENDO_JUPMST>(parameter);
        }

        public ENDO_JUPMST GetResultDrcodebyPtNo(string strPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RESULTDRCODE ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                                  ");
            parameter.AppendSql(" WHERE RDATE >= TRUNC(SYSDATE)                                 ");
            parameter.AppendSql("   AND RDATE <  TRUNC(SYSDATE + 1)                             ");
            parameter.AppendSql("   AND DEPTCODE IN ('TO','HR')                                 ");
            parameter.AppendSql("   AND GbSunap <> '*'                                          ");
            parameter.AppendSql("   AND PTNO = :PTNO                                            ");
            parameter.AppendSql("   AND RESULTDATE IS NOT NULL                                  ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<ENDO_JUPMST>(parameter);
        }


        public List<ENDO_JUPMST> GetListbyBdateDeptcodeBuse(string strBDate, string strDeptCode, string strBuse)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PTNO, SNAME, RESULTDATE                  ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                   ");
            parameter.AppendSql(" WHERE 1=1                                      ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                     ");
            parameter.AppendSql("   AND BUSE = :BUSE                             ");
            parameter.AppendSql(" ORDER BY SNAME                                 ");

            parameter.Add("BDATE", strBDate);
            parameter.Add("DEPTCODE", strDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BUSE", strBuse);

            return ExecuteReader<ENDO_JUPMST>(parameter);
        }

        public int UpDateGbsunapByPtnoRDate(string argPtno, string argGbsunap, string argDeptcode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.ENDO_JUPMST SET                  ");
            parameter.AppendSql("       GBSUNAP   = :GBSUNAP,                       ");
            parameter.AppendSql("       PACSSEND   = '*'                            ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                               ");
            parameter.AppendSql("   AND DeptCode = :DEPTCODE                        ");
            parameter.AppendSql("   AND RDATE >= TRUNC(SYSDATE)                     ");
            parameter.AppendSql("   AND RDATE < TRUNC(SYSDATE)+1                    ");
            parameter.AppendSql("   AND GBSUNAP NOT IN ('7')                        ");

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSUNAP", argGbsunap, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DEPTCODE", argDeptcode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }
        public int UpDateGbsunapByPtnoRDate1(string argPtno, string argGbsunap, string argDeptcode, List<string> argGbjob, string argFRtime, string argTRtime)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.ENDO_JUPMST SET                              ");
            parameter.AppendSql("       GBSUNAP   = :GBSUNAP                                    ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                                           ");
            parameter.AppendSql("   AND DeptCode = :DEPTCODE                                    ");
            parameter.AppendSql("   AND GBJOB IN (:GBJOB)                                       ");
            parameter.AppendSql("   AND RDATE >= TO_DATE(:FRDATE , 'YYYY-MM-DD')                ");
            parameter.AppendSql("   AND RDATE <= TO_DATE(:TRDATE , 'YYYY-MM-DD HH24:MI')        ");
            parameter.AppendSql("   AND GBSUNAP NOT IN ('*','7')                                 ");

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSUNAP", argGbsunap, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DEPTCODE", argDeptcode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("GBJOB", argGbjob, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FRDATE", argFRtime);
            parameter.Add("TRDATE", argTRtime);


            return ExecuteNonQuery(parameter);
        }

        public List<ENDO_JUPMST> GetItembyPtNoBDateDept(string argPtno, string argSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RESULTDATE                                                          ");
            parameter.AppendSql("  FROM ADMIN.ENDO_JUPMST                                              ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND PTNO = :PTNO                                                        ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                               ");
            parameter.AppendSql("   AND GBJOB IN ('2', '3')                                                 ");
            parameter.AppendSql("   AND GBSUNAP = '7'                                                       ");
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')                                             ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("BDATE", argSDate);

            return ExecuteReader<ENDO_JUPMST>(parameter);
        }
    }
}
