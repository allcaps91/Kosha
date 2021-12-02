namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuHeaExjongRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuHeaExjongRepository()
        {
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetGbStsbyWrtNo(string strSDate, string strSName = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT A.WRTNO,A.SNAME,A.GJJONG, B.NAME, A.PTNO, A.LTDCODE,A.ACTMEMO  ");
            parameter.AppendSql("      , A.INBODY,A.AMPM2,A.GONGDAN,A.GBEXAM                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU  A                                        ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_EXJONG B                                        ");
            parameter.AppendSql(" WHERE A.SDATE = TO_DATE(:SDATE,'YYYY-MM-DD')                          ");
            parameter.AppendSql("   AND A.DELDATE IS  NULL                                              ");
            parameter.AppendSql("   AND A.GBSTS NOT IN ('0','D')                                        ");
            parameter.AppendSql("   AND A.GJJONG = B.CODE(+)                                            ");
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND A.SNAME LIKE :SNAME                                         ");
            }
            parameter.AppendSql(" ORDER By A.SNAME                                                      ");

            parameter.Add("SDATE", strSDate);
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", strSName);
            }

            return ExecuteReader<HIC_JEPSU_HEA_EXJONG>(parameter);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetItembyJepDate(string strFrDate, string strToDate, string strJob, string strLtdCode, string strGjJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.SName,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.GjJong    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_EXJONG b                   ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                   ");
            parameter.AppendSql("   AND a.GJYEAR >='2009'                                                   ");
            if (strJob == "1") //신규
            {
                parameter.AppendSql("   AND (a.GBMUNJIN1 IS NULL OR a.GBMUNJIN1<>'Y')                       ");
            }
            else
            {
                parameter.AppendSql("   AND a.GBMUNJIN1 = 'Y'                                               ");
            }
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                            ");
            }
            if (strGjJong != "**" && !strGjJong.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GJJONG = :GJJONG                                                ");
            }
            parameter.AppendSql("   AND a.GjJong NOT IN ('31','35')                                         "); //암검진 제외
            parameter.AppendSql("   AND a.GjJong = b.Code(+)                                                ");
            parameter.AppendSql("   AND a.GjYear >='2009'                                                   "); //2009년 부터사용
            parameter.AppendSql("   AND b.GbMunjin IN ('1','3','4')                                         "); //건강보험,건강보험+특수
            parameter.AppendSql(" ORDER BY a.SName,a.WRTNO                                                  ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", strLtdCode);
            }
            if (strGjJong != "**" && !strGjJong.IsNullOrEmpty())
            {
                parameter.Add("GJJONG", strGjJong, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_JEPSU_HEA_EXJONG>(parameter);
        }

        public HIC_JEPSU_HEA_EXJONG GetHeaJepInfo(string argPtno, string argDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT WRTNO, SNAME, GJJONG, PANO, PTNO, LTDCODE, GBSTS   ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HEA_JEPSU                              ");
            parameter.AppendSql("  WHERE 1 = 1                                              ");
            parameter.AppendSql("    AND SDATE >= TO_DATE(:SDATE, 'YYYY-MM-DD')             ");
            parameter.AppendSql("    AND PTNO =:PTNO                                        ");
            parameter.AppendSql("    AND DELDATE IS  NULL                                   ");
            parameter.AppendSql("    AND GBSTS != 'D'                                       ");
            parameter.AppendSql("  ORDER BY SDATE DESC                                      ");

            parameter.Add("SDATE", argDate);
            parameter.Add("PTNO", argPtno);

            return ExecuteReaderSingle<HIC_JEPSU_HEA_EXJONG>(parameter);
        }

        public HIC_JEPSU_HEA_EXJONG GetHeaJepInfoByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT WRTNO, SNAME, GJJONG, PANO, PTNO, LTDCODE, GBSTS   ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HEA_JEPSU                              ");
            parameter.AppendSql("  WHERE 1 = 1                                              ");
            parameter.AppendSql("    AND WRTNO =:WRTNO                                      ");
            parameter.AppendSql("    AND DELDATE IS  NULL                                   ");
            parameter.AppendSql("    AND GBSTS != 'D'                                       ");
            parameter.AppendSql("  ORDER BY SDATE DESC                                      ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReaderSingle<HIC_JEPSU_HEA_EXJONG>(parameter);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetListGaJepsuByPtnoYear(string argPtno, string argYear)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.ROWID AS RID                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK a                    ");
            parameter.AppendSql(" WHERE a.PTNO    = :PTNO                               ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                               ");
            parameter.AppendSql("   AND a.GJYEAR =:GJYEAR                               ");

            parameter.Add("PTNO", argPtno, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GJYEAR", argYear);

            return ExecuteReader<HIC_JEPSU_HEA_EXJONG>(parameter);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetListGaJepsuByPtnoYear_Old(string argPtno, string argYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.ROWID AS RID, '1' AS JEPGBN                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK a                    ");
            parameter.AppendSql(" WHERE a.PTNO    = :PTNO                               ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                               ");
            parameter.AppendSql("   AND a.GJYEAR =:GJYEAR                               ");
            parameter.AppendSql(" UNION ALL                                             ");
            parameter.AppendSql("SELECT b.ROWID AS RID, '2' AS JEPGBN                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU b                         ");
            parameter.AppendSql(" WHERE b.PTNO    = :PTNO                               ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                               ");
            parameter.AppendSql("   AND b.GBSTS = '0'                                   ");

            parameter.Add("PTNO", argPtno, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GJYEAR", argYear);

            return ExecuteReader<HIC_JEPSU_HEA_EXJONG>(parameter);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetListWrtnoByHic(string argPtno, string argDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO AS WRTNO, a.GJJONG, a.GWRTNO        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                     ");
            parameter.AppendSql(" WHERE a.PTNO    = :PTNO                           ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                           ");
            parameter.AppendSql("   AND a.JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD') ");
            parameter.AppendSql(" ORDER BY a.GJJONG                                 ");

            parameter.Add("PTNO", argPtno, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", argDate);

            return ExecuteReader<HIC_JEPSU_HEA_EXJONG>(parameter);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetListWrtnoByHicYear(string argPtno, string argDate, string argYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO AS WRTNO, a.GJJONG, a.GWRTNO        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                     ");
            parameter.AppendSql(" WHERE a.PTNO    = :PTNO                           ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                           ");
            parameter.AppendSql("   AND a.JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND a.GJYEAR = :YEAR                            ");
            parameter.AppendSql(" ORDER BY a.GJJONG                                 ");

            parameter.Add("PTNO", argPtno, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", argDate);
            parameter.Add("YEAR", argYear, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU_HEA_EXJONG>(parameter);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetListWrtnoByHicHea_Old(string argPtno, string argDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT x.WRTNO, x.GJJONG, x.JEPGBN                   ");
            parameter.AppendSql("  FROM (                                             ");
            parameter.AppendSql("SELECT a.WRTNO AS WRTNO, a.GJJONG, '1' AS JEPGBN     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                       ");
            parameter.AppendSql(" WHERE a.PTNO    = :PTNO                             ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                             ");
            parameter.AppendSql("   AND a.JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql(" UNION ALL                                           ");
            parameter.AppendSql("SELECT b.WRTNO AS WRTNO, b.GJJONG, '2' AS JEPGBN     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU b                       ");
            parameter.AppendSql(" WHERE b.PTNO    = :PTNO                             ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                             ");
            parameter.AppendSql("   AND b.SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql(" ) x                                                 ");
            parameter.AppendSql(" ORDER BY x.JEPGBN, x.GJJONG                         ");

            parameter.Add("PTNO", argPtno, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", argDate);
            parameter.Add("SDATE", argDate);

            return ExecuteReader<HIC_JEPSU_HEA_EXJONG>(parameter);
        }

        public IList<HIC_JEPSU_HEA_EXJONG> ValidJepsu(string argPtno, string argDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO AS WRTNO                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                       ");
            parameter.AppendSql(" WHERE a.PTNO    = :PTNO                             ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                             ");
            parameter.AppendSql("   AND a.JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql(" UNION ALL                                           ");
            parameter.AppendSql("SELECT b.WRTNO AS WRTNO                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU b                       ");
            parameter.AppendSql(" WHERE b.PTNO    = :PTNO                             ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                             ");
            parameter.AppendSql("   AND b.SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')       ");

            parameter.Add("PTNO", argPtno, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", argDate);
            parameter.Add("SDATE", argDate);

            return ExecuteReader<HIC_JEPSU_HEA_EXJONG>(parameter);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetWrtnobyGubun_All(string argJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT A.WRTNO, A.SNAME, A.GJJONG, B.NAME, A.PANO, A.PTNO, A.LTDCODE, '' ACTMEMO ");
            parameter.AppendSql("   FROM HIC_JEPSU  A                                                       ");
            parameter.AppendSql("      , HIC_EXJONG B                                                       ");
            parameter.AppendSql("  WHERE A.JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                        ");
            parameter.AppendSql("    AND A.DELDATE IS  NULL                                                 ");
            parameter.AppendSql("    AND A.GbSTS = '1'                                                      ");
            parameter.AppendSql("    AND A.GJJONG = B.CODE(+)                                               ");
            parameter.AppendSql("  UNION ALL                                                                ");
            parameter.AppendSql(" SELECT A.WRTNO, A.SNAME, A.GJJONG, B.NAME, A.PANO, A.PTNO, A.LTDCODE, A.ACTMEMO ");
            parameter.AppendSql("   FROM HEA_JEPSU  A                                                       ");
            parameter.AppendSql("      , HEA_EXJONG B                                                       ");
            parameter.AppendSql("  WHERE A.SDATE = TRUNC(SYSDATE)                                           ");
            parameter.AppendSql("    AND A.DELDATE IS  NULL                                                 ");
            parameter.AppendSql("    AND A.GJJONG = B.CODE(+)                                               ");
            parameter.AppendSql("  ORDER By WRTNO                                                           ");

            parameter.Add("JEPDATE", argJepDate);

            return ExecuteReader<HIC_JEPSU_HEA_EXJONG>(parameter);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetWrtnobyGubun_All_HIC(string argJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT A.WRTNO, A.SNAME, A.GJJONG, B.NAME, A.PANO, A.PTNO, A.LTDCODE, '' ACTMEMO ");
            parameter.AppendSql("   FROM HIC_JEPSU  A                                                       ");
            parameter.AppendSql("      , HIC_EXJONG B                                                       ");
            parameter.AppendSql("  WHERE A.JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                        ");
            parameter.AppendSql("    AND A.DELDATE IS  NULL                                                 ");
            parameter.AppendSql("    AND A.GbSTS = '1'                                                      ");
            parameter.AppendSql("    AND A.GJJONG = B.CODE(+)                                               ");
            parameter.AppendSql("  ORDER By WRTNO                                                           ");

            parameter.Add("JEPDATE", argJepDate);

            return ExecuteReader<HIC_JEPSU_HEA_EXJONG>(parameter);
        }

        public List<HIC_JEPSU_HEA_EXJONG> GetWrtnobyGubun_All_HEA(string argJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT A.WRTNO, A.SNAME, A.GJJONG, B.NAME, A.PANO, A.PTNO, A.LTDCODE, A.ACTMEMO ");
            parameter.AppendSql("   FROM HEA_JEPSU  A                                                       ");
            parameter.AppendSql("      , HEA_EXJONG B                                                       ");
            parameter.AppendSql("  WHERE A.SDATE = TRUNC(SYSDATE)                                           ");
            parameter.AppendSql("    AND A.DELDATE IS  NULL                                                 ");
            parameter.AppendSql("    AND A.GJJONG = B.CODE(+)                                               ");
            parameter.AppendSql("  ORDER By WRTNO                                                           ");

            parameter.Add("JEPDATE", argJepDate);

            return ExecuteReader<HIC_JEPSU_HEA_EXJONG>(parameter);
        }
    }
}
