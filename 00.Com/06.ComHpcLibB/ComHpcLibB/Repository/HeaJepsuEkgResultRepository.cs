namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuEkgResultRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuEkgResultRepository()
        {
        }

        public List<HEA_JEPSU_EKG_RESULT> GetItembySDate(string strFrDate, string strToDate, List<string> strGbSts, string strEkg)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.WRTNO, A.SName,TO_CHAR(A.SDate,'YY-MM-DD') SDate, A.GjJong ,A.GBEKG, B.EKGRESULT, A.PTNO  ");
            parameter.AppendSql("  FROM ADMIN.HEA_JEPSU A, ADMIN.HEA_EKG_RESULT B                                       ");
            parameter.AppendSql(" WHERE A.SDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                   ");
            parameter.AppendSql("   AND A.SDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                   ");
            if (!strGbSts.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND A.GbSTS IN (:GBSTS)                                                                     ");
            }
            parameter.AppendSql("   AND A.DELDATE IS  NULL                                                                          ");
            parameter.AppendSql("   AND A.WRTNO = B.WRTNO(+)                                                                        ");
            if (strEkg == "Y")
            {
                parameter.AppendSql("   AND EKGRESULT IS NULL                                                                       ");
            }
            parameter.AppendSql(" ORDER BY SName,WRTNO                                                                              ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strGbSts.IsNullOrEmpty())
            {
                parameter.AddInStatement("GBSTS", strGbSts);
            }

            return ExecuteReader<HEA_JEPSU_EKG_RESULT>(parameter);
        }
    }
}
