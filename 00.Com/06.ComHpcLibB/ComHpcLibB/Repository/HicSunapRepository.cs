namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicSunapRepository : BaseRepository
    {        
        /// <summary>
        /// 
        /// </summary>
        public HicSunapRepository()
        {
        }

        public HIC_SUNAP Read_Hic_Sunap(long argWrtNo)
        {   
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SUM(TOTAMT) TOTAMT,SUM(HALINAMT) HALINAMT, SUM(JOHAPAMT) JOHAPAMT       ");
            parameter.AppendSql("     , SUM(LTDAMT) LTDAMT, SUM(BONINAMT) BOINAMT                               ");
            parameter.AppendSql("     , SUM(BOGENAMT) BOGENAMT                                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAP                                                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                          ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReaderSingle<HIC_SUNAP>(parameter);            
        }

        public int InsertSelectbyWrtNOSeqNo(long nWrtNo, long nSeq, long nJobSabun, long fnWrtNo, long nOLD_Seq, string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SUNAP                                                          ");
            parameter.AppendSql("       (WRTNO,SUDATE,SEQNO,PANO,TOTAMT,HALINGYE,HALINAMT,JOHAPAMT,LTDAMT,BONINAMT          ");
            parameter.AppendSql("      , MISUGYE,MISUAMT,SUNAPAMT,JOBSABUN,ENTTIME,BOGUNSOAMT,CARDSEQNO,MIRNO1,MIRNO2       ");
            parameter.AppendSql("      , MIRNO3 , MISUNO1, MIRNO4, SUNAPAMT2, MISUNO2, MIRNO5, MISUNO3)                     ");
            parameter.AppendSql(" SELECT :WRTNO, TO_DATE(:TODAY,'YYYY-MM-DD')                                               ");
            parameter.AppendSql("      , :SEQNO, PANO,TOTAMT,HALINGYE,HALINAMT                                              ");
            parameter.AppendSql("      , JOHAPAMT,LTDAMT,BONINAMT                                                           ");
            parameter.AppendSql("      , MISUGYE,MISUAMT,SUNAPAMT, :JOBSABUN, SYSDATE                                       ");
            parameter.AppendSql("      , BOGUNSOAMT,CARDSEQNO,MIRNO1,MIRNO2                                                 ");
            parameter.AppendSql("      , MIRNO3 , MISUNO1, MIRNO4, SUNAPAMT2, MISUNO2, MIRNO5, MISUNO3                      ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_SUNAP                                                              ");
            parameter.AppendSql("  WHERE WRTNO = :FWRTNO                                                                    ");
            parameter.AppendSql("    AND SEQNO = :OLDSEQNO                                                                  ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("FWRTNO", fnWrtNo);
            parameter.Add("TODAY", strDate);
            parameter.Add("JOBSABUN", nJobSabun);
            parameter.Add("SEQNO", nSeq);
            parameter.Add("OLDSEQNO", nOLD_Seq);

            return ExecuteNonQuery(parameter);
        }

        public HIC_SUNAP GetHeaSunapAmtByWRTNO(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("       SELECT SUM(TOTAMT) TOTAMT, SUM(HALINAMT) HALINAMT, 0 JOHAPAMT                      ");
            parameter.AppendSql("             ,SUM(LTDAMT) LTDAMT, SUM(BONINAMT) BONINAMT,  SUM(MISUAMT) MISUAMT           ");
            parameter.AppendSql("             ,0 BOGENAMT                                                                  ");
            parameter.AppendSql("             ,SUM(SUNAPAMT1) SUNAPAMT1, SUM(SUNAPAMT2) SUNAPAMT2                          ");
            parameter.AppendSql("         FROM KOSMOS_PMPA.HEA_SUNAP                                                       ");
            parameter.AppendSql("        WHERE 1 = 1                                                                       ");
            parameter.AppendSql("          AND WRTNO = :WRTNO                                                              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_SUNAP>(parameter);
        }
        public HIC_SUNAP GetItemByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("       SELECT WRTNO, CODE                          ");
            parameter.AppendSql("         FROM KOSMOS_PMPA.HIC_SUNAP                ");
            parameter.AppendSql("        WHERE 1 = 1                                ");
            parameter.AppendSql("          AND WRTNO = :WRTNO                       ");
            parameter.AppendSql("          AND CODE IN ('3101')                     ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_SUNAP>(parameter);
        }
        public HIC_SUNAP GetHicSunapAmtByWRTNO(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT SUM(TOTAMT) TOTAMT,     SUM(HALINAMT) HALINAMT,  SUM(JOHAPAMT) JOHAPAMT      ");
            parameter.AppendSql("       ,SUM(LTDAMT) LTDAMT,     SUM(BONINAMT) BONINAMT,   SUM(MISUAMT) MISUAMT        ");
            parameter.AppendSql("       ,SUM(BOGENAMT) BOGENAMT, SUM(SUNAPAMT) SUNAPAMT, SUM(SUNAPAMT2) SUNAPAMT2    ");
            //parameter.AppendSql("       ,MAX(SEQNO) SEQNO                                                             ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_SUNAP                                                        ");
            parameter.AppendSql("  WHERE 1 = 1                                                                        ");
            parameter.AppendSql("    AND WRTNO = :WRTNO                                                               ");
            
            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_SUNAP>(parameter);
        }

        public string GetMisuGyeByWrtnoSeqNo(long nWRTNO, long nSeqNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MISUGYE                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAP                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND SEQNO = :SEQNO                          ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("SEQNO", nSeqNo);

            return ExecuteScalar<string>(parameter);
        }

        public void Insert(HIC_SUNAP item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SUNAP (                                                    ");
            parameter.AppendSql("       WRTNO,SUDATE,SEQNO,PANO,TOTAMT,HALINGYE,HALINAMT,JOHAPAMT,LTDAMT                ");
            parameter.AppendSql("      ,BONINAMT,BOGENAMT,MISUGYE,MISUAMT,SUNAPAMT,SUNAPAMT2,JOBSABUN,ENTTIME           ");
            parameter.AppendSql(" ) VALUES (                                                                            ");
            parameter.AppendSql("      :WRTNO,TRUNC(SYSDATE),:SEQNO,:PANO,:TOTAMT,:HALINGYE,:HALINAMT,:JOHAPAMT,:LTDAMT ");
            parameter.AppendSql("     ,:BONINAMT,:BOGENAMT,:MISUGYE,:MISUAMT,:SUNAPAMT,:SUNAPAMT2,:JOBSABUN,SYSDATE     ");
            parameter.AppendSql(" )                                                                                     ");

            parameter.Add("WRTNO",      item.WRTNO);
            parameter.Add("SEQNO",      item.SEQNO);
            parameter.Add("PANO",       item.PANO);
            parameter.Add("TOTAMT",     item.TOTAMT);
            parameter.Add("HALINGYE",   item.HALINGYE.To<string>(""));
            parameter.Add("HALINAMT",   item.HALINAMT);
            parameter.Add("JOHAPAMT",   item.JOHAPAMT);
            parameter.Add("LTDAMT",     item.LTDAMT);
            parameter.Add("BONINAMT",   item.BONINAMT);
            parameter.Add("BOGENAMT",   item.BOGENAMT);
            parameter.Add("MISUGYE",    item.MISUGYE.To<string>(""));
            parameter.Add("MISUAMT",    item.MISUAMT);
            parameter.Add("SUNAPAMT",   item.SUNAPAMT);
            parameter.Add("SUNAPAMT2",  item.SUNAPAMT2);
            parameter.Add("JOBSABUN", clsType.User.IdNumber);
            
            ExecuteNonQuery(parameter);
        }

        public void InsertHea(HIC_SUNAP item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_SUNAP (                                            ");
            parameter.AppendSql("       WRTNO,SUDATE,SEQNO,PANO,TOTAMT,HALINGYE,HALINAMT,LTDAMT                 ");
            parameter.AppendSql("      ,BONINAMT,MISUGYE,MISUAMT,SUNAPAMT1,SUNAPAMT2,JOBSABUN,ENTTIME           ");
            parameter.AppendSql(" ) VALUES (                                                                    ");
            parameter.AppendSql("      :WRTNO,TRUNC(SYSDATE),:SEQNO,:PANO,:TOTAMT,:HALINGYE,:HALINAMT,:LTDAMT   ");
            parameter.AppendSql("     ,:BONINAMT,:MISUGYE,:MISUAMT,:SUNAPAMT1,:SUNAPAMT2,:JOBSABUN,SYSDATE      ");
            parameter.AppendSql(" )                                                                             ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SEQNO", item.SEQNO);
            parameter.Add("PANO", item.PANO);
            parameter.Add("TOTAMT", item.TOTAMT);
            parameter.Add("HALINGYE", item.HALINGYE.To<string>(""));
            parameter.Add("HALINAMT", item.HALINAMT);
            parameter.Add("LTDAMT", item.LTDAMT);
            parameter.Add("BONINAMT", item.BONINAMT);
            parameter.Add("MISUGYE", item.MISUGYE.To<string>(""));
            parameter.Add("MISUAMT", item.MISUAMT);
            parameter.Add("SUNAPAMT1", item.SUNAPAMT1);
            parameter.Add("SUNAPAMT2", item.SUNAPAMT2);
            parameter.Add("JOBSABUN", clsType.User.IdNumber);

            ExecuteNonQuery(parameter);
        }

        public void UpdateTotAmtJhpAmtbyRowid(HIC_SUNAP item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SUNAP SET   ");
            parameter.AppendSql("       TOTAMT = :TOTAMT            ");
            parameter.AppendSql("      ,JOHAPAMT = :JOHAPAMT        ");
            parameter.AppendSql(" WHERE ROWID = :RID                ");

            parameter.Add("TOTAMT", item.TOTAMT);
            parameter.Add("JOHAPAMT", item.JOHAPAMT);
            parameter.Add("RID", item.RID);

            ExecuteNonQuery(parameter);
        }

        public HIC_SUNAP GetHicSunapByWRTNO(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT TOTAMT, JOHAPAMT, SUNAPAMT, SUNAPAMT2, SEQNO, ROWID AS RID    ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_SUNAP                           ");
            parameter.AppendSql("  WHERE 1 = 1                                           ");
            parameter.AppendSql("    AND WRTNO = :WRTNO                                  ");
            parameter.AppendSql("  ORDER BY SEQNO DESC                                   "); 

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_SUNAP>(parameter);
        }

        public long GetTotAmtbyWrtNo2(long wRTNO2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT SUM(TotAmt) TotAmt                                     ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_SUNAP                                  ");
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                                         ");

            parameter.Add("WRTNO", wRTNO2);

            return ExecuteScalar<long>(parameter);
        }

        public HIC_SUNAP GetJohapAmtBogenAmtbyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT SUM(JohapAmt) JohapAmt,SUM(BogenAmt) BogenAmt          ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_SUNAP                                  ");
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                                         ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReaderSingle<HIC_SUNAP>(parameter);
        }

        public long GetJohapAmtbyMirNo(long fnMirNo, string strJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT SUM(JohapAmt) JOHAPAMT                                 ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_SUNAP                                  ");
            parameter.AppendSql("  WHERE Wrtno IN (SELECT Wrtno                                 ");
            parameter.AppendSql("                    FROM KOSMOS_PMPA.HIC_JEPSU                 ");
            if (strJong == "4")
            {
                parameter.AppendSql("                   WHERE MIRNO3 = :MIRNO )                 ");
            }
            else
            {
                parameter.AppendSql("                   WHERE MIRNO5 = :MIRNO )                 ");
            }

            parameter.Add("MIRNO", fnMirNo);

            return ExecuteScalar<long>(parameter);
        }

        public int UpdateMirNo5byMirNo5(long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SUNAP SET               ");
            parameter.AppendSql("       MIRNO5 = 0                              ");
            parameter.AppendSql(" WHERE MIRNO5 = :MIRNO                         ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateGongDan(long nMisuNo, List<string> WRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE             KOSMOS_PMPA.HIC_SUNAP SET           ");
            parameter.AppendSql("                   MISUNO4=:MISUNO                     ");
            parameter.AppendSql("WHERE              WRTNO IN (:WRTNO)                    ");
            parameter.AppendSql("   AND             (MISUNO4 IS NULL OR MISUNO4 = 0)    ");

            parameter.Add("MISUNO", nMisuNo);
            parameter.AddInStatement("WRTNO", WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int GuGangGongDan(long nMisuNo, List<string> wrtno, string fstrJong)
        {
            MParameter parameter = CreateParameter();

            if (fstrJong != "6")
            {
                parameter.AppendSql("UPDATE             KOSMOS_PMPA.HIC_SUNAP SET                   ");
                parameter.AppendSql("                   MISUNO2=:MISUNO                             ");
                parameter.AppendSql("WHERE              WRTNO IN (:WRTNO)                           ");
                parameter.AppendSql("   AND             (MISUNO2 IS NULL OR MISUNO2 = 0)            ");
            }
            else
            {
                parameter.AppendSql("UPDATE             KOSMOS_PMPA.HIC_SUNAP SET                   ");
                parameter.AppendSql("                   MISUNO3=:MISUNO                             ");
                parameter.AppendSql("WHERE              WRTNO IN (:WRTNO)                           ");
                parameter.AppendSql("   AND             (MISUNO3 IS NULL OR MISUNO3 = 0)            ");
            }

            if (fstrJong != "6")
            {
                parameter.Add("MISUNO", nMisuNo);
                parameter.AddInStatement("WRTNO", wrtno);
            }
            else
            {
                parameter.Add("MISUNO", nMisuNo);
                parameter.AddInStatement("WRTNO", wrtno);
            }



            return ExecuteNonQuery(parameter);
        }

        public int CompanySuNapUpdate(long nMisuNo, string strFDate, string strTDate, string strltdcode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE                 KOSMOS_PMPA.HIC_SUNAP SET                                   ");
            parameter.AppendSql("                       MISUNO2=:MISUNO                                             ");

            parameter.AppendSql("WHERE                  WRTNO IN ( SELECT WRTNO FROM HIC_JEPSU                      ");
            parameter.AppendSql("           WHERE       JEPDATE>=TO_DATE(:FDATE,'YYYY-MM-DD')                       ");
            parameter.AppendSql("               AND     JEPDATE<=TO_DATE(:TDATE,'YYYY-MM-DD')                       ");
            parameter.AppendSql("               AND     LTDCODE = :LTDCODE                                          ");
            parameter.AppendSql("               AND     DELDATE IS NULL                                             ");
            parameter.AppendSql("               AND     PANO <> 999                                                 ");
            parameter.AppendSql("               AND     MISUNO2 = :MISUNO)                                          ");

            parameter.AppendSql("AND                    (MISUNO2 IS NULL OR MISUNO2 = 0)                            ");
            
            parameter.Add("MISUNO", nMisuNo);
            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);
            parameter.Add("LTDCODE", strltdcode);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateMirno2byMirNo2(long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SUNAP SET               ");
            parameter.AppendSql("       MIRNO2 = 0                              ");
            parameter.AppendSql(" WHERE MIRNO2 = :MIRNO                         ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateMirNo3byMirNo3(long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SUNAP SET               ");
            parameter.AppendSql("       MIRNO3 = 0                              ");
            parameter.AppendSql(" WHERE MIRNO3 = :MIRNO                         ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyMirNo1(long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SUNAP SET               ");
            parameter.AppendSql("       MIRNO1 = 0                              ");
            parameter.AppendSql(" WHERE MIRNO1 = :MIRNO                         ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateMirNo0byWrtNo(long nWRTNO, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SUNAP SET               ");
            if (strGubun == "1")
            {
                parameter.AppendSql("       MIRNO1 = 0                          ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql("       MIRNO2 = 0                          ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql("       MIRNO3 = 0                          ");
                parameter.AppendSql("     , MIRNO4 = 0                          ");
            }
            else if (strGubun == "E")
            {
                parameter.AppendSql("       MIRNO5 = 0                          ");
            }
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public long GetLtdAmtbyMirNo(long fnMirNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT SUM(LtdAmt) LtdAmt                                     ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_SUNAP                                  ");
            parameter.AppendSql("  WHERE WRTNO IN (SELECT Wrtno                                 ");
            parameter.AppendSql("                    FROM KOSMOS_PMPA.HIC_JEPSU                 ");
            parameter.AppendSql("                   WHERE MIRNO3 = :MIRNO                       ");
            parameter.AppendSql("                     AND MURYOAM = 'Y'                         ");
            parameter.AppendSql("                     AND MuryoGbn  > '0' )                     ");

            parameter.Add("MIRNO", fnMirNo);

            return ExecuteScalar<long>(parameter);
        }

        public long GetBogenAmtbyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT SUM(BogenAmt) BogenAmt                                 ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_SUNAP                                  ");
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                                         ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<long>(parameter);
        }

        public int UpdateMirNobyWrtNo(long nWRTNO, long nMirno, string sGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SUNAP SET               ");
            if (sGubun == "1")
            {
                parameter.AppendSql("       MIRNO1 = :MIRNO                     ");
            }
            if (sGubun == "2")
            {
                parameter.AppendSql("       MIRNO2 = :MIRNO                     ");
            }
            if (sGubun == "3")
            {
                parameter.AppendSql("       MIRNO3 = :MIRNO                     ");
            }
            if (sGubun == "5")
            {
                parameter.AppendSql("       MIRNO5 = :MIRNO                     ");
            }
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                         ");

            parameter.Add("MIRNO", nMirno);
            parameter.Add("WRTNO", nWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public void InsertSelectBySuDatePano(long wRTNO, string jEPDATE, long pANO, string argJong = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SUNAP (                                                        ");
            parameter.AppendSql("            WRTNO,SUDATE,SEQNO,PANO,TOTAMT,HALINGYE,HALINAMT                               ");
            parameter.AppendSql("           ,JOHAPAMT,LTDAMT,BONINAMT,SUNAPAMT,JOBSABUN,ENTTIME, BOGENAMT )                 ");
            parameter.AppendSql(" SELECT :WRTNO,TRUNC(SYSDATE), 0, PANO,SUM(TOTAMT),HALINGYE,SUM(HALINAMT)                  ");
            parameter.AppendSql("       ,SUM(JOHAPAMT),SUM(LTDAMT),SUM(BONINAMT),SUM(SUNAPAMT),111,SYSDATE, SUM(BOGUNSOAMT) ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAP_WORK                                                          ");
            parameter.AppendSql(" WHERE SUDATE = TO_DATE(:SUDATE,'YYYY-MM-DD')                                              ");
            parameter.AppendSql("   AND PANO = :PANO                                                                        ");
            if (!argJong.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GJJONG = :GJJONG                                                ");
            }
            parameter.AppendSql(" GROUP BY PANO,HALINGYE                                                    ");

            parameter.Add("WRTNO", wRTNO);
            parameter.Add("SUDATE", jEPDATE);
            parameter.Add("PANO", pANO);

            if (!argJong.IsNullOrEmpty())
            {
                parameter.Add("GJJONG", argJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            ExecuteNonQuery(parameter);
        }

        public long GetHeaSumMisuAmtByWrtnoSuDate(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SUM(MISUAMT) AS MISUAMT                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAP                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<long>(parameter);
        }

        public string GetHalinGyeByWrtnoSeqNo(long nWRTNO, long nSeqNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT HALINGYE                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAP                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND SEQNO = :SEQNO                          ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("SEQNO", nSeqNo);

            return ExecuteScalar<string>(parameter);
        }

        public string GetHeaMisuGyeByWrtnoSeqNo(long nWRTNO, long nSeqNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MISUGYE                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAP                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND SEQNO = :SEQNO                          ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("SEQNO", nSeqNo);

            return ExecuteScalar<string>(parameter);
        }

        public string GetHeaHalinGyeByWrtnoSeqNo(long nWRTNO, long nSeqNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT HALINGYE                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAP                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND SEQNO = :SEQNO                          ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("SEQNO", nSeqNo);

            return ExecuteScalar<string>(parameter);
        }

        public void InsertMinusSunapData(HIC_SUNAP item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SUNAP (                                                    ");
            parameter.AppendSql("       WRTNO,SUDATE,SEQNO,PANO,TOTAMT,HALINGYE,HALINAMT,JOHAPAMT,LTDAMT                ");
            parameter.AppendSql("      ,BONINAMT,BOGENAMT,MISUGYE,MISUAMT,SUNAPAMT,SUNAPAMT2,JOBSABUN,ENTTIME           ");
            parameter.AppendSql(" ) VALUES (                                                                            ");
            parameter.AppendSql("      :WRTNO,TRUNC(SYSDATE),:SEQNO,:PANO,:TOTAMT,:HALINGYE,:HALINAMT,:JOHAPAMT,:LTDAMT ");
            parameter.AppendSql("     ,:BONINAMT,:BOGENAMT,:MISUGYE,:MISUAMT,:SUNAPAMT,:SUNAPAMT2,:JOBSABUN,SYSDATE     ");
            parameter.AppendSql(" )                                                                                     ");

            parameter.Add("WRTNO",      item.WRTNO);
            parameter.Add("SEQNO",      item.SEQNO);
            parameter.Add("PANO",       item.PANO);
            parameter.Add("TOTAMT",     item.TOTAMT * -1);
            parameter.Add("HALINGYE",   item.HALINGYE);
            parameter.Add("HALINAMT",   item.HALINAMT * -1);
            parameter.Add("JOHAPAMT",   item.JOHAPAMT * -1);
            parameter.Add("LTDAMT",     item.LTDAMT * -1);
            parameter.Add("BONINAMT",   item.BONINAMT * -1);
            parameter.Add("BOGENAMT",   item.BOGENAMT * -1);
            parameter.Add("MISUGYE",    item.MISUGYE);
            parameter.Add("MISUAMT",    item.MISUAMT * -1);
            parameter.Add("SUNAPAMT",   item.SUNAPAMT * -1);
            parameter.Add("SUNAPAMT2",  item.SUNAPAMT2 * -1);
            parameter.Add("JOBSABUN",   clsType.User.IdNumber);
            
            ExecuteNonQuery(parameter);
        }

        public void InsertMinusSunapData_Hea(HIC_SUNAP item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_SUNAP (                                            ");
            parameter.AppendSql("       WRTNO,SUDATE,SEQNO,PANO,TOTAMT,HALINGYE,HALINAMT,LTDAMT                 ");
            parameter.AppendSql("      ,BONINAMT,MISUGYE,MISUAMT,SUNAPAMT1,SUNAPAMT2,JOBSABUN,ENTTIME           ");
            parameter.AppendSql(" ) VALUES (                                                                    ");
            parameter.AppendSql("      :WRTNO,TRUNC(SYSDATE),:SEQNO,:PANO,:TOTAMT,:HALINGYE,:HALINAMT,:LTDAMT   ");
            parameter.AppendSql("     ,:BONINAMT,:MISUGYE,:MISUAMT,:SUNAPAMT1,:SUNAPAMT2,:JOBSABUN,SYSDATE      ");
            parameter.AppendSql(" )                                                                             ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SEQNO", item.SEQNO);
            parameter.Add("PANO", item.PANO);
            parameter.Add("TOTAMT", item.TOTAMT * -1);
            parameter.Add("HALINGYE", item.HALINGYE);
            parameter.Add("HALINAMT", item.HALINAMT * -1);
            parameter.Add("LTDAMT", item.LTDAMT * -1);
            parameter.Add("BONINAMT", item.BONINAMT * -1);
            parameter.Add("MISUGYE", item.MISUGYE);
            parameter.Add("MISUAMT", item.MISUAMT * -1);
            parameter.Add("SUNAPAMT1", item.SUNAPAMT1 * -1);
            parameter.Add("SUNAPAMT2", item.SUNAPAMT2 * -1);
            parameter.Add("JOBSABUN", clsType.User.IdNumber);

            ExecuteNonQuery(parameter);
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SUNAP SET               ");
            parameter.AppendSql("       PANO = :PANO                            ");
            parameter.AppendSql(" WHERE PANO IN (SELECT PANO                    ");
            parameter.AppendSql("                  FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql("                 WHERE JUMIN2 = :JUMIN2        ");
            parameter.AppendSql("                   AND PANO <> :PANO)          ");

            parameter.Add("PANO", argPaNo);
            parameter.Add("JUMIN2", argJumin2);

            return ExecuteNonQuery(parameter);
        }

        public long GetMaxSeqbyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MAX(SeqNo) + 1 SEQ                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAP                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<long>(parameter);
        }

        public long GetHeaMaxSeqbyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MAX(SeqNo) + 1 SEQ                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAP                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<long>(parameter);
        }

        /// <summary>
        /// 검진비 수납내역 집계
        /// </summary>
        /// <param name="nGWRTNO"></param>
        /// <returns></returns>
        public IList<HIC_SUNAP> GetlistsByGWRTNO(long nGWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TOTAMT, HALINAMT, JOHAPAMT, LTDAMT, BOINAMT, MISUAMT, BOGENAMT, SUNAPAMT1, SUNAPAMT2, WRTNO, JEPBUN ");
            parameter.AppendSql("  FROM (                                                                                                   ");
            parameter.AppendSql("       SELECT SUM(TOTAMT) TOTAMT, SUM(HALINAMT) HALINAMT, SUM(JOHAPAMT) JOHAPAMT                           ");
            parameter.AppendSql("             ,SUM(LTDAMT) LTDAMT, SUM(BONINAMT) BOINAMT,  SUM(MISUAMT) MISUAMT                             ");
            parameter.AppendSql("             ,SUM(BOGENAMT) BOGENAMT                                                                       ");
            parameter.AppendSql("             ,SUM(SUNAPAMT) SUNAPAMT1, SUM(SUNAPAMT2) SUNAPAMT2                                            ");
            parameter.AppendSql("             ,A.WRTNO, B.JEPBUN                                                                            ");
            parameter.AppendSql("         FROM KOSMOS_PMPA.HIC_SUNAP A                                                                      ");
            parameter.AppendSql("             ,KOSMOS_PMPA.HIC_JEPSU B                                                                      ");
            parameter.AppendSql("        WHERE 1 = 1                                                                                        ");
            parameter.AppendSql("          AND A.GWRTNO = :GWRTNO                                                                           ");
            parameter.AppendSql("          AND A.WRTNO  = B.WRTNO                                                                           ");
            parameter.AppendSql("        GROUP By A.WRTNO, B.JEPBUN                                                                         ");
            parameter.AppendSql("        UNION ALL                                                                                          ");
            parameter.AppendSql("       SELECT SUM(TOTAMT) TOTAMT, SUM(HALINAMT) HALINAMT, 0 JOHAPAMT                                       ");
            parameter.AppendSql("             ,SUM(LTDAMT) LTDAMT, SUM(BONINAMT) BOINAMT,  SUM(MISUAMT) MISUAMT                             ");
            parameter.AppendSql("             ,0 BOGENAMT                                                                                   ");
            parameter.AppendSql("             ,SUM(SUNAPAMT1) SUNAPAMT1, SUM(SUNAPAMT2) SUNAPAMT2                                           ");
            parameter.AppendSql("             ,WRTNO, '4' AS JEPBUN                                                                         ");
            parameter.AppendSql("         FROM KOSMOS_PMPA.HEA_SUNAP                                                                        ");
            parameter.AppendSql("        WHERE GWRTNO = :GWRTNO                                                                             ");
            parameter.AppendSql("        GROUP By WRTNO                                                                                     ");
            parameter.AppendSql(" ) X                                                                                                       ");
            parameter.AppendSql(" ORDER By X.JEPBUN                                                                                         ");

            parameter.Add("GWRTNO", nGWRTNO);

            return ExecuteReader<HIC_SUNAP>(parameter);
        }

        public long GetSunapAmtbyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SUM(SunapAmt) SunapAmt                                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAP                                                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                          ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<long>(parameter);
        }
    }
}
