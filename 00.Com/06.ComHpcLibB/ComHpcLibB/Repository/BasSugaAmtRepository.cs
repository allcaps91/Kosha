namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class BasSugaAmtRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public BasSugaAmtRepository()
        {
        }

        public long GetBAmtBySuNext(string argSucode, string argBDate)
        {
            MParameter parameter = CreateParameter();

            //SQL = "SELECT SuCode,SuNext,TO_CHAR(SuDate,'YYYY-MM-DD') SuDate,BAmt "
            //SQL = SQL & " FROM BAS_SUGA_AMT "
            //SQL = SQL & "WHERE SuNext='" & Trim(UCase(strSuCode)) & "' "
            //SQL = SQL & "  AND SuDate=TO_DATE('" & Left(Trim(txtBDate.Text), 4) & "-01-01','YYYY-MM-DD') "
            //SQL = SQL & "ORDER BY SuCode "
            //Call AdoOpenSet(Rs, SQL)
            //nSuAmt = AdoGetNumber(Rs, "bamt", 0)
            //Call AdoCloseSet(Rs)

            parameter.AppendSql("SELECT BAMT FROM KOSMOS_PMPA.BAS_SUGA_AMT              ");
            parameter.AppendSql(" WHERE SUCODE LIKE :SUCODE                             ");
            parameter.AppendSql(" ORDER BY SUCODE                                       ");

            parameter.AddLikeStatement("SUCODE", argSucode);

            return ExecuteScalar<long>(parameter);
        }
    }
}
