namespace HC_Tong.Repository
{
    using ComBase;
    using ComBase.Mvc;
    using HC_Tong.Model;
    using System.Collections.Generic;


    /// <summary>
    /// 
    /// </summary>
    public class HicTongDailyOtherRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicTongDailyOtherRepository()
        {
        }
        

        public List<HIC_TONGDAILY_OTHER> GetTongData(string argFDate, string argTDate, bool rdoBtnCheck)
        {
            if (rdoBtnCheck)
            {
                MParameter parameter = CreateParameter();

                parameter.AppendSql("   SELECT a.GJJONG, b.NAME, a.CHASU, a.GBCHUL, SUM(a.JEPCNT) JEPCNT, ");
                parameter.AppendSql("          SUM(a.TOTAMT) TOTAMT, SUM(a.JOHAPAMT) JOHAPAMT, ");
                parameter.AppendSql("          SUM(a.LTDAMT) LTDAMT, SUM(a.BONINAMT) BONINAMT, ");
                parameter.AppendSql("          SUM(a.HALINAMT) HALINAMT, SUM(a.MISUAMT) MISUAMT, ");
                parameter.AppendSql("          SUM(a.SUNAPAMT) SUNAPAMT ");

                parameter.AppendSql("   FROM " + ComNum.DB_PMPA + "HIC_TONGDAILY a, HIC_EXJONG b");

                parameter.AppendSql("   WHERE       a.TDate >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
                parameter.AppendSql("       AND     a.TDate <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
                parameter.AppendSql("       AND     GUBUN = '1'  ");
                parameter.AppendSql("       AND     a.GJJONG = b.Code(+)  ");

                parameter.AppendSql("   GROUP BY a.GJJONG, b.NAME, a.CHASU, a.GBCHUL  ");
                parameter.AppendSql("   ORDER BY a.GJJONG, a.CHASU, a.GBCHUL  ");

                parameter.Add("FDATE", argFDate);
                parameter.Add("TDATE", argTDate);


                return ExecuteReader<HIC_TONGDAILY_OTHER>(parameter);
            }
            else
            {
                MParameter parameter = CreateParameter();

                parameter.AppendSql("   SELECT a.GJJONG, b.NAME, a.CHASU, a.GBCHUL, SUM(a.JEPCNT) JEPCNT, ");
                parameter.AppendSql("          SUM(a.TOTAMT) TOTAMT, SUM(a.JOHAPAMT) JOHAPAMT, ");
                parameter.AppendSql("          SUM(a.LTDAMT) LTDAMT, SUM(a.BONINAMT) BONINAMT, ");
                parameter.AppendSql("          SUM(a.HALINAMT) HALINAMT, SUM(a.MISUAMT) MISUAMT, ");
                parameter.AppendSql("          SUM(a.SUNAPAMT) SUNAPAMT ");

                parameter.AppendSql("   FROM " + ComNum.DB_PMPA + "HIC_TONGDAILY a, HIC_CODE b");

                parameter.AppendSql("   WHERE       a.TDate >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
                parameter.AppendSql("       AND     a.TDate <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
                parameter.AppendSql("       AND     a.GUBUN = '2'  ");
                parameter.AppendSql("       AND     a.GJJONG = b.Code(+)  ");
                parameter.AppendSql("       AND     b.Gubun = '24'  ");

                parameter.AppendSql("   GROUP BY a.GJJONG, b.NAME, a.CHASU, a.GBCHUL  ");
                parameter.AppendSql("   ORDER BY a.GJJONG, a.CHASU, a.GBCHUL  ");

                parameter.Add("FDATE", argFDate);
                parameter.Add("TDATE", argTDate);


                return ExecuteReader<HIC_TONGDAILY_OTHER>(parameter);
            }
        }
    }
}