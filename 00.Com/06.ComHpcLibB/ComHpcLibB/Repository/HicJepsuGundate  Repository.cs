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
    public class HicJepsuGundateRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuGundateRepository()
        {
        }

        public List<HIC_JEPSU_GUNDATE> GetItembyGjYear(string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.LtdCode                                                           ");
            parameter.AppendSql("     , MIN(TO_CHAR(b.Gundate,'YYYY-MM-DD')) MinDate1                       ");
            parameter.AppendSql("     , MAX(TO_CHAR(b.Gundate,'YYYY-MM-DD')) MaxDate1                       ");
            parameter.AppendSql("     , MIN(TO_CHAR(a.Jepdate,'YYYY-MM-DD')) MinDate                        ");
            parameter.AppendSql("     , MAX(TO_CHAR(a.Jepdate,'YYYY-MM-DD')) MaxDate                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_GUNDATE b                  ");
            parameter.AppendSql(" WHERE a.Wrtno = b.Wrtno                                                   ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                                  ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                   ");
            parameter.AppendSql("   AND a.GjJong IN ('11','12','14','41','42')                              "); //사업장1차,공무원1차,사업장회사부담1차
            parameter.AppendSql("   AND a.GjChasu = '1'                                                     ");
            parameter.AppendSql("   AND a.GbInwon in ('21','22','23','31','32','64','65','66','67','68')    ");
            parameter.AppendSql("   AND a.LtdCode IS NOT NULL                                               ");
            parameter.AppendSql("   AND b.GunDate IS NOT NULL                                               ");
            parameter.AppendSql(" GROUP BY a.LtdCode                                                        ");

            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU_GUNDATE>(parameter);
        }

        public List<HIC_JEPSU_GUNDATE> GetItembyJepDateGjYearBogunso(string strFrDate, string strToDate, string strYear, long nLtdCode, long nWrtNo, string strJep, string strDel, string strJong, string strJong1, string strJohap, string strBogunso, string strSort)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,TO_CHAR(b.GunDate,'YYYY-MM-DD') GunDate,a.MirSayu   ");
            parameter.AppendSql("     , a.Wrtno, a.SName,a.Age, a.GjJong, a.GjChasu, a.LtdCode,a.BOGUNSO                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_GUNDATE b                                          ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO(+)                                                                        ");
            parameter.AppendSql("   AND a.JepDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                                  ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                                                  ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                                                          ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                                           ");
            if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                    ");
            }
            if (!nWrtNo.IsNullOrEmpty() && nWrtNo != 0)
            {
                parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                                        ");
            }
            if (!strJong1.IsNullOrEmpty() && strJong1 != "**")
            {
                parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                                      ");
            }
            if (strJep == "Y")
            {
                parameter.AppendSql("   AND b.GunDate IS NOT NULL                                                                   ");
            }
            if (strDel == "1")      //미제외
            {
                if (strJong == "1")
                {
                    parameter.AppendSql("   AND a.GjJong IN ('11','16','12','17','13','18','41','42','43','44','45','46')           ");
                    parameter.AppendSql("   AND (a.Mirno1 ='0' OR a.Mirno1 IS NULL)                                                 "); //사업장
                }
                else if (strJong == "3")
                {
                    parameter.AppendSql("   AND a.GjJong IN ('11','16','12','17','13','18','41','42','43','44','45','46')           ");
                    parameter.AppendSql("   AND a.GbDental = 'Y'                                                                    ");
                    parameter.AppendSql("   AND (a.Mirno2 ='0' OR a.Mirno2 IS NULL)                                                 "); //구강검진
                }
                else if (strJong == "4")
                {
                    parameter.AppendSql("   AND a.GjJong IN ('31','35')                                                             ");
                    parameter.AppendSql("   AND (a.Mirno3 ='0' OR a.Mirno3 IS NULL)                                                 "); //공단암
                    parameter.AppendSql("   AND (a.Mirno5 IS NULL OR a.Mirno5 = '0' )                                               ");
                    parameter.AppendSql("   AND (a.Gubdaesang <> 'Y' OR a.Gubdaesang IS NULL)                                       ");
                }
                else if (strJong == "E")
                {
                    parameter.AppendSql("   AND a.GjJong IN ('31','35')                                                             ");
                    parameter.AppendSql("   AND (a.Mirno5 ='0' OR a.Mirno5 IS NULL)                                                 "); //의료급여
                    parameter.AppendSql("   AND (a.Mirno3 IS NULL OR a.Mirno3 ='0' )                                                ");
                    parameter.AppendSql("   AND a.Gubdaesang = 'Y'                                                                  ");
                }
            }
            else if (strDel == "0") //제외
            {
                if (strJong == "1")
                {
                    parameter.AppendSql("   AND a.Mirno1 = '1'                                                                      "); //사업장
                }
                else if (strJong == "3")
                {
                    parameter.AppendSql("   AND a.Mirno2 = '2'                                                                      "); //구강검진
                }
                else if (strJong == "4")
                {
                    parameter.AppendSql("   AND a.Mirno3 = '3'                                                                      "); //공단암
                    parameter.AppendSql("   AND (a.Gubdaesang <> 'Y' OR a.Gubdaesang IS NULL)                                       ");
                }
                else if (strJong == "E")
                {
                    parameter.AppendSql("   AND a.Mirno5 = '5'                                                                      "); //의료급여
                    parameter.AppendSql("   AND a.Gubdaesang = 'Y'                                                                  ");
                }
            }
            if (!strJohap.IsNullOrEmpty())
            {
                if (strJohap == "급여")
                {
                    parameter.AppendSql("   AND SUBSTR(a.Gkiho,1,1) IN ('9')                                                        ");
                }
                else if (strJohap == "직장")
                {
                    parameter.AppendSql("   AND SUBSTR(a.Gkiho,1,1) IN ('7','8')                                                    ");
                }
                else if (strJohap == "공교")
                {
                    parameter.AppendSql("   AND SUBSTR(a.Gkiho,1,1) IN ('5','6')                                                    ");
                }
                else if (strJohap == "지역")
                {
                    parameter.AppendSql("   AND SUBSTR(a.Gkiho,1,1) IN ('1','2','3')                                                ");
                }
            }
            if (!strBogunso.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.BOGUNSO = :BOGUNSO                                                                    ");
            }
            if (strSort == "0")
            {
                parameter.AppendSql(" ORDER BY JepDate,SName                                                                        ");
            }
            else if (strSort == "1")
            {
                parameter.AppendSql(" ORDER BY SName,JepDate                                                                        ");
            }
            else if (strSort == "2")
            {
                parameter.AppendSql(" ORDER BY Wrtno,SName,JepDate                                                                  ");
            }
            
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            if (!nWrtNo.IsNullOrEmpty() && nWrtNo != 0)
            {
                parameter.Add("WRTNO", nWrtNo);
            }
            if (!strJong1.IsNullOrEmpty() && strJong1 != "**")
            {
                parameter.Add("GJJONG", strJong1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (!strBogunso.IsNullOrEmpty())
            {
                parameter.Add("BOGUNSO", strBogunso, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_JEPSU_GUNDATE>(parameter);
        }

        public List<HIC_JEPSU_GUNDATE> GetItembyInLineWrtNo(string fstrFDate, string fstrTDate, string strJob, string fstrGjBangi, string strGjYear, string fstrLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(b.GunDate,'YYYY-MM-DD') GunDate, TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   a                                                                           ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_GUNDATE b                                                                           ");
            parameter.AppendSql(" WHERE a.wrtno in (                                                                                        ");
            parameter.AppendSql("                   SELECT a.wrtno as wrtno                                                                 ");
            parameter.AppendSql("                     FROM KOSMOS_PMPA.HIC_JEPSU       a                                                    ");
            parameter.AppendSql("                        , KOSMOS_PMPA.HIC_RES_SPECIAL b                                                    ");
            parameter.AppendSql("                        , KOSMOS_PMPA.HIC_LTD         c                                                    ");
            parameter.AppendSql("                    WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                      ");
            parameter.AppendSql("                      AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                      ");
            parameter.AppendSql("                      AND a.DelDate IS NULL                                                                ");
            parameter.AppendSql("                      AND a.LtdCode IS NOT NULL                                                            ");
            parameter.AppendSql("                      AND a.UCodes IS NOT NULL                                                             ");
            parameter.AppendSql("                      AND a.GJYEAR = :GJYEAR                                                               ");
            if (strJob == "0")//특수
            {
                parameter.AppendSql("                      AND a.Gjjong IN ('11','12','14','23','41','42')                                  ");
            }
            else if (strJob == "1") //채용
            {
                parameter.AppendSql("                      AND a.Gjjong IN ('22','24','30')                                                 ");
            }
            else if (strJob == "2") //수시
            {
                parameter.AppendSql("                      AND a.Gjjong IN ('25')                                                           ");
            }
            else if (strJob == "3") //임시
            {
                parameter.AppendSql("                      AND a.Gjjong  IN ('26')                                                          ");
            }
            parameter.AppendSql("                     AND a.WRTNO=b.WRTNO(+)                                                                ");
            parameter.AppendSql("                     AND b.PanjengDrno IS NOT NULL                                                         ");
            parameter.AppendSql("                     AND a.LtdCode=c.Code(+)                                                               ");
            if (fstrLtdCode != "")
            {
                parameter.AppendSql("                     AND a.LTDCODE = :LTDCODE                                                          ");
            }
            parameter.AppendSql("                   Union All                                                                               ");
            parameter.AppendSql("                  select a.wrtno                                                                           ");
            parameter.AppendSql("                    from KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_SPECIAL b, KOSMOS_PMPA.HIC_LTD c     ");
            parameter.AppendSql("                   where a.pano in(                                                                        ");
            parameter.AppendSql("                                   SELECT a.pano  as pano                                                  ");
            parameter.AppendSql("                                     FROM KOSMOS_PMPA.HIC_JEPSU a                                          ");
            parameter.AppendSql("                                        , KOSMOS_PMPA.HIC_RES_SPECIAL b, KOSMOS_PMPA.HIC_LTD c             ");
            parameter.AppendSql("                                    WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("                                      AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("                                      AND a.DelDate IS NULL                                                ");
            parameter.AppendSql("                                      AND a.LtdCode IS NOT NULL                                            ");
            parameter.AppendSql("                                      AND a.UCodes IS NOT NULL                                             ");
            parameter.AppendSql("                                      AND a.GJYEAR = :GJYEAR                                               ");
            if (strJob == "0") //특수                               
            {                                                      
                parameter.AppendSql("                                  AND a.Gjjong IN ('11','12','14','23','41','42')                      ");
            }                                                      
            else if (strJob == "1") //배치전                       
            {                                                      
                parameter.AppendSql("                                  AND a.Gjjong IN ('22','24','30')                                     ");
            }                                                      
            else if (strJob == "2") //수시                         
            {                                                      
                parameter.AppendSql("                                  AND a.Gjjong IN ('25')                                               ");
            }                                                      
            else if (strJob == "3") //임시                         
            {                                                      
                parameter.AppendSql("                                  AND a.Gjjong  IN ('26')                                              ");
            }                                                      
            parameter.AppendSql("                                      AND a.WRTNO=b.WRTNO(+)                                               ");
            parameter.AppendSql("                                      AND b.PANJENGDRNO IS NOT NULL                                        ");
            parameter.AppendSql("                                      AND a.LTDCODE = c.Code(+)                                            ");
            if (fstrLtdCode != "")
            {
                parameter.AppendSql("                                      AND a.LTDCODE = :LTDCODE                                         ");
            }
            parameter.AppendSql("                                 )                                                                         ");              
            parameter.AppendSql("                    AND a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                        ");
            parameter.AppendSql("                    AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                        ");
            parameter.AppendSql("                    AND a.DELDATE IS NULL                                                                  ");
            parameter.AppendSql("                    AND a.LTDCODE IS NOT NULL                                                              ");
            parameter.AppendSql("                    AND a.GJYEAR = :GJYEAR                                                                 ");
            if (strJob == "0") //특수
            {
                parameter.AppendSql("                    AND a.Gjjong IN ('11','12','14','23','41','42')                                    ");
            }
            else if (strJob == "1") //배치전
            {
                parameter.AppendSql("                    AND a.Gjjong IN ('22','24','30')                                                   ");
            }
            else if (strJob == "2") //수시
            {
                parameter.AppendSql("                    AND a.Gjjong IN ('25')                                                             ");
            }
            else if (strJob == "3") //임시
            {
                parameter.AppendSql("                    AND a.Gjjong  IN ('26')                                                            ");
            }
            parameter.AppendSql("                    AND a.WRTNO=b.WRTNO(+)                                                                 ");
            parameter.AppendSql("                    AND b.PANJENGDRNO IS NOT NULL                                                          ");
            parameter.AppendSql("                    AND a.LTDCODE = c.Code(+)                                                              ");
            if (fstrLtdCode != "")
            {
                parameter.AppendSql("                    AND a.LTDCODE = :LTDCODE                                                           ");
            }
            parameter.AppendSql("                  )                                                                                        ");
            parameter.AppendSql("   AND A.WRTNO = B.WRTNO(+)                                                                                ");

            parameter.Add("FRDATE", fstrFDate);
            parameter.Add("TODATE", fstrTDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (fstrLtdCode != "")
            {
                parameter.Add("LTDCODE", fstrLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_GUNDATE>(parameter);
        }

        public HIC_JEPSU_GUNDATE GetItembyWrtNo(string fstrFDate, string fstrTDate, string strGjYear, string fstrLtdCode, string strJob)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MIN(TO_CHAR(a.JepDate,'YYYY-MM-DD')) MinDate,MAX(TO_CHAR(a.JepDate,'YYYY-MM-DD')) MaxDate       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   a                                                                       ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_GUNDATE b                                                                       ");
            parameter.AppendSql(" WHERE a.wrtno in (                                                                                    ");
            parameter.AppendSql("                   SELECT a.wrtno as wrtno                                                             ");
            parameter.AppendSql("                     FROM KOSMOS_PMPA.HIC_JEPSU       a                                                ");
            parameter.AppendSql("                        , KOSMOS_PMPA.HIC_RES_SPECIAL b                                                ");
            parameter.AppendSql("                        , KOSMOS_PMPA.HIC_LTD         c                                                ");
            parameter.AppendSql("                    WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                  ");
            parameter.AppendSql("                      AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                  ");
            parameter.AppendSql("                      AND a.DelDate IS NULL                                                            ");
            parameter.AppendSql("                      AND a.LtdCode IS NOT NULL                                                        ");
            parameter.AppendSql("                      AND a.UCodes IS NOT NULL                                                         ");
            parameter.AppendSql("                      AND a.GJYEAR = :GJYEAR                                                           ");
            if (strJob == "1")//특수
            {
                parameter.AppendSql("                      AND a.Gjjong IN ('11','12','14','23','41')                                   ");
            }
            else if (strJob == "2") //채용
            {
                parameter.AppendSql("                      AND a.Gjjong IN ('22','24','30')                                             ");
            }
            else
            {
                parameter.AppendSql("                      AND a.Gjjong  IN ('69')                                                      ");
            }


            parameter.AppendSql("                     AND a.WRTNO=b.WRTNO(+)                                                            ");
            parameter.AppendSql("                     AND b.PanjengDrno IS NOT NULL                                                     ");
            parameter.AppendSql("                     AND a.LtdCode=c.Code(+)                                                           ");
            parameter.AppendSql("                     AND a.LTDCODE = :LTDCODE                                                          ");
            parameter.AppendSql("                   UNION ALL                                                                           ");
            parameter.AppendSql("                  SELECT A.WRTNO                                                                       ");
            parameter.AppendSql("                    FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_SPECIAL b, KOSMOS_PMPA.HIC_LTD c ");
            parameter.AppendSql("                   WHERE A.PANO IN(                                                                    ");
            parameter.AppendSql("                                   SELECT a.PANO  AS PANO                                              ");
            parameter.AppendSql("                                     FROM KOSMOS_PMPA.HIC_JEPSU a                                      ");
            parameter.AppendSql("                                        , KOSMOS_PMPA.HIC_RES_SPECIAL b, KOSMOS_PMPA.HIC_LTD c         ");
            parameter.AppendSql("                                    WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                  ");
            parameter.AppendSql("                                      AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                  ");
            parameter.AppendSql("                                      AND a.DelDate IS NULL                                            ");
            parameter.AppendSql("                                      AND a.LtdCode IS NOT NULL                                        ");
            parameter.AppendSql("                                      AND a.UCodes IS NOT NULL                                         ");
            parameter.AppendSql("                                      AND a.GJYEAR = :GJYEAR                                           ");
            if (strJob == "1") //특수
            {
                parameter.AppendSql("                                      AND a.Gjjong IN ('11','12','14','23','41')                   ");
            }
            else if (strJob == "2") //채용
            {
                parameter.AppendSql("                                      AND a.Gjjong IN ('22','24','30')                             ");
            }
            else
            {
                parameter.AppendSql("                                      AND a.Gjjong  IN ('69')                                      ");
            }
            parameter.AppendSql("                                      AND a.WRTNO=b.WRTNO(+)                                           ");
            parameter.AppendSql("                                      AND b.PANJENGDRNO IS NOT NULL                                    ");
            parameter.AppendSql("                                      AND a.LTDCODE = c.Code(+)                                        ");
            parameter.AppendSql("                                      AND a.LTDCODE = :LTDCODE                                         ");
            parameter.AppendSql("                                   )                                                                   ");
            parameter.AppendSql("                    AND a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                    ");
            parameter.AppendSql("                    AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                    ");
            parameter.AppendSql("                    AND a.DELDATE IS NULL                                                              ");
            parameter.AppendSql("                    AND a.LTDCODE IS NOT NULL                                                          ");
            parameter.AppendSql("                    AND a.GJYEAR = :GJYEAR                                                             ");

            if (strJob == "1") //특수
            {
                parameter.AppendSql("                    AND a.Gjjong IN ('16','17','19','28','44')                                     ");
            }
            else if (strJob == "2") //채용
            {
                parameter.AppendSql("                    AND a.Gjjong IN ('29')                                                         ");
            }
            else
            {
                parameter.AppendSql("                    AND a.Gjjong  IN ('69')                                                        ");
            }


            parameter.AppendSql("                    AND a.WRTNO=b.WRTNO(+)                                                             ");
            parameter.AppendSql("                    AND b.PanjengDrno IS NOT NULL                                                      ");
            parameter.AppendSql("                    AND a.LtdCode = c.Code(+)                                                          ");
            if (fstrLtdCode != "")
            {
                parameter.AppendSql("                    AND a.LTDCODE = :LTDCODE                                                       ");
            }
            parameter.AppendSql("                  )                                                                                    ");

            parameter.Add("FRDATE", fstrFDate);
            parameter.Add("TODATE", fstrTDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (fstrLtdCode != "")
            {
                parameter.Add("LTDCODE", fstrLtdCode);
            }

            return ExecuteReaderSingle<HIC_JEPSU_GUNDATE>(parameter);
        }

        public List<HIC_JEPSU_GUNDATE> GetItembyJepDateGjYear(string fstrFDate, string fstrTDate, string strGjYear, string fstrLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("select TO_CHAR(b.GunDate,'YYYY-MM-DD') GunDate, TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate    ");
            parameter.AppendSql("  from KOSMOS_PMPA.hic_jepsu   a                                                           ");
            parameter.AppendSql("     , KOSMOS_PMPA.hic_gundate b                                                           ");
            parameter.AppendSql(" where a.wrtno in (                                                                        ");
            parameter.AppendSql("                   SELECT a.WRTNO as WRTNO                                                 ");
            parameter.AppendSql("                     FROM KOSMOS_PMPA.HIC_JEPSU      a                                     ");
            parameter.AppendSql("                        , KOSMOS_PMPA.HIC_RES_BOHUM1 b                                     ");
            parameter.AppendSql("                        , KOSMOS_PMPA.HIC_LTD        c                                     ");
            parameter.AppendSql("                    WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("                      AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("                      AND a.DelDate IS NULL                                                ");
            parameter.AppendSql("                      AND a.LTDCODE IS NOT NULL                                            ");
            parameter.AppendSql("                      AND a.GbInwon in ('21','22','23','31','32','64','65','66','67','68') ");
            parameter.AppendSql("                      AND a.GJYEAR = :GJYEAR                                               ");
            parameter.AppendSql("                      AND a.Gjjong IN ('11','12','14','41','42')                           ");
            parameter.AppendSql("                      AND a.WRTNO   = b.WRTNO(+)                                           ");
            parameter.AppendSql("                      AND a.LTDCODE = c.Code(+)                                            ");
            parameter.AppendSql("                      AND a.LTDCODE = :LTDCODE                                             ");
            parameter.AppendSql("                    UNION ALL                                                              ");
            parameter.AppendSql("                   SELECT a.wrtno                                                          ");
            parameter.AppendSql("                     from KOSMOS_PMPA.HIC_JEPSU      a                                     ");
            parameter.AppendSql("                        , KOSMOS_PMPA.HIC_RES_BOHUM1 b                                     ");
            parameter.AppendSql("                        , KOSMOS_PMPA.HIC_LTD        c                                     ");
            parameter.AppendSql("                    WHERE a.pano in (                                                      ");
            parameter.AppendSql("                   SELECT a.PANO as PANO                                                   ");
            parameter.AppendSql("                     FROM KOSMOS_PMPA.HIC_JEPSU      a                                     ");
            parameter.AppendSql("                        , KOSMOS_PMPA.HIC_RES_BOHUM1 b                                     ");
            parameter.AppendSql("                        , KOSMOS_PMPA.HIC_LTD        c                                     ");
            parameter.AppendSql("                    WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("                      AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("                      AND a.DelDate IS NULL   AND a.LtdCode IS NOT NULL                    ");
            parameter.AppendSql("                      AND a.GJYEAR = :GJYEAR                                               ");
            parameter.AppendSql("                      AND a.Gjjong IN ('11','12','14','41','42')                           ");
            parameter.AppendSql("                      AND a.WRTNO   = b.WRTNO(+)                                           ");
            parameter.AppendSql("                      AND a.LtdCode = c.Code(+)                                            ");
            parameter.AppendSql("                      AND a.LTDCODE = :LTDCODE )                                           ");
            parameter.AppendSql("                      AND a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("                      AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("                      AND a.DelDate IS NULL                                                ");
            parameter.AppendSql("                      AND a.LtdCode IS NOT NULL                                            ");
            parameter.AppendSql("                      AND a.GJYEAR = :GJYEAR                                               ");
            parameter.AppendSql("                      AND a.Gjjong IN ('16','17','18','19','44','45')                      ");
            parameter.AppendSql("                      AND a.WRTNO   = b.WRTNO(+)                                           ");
            parameter.AppendSql("                      AND a.LtdCode = c.Code(+)                                            ");
            parameter.AppendSql("                      AND a.LTDCODE = :LTDCODE                                             ");
            parameter.AppendSql("                   )                                                                       ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                ");
            parameter.AppendSql(" ORDER BY a.JEPDATE                                                                        ");

            parameter.Add("FRDATE", fstrFDate);
            parameter.Add("TODATE", fstrTDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", fstrLtdCode);

            return ExecuteReader<HIC_JEPSU_GUNDATE>(parameter);
        }

        public HIC_JEPSU_GUNDATE GetGunDateByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT TO_CHAR(GunDate,'YYYY-MM-DD') GunDate              ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_GUNDATE                              ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                    ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReaderSingle<HIC_JEPSU_GUNDATE>(parameter);
        }
    }
}
