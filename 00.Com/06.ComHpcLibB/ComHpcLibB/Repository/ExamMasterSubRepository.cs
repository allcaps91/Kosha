namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class ExamMasterSubRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public ExamMasterSubRepository()
        {
        }

        public List<EXAM_MASTER_SUB> GetNormalByCode(string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NORMAL                      ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_MASTER_SUB  ");
            parameter.AppendSql(" WHERE MASTERCODE =:MASTERCODE     ");
            parameter.AppendSql("   AND GUBUN = '31'                ");
            parameter.AppendSql("   AND NORMAL > ' '                ");
            parameter.AppendSql(" ORDER BY SORT                     ");

            parameter.Add("MASTERCODE", argCode);

            return ExecuteReader<EXAM_MASTER_SUB>(parameter);
        }
    }
}
