namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicCancerResv2PatientRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicCancerResv2PatientRepository()
        {
        }

        public List<HIC_CANCER_RESV2_PATIENT> GetItembyRTime(string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(a.RTIME,'HH24:MI') RTIME, a.JUMIN, a.SNAME  ");
            parameter.AppendSql("     , a.PANO, a.HPHONE, a.JUMIN2, b.SEX                   ");
            parameter.AppendSql("  FROM ADMIN.HIC_CANCER_RESV2 a                      ");
            parameter.AppendSql("     , ADMIN.BAS_PATIENT      b                      ");
            parameter.AppendSql(" WHERE a.RTIME >= TO_DATE(:RTIME, 'YYYY-MM-DD')            ");
            parameter.AppendSql("   AND a.RTIME <= TO_DATE(:RTIME, 'YYYY-MM-DD') + 0.999999 ");
            parameter.AppendSql("   AND a.PANO IS NOT NULL                                  ");
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                                  ");
            parameter.AppendSql("   AND a.GBGFSH = 'Y'                                      ");
            parameter.AppendSql("   AND a.PANO > 1000                                       ");
            parameter.AppendSql(" ORDER BY a.SNAME                                          ");

            parameter.Add("RTIME", strDate);

            return ExecuteReader<HIC_CANCER_RESV2_PATIENT>(parameter);
        }

        public List<HIC_CANCER_RESV2_PATIENT> GetItembySDate(string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(c.SDATE,'HH24:MI') RTIME, a.SNAME, a.PTNO, b.SEX, a.PANO  ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU_WORK a                        ");
            parameter.AppendSql("     , ADMIN.BAS_PATIENT b                           ");
            parameter.AppendSql("     , ADMIN.HEA_JEPSU c                             ");
            parameter.AppendSql(" WHERE c.SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')             ");
            parameter.AppendSql("   AND a.GJJONG = '31'                                     ");
            parameter.AppendSql("   AND a.PTNO = b.PANO                                     ");
            parameter.AppendSql("   AND a.PTNO = c.PTNO                                     ");
            parameter.AppendSql("   AND a.SEXAMS LIKE '%3111%'                              ");
            parameter.AppendSql("   AND a.PANO > 1000                                       ");
            parameter.AppendSql(" ORDER BY a.SNAME                                          ");

            parameter.Add("SDATE", strDate);

            return ExecuteReader<HIC_CANCER_RESV2_PATIENT>(parameter);
        }
    }
}
