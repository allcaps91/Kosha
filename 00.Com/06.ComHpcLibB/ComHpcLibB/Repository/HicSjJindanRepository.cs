namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicSjJindanRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSjJindanRepository()
        {
        }

        public int Insert(string strGjYear, long nLtdCode, string strPan, string strBuse, string strYuhe, string strJanggi, long nInwon)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SJ_JINDAN                                  ");
            parameter.AppendSql("       (GJYEAR, LTDCODE, PANJENG, BUSE, YUHE, JANGGI, INWON)           ");
            parameter.AppendSql("VALUES                                                                 ");
            parameter.AppendSql("       (:GJYEAR, :LTDCODE, :PANJENG, :BUSE, :YUHE, :JANGGI, :INWON)    ");

            parameter.Add("GJYEAR", strGjYear, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);
            parameter.Add("PANJENG", strPan);
            parameter.Add("BUSE", strBuse);
            parameter.Add("YUHE", strYuhe);
            parameter.Add("JANGGI", strJanggi);
            parameter.Add("INWON", nInwon);

            return ExecuteNonQuery(parameter);
        }

        public int DeletebyGjYearLtdCode(string strGjYear, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SJ_JINDAN           ");
            parameter.AppendSql(" WHERE GJYEAR  = :GJYEAR                   ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                  ");

            parameter.Add("GJYEAR", strGjYear, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_SJ_JINDAN> GetItembyGjYearLtdCode(string strGjYear, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Panjeng,Buse,Yuhe,Janggi,Inwon,ROWID RID    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SJ_JINDAN                   ");
            parameter.AppendSql(" WHERE GJYEAR  = :GJYEAR                           ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                          ");
            parameter.AppendSql(" ORDER BY PANJENG, BUSE                            ");

            parameter.Add("GJYEAR", strGjYear, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);

            return ExecuteReader<HIC_SJ_JINDAN>(parameter);
        }

        public int UpdateInwon(long nInwon, string rID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SJ_JINDAN SET   ");
            parameter.AppendSql("       INWON = :INWON                  ");
            parameter.AppendSql(" WHERE ROWID  = :RID                   ");

            parameter.Add("INWON", nInwon);
            parameter.Add("RID", rID);

            return ExecuteNonQuery(parameter);
        }

        public HIC_SJ_JINDAN GetInwonRowIdbyGjYear(string strGjYear, long nLtdCode, string strPanjeng, string strBuseName, string strYuhe)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID,Inwon FROM HIC_SJ_JINDAN  ");
            parameter.AppendSql(" WHERE GJYEAR  = :GJYEAR               ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE              ");
            parameter.AppendSql("   AND PANJENG = :PANJENG              ");
            parameter.AppendSql("   AND BUSE    = :BUSE                 ");
            parameter.AppendSql("   AND YUHE    = :YUHE                 ");

            parameter.Add("GJYEAR", strGjYear, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);
            parameter.Add("PANJENG", strPanjeng);
            parameter.Add("BUSE", strBuseName);
            parameter.Add("YUHE", strYuhe);

            return ExecuteReaderSingle<HIC_SJ_JINDAN>(parameter);
        }

        public int Update(string strPan, string strBuse, string strYuhe, string strJanggi, long nInwon, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SJ_JINDAN SET   ");
            parameter.AppendSql("     , PANJENG = :PANJENG              ");
            parameter.AppendSql("     , BUSE    = :BUSE                 ");            
            parameter.AppendSql("     , YUHE    = :YUHE                 ");            
            parameter.AppendSql("     , JANGGI  = :JANGGI               ");            
            parameter.AppendSql("     , INWON   = :INWON                ");
            parameter.AppendSql(" WHERE ROWID   = :RID                  ");

            parameter.Add("PANJENG", strPan); 
            parameter.Add("BUSE", strBuse);
            parameter.Add("YUHE", strYuhe);
            parameter.Add("JANGGI", strJanggi);
            parameter.Add("INWON", nInwon);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int Delete(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SJ_JINDAN   ");
            parameter.AppendSql(" WHERE ROWID = :RID                ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }
    }
}
