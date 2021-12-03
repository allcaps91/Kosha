namespace HC.OSHA.Site.Management.Repository
{
    using ComBase.Mvc;
    using HC.OSHA.Site.Management.Model;
    using System.Collections.Generic;
    using HC.Core.Site.Dto;


    public class HcOshaSiteModelRepository : BaseRepository
    {
        public HC_OSHA_SITE_MODEL FindById(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT B.*, A.ISACTIVE FROM HC_OSHA_SITE A ");
            parameter.AppendSql("INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("ON A.ID = B.ID       ");
            parameter.AppendSql("WHERE A.ID LIKE :ID     ");
            parameter.AddLikeStatement("ID", id);
            return ExecuteReaderSingle<HC_OSHA_SITE_MODEL>(parameter);
        }

        public List<HC_OSHA_SITE_MODEL> FindById(string id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT B.*, A.ISACTIVE FROM HC_OSHA_SITE A ");
            parameter.AppendSql("INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("ON A.ID = B.ID       ");
            parameter.AppendSql("WHERE A.ID LIKE :ID     ");
            parameter.AddLikeStatement("ID", id);
            return ExecuteReader<HC_OSHA_SITE_MODEL>(parameter);
        }


        public List<HC_OSHA_SITE_MODEL> FindByName(string name)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT B.*, A.ISACTIVE FROM HC_OSHA_SITE A ");
            parameter.AppendSql("INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("ON A.ID = B.ID       ");
            parameter.AppendSql("WHERE B.NAME LIKE :NAME     ");
            parameter.AddLikeStatement("NAME", name);

            return ExecuteReader<HC_OSHA_SITE_MODEL>(parameter);
        }
    }
}
