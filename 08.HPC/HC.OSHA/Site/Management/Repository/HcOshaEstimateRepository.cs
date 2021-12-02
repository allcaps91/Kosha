namespace HC.OSHA.Site.Management.Repository
{
    using ComBase.Mvc;
    using HC.OSHA.Site.Management.Dto;
    using HC.Core.Common.Service;


    /// <summary>
    /// 보건관리전문 견적 쿼리
    /// </summary>
    public class HcOshaEstimateRepository : BaseRepository
    {
        public HC_OSHA_ESTIMATE FindById(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_OSHA_ESTIMATE    ");
            parameter.AppendSql("WHERE ID = :ID");
            parameter.Add("ID", id);

            HC_OSHA_ESTIMATE dto = ExecuteReaderSingle<HC_OSHA_ESTIMATE>(parameter);
            return dto;

        }
        public HC_OSHA_ESTIMATE Insert(HC_OSHA_ESTIMATE dto)
        {

            long id = GetSequenceNextVal("HC_OSHA_ESTIMATE_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HC_OSHA_ESTIMATE    ");
            parameter.AppendSql("(                              ");
            parameter.AppendSql(" ID, OSHA_SITE_ID, ESTIMATEDATE, STARTDATE, WORKERTOTALCOUNT, OFFICIALFEE, SITEFEE,    ");
            parameter.AppendSql(" MONTHLYFEE, FEETYPE, PRINTDATE, SENDMAILDATE, ISDELETED, MODIFIED, MODIFIEDUSER, CREATED, CREATEDUSER  ");
            parameter.AppendSql(") ");
            parameter.AppendSql("VALUES ");
            parameter.AppendSql("(");
            parameter.AppendSql(" :ID, :OSHA_SITE_ID, :ESTIMATEDATE, :STARTDATE, :WORKERTOTALCOUNT, :OFFICIALFEE, :SITEFEE, ");
            parameter.AppendSql(" :MONTHLYFEE, :FEETYPE, :PRINTDATE, :SENDMAINDATE, 'N', SYSTIMESTAMP, :MODIFIEDUSER, SYSTIMESTAMP, :CREATEDUSER  ");
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

            parameter.Add("SENDMAINDATE", dto.SENDMAILDATE);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

            return FindById(id);
        }
        public HC_OSHA_ESTIMATE Update(HC_OSHA_ESTIMATE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_OSHA_ESTIMATE    ");
            parameter.AppendSql("SET  ");
            parameter.AppendSql("ESTIMATEDATE = :ESTIMATEDATE, STARTDATE = :STARTDATE, WORKERTOTALCOUNT = :WORKERTOTALCOUNT,");
            parameter.AppendSql("OFFICIALFEE = :OFFICIALFEE, SITEFEE = :SITEFEE, MONTHLYFEE = :MONTHLYFEE,                  ");
            parameter.AppendSql("FEETYPE = :FEETYPE , PRINTDATE = :PRINTDATE, SENDMAILDATE = :SENDMAILDATE, MODIFIED = SYSTIMESTAMP, MODIFIEDUSER = :MODIFIEDUSER ");
            parameter.AppendSql("WHERE ID = :ID    ");

            parameter.Add("ESTIMATEDATE", dto.ESTIMATEDATE);
            parameter.Add("STARTDATE", dto.STARTDATE);
            parameter.Add("WORKERTOTALCOUNT", dto.WORKERTOTALCOUNT);
            parameter.Add("OFFICIALFEE", dto.OFFICIALFEE);
            parameter.Add("SITEFEE", dto.SITEFEE);
            parameter.Add("MONTHLYFEE", dto.MONTHLYFEE);
            parameter.Add("FEETYPE", dto.FEETYPE);
            parameter.Add("PRINTDATE", dto.PRINTDATE);
            parameter.Add("SENDMAILDATE", dto.SENDMAILDATE);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("ID", dto.ID);

            ExecuteNonQuery(parameter);

            return FindById(dto.ID);
        }

        public void Delete(HC_OSHA_ESTIMATE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HC_OSHA_ESTIMATE    ");
            parameter.AppendSql("WHERE ID = :ID     ");

            parameter.Add("ID", dto.ID);
            ExecuteNonQuery(parameter);
        }
    }
}
