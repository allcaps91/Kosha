namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuResEtcSangdamNewLtdRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResEtcSangdamNewLtdRepository()
        {
        }

        public List<HIC_JEPSU_RES_ETC_SANGDAM_NEW_LTD> GetItembyJepDate(string strFrDate, string strToDate, long nLtdCode, string strSName
                                                                      , string strJob, string strSort, long nLicense, int nPan)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.SName,TO_CHAR(a.JepDate,'YY-MM-DD') JepDate,a.GjJong,a.ltdcode            ");
            parameter.AppendSql("     , a.UCODES,a.Sex,a.Age,b.PanjengDrNo,d.Name LtdName,b.GbPanjeng                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU       a                                                       ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_RES_ETC     b                                                       ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_SANGDAM_NEW c                                                       ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_LTD         d                                                       ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                          ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')                                          ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                                   ");
            parameter.AppendSql("   AND a.GbSTS > '1'                                                                       ");
            parameter.AppendSql("   AND b.Gubun = '1'                                                                       ");
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                            ");
            }
            if (strSName != "")
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                             ");
            }
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                ");
            parameter.AppendSql("   AND b.WRTNO > 0                                                                         ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO(+)                                                                ");
            if (strJob == "1")
            {
                parameter.AppendSql("   AND (b.GbPanjeng IS NULL OR b.GbPanjeng <> 'Y')                                     ");
            }
            else
            {
                parameter.AppendSql("   AND b.GbPanjeng = 'Y'                                                               ");
                //이름으로 검색 시 다른 과장님도 표시함
                if (strSName == "")
                {
                    if (nLicense > 0)
                    {
                        if (nPan > 0)
                        {
                            parameter.AppendSql("   AND ( b.PANJENGDRNO = :PANJENGDRNO OR b.PANJENGDRNO = :PAN )            ");
                        }
                        else
                        {
                            parameter.AppendSql("   AND  b.PANJENGDRNO = :PANJENGDRNO                                       ");
                        }
                    }
                    else if (nPan > 0)
                    {
                        parameter.AppendSql("   AND b.PANJENGDRNO = :PAN                                                    ");
                    }
                }
            }
            parameter.AppendSql("   AND a.LtdCode = d.Code(+)                                                               ");
            if (strSort == "1")
            {
                parameter.AppendSql(" ORDER BY a.Sname,a.JepDate,a.ltdcode                                                  ");
            }
            else if (strSort == "2")
            {
                parameter.AppendSql(" ORDER BY a.JepDate,a.ltdcode,a.Sname                                                  ");
            }
            else if (strSort == "3")
            {
                parameter.AppendSql(" ORDER BY a.ltdcode,a.SName,a.JepDate                                                  ");
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

            if (nLicense != 0)
            {
                parameter.Add("PANJENGDRNO", nLicense);
            }

            if (nPan != 0)
            {
                parameter.Add("PAN", nPan);
            }

            return ExecuteReader<HIC_JEPSU_RES_ETC_SANGDAM_NEW_LTD>(parameter);
        }
    }
}
