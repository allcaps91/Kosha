namespace HC.OSHA.Site.Management.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.OSHA.Site.Management.Dto;
    using HC.OSHA.Site.Management.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaContractManagerRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public List<HC_OSHA_CONTRACT_MANAGER_MODEL> FindContractManager(long estimageId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.*,  B.ESTIMATE_ID, B.WORKER_ID FROM HC_SITE_WORKER A                                    ");
            parameter.AppendSql("INNER JOIN HC_OSHA_CONTRACT_MANAGER B                                    ");
            parameter.AppendSql("ON A.ID = B.WORKER_ID                                    ");
            parameter.AppendSql("AND B.ISDELETED = 'N'                                    ");
            parameter.AppendSql("WHERE B.ESTIMATE_ID = :ESTIMATE_ID                                    ");
            parameter.Add("ESTIMATE_ID", estimageId);
           return ExecuteReader<HC_OSHA_CONTRACT_MANAGER_MODEL>(parameter);
        }

        public void Insert(HC_OSHA_CONTRACT_MANAGER dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HC_OSHA_CONTRACT_MANAGER                                        ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  WORKER_ID,                                                                ");
            parameter.AppendSql("  WORKER_ROLE,                                                              ");
            parameter.AppendSql("  ISDELETED                                                               ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :ESTIMATE_ID,                                                             ");
            parameter.AppendSql("  :WORKER_ID,                                                               ");
            parameter.AppendSql("  :WORKER_ROLE,                                                             ");
            parameter.AppendSql("  'N'                                                                       ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("WORKER_ID", dto.WORKER_ID);
            parameter.Add("WORKER_ROLE", dto.WORKER_ROLE);
            ExecuteNonQuery(parameter);
        }

        public void Delete(HC_OSHA_CONTRACT_MANAGER dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_OSHA_CONTRACT_MANAGER                                        ");
            parameter.AppendSql("   SET ISDELETED = 'Y'                                                                      ");
            parameter.AppendSql("WHERE ESTIMATE_ID = :ESTIMATE_ID                                                                      ");
            parameter.AppendSql("AND WORKER_ID = :WORKER_ID                                                                      ");

            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("WORKER_ID", dto.WORKER_ID);

            ExecuteNonQuery(parameter);
        }
        



    }
}

