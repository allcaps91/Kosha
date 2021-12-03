namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class ExamSpecodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public ExamSpecodeRepository()
        {
        }

        public EXAM_SPECODE GetItemByCodeGubun(string argSpecCode, string argGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME, YNAME, WSGROUP  ");
            parameter.AppendSql("  FROM ADMIN.EXAM_SPECODE     ");
            parameter.AppendSql(" WHERE CODE =:CODE                 ");
            parameter.AppendSql("   AND GUBUN =:GUBUN               ");

            parameter.Add("CODE", argSpecCode);
            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<EXAM_SPECODE>(parameter);
        }

        public List<EXAM_SPECODE> GetNamebyCode(string vOLUMECODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME FROM ADMIN.EXAM_SPECODE   ");
            parameter.AppendSql(" WHERE Gubun = '16'                        ");
            parameter.AppendSql("   AND (Code LIKE 'W%' OR Code LIKE 'V%')  ");
            parameter.AppendSql("   AND CODE = :CODE                        ");

            parameter.Add("CODE", vOLUMECODE);

            return ExecuteReader<EXAM_SPECODE>(parameter);
        }
    }
}
