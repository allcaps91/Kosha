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
    public class BasIllsRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public BasIllsRepository()
        {
        }

        public string GetIllNameKbyIllCode(string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ILLNAMEK                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_ILLS    ");
            parameter.AppendSql(" WHERE ILLCODE = :ILLCODE      ");

            parameter.Add("ILLCODE", argCode);

            return ExecuteScalar<string>(parameter);
        }

        public List<BAS_ILLS> GetListByIllNameK(string strName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT IllCode CODE, IllNameK NAME ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_ILLS        ");
            parameter.AppendSql(" WHERE ROWNUM <= 500               ");
            if (!strName.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND ILLNAMEK LIKE :ILLNAMEK   ");
            }
            parameter.AppendSql(" ORDER BY ILLCODE                  ");

            if (!strName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("ILLNAMEK", strName);
            }

            return ExecuteReader<BAS_ILLS>(parameter);
        }
    }
}
