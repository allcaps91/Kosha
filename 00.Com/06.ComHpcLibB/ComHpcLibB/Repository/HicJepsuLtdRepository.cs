namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuLtdRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuLtdRepository()
        {
        }

        public List<HIC_JEPSU_LTD> GetItembyJepDateGjYearGjBangiLtdCode(string strFDate, string strTDate, string strYear, string strBangi, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.Name,a.LtdCode,COUNT(*) CNT                                       ");
            parameter.AppendSql("     , MIN(TO_CHAR(a.JepDate,'YYYY-MM-DD')) MinDate                        ");
            parameter.AppendSql("     , MAX(TO_CHAR(a.JepDate,'YYYY-MM-DD')) MaxDate                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_LTD b                      ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND a.LtdCode = b.Code(+)                                               ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                   ");
            parameter.AppendSql("   AND a.LtdCode IS NOT NULL                                               ");
            parameter.AppendSql("   AND a.GbInwon IN ('31','67')                                            ");
            parameter.AppendSql("   AND a.UCodes IS NULL                                                    ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                                  ");
            parameter.AppendSql("   AND a.GJBANGI = :GJBANGI                                                ");
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                            ");
            }
            parameter.AppendSql(" GROUP BY b.Name,a.LtdCode                                                 ");
            parameter.AppendSql(" ORDER BY b.Name,a.LtdCode                                                 ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJBANGI", strBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_LTD>(parameter);
        }

        public List<HIC_JEPSU_LTD> GetNamebyWrtNo(string fstrWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT B.NAME                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU A, KOSMOS_PMPA.HIC_LTD B  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");
            parameter.AppendSql("   AND A.LTDCODE = B.CODE                              ");

            parameter.Add("WRTNO", fstrWRTNO);

            return ExecuteReader<HIC_JEPSU_LTD>(parameter);
        }

        public List<HIC_JEPSU_LTD> GetItembyJepDateGjYearGjBangiLtdCode_New(string strFDate, string strTDate, string strYear, string strBangi, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.NAME, a.LTDCODE, COUNT(*) CNT                                     ");
            parameter.AppendSql("     , MIN(TO_CHAR(a.JEPDATE,'YYYY-MM-DD')) MINDATE                        ");
            parameter.AppendSql("     , MAX(TO_CHAR(a.JEPDATE,'YYYY-MM-DD')) MAXDATE                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_LTD b                      ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND a.LTDCODE = b.Code(+)                                               ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                   ");
            parameter.AppendSql("   AND a.LTDCODE IS NOT NULL                                               ");
            parameter.AppendSql("   AND a.GBINWON IN ('21','22','23','31','32','64','65','66','67','68')    ");
            if (strYear != "")
            {
                parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                              ");
            }
            if (strBangi != "")
            {
                parameter.AppendSql("   AND a.GJBANGI = :GJBANGI                                            ");
            }

            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                            ");
            }
            parameter.AppendSql(" GROUP BY b.Name,a.LtdCode                                                 ");
            parameter.AppendSql(" ORDER BY b.Name,a.LtdCode                                                 ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            if (strYear != "")
            {
                parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (strBangi != "")
            {
                parameter.Add("GJBANGI", strBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_LTD>(parameter);
        }



        public List<HIC_JEPSU_LTD> GetListByItems(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, JEPDATE, SNAME, (SEX || '/' || AGE) SEX, AGE, GJJONG, SANGHO                                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU A, KOSMOS_PMPA.HIC_LTD B                                                      ");
            parameter.AppendSql("  WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                        ");
            parameter.AppendSql("  AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                          ");
            parameter.AppendSql("  AND A.LTDCODE = B.CODE(+)                                                                                ");
            parameter.AppendSql("  ORDER BY WRTNO ASC                                                                                       ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);


            return ExecuteReader<HIC_JEPSU_LTD>(parameter);
        }
    }
}
