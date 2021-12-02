namespace ComHpcLibB.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HicPatientLtdRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicPatientLtdRepository()
        {
        }

        public HIC_PATIENT_LTD GetItembyJumin(string strJumin)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.LTDCODE, B.NAME                                   ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_PATIENT A, KOSMOS_PMPA.HIC_LTD B     ");
            parameter.AppendSql(" WHERE 1 =1                                                ");
            parameter.AppendSql(" AND A.JUMIN2 = :JUMIN                                     ");
            parameter.AppendSql(" AND A.LTDCODE = B.CODE                                    ");

            parameter.Add("JUMIN", strJumin);
            return ExecuteReaderSingle<HIC_PATIENT_LTD>(parameter);
        }
    }
}
