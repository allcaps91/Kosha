namespace HC.OSHA.Site.Management.Repository
{
    using ComBase.Mvc;
    using ComHpcLibB.Model;
    using HC.OSHA.Site.Management.Model;
    using HC.Core.Site.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaSiteRepository : BaseRepository
    {
        public HC_OSHA_SITE FindById(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_OSHA_SITE WHERE ID = :ID ");
            parameter.Add("ID", id);
            return ExecuteReaderSingle<HC_OSHA_SITE>(parameter);

        }

        public void Save(HC_SITE_VIEW view)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HC_OSHA_SITE   ");
            parameter.AppendSql("(ID, ISACTIVE)             ");
            parameter.AppendSql("VALUES(:ID, 'Y')             ");
           
            parameter.Add("ID", view.ID);
           
            ExecuteNonQuery(parameter);
        }

      

        
    }
}
