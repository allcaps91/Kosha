namespace HC_OSHA.Migration
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.OSHA.Model;


    /// <summary>
    /// 
    /// </summary>
    public class MigrationRepository : BaseRepository
    {
        public List<SiteMaxModel> GetMaxContract(long siteid)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT YYYYMM  FROM ADMIN.HIC_BOGENLTDSET ");
            parameter.AppendSql("       WHERE LtdCode = :SITEID ");
            parameter.AppendSql("   AND(DELDATE IS NULL OR DELDATE = '') ");
            parameter.AppendSql("  ORDER BY YYYYMM DESC ");

            parameter.Add("SITEID", siteid);

            return ExecuteReader<SiteMaxModel>(parameter);
        }
    }
}
