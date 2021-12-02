namespace HC.OSHA.Site.Card.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.OSHA.Site.Card.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard6Repository : BaseRepository
    {


        public List<HC_OSHA_CARD6> FindAll(long estimateId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_OSHA_CARD6                                               ");
            parameter.AppendSql("WHERE ESTIMATE_ID = :ESTIMATE_ID   ORDER BY ID DESC                                ");

            parameter.Add("ESTIMATE_ID", estimateId);

            return ExecuteReader<HC_OSHA_CARD6>(parameter);

        }
        public void Insert(HC_OSHA_CARD6 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HC_OSHA_CARD6                                                   ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  IND_ACC_TYPE,                                                             ");
            parameter.AppendSql("  NAME,                                                                     ");
            parameter.AppendSql("  JUMIN_NO,                                                                 ");
            parameter.AppendSql("  ACC_DATE,                                                                 ");
            parameter.AppendSql("  APPROVE_DATE,                                                             ");
            parameter.AppendSql("  REQUEST_DATE,                                                             ");
            parameter.AppendSql("  ILLNAME,                                                                  ");
            parameter.AppendSql("  REMARK                                                                   ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  HC_OSHA_CARD_ID_SEQ.NEXTVAL,                                                                      ");
            parameter.AppendSql("  :ESTIMATE_ID,                                                             ");
            parameter.AppendSql("  :IND_ACC_TYPE,                                                            ");
            parameter.AppendSql("  :NAME,                                                                    ");
            parameter.AppendSql("  :JUMIN_NO,                                                                ");
            parameter.AppendSql("  :ACC_DATE,                                                                ");
            parameter.AppendSql("  :APPROVE_DATE,                                                            ");
            parameter.AppendSql("  :REQUEST_DATE,                                                            ");
            parameter.AppendSql("  :ILLNAME,                                                                 ");
            parameter.AppendSql("  :REMARK                                                                  ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("ID", dto.ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("IND_ACC_TYPE", dto.IND_ACC_TYPE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("JUMIN_NO", dto.JUMIN_NO);
            parameter.Add("ACC_DATE", dto.ACC_DATE);
            parameter.Add("APPROVE_DATE", dto.APPROVE_DATE);
            parameter.Add("REQUEST_DATE", dto.REQUEST_DATE);
            parameter.Add("ILLNAME", dto.ILLNAME);
            parameter.Add("REMARK", dto.REMARK);

            ExecuteNonQuery(parameter);
        }

        public void Update(HC_OSHA_CARD6 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_OSHA_CARD6                                                        ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  IND_ACC_TYPE = :IND_ACC_TYPE,                                             ");
            parameter.AppendSql("  NAME = :NAME,                                                             ");
            parameter.AppendSql("  JUMIN_NO = :JUMIN_NO,                                                     ");
            parameter.AppendSql("  ACC_DATE = :ACC_DATE,                                                     ");
            parameter.AppendSql("  APPROVE_DATE = :APPROVE_DATE,                                             ");
            parameter.AppendSql("  REQUEST_DATE = :REQUEST_DATE,                                             ");
            parameter.AppendSql("  ILLNAME = :ILLNAME,                                                       ");
            parameter.AppendSql("  REMARK = :REMARK                                                          ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            parameter.Add("IND_ACC_TYPE", dto.IND_ACC_TYPE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("JUMIN_NO", dto.JUMIN_NO);
            parameter.Add("ACC_DATE", dto.ACC_DATE);
            parameter.Add("APPROVE_DATE", dto.APPROVE_DATE);
            parameter.Add("REQUEST_DATE", dto.REQUEST_DATE);
            parameter.Add("ILLNAME", dto.ILLNAME);
            parameter.Add("REMARK", dto.REMARK);

            ExecuteNonQuery(parameter);

        }

        public void Delete(HC_OSHA_CARD6 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HC_OSHA_CARD6                                               ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            ExecuteNonQuery(parameter);

        }
    }
}
