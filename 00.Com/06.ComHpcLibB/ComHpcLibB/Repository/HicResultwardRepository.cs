namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicResultwardRepository : BaseRepository
    {   
        /// <summary>
        /// 
        /// </summary>
        public HicResultwardRepository()
        {
        }

        public string Read_WardName(string CODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WARDNAME                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULTWARD  ");
            parameter.AppendSql(" WHERE CODE = :CODE                ");
            parameter.AppendSql("   AND GUBUN = '01'                ");

            parameter.Add("CODE", CODE, Oracle.DataAccess.Client.OracleDbType.Char);
            
            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_RESULTWARD> GetItembyGubun(string fstrGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, GUBUN2, WARDNAME, SEQNO, EXAMWARD, ROWID  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULTWARD                      ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                                  ");
            parameter.AppendSql(" ORDER BY SEQNO, CODE, GUBUN                           ");

            parameter.Add("GUBUN", fstrGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULTWARD>(parameter);
        }

        public List<HIC_RESULTWARD> GetItembyGubunExamWardGubun2(string fstrGubun, string fstrExam, string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WardName, ROWID, GUBUN2,SEQNO       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULTWARD          ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                      ");
            parameter.AppendSql("   AND EXAMWARD = :EXAMWARD                ");
            if (strGubun != "*")
            {
                parameter.AppendSql("   AND GUBUN2 = :GUBUN2                ");
            }
            parameter.AppendSql(" ORDER BY SeqNo                            ");

            parameter.Add("GUBUN", fstrGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("EXAMWARD", fstrExam, Oracle.DataAccess.Client.OracleDbType.Char);
            if (strGubun != "*")
            {
                parameter.Add("GUBUN2", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_RESULTWARD>(parameter);
        }

        public int Update(string strROWID, string strCODE, string strName)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULTWARD SET  ");
            parameter.AppendSql("       CODE     = :CODE                ");
            parameter.AppendSql("     , WARDNAME = :WARDNAME            ");
            parameter.AppendSql(" WHERE ROWID    = :RID                 ");

            parameter.Add("CODE", strCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("WARDNAME", strName);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(string idNumber, string strCODE, string strName, string fstrGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_RESULTWARD ");
            parameter.AppendSql("      (SABUN, CODE, WARDNAME, GUBUN)       ");
            parameter.AppendSql("VALUES                                     ");
            parameter.AppendSql("      (:SABUN, :CODE, :WARDNAME, :GUBUN)   ");

            parameter.Add("SABUN", idNumber);
            parameter.Add("CODE", strCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("WARDNAME", strName);
            parameter.Add("GUBUN", fstrGbn, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int DeletebyRowId(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_RESULTWARD ");
            parameter.AppendSql(" WHERE ROWID = :RID               ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }
    }
}
