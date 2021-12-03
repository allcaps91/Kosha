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
    public class HicJepsuWorkPatientHeaJepsuRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuWorkPatientHeaJepsuRepository()
        {
        }

        public List<HIC_JEPSU_WORK_PATIENT_HEA_JEPSU> GetItembyGjJong(string strGjJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.SNAME,a.JUMINNO2,a.LTDCODE,a.pano,a.gjjong,to_char(a.jepdate,'yyyy-mm-dd') jepdate, a.gjyear  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK A, KOSMOS_PMPA.HIC_PATIENT B, KOSMOS_PMPA.HEA_JEPSU C                ");
            parameter.AppendSql(" WHERE (A.AGE = '40' OR A.AGE = '50' OR A.AGE = '60' OR A.AGE = '70')                                  ");
            parameter.AppendSql("   AND (A.SEXAMS NOT LIKE '%1164%' OR A.SEXAMS IS NULL)                                                ");
            parameter.AppendSql("   AND A.GJJONG = :GJJONG                                                                              ");
            parameter.AppendSql("   AND A.PANO = B.PANO                                                                                 ");
            parameter.AppendSql("   AND A.PANO = C.PANO                                                                                 ");
            parameter.AppendSql("   AND A.PANO IS NOT NULL                                                                              ");
            parameter.AppendSql("   AND C.SDATE >= TRUNC(SYSDATE) + 1                                                                   ");
            parameter.AppendSql(" ORDER BY A.SNAME                                                                                      ");

            parameter.Add("GJJONG", strGjJong);

            return ExecuteReader<HIC_JEPSU_WORK_PATIENT_HEA_JEPSU>(parameter);
        }

        public int GetCountbyJuminGjYearGjJong(string strJumin, string strGjYear, string strGjJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK A, KOSMOS_PMPA.HIC_PATIENT B     ");
            parameter.AppendSql(" WHERE a.PANO = b.Pano                                             ");
            parameter.AppendSql("   AND b.JUMIN2 = :JUMIN2                                          ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                          ");
            switch (strGjJong)
            {
                case "11":
                    parameter.AppendSql("   AND a.GJJONG = '16'                                     ");
                    break;
                case "12":
                    parameter.AppendSql("   AND a.GJJONG = '17'                                     ");
                    break;
                case "13":
                    parameter.AppendSql("   AND a.GJJONG = '18'                                     ");
                    break;
                case "14":
                    parameter.AppendSql("   AND a.GJJONG = '19'                                     ");
                    break;
                case "41":
                    parameter.AppendSql("   AND a.GJJONG = '44'                                     ");
                    break;
                case "42":
                    parameter.AppendSql("   AND a.GJJONG = '45'                                     ");
                    break;
                case "43":
                    parameter.AppendSql("   AND a.GJJONG = '46'                                     ");
                    break;
                case "23":
                    parameter.AppendSql("   AND a.GJJONG = '28'                                     ");
                    break;
                default:
                    break;
            }

            parameter.Add("JUMIN2", strJumin);
            parameter.Add("GJYEAR", strGjYear);

            return ExecuteScalar<int>(parameter);
        }

        public int GetPaNoPtNobyJumin2GjYear(string strJumin, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK A, KOSMOS_PMPA.HIC_PATIENT B     ");
            parameter.AppendSql(" WHERE a.PANO = b.Pano                                             ");
            parameter.AppendSql("   AND b.JUMIN2 = :JUMIN2                                          ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                          ");

            parameter.Add("JUMIN2", strJumin);
            parameter.Add("GJYEAR", strGjYear);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_JEPSU_WORK_PATIENT_HEA_JEPSU> GetItembyLtdCodeSNameGjJong(string strLtdCode, string strSname, string strGjJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.SNAME,a.JUMINNO,a.LTDCODE,a.PANO,a.GJJONG,TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE, a.GJYEAR, a.PTNO  ");
            parameter.AppendSql("      ,KOSMOS_PMPA.FC_HIC_LTDNAME(a.LTDCODE) AS LTDNAME                                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK a, KOSMOS_PMPA.HIC_PATIENT b                                         ");
            if (!strLtdCode.IsNullOrEmpty() && !strSname.IsNullOrEmpty())
            {
                parameter.AppendSql(" WHERE a.LTDCODE = :LTDCODE                                                                        ");
                parameter.AppendSql("   AND a.SNAME   = :SNAME                                                                          ");
                parameter.AppendSql("   AND a.PANO IS NOT NULL                                                                          ");
            }
            else if (!strLtdCode.IsNullOrEmpty() && strSname.IsNullOrEmpty())
            {
                parameter.AppendSql(" WHERE a.LTDCODE = :LTDCODE                                                                        ");
                parameter.AppendSql("   AND a.PANO IS NOT NULL                                                                          ");
            }
            else if (strLtdCode.IsNullOrEmpty() && !strSname.IsNullOrEmpty())
            {
                parameter.AppendSql(" WHERE a.SNAME = :SNAME                                                                            ");
                parameter.AppendSql("   AND a.PANO IS NOT NULL                                                                          ");
            }
            else
            {
                parameter.AppendSql(" WHERE a.PANO IS NOT NULL                                                                          ");
            }
            parameter.AppendSql("   AND a.PANO = b.PANO                                                                                 ");
            if (strGjJong != "00" && !strGjJong.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND a.GJJONG = :GJJONG                                                                            ");
            }
            parameter.AppendSql(" ORDER by a.SNAME                                                                                      ");

            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", strLtdCode);
            }
            if (!strSname.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strSname);
            }
            if (strGjJong != "00" && !strGjJong.IsNullOrEmpty())
            {
                parameter.Add("GJJONG", strGjJong);
            }

            return ExecuteReader<HIC_JEPSU_WORK_PATIENT_HEA_JEPSU>(parameter);
        }
    }
}
