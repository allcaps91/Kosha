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
    public class HicJepsuResDentalRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResDentalRepository()
        {
        }

        public List<HIC_JEPSU_RES_DENTAL> GetItembyJepDate(string strFrDate, string strToDate, string strJob, string strChul, string strSName, long nLtdCode, string strJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,a.SName,TO_CHAR(a.JepDate,'YY-MM-DD') JepDate,FC_HIC_GJJONG_NAME(a.GjJong, a.UCodes) GJJONG, a.UCodes,a.Gbjinchal2    ");
            parameter.AppendSql("     , a.GjYear,b.PanjengDrno, b.PanjengDate, a.PTNO                                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_DENTAL b                                   ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'yyyy-mm-dd')                                             ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'yyyy-mm-dd')                                             ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                    ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                       ");
            parameter.AppendSql("   AND a.GBDENTAL = 'Y'                                                                        ");
            if (strJob == "1")       //대기자
            {
                parameter.AppendSql("   AND (a.GBJINCHAL2 IS NULL OR a.GBJINCHAL2 = 'N')                                        ");
                parameter.AppendSql("   AND (b.PanjengDrno IS NULL OR b.PanjengDrno=0)                                          ");
            }
            else  //상담자
            {
                parameter.AppendSql("  AND ((a.GBJINCHAL2 = 'Y'                                                                 ");
                parameter.AppendSql("       AND b.PANJENGDATE IS NOT NULL                                                       ");
                parameter.AppendSql("       AND b.PanjengDrno IS NOT NULL)                                                      ");
                parameter.AppendSql("   OR ((a.GBJINCHAL2 IS NULL OR a.GBJINCHAL2 = 'N')                                        ");    
                parameter.AppendSql("       AND b.PanjengDrno>0))                                                               ");
            }
            if (strChul == "1")     //내원
            {
                parameter.AppendSql("   AND a.GBCHUL = 'N'                                                                      ");
            }
            else if (strChul == "2")    //출장
            {
                parameter.AppendSql("   AND a.GBCHUL = 'Y'                                                                      ");
            }
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                                 ");
            }
            if (strJong != "**" && !strJong.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                                  ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                ");
            }
            parameter.AppendSql("   AND a.GjJong NOT IN ('31','35','56')                                                        ");        //암검진 제외
            parameter.AppendSql(" ORDER BY a.SName,a.WRTNO,a.GjJong                                                             ");


            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", strSName);
            }
            if (strJong != "**" && !strJong.IsNullOrEmpty())
            {
                parameter.Add("GJJONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_RES_DENTAL>(parameter);
        }

        public List<HIC_JEPSU_RES_DENTAL> GetItembyJepDateMirNo(string argFrDate, string argToDate, long argMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,a.Pano,a.Kiho,a.GjJong,a.JikGbn,a.Sname,a.Sex,a.Age, A.GBCHUL   ");
            parameter.AppendSql("     , TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate                                 ");
            parameter.AppendSql("     , TO_CHAR(a.DelDate,'YYYY-MM-DD') DelDate                                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_DENTAL b                   ");
            parameter.AppendSql(" WHERE a.Wrtno = b.Wrtno(+)                                                    ");
            parameter.AppendSql("   AND a.JEPDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')                              ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')                              ");
            parameter.AppendSql("   AND a.MIRNO2 = :MIRNO                                                       ");

            parameter.Add("FRDATE", argFrDate);
            parameter.Add("TODATE", argToDate);
            parameter.Add("MIRNO", argMirno);

            return ExecuteReader<HIC_JEPSU_RES_DENTAL>(parameter);
        }
    }
}
