namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuResEtcExjongLtdRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResEtcExjongLtdRepository()
        {
        }

        public List<HIC_JEPSU_RES_ETC_EXJONG_LTD> GetItembyJepDate(string strFrDate, string strToDate, long nLtdCode, string strSName, string strJob, string strSort)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.SName,TO_CHAR(a.JepDate,'YY-MM-DD') JepDate,a.GjJong,a.ltdcode                    ");
            parameter.AppendSql("     , a.UCodes,a.SExams,a.Sex,a.Age,b.PanjengDrNo,c.Name ExName,d.Name LtdName,b.GbPanjeng        ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU   a                                                                   ");
            parameter.AppendSql("     , ADMIN.HIC_RES_ETC b                                                                   ");
            parameter.AppendSql("     , ADMIN.HIC_EXJONG  c                                                                   ");
            parameter.AppendSql("     , ADMIN.HIC_LTD     d                                                                   ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                 ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                 ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                           ");
            parameter.AppendSql("   AND a.GjJong IN ('69')                                                                          "); //69종
            parameter.AppendSql("   AND a.GbSTS > '1'                                                                               "); //결과입력완료
            parameter.AppendSql("   AND b.Gubun = '3'                                                                               ");
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                      ");
            }
            if (strSName != "")
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                                     ");
            }
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO                                                                           ");
            parameter.AppendSql("   AND b.WRTNO > 0                                                                                 ");
            if (strJob == "1")
            {
                parameter.AppendSql("   AND (b.GbPanjeng IS NULL OR b.GbPanjeng <> 'Y')                                             ");
            }
            else
            {
                parameter.AppendSql("   AND b.GbPanjeng = 'Y'                                                                       ");
            }
            parameter.AppendSql("   AND a.GjJong  = c.Code(+)                                                                       ");
            parameter.AppendSql("   AND a.LtdCode = d.Code(+)                                                                       ");
            if (strSort == "1")
            {
                parameter.AppendSql(" ORDER BY a.JepDate, a.GjJong                                                                  ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY a.SName, a.GjJong                                                                    ");
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            if (strSName != "")
            {
                parameter.Add("SNAME", strSName);
            }

            return ExecuteReader<HIC_JEPSU_RES_ETC_EXJONG_LTD>(parameter);
        }
    }
}
