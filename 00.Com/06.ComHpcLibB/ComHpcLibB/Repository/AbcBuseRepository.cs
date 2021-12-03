namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class AbcBuseRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public AbcBuseRepository()
        {
        }

        public List<ABC_BUSE> BuseList()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.BuCode,b.Name                 ");
            parameter.AppendSql("  FROM ADMIN.HIC_ExJong a,       ");
            parameter.AppendSql("       ADMIN.BAS_BUSE b          ");
            parameter.AppendSql(" WHERE a.BuCode=b.AbcBuCode            ");
            parameter.AppendSql(" GROUP BY  a.BuCode,b.Name             ");

            return ExecuteReader<ABC_BUSE>(parameter);
        }
    }
}
