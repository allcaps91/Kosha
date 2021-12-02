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
    public class HeaResvExamRepository : BaseRepository
    {        
        /// <summary>
        /// 
        /// </summary>
        public HeaResvExamRepository()
        {
        }

        public string Read_Hes_Resv_Exam(string SDATE, long PANO, string CODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM               ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'yyyy-MM-dd')   ");
            parameter.AppendSql("   AND PANO   = :PANO                          ");
            parameter.AppendSql("   AND EXCODE = :EXCODE                        ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");

            parameter.Add("SDATE", SDATE);
            parameter.Add("PANO", PANO);
            parameter.Add("EXCODE", CODE, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int GetCountbyPano(string sFrDate, string sToDate, string sGbExam, long nPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                   ");
            parameter.AppendSql(" WHERE RTIME >= TO_DATE(:FRDATE, 'YYYY-MM-DD')     ");
            parameter.AppendSql("   AND RTIME <  TO_DATE(:TODATE, 'YYYY-MM-DD')     ");
            if (nPano > 0)
            {
                parameter.AppendSql("   AND PANO =:PANO     ");
            }
            parameter.AppendSql("   AND GBEXAM = :GBEXAM                            ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");
            parameter.AppendSql(" ORDER BY SNAME                                    ");

            parameter.Add("FRDATE", sFrDate);
            parameter.Add("TODATE", sToDate);
            if (nPano > 0)
            {
                parameter.Add("PANO", nPano);
            }
            parameter.Add("GBEXAM", sGbExam, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public List<HEA_RESV_EXAM> GetItembyRTimeGbExam(string[] strGbExam)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(RTime,'YYYY-MM-DD') RDate,Pano,GbExam   ");
            parameter.AppendSql("     , TO_CHAR(SDate,'YYYY-MM-DD') SDate               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                       ");
            parameter.AppendSql(" WHERE RTime >= TRUNC(SYSDATE)                         ");
            parameter.AppendSql("   AND RTime <= TRUNC(SYSDATE+1)                       ");
            parameter.AppendSql("   AND SDate < TRUNC(SYSDATE)                          "); 
            parameter.AppendSql("   AND GBEXAM IN (:GBEXAM)                             ");

            parameter.Add("GBEXAM", strGbExam);

            return ExecuteReader<HEA_RESV_EXAM>(parameter);
        }

        public int GetCountbyPaNoGbExam(long argPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND GBEXAM IN ('01','02')                           ");
            parameter.AppendSql("   AND RTIME >= TRUNC(SYSDATE)                         ");
            parameter.AppendSql("   AND RTIME < TRUNC(SYSDATE) + 1                      ");
            parameter.AppendSql("   AND DELDATE IS NULL                                 ");

            parameter.Add("PANO", argPano);

            return ExecuteScalar<int>(parameter);
        }

        public void Insert(HEA_RESV_EXAM code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_RESV_EXAM (                        ");
            parameter.AppendSql("       RTIME,PANO,SNAME,GBEXAM,EXAMNAME,SDATE                  ");
            parameter.AppendSql("      ,ENTSABUN,ENTTIME,EXCODE,AMPM                            ");
            parameter.AppendSql(" ) VALUES (                                                    ");
            parameter.AppendSql("       TO_DATE(:RTIME, 'YYYY-MM-DD HH24:MI'),:PANO             ");
            parameter.AppendSql("      ,:SNAME,:GBEXAM,:EXAMNAME,TO_DATE(:SDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("      ,:ENTSABUN,SYSDATE,:EXCODE,:AMPM                         ");
            parameter.AppendSql(" )                                                             ");

            parameter.Add("RTIME",      code.RTIME);
            parameter.Add("PANO",       code.PANO);
            parameter.Add("SNAME",      code.SNAME);
            parameter.Add("GBEXAM",     code.GBEXAM);
            parameter.Add("EXAMNAME",   code.EXAMNAME);
            parameter.Add("SDATE",      code.SDATE);
            parameter.Add("ENTSABUN",   code.ENTSABUN);
            parameter.Add("EXCODE",     code.EXCODE);
            parameter.Add("AMPM",       code.AMPM);

            ExecuteNonQuery(parameter);
        }

        public string GetRTimeByPanoSDateGubun1(long nPano, string strSDate, string strGubn1)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(a.RTIME,'YYYY-MM-DD HH24:MI') RTIME             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM a                             ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_CODE b                                  ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                          ");
            parameter.AppendSql("   AND a.SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')                 ");
            parameter.AppendSql("   AND b.GUBUN = '13'                                          ");
            parameter.AppendSql("   AND a.GBEXAM = b.GUBUN2                                     ");
            parameter.AppendSql("   AND b.GUBUN1 = :GUBUN1                                      ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                       ");

            parameter.Add("PANO", nPano);
            parameter.Add("SDATE", strSDate);
            parameter.Add("GUBUN1", strGubn1);

            return ExecuteScalar<string>(parameter);
        }

        public void UpDateDelDateByPanoNotSDate(long argPano, string argSDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_RESV_EXAM               ");
            parameter.AppendSql("   SET DELDATE = SYSDATE                       ");
            parameter.AppendSql(" WHERE PANO =:PANO                             ");
            parameter.AppendSql("   AND SDATE != TO_DATE(:SDATE, 'YYYY-MM-DD')  ");

            parameter.Add("PANO", argPano);
            parameter.Add("SDATE", argSDate);

            ExecuteNonQuery(parameter);
        }

        public List<HEA_RESV_EXAM> GetListByRTimeExCodeIN(string strDate, List<string> strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.PTNO, TO_CHAR(b.RTIME,'HH24:MI') RTIME, a.PANO, a.SNAME, b.GBEXAM   ");
            parameter.AppendSql("     , b.EXCODE, a.SDATE, a.WRTNO, b.EXAMNAME, TO_CHAR(a.JEPDATE, 'YYYY-MM-DD') BDATE ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a                             ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_RESV_EXAM b                         ");
            parameter.AppendSql(" WHERE b.RTIME >= TO_DATE(:RTIME, 'YYYY-MM-DD')            ");
            parameter.AppendSql("   AND b.RTIME <  TO_DATE(:RTIME, 'YYYY-MM-DD') + 0.999999 ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                   ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                   ");
            parameter.AppendSql("   AND a.PANO = b.PANO                                     ");
            parameter.AppendSql("   AND a.SDATE = b.SDATE                                   ");
            parameter.AppendSql("   AND b.EXCODE IN (:EXCODE)                               ");
            parameter.AppendSql(" ORDER BY b.RTIME,a.SName                                  ");

            parameter.Add("RTIME", strDate);
            parameter.AddInStatement("EXCODE", strExCode);

            return ExecuteReader<HEA_RESV_EXAM>(parameter);
        }

        public string GetRowidByPanoRTimeGbExam(long nPano, string sFrDate, string sToDate, string argGbExam)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                                   ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                ");
            parameter.AppendSql("   AND RTIME >= TO_DATE(:FDATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("   AND RTIME <  TO_DATE(:TDATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("   AND DELDATE IS NULL                                             ");
            parameter.AppendSql("   AND GBEXAM =:GBEXAM                                             ");

            parameter.Add("PANO", nPano);
            parameter.Add("FDATE", sFrDate);
            parameter.Add("TDATE", sToDate);
            parameter.Add("GBEXAM", argGbExam);

            return ExecuteScalar<string>(parameter);
        }

        public void UpDateDelDateByPanoNotExam(long argPano, List<string> varGED)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_RESV_EXAM               ");
            parameter.AppendSql("   SET DELDATE = SYSDATE                       ");
            parameter.AppendSql(" WHERE PANO =:PANO                             ");
            parameter.AppendSql("   AND EXCODE NOT IN (:EXCODE)                 ");

            parameter.Add("PANO", argPano);
            parameter.AddInStatement("EXCODE", varGED);

            ExecuteNonQuery(parameter);
        }

        public List<HEA_RESV_EXAM> GetEndoResvListByPanoSDate(long argPano, string argSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(RTIME,'YYYY-MM-DD HH24:MI') RTIME, PANO, SNAME, GBEXAM, EXAMNAME    ");
            parameter.AppendSql("     , TO_CHAR(RTIME,'YYYY-MM-DD') RDATE, TO_CHAR(RTIME,'HH24:MI') STIME           ");
            parameter.AppendSql("     , TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, EXCODE, AMPM, ROWID AS RID               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                                                   ");
            parameter.AppendSql(" WHERE SDATE >= TO_DATE(:SDATE, 'YYYY-MM-DD')                                      ");
            parameter.AppendSql("   AND PANO =:PANO                                                                 ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                             ");
            parameter.AppendSql("   AND GBEXAM IN ('01', '02')                                                      ");

            parameter.Add("SDATE", argSDate);
            parameter.Add("PANO", argPano);

            return ExecuteReader<HEA_RESV_EXAM>(parameter);
        }

        public List<HEA_RESV_EXAM> GetListByPanoSDate(long argPano, string argSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(RTIME,'YYYY-MM-DD HH24:MI') RTIME, PANO, SNAME, GBEXAM, EXAMNAME    ");
            parameter.AppendSql("     , TO_CHAR(RTIME,'YYYY-MM-DD') RDATE, TO_CHAR(RTIME,'HH24:MI') STIME           ");
            parameter.AppendSql("     , TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, EXCODE, AMPM, ROWID AS RID               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                                                   ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')                                       ");
            parameter.AppendSql("   AND PANO =:PANO                                                                 ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                             ");

            parameter.Add("SDATE", argSDate);
            parameter.Add("PANO", argPano);

            return ExecuteReader<HEA_RESV_EXAM>(parameter);
        }

        public string GetNotEqualResvExamByPanoSDate(long argPano, string argSDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                                   ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')                       ");
            parameter.AppendSql("   AND TO_DATE(RTIME,'YYYY-MM-DD') != TO_DATE(SDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND RTIME >= TRUNC(SYSDATE)                                     ");
            parameter.AppendSql("   AND RTIME < TRUNC(SYSDATE + 1)                                  ");

            parameter.Add("PANO", argPano);
            parameter.Add("SDATE", argSDate);

            return ExecuteScalar<string>(parameter);
        }

        public void UpDateDelDateByPanoSDate(long argPano, string argSDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_RESV_EXAM               ");
            parameter.AppendSql("   SET DELDATE = SYSDATE                       ");
            parameter.AppendSql(" WHERE PANO =:PANO                             ");
            parameter.AppendSql("   AND SDATE =TO_DATE(:SDATE, 'YYYY-MM-DD')    ");

            parameter.Add("PANO", argPano);
            parameter.Add("SDATE", argSDate);

            ExecuteNonQuery(parameter);
        }

        public string GetRTimeByPanoExCodeSDate(long nPano, string argExCode, string argCurDate, string argType = "")
        {
            MParameter parameter = CreateParameter();

            if (argType == "HH24:MI")
            {
                parameter.AppendSql("SELECT TO_CHAR(RTIME,'YYYY-MM-DD HH24:MI') RTIME    ");
            }
            else
            {
                parameter.AppendSql("SELECT TO_CHAR(RTIME,'YYYY-MM-DD') RTIME            ");
            }
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND EXCODE = :EXCODE                                ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND DELDATE IS NULL                                 ");

            parameter.Add("PANO", nPano);
            parameter.Add("EXCODE", argExCode);
            parameter.Add("SDATE", argCurDate);

            return ExecuteScalar<string>(parameter);
        }

        public HEA_RESV_EXAM GetRTimebyPaNoGbExamSDate(long nPANO, string strSDate, string argGbExam)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(RTIME, 'YYYY-MM-DD HH24:MI') RTIME      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND GBEXAM = :GBEXAM                                "); //µ¿¸Æ°æÈ­ÇùÂø°Ë»ç
            parameter.AppendSql("   AND DELDATE IS NULL                                 ");

            parameter.Add("PANO", nPANO);
            parameter.Add("SDATE", strSDate);
            parameter.Add("GBEXAM", argGbExam);

            return ExecuteReaderSingle<HEA_RESV_EXAM>(parameter);
        }

        public long GetExistCountbyPanoGbExam(string strRDate, string strTDate, long fnPano, string strGb, string strAMPM, List<long> lstLtdCodes)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(a.PANO) CNT                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM a                     ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT b                       ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            parameter.AppendSql("   AND a.RTIME >= TO_DATE(:FTIME, 'YYYY-MM-DD')        ");
            parameter.AppendSql("   AND a.RTIME <  TO_DATE(:TTIME, 'YYYY-MM-DD')        ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                               ");
            parameter.AppendSql("   AND a.GBEXAM = :GBEXAM                              ");
            parameter.AppendSql("   AND a.PANO != :PANO                                 ");
            parameter.AppendSql("   AND a.AMPM = :AMPM                                  ");
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                              ");

            if (!lstLtdCodes.IsNullOrEmpty() && lstLtdCodes.Count > 0)
            {
                parameter.AppendSql("   AND b.LTDCODE IN (:LTDCODE)                         ");
            }
            
            parameter.Add("FTIME", strRDate);
            parameter.Add("TTIME", strTDate);
            parameter.Add("GBEXAM", strGb);
            parameter.Add("PANO", fnPano);
            parameter.Add("AMPM", strAMPM);

            if (!lstLtdCodes.IsNullOrEmpty() && lstLtdCodes.Count > 0)
            {
                parameter.AddInStatement("LTDCODE", lstLtdCodes);
            }

            return ExecuteScalar<long>(parameter);
        }

        public string GetSDateByPanoRTimeExCode(long pANO, string argFDate, string argTDate, string argExcode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND RTIME >= TO_DATE(:FSDATE, 'YYYY-MM-DD')         ");
            parameter.AppendSql("   AND RTIME <  TO_DATE(:TSDATE, 'YYYY-MM-DD')         ");
            parameter.AppendSql("   AND DELDATE IS NULL                                 ");
            parameter.AppendSql("   AND SDATE != TRUNC(SYSDATE)                         ");
            parameter.AppendSql("   AND EXCODE = :EXCODE                                ");

            parameter.Add("PANO", pANO);
            parameter.Add("FSDATE", argFDate);
            parameter.Add("TSDATE", argTDate);
            parameter.Add("EXCODE", argExcode);

            return ExecuteScalar<string>(parameter);
        }

        public HEA_RESV_EXAM GetRTimeGbExamByPanoExCodeSDate(long pANO, string jEPDATE, string argExcode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(RTime,'YYYY-MM-DD HH24:MI') RTIME, GBEXAM   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                           ");
            parameter.AppendSql(" WHERE 1 = 1                                               ");
            parameter.AppendSql("   AND PANO = :PANO                                        ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')               ");
            parameter.AppendSql("   AND EXCODE =:EXCODE                                     ");
            parameter.AppendSql("   AND DELDATE IS NULL                                     ");

            parameter.Add("PANO", pANO);
            parameter.Add("SDATE", jEPDATE);
            parameter.Add("EXCODE", argExcode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_RESV_EXAM>(parameter);
        }

        public List<HEA_RESV_EXAM> GetListByPanoRDate(long nPano, string argCurDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, SNAME, EXCODE, AMPM                       ");
            parameter.AppendSql("      ,TO_CHAR(SDATE,'YYYY-MM-DD') SDATE               ");
            parameter.AppendSql("      ,TO_CHAR(RTIME,'YYYY-MM-DD') RTIME               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                       ");
            parameter.AppendSql(" WHERE PANO =:PANO                                     ");
            parameter.AppendSql("   AND TRUNC(RTIME) = TO_DATE(:RTIME, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DELDATE IS NULL                                 ");

            parameter.Add("PANO", nPano);
            parameter.Add("RTIME", argCurDate);

            return ExecuteReader<HEA_RESV_EXAM>(parameter);
        }

        public string GetRowidByPanoExcode(long nPano, string argExcode, string argCurDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID AS RID                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                       ");
            parameter.AppendSql(" WHERE TRUNC(RTIME) = TO_DATE(:RTIME, 'YYYY-MM-DD')    ");
            if (!argExcode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND EXCODE = :EXCODE                                ");
            }
            parameter.AppendSql("   AND PANO = :PANO                                    ");
            
            parameter.Add("RTIME", argCurDate);
            if (!argExcode.IsNullOrEmpty())
            { 
                parameter.Add("EXCODE", argExcode);
            }
            parameter.Add("PANO", nPano);

            return ExecuteScalar<string>(parameter);
        }

        public void Update(HEA_RESV_EXAM code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_RESV_EXAM SET                   ");
            
            if (!code.TONGBODATE.IsNullOrEmpty() || code.CONFIRM == "Y")
            {
                parameter.AppendSql("  TONGBODATE =TO_DATE(:TONGBODATE,'YYYY-MM-DD')    ");
                parameter.AppendSql(" ,TONGBOSABUN =:TONGBOSABUN                        ");
            }
            else
            {
                parameter.AppendSql("  TONGBODATE = ''                          ");
                parameter.AppendSql(" ,TONGBOSABUN = 0                          ");
            }

            if (code.CONFIRM == "Y")
            {
                parameter.AppendSql("     ,CONFIRM = '1'                        ");
            }
            else
            {
                parameter.AppendSql("     ,CONFIRM = ''                         ");
            }

            if (!code.RTIME.IsNullOrEmpty()) { parameter.AppendSql(" ,RTIME =TO_DATE(:RTIME, 'YYYY-MM-DD HH24:MI') "); }
            if (!code.SNAME.IsNullOrEmpty()) { parameter.AppendSql(" ,SNAME =:SNAME     "); }
            if (!code.AMPM.IsNullOrEmpty()) { parameter.AppendSql(" ,AMPM =:AMPM     "); }

            parameter.AppendSql(" WHERE ROWID =:RID                             ");

            if (!code.TONGBODATE.IsNullOrEmpty() || code.CONFIRM == "Y")
            {
                parameter.Add("TONGBODATE", code.TONGBODATE);
                parameter.Add("TONGBOSABUN", clsType.User.IdNumber.To<long>());
            }

            if (!code.RTIME.IsNullOrEmpty()) { parameter.Add("RTIME", code.RTIME); }
            if (!code.SNAME.IsNullOrEmpty()) { parameter.Add("SNAME", code.SNAME); }
            if (!code.AMPM.IsNullOrEmpty()) { parameter.Add("AMPM", code.AMPM); }

            parameter.Add("RID", code.RID);

            ExecuteNonQuery(parameter);
        }

        public int GetCountbyPanoSDate(string strPano, string strJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                       ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND GBEXAM IN ('01')                                ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE,'YYYY-MM-DD')            ");

            parameter.Add("PANO", strPano);
            parameter.Add("SDATE", strJepDate);

            return ExecuteScalar<int>(parameter);
        }

        public List<HEA_RESV_EXAM> GetCNTAMPMbyRTime(string argFDate, string argTDate, string argGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(PANO) AS CNT, AMPM                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM               ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql("   AND RTIME >= TO_DATE(:RFTIME, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND RTIME <  TO_DATE(:RTTIME, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND GBEXAM =:GBEXAM                         ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql(" GROUP By AMPM                                 ");

            parameter.Add("RFTIME", argFDate);
            parameter.Add("RTTIME", argTDate);
            parameter.Add("GBEXAM", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_RESV_EXAM>(parameter);
        }

        public HEA_RESV_EXAM GetCountAMPMbyRTime(string argFDate, string argTDate, string argGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(DECODE(AMPM,'A',1,0)) AMCNT         ");
            parameter.AppendSql("      ,SUM(DECODE(AMPM,'A',0,1)) PMCNT         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM               ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql("   AND RTIME >= TO_DATE(:RFTIME, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND RTIME <  TO_DATE(:RTTIME, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND GBEXAM =:GBEXAM                         ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");

            parameter.Add("RFTIME", argFDate);
            parameter.Add("RTTIME", argTDate);
            parameter.Add("GBEXAM", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_RESV_EXAM>(parameter);
        }

        public List<HEA_RESV_EXAM> GetItembyRTime(string sFrDate, string sToDate, string sGbExam)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, SNAME, TO_CHAR(RTIME, 'HH24:MI') RTIME     ");
            parameter.AppendSql("     , TO_CHAR(SDATE,'YYYY-MM-DD') SDATE                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                                   ");
            parameter.AppendSql(" WHERE RTIME >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND RTIME <  TO_DATE(:TODATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND GBEXAM = :GBEXAM                                            ");
            parameter.AppendSql("   AND DELDATE IS NULL                                             ");
            parameter.AppendSql("   AND PANO > 1000                                                 ");
            parameter.AppendSql(" ORDER BY RTIME, SNAME                                             ");

            parameter.Add("FRDATE", sFrDate);
            parameter.Add("TODATE", sToDate);
            parameter.Add("GBEXAM", sGbExam, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_RESV_EXAM>(parameter);
        }

        public HEA_RESV_EXAM GetCountbyPaNo(long pANO, string strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, GBEXAM, AMPM                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM               ");
            parameter.AppendSql(" WHERE PANO   = :PANO                          ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)                     ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");

            parameter.Add("PANO", pANO);
            parameter.Add("EXCODE", strExCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_RESV_EXAM>(parameter);
        }

        public HEA_RESV_EXAM GetCountbyPaNo1(long pANO, string strGbExam, string strSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, GBEXAM, AMPM                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM               ");
            parameter.AppendSql(" WHERE PANO   = :PANO                          ");
            parameter.AppendSql("   AND GBEXAM = :GBEXAM                        ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')   ");

            parameter.Add("PANO", pANO);
            parameter.Add("GBEXAM", strGbExam, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("SDATE", strSDate);
            

            return ExecuteReaderSingle<HEA_RESV_EXAM>(parameter);
        }

        
        public HEA_RESV_EXAM GetRTimebySdatePanoExcode(string argSdate, string argPano, string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(RTIME,'YYYY-MM-DD HH24:MI') RTIME               ");                                     
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM                               ");
            parameter.AppendSql(" WHERE PANO = :PANO                                            ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')                   ");
            parameter.AppendSql("   AND DELDATE IS NULL                                         ");
            parameter.AppendSql("   AND EXCODE IN (                                             ");
            parameter.AppendSql("   SELECT Rtrim(NAME) EXCODE FROM HEA_CODE                     ");
            parameter.AppendSql("   WHERE CODE = :CODE                                          ");
            parameter.AppendSql("   AND GUBUN = '15')                                           ");

            parameter.Add("SDATE", argSdate);
            parameter.Add("PANO", argPano);
            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_RESV_EXAM>(parameter);
        }

        public HEA_RESV_EXAM GetCountBySdateGbexam(string argSdate, string argGbExam, string argSTime)
        {

            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT  COUNT('X') CNT                                    ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HEA_JEPSU A, KOSMOS_PMPA.HEA_RESV_EXAM B ");
            parameter.AppendSql(" WHERE A.PANO = B.PANO                                     ");
            parameter.AppendSql("  AND A.SDATE = B.SDATE                                    ");
            parameter.AppendSql("  AND A.SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')              ");
            parameter.AppendSql("  AND B.GBEXAM = :GBEXAM                                   ");
            parameter.AppendSql("  AND A.STIME = :STIME                                     ");
            parameter.AppendSql("  AND A.DELDATE IS NULL                                    ");
            parameter.AppendSql("  AND B.DELDATE IS NULL                                    ");

            parameter.Add("SDATE", argSdate);
            parameter.Add("GBEXAM", argGbExam, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("STIME", argSTime, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_RESV_EXAM>(parameter);
        }
    }
}
