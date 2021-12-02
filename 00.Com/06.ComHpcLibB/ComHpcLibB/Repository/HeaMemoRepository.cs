namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaMemoRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaMemoRepository()
        {
        }

        public int Insert(HEA_MEMO item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_MEMO           ");
            parameter.AppendSql("       (WRTNO, MEMO, ENTTIME, JOBSABUN)    ");
            parameter.AppendSql("VALUES                                     ");
            parameter.AppendSql("       (:WRTNO, :MEMO, SYSDATE, :JOBSABUN) ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("MEMO", item.MEMO);
            parameter.Add("JOBSABUN", item.JOBSABUN);

            return ExecuteNonQuery(parameter);
        }

        public int Update(string strROWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE KOSMOS_PMPA.HEA_MEMO    ");
            parameter.AppendSql(" WHERE ROWID  = :RID           ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_MEMO> GetItembyPaNo(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MEMO,ENTTIME,JOBSABUN                                               ");
            parameter.AppendSql("      ,KOSMOS_OCS.FC_BAS_USER(JOBSABUN) AS JOBNAME,PTNO,ROWID AS RID       ");
            parameter.AppendSql("  From KOSMOS_PMPA.HEA_MEMO                                                ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                                        ");
            parameter.AppendSql("   AND MEMO IS NOT NULL                                                    ");
            parameter.AppendSql(" ORDER BY ENTTIME DESC                                                     ");

            parameter.Add("PTNO", argPtno);

            return ExecuteReader<HEA_MEMO>(parameter);
        }
    }
}
