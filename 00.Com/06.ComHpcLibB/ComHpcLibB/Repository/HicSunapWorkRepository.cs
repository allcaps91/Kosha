namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicSunapWorkRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSunapWorkRepository()
        {
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SUNAP_WORK SET          ");
            parameter.AppendSql("       PANO = :PANO                            ");
            parameter.AppendSql(" WHERE PANO IN (SELECT PANO                    ");
            parameter.AppendSql("                  FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql("                 WHERE JUMIN2 = :JUMIN2        ");
            parameter.AppendSql("                   AND PANO <> :PANO)          ");

            parameter.Add("PANO", argPaNo);
            parameter.Add("JUMIN2", argJumin2);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyPaNoGjJong(long argPano, string argGjjong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAP_WORK          ");
            parameter.AppendSql(" WHERE PANO   = :PANO                      ");
            parameter.AppendSql("   AND GJJONG = :GJJONG                    ");

            parameter.Add("PANO", argPano);
            parameter.Add("GJJONG", argGjjong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public void DeleteByPanoSuDate(string jEPDATE, long pANO, string argJong = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SUNAP_WORK              ");
            parameter.AppendSql(" WHERE PANO   = :PANO                          ");
            parameter.AppendSql("   AND SUDATE = TO_DATE(:SUDATE,'YYYY-MM-DD')  ");
            if (!argJong.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GJJONG = :GJJONG  ");
            }

            parameter.Add("PANO", pANO);
            parameter.Add("SUDATE", jEPDATE);
            if (!argJong.IsNullOrEmpty())
            {
                parameter.Add("GJJONG", argJong);
            }

            ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_SUNAP_WORK item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO HIC_SUNAP_WORK                                                                                         ");
            parameter.AppendSql("       (WRTNO, SUDATE, SEQNO, PANO, TOTAMT, HALINGYE, HALINAMT                                                     ");
            parameter.AppendSql("     , JOHAPAMT,LTDAMT,BONINAMT,MISUGYE,MISUAMT,SUNAPAMT,JOBSABUN,ENTTIME,GJJONG, BOGUNSOAMT )                     ");
            parameter.AppendSql("VALUES                                                                                                             ");
            parameter.AppendSql("       (:WRTNO, TO_DATE(:SUDATE, 'YYYY-MM-DD'), :SEQNO, :PANO, :TOTAMT, :HALINGYE, :HALINAMT                       ");
            parameter.AppendSql("     , :JOHAPAMT, :LTDAMT, :BONINAMT, :MISUGYE, :MISUAMT, :SUNAPAMT, :JOBSABUN, SYSDATE, :GJJONG, :BOGUNSOAMT)     ");

            parameter.Add("WRTNO",    item.WRTNO); 
            parameter.Add("SUDATE",   item.SUDATE);
            parameter.Add("SEQNO",    item.SEQNO);
            parameter.Add("PANO",     item.PANO);
            parameter.Add("TOTAMT",   item.TOTAMT);
            parameter.Add("HALINGYE", item.HALINGYE);
            parameter.Add("HALINAMT", item.HALINAMT);
            parameter.Add("JOHAPAMT", item.JOHAPAMT);
            parameter.Add("LTDAMT",   item.LTDAMT);
            parameter.Add("BONINAMT", item.BONINAMT);
            parameter.Add("MISUGYE",  item.MISUGYE);
            parameter.Add("MISUAMT",  item.MISUAMT);
            parameter.Add("SUNAPAMT", item.SUNAPAMT);
            parameter.Add("JOBSABUN", item.JOBSABUN);
            parameter.Add("BOGUNSOAMT", item.BOGUNSOAMT);
            parameter.Add("GJJONG",   item.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int DeletebyPaNoGjJong(long argPaNo, string argGjjong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SUNAP_WORK              ");
            parameter.AppendSql(" WHERE PANO   = :PANO                          ");
            parameter.AppendSql("   AND GJJONG = :GJJONG                        ");

            parameter.Add("PANO", argPaNo);
            parameter.Add("GJJONG", argGjjong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }


        public HIC_SUNAP_WORK Read_Hic_Sunap_Work(long argPANO, string argJONG, string argSUDATE1, string argSUDATE2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT                                                                         ");
            parameter.AppendSql(" WRTNO,SUDATE,SEQNO,PANO,TOTAMT,HALINGYE,HALINAMT,JOHAPAMT,LTDAMT,BONINAMT     ");
            parameter.AppendSql(" ,MISUGYE,MISUAMT,SUNAPAMT,JOBSABUN,ENTTIME,BOGUNSOAMT,CARDSEQNO,GJJONG        ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_SUNAP_WORK                                               ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                            ");
            parameter.AppendSql(" AND GJJONG = :GJJONG                                                          ");
            parameter.AppendSql(" AND SUDATE >= TO_DATE(:SUDATE1,'YYYY-MM-DD')                                   ");
            parameter.AppendSql(" AND SUDATE <= TO_DATE(:SUDATE2,'YYYY-MM-DD')                                   ");

            parameter.Add("PANO", argPANO);
            parameter.Add("GJJONG", argJONG);
            parameter.Add("SUDATE1", argSUDATE1);
            parameter.Add("SUDATE2", argSUDATE2);

            return ExecuteReaderSingle<HIC_SUNAP_WORK>(parameter);
        }
    }
}
