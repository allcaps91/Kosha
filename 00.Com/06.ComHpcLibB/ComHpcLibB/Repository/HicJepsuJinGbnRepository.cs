namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuJinGbnRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuJinGbnRepository()
        {
        }

        public List<HIC_JEPSU_JIN_GBN> GetItembyJepDate(string strFrDate, string strToDate, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,a.SName,TO_CHAR(a.JepDate,'YY-MM-DD') JepDate,a.GjJong,b.Gubun,b.ROWID RID  ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_CODENAME('J1', b.GUBUN) GUBUNNAME                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_JIN_GBN b                                  ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO(+)                                                                ");
            parameter.AppendSql("   AND a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                   ");
            parameter.AppendSql("   AND a.GjJong IN ('32')                                                                  "); //진단서만
            if (nLtdCode != 0)                                                                                              
            {                                                                                                               
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                            ");
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_JIN_GBN>(parameter);
        }

        public HIC_JEPSU_JIN_GBN GetItembyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Pano,a.SName,a.Age,a.Sex,a.LtdCode,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate    ");
            parameter.AppendSql("     , a.GjJong,b.PanjengDrNo                                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_JIN_GBN b                              ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO(+)                                                            ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                                ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_JEPSU_JIN_GBN>(parameter);
        }
    }
}
