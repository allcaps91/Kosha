namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class EndoJupmstOrdercodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public EndoJupmstOrdercodeRepository()
        {
        }

        public List<ENDO_JUPMST_ORDERCODE> GetListByRDate(string argFDate, string argTDate, bool chkHc)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.PTNO, a.SNAME, (a.SEX || '/' || KOSMOS_OCS.FC_GET_AGE2(a.PTNO, a.BDATE)) AS S_AGE     ");
            parameter.AppendSql("      ,b.DISPHEADER || ' ' || b.ORDERNAME AS ORDERNAME, TO_CHAR(a.RDATE, 'HH24:MI') RDATE, a.DEPTCODE                    ");
            parameter.AppendSql("      ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.DRCODE) AS DRNAME                                     ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ENDO_JUPMST a                                                                ");
            parameter.AppendSql("      ,KOSMOS_OCS.OCS_ORDERCODE b                                                              ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                   ");
            parameter.AppendSql("   AND a.RDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                                ");
            parameter.AppendSql("   AND a.RDATE <  TO_DATE(:TDATE, 'YYYY-MM-DD')                                                ");
            if (chkHc)
            {
                parameter.AppendSql("   AND a.DEPTCODE IN ('TO','HR')                                                           ");
            }
            parameter.AppendSql("   AND a.ORDERCODE = b.ORDERCODE(+)                                                            ");
            parameter.AppendSql("   AND (b.SLIPNO = '0044' OR b.SLIPNO = '0064')                                                ");
            parameter.AppendSql("   AND a.GBSUNAP IN ('1', '2')                                                                 ");
            parameter.AppendSql(" ORDER BY a.RDATE, a.ENTDATE DESC                                                              ");

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);

            return ExecuteReader<ENDO_JUPMST_ORDERCODE>(parameter);
        }
    }
}
