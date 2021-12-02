namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicResSahusangdamRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicResSahusangdamRepository()
        {
        }

        public int GetCountbyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_SAHUSANGDAM ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int DeletebyRowId(string fstrROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_RES_SAHUSANGDAM    ");
            parameter.AppendSql("  WHERE ROWID = :RID                       ");

            parameter.Add("RID", fstrROWID);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(long fnWRTNO, string fstrJepDate, string strSDate, string idNumber, string strSogen, string fstrPanjengGbn, string strGbn, string strRemark)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_RES_SAHUSANGDAM                            ");
            parameter.AppendSql("       (WRTNO, JEPDATE, SDATE, SABUN, SOGEN, PANJENGGBN, GBN, REMARK)  ");
            parameter.AppendSql("VALUES                                                                 ");
            parameter.AppendSql("       (:WRTNO                                                         ");
            parameter.AppendSql("     , TO_DATE(:JEPDATE, 'YYYY-MM-DD'), TO_DATE(:SDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("     , :SABUN, :SOGEN, :PANJENGGBN, :GBN, :REMARK)                     ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("JEPDATE", fstrJepDate);
            parameter.Add("SDATE", strSDate);
            parameter.Add("SABUN", idNumber);
            parameter.Add("SOGEN", strSogen);
            parameter.Add("PANJENGGBN", fstrPanjengGbn);
            parameter.Add("GBN", strGbn, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("REMARK", strRemark);

            return ExecuteNonQuery(parameter);
        }

        public long GetWrtNobyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_SAHUSANGDAM     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<long>(parameter);
        }

        public HIC_RES_SAHUSANGDAM GetItembyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, SABUN, SOGEN, GBN, REMARK, ROWID ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_SAHUSANGDAM                                     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                      ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_RES_SAHUSANGDAM>(parameter);
        }

        public int Update(string fstrJepDate, string strSDate, string fstrSogen, string fstrPanjengGbn, string strGbn, string argSabun, string strRemark, string fstrROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SAHUSANGDAM SET             ");
            parameter.AppendSql("       JEPDATE    = TO_DATE(:JEPDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("     , SDATE      = TO_DATE(:SDATE, 'YYYY-MM-DD')      ");
            parameter.AppendSql("     , SOGEN      = :SOGEN                             ");
            parameter.AppendSql("     , PANJENGGBN = :PANJENGGBN                        ");
            parameter.AppendSql("     , GBN        = :GBN                               ");
            parameter.AppendSql("     , SABUN      = :SABUN                               ");
            parameter.AppendSql("     , REMARK     = :REMARK                            ");
            parameter.AppendSql("WHERE ROWID       = :RID                               ");

            parameter.Add("JEPDATE", fstrJepDate);
            parameter.Add("SDATE", strSDate);
            parameter.Add("SOGEN", fstrSogen);
            parameter.Add("PANJENGGBN", fstrPanjengGbn);
            parameter.Add("GBN", strGbn, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SABUN", argSabun);
            parameter.Add("REMARK", strRemark);
            parameter.Add("RID", fstrROWID);

            return ExecuteNonQuery(parameter);
        }
    }
}
