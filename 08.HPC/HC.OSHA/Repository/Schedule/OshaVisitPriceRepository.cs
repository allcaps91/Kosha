using ComBase;
using ComBase.Mvc;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC_Core.Service;
using HC_OSHA.Model.Visit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Repository.Schedule
{
    public class OshaVisitPriceRepository : BaseRepository
    {
        public List<VisitPriceModel> FindByYear(long estimateId, string startYear, string endYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.VISITDATETIME, B.* "); 
            parameter.AppendSql("  FROM HIC_OSHA_VISIT A ");
            parameter.AppendSql("  INNER JOIN HIC_OSHA_VISIT_PRICE B ");
            parameter.AppendSql("          ON A.ID = B.VISIT_ID ");
            parameter.AppendSql("         AND B.ISDELETED = 'N' ");
            parameter.AppendSql("         AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" WHERE 1 = 1 ");
            parameter.AppendSql("   AND A.SITE_ID = :SITE_ID ");
            parameter.AppendSql("   AND A.VisitDateTime >= :START_YEAR ");
            parameter.AppendSql("   AND A.VisitDateTime <= :END_YEAR ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" ORDER BY A.VISITDATETIME ");

            parameter.Add("SITE_ID", estimateId);
            parameter.Add("START_YEAR", startYear);
            parameter.Add("END_YEAR", endYear);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<VisitPriceModel>(parameter);
        }

        public OSHA_VISIT_PRICE FindMaxId(long visitId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("    SELECT * FROM HIC_OSHA_VISIT_PRICE ");
            parameter.AppendSql("    WHERE ID = (SELECT MAX(ID) FROM HIC_OSHA_VISIT_PRICE   ");
            parameter.AppendSql("          WHERE VISIT_ID = :VISIT_ID  ");
            parameter.AppendSql("            AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("            AND ISDELETED = 'N' ) ");
            parameter.AppendSql("      AND SWLICENSE = :SWLICENSE ");
            parameter.Add("VISIT_ID", visitId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReaderSingle<OSHA_VISIT_PRICE>(parameter);
        }
        public List<OSHA_VISIT_PRICE> FindVisitPrice(long siteId, string startDate, string endDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.* FROM HIC_OSHA_VISIT A ");
            parameter.AppendSql("       INNER JOIN HIC_OSHA_VISIT_PRICE B ");
            parameter.AppendSql("             ON  A.ID = B.VISIT_ID ");
            parameter.AppendSql("             AND B.ISDELETED = 'N' ");
            parameter.AppendSql("             AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" WHERE A.SITE_ID = :SITE_ID ");
            parameter.AppendSql("   AND A.ISDELETED = 'N' ");
            parameter.AppendSql("   AND A.VISITDATETIME >= :startDate ");
            parameter.AppendSql("   AND A.VISITDATETIME <= :endDate ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<OSHA_VISIT_PRICE>(parameter);
        }

        public OSHA_VISIT_PRICE FindOne(long visitId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT_PRICE ");
            parameter.AppendSql(" WHERE ISDELETED = 'N' ");
            parameter.AppendSql("   AND VISIT_ID = :VISIT_ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("VISIT_ID", visitId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<OSHA_VISIT_PRICE>(parameter);
        }

        public List<OshaVisitPriceModel> FindAllByEstimate(long estimateId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID, A.VISITDATETIME, A.VISITUSERNAME, B.*, TO_CHAR(B.CREATED,'YYYY-MM-DD') as CREATED ");
            parameter.AppendSql("  FROM HIC_OSHA_VISIT A ");
            parameter.AppendSql("       INNER JOIN  HIC_OSHA_VISIT_PRICE B ");
            parameter.AppendSql("             ON  A.ID = B.VISIT_ID ");
            parameter.AppendSql("             AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" WHERE A.ESTIMATE_ID = :estimateId ");
            parameter.AppendSql("   AND A.ISDELETED = 'N' ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("ORDER BY B.CREATED DESC, B.ID DESC ");

            parameter.Add("estimateId", estimateId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<OshaVisitPriceModel>(parameter);
        }

        public void Insert(OSHA_VISIT_PRICE dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_VISIT_PRICE_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_VISIT_PRICE ");
            parameter.AppendSql("( ");
            parameter.AppendSql("    ID ");
            parameter.AppendSql("  , VISIT_ID ");
            parameter.AppendSql("  , WORKERCOUNT ");
            parameter.AppendSql("  , UNITPRICE ");
            parameter.AppendSql("  , UNITTOTALPRICE ");
            parameter.AppendSql("  , TOTALPRICE ");
            parameter.AppendSql("  , CHARGEPRICE ");
            parameter.AppendSql("  , ISPRECHARGE ");
            parameter.AppendSql("  , MODIFIED ");
            parameter.AppendSql("  , MODIFIEDUSER ");
            parameter.AppendSql("  , CREATED ");
            parameter.AppendSql("  , CREATEDUSER ");
            parameter.AppendSql("  , ISDELETED ");
            parameter.AppendSql("  , SWLICENSE ");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :ID ");
            parameter.AppendSql("  ,  :VISIT_ID ");
            parameter.AppendSql("  , :WORKERCOUNT ");
            parameter.AppendSql("  , :UNITPRICE ");
            parameter.AppendSql("  , :UNITTOTALPRICE ");
            parameter.AppendSql("  , :TOTALPRICE ");
            parameter.AppendSql("  , :CHARGEPRICE ");
            parameter.AppendSql("  , :ISPRECHARGE ");
            parameter.AppendSql("  , SYSDATE ");
            parameter.AppendSql("  , :MODIFIEDUSER ");
            parameter.AppendSql("  , SYSDATE ");
            parameter.AppendSql("  , :CREATEDUSER ");
            parameter.AppendSql("  , 'N' ");
            parameter.AppendSql("  , :SWLICENSE ");
            parameter.AppendSql(") ");
            parameter.Add("ID", dto.ID);
            parameter.Add("VISIT_ID", dto.VISIT_ID);
            parameter.Add("WORKERCOUNT", dto.WORKERCOUNT);
            parameter.Add("UNITPRICE", dto.UNITPRICE);
            parameter.Add("UNITTOTALPRICE", dto.UNITTOTALPRICE);
            parameter.Add("TOTALPRICE", dto.TOTALPRICE);
            parameter.Add("CHARGEPRICE", dto.CHARGEPRICE);
            parameter.Add("ISPRECHARGE", dto.ISPRECHARGE);
            parameter.Add("CREATEDUSER",CommonService.Instance.Session.UserId);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Insert("HIC_OSHA_VISIT_PRICE", dto.ID);
        }

        public void Delete(long visitId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_VISIT_PRICE ");
            parameter.AppendSql("    SET ");
            parameter.AppendSql("       ISDELETED = 'Y' , ");
            parameter.AppendSql("       MODIFIED = SYSDATE, ");
            parameter.AppendSql("       MODIFIEDUSER = :MODIFIEDUSER ");
            parameter.AppendSql("WHERE VISIT_ID = :VISIT_ID ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("VISIT_ID", visitId);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_VISIT_PRICE", visitId);
        }
    }
}
