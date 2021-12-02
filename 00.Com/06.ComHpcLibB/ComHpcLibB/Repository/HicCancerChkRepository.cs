namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicCancerChkRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicCancerChkRepository()
        {
        }

        public string GetRemarkbyPanoYear(long pANO, string strYear)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT REMARK                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_CHK      ");
            parameter.AppendSql(" WHERE PANO  = :PANO                   ");
            parameter.AppendSql("   AND YEAR  = :YEAR                   ");

            parameter.Add("PANO", pANO);
            parameter.Add("YEAR", strYear, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int Update(HIC_CANCER_CHK item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CANCER_CHK SET  ");
            parameter.AppendSql("       REMARK   = :REMARK              ");
            parameter.AppendSql("     , JOBSABUN = :JOBSABUN            ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE              ");
            parameter.AppendSql(" WHERE ROWID    = :ROWID               ");

            parameter.Add("PANO", item.PANO);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("JOBSABUN", item.JOBSABUN);
            parameter.Add("ROWID", item.ROWID);

            return ExecuteNonQuery(parameter);
        }

        public string GetRowIdbyPanoYear(string strPano, string strYear)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_CHK      ");
            parameter.AppendSql(" WHERE PANO  = :PANO                   ");
            parameter.AppendSql("   AND YEAR  = :YEAR                   ");

            parameter.Add("PANO", strPano);
            parameter.Add("YEAR", strYear, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int Insert(HIC_CANCER_CHK item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql("       (PANO, YEAR, REMARK, JOBSABUN, ENTTIME)     ");
            parameter.AppendSql("VALUES                                             ");
            parameter.AppendSql("       (:PANO, :YEAR, :REMARK, :JOBSABUN, SYSDATE) ");

            parameter.Add("PANO", item.PANO);
            parameter.Add("YEAR", item.YEAR);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("JOBSABUN", item.JOBSABUN);

            return ExecuteNonQuery(parameter);
        }
    }
}
