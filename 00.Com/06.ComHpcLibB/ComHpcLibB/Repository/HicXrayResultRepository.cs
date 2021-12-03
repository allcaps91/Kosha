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
    public class HicXrayResultRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicXrayResultRepository()
        {
        }

        
        public List<HIC_XRAY_RESULT> GetItemByPtnoJepDateList(string argPTNO, string argJepDate1, string argJepDate2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT XRAYNO,TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,GjJong,SName,Sex                               ");
            parameter.AppendSql("     , Result1,Result1_1,Result2,Result2_1,Result3,Result3_1                                       ");
            parameter.AppendSql("     , Result4,Result4_1,GbSTS,XCode,GbRead,GbChul,DelDate,ReadDoct1,ReadDoct2,GbPacs,PTNO,ROWID   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                                                                 ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                                                                ");
            parameter.AppendSql("   AND JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                   ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                   ");
            parameter.AppendSql("   AND DelDate IS NULL                                                                             ");
            parameter.AppendSql(" ORDER BY JepDate DESC, XrayNo                                                                     ");  

            parameter.Add("PTNO", argPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FRDATE", argJepDate1);
            parameter.Add("TODATE", argJepDate2);

            return ExecuteReader<HIC_XRAY_RESULT>(parameter);
        }

        public HIC_XRAY_RESULT GetItemByPtnoJepDate(string argPTNO, string argJepDate1, string argJepDate2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JEPDATE,XRAYNO,PANO,SNAME,SEX,AGE,GJJONG,GBCHUL,LTDCODE,XCODE,GBREAD                            ");
            parameter.AppendSql("      ,GBSTS, RESULT1, RESULT2, RESULT3, RESULT4, READTIME1, READDOCT1, READTIME2, READDOCT2           ");
            parameter.AppendSql("      ,GBPRINT, GBRESULTSEND, DELDATE, ENTSABUN, ENTTIME, READDATE, RESULT1_1, RESULT2_1               ");
            parameter.AppendSql("      ,RESULT3_1, RESULT4_1, GBORDER_SEND, GBPACS, PTNO, GBCONV, XRAYNO2, LTDCODE2, INPS, INPT_DT      ");
            parameter.AppendSql("      ,UPPS, UP_DT, EXID, PASTCT, PASTCTYYYY, PASTCTMM, NUDOYN_1, NUDOICON_1, NUDOSITE_1, NUDOSIZE1_1  ");
            parameter.AppendSql("      ,NUDOSIZE2_1, NUDOPOSITIVE_1, NUDOTRACECHK_1, NUDOTRACECHK2_1, NUDOYN_2, NUDOICON_2              ");
            parameter.AppendSql("      ,NUDOSITE_2, NUDOSIZE1_2, NUDOSIZE2_2, NUDOPOSITIVE_2, NUDOTRACECHK_2, NUDOTRACECHK2_2           ");
            parameter.AppendSql("      ,NUDOYN_3, NUDOICON_3, NUDOSITE_3, NUDOSIZE1_3, NUDOSIZE2_3, NUDOPOSITIVE_3, NUDOTRACECHK_3      ");
            parameter.AppendSql("      ,NUDOTRACECHK2_3, NUDOYN_4, NUDOICON_4, NUDOSITE_4, NUDOSIZE1_4, NUDOSIZE2_4, NUDOPOSITIVE_4     ");
            parameter.AppendSql("      ,NUDOTRACECHK_4, NUDOTRACECHK2_4, NUDOYN_5, NUDOICON_5, NUDOSITE_5, NUDOSIZE1_5                  ");
            parameter.AppendSql("      ,NUDOSIZE2_5, NUDOPOSITIVE_5, NUDOTRACECHK_5, NUDOTRACECHK2_5, NUDOYN_6, NUDOICON_6              ");
            parameter.AppendSql("      ,NUDOSITE_6, NUDOSIZE1_6, NUDOSIZE2_6, NUDOPOSITIVE_6, NUDOTRACECHK_6, NUDOTRACECHK2_6           ");
            parameter.AppendSql("      ,INDICATIOCHK, INDICATIOETC, SISACHK, SISAETC, NUDOMEAN1, NUDOMEAN2, NUDOMEAN2_1                 ");
            parameter.AppendSql("      ,NUDOMEAN3, NUDOMEAN4, NUDOMEAN5, NUDOMEAN6, NUDOMEAN7, NUDOMEAN8, NUDOMEAN9, NUDOMEAN9_9        ");
            parameter.AppendSql("      ,NUDOUNACTIVE, NUDOPANGU, NUDOPANGU2, SIZE1_1, SIZE1_2, GOSIZE1_1, GOSIZE1_2, IMAGENO_1          ");
            parameter.AppendSql("      ,NUDO4X_1, NUDO4XETC_1, CATEGORY1, SIZE2_1, SIZE2_2, GOSIZE2_1, GOSIZE2_2, IMAGENO_2             ");
            parameter.AppendSql("      ,NUDO4X_2, NUDO4XETC_2, CATEGORY2, SIZE3_1, SIZE3_2, GOSIZE3_1, GOSIZE3_2, IMAGENO_3             ");
            parameter.AppendSql("      ,NUDO4X_3, NUDO4XETC_3, CATEGORY3, SIZE4_1, SIZE4_2, GOSIZE4_1, GOSIZE4_2, IMAGENO_4             ");
            parameter.AppendSql("      ,NUDO4X_4, NUDO4XETC_4, CATEGORY4, SIZE5_1, SIZE5_2, GOSIZE5_1, GOSIZE5_2, IMAGENO_5             ");
            parameter.AppendSql("      ,NUDO4X_5, NUDO4XETC_5, CATEGORY5, SIZE6_1, SIZE6_2, GOSIZE6_1, GOSIZE6_2, IMAGENO_6             ");
            parameter.AppendSql("      ,NUDO4X_6, NUDO4XETC_6, CATEGORY6                                                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                                                                     ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                           ");
            parameter.AppendSql("   AND PTNO =:PTNO                                                                                    ");
            parameter.AppendSql("   AND JEPDATE >= TO_DATE(:JEPDATE1, 'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:JEPDATE2, 'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                                                 ");
            parameter.AppendSql(" ORDER BY JepDate DESC, XrayNo                                                                         ");

            parameter.Add("PTNO",     argPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE1", argJepDate1);
            parameter.Add("JEPDATE2", argJepDate2);

            return ExecuteReaderSingle<HIC_XRAY_RESULT>(parameter);
        }

        public int GetCountbyPaNoXrayNoJepDate(long nPano, string strXrayno, string strJobDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                         ");
            parameter.AppendSql(" WHERE PANO    = :PANO                                     ");
            parameter.AppendSql("   AND XRAYNO  = :XRAYNO                                   ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE,'YYYY-MM-DD')            ");

            parameter.Add("PANO", nPano);
            parameter.Add("XRAYNO", strXrayno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", strJobDate);

            return ExecuteScalar<int>(parameter);
        }

        public void UpDateGjJongPanoByRowid(string argGjJong, long argPano, string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_XRAY_RESULT SET     ");
            parameter.AppendSql("       GJJONG =:GJJONG                     ");
            parameter.AppendSql("      ,PANO =:PANO                         ");
            parameter.AppendSql(" WHERE ROWID = :RID                        ");

            parameter.Add("GJJONG", argGjJong);
            parameter.Add("PANO", argPano);
            parameter.Add("RID", argRowid);

            ExecuteNonQuery(parameter);
        }

        public string GetXrayNoByJepDate(string strDate, string strPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT XRAYNO                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT_WORK            ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");

            parameter.Add("PTNO", strPtNo);
            parameter.Add("JEPDATE", strDate);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_XRAY_RESULT> GetListItemByHeaPtno(string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PTNO, TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE, RESULT1, RESULT1_1,ROWID AS RID        ");
            parameter.AppendSql("     , RESULT2, RESULT2_1, RESULT3, RESULT3_1, RESULT4, RESULT4_1, READDOCT1, READDOCT2    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                                                         ");
            parameter.AppendSql(" WHERE PTNO IN ( SELECT PTNO FROM HEA_JEPSU WHERE SDATE =TO_DATE(:SDATE, 'YYYY-MM-DD') AND DELDATE IS NULL ) ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')                                             ");
            parameter.AppendSql("   AND GBREAD= '2'                                                                         ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                                     ");

            parameter.Add("SDATE", strDate);

            return ExecuteReader<HIC_XRAY_RESULT>(parameter);
        }

        public int SaveHicXrayResultWork(HIC_XRAY_RESULT item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("MERGE INTO KOSMOS_PMPA.HIC_XRAY_RESULT_WORK a      ");
            parameter.AppendSql("using dual d                                       ");
            parameter.AppendSql("   on (a.JEPDATE = TO_DATE(:JEPDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("  and  a.XCODE   = :XCODE                          ");
            parameter.AppendSql("  and  a.PANO    = :PANO)                          ");
            parameter.AppendSql(" when matched then                                 ");
            parameter.AppendSql("  update set                                       ");
            parameter.AppendSql("         XRAYNO  = :XRAYNO                         ");
            parameter.AppendSql("       , DELDATE = ''                              ");
            parameter.AppendSql("    when not matched then                          ");
            parameter.AppendSql("  insert                                           ");
            parameter.AppendSql("        (  JEPDATE                                 ");
            parameter.AppendSql("         , XRAYNO                                  ");
            parameter.AppendSql("         , PANO                                    ");
            parameter.AppendSql("         , PTNO                                    ");
            parameter.AppendSql("         , SNAME                                   ");
            parameter.AppendSql("         , SEX                                     ");
            parameter.AppendSql("         , AGE                                     ");
            parameter.AppendSql("         , GJJONG                                  ");
            parameter.AppendSql("         , GBCHUL                                  ");
            parameter.AppendSql("         , LTDCODE                                 ");
            parameter.AppendSql("         , XCODE                                   ");
            parameter.AppendSql("         , GBREAD                                  ");
            parameter.AppendSql("         , GBSTS                                   ");
            parameter.AppendSql("         , ENTSABUN                                ");
            parameter.AppendSql("         , ENTTIME                                 ");
            parameter.AppendSql("         , GBCONV)                                 ");
            parameter.AppendSql("  values                                           ");
            parameter.AppendSql("        ( TO_DATE(:JEPDATE, 'YYYY-MM-DD')          ");
            parameter.AppendSql("         , :XRAYNO                                 ");
            parameter.AppendSql("         , :PANO                                   ");
            parameter.AppendSql("         , :PTNO                                   ");
            parameter.AppendSql("         , :SNAME                                  ");
            parameter.AppendSql("         , :SEX                                    ");
            parameter.AppendSql("         , :AGE                                    ");
            parameter.AppendSql("         , :GJJONG                                 ");
            parameter.AppendSql("         , :GBCHUL                                 ");
            parameter.AppendSql("         , :LTDCODE                                ");
            parameter.AppendSql("         , :XCODE                                  ");
            parameter.AppendSql("         , :GBREAD                                 ");
            parameter.AppendSql("         , :GBSTS                                  ");
            parameter.AppendSql("         , :ENTSABUN                               ");
            parameter.AppendSql("         , SYSDATE                                 ");
            parameter.AppendSql("         , :GBCONV)                                ");

            parameter.Add("JEPDATE",   item.JEPDATE );
            parameter.Add("XRAYNO",    item.XRAYNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANO",      item.PANO    );
            parameter.Add("PTNO",      item.PTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SNAME",     item.SNAME   );
            parameter.Add("SEX",       item.SEX, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE",       item.AGE     );
            parameter.Add("GJJONG",   item.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBCHUL",    item.GBCHUL, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE",   item.LTDCODE );
            parameter.Add("XCODE",     item.XCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBREAD",    item.GBREAD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSTS",     item.GBSTS, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN",  item.ENTSABUN);
            parameter.Add("GBCONV",    item.GBCONV, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyJepDateXrayno(string strDate, string strXrayno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT_WORK            ");
            parameter.AppendSql(" WHERE XRAYNO = :XRAYNO                            ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");

            parameter.Add("XRAYNO", strXrayno);
            parameter.Add("JEPDATE", strDate);

            return ExecuteScalar<int>(parameter);
        }

        public string GetRowidByPtnoJepDate(string pTNO, string jEPDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID FROM KOSMOS_PMPA.HIC_XRAY_RESULT      ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND GBPACS = 'Y'                                ");

            parameter.Add("PTNO", pTNO);
            parameter.Add("JEPDATE", jEPDATE);

            return ExecuteScalar<string>(parameter);
        }

        public void UpDateDelDateNullByJepDatePtnoXrayNo(string argPano, string argDate, string argXrayNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_XRAY_RESULT SET             ");
            parameter.AppendSql("       DELDATE = ''                                ");
            parameter.AppendSql(" WHERE JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND PTNO = :PTNO                                ");
            parameter.AppendSql("   AND XCODE = 'A142'                              ");
            parameter.AppendSql("   AND XRAYNO = :XRAYNO                            ");
            parameter.AppendSql("   AND DELDATE IS NOT NULL                         ");

            parameter.Add("JEPDATE", argDate.Substring(0, 10));
            parameter.Add("PTNO", argPano);
            parameter.Add("XRAYNO", argXrayNo.To<string>(""));

            ExecuteNonQuery(parameter);
        }

        public string GetXrayNoByJepDatePtno(string argPano, string argDate, string argXCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT XRAYNO FROM KOSMOS_PMPA.HIC_XRAY_RESULT     ");
            parameter.AppendSql(" WHERE JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND PTNO = :PTNO                                ");
            parameter.AppendSql("   AND XCODE = :XCODE                              ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            if (argXCode == "TY10")
            {
                parameter.AppendSql("   AND GBPACS = 'Y'                            ");
            }

            parameter.Add("JEPDATE", argDate.Substring(0, 10));
            parameter.Add("PTNO", argPano);
            parameter.Add("XCODE", argXCode);

            return ExecuteScalar<string>(parameter);
        }

        public void UpDateDelDateByXrayNo(string argXrayNo, long argPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_XRAY_RESULT SET         ");
            parameter.AppendSql("       DELDATE = TRUNC(SYSDATE)                ");
            parameter.AppendSql(" WHERE XRAYNO =:XRAYNO                         ");
            parameter.AppendSql("   AND PANO =:PANO                             ");
            parameter.AppendSql("   AND (GBPACS IS NULL OR GBPACS <> 'Y')       ");

            parameter.Add("XRAYNO", argXrayNo);
            parameter.Add("PANO", argPano);
            
            ExecuteNonQuery(parameter);
        }

        public string GetRowidByXrayNo(string argXrayNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID FROM KOSMOS_PMPA.HIC_XRAY_RESULT  ");
            parameter.AppendSql(" WHERE XRAYNO = :XRAYNO                        ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND GBPACS = 'Y'                            ");

            parameter.Add("XRAYNO", argXrayNo);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_XRAY_RESULT SET         ");
            parameter.AppendSql("       PANO = :PANO                            ");
            parameter.AppendSql(" WHERE PANO IN (SELECT PANO                    ");
            parameter.AppendSql("                  FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql("                 WHERE JUMIN2 = :JUMIN2        ");
            parameter.AppendSql("                   AND PANO <> :PANO)          ");

            parameter.Add("PANO", argPaNo);
            parameter.Add("JUMIN2", argJumin2);

            return ExecuteNonQuery(parameter);
        }

        public HIC_XRAY_RESULT GetAllbyPtNoJepDate(string fstrPano, string fstrJepDate, string strXCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JEPDATE, XRAYNO, PANO, SNAME, SEX, AGE, GJJONG, GBCHUL, LTDCODE, XCODE, GBREAD  ");
            parameter.AppendSql("     , GBSTS, RESULT1, RESULT2, RESULT3, RESULT4, READTIME1, READDOCT1, READTIME2      ");
            parameter.AppendSql("     , READDOCT2, GBPRINT, GBRESULTSEND, DELDATE, ENTSABUN, ENTTIME, READDATE          ");
            parameter.AppendSql("     , RESULT1_1, RESULT2_1, RESULT3_1, RESULT4_1, GBORDER_SEND, GBPACS, PTNO          ");
            parameter.AppendSql("     , GBCONV, XRAYNO2, LTDCODE2, INPS, INPT_DT, UPPS, UP_DT, EXID, PASTCT             ");
            parameter.AppendSql("     , PASTCTYYYY, PASTCTMM, NUDOYN_1, NUDOICON_1, NUDOSITE_1, NUDOSIZE1_1             ");
            parameter.AppendSql("     , NUDOSIZE2_1, NUDOPOSITIVE_1, NUDOTRACECHK_1, NUDOTRACECHK2_1, NUDOYN_2          ");
            parameter.AppendSql("     , NUDOICON_2, NUDOSITE_2, NUDOSIZE1_2, NUDOSIZE2_2, NUDOPOSITIVE_2                ");
            parameter.AppendSql("     , NUDOTRACECHK_2, NUDOTRACECHK2_2, NUDOYN_3, NUDOICON_3, NUDOSITE_3               ");
            parameter.AppendSql("     , NUDOSIZE1_3, NUDOSIZE2_3, NUDOPOSITIVE_3, NUDOTRACECHK_3, NUDOTRACECHK2_3       ");
            parameter.AppendSql("     , NUDOYN_4, NUDOICON_4, NUDOSITE_4, NUDOSIZE1_4, NUDOSIZE2_4, NUDOPOSITIVE_4      ");
            parameter.AppendSql("     , NUDOTRACECHK_4, NUDOTRACECHK2_4, NUDOYN_5, NUDOICON_5, NUDOSITE_5               ");
            parameter.AppendSql("     , NUDOSIZE1_5, NUDOSIZE2_5, NUDOPOSITIVE_5, NUDOTRACECHK_5, NUDOTRACECHK2_5       ");
            parameter.AppendSql("     , NUDOYN_6, NUDOICON_6, NUDOSITE_6, NUDOSIZE1_6, NUDOSIZE2_6, NUDOPOSITIVE_6      ");
            parameter.AppendSql("     , NUDOTRACECHK_6, NUDOTRACECHK2_6, INDICATIOCHK, INDICATIOETC, SISACHK, SISAETC   ");
            parameter.AppendSql("     , NUDOMEAN1, NUDOMEAN2, NUDOMEAN2_1, NUDOMEAN3, NUDOMEAN4, NUDOMEAN5, NUDOMEAN6   ");
            parameter.AppendSql("     , NUDOMEAN7, NUDOMEAN8, NUDOMEAN9, NUDOMEAN9_9, NUDOUNACTIVE, NUDOPANGU           ");
            parameter.AppendSql("     , NUDOPANGU2, SIZE1_1, SIZE1_2, GOSIZE1_1, GOSIZE1_2, IMAGENO_1, NUDO4X_1         ");
            parameter.AppendSql("     , NUDO4XETC_1, CATEGORY1, SIZE2_1, SIZE2_2, GOSIZE2_1, GOSIZE2_2, IMAGENO_2       ");
            parameter.AppendSql("     , NUDO4X_2, NUDO4XETC_2, CATEGORY2, SIZE3_1, SIZE3_2, GOSIZE3_1, GOSIZE3_2        ");
            parameter.AppendSql("     , IMAGENO_3, NUDO4X_3, NUDO4XETC_3, CATEGORY3, SIZE4_1, SIZE4_2, GOSIZE4_1        ");
            parameter.AppendSql("     , GOSIZE4_2, IMAGENO_4, NUDO4X_4, NUDO4XETC_4, CATEGORY4, SIZE5_1, SIZE5_2        ");
            parameter.AppendSql("     , GOSIZE5_1, GOSIZE5_2, IMAGENO_5, NUDO4X_5, NUDO4XETC_5, CATEGORY5, SIZE6_1      ");
            parameter.AppendSql("     , SIZE6_2, GOSIZE6_1, GOSIZE6_2, IMAGENO_6, NUDO4X_6, NUDO4XETC_6, CATEGORY6      ");
            parameter.AppendSql("     , NUDOUNACTCHKETC, NUDOUNACTETCSOGEN, PASTCANCER, NUDOMAXRESULT, CTDOSE           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                                                     ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                                                    ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                       ");
            parameter.AppendSql("   AND XCODE   = :XCODE                                                                ");
            //parameter.AppendSql("   AND GbPacs  = '2'                                                                   ");

            parameter.Add("PTNO", fstrPano, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", fstrJepDate); 
            parameter.Add("XCODE", strXCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_XRAY_RESULT>(parameter);
        }

        public HIC_XRAY_RESULT GetAllbyPtNoJepDateXCode(string fstrPano, string fstrJepDate, string strXCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JEPDATE, XRAYNO, PANO, SNAME, SEX, AGE, GJJONG, GBCHUL, LTDCODE, XCODE, GBREAD  ");
            parameter.AppendSql("     , GBSTS, RESULT1, RESULT2, RESULT3, RESULT4, READTIME1, READDOCT1, READTIME2      ");
            parameter.AppendSql("     , READDOCT2, GBPRINT, GBRESULTSEND, DELDATE, ENTSABUN, ENTTIME, READDATE          ");
            parameter.AppendSql("     , RESULT1_1, RESULT2_1, RESULT3_1, RESULT4_1, GBORDER_SEND, GBPACS, PTNO          ");
            parameter.AppendSql("     , GBCONV, XRAYNO2, LTDCODE2, INPS, INPT_DT, UPPS, UP_DT, EXID, PASTCT             ");
            parameter.AppendSql("     , PASTCTYYYY, PASTCTMM, NUDOYN_1, NUDOICON_1, NUDOSITE_1, NUDOSIZE1_1             ");
            parameter.AppendSql("     , NUDOSIZE2_1, NUDOPOSITIVE_1, NUDOTRACECHK_1, NUDOTRACECHK2_1, NUDOYN_2          ");
            parameter.AppendSql("     , NUDOICON_2, NUDOSITE_2, NUDOSIZE1_2, NUDOSIZE2_2, NUDOPOSITIVE_2                ");
            parameter.AppendSql("     , NUDOTRACECHK_2, NUDOTRACECHK2_2, NUDOYN_3, NUDOICON_3, NUDOSITE_3               ");
            parameter.AppendSql("     , NUDOSIZE1_3, NUDOSIZE2_3, NUDOPOSITIVE_3, NUDOTRACECHK_3, NUDOTRACECHK2_3       ");
            parameter.AppendSql("     , NUDOYN_4, NUDOICON_4, NUDOSITE_4, NUDOSIZE1_4, NUDOSIZE2_4, NUDOPOSITIVE_4      ");
            parameter.AppendSql("     , NUDOTRACECHK_4, NUDOTRACECHK2_4, NUDOYN_5, NUDOICON_5, NUDOSITE_5               ");
            parameter.AppendSql("     , NUDOSIZE1_5, NUDOSIZE2_5, NUDOPOSITIVE_5, NUDOTRACECHK_5, NUDOTRACECHK2_5       ");
            parameter.AppendSql("     , NUDOYN_6, NUDOICON_6, NUDOSITE_6, NUDOSIZE1_6, NUDOSIZE2_6, NUDOPOSITIVE_6      ");
            parameter.AppendSql("     , NUDOTRACECHK_6, NUDOTRACECHK2_6, INDICATIOCHK, INDICATIOETC, SISACHK, SISAETC   ");
            parameter.AppendSql("     , NUDOMEAN1, NUDOMEAN2, NUDOMEAN2_1, NUDOMEAN3, NUDOMEAN4, NUDOMEAN5, NUDOMEAN6   ");
            parameter.AppendSql("     , NUDOMEAN7, NUDOMEAN8, NUDOMEAN9, NUDOMEAN9_9, NUDOUNACTIVE, NUDOPANGU           ");
            parameter.AppendSql("     , NUDOPANGU2, SIZE1_1, SIZE1_2, GOSIZE1_1, GOSIZE1_2, IMAGENO_1, NUDO4X_1         ");
            parameter.AppendSql("     , NUDO4XETC_1, CATEGORY1, SIZE2_1, SIZE2_2, GOSIZE2_1, GOSIZE2_2, IMAGENO_2       ");
            parameter.AppendSql("     , NUDO4X_2, NUDO4XETC_2, CATEGORY2, SIZE3_1, SIZE3_2, GOSIZE3_1, GOSIZE3_2        ");
            parameter.AppendSql("     , IMAGENO_3, NUDO4X_3, NUDO4XETC_3, CATEGORY3, SIZE4_1, SIZE4_2, GOSIZE4_1        ");
            parameter.AppendSql("     , GOSIZE4_2, IMAGENO_4, NUDO4X_4, NUDO4XETC_4, CATEGORY4, SIZE5_1, SIZE5_2        ");
            parameter.AppendSql("     , GOSIZE5_1, GOSIZE5_2, IMAGENO_5, NUDO4X_5, NUDO4XETC_5, CATEGORY5, SIZE6_1      ");
            parameter.AppendSql("     , SIZE6_2, GOSIZE6_1, GOSIZE6_2, IMAGENO_6, NUDO4X_6, NUDO4XETC_6, CATEGORY6      ");
            parameter.AppendSql("     , NUDOUNACTCHKETC, NUDOUNACTETCSOGEN, PASTCANCER, NUDOMAXRESULT, CTDOSE           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                                                     ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                                                    ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                       ");
            parameter.AppendSql("   AND XCODE   = :XCODE                                                                ");

            parameter.Add("PTNO", fstrPano, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", fstrJepDate);
            parameter.Add("XCODE", strXCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_XRAY_RESULT>(parameter);
        }

        public int GetCountbyPtNoXCode(string strPtNo, string strXCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(*)                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT     ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                    ");
            parameter.AppendSql("   AND JEPDATE = TRUNC(SYSDATE)        ");
            parameter.AppendSql("   AND XCODE   = :XCODE                ");
            parameter.AppendSql("   AND GbPacs  = 'Y'                   ");
            parameter.AppendSql(" ORDER BY JepDate DESC, XrayNo         ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("XCODE", strXCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int Update_Patient_Exid(string strSabun, string strRowId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_XRAY_RESULT SET     ");
            parameter.AppendSql("       EXID  = :EXID                       ");
            parameter.AppendSql(" WHERE ROWID = :ROWID                      ");

            parameter.Add("EXID", strSabun);
            parameter.Add("ROWID", strRowId);

            return ExecuteNonQuery(parameter);
        }

        public HIC_XRAY_RESULT GetItembyJepDatePtNo(string strJepDate, string fstrPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PANO,SNAME,TO_CHAR(READTIME1,'YYYY-MM-DD') READDATE             ");
            parameter.AppendSql("     , TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE, SEX,AGE,READDOCT1        ");
            parameter.AppendSql("     , READDOCT2, GBSTS, XCODE,RESULT1,RESULT2,RESULT3,RESULT4, ROWID  ");
            parameter.AppendSql("     , RESULT1_1, RESULT2_1, RESULT3_1, RESULT4_1                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                                     ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:JEPDATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("   AND PTNO = :PTNO                                                    ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                 ");
            parameter.AppendSql(" ORDER BY JEPDATE                                                      ");

            parameter.Add("PTNO", fstrPano, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", strJepDate);

            return ExecuteReaderSingle<HIC_XRAY_RESULT>(parameter);
        }

        public string GetXrayNobyPtNoJepDate(string pTNO, string jEPDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT XRayNo FROM KOSMOS_PMPA.HIC_XRAY_RESULT     ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");

            parameter.Add("PTNO", pTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", jEPDATE);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateGbResultSendbyRowId(string rOWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_XRAY_RESULT ");
            parameter.AppendSql("   SET GBRESULTSEND = 'Y'          ");
            parameter.AppendSql(" WHERE ROWID        = :RID         ");

            parameter.Add("RID", rOWID);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_XRAY_RESULT> GetItembyReadTime1(string strGTime)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Pano,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,XRayNo           ");
            parameter.AppendSql("     , Result1,Result2,Result3,Result4, ROWID                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                                 ");
            parameter.AppendSql(" WHERE ReadDate>=TRUNC(SYSDATE-10)                                 ");
            parameter.AppendSql("   AND DelDate IS NULL                                             ");
            parameter.AppendSql("   AND GbResultSend IS NULL                                        ");
            parameter.AppendSql("   AND GbSTS = '2'                                                 ");     //판독완료
            parameter.AppendSql("   AND RESULT1 IN ('01','02')                                      ");     //정상
            parameter.AppendSql("   AND READTIME1 <= TO_DATE(:READTIME1, 'YYYY-MM-DD HH24:MI')      ");

            parameter.Add("READTIME1", strGTime);

            return ExecuteReader<HIC_XRAY_RESULT>(parameter);
        }

        public List<HIC_XRAY_RESULT> GetListItemByPtnoJepDate(string fstrPtno, string fstrJepDate, string strExCode = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PANO,SNAME,TO_CHAR(READTIME1,'YYYY-MM-DD') READDATE             ");
            parameter.AppendSql("     , TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE, SEX,AGE,READDOCT1        ");
            parameter.AppendSql("     , READDOCT2, GBSTS, XCODE,RESULT1,RESULT2,RESULT3,RESULT4, ROWID  ");
            parameter.AppendSql("     , RESULT1_1, RESULT2_1, RESULT3_1, RESULT4_1                      ");
            parameter.AppendSql("     , TO_CHAR(READTIME1,'YYYY-MM-DD HH24:MI') READDATE1               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                                     ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                                    ");
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                       ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                 ");

            if (!strExCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND XCODE = :XCODE                                                  ");
            }

            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", fstrJepDate);
            if (!strExCode.IsNullOrEmpty())
            {
                parameter.Add("XCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_XRAY_RESULT>(parameter);
        }

        public HIC_XRAY_RESULT GetItembyRowId(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Pano,SName,TO_CHAR(ReadTime1,'YYYY-MM-DD') ReadDate,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate   ");
            parameter.AppendSql("     , Sex,Age,ReadDoct1,ReadDoct2,GbSTS                                                           ");
            parameter.AppendSql("     , XCode,Result1,Result2,Result3,Result4,Result1_1,Result2_1,Result3_1,Result4_1               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                                                                 ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                                                ");
            parameter.AppendSql("   AND DelDate is NULL                                                                             ");

            parameter.Add("RID", strROWID);

            return ExecuteReaderSingle<HIC_XRAY_RESULT>(parameter);
        }

        public HIC_XRAY_RESULT GetXrayNobyPtNo(string pTNO, string text)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT XRAYNO                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                 ");
            parameter.AppendSql(" WHERE JEPDATE = TO_DATE(:JEPDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND PTNO    = :PTNO                             ");

            parameter.Add("PTNO", pTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", text);

            return ExecuteReaderSingle<HIC_XRAY_RESULT>(parameter);
        }

        public int Insert(HIC_XRAY_RESULT item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_XRAY_RESULT                                                ");
            parameter.AppendSql("       (JEPDATE, XRAYNO, PANO, SNAME, SEX, AGE, GJJONG, GBCHUL, LTDCODE, XCODE         ");
            parameter.AppendSql("       ,GBREAD, GBSTS, GBORDER_SEND, GBPACS, GBCONV, PTNO, ENTTIME, ENTSABUN,EXID)     ");
            parameter.AppendSql("VALUES                                                                                 ");
            parameter.AppendSql("       (TO_DATE(:JEPDATE, 'YYYY-MM-DD'), :XRAYNO, :PANO, :SNAME, :SEX, :AGE            ");
            parameter.AppendSql("     , :GJJONG, :GBCHUL, :LTDCODE, :XCODE, :GBREAD, :GBSTS, :GBORDER_SEND              ");
            parameter.AppendSql("     , :GBPACS, :GBCONV, :PTNO, SYSDATE, :ENTSABUN, :EXID)                             ");

            parameter.Add("JEPDATE", item.JEPDATE);
            parameter.Add("XRAYNO", item.XRAYNO);
            parameter.Add("PANO", item.PANO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX);
            parameter.Add("AGE", item.AGE);
            parameter.Add("GJJONG", item.GJJONG);
            parameter.Add("GBCHUL", item.GBCHUL);
            parameter.Add("LTDCODE", item.LTDCODE);
            parameter.Add("XCODE", item.XCODE);
            parameter.Add("GBREAD", item.GBREAD);
            parameter.Add("GBSTS", item.GBSTS);
            parameter.Add("GBORDER_SEND", item.GBORDER_SEND);
            parameter.Add("GBPACS", item.GBPACS);
            parameter.Add("GBCONV", item.GBCONV);
            parameter.Add("PTNO", item.PTNO);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("EXID", item.EXID);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyPtNoPaNo(string text, string strJepDate, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                 ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            if (strGubun == "1")
            {
                parameter.AppendSql("   AND PANO    = :PANO                         ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql("   AND PTNO    = :PANO                         ");
            }
            parameter.AppendSql("   AND JEPDATE = TO_DATE(:JEPDATE,'YYYY-MM-DD')    "); 
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            parameter.AppendSql("   AND GJJONG = '83'                               ");
            parameter.AppendSql("   AND GBREAD = '2'                                ");

            parameter.Add("PANO", text, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", strJepDate);

            return ExecuteScalar<int>(parameter);
        }

        public HIC_XRAY_RESULT GetItembyPaNo(long nPano, string jEPDATE, string strXrayNo = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBPACS, READDOCT1, TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE, XRAYNO    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                                         ");
            parameter.AppendSql(" WHERE JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                           ");
            parameter.AppendSql("   AND PANO    = :PANO                                                     ");
            if (!strXrayNo.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND XRAYNO    = :XRAYNO                                             ");
            }

            parameter.Add("PANO", nPano);
            parameter.Add("JEPDATE", jEPDATE);
            if (!strXrayNo.IsNullOrEmpty())
            {
                parameter.Add("XRAYNO", strXrayNo);
            }

            return ExecuteReaderSingle<HIC_XRAY_RESULT>(parameter);
        }

        public int GetCountbyPaNo(long fnPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT     ");
            parameter.AppendSql(" WHERE JEPDATE = TRUNC(SYSDATE)        ");
            parameter.AppendSql("   AND PANO    = :PANO                 ");
            parameter.AppendSql("   AND GbPACS  = 'Y'                   ");

            parameter.Add("PANO", fnPano);

            return ExecuteScalar<int>(parameter);
        }

        public int GetEndoCountByPtnoJepDate(string argPTNO, string argJepDate1, string argJepDate2, string argGbJob)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ENDO_JUPMST                      ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                ");                
            parameter.AppendSql("   AND RDATE >= TO_DATE(:RDATE1,'YYYY-MM-DD')      ");
            parameter.AppendSql("   AND RDATE <  TO_DATE(:RDATE2,'YYYY-MM-DD')      ");
            parameter.AppendSql("   AND GBJOB = :GBJOB                              ");//2:위, 3:대장
            parameter.AppendSql("   AND GBSUNAP IN ('1','7')                        ");//접수만-취소제외
            parameter.AppendSql("   AND PACSUID IS NOT NULL                         ");

            parameter.Add("PTNO", argPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RDATE1", argJepDate1);
            parameter.Add("RDATE2", argJepDate2);
            parameter.Add("GBJOB", argGbJob, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteScalar<int>(parameter);
        }

        public string GetXrayNobyXrayNo(string strXrayno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT XRAYNO                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT ");
            parameter.AppendSql(" WHERE XRAYNO = :XRAYNO            ");

            parameter.Add("XRAYNO", strXrayno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int GetXrayCountByPtnoJepDate(string argPTNO, string argJepDate1, string argJepDate2, string strXCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT count('X') cnt                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                 ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            parameter.AppendSql("   AND PTNO =:PTNO                                ");
            parameter.AppendSql("   AND JEPDATE >= to_date(:JEPDATE1, 'yyyy-MM-dd') ");
            parameter.AppendSql("   AND JEPDATE <= to_date(:JEPDATE2, 'yyyy-MM-dd') ");
            parameter.AppendSql("   AND XCODE = :XCODE                              ");
            parameter.AppendSql("   AND XSENDDATE IS NOT NULL                       ");

            parameter.Add("PTNO", argPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE1", argJepDate1);
            parameter.Add("JEPDATE2", argJepDate2);
            parameter.Add("XCODE", strXCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteScalar<int>(parameter);
        }

        public string GetEndoResultDateByPtno(string argPTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT to_char(RESULTDATE, 'yyyy-MM-dd') RESULTDATE    ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ENDO_JUPMST                          ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                    ");
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')                         ");
            parameter.AppendSql("   AND JDATE >= TRUNC(SYSDATE - 2)                     ");

            parameter.Add("PTNO", argPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_XRAY_RESULT> GetItemByJepDate(string argJepDate1, string argJepDate2, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JepDate,'YYYY-MM-DD') JepDate                                                           ");
            parameter.AppendSql("      ,DECODE(GbChul,'Y','출장','내원') GbChul,LtdCode,COUNT(*) CNT                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                                                                     ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                           ");
            parameter.AppendSql("   AND JEPDATE >= TO_DATE(:JEPDATE1, 'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:JEPDATE2, 'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                                                 ");

            if (argGubun == "내원")
            {
                parameter.AppendSql("   AND GbChul ='N'                                                                                 ");
            }
            else if (argGubun == "출장")
            {
                parameter.AppendSql("   AND GbChul ='Y'                                                                                 ");
            }

            parameter.AppendSql(" GROUP BY JepDate,GbChul,LtdCode                                                                       ");

            parameter.Add("JEPDATE1", argJepDate1);
            parameter.Add("JEPDATE2", argJepDate2);

            return ExecuteReader<HIC_XRAY_RESULT>(parameter);
        }

        public HIC_XRAY_RESULT GetItemByPanoXrayno(long argPano, string argXrayno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JepDate,'YYYYMMDD') JepDate,XCode,Exid,GBREAD   ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_XRAY_RESULT                              ");
            parameter.AppendSql(" WHERE PANO = :PANO                                            ");
            parameter.AppendSql(" AND XRayNo = :XRAYNO                                          ");
            parameter.AppendSql(" ORDER BY JEPDATE DESC                                         ");

            parameter.Add("PANO", argPano);
            parameter.Add("XRAYNO", argXrayno);

            return ExecuteReaderSingle<HIC_XRAY_RESULT>(parameter);
        }

        public HIC_XRAY_RESULT GetItemByPtnoXrayno(string argPtno, string argXrayno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,SName,Sex,Age,XCode  ");
            parameter.AppendSql(" ,TO_CHAR(ReadTime1,'YYYY-MM-DD HH24:MI') ReadTime1                ");
            parameter.AppendSql(" ,TO_CHAR(ReadTime2,'YYYY-MM-DD HH24:MI') ReadTime2                ");
            parameter.AppendSql(" ,GbRead,Result1,Result2,Result3,Result4,ReadDoct1,ReadDoct2       ");
            parameter.AppendSql(" ,Result1_1,Result2_1,Result3_1,Result4_1                          ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_XRAY_RESULT                                  ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                                ");
            parameter.AppendSql(" AND XRayNo = :XRAYNO                                              ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("XRAYNO", argXrayno);

            return ExecuteReaderSingle<HIC_XRAY_RESULT>(parameter);
        }

        public void UpDateGbOrderSendByPanoXrayNo(long argPano, string argXrayNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_XRAY_RESULT SET         ");
            parameter.AppendSql(" GBORDER_SEND = 'Y'                            ");
            parameter.AppendSql(" WHERE PANO =:PANO                             ");
            parameter.AppendSql(" AND XRAYNO =:XRAYNO                           ");

            parameter.Add("PANO", argPano);
            parameter.Add("XRAYNO", argXrayNo);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_XRAY_RESULT> GetItemByJepDateLtdCodeGubun( string argFdate, string argTDate, long argLtdCode, string argGubun, string argSname)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT LTDCODE,GJJONG,GBREAD,TO_CHAR(READDATE,'YYYY-MM-DD') READDATE          ");
            parameter.AppendSql("     , READDOCT1,READDOCT2,COUNT(*) CNT                                        ");
            parameter.AppendSql("     , TO_CHAR(MIN(JEPDATE),'YYYY-MM-DD') MINDATE                              ");
            parameter.AppendSql("     , TO_CHAR(MAX(JEPDATE),'YYYY-MM-DD') MAXDATE                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                                             ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                 ");
            parameter.AppendSql(" AND JEPDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                                  ");
            parameter.AppendSql(" AND GBSTS = '2'                                                               ");
            parameter.AppendSql(" AND DELDATE IS NULL                                                           ");
            if(argGubun == "Y")
            {
                parameter.AppendSql(" AND GBPRINT = 'Y'                         ");
            }
            else
            {
                parameter.AppendSql(" AND (GBPRINT IS NULL OR GBPRINT <> 'Y')   ");
            }

            if (!argSname.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND SNAME = :SNAME                        ");
            }
            if (argLtdCode>0)
            {
                parameter.AppendSql(" AND LtdCode = :LTDCODE                    ");
            }

            parameter.AppendSql(" GROUP BY LtdCode,GjJong,GbRead,ReadDate,ReadDoct1,ReadDoct2                   ");
            parameter.AppendSql(" ORDER BY LtdCode,GjJong,GbRead,ReadDate,ReadDoct1,ReadDoct2                   ");

            parameter.Add("FDATE", argFdate);
            parameter.Add("TDATE", argTDate);

            if(!argSname.IsNullOrEmpty())
            {
                parameter.Add("SNAME", argSname);
            }

            if (argLtdCode > 0)
            {
                parameter.Add("LTDCODE", argLtdCode);
            }

            return ExecuteReader<HIC_XRAY_RESULT>(parameter);
        }

        public List<HIC_XRAY_RESULT> GetItemByJepDateLtdCodeDocJongGubun(string argFdate, string argTDate, long argLtdCode, string argGbRead, string argJong, long argReadDoc1, long argReadDoc2, string argGubun, string argReadDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT *                                                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT                                             ");
            parameter.AppendSql(" WHERE READDATE = TO_DATE(:READDATE, 'YYYY-MM-DD')                             ");
            parameter.AppendSql(" AND JEPDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                   ");
            parameter.AppendSql(" AND JEPDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                                  ");
            parameter.AppendSql(" AND GBSTS = '2'                                                               ");
            parameter.AppendSql(" AND DELDATE IS NULL                                                           ");
            parameter.AppendSql(" AND GBREAD = :GBREAD                                                          ");
            parameter.AppendSql(" AND GJJONG = :GJJONG                                                          ");
            parameter.AppendSql(" AND READDOCT1 = :READDOCT1                                                    ");
            if(!argReadDoc2.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND READDOCT2 = :READDOCT2                 ");
            }
            if (argLtdCode > 0)
            {
                parameter.AppendSql(" AND LtdCode = :LTDCODE                    ");
            }
            if (argGubun == "Y")
            {
                parameter.AppendSql(" AND GBPRINT = 'Y'                         ");
            }
            else
            {
                parameter.AppendSql(" AND (GBPRINT IS NULL OR GBPRINT <> 'Y'    ");
            }

            parameter.AppendSql(" GROUP BY LtdCode,GjJong,GbRead,ReadDate,ReadDoct1,ReadDoct2                   ");
            parameter.AppendSql(" ORDER BY LtdCode,GjJong,GbRead,ReadDate,ReadDoct1,ReadDoct2                   ");

            parameter.Add("READDATE", argReadDate); 
            parameter.Add("FDATE", argFdate);
            parameter.Add("TDATE", argTDate);
            parameter.Add("GBREAD", argGbRead);
            parameter.Add("GJJONG", argJong);
            parameter.Add("READDOCT1", argReadDoc1);
            if (!argReadDoc2.IsNullOrEmpty())
            {
                parameter.Add("READDOCT2", argReadDoc2);
            }

            if (argLtdCode > 0)
            {
                parameter.Add("LTDCODE", argLtdCode);
            }

            return ExecuteReader<HIC_XRAY_RESULT>(parameter);
        }

        public int UpdateGbPrintByXrayNo(string argXrayNo, string argGbPrint)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_XRAY_RESULT     ");
            parameter.AppendSql("   SET GBPRINT = :GBPRINT              ");
            parameter.AppendSql(" WHERE XRAYNO = :XRAYNO                ");

            parameter.Add("XRAYNO", argXrayNo);
            parameter.Add("GBPRINT", argGbPrint);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateGbPrintByXrayNo1(List<string> argXrayNo, string argGbPrint)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_XRAY_RESULT     ");
            parameter.AppendSql("   SET GBPRINT = :GBPRINT              ");
            parameter.AppendSql(" WHERE XRAYNO IN (:XRAYNO)             ");

            parameter.AddInStatement("XRAYNO", argXrayNo);
            parameter.Add("GBPRINT", argGbPrint);

            return ExecuteNonQuery(parameter);
        }

        public HIC_XRAY_RESULT GetMaxMinXrayNoByXrayNo(List<string> argXrayNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MIN(XRAYNO) MINXRAY, MAX(XRAYNO) MAXXRAY    ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_XRAY_RESULT                  ");
            parameter.AppendSql(" WHERE XRAYNO IN (:XRAYNO)                            ");
            parameter.AppendSql(" AND DELDATE IS NULL                               ");

            parameter.AddInStatement("XRAYNO", argXrayNo);

            return ExecuteReaderSingle<HIC_XRAY_RESULT>(parameter);
        }

        public HIC_XRAY_RESULT GetReadDoctByXrayNo(List<string> argXrayNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT READDOCT1, READDOCT2                        ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_XRAY_RESULT                  ");
            parameter.AppendSql(" WHERE XRAYNO IN (:XRAYNO)                            ");
            parameter.AppendSql(" AND DELDATE IS NULL                               ");
            parameter.AppendSql(" GROUP BY READDOCT1, READDOCT2                     ");

            parameter.AddInStatement("XRAYNO", argXrayNo);

            return ExecuteReaderSingle<HIC_XRAY_RESULT>(parameter);
        }

        public List<HIC_XRAY_RESULT> GetItemByXrayNo(List<string> argXrayNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SName,XrayNo,Pano,Result2,Result4, GbPrint          ");
            parameter.AppendSql(" , ReadDoct1, ReadDoct2,GbRead                             ");
            parameter.AppendSql(" , Result2_1, Result4_1, SEX, AGE, PTNO                    ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_XRAY_RESULT                          ");
            parameter.AppendSql(" WHERE XRAYNO IN (:XRAYNO)                                 ");
            parameter.AppendSql(" AND DELDATE IS NULL                                       ");
            parameter.AppendSql(" ORDER BY XRAYNO                                           ");

            parameter.AddInStatement ("XRAYNO", argXrayNo);

            return ExecuteReader<HIC_XRAY_RESULT>(parameter);
        }


        public List<HIC_XRAY_RESULT> GetItemByJepdateJongLtd(string argFDate, string argTDate, string argGjJong, long argLtdCode, string argGbsts, string argGbChul)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT XRAYNO,GJJONG,LTDCODE,SNAME,RESULT1,RESULT1_1,RESULT2,RESULT2_1             ");
            parameter.AppendSql(" ,RESULT3,RESULT3_1, RESULT4,RESULT4_1,AGE,SEX,GBSTS,XCODE,GBREAD,GBCHUL           ");
            parameter.AppendSql(" ,DELDATE,READDOCT1,READDOCT2,GBPACS,PTNO,ROWID AS RID                             ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_XRAY_RESULT                                                  ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                     ");
            parameter.AppendSql(" AND JEPDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                                      ");
            parameter.AppendSql(" AND DELDATE IS NULL                                                               ");
            parameter.AppendSql(" AND GBPACS = 'Y'                                                                  ");
            if (argGjJong != "**")
            {
                parameter.AppendSql(" AND GJJONG = :GJJONG                                                              ");
            }
            if (argLtdCode > 0 )
            {
                parameter.AppendSql(" AND LTDCODE = :LTDCODE                                                            ");
            }

            if (!argGbChul.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND GBCHUL = :GBCHUL                                                            ");
            }
            if (!argGbsts.IsNullOrEmpty())
            {
                if(argGbsts == "2")
                {
                    parameter.AppendSql(" AND GBSTS IN ('0','1')                                                      ");
                }
                else if (argGbsts == "3")
                {
                    parameter.AppendSql(" AND GBSTS IN ('2')                                                          ");
                }
                else if (argGbsts == "4")
                {
                    parameter.AppendSql(" AND GBSTS IN ('P')                                                          ");
                }
            }


            parameter.AppendSql(" ORDER BY JEPDATE, XRAYNO                                                          ");

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);
            if (argGjJong != "**")
            {
                parameter.Add("GJJONG", argGjJong);
            }
            if (argLtdCode > 0)
            {
                parameter.Add("LTDCODE", argLtdCode);
            }

            if (!argGbChul.IsNullOrEmpty())
            {
                parameter.Add("GBCHUL", argGbChul);
            }
            

            return ExecuteReader<HIC_XRAY_RESULT>(parameter);
        }

        public int UpdateGbReadByXrayNo(string argGbRead, string argXrayNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_XRAY_RESULT     ");
            parameter.AppendSql("   SET GBREAD = :GBREAD              ");
            parameter.AppendSql(" WHERE XRAYNO = :XRAYNO             ");

            parameter.Add("GBREAD", argGbRead);
            parameter.Add("XRAYNO", argXrayNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateGbChulByXrayNo(string argGbChul, string argXrayNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_XRAY_RESULT     ");
            parameter.AppendSql("   SET GBCHUL = :GBCHUL              ");
            parameter.AppendSql(" WHERE XRAYNO = :XRAYNO             ");

            parameter.Add("GBCHUL", argGbChul);
            parameter.Add("XRAYNO", argXrayNo);

            return ExecuteNonQuery(parameter);
        }
    }
}
