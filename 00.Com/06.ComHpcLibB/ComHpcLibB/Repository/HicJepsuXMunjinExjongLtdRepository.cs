namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuXMunjinExjongLtdRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuXMunjinExjongLtdRepository()
        {
        }

        public List<HIC_JEPSU_X_MUNJIN_EXJONG_LTD> GetItembyJepDateLtdCodeSName(string strFrDate, string strToDate, long nLtdCode, string strSName, long gnHicLicense, string strJob, string strAll, string strAllDoctor, string strSort)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,a.SName,TO_CHAR(a.JepDate,'YY-MM-DD') JepDate,a.GjJong,a.ltdcode,b.Panjeng, b.PanjengDrno       ");
            parameter.AppendSql("     , a.UCodes,a.Sex,a.Age,c.Name ExName,d.Name LtdName, b.MunDrno                                            ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_X_MUNJIN b, ADMIN.HIC_EXJONG C, ADMIN.HIC_LTD D    ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                             ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                             ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                                       ");
            parameter.AppendSql("   AND a.GjJong IN ('51')                                                                                      ");
            if (strAll == "Y")
            {
                parameter.AppendSql("   AND a.GbSTS > '1'                                                                                       ");//결과입력완료
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                                  ");
            }
            if (strSName != "")
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                                                 ");
            }
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                                    ");
            parameter.AppendSql("   AND a.WRTNO > 0                                                                                             ");
            if (strJob == "1")
            {
                parameter.AppendSql("   AND (b.Sts IS NULL OR b.Sts <> 'Y')                                                                     ");
                if (strAllDoctor == "Y")
                {
                    parameter.AppendSql("   AND b.MUNDRNO IS NOT NULL                                                                           ");
                }
                else if (gnHicLicense > 0)
                {
                    parameter.AppendSql("   AND b.MUNDRNO = :MUNDRNO                                                                            ");
                }
                else
                {
                    parameter.AppendSql("   AND b.MunDrNo IS NOT NULL                                                                           ");
                }
            }
            else
            {
                parameter.AppendSql("   AND b.Sts = 'Y'                                                                                         ");
            }
            parameter.AppendSql("   AND a.GjJong = c.Code(+)                                                                                    ");
            parameter.AppendSql("   AND a.LtdCode = d.Code(+)                                                                                   ");
            if (strSort == "Y")
            {
                parameter.AppendSql(" ORDER BY a.JepDate, a.GjJong                                                                              ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY  a.SName, a.Pano                                                                                 ");
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("MUNDRNO", gnHicLicense); 

            if (strSName != "")
            {
                parameter.Add("SNAME", strSName);
            }
            
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_X_MUNJIN_EXJONG_LTD>(parameter);
        }
    }
}
