using ComBase.Controls;
using ComBase;
using ComBase.Mvc;
using ComBase.Mvc.Utils;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Model;
using HC_Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Repository.StatusReport
{
    public class HcOshaRelationRepository : BaseRepository
    {

        public HC_OSHA_RELATION FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT  *  ");
            parameter.AppendSql("  FROM HIC_OSHA_RELATION");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            HC_OSHA_RELATION dto = ExecuteReaderSingle<HC_OSHA_RELATION>(parameter);
            return dto;
        }

        public List<HC_OSHA_RELATION_MODEL> FindAll(long parentId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT A.*, C.NAME AS PARENT_NAME, B.NAME as CHILD_NAME FROM HIC_OSHA_RELATION A ");
            parameter.AppendSql(" INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql(" ON A.CHILD_ID = B.ID  ");
            parameter.AppendSql("   INNER JOIN HC_SITE_VIEW C   ");
            parameter.AppendSql("   ON A.PARENT_ID = C.ID   ");
            parameter.AppendSql(" WHERE A.PARENT_ID = :PARENT_ID   ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE ");

            parameter.Add("PARENT_ID", parentId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_RELATION_MODEL>(parameter);
        }
        public List<HC_OSHA_RELATION_MODEL> FindAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT A.PARENT_ID, C.NAME AS PARENT_NAME, A.CHILD_ID, B.NAME AS CHILD_NAME from HIC_OSHA_RELATION A   ");
            parameter.AppendSql("   INNER JOIN HC_SITE_VIEW B   ");
            parameter.AppendSql("   ON A.CHILD_ID = B.ID   ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   INNER JOIN HC_SITE_VIEW C   ");
            parameter.AppendSql("   ON A.PARENT_ID = C.ID   ");
            parameter.AppendSql("   AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   WHERE A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   ORDER BY C.NAME, B.NAME");

            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_RELATION_MODEL>(parameter);
        }
        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_RELATION   ");
            parameter.AppendSql("WHERE ID = :ID             ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_RELATION", id);
        }
        public HC_OSHA_RELATION Insert(HC_OSHA_RELATION item)
        {
            item.ID = GetSequenceNextVal("HIC_OSHA_RELATION_SEQ ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_RELATION");
            parameter.AppendSql("(");
            parameter.AppendSql("    ID");
            parameter.AppendSql("  , PARENT_ID");
            parameter.AppendSql("  , CHILD_ID");
            parameter.AppendSql("  , SWLICENSE");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :ID");
            parameter.AppendSql("  , :PARENT_ID");
            parameter.AppendSql("  , :CHILD_ID");
            parameter.AppendSql("  , :SWLICENSE ");
            parameter.AppendSql(") ");

            parameter.Add("ID", item.ID);
            parameter.Add("PARENT_ID", item.PARENT_ID);
            parameter.Add("CHILD_ID", item.CHILD_ID);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_RELATION", item.ID);
            

            return FindOne(item.ID);
        }
    }
}
