namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuResBohum2Repository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResBohum2Repository()
        {
        }

        public HIC_JEPSU_RES_BOHUM2 GetItembyPanoGjYearGjBangiJepsuDate(string strPANO, string strGjYear, string strGJBANGI, string strJEPDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.SName,TO_CHAR(a.JEPDate,'YYYY-MM-DD') JEPDate,a.GjJong            ");
            parameter.AppendSql("     , a.BuseName,a.GjChasu,a.Pano,b.Panjeng                                       ");
            parameter.AppendSql("     , b.Chest_Res, b.Cycle_Res, b.Goji_Res, b.Liver_Res, b.Kidney_Res             ");
            parameter.AppendSql("     , b.Amemia_Res, b.Diabetes_Res, TO_CHAR(b.PanjengDate,'MM/DD') PanjengDate    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_BOHUM2 b                       ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                                              ");
            parameter.AppendSql("   AND a.GJCHASU = '2'                                                             ");
            parameter.AppendSql("   AND a.GJYEAR  = :GJYEAR                                                         ");
            parameter.AppendSql("   AND a.GJBANGI = :GJBANGI                                                        ");
            parameter.AppendSql("   AND a.JEPDATE > TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("   AND a.Wrtno   = b.Wrtno(+)                                                      ");

            parameter.Add("PANO", strPANO);
            parameter.Add("GJYEAR", strGjYear, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GJBANGI", strGJBANGI, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", strJEPDATE);

            return ExecuteReaderSingle<HIC_JEPSU_RES_BOHUM2>(parameter);
        }

        public HIC_JEPSU_RES_BOHUM2 GetItembyJepDateMirNo(string argFrDate, string argToDate, long argMirno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.Pano,a.Kiho,a.GjJong,a.JikGbn,a.Sname,a.Sex,a.Age,a.GjChasu       ");
            parameter.AppendSql("     , TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,b.PanjengDrno                       ");
            parameter.AppendSql("     , TO_CHAR(b.PanjengDate,'YYYY-MM-DD') PanjengDate                             ");
            parameter.AppendSql("     , TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_BOHUM2 b                       ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO                                                           ");
            parameter.AppendSql("   AND a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("   AND a.MIRNO1 = :MIRNO                                                           ");
            parameter.AppendSql("   AND a.GjChasu NOT IN ('1')                                                      ");
            parameter.AppendSql("   AND a.Pano                                                                      ");

            parameter.Add("FRDATE", argFrDate);
            parameter.Add("TODATE", argToDate);
            parameter.Add("MIRNO", argMirno);

            return ExecuteReaderSingle<HIC_JEPSU_RES_BOHUM2>(parameter);
        }
    }
}
