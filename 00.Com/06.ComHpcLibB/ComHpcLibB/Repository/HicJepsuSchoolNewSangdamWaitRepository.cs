namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuSchoolNewSangdamWaitRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuSchoolNewSangdamWaitRepository()
        {
        }

        public List<HIC_JEPSU_SCHOOL_NEW_SANGDAM_WAIT> GetItembyJepDate(string strFDate, string strTDate, string strSName, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,a.SName,TO_CHAR(a.JepDate,'YY-MM-DD') JepDate,a.GjJong,a.UCodes,a.SangDamDrno   ");
            parameter.AppendSql("     , b.DPANDRNO, b.DPANDATE                                                                  ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_SCHOOL_NEW b, ADMIN.HIC_SANGDAM_WAIT c   ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE,'yyyy-mm-dd')                                              ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE,'yyyy-mm-dd')                                              ");
            parameter.AppendSql("   AND b.WRTNO = a.WRTNO                                                                       ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO                                                                       ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                       ");
            parameter.AppendSql("   AND a.GjYear >='2009'                                                                       ");   //2009년 부터사용
            parameter.AppendSql("   AND c.WRTNO > 0                                                                             ");
            parameter.AppendSql("   AND (b.DPANDRNO IS NULL OR b.DPANDRNO = 0)                                                  ");
            if (strSName != "")
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                                 ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                ");
            }
            parameter.AppendSql(" ORDER BY c.WaitNo, a.WRTNO, a.GjJong                                                          ");


            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strFDate);
            if (strSName != "")
            {
                parameter.Add("SNAME", strSName);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_SCHOOL_NEW_SANGDAM_WAIT>(parameter);
        }
    }
}
