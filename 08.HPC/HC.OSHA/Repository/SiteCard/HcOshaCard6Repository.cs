namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using HC.OSHA.Dto;
    using HC_Core.Service;

    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard6Repository : BaseRepository
    {
        public HC_OSHA_CARD6 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_CARD6         ");
            parameter.AppendSql("WHERE ID = :ID                       ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_CARD6>(parameter);

        }
        public List<HC_OSHA_CARD6> FindAllByYear(long estimateId, string startYear, string endYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_CARD6        ");
            parameter.AppendSql("WHERE ESTIMATE_ID = :ESTIMATE_ID    ");
            parameter.AppendSql("  AND ACC_DATE >= :startYear        ");
            parameter.AppendSql("  AND ACC_DATE <= :endYear          ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ESTIMATE_ID", estimateId);
            parameter.Add("startYear", startYear);
            parameter.Add("endYear", endYear);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD6>(parameter);

        }
        public List<HC_OSHA_CARD6> FindAll(long estimateId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_CARD6            ");
            parameter.AppendSql("WHERE ESTIMATE_ID = :ESTIMATE_ID        ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE            ");
            parameter.AppendSql("ORDER BY ID DESC                        ");
            parameter.Add("ESTIMATE_ID", estimateId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD6>(parameter);

        }
        public HC_OSHA_CARD6 Insert(HC_OSHA_CARD6 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD6             ");
            parameter.AppendSql("(                                      ");
            parameter.AppendSql("  ID,                                  ");
            parameter.AppendSql("  ESTIMATE_ID,                         ");
            parameter.AppendSql("  IND_ACC_TYPE,                        ");
            parameter.AppendSql("  NAME,                                ");
            parameter.AppendSql("  JUMIN_NO,                            ");
            parameter.AppendSql("  ACC_DATE,                            ");
            parameter.AppendSql("  APPROVE_DATE,                        ");
            parameter.AppendSql("  REQUEST_DATE,                        ");
            parameter.AppendSql("  ILLNAME,                             ");
            parameter.AppendSql("  REMARK,                              ");
            parameter.AppendSql("  SWLICENSE                            ");
            parameter.AppendSql(")                                      ");
            parameter.AppendSql("VALUES                                 ");
            parameter.AppendSql("(                                      ");
            parameter.AppendSql("  :ID,                                 ");
            parameter.AppendSql("  :ESTIMATE_ID,                        ");
            parameter.AppendSql("  :IND_ACC_TYPE,                       ");
            parameter.AppendSql("  :NAME,                               ");
            parameter.AppendSql("  :JUMIN_NO,                           ");
            parameter.AppendSql("  :ACC_DATE,                           ");
            parameter.AppendSql("  :APPROVE_DATE,                       ");
            parameter.AppendSql("  :REQUEST_DATE,                       ");
            parameter.AppendSql("  :ILLNAME,                            ");
            parameter.AppendSql("  :REMARK                              ");
            parameter.AppendSql("  :SWLICENSE                           ");
            parameter.AppendSql(")                                      ");
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
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_CARD6", dto.ID);
            return FindOne(dto.ID);
        }

        public void Update(HC_OSHA_CARD6 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD6                           ");
            parameter.AppendSql("SET                                             ");
            parameter.AppendSql("  IND_ACC_TYPE = :IND_ACC_TYPE,                 ");
            parameter.AppendSql("  NAME = :NAME,                                 ");
            parameter.AppendSql("  JUMIN_NO = :JUMIN_NO,                         ");
            parameter.AppendSql("  ACC_DATE = :ACC_DATE,                         ");
            parameter.AppendSql("  APPROVE_DATE = :APPROVE_DATE,                 ");
            parameter.AppendSql("  REQUEST_DATE = :REQUEST_DATE,                 ");
            parameter.AppendSql("  ILLNAME = :ILLNAME,                           ");
            parameter.AppendSql("  REMARK = :REMARK                              ");
            parameter.AppendSql("WHERE ID = :ID                                  ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("IND_ACC_TYPE", dto.IND_ACC_TYPE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("JUMIN_NO", dto.JUMIN_NO);
            parameter.Add("ACC_DATE", dto.ACC_DATE);
            parameter.Add("APPROVE_DATE", dto.APPROVE_DATE);
            parameter.Add("REQUEST_DATE", dto.REQUEST_DATE);
            parameter.Add("ILLNAME", dto.ILLNAME);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD6", dto.ID);

        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD6      ");
            parameter.AppendSql("WHERE ID = :ID                  ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE    ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_CARD6", id);
        }
    }
}
