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
    public class WorkNhicRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public WorkNhicRepository()
        {
        }

        public WORK_NHIC GetOneItemByNewData(string argGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JUMIN, JUMIN2, SNAME, YEAR, ROWID AS RID    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.WORK_NHIC                       ");
            parameter.AppendSql(" WHERE 1 = 1                                       "); 
            parameter.AppendSql("   AND RTIME >= TRUNC(SYSDATE)                     ");
            parameter.AppendSql("   AND CTIME IS NULL                               ");
            parameter.AppendSql("   AND GUBUN =:GUBUN                               ");
            parameter.AppendSql("   AND GBSTS  = '0'                                ");
            parameter.AppendSql("   AND ROWNUM = 1                                  ");

            parameter.Add("GUBUN", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<WORK_NHIC>(parameter);
        }

        public WORK_NHIC GetNhicInfo(string argGbn, string argJuminNo, string argSName, string argYear, string argGbSts)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RTIME,CTIME,GUBUN,PANO,SNAME,JUMIN,BI,PNAME,KIHO,GKIHO,BDATE,REL,JISA                               ");
            parameter.AppendSql("      ,YEAR, TRANS, FIRST, EKG, LIVER, CANCER11, CANCER12, CANCER21, CANCER22, CANCER31, CANCER32          ");
            parameter.AppendSql("      ,CANCER41, CANCER42, CANCER51, CANCER52, GBERROR, CANCER53, DDATE, JBUNHO, FDATE, TDATE              ");
            parameter.AppendSql("      ,CANJISA, ILLCODE1, REMARK, GJONG, GBCHK01, GBCHK01_CHUL, GBCHK01_NAME, GBCHK02, GBCHK02_CHUL        ");
            parameter.AppendSql("      ,GBCHK02_NAME, GBCHK03, GBCHK03_CHUL, GBCHK03_NAME, GBCHK04, GBCHK04_CHUL, GBCHK04_NAME              ");
            parameter.AppendSql("      ,GBCHK05, GBCHK05_CHUL, GBCHK05_NAME, GBCHK06, GBCHK06_CHUL, GBCHK06_NAME, GBCHK07                   ");
            parameter.AppendSql("      ,GBCHK07_CHUL, GBCHK07_NAME, GBCHK08, GBCHK08_CHUL, GBCHK08_NAME, GBSTS, LIVER2                      ");
            parameter.AppendSql("      ,SECOND, JUMIN2, CANCER61, CANCER62, GBCHK09, GBCHK09_CHUL, GBCHK09_NAME, LIVERC, FIRSTADD           ");
            parameter.AppendSql("      ,SECONDADD, DENTAL, EXAMA, EXAMD, EXAME, EXAMF, EXAMG, EXAMH, EXAMI, LUNG, GBCHK10, GBCHK10_CHUL     ");
            parameter.AppendSql("      ,GBCHK10_NAME, CANCER71, CANCER72                                                                    ");
            parameter.AppendSql("      ,GBCHK15, GBCHK15_CHUL, GBCHK15_NAME, GBCHK16, GBCHK16_CHUL, GBCHK16_NAME                            ");
            parameter.AppendSql("      ,GBCHK17,GBCHK17_CHUL,GBCHK17_NAME                                                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.WORK_NHIC                                                                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                               ");
            parameter.AppendSql("   AND JUMIN2  = :JUMIN2                                                                                   ");
            if (!argSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SNAME   = :SNAME                                                                                ");
            }

            if (!argGbn.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GUBUN   = :GUBUN                                                                                ");
            }
            parameter.AppendSql("   AND GBERROR = 'N'                                                                                       ");
            parameter.AppendSql("   AND GBSTS   = :GBSTS                                                                                    ");
            parameter.AppendSql("   AND YEAR    = :YEAR                                                                                     ");
            parameter.AppendSql("   AND RTIME   >= TRUNC(SYSDATE)                                                                           ");
            //parameter.AppendSql("   AND EXAMI IS NOT NULL                                                                                   ");
            parameter.AppendSql(" ORDER BY CTIME DESC                                                                                       ");

            parameter.Add("JUMIN2", argJuminNo);

            if (!argSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", argSName);
            }

            if (!argGbn.IsNullOrEmpty())
            {
                parameter.Add("GUBUN", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            parameter.Add("GBSTS", argGbSts, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("YEAR", argYear, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<WORK_NHIC>(parameter);
        }

        public string GetRowidByJumin2Rtime(string argJumin)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID FROM KOSMOS_PMPA.WORK_NHIC    ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2                    ");
            parameter.AppendSql("   AND TRUNC(RTIME) = TRUNC(SYSDATE)       ");
            parameter.AppendSql(" ORDER BY RTIME DESC                       ");

            parameter.Add("JUMIN2", argJumin);

            return ExecuteScalar<string>(parameter);
        }

        public WORK_NHIC GetNhicInfo_Am(string argGbn, string argJuminNo, string argSName, string argYear, string argGbSTS, string strLifeGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RTIME,CTIME,GUBUN,PANO,SNAME,JUMIN,BI,PNAME,KIHO,GKIHO,BDATE,REL,JISA                               ");
            parameter.AppendSql("     , YEAR, TRANS, FIRST, EKG, LIVER, CANCER11, CANCER12, CANCER21, CANCER22, CANCER31, CANCER32          ");
            parameter.AppendSql("     , CANCER41, CANCER42, CANCER51, CANCER52, GBERROR, CANCER53, DDATE, JBUNHO, FDATE, TDATE              ");
            parameter.AppendSql("     , CANJISA, ILLCODE1, REMARK, GJONG, GBCHK01, GBCHK01_CHUL, GBCHK01_NAME, GBCHK02, GBCHK02_CHUL        ");
            parameter.AppendSql("     , GBCHK02_NAME, GBCHK03, GBCHK03_CHUL, GBCHK03_NAME, GBCHK04, GBCHK04_CHUL, GBCHK04_NAME              ");
            parameter.AppendSql("     , GBCHK05, GBCHK05_CHUL, GBCHK05_NAME, GBCHK06, GBCHK06_CHUL, GBCHK06_NAME, GBCHK07                   ");
            parameter.AppendSql("     , GBCHK07_CHUL, GBCHK07_NAME, GBCHK08, GBCHK08_CHUL, GBCHK08_NAME, GBSTS, LIVER2                      ");
            parameter.AppendSql("     , SECOND, JUMIN2, CANCER61, CANCER62, GBCHK09, GBCHK09_CHUL, GBCHK09_NAME, LIVERC, FIRSTADD           ");
            parameter.AppendSql("     , SECONDADD, DENTAL, EXAMA, EXAMD, EXAME, EXAMF, EXAMG, EXAMH, EXAMI, LUNG, GBCHK10, GBCHK10_CHUL     ");
            parameter.AppendSql("     , GBCHK10_NAME, CANCER71, CANCER72, GBCHK15, GBCHK15_CHUL, GBCHK15_NAME                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.WORK_NHIC                                                                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                               ");
            parameter.AppendSql("   AND JUMIN2  = :JUMIN2                                                                                   ");
            if (!argSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SNAME   = :SNAME                                                                                ");
            }

            if (!argGbn.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GUBUN   = :GUBUN                                                                                ");
            }
            if (strLifeGubun == "N")
            {
                parameter.AppendSql("   AND (GJong IS NULL OR GJong NOT IN ('35','41','42','43','44','45','46' ) )                          ");
            }
            else
            {
                parameter.AppendSql("   AND (GJong IS NULL OR GJong IN ('35','41','42','43','44','45','46' ) )                              ");
            }
            parameter.AppendSql("   AND CTime >= TRUNC(SYSDATE-3)                                                                           ");
            parameter.AppendSql("   AND GBSTS   = :GBSTS                                                                                    ");
            parameter.AppendSql("   AND YEAR    = :YEAR                                                                                     ");
            parameter.AppendSql("   AND EXAMI IS NOT NULL                                                                                   ");
            parameter.AppendSql(" ORDER BY CTIME DESC                                                                                       ");

            parameter.Add("JUMIN2", argJuminNo);

            if (!argSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", argSName);
            }

            if (!argGbn.IsNullOrEmpty())
            {
                parameter.Add("GUBUN", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            parameter.Add("GBSTS", argGbSTS, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("YEAR", argYear, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<WORK_NHIC>(parameter);
        }

        public List<WORK_NHIC> GetItembyJumin2SName(string strJumin, string strSName, string strLifeGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RTIME,CTIME,GUBUN,PANO,SNAME,JUMIN,BI,PNAME,KIHO,GKIHO,BDATE,REL,JISA                               ");
            parameter.AppendSql("     , YEAR, TRANS, FIRST, EKG, LIVER, CANCER11, CANCER12, CANCER21, CANCER22, CANCER31, CANCER32          ");
            parameter.AppendSql("     , CANCER41, CANCER42, CANCER51, CANCER52, GBERROR, CANCER53, DDATE, JBUNHO, FDATE, TDATE              ");
            parameter.AppendSql("     , CANJISA, ILLCODE1, REMARK, GJONG, GBCHK01, GBCHK01_CHUL, GBCHK01_NAME, GBCHK02, GBCHK02_CHUL        ");
            parameter.AppendSql("     , GBCHK02_NAME, GBCHK03, GBCHK03_CHUL, GBCHK03_NAME, GBCHK04, GBCHK04_CHUL, GBCHK04_NAME              ");
            parameter.AppendSql("     , GBCHK05, GBCHK05_CHUL, GBCHK05_NAME, GBCHK06, GBCHK06_CHUL, GBCHK06_NAME, GBCHK07                   ");
            parameter.AppendSql("     , GBCHK07_CHUL, GBCHK07_NAME, GBCHK08, GBCHK08_CHUL, GBCHK08_NAME, GBSTS, LIVER2                      ");
            parameter.AppendSql("     , SECOND, JUMIN2, CANCER61, CANCER62, GBCHK09, GBCHK09_CHUL, GBCHK09_NAME, LIVERC, FIRSTADD           ");
            parameter.AppendSql("     , SECONDADD, DENTAL, EXAMA, EXAMD, EXAME, EXAMF, EXAMG, EXAMH, EXAMI, LUNG, GBCHK10, GBCHK10_CHUL     ");
            parameter.AppendSql("     , GBCHK10_NAME, CANCER71, CANCER72                                                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.WORK_NHIC                                                                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                               ");
            parameter.AppendSql("   AND JUMIN2  = :JUMIN2                                                                                   ");
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SNAME = :SNAME                                                                                  ");
            }
            parameter.AppendSql("   AND Gubun = 'A'                                                                                         "); //건강검진 대상자
            parameter.AppendSql("   AND CTime >= TRUNC(SYSDATE-3)                                                                           ");
            parameter.AppendSql("   AND GBSTS = '1'                                                                                         ");
            if (strLifeGubun == "N")
            {
                parameter.AppendSql("   AND (GJong IS NULL OR GJong NOT IN ('35','41','42','43','44','45','46' ) )                          ");
            }
            else
            {
                parameter.AppendSql("   AND (GJong IS NULL OR GJong IN ('35','41','42','43','44','45','46' ) )                              ");
            }
            parameter.AppendSql(" ORDER BY CTIME DESC                                                                                       ");

            parameter.Add("JUMIN2", strJumin);

            if (!strSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strSName);
            }

            return ExecuteReader<WORK_NHIC>(parameter);
        }

        public WORK_NHIC GetItembyJumin2(string argJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GKIHO,REL,YEAR,LIVER,LIVER2,JISA,FIRST, GBCHK01_NAME,GBCHK02_NAME,GBCHK03_NAME  ");
            parameter.AppendSql("     , GBCHK01,GBCHK03,GBCHK03, EXAMA, EXAME, EXAMG, EXAMF                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.WORK_NHIC                                                           ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2                                                                ");
            parameter.AppendSql("   AND GUBUN  = 'H'                                                                    "); //가접수 대상자
            parameter.AppendSql("   AND CTIME >= TRUNC(SYSDATE - 3)                                                     ");
            parameter.AppendSql("   AND GBSTS  = '1'                                                                    ");
            parameter.AppendSql(" ORDER BY CTIME DESC                                                                   ");

            parameter.Add("JUMIN2", argJumin2);

            return ExecuteReaderSingle<WORK_NHIC>(parameter);
        }

        public WORK_NHIC GetSNamebyJumin2(string argJumin, string argYear)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SName FROM KOSMOS_PMPA.WORK_NHIC    ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2                    ");
            parameter.AppendSql("   AND TRUNC(RTime)>=TRUNC(SYSDATE)        ");
            parameter.AppendSql("   AND GBSTS = '2'                         ");
            if (!argYear.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND YEAR = :YEAR                    ");
            }

            parameter.Add("JUMIN2", argJumin);
            parameter.Add("YEAR", argYear);

            return ExecuteReaderSingle<WORK_NHIC>(parameter);
        }

        public WORK_NHIC GetSNamebyJumin2Year(string argJumin, string argYear)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SName FROM KOSMOS_PMPA.WORK_NHIC                ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2                                ");
            parameter.AppendSql("   AND TRUNC(RTime) >= TRUNC(SYSDATE)                  ");
            parameter.AppendSql("   AND GBSTS = '0'                                     ");
            if (argYear.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND YEAR = :YEAR                                ");
            }
            parameter.AppendSql(" ORDER BY CTime DESC                                   ");

            parameter.Add("JUMIN2", argJumin);
            if (argYear.IsNullOrEmpty())
            {
                parameter.Add("YEAR", argYear);
            }

            return ExecuteReaderSingle<WORK_NHIC>(parameter);
        }

        public WORK_NHIC GetItembyJuminSName(string argJumin, string argSname)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GKiho,Rel,Year,Liver2,Jisa,First, GBCHK01_NAME,GBCHK02_NAME,GBCHK03_NAME    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.WORK_NHIC                                                       ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2                                                            ");
            parameter.AppendSql("   AND SNAME  = :SNAME                                                             ");
            parameter.AppendSql("   AND GUBUN  = 'H'                                                                ");
            parameter.AppendSql("   AND CTIME >= TRUNC(SYSDATE-3)                                                   ");
            parameter.AppendSql("   AND GBSTS  ='1'                                                                 ");
            parameter.AppendSql(" ORDER BY CTIME DESC                                                               ");

            parameter.Add("JUMIN2", argJumin);
            parameter.Add("SNAME", argSname);

            return ExecuteReaderSingle<WORK_NHIC>(parameter);
        }

        public void UpdateNewTarget(string argRid)
        {
            return;

            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.WORK_NHIC   ");
            parameter.AppendSql("   SET GBSTS = '0'             ");
            parameter.AppendSql("      ,GUBUN = 'H'             ");
            parameter.AppendSql(" WHERE ROWID = :RID            ");

            #region Query 변수대입
            parameter.Add("RID", argRid);
            #endregion

            ExecuteNonQuery(parameter);
        }

        public void UpDateError(string argRid)
        {
            return;

            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.WORK_NHIC   ");
            parameter.AppendSql("   SET GBERROR = 'Y'           ");
            parameter.AppendSql(" WHERE ROWID = :RID            ");

            #region Query 변수대입
            parameter.Add("RID", argRid);
            #endregion

            ExecuteNonQuery(parameter);
        }

        public int DeleteDataAllByJuminNo(string argJumin)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE FROM KOSMOS_PMPA.WORK_NHIC WHERE JUMIN2 = :JUMIN2   ");

            #region Query 변수대입
            parameter.Add("JUMIN2", argJumin);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public int InsertData(WORK_NHIC item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.WORK_NHIC (                            ");
            parameter.AppendSql("       RTIME,GUBUN,SNAME,JUMIN,JUMIN2,PANO,GBSTS,YEAR          ");
            parameter.AppendSql(") VALUES (                                                     ");
            parameter.AppendSql("       SYSDATE,:GUBUN,:SNAME,:JUMIN,:JUMIN2,:PANO,:GBSTS,:YEAR  ");
            parameter.AppendSql(")  ");

            #region Query 변수대입
            parameter.Add("GUBUN", item.GUBUN);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("JUMIN", item.JUMIN);
            parameter.Add("JUMIN2", item.JUMIN2);
            parameter.Add("PANO", item.PANO);
            parameter.Add("GBSTS", item.GBSTS);
            parameter.Add("YEAR", item.YEAR);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public WORK_NHIC GetNhicInfoByTime(string argGbn, string argJuminNo, string argSName, string argYear, string argGbSTS, string argTimeGb)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RTIME,CTIME,GUBUN,PANO,SNAME,JUMIN,BI,PNAME,KIHO,GKIHO,BDATE,REL,JISA                               ");
            parameter.AppendSql("      ,YEAR, TRANS, FIRST, EKG, LIVER, CANCER11, CANCER12, CANCER21, CANCER22, CANCER31, CANCER32          ");
            parameter.AppendSql("      ,CANCER41, CANCER42, CANCER51, CANCER52, GBERROR, CANCER53, DDATE, JBUNHO, FDATE, TDATE              ");
            parameter.AppendSql("      ,CANJISA, ILLCODE1, REMARK, GJONG, GBCHK01, GBCHK01_CHUL, GBCHK01_NAME, GBCHK02, GBCHK02_CHUL        ");
            parameter.AppendSql("      ,GBCHK02_NAME, GBCHK03, GBCHK03_CHUL, GBCHK03_NAME, GBCHK04, GBCHK04_CHUL, GBCHK04_NAME              ");
            parameter.AppendSql("      ,GBCHK05, GBCHK05_CHUL, GBCHK05_NAME, GBCHK06, GBCHK06_CHUL, GBCHK06_NAME, GBCHK07                   ");
            parameter.AppendSql("      ,GBCHK07_CHUL, GBCHK07_NAME, GBCHK08, GBCHK08_CHUL, GBCHK08_NAME, GBSTS, LIVER2                      ");
            parameter.AppendSql("      ,SECOND, JUMIN2, CANCER61, CANCER62, GBCHK09, GBCHK09_CHUL, GBCHK09_NAME, LIVERC, FIRSTADD           ");
            parameter.AppendSql("      ,SECONDADD, DENTAL, EXAMA, EXAMD, EXAME, EXAMF, EXAMG, EXAMH, EXAMI, LUNG, GBCHK10, GBCHK10_CHUL     ");
            parameter.AppendSql("      ,GBCHK10_NAME, CANCER71, CANCER72                                                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.WORK_NHIC                                                                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                               ");
            parameter.AppendSql("   AND JUMIN2  = :JUMIN2                                                                                   ");

            if (argTimeGb.Equals("C"))
            {
                parameter.AppendSql("   AND CTime>=TRUNC(SYSDATE)   ");
                parameter.AppendSql("   AND GBERROR = 'N'           ");
            }
            else
            {
                parameter.AppendSql("   AND TRUNC(RTime) >=TRUNC(SYSDATE) ");
            }

            if (!argSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SNAME   = :SNAME                                                                                    ");
            }

            if (!argGbn.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GUBUN   = :GUBUN                                                                                    ");
            }

            if (!argGbSTS.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GBSTS   = :GBSTS                                                                                       ");
            }

            parameter.AppendSql("   AND YEAR    = :YEAR                                                                                     ");
            parameter.AppendSql(" ORDER BY CTIME DESC                                                                                       ");

            parameter.Add("JUMIN2", argJuminNo);

            if (!argSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", argSName);
            }

            if (!argGbn.IsNullOrEmpty())
            {
                parameter.Add("GUBUN", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            if (!argGbSTS.IsNullOrEmpty())
            {
                parameter.Add("GBSTS", argGbSTS, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            parameter.Add("YEAR", argYear, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<WORK_NHIC>(parameter);
        }
    }
}
