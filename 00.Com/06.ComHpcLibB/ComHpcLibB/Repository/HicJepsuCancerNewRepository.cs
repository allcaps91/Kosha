namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuCancerNewRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuCancerNewRepository()
        {
        }

        public List<HIC_JEPSU_CANCER_NEW> GetItembyJepDate(string strFrDate, string strToDate, string strJob)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT a.WRTNO, a.Sname, b.S_SOGEN,b.S_SOGEN2,b.C_SOGEN,b.C_SOGEN2,b.C_SOGEN3,b.L_SOGEN   ");
            parameter.AppendSql("      , b.B_SOGEN,b.W_SOGEN                                                                ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_CANCER_NEW b                              ");
            parameter.AppendSql("  WHERE a.WRTNO = b.WRTNO                                                                  ");
            parameter.AppendSql("    AND a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                        ");
            parameter.AppendSql("    AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                        ");
            parameter.AppendSql("    AND a.DELDATE IS NULL                                                                  ");
            parameter.AppendSql("    AND a.GJJONG = '31'                                                                    ");
            switch (strJob)
            {
                case "1":
                    parameter.AppendSql("    AND b.S_SOGEN IS NOT NULL                                                      ");
                    break;
                case "2":
                    parameter.AppendSql("    AND b.S_SOGEN2 IS NOT NULL                                                     ");
                    break;
                case "3":
                    parameter.AppendSql("    AND b.C_SOGEN IS NOT NULL                                                      ");
                    break;
                case "4":
                    parameter.AppendSql("    AND b.C_SOGEN2 IS NOT NULL                                                     ");
                    break;
                case "5":
                    parameter.AppendSql("    AND b.C_SOGEN3 IS NOT NULL                                                     ");
                    break;
                case "6":
                    parameter.AppendSql("    AND b.L_SOGEN IS NOT NULL                                                      ");
                    break;
                case "7":
                    parameter.AppendSql("    AND b.B_SOGEN IS NOT NULL                                                      ");
                    break;
                case "8":
                    parameter.AppendSql("    AND b.W_SOGEN IS NOT NULL                                                      ");
                    break;
                default:
                    break;
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReader<HIC_JEPSU_CANCER_NEW>(parameter);
        }

        public List<HIC_JEPSU_CANCER_NEW> GetItemWrtNoInbyJepDateMirNo(string strFrDate, string strToDate, long nMirNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT a.WRTNO,b.SNAME,TO_CHAR(b.JepDate,'YYYY-MM-DD') JepDate                ");
            parameter.AppendSql("      , a.GBSTOMACH,a.GbLiver,a.GBRECTUM, a.GBBREAST,a.GbWomb                  ");
            parameter.AppendSql("      , DECODE(a.S_ANATGBN,'1','1','2','0','0')  AS S_ANATGBN                  "); // 위조직실시
            parameter.AppendSql("      , DECODE(a.S_ANAT,' ','0','','0','1')  AS S_ANAT                         "); //위조직결과
            parameter.AppendSql("      , DECODE(a.C_ANATGBN,'1','1','2','0','0')  AS C_ANATGBN                  "); //대장조직실시
            parameter.AppendSql("      , DECODE(a.C_ANAT,' ','0','','0','1')  AS C_ANAT                         "); //대장조직결과
            parameter.AppendSql("      , JINCHALGBN                                                             ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_CANCER_NEW a, KOSMOS_PMPA.HIC_JEPSU b                  ");
            parameter.AppendSql("  WHERE a.WRTNO IN ( SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU                   ");
            parameter.AppendSql("                      WHERE JepDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')           ");
            parameter.AppendSql("                        AND JepDate <= TO_DATE(:TODATE,'YYYY-MM-DD')           ");
            parameter.AppendSql("                        AND MIRNO3 = :MIRNO                                    ");
            parameter.AppendSql("                   )                                                           ");
            parameter.AppendSql("    AND a.WrTNO = b.WRTNO(+)                                                   ");
            parameter.AppendSql("   ORDER BY a.WRTNO                                                            ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("MIRNO", nMirNo);

            return ExecuteReader<HIC_JEPSU_CANCER_NEW>(parameter);
        }

        public List<HIC_JEPSU_CANCER_NEW> GetItembyJepDateMirNo2(string argFrDate, string argToDate, long argMirno, string strJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT a.WRTNO,a.Pano,a.Kiho,a.GjJong,a.JikGbn,a.Sname,a.Sex,a.Age,a.GjChasu  ");
            parameter.AppendSql("      , TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate                                ");
            parameter.AppendSql("      , TO_CHAR(b.GunDate,'YYYY-MM-DD') GunDate                                ");
            parameter.AppendSql("      , TO_CHAR(DelDate, 'YYYY-MM-DD') DelDate                                 ");
            parameter.AppendSql("      , TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate                          ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_CANCER_NEW b                  ");
            parameter.AppendSql("  WHERE a.Wrtno = b.Wrtno(+)                                                   ");
            parameter.AppendSql("    AND a.JepDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')                             ");
            parameter.AppendSql("    AND a.JepDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                             ");
            if (strJong == "4")
            {
                parameter.AppendSql("    AND a.MIRNO3 = :MIRNO                                                  ");
            }
            else
            {
                parameter.AppendSql("    AND a.MIRNO5 = :MIRNO                                                  ");
            }
            parameter.Add("FRDATE", argFrDate);
            parameter.Add("TODATE", argToDate);
            parameter.Add("MIRNO", argMirno);

            return ExecuteReader<HIC_JEPSU_CANCER_NEW>(parameter);
        }

        public List<HIC_JEPSU_CANCER_NEW> GetItembyJepDateMirNo(string argFrDate, string argToDate, long argMirno, string strJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT a.WRTNO,a.Pano,a.Kiho,a.GjJong,a.JikGbn,a.Sname,a.Sex,a.Age,a.GjChasu  ");
            parameter.AppendSql("      , TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate                                ");
            parameter.AppendSql("      , TO_CHAR(b.GunDate,'YYYY-MM-DD') GunDate                                ");
            parameter.AppendSql("      , TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate                          ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_CANCER_NEW b                  ");
            parameter.AppendSql("  WHERE a.Wrtno = b.Wrtno                                                      ");
            parameter.AppendSql("    AND a.JepDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')                             ");
            parameter.AppendSql("    AND a.JepDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                             ");
            if (strJong == "4" || strJong == "C")
            {
                parameter.AppendSql("    AND a.MIRNO3 = :MIRNO                                                  ");
            }
            else
            {
                parameter.AppendSql("    AND a.MIRNO4 = :MIRNO                                                  ");
            }
            parameter.Add("FRDATE", argFrDate);
            parameter.Add("TODATE", argToDate);
            parameter.Add("MIRNO", argMirno);

            return ExecuteReader<HIC_JEPSU_CANCER_NEW>(parameter);
        }

        public List<HIC_JEPSU_CANCER_NEW> GetItembyJepDateJongGMirNo(string sJepDate, string sJong, long argMirNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT a.LtdCode,a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.GjJong,a.GjChasu,a.BOGUNSO,a.GbChul,a.GbChul2                      ");
            parameter.AppendSql("      , a.SECOND_Flag,TO_CHAR(a.SECOND_Date,'YYYY-MM-DD') Second_Date, b.Jumin2, a.Kiho,a.Muryoam , a.Gkiho ,c.*                       ");
            parameter.AppendSql("      , TO_CHAR(c.GunDate,'YYYY-MM-DD') GunDate,TO_CHAR(c.TongboDate,'YYYY-MM-DD') TongboDate, a.Juso1 || ' ' || a.Juso2 AS JusoAA     ");
            parameter.AppendSql("      , TO_CHAR(c.S_PanjengDate,'YYYY-MM-DD') S_PanjengDate,TO_CHAR(c.C_PanjengDate,'YYYY-MM-DD') C_PanjengDate                        ");
            parameter.AppendSql("      , TO_CHAR(c.L_PanjengDate,'YYYY-MM-DD') L_PanjengDate,TO_CHAR(c.B_PanjengDate,'YYYY-MM-DD') B_PanjengDate                        ");
            parameter.AppendSql("      , TO_CHAR(c.W_PanjengDate,'YYYY-MM-DD') W_PanjengDate,a.ptno                                                                     ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_PATIENT b , KOSMOS_PMPA.HIC_CANCER_NEW c                                              ");
            parameter.AppendSql("  WHERE a.JEPDATE>=TO_DATE(:JEPDATE,'YYYY-MM-DD')                                                                                      ");
            if (sJong == "4")
            {
                parameter.AppendSql("     AND a.MIRNO3 = :MIRNO                                                                                                         ");
            }
            else if (sJong == "E")
            {
                parameter.AppendSql("     AND a.MIRNO5 = :MIRNO                                                                                                         ");
            }
            parameter.AppendSql("     AND a.Pano = b.Pano                                                                                                               ");
            parameter.AppendSql("     AND a.Wrtno = c.Wrtno                                                                                                             ");
            parameter.AppendSql("  ORDER BY a.Pano,a.WRTNO                                                                                                              ");

            parameter.Add("PANO", sJepDate);
            parameter.Add("MIRNO", argMirNo);

            return ExecuteReader<HIC_JEPSU_CANCER_NEW>(parameter);
        }

        public HIC_JEPSU_CANCER_NEW GetItembyPaNoGjYear(long argPano, string strYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT LUNG_RESULT070, LUNG_RESULT071                         ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_CANCER_NEW b  ");
            parameter.AppendSql("  WHERE a.WRTNO  = b.WRTNO                                     ");
            parameter.AppendSql("    AND A.GJJONG = '31'                                        ");
            parameter.AppendSql("    AND A.PANO   = :PANO                                       ");
            parameter.AppendSql("    AND A.GJYEAR = :GJYEAR                                     ");
            parameter.AppendSql("    AND A.SEXAMS LIKE '%3169%'                                 ");
            parameter.AppendSql("    AND A.DELDATE IS NULL                                      ");
            
            parameter.Add("PANO", argPano);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_JEPSU_CANCER_NEW>(parameter);
        }
    }
}
