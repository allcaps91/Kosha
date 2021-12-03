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
    public class HicJepsuSchoolNewRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuSchoolNewRepository()
        {
        }

        public List<HIC_JEPSU_SCHOOL_NEW> GetItembyJepDate(string strFDate, string strTDate, string strJob, string strSName, long nLtdCode, long nLicense)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,a.SName,TO_CHAR(a.JepDate,'YY-MM-DD') JepDate,a.GjJong,a.UCodes,a.SangDamDrno   ");
            parameter.AppendSql("     , b.DPANDRNO, b.DPANDATE                                                                  ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_SCHOOL_NEW b                                   ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'yyyy-mm-dd')                                             ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'yyyy-mm-dd')                                             ");
            parameter.AppendSql("   AND b.WRTNO = a.WRTNO                                                                       ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                       ");
            parameter.AppendSql("   AND a.GjYear >= '2009'                                                                      ");   //2009년 부터사용
            if (strJob == "1")  //대기자
            {
                parameter.AppendSql("   AND (b.DPANDRNO IS NULL OR b.DPANDRNO = 0)                                              ");
            }
            else    //상담자
            {
                parameter.AppendSql("   AND b.DPANDRNO = :DPANDRNO                                                              ");
            }
            if (strSName != "")
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                                 ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                ");
            }
            parameter.AppendSql("   AND a.GjJong IN ('56')                                                                      "); //학생검진만
            parameter.AppendSql(" ORDER BY a.SName,a.WRTNO,a.GjJong                                                             ");
            
            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            if (strSName != "")
            {
                parameter.Add("SNAME", strSName);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            if (strJob != "1")  //대기자
            {
                parameter.Add("DPANDRNO", nLicense);
            }

            return ExecuteReader<HIC_JEPSU_SCHOOL_NEW>(parameter);
        }

        public List<HIC_JEPSU_SCHOOL_NEW> GetItembyJepDateClassBanBun(string strFrDate, string strToDate, string strSName, long nLtdCode, string strClass, string strBan, string strBun, string strSort)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.sname, a.ltdcode, b.class, b.ban, b.bun, a.sex, a.age,to_char(a.jepdate,'yyyy-mm-dd') jepdate     ");
            parameter.AppendSql("     , a.wrtno,b.jumin2,b.PPANA1,b.PPANA2,b.PPANA3,b.PPANA4                                                ");
            parameter.AppendSql("  FROM ADMIN.hic_jepsu a, ADMIN.hic_school_new b                                               ");
            parameter.AppendSql(" WHERE a.jepdate >= to_date(:FRDATE, 'YYYY-MM-DD')                                                         ");
            parameter.AppendSql("   AND a.jepdate <= to_date(:TODATE, 'YYYY-MM-DD')                                                         ");
            parameter.AppendSql("   AND a.wrtno = b.wrtno(+)                                                                                ");
            parameter.AppendSql("   AND a.Deldate is null                                                                                   ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('D')                                                                                ");
            parameter.AppendSql("   AND a.GjJong='56'                                                                                       ");    //학생신검만
            //성명으로 찾기
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND a.SNAME Like :SNAME                                                                               ");
            }
            if (!nLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND a.LTDCODE = :LTDCODE                                                                              ");
            }
            if (strClass != "*.전체" && !strClass.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND b.CLASS = :CLASS                                                                                  ");
            }
            if (strBan != "*.전체" && !strBan.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND b.BAN = :BAN                                                                                      ");
            }
            if (!strBun.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND b.BUN = :BUN                                                                                      ");
            }
            if (strSort == "1")
            {
                parameter.AppendSql(" ORDER BY a.ltdcode, b.class, b.ban, b.bun, a.sname                                                    ");
            }
            if (strSort == "2")
            {
                parameter.AppendSql(" ORDER BY a.sname, a.ltdcode, b.class, b.ban, b.bun                                                    ");
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            if (!strSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strSName);
            }
            if (!nLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            if (strClass != "*.전체" && strClass.IsNullOrEmpty())
            {
                parameter.Add("CLASS", strClass);
            }
            if (strBan != "*.전체" && !strBan.IsNullOrEmpty())
            {
                parameter.Add("BAN", strBan);
            }
            if (!strBun.IsNullOrEmpty())
            {
                parameter.Add("BUN", strBun);
            }

            return ExecuteReader<HIC_JEPSU_SCHOOL_NEW>(parameter);
        }

        public HIC_JEPSU_SCHOOL_NEW GetItembyJepDateWrtNo(string strFrDate, string strToDate, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE,a.PANO,a.SNAME,a.SEX,a.AGE  ");
            parameter.AppendSql("     , a.WRTNO,a.LTDCODE,b.JUMIN2, c.*                                     ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU      a                                        ");
            parameter.AppendSql("     , ADMIN.HIC_PATIENT    b                                        ");
            parameter.AppendSql("     , ADMIN.HIC_SCHOOL_NEW c                                        ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                   ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('D')                                                ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                    ");
            parameter.AppendSql("   AND a.GJJONG = '56'                                                     "); //학생신검만
            parameter.AppendSql("   AND a.PANO  = b.Pano(+)                                                 ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO(+)                                                ");
            parameter.AppendSql(" ORDER BY a.JepDate,a.SName                                                ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReaderSingle<HIC_JEPSU_SCHOOL_NEW>(parameter);
        }
    }
}
