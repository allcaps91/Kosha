namespace HC.OSHA.Site.ETC.Repository
{
    using ComBase.Mvc;
    using HC.OSHA.Site.ETC.Dto;
    using HC.Core.Common.Service;
    using System.Collections.Generic;


    /// <summary>
    /// 
    /// </summary>
    public class HcSiteProductRepository : BaseRepository
    {
        
        public List<HC_SITE_PRODUCT> FindAll(long estimateId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_SITE_PRODUCT                                                  ");
            parameter.AppendSql("WHERE ESTIMATE_ID = :ESTIMATE_ID                                               ");
            parameter.AppendSql("ORDER BY ID                                                                    ");

            parameter.Add("ESTIMATE_ID", estimateId);

            return ExecuteReader<HC_SITE_PRODUCT>(parameter);
        }
        public void Insert(HC_SITE_PRODUCT dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HC_SITE_PRODUCT                                                 ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  PRODUCTNAME,                                                              ");
            parameter.AppendSql("  PROCESS,                                                                  ");
            parameter.AppendSql("  USAGE,                                                                    ");
            parameter.AppendSql("  MANUFACTURER,                                                             ");
            parameter.AppendSql("  MONTHLYAMOUNT,                                                            ");
            parameter.AppendSql("  REVISIONDATE,                                                             ");
            parameter.AppendSql("  GHSPICTURE,                                                                ");
            parameter.AppendSql("  MODIFIED,                                                                  ");
            parameter.AppendSql("  MODIFIEDUSER,                                                              ");
            parameter.AppendSql("  CREATED,                                                                   ");
            parameter.AppendSql("  CREATEDUSER                                                                ");
            parameter.AppendSql(")                                                                            ");
            parameter.AppendSql("VALUES                                                                       ");
            parameter.AppendSql("(                                                                            ");
            parameter.AppendSql("  HC_SITE_PRODUCT_ID_SEQ.NEXTVAL,                                            ");
            parameter.AppendSql("  :ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  :PRODUCTNAME,                                                              ");
            parameter.AppendSql("  :PROCESS,                                                                  ");
            parameter.AppendSql("  :USAGE,                                                                    ");
            parameter.AppendSql("  :MANUFACTURER,                                                             ");
            parameter.AppendSql("  :MONTHLYAMOUNT,                                                            ");
            parameter.AppendSql("  :REVISIONDATE,                                                             ");
            parameter.AppendSql("  :GHSPICTURE,                                                               ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                            ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :CREATEDUSER                                                              ");
            parameter.AppendSql(")                                                                           ");
        //    parameter.Add("ID", dto.ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("PRODUCTNAME", dto.PRODUCTNAME);
            parameter.Add("PROCESS", dto.PROCESS);
            parameter.Add("USAGE", dto.USAGE);
            parameter.Add("MANUFACTURER", dto.MANUFACTURER);
            parameter.Add("MONTHLYAMOUNT", dto.MONTHLYAMOUNT);
            parameter.Add("REVISIONDATE", dto.REVISIONDATE);
            parameter.Add("GHSPICTURE", dto.GHSPICTURE);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            ExecuteNonQuery(parameter);

        }

        public void Update(HC_SITE_PRODUCT dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_SITE_PRODUCT                                                      ");
            parameter.AppendSql("SET                                                                         ");         
            parameter.AppendSql("  ESTIMATE_ID = :ESTIMATE_ID,                                               ");
            parameter.AppendSql("  PRODUCTNAME = :PRODUCTNAME,                                               ");
            parameter.AppendSql("  PROCESS = :PROCESS,                                                       ");
            parameter.AppendSql("  USAGE = :USAGE,                                                           ");
            parameter.AppendSql("  MANUFACTURER = :MANUFACTURER,                                             ");
            parameter.AppendSql("  MONTHLYAMOUNT = :MONTHLYAMOUNT,                                           ");
            parameter.AppendSql("  REVISIONDATE = :REVISIONDATE,                                             ");
            parameter.AppendSql("  GHSPICTURE = :GHSPICTURE,                                                   ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                   ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                               ");
            parameter.AppendSql("WHERE ID = :ID                                                               ");
            parameter.Add("ID", dto.ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("PRODUCTNAME", dto.PRODUCTNAME);
            parameter.Add("PROCESS", dto.PROCESS);
            parameter.Add("USAGE", dto.USAGE);
            parameter.Add("MANUFACTURER", dto.MANUFACTURER);
            parameter.Add("MONTHLYAMOUNT", dto.MONTHLYAMOUNT);
            parameter.Add("REVISIONDATE", dto.REVISIONDATE);
            parameter.Add("GHSPICTURE", dto.GHSPICTURE);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HC_SITE_PRODUCT                                                 ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", id);

            ExecuteNonQuery(parameter);

        }
    }
}
