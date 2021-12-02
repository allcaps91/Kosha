using System.Collections.Generic;
using ComBase.Mvc;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC_Core.Service;

namespace HC.OSHA.Repository
{
    public class HcOshaCard22Repository : BaseRepository
    {

        public HC_OSHA_CARD22 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_CARD22                                               ");
            parameter.AppendSql("WHERE ID = :ID                                                             ");
            parameter.Add("ID", id);

            return ExecuteReaderSingle<HC_OSHA_CARD22>(parameter);

        }
        public HC_OSHA_CARD22 FindByEstimateId(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_CARD22                                                  ");
            parameter.AppendSql("WHERE ESTIMATE_ID = :ESTIMATEID                                                                           ");
            parameter.Add("ESTIMATEID", id);

            return ExecuteReaderSingle<HC_OSHA_CARD22>(parameter);
        }
        public HC_OSHA_CARD22 Insert(HC_OSHA_CARD22 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD22                                                  ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                                  ");
            parameter.AppendSql("  ESTIMATE_ID,                                                             ");
            parameter.AppendSql("  YEAR,                                                               ");
            parameter.AppendSql("  IMAGEDATA                                                               ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :ID,                                                                      ");
            parameter.AppendSql("  :SITE_ID,                                                                 ");
            parameter.AppendSql("  :ESTIMATE_ID,                                                             ");
            parameter.AppendSql("  :YEAR,                                                             ");
            parameter.AppendSql("  :IMAGEDATA                                                              ");
            parameter.AppendSql(")                                                  ");
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("ID", dto.ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("IMAGEDATA", dto.ImageData, Oracle.ManagedDataAccess.Client.OracleDbType.LongRaw);
            
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_CARD22", dto.ID);
            return FindOne(dto.ID);
        }

        public void Update(HC_OSHA_CARD22 dto)
        {

            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD22                                                  ");
            parameter.AppendSql("  SET IMAGEDATA = :IMAGEDATA                                                            ");
            parameter.AppendSql("  , YEAR = :YEAR                                                            ");
            parameter.AppendSql("WHERE ID = :ID                                                                           ");
            
            parameter.Add("ID", dto.ID);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("IMAGEDATA", dto.ImageData, Oracle.ManagedDataAccess.Client.OracleDbType.LongRaw);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD22", dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD22                                                   ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", id);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_CARD22", id);
        }
    }
}
