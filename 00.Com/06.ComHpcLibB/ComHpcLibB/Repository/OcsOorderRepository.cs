
namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    public class OcsOorderRepository : BaseRepository
    {

        public OcsOorderRepository()
        {
        }

        public int DeleteOcsOorder(string argPtno, string argBdate, string argDeptcode, long argWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" DELETE KOSMOS_OCS.OCS_OORDER                                              ");
            parameter.AppendSql("  WHERE PTNO = :PTNO                                                       ");
            parameter.AppendSql("  AND BDATE = TO_DATE(:BDATE ,'YYYY-MM-DD')                                ");
            parameter.AppendSql("  AND DEPTCODE = :DEPTCODE                                                 ");
            parameter.AppendSql("  AND SUCODE IN ( SELECT SUCODE FROM KOSMOS_PMPA.HIC_HYANG_APPROVE         ");
            parameter.AppendSql("  WHERE BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                              ");
            parameter.AppendSql("  AND WRTNO = :WRTNO                                                       ");
            parameter.AppendSql("  AND DELDATE IS NULL)                                                     ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("BDATE", argBdate);
            parameter.Add("DEPTCODE", argDeptcode);
            parameter.Add("WRTNO", argWrtno);

            return ExecuteNonQuery(parameter);
        }

    }
}
