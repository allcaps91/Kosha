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
    public class HeaResultwardRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaResultwardRepository()
        {
        }

        public HEA_RESULTWARD GetWardNameByCode(string argGbn, long nSabun, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WardName                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULTWARD  ");
            parameter.AppendSql(" WHERE GUBUN =:GUBUN              ");
            parameter.AppendSql("   AND SABUN =:SABUN              ");
            parameter.AppendSql("   AND CODE  =:CODE               ");

            parameter.Add("GUBUN", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("SABUN", nSabun);
            parameter.Add("CODE", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_RESULTWARD>(parameter);
        }

        public List<HEA_RESULTWARD> GetCodeNameBySabunGubunJong(string idNumber, string strGubun, string strJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, WARDNAME, ROWID           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULTWARD      ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND SABUN = :SABUN                  ");

            if (strJong != "" && strJong != "*")
            {
                parameter.AppendSql("   AND SUBSTR(CODE, 1) LIKE :JONG  ");
            }
            parameter.AppendSql("  ORDER BY CODE,GUBUN                  ");

            parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("SABUN", idNumber);
            if (strJong != "" && strJong != "*")
            {
                parameter.AddLikeStatement("JONG", strJong);
            }

            return ExecuteReader<HEA_RESULTWARD>(parameter);
        }

        public List<HEA_RESULTWARD> GetItembySabunCodeStep(long fnJobSabun, string strSts, string strGubun, string strStep)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SEQNO, CODE, WARDNAME, STEP, ROWID  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULTWARD          ");
            parameter.AppendSql(" WHERE SABUN = :SABUN                      ");
            parameter.AppendSql("   AND CODE  = :CODE                       ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                      ");
            if (strStep.Trim() != "전체" && strStep != "")
            {
                parameter.AppendSql("   AND STEP = :STEP                    "); 
            }
            parameter.AppendSql(" ORDER BY SEQNO ,STEP, CODE, GUBUN         ");

            parameter.Add("SABUN", fnJobSabun);
            parameter.Add("CODE", strSts, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            if (strStep.Trim() != "전체" && strStep != "")
            {
                parameter.Add("STEP", strStep, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HEA_RESULTWARD>(parameter);
        }

        public int UpdateWardNameStepSeqNobyRowId(string strROWID, string strWard, string strStep, int nSeqNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_RESULTWARD SET  ");
            parameter.AppendSql("       WARDNAME = :WARDNAME            ");
            parameter.AppendSql("     , STEP     = :STEP                ");
            parameter.AppendSql("     , SEQNO    = :SEQNO               ");
            parameter.AppendSql(" WHERE ROWID = :RID                    ");

            parameter.Add("WARDNAME", strWard);            
            parameter.Add("STEP", strStep, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("SEQNO", nSeqNo);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int InsertItem(string idNumber, int nSeqNo, string strCode, string strStep, string strGubun, string strWard)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_RESULTWARD                     ");
            parameter.AppendSql("       (SABUN, SEQNO, CODE, WARDNAME, GUBUN, STEP)         ");
            parameter.AppendSql("VALUES                                                     ");
            parameter.AppendSql("       (:SABUN, :SEQNO, :CODE, :WARDNAME, :GUBUN, :STEP)   ");

            parameter.Add("SABUN", idNumber);
            parameter.Add("SEQNO", nSeqNo);
            parameter.Add("CODE", strCode, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("WARDNAME", strWard);
            parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("STEP", strStep, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int DeletebyRowId(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HEA_RESULTWARD      ");
            parameter.AppendSql(" WHERE ROWID = :RID                    ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_RESULTWARD> GetItembySabuncodeGubun(string idNumber, string strCODE, string strGubun, string strStep = "", string strKeyWord = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SEQNO, CODE, WARDNAME, GUBUN, STEP  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULTWARD          ");
            parameter.AppendSql(" WHERE SABUN = :SABUN                      ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                      ");
            if (!strCODE.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND CODE = :CODE                    ");
            }
            if (!strStep.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND STEP = :STEP                    ");
            }

            if (!strKeyWord.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND WARDNAME LIKE :WARDNAME                    ");
            }

            parameter.AppendSql(" ORDER BY SEQNO, CODE,GUBUN                ");

            if (!strCODE.IsNullOrEmpty())
            {
                parameter.Add("CODE", strCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            }
            if (!strStep.IsNullOrEmpty())
            {
                parameter.Add("STEP", strStep);
            }

            if (!strKeyWord.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("WARDNAME", strKeyWord);
            }

            parameter.Add("SABUN", idNumber);                        
            parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_RESULTWARD>(parameter);
        }

        public int UpdateWardNamebyRowId(string strWard, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_RESULTWARD SET  ");
            parameter.AppendSql("       WARDNAME = :WARDNAME            ");
            parameter.AppendSql(" WHERE ROWID = :RID                    ");

            parameter.Add("WARDNAME", strWard);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int InsertCode(string idNumber, string strCODE, string strWard, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_RESULTWARD     ");
            parameter.AppendSql("       (SABUN, CODE, WARDNAME, GUBUN)      ");
            parameter.AppendSql("VALUES                                     ");
            parameter.AppendSql("       (:SABUN, :CODE, :WARDNAME, :GUBUN)  ");

            parameter.Add("SABUN", idNumber);
            parameter.Add("CODE", strCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("WARDNAME", strWard);
            parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int DeleteCode(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE FROM KOSMOS_PMPA.HEA_RESULTWARD ");
            parameter.AppendSql(" WHERE ROWID = :RID                    ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_RESULTWARD> GetCodeNameBySabunGubun(string idNumber, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, WARDNAME, ROWID       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULTWARD  ");
            parameter.AppendSql(" WHERE GUBUN =:GUBUN              ");
            parameter.AppendSql("   AND SABUN =:SABUN              ");

            parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("SABUN", idNumber);

            return ExecuteReader<HEA_RESULTWARD>(parameter);
        }
    }
}
