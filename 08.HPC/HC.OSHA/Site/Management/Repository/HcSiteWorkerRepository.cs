namespace HC.OSHA.Site.Management.Repository
{
    using ComBase.Mvc;
    using HC.OSHA.Site.Management.Dto;
    using HC.Core.Common.Service;
    using System.Collections.Generic;


    /// <summary>
    /// 
    /// </summary>
    public class HcSiteWorkerRepository : BaseRepository
    {
        public List<HC_SITE_WORKER> FindAll(long siteId) {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_SITE_WORKER                                                  ");
            parameter.AppendSql("WHERE SITEID = :SITEID                                                                ");
            parameter.AppendSql("AND ISDELETED = 'N'                                                               ");

            parameter.Add("SITEID", siteId);

            return ExecuteReader<HC_SITE_WORKER>(parameter);

        }
        /// <summary>
        /// WORKER_ROLE 코드에 속한 근로자 가져오기
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public List<HC_SITE_WORKER> FindWorkerRole(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.* FROM HC_SITE_WORKER A                                            ");
            parameter.AppendSql("INNER JOIN HC_CODE B                                                        ");
            parameter.AppendSql("ON A.WORKER_ROLE = B.CODE                                                   ");
            parameter.AppendSql("AND B.CODE <> 'EMP_ROLE' AND B.ISDELETED ='N'                               ");
            parameter.AppendSql("WHERE A.SITEID = :SITEID                                                                ");
            parameter.AppendSql("AND A.ISDELETED = 'N'                                                               ");

            parameter.Add("SITEID", siteId);

            return ExecuteReader<HC_SITE_WORKER>(parameter);

        }

        public void Insert(HC_SITE_WORKER dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HC_SITE_WORKER                                                  ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SITEID,                                                                   ");
            parameter.AppendSql("  NAME,                                                                     ");
            parameter.AppendSql("  WORKER_ROLE,                                                              ");
            parameter.AppendSql("  DEPT,                                                                     ");
            parameter.AppendSql("  TEL,                                                                      ");
            parameter.AppendSql("  HP,                                                                       ");
            parameter.AppendSql("  EMAIL,                                                                    ");
            parameter.AppendSql("  ISDELETED,                                                                ");
            parameter.AppendSql("  ISRETIRE,                                                                ");
            parameter.AppendSql("  MODIFIED,                                                                 ");
            parameter.AppendSql("  MODIFIEDUSER,                                                             ");
            parameter.AppendSql("  CREATED,                                                                  ");
            parameter.AppendSql("  CREATEDUSER                                                               ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  HC_SITE_WORKER_ID_SEQ.NEXTVAL,                                                                      ");
            parameter.AppendSql("  :SITEID,                                                                  ");
            parameter.AppendSql("  :NAME,                                                                    ");
            parameter.AppendSql("  :WORKER_ROLE,                                                             ");
            parameter.AppendSql("  :DEPT,                                                                    ");
            parameter.AppendSql("  :TEL,                                                                     ");
            parameter.AppendSql("  :HP,                                                                      ");
            parameter.AppendSql("  :EMAIL,                                                                   ");
            parameter.AppendSql("  'N',                                                               ");
            parameter.AppendSql("  :ISRETIRE,                                                                ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                            ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :CREATEDUSER                                                              ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("SITEID", dto.SITEID);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("WORKER_ROLE", dto.WORKER_ROLE);
            parameter.Add("DEPT", dto.DEPT);
            parameter.Add("TEL", dto.TEL);
            parameter.Add("HP", dto.HP);
            parameter.Add("EMAIL", dto.EMAIL);            
            parameter.Add("ISRETIRE", dto.ISRETIRE);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            ExecuteNonQuery(parameter);

        }

        public void Update(HC_SITE_WORKER dto)
        {

            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_SITE_WORKER                                                       ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  NAME = :NAME,                                                             ");
            parameter.AppendSql("  WORKER_ROLE = :WORKER_ROLE,                                               ");
            parameter.AppendSql("  DEPT = :DEPT,                                                             ");
            parameter.AppendSql("  TEL = :TEL,                                                               ");
            parameter.AppendSql("  HP = :HP,                                                                 ");
            parameter.AppendSql("  EMAIL = :EMAIL,                                                           ");
            parameter.AppendSql("  ISRETIRE = :ISRETIRE,                                                      ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                  ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                              ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("WORKER_ROLE", dto.WORKER_ROLE);
            parameter.Add("DEPT", dto.DEPT);
            parameter.Add("TEL", dto.TEL);
            parameter.Add("HP", dto.HP);
            parameter.Add("EMAIL", dto.EMAIL);
            parameter.Add("ISRETIRE", dto.ISRETIRE);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE  HC_SITE_WORKER                                                 ");
            parameter.AppendSql("SET ISDELETED = 'Y',                                                   ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                             ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                         ");
            parameter.AppendSql("WHERE ID = :ID                                                         ");
            parameter.Add("ID", id);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

        }
    }
}
