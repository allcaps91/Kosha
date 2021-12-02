namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuPatientJinGbdRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuPatientJinGbdRepository()
        {
        }

        public HIC_JEPSU_PATIENT_JIN_GBD GetItembyWrtNoGjJong(long nWRTNO, string strGjJong, string strGbPrint)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(D.ENTDATE,'YYYY-MM-DD') ENTDATE,ENTSABUN, B.PRTSABUN, B.TONGBODATE      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU b , KOSMOS_PMPA.HIC_PATIENT c, KOSMOS_PMPA.HIC_JIN_GBN d  ");
            parameter.AppendSql(" WHERE b.WRTNO=d.WRTNO                                                                 ");
            parameter.AppendSql("   AND B.WRTNO = :WRTNO                                                                ");
            parameter.AppendSql("   AND b.Pano = c.Pano(+)                                                              ");
            parameter.AppendSql("   AND b.GJJONG = :GJJONG                                                              "); //진단서
            parameter.AppendSql("   AND d.GBPRINT = :GBPRINT                                                            "); //인쇄완료
            parameter.AppendSql(" ORDER BY b.LtdCode,b.SName                                                            ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("GJJONG", strGjJong);
            parameter.Add("GBPRINT", strGbPrint);

            return ExecuteReaderSingle<HIC_JEPSU_PATIENT_JIN_GBD>(parameter);
        }

        public List<HIC_JEPSU_PATIENT_JIN_GBD> GetItembyJepDate(string strFDate, string strTDate, long nLtdCode, string strSName, string strGbRe)
        {
            MParameter parameter = CreateParameter();

           
            parameter.AppendSql(" SELECT C.PANO, C.SNAME, C.Juso1||'  '||c.Juso2 AS JUSO                                            ");
            parameter.AppendSql(" ,substr(c.Jumin,1,6)||'-'||substr(c.Jumin,7,7) AS JUMIN, C.SABUN , B.GJJONG                       ");
            parameter.AppendSql(" ,B.LtdCode, TO_CHAR(b.JepDate,'YYYY-MM-DD') JEPDATE , B.AGE, B.SEX                                ");
            parameter.AppendSql(" ,B.WRTNO, B.UCODES, A.GUBUN, A.PanjengDrNo                                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JIN_GBN A                                                                   ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_JEPSU B                                                                     ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT C                                                                   ");
            parameter.AppendSql(" WHERE 1=1                                                                                         ");
            parameter.AppendSql(" AND B.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                   ");
            parameter.AppendSql(" AND B.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                   ");
            parameter.AppendSql(" AND A.WRTNO = B.WRTNO(+)                                                                          ");
            parameter.AppendSql(" AND B.PANO = C.PANO(+)                                                                            ");
            parameter.AppendSql(" AND B.GJJONG IN ('32')                                                                            "); //69종
            parameter.AppendSql(" AND B.DelDate IS NULL                                                                             ");

            if (strGbRe == "Y")
            {
                parameter.AppendSql(" AND A.GBPRINT = 'Y'                                                                           ");
            }
            else
            {
                parameter.AppendSql(" AND (A.GBPRINT IS NULL OR A.GBPRINT <> 'Y')                                                   ");
            }

            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND B.LTDCODE = :LTDCODE                                                                    ");
            }
            if (strSName != "")
            {
                parameter.AppendSql("   AND B.SNAME LIKE :SNAME                                                                     ");
            }

            parameter.AppendSql(" ORDER BY B.LTDCODE, B.SNAME                                                                       ");


            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            if (strSName != "")
            {
                parameter.Add("SNAME", strSName);
            }

            return ExecuteReader<HIC_JEPSU_PATIENT_JIN_GBD>(parameter);
        }
    }
}
