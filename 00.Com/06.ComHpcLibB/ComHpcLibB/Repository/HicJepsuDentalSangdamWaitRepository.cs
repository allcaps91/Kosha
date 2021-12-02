namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuDentalSangdamWaitRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuDentalSangdamWaitRepository()
        {
        }

        public List<HIC_JEPSU_DENTAL_SANGDAM_WAIT> GetItembyJepDate(string strFrDate, string strToDate, string idNumber, string strSName, string strJong, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.SName,TO_CHAR(a.JepDate,'YY-MM-DD') JepDate,FC_HIC_GJJONG_NAME(a.GjJong, a.UCodes) GJJONG ,a.UCodes,a.Gbjinchal2    ");
            parameter.AppendSql("     , a.GjYear,b.PanjengDrno, b.PanjengDate, a.PTNO                                                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_DENTAL b, KOSMOS_PMPA.HIC_SANGDAM_WAIT c                               ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'yyyy-mm-dd')                                                                         ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'yyyy-mm-dd')                                                                         ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                                                ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO(+)                                                                                                ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                                                   ");
            parameter.AppendSql("   AND a.GBDENTAL = 'Y'                                                                                                    ");
            parameter.AppendSql("   AND (a.GBJINCHAL2 IS NULL OR a.GBJINCHAL2 = 'N')                                                                        ");
            parameter.AppendSql("   AND (b.PanjengDrno IS NULL OR b.PanjengDrno=0)                                                                          ");
            parameter.AppendSql("   AND a.GBCHUL = 'N'                                                                                                      ");
            parameter.AppendSql("   AND c.WRTNO > 0                                                                                                         ");
            switch (idNumber)
            {
                case "35712":
                    parameter.AppendSql("   AND c.Gubun = '08'                                                                  "); //김영배
                    break;
                case "37029":
                    parameter.AppendSql("   AND c.Gubun = '09'                                                                  "); //최영수
                    break;
                default:
                    break;
            }
            if (strSName != "")
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                                 ");
            }
            if (strJong != "**")
            {
                parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                                  ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                ");
            }
            parameter.AppendSql("   AND a.GjJong NOT IN ('31','35','56')                                                        ");  //암검진 제외
            parameter.AppendSql(" ORDER BY c.WaitNo,a.WRTNO,a.GjJong                                                            ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (strSName != "")
            {
                parameter.AddLikeStatement("SNAME", strSName);
            }
            if (strJong != "**")
            {
                parameter.Add("GJJONG", strJong, Oracle.DataAccess.Client.OracleDbType.Char);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_DENTAL_SANGDAM_WAIT>(parameter);
        }
    }
}
