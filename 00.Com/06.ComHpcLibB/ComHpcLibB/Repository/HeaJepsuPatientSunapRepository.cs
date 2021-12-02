namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuPatientSunapRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuPatientSunapRepository()
        {
        }

        public List<HEA_JEPSU_PATIENT_SUNAP> GetItemsbySDate(string strFrDate, string strToDate, string strJob)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.SName,a.Age,a.Sex,a.LtdCode                                           ");
            parameter.AppendSql("     , TO_CHAR(a.SDate,'YYYY-MM-DD') SDate                                             ");
            parameter.AppendSql("     , b.HPhone,b.VipRemark,SUM(c.TotAmt) TotAmt                                       ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(a.LTDCODE) LTDNAME                                   ");
            parameter.AppendSql("     , b.PTNO                                                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a, KOSMOS_PMPA.HIC_PATIENT b, KOSMOS_PMPA.HEA_SUNAP c     ");
            parameter.AppendSql(" WHERE a.SDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                        ");
            parameter.AppendSql("   AND a.SDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                                        ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                               ");
            parameter.AppendSql("   AND a.GbSTS NOT IN ('0','D')                                                        ");
            parameter.AppendSql("   AND a.Pano=b.Pano(+)                                                                ");
            if (strJob == "1")
            {
                parameter.AppendSql("   AND b.VipRemark IS NOT NULL                                                     ");
            }
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO(+)                                                            ");
            parameter.AppendSql(" GROUP BY a.WRTNO,a.SName,a.Age,a.Sex,a.LtdCode,TO_CHAR(a.SDate,'YYYY-MM-DD'),b.HPhone ");
            parameter.AppendSql("        , b.VipRemark, b.PTNO                                                          ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReader<HEA_JEPSU_PATIENT_SUNAP>(parameter);
        }

        public HEA_JEPSU_PATIENT_SUNAP GetGroupBySosokbyWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.SOSOK, a.SNAME, b.JUMIN2, SUM(c.TOTAMT) AS TOTAMT     ");
            parameter.AppendSql("      ,SUM(c.BONINAMT) AS BONINAMT                             ");
            parameter.AppendSql("      ,TO_CHAR(a.SDATE,'YYYY-MM-DD') AS SDATE                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a                                 ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_PATIENT b                               ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HEA_SUNAP c                                 ");
            parameter.AppendSql(" WHERE 1 = 1                                                   ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                        ");
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                                      ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO(+)                                    ");
            parameter.AppendSql(" GROUP BY b.SOSOK, a.SNAME, b.JUMIN2, a.SDATE                  ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReaderSingle<HEA_JEPSU_PATIENT_SUNAP>(parameter);
        }

        public HEA_JEPSU_PATIENT_SUNAP GetGroupByLtdCodebyWrtno(long argWRTNO)
        {
            //KOSMOS_PMPA.FC_HIC_LTDNAME(a.LTDCODE) LTDNAME
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.LTDCODE, a.SNAME, b.JUMIN2, SUM(c.TOTAMT) AS TOTAMT   ");
            parameter.AppendSql("      ,SUM(c.BONINAMT) AS BONINAMT                             ");
            parameter.AppendSql("      ,TO_CHAR(a.SDATE,'YYYY-MM-DD') AS SDATE                  ");
            parameter.AppendSql("      ,KOSMOS_PMPA.FC_HIC_LTDNAME(a.LTDCODE) AS LTDNAME        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a                                 ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_PATIENT b                               ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HEA_SUNAP c                                 ");
            parameter.AppendSql(" WHERE 1 = 1                                                   ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                        ");
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                                      ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO(+)                                    ");
            parameter.AppendSql(" GROUP BY a.LTDCODE, a.SNAME, b.JUMIN2, a.SDATE                ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReaderSingle<HEA_JEPSU_PATIENT_SUNAP>(parameter);
        }
    }
}
