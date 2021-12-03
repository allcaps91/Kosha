namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaSunapRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaSunapRepository()
        {
        }

        public List<HEA_SUNAP> GetSumbySuDate(string strFrDate, string strToDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT JobSabun,SUM(SunapAmt1) SunapAmt1,SUM(SunapAmt2) SunapAmt2,SUM(TotAmt) TotAmt   ");
            parameter.AppendSql("     , SUM(HalinAmt) HalinAmt,SUM(MisuAmt) MisuAmt                                     ");
            parameter.AppendSql("     , SUM(LtdAmt) LtdAmt                                                              ");
            parameter.AppendSql("     , SUM(BoninAmt) BoninAmt                                                          ");
            parameter.AppendSql("  FROM ADMIN.Hea_SUNAP                                                           ");
            parameter.AppendSql(" WHERE SuDate>=TO_DATE(:FRDATE, 'YYYY-MM-DD')                                          ");
            parameter.AppendSql("   AND SuDate<=TO_DATE(:TODATE, 'YYYY-MM-DD')                                          ");
            parameter.AppendSql(" GROUP BY JobSabun                                                                     ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReader<HEA_SUNAP>(parameter);
        }

        public long GetSumTotAmtByWrtno(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(TOTAMT) TOTAMT          ");
            parameter.AppendSql("  FROM ADMIN.HEA_SUNAP       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteScalar<long>(parameter);
        }

        public long GetSumBoninAmtByWrtno(long wRTNO1)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(BONINAMT) BONINAMT      ");
            parameter.AppendSql("  FROM ADMIN.HEA_SUNAP       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", wRTNO1);

            return ExecuteScalar<long>(parameter);
        }

        public long GetMisuAmtByWrtno(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(MISUAMT) MISUAMT      ");
            parameter.AppendSql("  FROM ADMIN.HEA_SUNAP     ");
            parameter.AppendSql(" WHERE 1 = 1                     ");
            parameter.AppendSql("   AND WRTNO = :WRTNO            ");
            parameter.AppendSql("   AND MISUGYE = '01'            ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<long>(parameter);
        }

        public void Insert(HEA_SUNAP haSUNAP)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HEA_SUNAP (                                            ");
            parameter.AppendSql("       WRTNO,SUDATE,SEQNO,PANO,TOTAMT,HALINGYE,HALINAMT,LTDAMT                 ");
            parameter.AppendSql("      ,BONINAMT,MISUGYE,MISUAMT,SUNAPAMT1,SUNAPAMT2,CARDNO,CARDSEQNO           ");
            parameter.AppendSql("      ,EXAMCODES,JOBSABUN,ENTTIME                                              ");
            parameter.AppendSql(" ) VALUES (                                                                    ");
            parameter.AppendSql("       :WRTNO,TRUNC(SYSDATE),:SEQNO,:PANO,:TOTAMT,:HALINGYE,:HALINAMT,:LTDAMT  ");
            parameter.AppendSql("      ,:BONINAMT,:MISUGYE,:MISUAMT,:SUNAPAMT1,:SUNAPAMT2,'',:CARDSEQNO         ");
            parameter.AppendSql("      ,:EXAMCODES,:JOBSABUN, SYSDATE                                           ");
            parameter.AppendSql(" )                                                                             ");

            parameter.Add("WRTNO",      haSUNAP.WRTNO);
            parameter.Add("SEQNO",      haSUNAP.SEQNO);
            parameter.Add("PANO",       haSUNAP.PANO);
            parameter.Add("TOTAMT",     haSUNAP.TOTAMT);
            parameter.Add("HALINGYE",   haSUNAP.HALINGYE);
            parameter.Add("HALINAMT",   haSUNAP.HALINAMT);
            parameter.Add("LTDAMT",     haSUNAP.LTDAMT);
            parameter.Add("BONINAMT",   haSUNAP.BONINAMT);
            parameter.Add("MISUGYE",    haSUNAP.MISUGYE);
            parameter.Add("MISUAMT",    haSUNAP.MISUAMT);
            parameter.Add("SUNAPAMT1",  haSUNAP.SUNAPAMT1);
            parameter.Add("SUNAPAMT2",  haSUNAP.SUNAPAMT2);
            parameter.Add("CARDSEQNO",  haSUNAP.CARDSEQNO);
            parameter.Add("EXAMCODES",  haSUNAP.EXAMCODES);
            parameter.Add("JOBSABUN",   haSUNAP.JOBSABUN);

            ExecuteNonQuery(parameter);
        }

        public string GetRowidByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                               ");
            parameter.AppendSql("  FROM ADMIN.HEA_SUNAP               ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public List<HEA_SUNAP> GetRowidByWrtno1(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID AS RID                        ");
            parameter.AppendSql("  FROM ADMIN.HEA_SUNAP               ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");
            parameter.AppendSql(" AND MISUGYE IS NULL                       ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HEA_SUNAP>(parameter);
        }
        public long GetSunapAmtByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(TOTAMT-HALINAMT) AS TAMT        ");
            parameter.AppendSql("  FROM ADMIN.HEA_SUNAP               ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");            

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<long>(parameter);
        }

        public HEA_SUNAP GetAmtbyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(TOTAMT) TOTAMT,SUM(HALINAMT) HALINAMT, SUM(LTDAMT) LTDAMT               ");
            parameter.AppendSql("     , SUM(BONINAMT) BONINAMT,SUM(SUNAPAMT1) CASHAMT, SUM(SUNAPAMT2) CARDAMT       ");
            parameter.AppendSql("  FROM ADMIN.HEA_SUNAP                                                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                              ");
            parameter.AppendSql(" GROUP BY JobSabun                                                                 ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReaderSingle<HEA_SUNAP>(parameter);
        }

        public void DeleteByRowid(string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HEA_SUNAP       ");
            parameter.AppendSql(" WHERE ROWID = :RID              ");

            parameter.Add("RID", argRowid);

            ExecuteNonQuery(parameter);
        }

        

    }
}
