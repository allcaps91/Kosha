namespace HC.Core.Site.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.Core.Site.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcSiteViewRepository : BaseRepository
    {
        
        public List<HC_SITE_VIEW> FindByName(string name)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT* FROM HC_SITE_VIEW   ");
            parameter.AppendSql(" WHERE NAME LIKE :NAME       ");
            parameter.AppendSql(" ORDER BY NAME ");

            parameter.AddLikeStatement("NAME", name);

            return ExecuteReader<HC_SITE_VIEW>(parameter);
        }

        public List<HC_SITE_VIEW> FindById(string id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT* FROM HC_SITE_VIEW   ");
            parameter.AppendSql(" WHERE ID LIKE :ID       ");
            parameter.AppendSql(" ORDER BY NAME ");

            parameter.AddLikeStatement("ID", id);

            return ExecuteReader<HC_SITE_VIEW>(parameter);
        }

        


    }
}
