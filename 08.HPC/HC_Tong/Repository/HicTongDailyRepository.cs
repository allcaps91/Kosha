namespace HC_Tong.Repository
{
    using ComBase;
    using ComBase.Mvc;
    using HC_Tong.Dto;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// 
    /// </summary>
    public class HicTongDailyRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicTongDailyRepository()
        {
        }

        public HIC_TONGDAILY GetEntTimeJobSabunByDate(string argFDate, string argTDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT ENTTIME, JOBSABUN ");
            parameter.AppendSql(" FROM " + ComNum.DB_PMPA + "HIC_TONGDAILY  ");
            parameter.AppendSql(" WHERE TDate>=TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql(" AND TDate<=TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql(" AND Gubun = '1'  ");
            parameter.AppendSql(" GROUP BY ENTTIME, JOBSABUN  ");
            parameter.AppendSql(" ORDER BY ENTTIME DESC, JOBSABUN  ");

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);

            return ExecuteReaderSingle<HIC_TONGDAILY>(parameter);
        }

        public List<HIC_TONGDAILY> GetSumDailyData(string argFDate, string argTDate, string argGbn)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT GJJONG, SUM(TOTAMT) TOTAMT, SUM(JOHAPAMT) JOHAPAMT, ");
            parameter.AppendSql("       SUM(LTDAMT) LTDAMT, SUM(BONINAMT) BONINAMT, SUM(HALINAMT) HALINAMT, ");
            parameter.AppendSql("       SUM(MISUAMT) MISUAMT, SUM(SUNAPAMT) SUNAPAMT, ");
            parameter.AppendSql("       SUM(YEYAKAMT) YEYAKAMT, SUM(YDAECHE) YDAECHE, SUM(SUNAPAMT2) SUNAPAMT2 ");

            parameter.AppendSql("   FROM " + ComNum.DB_PMPA + "HIC_TONGDAILY");

            parameter.AppendSql("   WHERE TDate >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND TDate <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND GUBUN = :Gubun  "); // 검진종류별 수입

            parameter.AppendSql("   GROUP BY GJJONG  ");
            parameter.AppendSql("   ORDER BY GJJONG  ");

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);
            parameter.Add("Gubun", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_TONGDAILY>(parameter);
        }

        public List<HIC_TONGDAILY> GetCashData(string dtpFDate, string strNextDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT SUM(TOTAMT) TOTAMT,SUM(JOHAPAMT) JOHAPAMT, SUM(LTDAMT) LTDAMT,SUM(BONINAMT) BONINAMT, ");
            parameter.AppendSql("       SUM(HALINAMT) HALINAMT, SUM(MISUAMT) MISUAMT,SUM(SUNAPAMT) SUNAPAMT, SUM(YEYAKAMT) YEYAKAMT, ");
            parameter.AppendSql("       SUM(YDAECHE) YDAECHE,SUM(SUNAPAMT2) SUNAPAMT2 ");

            parameter.AppendSql("   FROM " + ComNum.DB_PMPA + "HIC_TONGDAILY");

            parameter.AppendSql("   WHERE TDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND TDATE <= TO_DATE(:STRNEXTDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND GUBUN = '1'  ");
            parameter.AppendSql("       AND GJJONG < '81'  ");
            

            parameter.Add("FDATE", dtpFDate);
            parameter.Add("STRNEXTDATE", strNextDate);

            return ExecuteReader<HIC_TONGDAILY>(parameter);
        }

        public List<HIC_TONGDAILY> GetTotalData(string dtpFDate, string dtpTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT GJJONG, CHASU, GBCHUL, TO_CHAR(TDATE,'YYYY-MM-DD') TDATE, SUM(JEPCNT) JEPCNT ");

            parameter.AppendSql("   FROM " + ComNum.DB_PMPA + "HIC_TONGDAILY");

            parameter.AppendSql("   WHERE TDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND TDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND GUBUN = '2'  ");                                            // 인원통계분류별
            parameter.AppendSql("       AND (GJJONG = '91' OR GJJONG = '92')  ");

            parameter.AppendSql("   GROUP BY GJJONG, CHASU, GBCHUL, TDATE  ");

            parameter.Add("FDATE", dtpFDate);
            parameter.Add("TDATE", dtpTDate);

            return ExecuteReader<HIC_TONGDAILY>(parameter);
        }

        public List<HIC_TONGDAILY> GetTongData(string argFDate, string argTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT GJJONG, CHASU, GBCHUL, TO_CHAR(TDATE,'YYYY-MM-DD') TDATE, SUM(JEPCNT) JEPCNT ");

            parameter.AppendSql("   FROM " + ComNum.DB_PMPA + "HIC_TONGDAILY");

            parameter.AppendSql("   WHERE TDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND TDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND GUBUN = '2'  ");                                            // 인원통계분류별
            parameter.AppendSql("       AND GJJONG <= '70'  ");

            parameter.AppendSql("   GROUP BY GJJONG, CHASU, GBCHUL, TDATE  ");

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);

            return ExecuteReader<HIC_TONGDAILY>(parameter);
        }

        public List<HIC_TONGDAILY> GetCheckUpCash(string argFDate, string argTDate, string argGbn)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT GJJONG, SUM(SUNAPAMT) SUNAPAMT ");

            parameter.AppendSql("   FROM " + ComNum.DB_PMPA + "HIC_TONGDAILY");

            parameter.AppendSql("   WHERE TDate >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND TDate <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND GUBUN = :Gubun  "); 

            parameter.AppendSql("   GROUP BY GJJONG  ");
            parameter.AppendSql("   ORDER BY GJJONG  ");

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);
            parameter.Add("Gubun", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_TONGDAILY>(parameter);
        }
    }
}