namespace HC.OSHA.Visit.Management.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.OSHA.Visit.Management.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcOshaVisitInformationRepository : BaseRepository
    {


        public List<HC_OSHA_VISIT_INFORMATION> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_OSHA_VISIT_INFORMATION                                               ");
            parameter.AppendSql("WHERE SITE_ID = :siteId   ORDER BY REGDATE DESC                                           ");

            parameter.Add("siteId", siteId);

            return ExecuteReader<HC_OSHA_VISIT_INFORMATION>(parameter);

        }
        public void Insert(HC_OSHA_VISIT_INFORMATION dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HC_OSHA_VISIT_INFORMATION                                       ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                                  ");
            parameter.AppendSql("  REGDATE,                                                                  ");
            parameter.AppendSql("  REGUSERID,                                                                ");
            parameter.AppendSql("  INFORMATIONTYPE,                                                          ");
            parameter.AppendSql("  REMARK                                                                   ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  HC_OSHA_SHARED_ID_SEQ.NEXTVAL,                                                                      ");
            parameter.AppendSql("  :SITE_ID,                                                                 ");
            parameter.AppendSql("  :REGDATE,                                                                 ");
            parameter.AppendSql("  :REGUSERID,                                                               ");
            parameter.AppendSql("  :INFORMATIONTYPE,                                                         ");
            parameter.AppendSql("  :REMARK                                                                  ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("REGDATE", dto.REGDATE);
            parameter.Add("REGUSERID", dto.REGUSERID);
            parameter.Add("INFORMATIONTYPE", dto.INFORMATIONTYPE);
            parameter.Add("REMARK", dto.REMARK);
            ExecuteNonQuery(parameter);


        }

        public void Update(HC_OSHA_VISIT_INFORMATION dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_OSHA_VISIT_INFORMATION                                            ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  SITE_ID = :SITE_ID,                                                       ");
            parameter.AppendSql("  REGDATE = :REGDATE,                                                       ");
            parameter.AppendSql("  REGUSERID = :REGUSERID,                                                   ");
            parameter.AppendSql("  INFORMATIONTYPE = :INFORMATIONTYPE,                                       ");
            parameter.AppendSql("  REMARK = :REMARK                                                         ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("REGDATE", dto.REGDATE);
            parameter.Add("REGUSERID", dto.REGUSERID);
            parameter.Add("INFORMATIONTYPE", dto.INFORMATIONTYPE);
            parameter.Add("REMARK", dto.REMARK);
            ExecuteNonQuery(parameter);


        }

        public void Delete(HC_OSHA_VISIT_INFORMATION dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HC_OSHA_VISIT_INFORMATION                                               ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            ExecuteNonQuery(parameter);

        }
    }
}
