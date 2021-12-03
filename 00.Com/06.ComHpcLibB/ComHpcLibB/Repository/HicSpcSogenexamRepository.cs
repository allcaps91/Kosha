namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcSogenexamRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcSogenexamRepository()
        {
        }

        public HIC_SPC_SOGENEXAM GetHangbySogenCode(string fstrCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT HANG                            ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_SogenExam   ");
            parameter.AppendSql(" WHERE SOGENCODE = :SOGENCODE          ");

            parameter.Add("SOGENCODE", fstrCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_SPC_SOGENEXAM>(parameter);
        }
    }
}
