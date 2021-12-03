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
    public class HicJepsuWorkRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuWorkRepository()
        {
        }

        public IList<HIC_JEPSU_WORK> GetListByItem(string argYear, string argFDate, string argTDate, string argGbChul, string argGjJong, long argLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,GJYEAR,TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,PANO,SNAME,SEX,AGE,GJJONG,GJCHASU    ");
            parameter.AppendSql("      ,GJBANGI,GBCHUL,GBSTS,LTDCODE,MAILCODE,JUSO1,JUSO2,PTNO,BURATE,JISA,KIHO,GKIHO,JIKGBN    ");
            parameter.AppendSql("      ,KOSMOS_PMPA.FC_HIC_LTDNAME(LTDCODE) AS LTDNAME                                          ");
            parameter.AppendSql("      ,UCODES,SEXAMS,JIKJONG,SABUN,TO_CHAR(IPSADATE,'YYYY-MM-DD') IPSADATE,BUSENAME,OLDJIKJONG ");
            parameter.AppendSql("      ,BUSEIPSA,OLDSDATE,OLDEDATE,SECOND_DATE,GBSUCHEP,BALYEAR,BALSEQ,DELDATE,JOBSABUN         ");
            parameter.AppendSql("      ,ENTTIME,GBEXAM,GBMUNJIN1,GBMUNJIN2,GBDENTAL,GBINWON,BOGUNSO,LIVER2,JEPSUGBN,YOUNGUPSO   ");
            parameter.AppendSql("      ,REMARK,MILEAGEAM, MURYOAM, GUMDAESANG,JUMINNO2,JUMINNO,HPHONE,ROWID AS RID              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK                                                              ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                   ");
            parameter.AppendSql("   AND ENTTIME >= TO_DATE(:FTIME, 'YYYY-MM-DD')                                                ");
            parameter.AppendSql("   AND ENTTIME <= TO_DATE(:TTIME, 'YYYY-MM-DD')                                                ");
            parameter.AppendSql("   AND GJYEAR  = :GJYEAR                                                                       ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                                         ");

            if (argLtdCode > 0) { parameter.AppendSql(" AND LTDCODE =:LTDCODE                                                   "); }
            if (argGjJong != "**" && !argGjJong.IsNullOrEmpty()) { parameter.AppendSql(" AND GJJONG =:GJJONG                    "); }
            if (!argGbChul.IsNullOrEmpty()) { parameter.AppendSql("  AND GBCHUL =:GBCHUL                                        "); }

            parameter.AppendSql("  ORDER BY SNAME                                                                               ");

            parameter.Add("FTIME", argFDate);
            parameter.Add("TTIME", argTDate);
            parameter.Add("GJYEAR", argYear);

            if (argLtdCode > 0) { parameter.Add("LTDCODE", argLtdCode); }
            if (argGjJong != "**" && !argGjJong.IsNullOrEmpty()) { parameter.Add("GJJONG", argGjJong); }
            if (!argGbChul.IsNullOrEmpty()) { parameter.Add("GBCHUL", argGbChul); }

            return ExecuteReader<HIC_JEPSU_WORK>(parameter);
        }

        public List<HIC_JEPSU_WORK> GetItembyJepDateGjJongLtdCode(string strFrDate, string strToDate, string strGjJong, string strLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,GJJONG,PANO,SNAME,SEX,AGE,SEXAMS,UCODES,LTDCODE,BUSENAME,JUMINNO2 ");
            parameter.AppendSql("     , JISA,GKIHO,KIHO,PTNO,GBCHUL,GBINWON,GJYEAR,GJCHASU,GJBANGI,GBCHUL,MAILCODE,JUSO1,JUSO2,BURATE,JIKGBN    ");
            parameter.AppendSql("     , JIKJONG,TO_CHAR(IPSADATE,'YYYY-MM-DD') IPSADATE ,GBSUCHEP, GBDENTAL,BOGUNSO,TEL,LIVER2,YOUNGUPSO        ");
            parameter.AppendSql("     , MILEAGEAM,MURYOAM,GUMDAESANG,MILEAGEAMGBN,MURYOGBN,REMARK,EMAIL,CLASS,BAN,BUN,HPHONE                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK                                                                              ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                               ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                               ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                                                         ");
            if (strGjJong == "00")
            {
                parameter.AppendSql("   AND GJJONG = :GJJONG                                                                                    ");
            }
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                                  ");
            }
            parameter.AppendSql(" ORDER BY GJJONG, SNAME                                                                                        ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strGjJong.IsNullOrEmpty())
            {
                parameter.Add("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (!strLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", strLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_WORK>(parameter);
        }

        public List<HIC_JEPSU_WORK> GetListGjNameByPtnoJepDate(string argYear, string argPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GJJONG,KOSMOS_PMPA.FC_HIC_GJJONG_NAME(GJJONG, UCODES) AS NAME ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK                                  ");
            parameter.AppendSql(" WHERE 1 = 1                                                       ");
            parameter.AppendSql("   AND PTNO = :PTNO                                                ");
            parameter.AppendSql("   AND GJYEAR =:GJYEAR                                             ");
            parameter.AppendSql("   AND DELDATE IS NULL                                             ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("GJYEAR", argYear);

            return ExecuteReader<HIC_JEPSU_WORK>(parameter);
        }

        public HIC_JEPSU_WORK GetSecondJongItemByPanoGjYear(long nPano, string strYear, string argJob)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,GJYEAR,TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,PANO,SNAME,SEX,AGE,GJJONG,GJCHASU    ");
            parameter.AppendSql("      ,GJBANGI,GBCHUL,GBSTS,LTDCODE,MAILCODE,JUSO1,JUSO2,PTNO,BURATE,JISA,KIHO,GKIHO,JIKGBN    ");
            parameter.AppendSql("      ,UCODES,SEXAMS,JIKJONG,SABUN,TO_CHAR(IPSADATE,'YYYY-MM-DD') IPSADATE,BUSENAME,OLDJIKJONG ");
            parameter.AppendSql("      ,BUSEIPSA,OLDSDATE,OLDEDATE,SECOND_DATE,GBSUCHEP,BALYEAR,BALSEQ,DELDATE,JOBSABUN         ");
            parameter.AppendSql("      ,ENTTIME,GBEXAM,GBMUNJIN1,GBMUNJIN2,GBDENTAL,GBINWON,BOGUNSO,LIVER2,JEPSUGBN,YOUNGUPSO   ");
            parameter.AppendSql("      ,TEL ,HPHONE, MURYOGBN, GBN, CLASS, BAN, BUN, GBSABUN, EMAIL, XRAYNO, GBADDPAN           ");
            parameter.AppendSql("      ,REMARK,MILEAGEAM, MURYOAM, GUMDAESANG,JUMINNO,JUMINNO2,ROWID AS RID                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK                                                              ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                   ");
            parameter.AppendSql("   AND PANO =:PANO                                                                             ");
            parameter.AppendSql("   AND GJYEAR >=:GJYEAR                                                                        ");
            if (argJob == "2")
            {
                parameter.AppendSql("   AND GJJONG NOT IN ('16','17','18','19','27','28','44','45','46')                         ");
            }
            else
            {
                parameter.AppendSql("   AND GJJONG IN ('16','17','18','19','27','28','44','45','46')                             ");
                parameter.AppendSql("   AND GJCHASU = '2'                                                                           ");
            }
            
            parameter.AppendSql(" ORDER BY JEPDATE DESC                                                                         ");
            
            parameter.Add("PANO", nPano);
            parameter.Add("GJYEAR", strYear);

            return ExecuteReaderSingle<HIC_JEPSU_WORK>(parameter);
        }

        public void DeleteByRowid(string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE FROM KOSMOS_PMPA.HIC_JEPSU_WORK WHERE ROWID  = :RID       ");
            
            parameter.Add("RID", argRowid);

            ExecuteNonQuery(parameter);
        }

        public string GetRowidByGJjongPtnoYear(string gJJONG, string pTNO, string gJYEAR)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID AS RID                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK          ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                        ");
            parameter.AppendSql("   AND GJJONG = :GJJONG                    ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                    ");

            parameter.Add("PTNO", pTNO);
            parameter.Add("GJJONG", gJJONG);
            parameter.Add("GJYEAR", gJYEAR);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateXrayNo(string strXrayno, string strFDate, string strTDate, string strLtdCode, string strGjYear, long nPaNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU_WORK SET              ");
            parameter.AppendSql("       XRAYNO   = :XRAYNO                          ");        
            parameter.AppendSql(" WHERE JEPDATE <= TO_DATE(:FRDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND JEPDATE >= TO_DATE(:TODATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND LTDCODE  = :LTDCODE                         ");
            parameter.AppendSql("   AND GJYEAR   = :GJYEAR                          ");
            parameter.AppendSql("   AND PANO     = :PANO                            ");

            parameter.Add("XRAYNO", strXrayno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            parameter.Add("LTDCODE", strLtdCode);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANO", nPaNo);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyPaNoSuDate(string strPANO, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK    a                             ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_SUNAPDTL_WORK b                             ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                              ");
            parameter.AppendSql("   AND a.Pano = b.Pano                                             ");
            parameter.AppendSql("   AND b.SUDATE = TO_DATE(:SUDATE, 'YYYY-MM-DD')                   ");
            parameter.AppendSql("   AND b.Code IN ( SELECT CODE FROM HIC_CODE WHERE GUBUN ='98' )   ");
            parameter.AppendSql("   AND a.GbChul ='Y'                                               ");

            parameter.Add("PANO", strPANO);
            parameter.Add("SUDATE", strJepDate);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_JEPSU_WORK> GetItembyJepDateGjJongLtdCode2(string strFrDate, string strToDate, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SNAME, GJJONG, BUSENAME, JUMINNO, AGE,SEX, HPHONE, JEPDATE, PTNO    ");
            parameter.AppendSql("     , UCODES, PANO, LTDCODE                                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK                                          ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                           ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                           ");
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                              ");
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_WORK>(parameter);
        }

        public string GetHphonebyPtNoGjJong(string argPtNo, string argGjJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT HPHONE FROM KOSMOS_PMPA.HIC_JEPSU_WORK  ");
            parameter.AppendSql(" WHERE PTNO   = :PTNO                          ");
            parameter.AppendSql("   AND GJJONG = :GJJONG                        ");

            parameter.Add("PTNO", argPtNo);
            parameter.Add("GJJONG", argGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_JEPSU_WORK> GetItembyJepDateGjJong(string strFDate, string strTDate, List<string> strjong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.Ptno,a.SName,b.HPhone,b.Tel,TO_CHAR(a.JEPDATE,'YYYY-MM-DD') YDate ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(a.LTDCODE) LTDNAME, A.GBCHUL, A.GJJONG   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK a, KOSMOS_PMPA.HIC_PATIENT b             ");
            parameter.AppendSql(" WHERE a.Pano=b.Pano(+)                                                    ");
            parameter.AppendSql("   AND a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                         "); //검진일자
            parameter.AppendSql("   AND a.JEPDATE <  TO_DATE(:TODATE, 'YYYY-MM-DD')                         "); //검진일자
            parameter.AppendSql("   AND A.GJJONG IN ('11','23')                                             ");
            parameter.AppendSql(" ORDER BY JEPDATE,SName,HPhone                                             ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            parameter.AddInStatement("GJJONG", strjong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU_WORK>(parameter);
        }

        public List<HIC_JEPSU_WORK> GetItembyJepDate(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.Ptno,a.SName,b.HPhone,b.Tel,TO_CHAR(a.JEPDATE,'YYYY-MM-DD') YDate ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(a.LTDCODE) LTDNAME, A.GBCHUL, A.GJJONG   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK a, KOSMOS_PMPA.HIC_PATIENT b             ");
            parameter.AppendSql(" WHERE a.Pano=b.Pano(+)                                                    ");
            parameter.AppendSql("   AND a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                         "); //검진일자
            parameter.AppendSql("   AND a.JEPDATE <  TO_DATE(:TODATE, 'YYYY-MM-DD')                         "); //검진일자
            parameter.AppendSql("   AND A.GJJONG IN ('11','23')                                             ");
            parameter.AppendSql(" ORDER BY JEPDATE,SName,HPhone                                             ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteReader<HIC_JEPSU_WORK>(parameter);
        }

        public int UpdateAllbyRowId(HIC_JEPSU_WORK item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU_WORK SET          ");
            parameter.AppendSql("       GJYEAR     = :GJYEAR                    ");
            parameter.AppendSql("     , JEPDATE    = TO_DATE(:JEPDATE, 'YYYY-MM-DD')");
            parameter.AppendSql("     , PANO       = :PANO                      ");
            parameter.AppendSql("     , SNAME      = :SNAME                     ");
            parameter.AppendSql("     , SEX        = :SEX                       ");
            parameter.AppendSql("     , AGE        = :AGE                       ");
            parameter.AppendSql("     , GJJONG     = :GJJONG                    ");
            parameter.AppendSql("     , TEL        = :TEL                       ");
            parameter.AppendSql("     , GJCHASU    = :GJCHASU                   ");
            parameter.AppendSql("     , GJBANGI    = :GJBANGI                   ");
            parameter.AppendSql("     , LTDCODE    = :LTDCODE                   ");
            parameter.AppendSql("     , BUSENAME   = :BUSENAME                  ");
            parameter.AppendSql("     , MAILCODE   = :MAILCODE                  ");
            parameter.AppendSql("     , JUSO1      = :JUSO1                     ");
            parameter.AppendSql("     , JUSO2      = :JUSO2                     ");
            parameter.AppendSql("     , UCODES     = :UCODES                    ");
            parameter.AppendSql("     , SEXAMS     = :SEXAMS                    ");
            parameter.AppendSql("     , PTNO       = :PTNO                      ");
            parameter.AppendSql("     , BURATE     = :BURATE                    ");
            parameter.AppendSql("     , JISA       = :JISA                      ");
            parameter.AppendSql("     , KIHO       = :KIHO                      ");
            parameter.AppendSql("     , GKIHO      = :GKIHO                     ");
            parameter.AppendSql("     , JUMINNO    = :JUMINNO                   ");
            parameter.AppendSql("     , JUMINNO2   = :JUMINNO2                  ");
            if (!item.JIKGBN.IsNullOrEmpty())
            {
                parameter.AppendSql("     , JIKGBN     = :JIKGBN                ");
            }
            parameter.AppendSql("     , GBCHUL     = :GBCHUL                    ");
            parameter.AppendSql("     , GBDENTAL   = :GBDENTAL                  ");
            parameter.AppendSql("     , GBINWON    = :GBINWON                   ");
            parameter.AppendSql("     , JOBSABUN   = :JOBSABUN                  ");
            parameter.AppendSql("     , ENTTIME    = SYSDATE                    ");
            parameter.AppendSql("     , BOGUNSO    = :BOGUNSO                   ");
            parameter.AppendSql("     , YOUNGUPSO  = :YOUNGUPSO                 ");
            if (!item.IPSADATE.IsNullOrEmpty())
            {
                parameter.AppendSql("     , IPSADATE   = TO_DATE(:IPSADATE, 'YYYY-MM-DD')");
            }  
            parameter.AppendSql("     , MURYOAM    = :MURYOAM                   ");
            parameter.AppendSql("     , EMAIL      = :EMAIL                     ");
            parameter.AppendSql("     , REMARK     = :REMARK                    ");
            parameter.AppendSql("     , CLASS      = :CLASS                     ");
            parameter.AppendSql("     , BAN        = :BAN                       ");
            parameter.AppendSql("     , BUN        = :BUN                       ");
            parameter.AppendSql("     , HPHONE     = :HPHONE                    ");
            parameter.AppendSql(" WHERE ROWID      = :RID                       ");

            parameter.Add("PANO", item.PANO);
            parameter.Add("GJYEAR", item.GJYEAR, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", item.JEPDATE);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", item.AGE);
            parameter.Add("GJJONG", item.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("TEL", item.TEL);
            parameter.Add("GJCHASU", item.GJCHASU, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJBANGI", item.GJBANGI, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", item.LTDCODE);
            parameter.Add("BUSENAME", item.BUSENAME);
            parameter.Add("MAILCODE", item.MAILCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUSO1", item.JUSO1);
            parameter.Add("JUSO2", item.JUSO2);
            parameter.Add("UCODES", item.UCODES);
            parameter.Add("SEXAMS", item.SEXAMS);
            parameter.Add("PTNO", item.PTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BURATE", item.BURATE);
            parameter.Add("JISA", item.JISA, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("KIHO", item.KIHO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GKIHO", item.GKIHO);
            parameter.Add("JUMINNO", item.JUMINNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMINNO2", item.JUMINNO2);
            if (!item.JIKGBN.IsNullOrEmpty())
            {
                parameter.Add("JIKGBN", item.JIKGBN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            parameter.Add("GBCHUL", item.GBCHUL, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBDENTAL", item.GBDENTAL, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBINWON", item.GBINWON, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JOBSABUN", item.JOBSABUN);
            parameter.Add("BOGUNSO", item.BOGUNSO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("YOUNGUPSO", item.YOUNGUPSO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!item.IPSADATE.IsNullOrEmpty())
            {
                parameter.Add("IPSADATE", item.IPSADATE.Substring(0, 10));
            }
            parameter.Add("MURYOAM", item.MURYOAM, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EMAIL", item.EMAIL);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("CLASS", item.CLASS);
            parameter.Add("BAN", item.BAN);
            parameter.Add("BUN", item.BUN);
            parameter.Add("HPHONE", item.HPHONE);
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public int InsertAll(HIC_JEPSU_WORK item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_JEPSU_WORK                                                         ");
            parameter.AppendSql("       (GJYEAR,JEPDATE,PANO,SNAME,SEX,AGE,GJJONG,TEL                                           ");
            parameter.AppendSql("     , GJCHASU,GJBANGI,LTDCODE,BUSENAME,MAILCODE,JUSO1,JUSO2,UCODES                            ");
            parameter.AppendSql("     , SEXAMS,PTNO,BURATE,JISA,KIHO,GKIHO,JUMINNO,JUMINNO2,JIKGBN                              ");
            parameter.AppendSql("     , GBCHUL,GBDENTAL,GBINWON,JOBSABUN,ENTTIME,BOGUNSO                                        ");
            parameter.AppendSql("     , YOUNGUPSO,MURYOAM,EMAIL                                                                 ");
            parameter.AppendSql("     , REMARK,GBN,CLASS,BAN,BUN,HPHONE,SABUN,GBADDPAN                                          ");
            if (!item.IPSADATE.IsNullOrEmpty())
            {
                parameter.AppendSql("     ,IPSADATE                                                                             ");
            }
            parameter.AppendSql("      )                                                                                        ");

            parameter.AppendSql(" VALUES                                                                                        ");
            parameter.AppendSql("       (:GJYEAR, to_date(:JEPDATE, 'yyyy-MM-dd'), :PANO, :SNAME, :SEX, :AGE, :GJJONG, :TEL     ");
            parameter.AppendSql("     , :GJCHASU, :GJBANGI, :LTDCODE, :BUSENAME, :MAILCODE, :JUSO1, :JUSO2, :UCODES             ");
            parameter.AppendSql("     , :SEXAMS, :PTNO, :BURATE, :JISA, :KIHO, :GKIHO, :JUMINNO, :JUMINNO2, :JIKGBN             ");
            parameter.AppendSql("     , :GBCHUL, :GBDENTAL, :GBINWON, :JOBSABUN, SYSDATE, :BOGUNSO                              ");
            parameter.AppendSql("     , :YOUNGUPSO, :MURYOAM, :EMAIL                                                            ");
            parameter.AppendSql("     , :REMARK, :GBN,:CLASS, :BAN, :BUN, :HPHONE, :SABUN, :GBADDPAN                            ");
            if (!item.IPSADATE.IsNullOrEmpty())
            {
                parameter.AppendSql("     , TO_DATE(:IPSADATE, 'YYYY-MM-DD')                                                    ");
            }
            parameter.AppendSql("      )                                                                                        ");

            parameter.Add("PANO", item.PANO);
            parameter.Add("GJYEAR", item.GJYEAR, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", item.JEPDATE);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", item.AGE);
            parameter.Add("GJJONG", item.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("TEL", item.TEL);
            parameter.Add("GJCHASU", item.GJCHASU, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJBANGI", item.GJBANGI, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", item.LTDCODE);
            parameter.Add("BUSENAME", item.BUSENAME);
            parameter.Add("MAILCODE", item.MAILCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUSO1", item.JUSO1);
            parameter.Add("JUSO2", item.JUSO2);
            parameter.Add("UCODES", item.UCODES);
            parameter.Add("SEXAMS", item.SEXAMS);
            parameter.Add("PTNO", item.PTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BURATE", item.BURATE);
            parameter.Add("JISA", item.JISA, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("KIHO", item.KIHO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GKIHO", item.GKIHO);
            parameter.Add("JUMINNO", item.JUMINNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMINNO2", item.JUMINNO2);
            parameter.Add("JIKGBN", item.JIKGBN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBCHUL", item.GBCHUL, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBDENTAL", item.GBDENTAL, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBINWON", item.GBINWON, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JOBSABUN", item.JOBSABUN);
            parameter.Add("BOGUNSO", item.BOGUNSO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("YOUNGUPSO", item.YOUNGUPSO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("MURYOAM", item.MURYOAM, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EMAIL", item.EMAIL);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("GBN", item.GBN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("CLASS", item.CLASS);
            parameter.Add("BAN", item.BAN);
            parameter.Add("BUN", item.BUN);
            parameter.Add("HPHONE", item.HPHONE);
            parameter.Add("SABUN", item.SABUN);
            parameter.Add("GBADDPAN", item.GBADDPAN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            if (!item.IPSADATE.IsNullOrEmpty())
            {
                parameter.Add("IPSADATE", item.IPSADATE.Substring(0, 10));
            }

            return ExecuteNonQuery(parameter);
        }

        public int UpdatePaNobyPaNo(string argPaNo, string strJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_JEPSU_WORK SET          ");
            parameter.AppendSql("       PANO = :PANO                            ");
            parameter.AppendSql(" WHERE PANO IN (SELECT PANO                    ");
            parameter.AppendSql("                  FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql("                 WHERE JUMIN2 = :JUMIN2        ");
            parameter.AppendSql("                   AND PANO <> :PANO)          ");

            parameter.Add("PANO", argPaNo);
            parameter.Add("JUMIN2", strJumin2);

            return ExecuteNonQuery(parameter);
        }

        public HIC_JEPSU_WORK GetItemByRid(string argRid)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,GJYEAR,TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,PANO,SNAME,SEX,AGE,GJJONG,GJCHASU    ");
            parameter.AppendSql("      ,GJBANGI,GBCHUL,GBSTS,LTDCODE,MAILCODE,JUSO1,JUSO2,PTNO,BURATE,JISA,KIHO,GKIHO,JIKGBN    ");
            parameter.AppendSql("      ,UCODES,SEXAMS,JIKJONG,SABUN,TO_CHAR(IPSADATE,'YYYY-MM-DD') IPSADATE,BUSENAME,OLDJIKJONG ");
            parameter.AppendSql("      ,BUSEIPSA,OLDSDATE,OLDEDATE,SECOND_DATE,GBSUCHEP,BALYEAR,BALSEQ,DELDATE,JOBSABUN         ");
            parameter.AppendSql("      ,ENTTIME,GBEXAM,GBMUNJIN1,GBMUNJIN2,GBDENTAL,GBINWON,BOGUNSO,LIVER2,JEPSUGBN,YOUNGUPSO   ");
            parameter.AppendSql("      ,TEL ,HPHONE, MURYOGBN, GBN, CLASS, BAN, BUN, GBSABUN, EMAIL, XRAYNO, GBADDPAN           ");
            parameter.AppendSql("      ,REMARK,MILEAGEAM, MURYOAM, GUMDAESANG,JUMINNO,JUMINNO2,ROWID AS RID                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK                                                              ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                   ");
            parameter.AppendSql("   AND ROWID =:RID                                                                             ");

            parameter.Add("RID", argRid);

            return ExecuteReaderSingle<HIC_JEPSU_WORK>(parameter);
        }

        public List<HIC_JEPSU_WORK> GetItemByPtnoYear(string argPtno, string argYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,GJYEAR,TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,PANO,SNAME,SEX,AGE,GJJONG,GJCHASU    ");
            parameter.AppendSql("      ,GJBANGI,GBCHUL,GBSTS,LTDCODE,MAILCODE,JUSO1,JUSO2,PTNO,BURATE,JISA,KIHO,GKIHO,JIKGBN    ");
            parameter.AppendSql("      ,UCODES,SEXAMS,JIKJONG,SABUN,TO_CHAR(IPSADATE,'YYYY-MM-DD') IPSADATE,BUSENAME,OLDJIKJONG ");
            parameter.AppendSql("      ,BUSEIPSA,OLDSDATE,OLDEDATE,SECOND_DATE,GBSUCHEP,BALYEAR,BALSEQ,DELDATE,JOBSABUN         ");
            parameter.AppendSql("      ,ENTTIME,GBEXAM,GBMUNJIN1,GBMUNJIN2,GBDENTAL,GBINWON,BOGUNSO,LIVER2,JEPSUGBN,YOUNGUPSO   ");
            parameter.AppendSql("      ,TEL ,HPHONE, MURYOGBN, GBN, CLASS, BAN, BUN, GBSABUN, EMAIL, XRAYNO, GBADDPAN           ");
            parameter.AppendSql("      ,REMARK,MILEAGEAM, MURYOAM, GUMDAESANG,JUMINNO,JUMINNO2,ROWID AS RID                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK                                                              ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                   ");
            parameter.AppendSql("   AND PTNO =:PTNO                                                                             ");
            parameter.AppendSql("   AND GJYEAR  =:GJYEAR                                                                       ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                                         ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("GJYEAR", argYear);

            return ExecuteReader<HIC_JEPSU_WORK>(parameter);
        }

        public void Delete_SunapWork(HIC_JEPSU_WORK code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SUNAP_WORK              ");
            parameter.AppendSql(" WHERE PANO =:PANO                             ");
            parameter.AppendSql("   AND SUDATE =TO_DATE(:SUDATE,'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND GJJONG =:GJJONG                         ");

            parameter.Add("PANO", code.PANO);
            parameter.Add("SUDATE", code.JEPDATE);
            parameter.Add("GJJONG", code.GJJONG);

            ExecuteNonQuery(parameter);
        }

        public void Delete_JepsuWork(HIC_JEPSU_WORK code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_JEPSU_WORK                 ");
            parameter.AppendSql(" WHERE ROWID =:RID                             ");

            parameter.Add("RID", code.RID);

            ExecuteNonQuery(parameter);
        }

        public void Delete_SunapDtlWork(HIC_JEPSU_WORK code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SUNAPDTL_WORK           ");
            parameter.AppendSql(" WHERE PANO =:PANO                             ");
            parameter.AppendSql("   AND SUDATE =TO_DATE(:SUDATE,'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND GJJONG =:GJJONG                         ");

            parameter.Add("PANO", code.PANO);
            parameter.Add("SUDATE", code.JEPDATE);
            parameter.Add("GJJONG", code.GJJONG);

            ExecuteNonQuery(parameter);
        }

        public void Update(HIC_JEPSU_WORK code)
        {
            throw new NotImplementedException();
        }

        public void Insert(HIC_JEPSU_WORK code)
        {
            throw new NotImplementedException();
        }

        public List<HIC_JEPSU_WORK> GetItembyJuMin(string strJuMin, string strYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GJJONG,UCODES,ROWID                                     ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_JEPSU_WORK                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                   ");
            parameter.AppendSql(" AND JUMINNO2 = :JUMIN                                         ");
            parameter.AppendSql(" AND GJYEAR = :YEAR                                            ");

            parameter.Add("JUMIN", strJuMin);
            parameter.Add("YEAR", strYear);

            return ExecuteReader<HIC_JEPSU_WORK>(parameter);
        }

        public List<HIC_JEPSU_WORK> GetListItemByPanoGjYear(long nPano, string strYear, string argJob)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,GJYEAR,TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,PANO,SNAME,SEX,AGE,GJJONG,GJCHASU    ");
            parameter.AppendSql("      ,GJBANGI,GBCHUL,GBSTS,LTDCODE,MAILCODE,JUSO1,JUSO2,PTNO,BURATE,JISA,KIHO,GKIHO,JIKGBN    ");
            parameter.AppendSql("      ,UCODES,SEXAMS,JIKJONG,SABUN,TO_CHAR(IPSADATE,'YYYY-MM-DD') IPSADATE,BUSENAME,OLDJIKJONG ");
            parameter.AppendSql("      ,BUSEIPSA,OLDSDATE,OLDEDATE,SECOND_DATE,GBSUCHEP,BALYEAR,BALSEQ,DELDATE,JOBSABUN         ");
            parameter.AppendSql("      ,ENTTIME,GBEXAM,GBMUNJIN1,GBMUNJIN2,GBDENTAL,GBINWON,BOGUNSO,LIVER2,JEPSUGBN,YOUNGUPSO   ");
            parameter.AppendSql("      ,TEL ,HPHONE, MURYOGBN, GBN, CLASS, BAN, BUN, GBSABUN, EMAIL, XRAYNO, GBADDPAN           ");
            parameter.AppendSql("      ,REMARK,MILEAGEAM, MURYOAM, GUMDAESANG,JUMINNO,JUMINNO2,ROWID AS RID                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK                                                              ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                   ");
            parameter.AppendSql("   AND PANO =:PANO                                                                             ");
            parameter.AppendSql("   AND GJYEAR >=:GJYEAR                                                                        ");
            if (argJob == "2")
            {
                parameter.AppendSql("   AND GJJONG NOT IN ('16','17','18','19','27','28','44','45','46')                         ");
            }
            else
            {
                parameter.AppendSql("   AND GJJONG IN ('16','17','18','19','27','28','44','45','46')                             ");
                parameter.AppendSql("   AND GJCHASU = '2'                                                                           ");
            }

            parameter.AppendSql(" ORDER BY JEPDATE DESC                                                                         ");

            parameter.Add("PANO", nPano);
            parameter.Add("GJYEAR", strYear);

            return ExecuteReader<HIC_JEPSU_WORK>(parameter);
        }

    }
}
