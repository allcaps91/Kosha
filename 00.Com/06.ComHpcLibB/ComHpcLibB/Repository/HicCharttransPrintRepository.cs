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
    public class HicCharttransPrintRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicCharttransPrintRepository()
        {
        }

        public HIC_CHARTTRANS_PRINT GetJobTimebyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JOBTIME                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CHARTTRANS_PRINT            ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReaderSingle<HIC_CHARTTRANS_PRINT>(parameter);
        }

        public string GetRowidByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CHARTTRANS_PRINT            ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public void UpDate(string argLtdName, long nWRTNO, string argSname)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HIC_CHARTTRANS_PRINT   ");
            parameter.AppendSql("    SET LTDNAME =:LTDNAME                  ");
            parameter.AppendSql("    ,SNAME =:SNAME                  ");
            parameter.AppendSql("  WHERE WRTNO =:WRTNO                      ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("LTDNAME", argLtdName);
            parameter.Add("SNAME", argSname);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_CHARTTRANS_PRINT> GetItembyJepDateJob(string strFrDate, string strToDate, string strSName, string strJob)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, JEPDATE, SNAME,  BIRTH, LTDNAME, ENTTIME, ENTSABUN, RECVTIME, RECVSABUN  ");
            parameter.AppendSql("     , JOBTIME, JOBSABUN, REMARK, GJJONG                                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CHARTTRANS_PRINT                                                ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                       ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                       ");
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SNAME = :SNAME                                                              ");
            }
            if (strJob == "1")
            {
                parameter.AppendSql("   AND JOBTIME IS NULL                                                             ");
            }
            else if (strJob == "2")
            {
                parameter.AppendSql("   AND JOBTIME IS NOT NULL                                                         ");
            }
            else if (strJob == "3")
            {
                parameter.AppendSql("   AND ENTTIME IS NULL                                                             ");
            }
            else if (strJob == "4")
            {
                parameter.AppendSql("   AND RECVTIME IS NULL                                                            ");
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            if (!strSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strSName);
            }

            return ExecuteReader<HIC_CHARTTRANS_PRINT>(parameter);
        }

        public int UpdateRemarkbyWrtNo(string strRemark, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HIC_CHARTTRANS_PRINT SET               ");            
            parameter.AppendSql("        REMARK = :REMARK                                   ");        
            parameter.AppendSql("  WHERE WRTNO  = :WRTNO                                    ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("REMARK", strRemark);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateJobbyWrtNo(string strDate, string strSysDate, string idNumber, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HIC_CHARTTRANS_PRINT SET                   ");
            if (strDate.IsNullOrEmpty())
            {
                parameter.AppendSql("        JOBTIME = TO_DATE(:ENTTIME, 'yyyy-mm-dd hh24:mi') ");
                parameter.AppendSql("      , JOBSABUN = :RECVSABUN                             ");
            }
            else
            {
                parameter.AppendSql("        JOBTIME = TO_DATE(:ENTTIME, 'yyyy-mm-dd hh24:mi') ");
                parameter.AppendSql("      , JOBSABUN = ''                                     ");
            }
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                                         ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("JOBTIME", strSysDate);
            if (strDate.IsNullOrEmpty())
            {
                parameter.Add("JOBSABUN", idNumber);
            }

            return ExecuteNonQuery(parameter);
        }

        public int UpdateRecvbyWrtNo(string strDate, string strSysDate, string idNumber, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HIC_CHARTTRANS_PRINT SET                   ");
            if (strDate.IsNullOrEmpty())
            {
                parameter.AppendSql("        RECVTIME = TO_DATE(:ENTTIME, 'yyyy-mm-dd hh24:mi') ");
                parameter.AppendSql("      , RECVSABUN = :RECVSABUN                             ");
            }
            else
            {
                parameter.AppendSql("        RECVTIME = TO_DATE(:ENTTIME, 'yyyy-mm-dd hh24:mi') ");
                parameter.AppendSql("      , RECVSABUN = ''                                     ");
            }
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                                         ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("RECVTIME", strSysDate);
            if (strDate.IsNullOrEmpty())
            {
                parameter.Add("RECVSABUN", idNumber);
            }

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyWrtNo(string strDate, string strSysDate, string idNumber, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HIC_CHARTTRANS_PRINT SET                   ");
            if (strDate.IsNullOrEmpty())
            {
                parameter.AppendSql("        ENTTIME = TO_DATE(:ENTTIME, 'yyyy-mm-dd hh24:mi')  ");
                parameter.AppendSql("      , ENTSABUN = :ENTSABUN                               ");
            }
            else
            {
                parameter.AppendSql("        ENTTIME = TO_DATE(:ENTTIME, 'yyyy-mm-dd hh24:mi')  ");
                parameter.AppendSql("      , ENTSABUN = ''                                      ");
            }
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                                         ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("ENTTIME", strSysDate);
            if (strDate.IsNullOrEmpty())
            {
                parameter.Add("ENTSABUN", idNumber);
            }

            return ExecuteNonQuery(parameter);
        }

        public void Delete(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" DELETE KOSMOS_PMPA.HIC_CHARTTRANS_PRINT   ");
            parameter.AppendSql("  WHERE WRTNO =:WRTNO                      ");

            parameter.Add("WRTNO", nWRTNO);

            ExecuteNonQuery(parameter);
        }

        public void Insert(HIC_CHARTTRANS_PRINT nHCP)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.HIC_CHARTTRANS_PRINT (            ");
            parameter.AppendSql("   WRTNO, JEPDATE, SNAME, BIRTH, LTDNAME, GJJONG           ");
            parameter.AppendSql(" ) VALUES (                                                ");
            parameter.AppendSql("  :WRTNO,:JEPDATE,:SNAME,:BIRTH,:LTDNAME,:GJJONG           ");
            parameter.AppendSql(" )                                                         ");

            parameter.Add("WRTNO",      nHCP.WRTNO);
            parameter.Add("JEPDATE",    nHCP.JEPDATE);
            parameter.Add("SNAME",      nHCP.SNAME);
            parameter.Add("BIRTH",      nHCP.BIRTH);
            parameter.Add("LTDNAME",    nHCP.LTDNAME);
            parameter.Add("GJJONG",     nHCP.GJJONG);

            ExecuteNonQuery(parameter);
        }
    }
}
