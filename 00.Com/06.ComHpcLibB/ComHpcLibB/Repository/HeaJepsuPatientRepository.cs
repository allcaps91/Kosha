namespace ComHpcLibB.Repository
{
    using ComBase;
    using ComBase.Mvc;
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;
    using ComBase.Controls;

    public class HeaJepsuPatientRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuPatientRepository()
        {
        }

        public List<HEA_JEPSU_PATIENT> GetItembyLtdCode(string strFDate, string strTDate, string strLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.PANO, a.SNAME, a.SEX, a.AGE, b.JUMIN, b.JUMIN2, b.BUSENAME, a.PANREMARK   ");
            parameter.AppendSql("     , TO_CHAR(a.SDATE, 'yyyy-mm-dd') SDATE                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU   a                                                   ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT b                                                   ");
            parameter.AppendSql(" WHERE a.PANO = b.PANO                                                             ");
            parameter.AppendSql("   AND a.SDATE >= TO_DATE(:FRDATE, 'yyyy-mm-dd')                                   ");
            parameter.AppendSql("   AND a.SDATE <= TO_DATE(:TODATE, 'yyyy-mm-dd')                                   ");
            if (strLtdCode != "")
            {
                if (VB.Val(strLtdCode) == 37 || VB.Val(strLtdCode) == 38)
                {
                    parameter.AppendSql("   AND a.LTDCODE in (37, 38)                                               ");
                }
                else
                {
                    parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                ");
                }
            }
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                           ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('D','0')                                                    ");
            parameter.AppendSql(" ORDER BY SNAME                                                                    ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            if (strLtdCode != "")
            {
                parameter.Add("LTDCODE", long.Parse(strLtdCode));
            }

            return ExecuteReader<HEA_JEPSU_PATIENT>(parameter);
        }

        public List<HEA_JEPSU_PATIENT> GetListBySDateAMPM2(string strSDate, string strAmPm2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.AMPM2, a.SNAME,b.JUMIN2,a.GBSTS,a.PANO,a.WRTNO,a.PTNO,a.AGE,a.SEX,a.LTDCODE   ");
            parameter.AppendSql("      ,a.GJJONG,TO_CHAR(a.SDATE,'YYYY-MM-DD') SDATE,b.TEL,b.HPHONE,b.JUSO1,b.JUSO2     ");
            parameter.AppendSql("      ,KOSMOS_PMPA.FC_HIC_LTDNAME(a.LTDCODE) LTDNAME                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU   a                                                       ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT b                                                       ");
            parameter.AppendSql(" WHERE a.SDATE = TO_DATE(:SDATE, 'yyyy-mm-dd')                                         ");
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                                                              ");
            parameter.AppendSql("   AND a.GBSTS = '0'                                                                   ");
            parameter.AppendSql("   AND a.AMPM2 = :AMPM2                                                                ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                               ");
            parameter.AppendSql(" ORDER BY a.SNAME                                                                      ");

            parameter.Add("SDATE", strSDate);
            parameter.Add("AMPM2", strAmPm2);

            return ExecuteReader<HEA_JEPSU_PATIENT>(parameter);
        }

        public HEA_JEPSU_PATIENT GetItembyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Ptno, a.Sname, b.Jumin2,b.JUSO1 || b.JUSO2 Juso,a.Sex,a.Age,a.EndoGbn     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a, KOSMOS_PMPA.HIC_PATIENT b                          ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                            ");
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                                                          ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReaderSingle<HEA_JEPSU_PATIENT>(parameter);
        }

        public HEA_JEPSU_PATIENT GetItemByJumin2(string strJumin)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.WRTNO, b.GJJONG, b.PANO, a.JUMIN2                     ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(b.LTDCODE) AS LTDNAME        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT a                               ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_JEPSU   b                               ");
            parameter.AppendSql(" WHERE a.PANO = b.PANO(+)                                      ");
            parameter.AppendSql("   AND b.SDATE = TRUNC(SYSDATE)                                ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                       ");
            parameter.AppendSql("   AND a.JUMIN2 =:JUMIN2                                       ");

            parameter.Add("JUMIN2", strJumin);

            return ExecuteReaderSingle<HEA_JEPSU_PATIENT>(parameter);
        }

        public List<HEA_JEPSU_PATIENT> GetJepsuListbyToDayAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" select B.SNAME, B.PTNO, '' HEIGHT, B.SEX, B.BIRTHDAY, A.AGE           ");
            parameter.AppendSql("      , B.HPHONE, B.TEL, B.MAILCODE, B.JUSO1 || ' ' || B.JUSO2 ADDRESS ");
            parameter.AppendSql("      , B.EMAIL, TRUNC(SYSDATE), '' MEMO                               ");
            parameter.AppendSql("      , B.JUMIN2                                                       ");
            parameter.AppendSql("   from KOSMOS_PMPA.HEA_JEPSU   a                                      ");
            parameter.AppendSql("      , KOSMOS_PMPA.HIC_PATIENT b                                      ");
            parameter.AppendSql("  where a.ptno = b.ptno                                                ");
            parameter.AppendSql("    and a.SDATE = TRUNC(SYSDATE)                                       ");
            parameter.AppendSql("    and a.DELDATE IS NULL                                              ");
            parameter.AppendSql("    and a.GBSTS NOT IN('D')                                            ");

            return ExecuteReader<HEA_JEPSU_PATIENT>(parameter);
        }

        public HEA_JEPSU_PATIENT GetJepsuListbyToDay(long nPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT PANO,SNAME,JUMIN2,SEX,BIRTHDAY,GBBIRTH,MAILCODE,JUSO1,JUSO2,TEL,HPHONE     ");
            parameter.AppendSql("      , GBSMS,LTDCODE,BUSENAME,SABUN,LTDTEL,GAMCODE,RELIGION,STARTDATE,LASTDATE    ");
            parameter.AppendSql("      , JINCOUNT , PTNO, REMARK, TEL_CONFIRM, FAMILLY, GAMCODE2                    ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_PATIENT                                                    ");
            parameter.AppendSql("  WHERE PANO = :PANO                                                               ");

            parameter.Add("PANO", nPano);

            return ExecuteReaderSingle<HEA_JEPSU_PATIENT>(parameter);
        }

        public HEA_JEPSU GetWrtnoBySDateSNameJumin2(string strFDate, string strTDate, string strSName, string strAESJumin, string strJumin1)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO, a.PTNO, a.PANO, TO_CHAR(a.JEPDATE, 'YYYY-MM-DD') JEPDATE                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU   a                   ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT b                   ");
            parameter.AppendSql(" WHERE a.SDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND a.SDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                           ");
            parameter.AppendSql("   AND a.SNAME =:SNAME                             ");
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                          "); 

            if (!strAESJumin.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND b.JUMIN2 =:JUMIN2                        ");
            }
            else
            {
                parameter.AppendSql("   AND b.JUMIN LIKE (:JUMIN)                    ");
            }

            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);
            parameter.Add("SNAME", strSName);
            if (!strAESJumin.IsNullOrEmpty())
            {
                parameter.Add("JUMIN2", strAESJumin);
            }
            else
            {
                parameter.Add("JUMIN", strJumin1);
            }
            
            return ExecuteReaderSingle<HEA_JEPSU>(parameter);
        }

        public List<HEA_JEPSU_PATIENT> GetItembyJepDateGjJong(string strFDate, string strTDate, List<string> strjong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Ptno,a.SName,b.HPhone,b.Tel,TO_CHAR(a.JEPDATE,'YYYY-MM-DD') YDate             ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(a.LTDCODE) LTDNAME, A.GBCHUL                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK a, KOSMOS_PMPA.HIC_PATIENT b                         ");
            parameter.AppendSql(" WHERE a.Pano=b.Pano(+)                                                                ");
            parameter.AppendSql("   AND a.JEPDATE >= TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                    "); //검진일자
            parameter.AppendSql("   AND A.GJJONG IN (:GJJONG)                                                           ");
            parameter.AppendSql("   AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019')                   "); //종검은 HPhone 컬럼 사용 4.30 보스코수녀님요청
            parameter.AppendSql(" ORDER BY JEPDATE,SName,HPhone                                                         ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            parameter.AddInStatement("GJJONG", strjong, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteReader<HEA_JEPSU_PATIENT>(parameter);
        }

        public List<HEA_JEPSU_PATIENT> GetItembySdateSname(string strFDate, string strTDate, string strSname)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("A.WRTNO, A.PTNO, B.SNAME, B.HPHONE, A.SDATE JEPDATE, A.GJJONG,A.SEX                        ");
            parameter.AppendSql(" , A.AGE, A.WebPrintSend, B.JUMIN, B.JUMIN2                                                ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HEA_JEPSU a, KOSMOS_PMPA.HIC_PATIENT b                                   ");
            parameter.AppendSql(" WHERE a.SDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                           ");
            parameter.AppendSql(" AND a.SDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                             ");
            parameter.AppendSql(" AND a.DelDate IS NULL                                                                     ");
            parameter.AppendSql(" AND A.WEBPRINTSEND IS NOT NULL                                                            ");
            if (!strSname.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME = :SNAME                                                                ");
            }
            parameter.AppendSql("   AND a.PTNO=b.PTNO(+)                                                                    ");
            parameter.AppendSql(" ORDER BY PTNO                                                                             ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            if(!strSname.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strSname);
            }
            

            return ExecuteReader<HEA_JEPSU_PATIENT>(parameter);
        }

        public List<HEA_JEPSU_PATIENT> GetItembyJepDate(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Ptno,a.SName,b.HPhone,b.Tel,TO_CHAR(a.SDate,'YYYY-MM-DD') YDate, a.ampm2,a.STime  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a, KOSMOS_PMPA.HIC_PATIENT b                                  ");
            parameter.AppendSql(" WHERE a.Pano = b.Pano(+)                                                                  ");
            parameter.AppendSql("   AND a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.JEPDATE <  TO_DATE(:TODATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.GbSTS = '0'                                                                       ");  //예약자만
            parameter.AppendSql("   AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019')                       ");
            parameter.AppendSql(" ORDER BY SDate,SName,HPhone                                                               ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteReader<HEA_JEPSU_PATIENT>(parameter);
        }

        public List<HEA_JEPSU_PATIENT> GetItembySDateLtdCode(string strSDate, string strGbSts, string strLtdCode, string strSort)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.SDATE,'YYYY-MM-DD') SDATE, a.GJJONG, a.PANO, a.SNAME, a.SEX, a.AGE    ");
            parameter.AppendSql("     , TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE, a.GBSTS, a.PTNO                        ");
            parameter.AppendSql("     , a.WRTNO, a.LTDCODE, b.BUSENAME, b.JUMIN, b.JUMIN2, b.PTNO, b.JINCOUNT           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a, KOSMOS_PMPA.HIC_PATIENT b                              ");
            parameter.AppendSql(" WHERE a.SDATE >= TO_DATE(:SDATE, 'YYYY-MM-DD')                                        ");
            parameter.AppendSql("   AND a.SDATE <= TO_DATE(:SDATE, 'YYYY-MM-DD')                                        ");
            if (strGbSts == "1")
            {
                parameter.AppendSql("   AND (a.GBSTS = '0' AND DELDATE IS NULL)                                         ");
            }
            else
            {
                parameter.AppendSql("   AND DELDATE IS NULL                                                             ");
            }
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                                                              ");
            if (strLtdCode != "")
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                        ");
            }

            if (strSort == "1")
            {
                parameter.AppendSql(" ORDER BY a.LTDCODE, a.SNAME                                                       ");
            }
            else if (strSort == "2")
            {
                parameter.AppendSql(" ORDER BY a.SNAME                                                                  ");
            }
            else if (strSort == "3")
            {
                parameter.AppendSql(" ORDER BY a.PTNO, a.SNAME                                                          ");
            }

            parameter.Add("SDATE", strSDate);
            if (strLtdCode != "")
            {
                parameter.Add("LTDCODE", strLtdCode);
            }

            return ExecuteReader<HEA_JEPSU_PATIENT>(parameter);
        }

        public List<HEA_JEPSU_PATIENT> GetItembyIDate(string strFrDate, string strToDate, string strName, string strGbn, string strLtdCode, string strJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.IDate,'YYYY-MM-DD') IDate                                             ");
            parameter.AppendSql("     , TO_CHAR(a.SDate,'YYYY-MM-DD') SDate                                             ");
            parameter.AppendSql("     , a.GJJONG, a.Pano, a.SName, a.Sex, a.Age                                         ");
            parameter.AppendSql("     , TO_CHAR(MailDate,'YYYY-MM-DD') MailDate, TO_CHAR(EntTime,'HH24:MI') EntTime     ");
            parameter.AppendSql("     , a.WRTNO, a.LtdCode, b.BuseName, b.Jumin2, b.PTno, a.GbSangDam, a.ROWID          ");
            parameter.AppendSql("     , DeCode(SUBSTR(a.GbSangDam,1,1),'A','OK','Y','OK','C','OK','') Arrive            ");
            parameter.AppendSql(" FROM HEA_JEPSU a,HIC_PATIENT b                                                        ");
            parameter.AppendSql("WHERE a.IDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                        ");
            parameter.AppendSql("  AND a.IDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                        ");
            parameter.AppendSql("  AND a.DELDATE IS NULL                                                                ");
            parameter.AppendSql("  AND a.GbSTS NOT IN ('0','D')                                                         ");
            //성명조회
            if (strName.Trim() != "")
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                         ");
            }
            //상담구분
            if (strGbn == "1")
            {
                parameter.AppendSql("   AND a.GBSANGDAM IN ('Y')                                                        ");
            }
            else if (strGbn == "2")
            {
                parameter.AppendSql("   AND (a.GbSangDam IS NULL OR a.GbSangDam <> 'Y' )                                ");
            }
            if (strJong != "00")
            {
                parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                          ");
            }
            if (strLtdCode != "")
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                        ");
            }
            parameter.AppendSql("   AND a.Pano = b.Pano(+)                                                              ");
            parameter.AppendSql(" ORDER BY a.IDate, a.EntTime ASC , a.SName, a.Pano, a.GjJong                           ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (strName.Trim() != "")
            {
                parameter.Add("SNAME", strToDate);
            }
            if (strJong != "00")
            {
                parameter.Add("GJJONG", strToDate, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (strLtdCode != "")
            {
                parameter.Add("LTDCODE", strToDate);
            }

            return ExecuteReader<HEA_JEPSU_PATIENT>(parameter);
        }

        public HEA_JEPSU_PATIENT GetItembyPano(long nPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.SNAME, a.PTNO, b.JUMIN, a.SEX, a.AGE  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU   a               ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT b               ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                          ");
            parameter.AppendSql("   AND a.PANDATE IS NULL                       ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                       ");
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                      ");

            parameter.Add("PANO", nPano);

            return ExecuteReaderSingle<HEA_JEPSU_PATIENT>(parameter);
        }

        public List<HEA_JEPSU_PATIENT> GetItembySDate(string strBDate, long nSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.SDATE,'YYYY-MM-DD') SDate,a.WRTNO,a.Pano,a.Sname,b.Sex            ");
            parameter.AppendSql("     , b.Ptno,b.JUSO1 || b.JUSO2 JUSO                                              ");
            parameter.AppendSql("     , b.JUMIN2,DECODE(a.AMPM,'A','1','P','2','1') as AMPM2                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU   a                                                   ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT b                                                   ");
            parameter.AppendSql(" WHERE a.SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')                                     ");
            if (nSabun != 23515)
            {
                parameter.AppendSql("   AND a.Pano <> 999                                                           ");
            }
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                           ");
            parameter.AppendSql("   AND a.Pano = b.Pano(+)                                                          ");
            parameter.AppendSql(" GROUP By a.SDATE, a.WRTNO, a.Pano, a.Sname, b.Sex ,b.Ptno, b.JUSO1, b.JUSO2       ");
            parameter.AppendSql("     , b.JUMIN2, a.AMPM                                                            ");
            parameter.AppendSql(" ORDER BY a.AMPM,a.Sname                                                           ");

            parameter.Add("SDATE", strBDate);

            return ExecuteReader<HEA_JEPSU_PATIENT>(parameter);
        }

        public List<HEA_JEPSU_PATIENT> GetItembySName(string strFrDate, string strToDate, long nLtdCode, string strSName, string strSend, string strSort)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.PTNO, a.WRTNO,a.SNAME,TO_CHAR(a.SDATE,'YYYY-MM-DD') SDATE     ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(a.LTDCODE) LTDNAME                   ");
            parameter.AppendSql("     , a.MAILWEIGHT, TO_CHAR(a.PRTDATE,'YYYY-MM-DD') PRTDATE           ");
            parameter.AppendSql("     , TO_CHAR(a.RECVDATE,'YYYY-MM-DD') RECVDATE                       ");
            parameter.AppendSql("     , TO_CHAR(a.MAILDATE,'YYYY-MM-DD') MAILDATE                       ");
            parameter.AppendSql("     , a.PANO, b.FAMILLY                                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU   a                                       ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT b                                       ");
            parameter.AppendSql(" WHERE a.SDATE >= TO_DATE(:SDATEFR, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("   AND a.SDATE <= TO_DATE(:SDATETO, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                               ");
            parameter.AppendSql("   AND a.GBSTS IN ('3','9')                                            "); //판정이 완료된것만
            parameter.AppendSql("   AND a.PANO = b.PANO                                                 ");
            parameter.AppendSql("   AND a.DRSABUN > 0                                                   ");
            if (strSend == "1") //미발송명단(미수령포함)
            {
                parameter.AppendSql("   AND (a.MailDATE IS NULL OR MailDATE ='')                        ");
                parameter.AppendSql("   AND (a.RecvDATE IS NULL OR RecvDATE ='')                        ");
            }
            else if (strSend == "2") //발송명단(내원수령포함)
            {
                parameter.AppendSql("   AND ( a.MailDATE IS NOT NULL OR a.RecvDATE IS NOT NULL )        ");
            }
            else if (strSend == "4")    //미출력명단
            {
                parameter.AppendSql("   AND(a.PRTDATE IS NULL OR a.PRTDATE = '')                        ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                        ");
            }
            if (strSName != "")
            {
                parameter.AppendSql("   AND a.SNAME LIKE   :SNAME                                       ");
            }
            if (strSort == "1")
            {
                parameter.AppendSql(" ORDER BY a.SDate, a.SName                                         ");
            }
            else if (strSort == "2")
            {
                parameter.AppendSql(" ORDER BY a.SName, a.LtdCode                                       ");
            }
            else if (strSort == "3")
            {
                parameter.AppendSql(" ORDER BY a.WRTNO, a.SName                                         ");
            }
            else if (strSort == "4")
            {
                parameter.AppendSql(" ORDER BY a.LtdCode, a.SDate, a.Sname                              ");
            }

            parameter.Add("SDATEFR", strFrDate);
            parameter.Add("SDATETO", strToDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            if (strSName != "")
            {
                parameter.AddLikeStatement("SNAME", strSName);
            }

            return ExecuteReader<HEA_JEPSU_PATIENT>(parameter);
        }

        public List<HEA_JEPSU_PATIENT> GetItemByHanwol(string strFDate, string strTDate, long nLtdcode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.SNAME, B.JUMIN, A.SDATE, A.SEX, A.AGE, A.WRTNO        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU A,                                ");
            parameter.AppendSql("  KOSMOS_PMPA.HIC_PATIENT B                                    ");
            parameter.AppendSql(" WHERE 1 = 1                                                   ");
            parameter.AppendSql(" AND A.PTNO = B.PTNO                                           ");
            parameter.AppendSql(" AND A.SDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                  ");
            parameter.AppendSql(" AND A.SDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                  ");
            parameter.AppendSql(" AND A.LTDCODE = :LTDCODE                                      ");
            parameter.AppendSql(" AND A.DELDATE IS NULL                                         ");
            parameter.AppendSql(" AND(A.PANREMARK LIKE '%결절%' OR A.PANREMARK LIKE '%잘모른다%' OR A.PANREMARK LIKE '%암의심%' OR A.PANREMARK LIKE '%악성%' OR A.PANREMARK LIKE '%선종%'OR A.PANREMARK LIKE '%석회화%'         ");
            parameter.AppendSql(" OR A.PANREMARK2 LIKE '%결절%' OR A.PANREMARK2 LIKE '%잘모른다%' OR A.PANREMARK2 LIKE '%암의심%' OR A.PANREMARK2 LIKE '%악성%' OR A.PANREMARK2 LIKE '%선종%' OR A.PANREMARK2 LIKE '%석회화%')  ");
            parameter.AppendSql(" ORDER BY A.SDATE, A.SNAME                                     ");

            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);
            parameter.Add("LTDCODE", nLtdcode);

            return ExecuteReader<HEA_JEPSU_PATIENT>(parameter);
        }


    }
}
