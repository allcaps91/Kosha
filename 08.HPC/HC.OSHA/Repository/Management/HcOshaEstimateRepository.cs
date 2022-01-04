namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using HC.Core.Service;
    using HC.OSHA.Dto;

    /// <summary>
    /// 보건관리전문 견적 쿼리
    /// </summary>
    public class HcOshaEstimateRepository : BaseRepository
    {
        public HC_OSHA_ESTIMATE FindById(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_ESTIMATE    ");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            HC_OSHA_ESTIMATE dto = ExecuteReaderSingle<HC_OSHA_ESTIMATE>(parameter);
            return dto;
        }

        public HC_OSHA_ESTIMATE Insert(HC_OSHA_ESTIMATE dto)
        {
            long id = GetSequenceNextVal("HC_OSHA_ESTIMATE_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_ESTIMATE ");
            parameter.AppendSql("(                              ");
            parameter.AppendSql(" ID, OSHA_SITE_ID, ESTIMATEDATE, STARTDATE, WORKERTOTALCOUNT, OFFICIALFEE, SITEFEE,    ");
            parameter.AppendSql(" MONTHLYFEE, FEETYPE, PRINTDATE, SENDMAILDATE, ISDELETED, REMARK,");
            parameter.AppendSql(" EXCELPATH, MODIFIED, MODIFIEDUSER, CREATED, CREATEDUSER,");
            parameter.AppendSql(" BLUEMALE, BLUEFEMALE, WHITEMALE, WHITEFEMALE, SWLICENSE ");
            parameter.AppendSql(") ");
            parameter.AppendSql("VALUES ");
            parameter.AppendSql("(");
            parameter.AppendSql(" :ID, :OSHA_SITE_ID, :ESTIMATEDATE, :STARTDATE, :WORKERTOTALCOUNT, :OFFICIALFEE, :SITEFEE, ");
            parameter.AppendSql(" :MONTHLYFEE, :FEETYPE, :PRINTDATE, :SENDMAINDATE, 'N', :REMARK, :EXCELPATH, SYSTIMESTAMP, :MODIFIEDUSER, SYSTIMESTAMP, :CREATEDUSER,  ");
            parameter.AppendSql(" :BLUEMALE, :BLUEFEMALE, :WHITEMALE, :WHITEFEMALE, :SWLICENSE ");
            parameter.AppendSql(") ");

            parameter.Add("ID", id);
            parameter.Add("OSHA_SITE_ID", dto.OSHA_SITE_ID);
            parameter.Add("ESTIMATEDATE", dto.ESTIMATEDATE);
            parameter.Add("STARTDATE", dto.STARTDATE);
            parameter.Add("WORKERTOTALCOUNT", dto.WORKERTOTALCOUNT);
            parameter.Add("OFFICIALFEE", dto.OFFICIALFEE);
            parameter.Add("SITEFEE", dto.SITEFEE);
            parameter.Add("MONTHLYFEE", dto.MONTHLYFEE);
            parameter.Add("FEETYPE", dto.FEETYPE);
            parameter.Add("PRINTDATE", dto.PRINTDATE);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("SENDMAINDATE", dto.SENDMAILDATE);
            parameter.Add("EXCELPATH", dto.EXCELPATH);
            parameter.Add("BLUEMALE", dto.BLUEMALE);
            parameter.Add("BLUEFEMALE", dto.BLUEFEMALE);
            parameter.Add("WHITEMALE", dto.WHITEMALE);
            parameter.Add("WHITEFEMALE", dto.WHITEFEMALE);

            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            return FindById(id);
        }
        public HC_OSHA_ESTIMATE Update(HC_OSHA_ESTIMATE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_ESTIMATE    ");
            parameter.AppendSql("SET  ");
            parameter.AppendSql("ESTIMATEDATE = :ESTIMATEDATE, STARTDATE = :STARTDATE, WORKERTOTALCOUNT = :WORKERTOTALCOUNT,");
            parameter.AppendSql("OFFICIALFEE = :OFFICIALFEE, SITEFEE = :SITEFEE, MONTHLYFEE = :MONTHLYFEE,                  ");
            parameter.AppendSql("FEETYPE = :FEETYPE , PRINTDATE = :PRINTDATE, SENDMAILDATE = :SENDMAILDATE, REMARK=:REMARK, EXCELPATH = :EXCELPATH, MODIFIED = SYSTIMESTAMP, MODIFIEDUSER = :MODIFIEDUSER ");
            parameter.AppendSql(", BLUEMALE =:BLUEMALE   ");
            parameter.AppendSql(", BLUEFEMALE =:BLUEFEMALE   ");
            parameter.AppendSql(", WHITEMALE= :WHITEMALE   ");
            parameter.AppendSql(", WHITEFEMALE =:WHITEFEMALE   ");
            parameter.AppendSql("WHERE ID = :ID    ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ESTIMATEDATE", dto.ESTIMATEDATE);
            parameter.Add("STARTDATE", dto.STARTDATE);
            parameter.Add("WORKERTOTALCOUNT", dto.WORKERTOTALCOUNT);
            parameter.Add("OFFICIALFEE", dto.OFFICIALFEE);
            parameter.Add("SITEFEE", dto.SITEFEE);
            parameter.Add("MONTHLYFEE", dto.MONTHLYFEE);
            parameter.Add("FEETYPE", dto.FEETYPE);
            parameter.Add("PRINTDATE", dto.PRINTDATE);
            parameter.Add("SENDMAILDATE", dto.SENDMAILDATE);
            parameter.Add("REMARK", dto.REMARK);
            
            parameter.Add("EXCELPATH", dto.EXCELPATH);

            parameter.Add("BLUEMALE", dto.BLUEMALE);
            parameter.Add("BLUEFEMALE", dto.BLUEFEMALE);
            parameter.Add("WHITEMALE", dto.WHITEMALE);
            parameter.Add("WHITEFEMALE", dto.WHITEFEMALE);

            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("ID", dto.ID);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            return FindById(dto.ID);
        }
        public HC_OSHA_ESTIMATE UpdatePRINTDATE(HC_OSHA_ESTIMATE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_ESTIMATE    ");
            parameter.AppendSql("SET  ");
            parameter.AppendSql("PRINTDATE = SYSTIMESTAMP");
            parameter.AppendSql("WHERE ID = :ID    ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", dto.ID);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            return FindById(dto.ID);
        }

        public HC_OSHA_ESTIMATE UpdateSendMail(HC_OSHA_ESTIMATE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_ESTIMATE    ");
            parameter.AppendSql("SET  ");
            parameter.AppendSql("SENDMAILDATE = SYSTIMESTAMP");
            parameter.AppendSql("WHERE ID = :ID    ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", dto.ID);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            return FindById(dto.ID);
        }
        public void Delete(HC_OSHA_ESTIMATE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE  HIC_OSHA_ESTIMATE    ");
            parameter.AppendSql("SET ISDELETED = 'Y'     ");
            parameter.AppendSql("WHERE ID = :ID     ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", dto.ID);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
        }

        /// <summary>
        /// 사용중인 견적 아이디 가져오기
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public long GetEstimateId(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("  select max(a.id) from hic_osha_estimate a ");
            parameter.AppendSql("inner join HIC_OSHA_CONTRACT B ");
            parameter.AppendSql("ON A.id = b.estimate_id ");
            parameter.AppendSql("where A.isdeleted = 'N' AND B.ISCONTRACT = 'Y' ");
            parameter.AppendSql("  and A.osha_site_id = :siteId ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE ");
            parameter.Add("siteId", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteScalar<long>(parameter);
        }
    }
}
