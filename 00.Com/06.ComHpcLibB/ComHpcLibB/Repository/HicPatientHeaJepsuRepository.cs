namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicPatientHeaJepsuRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicPatientHeaJepsuRepository()
        {
        }

        public HIC_PATIENT_HEA_JEPSU GetItembyJuminOrBirth(string strJumin, string strAesJumin, string strBirth, string strYearFr, string strYearTo, string strSName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.WRTNO, b.PANO, TO_CHAR(b.SDATE,'YYYY-MM-DD') SDATE    ");
            parameter.AppendSql("     , b.STIME, b.SEX, b.AGE, b.SNAME, b.GBSTS                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT a                               ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_JEPSU   b                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                   ");
            if (strJumin.Length == 13)
            {
                parameter.AppendSql("   AND a.JUMIN2 = :JUMIN2                                   ");
            }
            else
            {
                parameter.AppendSql("   AND a.SNAME = :SNAME                                    ");
                parameter.AppendSql("   AND a.JUMIN LIKE   :BIRTH                               ");
            }
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                                      ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                       ");
            parameter.AppendSql("   AND b.SDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')               ");
            parameter.AppendSql("   AND b.SDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')               ");

            if (strJumin.Length == 13)
            {
                parameter.Add("JUMIN2", strAesJumin);
            }
            else
            {
                parameter.AddLikeStatement("BIRTH", strBirth);
                parameter.Add("SNAME", strSName);
            }
            
            parameter.Add("FRDATE", strYearFr);
            parameter.Add("TODATE", strYearTo);            

            return ExecuteReaderSingle<HIC_PATIENT_HEA_JEPSU>(parameter);
        }
    }
}
