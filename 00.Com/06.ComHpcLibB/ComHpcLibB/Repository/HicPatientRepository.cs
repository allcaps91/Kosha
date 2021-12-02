namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicPatientRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicPatientRepository()
        {
        }

        public long Read_HicPano()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT KOSMOS_PMPA.SEQ_HICPANO.NEXTVAL HicPano FROM DUAL ");

            return ExecuteScalar<long>(parameter);
        }

        /// <summary>
        /// Read_HeaWrtNo() / Read_HicWrtNo() Merge
        /// </summary>
        /// <returns></returns>
        public long Read_HicWrtNo()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT KOSMOS_PMPA.SEQ_HICJEPNO.NEXTVAL HicWRTNO FROM DUAL ");

            return ExecuteScalar<long>(parameter);
        }

        public long Read_HicGWrtNo()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT KOSMOS_PMPA.SEQ_HICGWRTNO.NEXTVAL HicWRTNO FROM DUAL ");

            return ExecuteScalar<long>(parameter);
        }

        public HIC_PATIENT GetHcPatInfoByPano(long nPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO,SNAME,JUMIN,SEX,MAILCODE,JUSO1,JUSO2,TEL,LTDCODE,JIKGBN,JIKJONG                     ");
            parameter.AppendSql("      ,SABUN, BUSENAME, GONGJENG, IPSADATE, BUSEIPSA, JISA, GKIHO, GBSUCHEP, PTNO, REMARK, KIHO ");
            parameter.AppendSql("      ,BOGUNSO, YOUNGUPSO, LIVER2, GUMDAESANG, EMAIL, HPHONE, GBIEMUNJIN, GBSMS, TEL_CONFIRM    ");
            parameter.AppendSql("      ,JUMIN2, GBPRIVACY, BUILDNO, LTDCODE2, BIRTHDAY, GBBIRTH, LTDTEL, GAMCODE, RELIGION       ");
            parameter.AppendSql("      ,STARTDATE, LASTDATE, JINCOUNT, FAMILLY, GAMCODE2, SOSOK, VIPREMARK, GBJIKWON             ");
            parameter.AppendSql("      ,GBFOREIGNER, ENAME, SNAME2, FOREIGNERNUM, GBPRIVACY_NEW, WORKER_ROLE, ISMANAGEOSHA       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT                                                                  ");
            parameter.AppendSql(" WHERE PANO =:PANO ");

            parameter.Add("PANO", nPano);

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public HIC_PATIENT GetHcPatInfoByPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO,SNAME,JUMIN,SEX,MAILCODE,JUSO1,JUSO2,TEL,LTDCODE,JIKGBN,JIKJONG                     ");
            parameter.AppendSql("      ,SABUN, BUSENAME, GONGJENG, IPSADATE, BUSEIPSA, JISA, GKIHO, GBSUCHEP, PTNO, REMARK, KIHO ");
            parameter.AppendSql("      ,BOGUNSO, YOUNGUPSO, LIVER2, GUMDAESANG, EMAIL, HPHONE, GBIEMUNJIN, GBSMS, TEL_CONFIRM    ");
            parameter.AppendSql("      ,JUMIN2, GBPRIVACY, BUILDNO, LTDCODE2, BIRTHDAY, GBBIRTH, LTDTEL, GAMCODE, RELIGION       ");
            parameter.AppendSql("      ,STARTDATE, LASTDATE, JINCOUNT, FAMILLY, GAMCODE2, SOSOK, VIPREMARK, GBJIKWON             ");
            parameter.AppendSql("      ,GBFOREIGNER, ENAME, SNAME2, FOREIGNERNUM, GBPRIVACY_NEW, WORKER_ROLE, ISMANAGEOSHA       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT                                                                  ");
            parameter.AppendSql(" WHERE PTNO =:PTNO                                                                             ");
            parameter.AppendSql("   AND (SNAME IS NULL OR  SNAME <> '이중챠트')                                                  ");

            parameter.Add("PTNO", argPtno, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public long Read_Jumin_HicPano(string argJumin)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO                ");
            parameter.AppendSql("  FROM HIC_PATIENT         ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2    ");

            parameter.Add("JUMIN2", argJumin);

            return ExecuteScalar<long>(parameter);
        }

        public HIC_PATIENT GetPaNobyPtNo(string argPtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO                ");
            parameter.AppendSql("  FROM HIC_PATIENT         ");
            parameter.AppendSql(" WHERE PTNO = :PTNO        ");

            parameter.Add("PTNO", argPtNo);

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public string GetLtdNameByJumin2(string strJumin)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT KOSMOS_PMPA.FC_HIC_LTDNAMEPANO(LTDCODE) AS NAME     ");
            parameter.AppendSql("  FROM HIC_PATIENT                                         ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2                                    ");

            parameter.Add("JUMIN2", strJumin);

            return ExecuteScalar<string>(parameter);
        }

        public string GetPrivacyNewByPtno(string fstrPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(GBPRIVACY_NEW, 'YYYY-MM-DD') GBPRIVACY_NEW  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT                             ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                        ");

            parameter.Add("PTNO", fstrPtno);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_PATIENT> GetDoubleChartSearch()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO                ");
            parameter.AppendSql("  FROM HIC_PATIENT         ");
            parameter.AppendSql(" WHERE SNAME = '이중챠트'  ");

            return ExecuteReader<HIC_PATIENT>(parameter);
        }

        public int InsertHicPatientOverLap(long nPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO HIC_PATIENT_OVERLAP (                                                      ");
            parameter.AppendSql("     , PANO,SNAME,JUMIN,SEX,MAILCODE,JUSO1,JUSO2,TEL,LTDCODE,JIKGBN,JIKJONG            ");
            parameter.AppendSql("     , SABUN,BUSENAME,GONGJENG,IPSADATE,BUSEIPSA,JISA,GKIHO,GBSUCHEP,PTNO,REMARK,KIHO  ");
            parameter.AppendSql("     , BOGUNSO,YOUNGUPSO,LIVER2,GUMDAESANG,EMAIL,HPHONE,GBIEMUNJIN,GBSMS,TEL_CONFIRM   ");
            parameter.AppendSql("     , JUMIN2,GBPRIVACY)                                                               ");
            parameter.AppendSql("SELECT PANO,SNAME,JUMIN,SEX,MAILCODE,JUSO1,JUSO2,TEL,LTDCODE,JIKGBN,JIKJONG            ");
            parameter.AppendSql("     , SABUN,BUSENAME,GONGJENG,IPSADATE,BUSEIPSA,JISA,GKIHO,GBSUCHEP,PTNO,REMARK,KIHO  ");
            parameter.AppendSql("     , BOGUNSO,YOUNGUPSO,LIVER2,GUMDAESANG,EMAIL,HPHONE,GBIEMUNJIN,GBSMS,TEL_CONFIRM   ");
            parameter.AppendSql("     , JUMIN2,GBPRIVACY                                                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT                                                         ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                                    ");

            parameter.Add("PANO", nPano);

            return ExecuteNonQuery(parameter);
        }

        public int DeletebyPano(long nPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_PATIENT     ");
            parameter.AppendSql(" WHERE PANO  = :PANO               ");

            parameter.Add("PANO", nPano);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebySNamePaNo(string strSName, long nPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_PATIENT SET         ");
            parameter.AppendSql("       SNAME = :SNAME                      ");
            parameter.AppendSql(" WHERE PANO  = :PANO                       ");

            parameter.Add("SNAME", strSName);
            parameter.Add("PANO", nPano);

            return ExecuteNonQuery(parameter);
        }

        public long Read_MisuNo()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SEQ_HIC_MISUNO.NEXTVAL HicWRTNO FROM DUAL       ");
            
            return ExecuteScalar<long>(parameter);

        }

        public List<HIC_PATIENT> GetDblChartbyJumin2(string strJumin2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME, PANO, LTDCODE, PTNO  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT     ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2            ");
            parameter.AppendSql("   AND SNAME <> '이중챠트'         ");

            parameter.Add("JUMIN2", strJumin2);

            return ExecuteReader<HIC_PATIENT>(parameter);
        }

        public HIC_PATIENT GetItembyName(string fstrPtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME, JUMIN2               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT     ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                ");
            parameter.AppendSql("   AND SNAME <> '이중챠트'         ");

            parameter.Add("PTNO", fstrPtNo);

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public List<HIC_PATIENT> GetItembySabun(string argSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME, JUMIN, JUMIN2, PTNO  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT     ");
            parameter.AppendSql(" WHERE SABUN = :SABUN              ");
            parameter.AppendSql("   AND SNAME <> '이중챠트'          ");

            parameter.Add("SABUN", argSabun);

            return ExecuteReader<HIC_PATIENT>(parameter);
        }

        public List<HIC_PATIENT> GetItembySomeone(string strJob, string strSName, List<string> b04_NOT_PATIENT, string strGubun)
        {
            MParameter parameter = CreateParameter();

            if (strJob == "0")  //성명
            {
                parameter.AppendSql("SELECT PANO,SNAME,JUMIN,JUMIN2,PTNO                                ");
                parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(LTDCODE) LTDNAME                 ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT a                                   ");
                parameter.AppendSql(" WHERE SNAME LIKE :SNAME                                           ");
            }
            else if (strJob == "1") //주민번호
            {
                parameter.AppendSql("SELECT PANO,SNAME,JUMIN,JUMIN2,PTNO                                ");
                parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(LTDCODE) LTDNAME                 ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT a                                   ");
                parameter.AppendSql(" WHERE JUMIN2 = :SNAME                                             ");
                if (b04_NOT_PATIENT.Count > 0)
                {
                    parameter.AppendSql("   AND SNAME NOT IN (:B04_NOT_PATIENT)                         ");
                }
                parameter.AppendSql(" order by jumin                                                    ");
            }
            else if (strJob == "2") //회사코드
            {
                parameter.AppendSql("SELECT PANO, SNAME,JUMIN,JUMIN2,PTNO                               ");
                parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(LTDCODE) LTDNAME                 ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT a                                   ");
                parameter.AppendSql(" WHERE LTDCODE = :SNAME                                            ");
                if (b04_NOT_PATIENT.Count > 0)
                {
                    parameter.AppendSql("   AND SNAME NOT IN (:B04_NOT_PATIENT)                         ");
                }
                parameter.AppendSql(" order by sname                                                    ");
            }
            else if (strJob == "3") //검진번호
            {
                parameter.AppendSql("SELECT PANO,SNAME,JUMIN,JUMIN2,PTNO                                ");
                parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(LTDCODE) LTDNAME                 ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT a                                   ");
                parameter.AppendSql(" WHERE PANO = :SNAME                                               ");
                parameter.AppendSql("   AND SNAME <> '이중챠트'                                         ");
            }
            else if (strJob == "4") //접수번호
            {
                if (strGubun == "1")
                {
                    parameter.AppendSql("SELECT a.Pano,a.SName,b.Jumin,b.Jumin2,a.PTNO                      ");
                    parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(b.LTDCODE) LTDNAME               ");
                    parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_PATIENT b          ");
                    //parameter.AppendSql(" WHERE a.Pano = b.Pano                                             ");
                    parameter.AppendSql(" WHERE a.PANO = b.Pano                                            ");
                    parameter.AppendSql("   AND a.WRTNO = :SNAME                                            ");
                    if (b04_NOT_PATIENT.Count > 0)
                    {
                        parameter.AppendSql("   AND B.SNAME NOT IN (:B04_NOT_PATIENT)                       ");
                    }
                    parameter.AppendSql(" GROUP BY a.Pano,a.SName,b.Jumin,b.Jumin2,a.Ptno                   ");
                    parameter.AppendSql("        , KOSMOS_PMPA.FC_HIC_LTDNAME(b.LTDCODE)                    ");
                    parameter.AppendSql(" UNION ALL                                                         ");
                    parameter.AppendSql("SELECT a.Pano,a.SName,b.Jumin,b.Jumin2,a.PTNO                      ");
                    parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(b.LTDCODE) LTDNAME               ");
                    parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a, KOSMOS_PMPA.HIC_PATIENT b          ");
                    //parameter.AppendSql(" WHERE a.Pano = b.Pano                                             ");
                    parameter.AppendSql(" WHERE a.PANO = b.Pano                                            ");
                    parameter.AppendSql("   AND a.WRTNO = :SNAME                                            ");
                    if (b04_NOT_PATIENT.Count > 0)
                    {
                        parameter.AppendSql("   AND B.SNAME NOT IN (:B04_NOT_PATIENT)                       ");
                    }
                    parameter.AppendSql(" GROUP BY a.Pano,a.SName,b.Jumin,b.Jumin2,a.Ptno                   ");
                    parameter.AppendSql("      , KOSMOS_PMPA.FC_HIC_LTDNAME(b.LTDCODE)                      ");
                }
                else
                {
                    if (strGubun == "2")    //일검
                    {
                        parameter.AppendSql("SELECT a.Pano,a.SName,b.Jumin,b.Jumin2,a.PTNO                  ");
                        parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(b.LTDCODE) LTDNAME           ");
                        parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_PATIENT b      ");
                    }
                    else if (strGubun == "3") //종검
                    {
                        parameter.AppendSql("SELECT a.Pano,a.SName,b.Jumin,b.Jumin2,a.PTNO                  ");
                        parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(b.LTDCODE) LTDNAME           ");
                        parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a, KOSMOS_PMPA.HIC_PATIENT b      ");
                    }
                    //parameter.AppendSql(" WHERE a.Pano = b.Pano                                             ");
                    parameter.AppendSql(" WHERE a.PANO = b.Pano                                            ");
                    parameter.AppendSql("   AND a.WRTNO = :SNAME                                            ");
                    if (b04_NOT_PATIENT.Count > 0)
                    {
                        parameter.AppendSql("   AND B.SNAME NOT IN (:B04_NOT_PATIENT)                       ");
                    }
                    parameter.AppendSql(" GROUP BY a.Pano,a.SName,b.Jumin,b.Jumin2,a.Ptno                   ");
                    parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(b.LTDCODE)                       ");
                }
            }
            else if (strJob == "5") //등록번호
            {   
                parameter.AppendSql("SELECT b.PANO,b.SNAME,b.JUMIN,b.JUMIN2,b.PTNO                          ");
                parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(b.LTDCODE) LTDNAME                   ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT b                                       ");
                parameter.AppendSql(" WHERE b.PTNO = :SNAME                                                 ");
                if (b04_NOT_PATIENT.Count > 0)
                {
                    parameter.AppendSql("   AND b.SNAME NOT IN (:B04_NOT_PATIENT)                           ");
                }
            }
            else if (strJob == "6") //휴대폰
            {
                parameter.AppendSql("SELECT b.PANO,b.SNAME,b.JUMIN,b.JUMIN2,b.PTNO                          ");
                parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(b.LTDCODE) LTDNAME                   ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT b                                       ");
                parameter.AppendSql(" WHERE b.HPhone = :SNAME                                               ");
                if (!b04_NOT_PATIENT.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND b.SNAME NOT IN (:B04_NOT_PATIENT)                           ");
                }
            }

            if (strJob == "4") //접수번호
            {
                if (strGubun == "2")    //일검
                {
                    parameter.AppendSql("   AND A.PTNO IN (SELECT UNIQUE PTNO FROM KOSMOS_PMPA.HIC_JEPSU WHERE PTNO = a.PTNO)   ");
                }
                else if (strGubun == "3")    //종검
                {
                    parameter.AppendSql("   AND A.PTNO IN (SELECT UNIQUE PTNO FROM KOSMOS_PMPA.HEA_JEPSU WHERE PTNO = a.PTNO)   ");
                }
            }

            if (strJob == "5" || strJob == "6")
            {
                parameter.AppendSql(" GROUP BY b.PANO,b.SNAME,b.JUMIN,b.JUMIN2,b.PTNO, KOSMOS_PMPA.FC_HIC_LTDNAME(b.LTDCODE)    ");
            }

            if (strJob == "0")
            {
                parameter.AddLikeStatement("SNAME", strSName);
            }
            else
            {
                parameter.Add("SNAME", strSName);
            }

            if (strJob != "0" && strJob != "3")
            {
                if (b04_NOT_PATIENT.Count > 0)
                {
                    parameter.AddInStatement("B04_NOT_PATIENT", b04_NOT_PATIENT);
                }
            }

            return ExecuteReader<HIC_PATIENT>(parameter);
        }

        public string GetJumin2byPtno(string strPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT JUMIN2 FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql(" WHERE PTNO =:PTNO                         ");

            parameter.Add("PTNO", strPtno);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_PATIENT GetJumin2PtNobyPaNo(long fnPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT JUMIN2, PTNO FROM KOSMOS_PMPA.HIC_PATIENT   ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");

            parameter.Add("PANO", fnPano);

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public string GetJumin2BySnameJuminLikeLtdCode(string strSName, string argJumin)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT JUMIN2 FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql(" WHERE SNAME =:SNAME                       ");
            parameter.AppendSql("   AND JUMIN LIKE (:JUMIN)                 ");

            parameter.Add("SNAME", strSName);
            parameter.AddLikeStatement("JUMIN", argJumin);

            return ExecuteScalar<string>(parameter);
        }

        public string GetHphonebyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.HPHONE                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   a           ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT b           ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                    ");
            parameter.AppendSql("   AND a.PTNO = b.PTNO                     ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_PATIENT GetItembyJumin2NotInSName(string strJumin, List<string> b04_NOT_PATIENT)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SNAME,JUSO1,JUSO2,TEL,HPHONE,PANO,PTNO,JUMIN        ");
            parameter.AppendSql("      ,LTDCODE,TO_CHAR(GBPRIVACY,'YYYY-MM-DD') GBPRIVACY   ");
            parameter.AppendSql("      ,KOSMOS_PMPA.FC_HIC_LTDNAME(LTDCODE) AS LTDNAME      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT                             ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2                                    ");
            
            if (b04_NOT_PATIENT.Count > 0)
            {
                parameter.AppendSql("   AND SNAME NOT IN (:SNAME)           ");
            }

            parameter.Add("JUMIN2", strJumin);

            if (b04_NOT_PATIENT.Count > 0)
            {
                parameter.AddInStatement("SNAME", b04_NOT_PATIENT);
            }

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public List<HIC_PATIENT> GetDblCharSearch()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(PANO) CNT, JUMIN2, SNAME         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT             ");
            parameter.AppendSql(" WHERE PANO > 0                            ");
            parameter.AppendSql("   AND JUMIN2 IS NOT NULL                  ");
            parameter.AppendSql("   AND SNAME <> '이중챠트'                 ");
            parameter.AppendSql(" GROUP BY JUMIN2,SNAME                     ");
            parameter.AppendSql("HAVING COUNT(PANO) > 1                        ");

            return ExecuteReader<HIC_PATIENT>(parameter);
        }

        public string GetSNameByPano(long argPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql(" WHERE PANO = :PANO            ");

            parameter.Add("PANO", argPano);

            return ExecuteScalar<string>(parameter);
        }

        public void UpdateItemsByPaNo(HIC_PATIENT iHP)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_PATIENT     ");
            parameter.AppendSql("   SET MAILCODE = :MAILCODE        ");
            parameter.AppendSql("      ,JUSO1   = :JUSO1            ");
            parameter.AppendSql("      ,JUSO2   = :JUSO2            ");
            parameter.AppendSql("      ,SEX     = :SEX              ");
            parameter.AppendSql("      ,SNAME   = :SNAME            ");
            parameter.AppendSql("      ,TEL     = :TEL              ");
            parameter.AppendSql("      ,HPHONE  = :HPHONE           ");
            parameter.AppendSql("      ,PTNO    = :PTNO             ");
            parameter.AppendSql(" WHERE PANO = :PANO                ");

            parameter.Add("MAILCODE", iHP.MAILCODE);
            parameter.Add("JUSO1",  iHP.JUSO1);
            parameter.Add("JUSO2",  iHP.JUSO2);
            parameter.Add("SEX",    iHP.SEX);
            parameter.Add("SNAME",  iHP.SNAME);
            parameter.Add("TEL",    iHP.TEL);
            parameter.Add("HPHONE", iHP.HPHONE);
            parameter.Add("PTNO",   iHP.PTNO);
            parameter.Add("PANO",   iHP.PANO);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_PATIENT> GetCountbyPtNo(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT JUMIN, JUMIN2, SNAME, PANO, PTNO    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT             ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                        ");
            parameter.AppendSql("   AND SNAME <> '이중챠트'                 ");

            parameter.Add("PTNO", argPtno);

            return ExecuteReader<HIC_PATIENT>(parameter);
        }

        public long GetPanobyPtno(string strPtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql(" WHERE PTNO = :PTNO            ");
            parameter.AppendSql(" ORDER BY Pano                 ");

            parameter.Add("PTNO", strPtNo, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<long>(parameter);
        }

        public int UpdatePtNobyPaNo(string strPtNo, string strPANO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql("   SET PTNO = :PTNO            ");
            parameter.AppendSql(" WHERE PANO = :PANO            ");

            parameter.Add("PTNO", strPtNo);
            parameter.Add("PANO", strPANO);

            return ExecuteNonQuery(parameter);
        }

        public int InsertItem(long fnPano, string argJumin, string argJuminAes, string argSName, string argTel, string argPaNo, string argSex, long argLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_PATIENT                                    ");
            parameter.AppendSql("       (PANO, JUMIN, JUMIN2, SNAME, TEL, PTNO, SEX, LTDCODE)           ");
            parameter.AppendSql("VALUES                                                                 ");
            parameter.AppendSql("       (:PANO, :JUMIN, :JUMIN2, :SNAME, :TEL, :PTNO, :SEX, :LTDCODE)   ");

            parameter.Add("PANO", fnPano);
            parameter.Add("JUMIN", argJumin, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMIN2", argJuminAes);
            parameter.Add("SNAME", argSName);
            parameter.Add("TEL", argTel);
            parameter.Add("PTNO", argPaNo, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("SEX", argSex, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", argLtdCode);
            

            return ExecuteNonQuery(parameter);
        }

        public HIC_PATIENT GetPaNoLtdCodebytJumin2(string strJumin2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, LTDCODE           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2        ");

            parameter.Add("JUMIN2", strJumin2);

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public int UpdateLtdCodebyPano(long nLtdCode, long nPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_PATIENT     ");
            parameter.AppendSql("   SET LTDCODE = :LTDCODE          ");
            parameter.AppendSql(" WHERE PANO    = :PANO             ");

            parameter.Add("LTDCODE", nLtdCode);
            parameter.Add("PANO", nPano);

            return ExecuteNonQuery(parameter);
        }

        public HIC_PATIENT GetItembyJumin2(string argJumin2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO,PTNO, LTDCODE,TO_CHAR(IPSADATE,'YYYY-MM-DD') IPSADATE, BUSEIPSA, JISA, GKIHO   ");
            parameter.AppendSql("     , GBSUCHEP, KIHO, BOGUNSO, MAILCODE, JUSO1, JUSO2, YOUNGUPSO, LIVER2, TEL, HPHONE     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT                                                             ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2                                                                    ");

            parameter.Add("JUMIN2", argJumin2, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public int GetCountbyPaNo(string strPANO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(PANO) CNT          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql(" WHERE PANO = :PANO            ");

            parameter.Add("PANO", strPANO);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdatebyPtno(string strGongjeng, string strBuseName, string strSaBun, string strHPhone, string strGkiho, string strPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_PATIENT SET             ");
            parameter.AppendSql("       BUSENAME = :BUSENAME                    ");
            if (!strGongjeng.IsNullOrEmpty())
            {
                parameter.AppendSql("     , GONGJENG = :GONGJENG                ");
            }
            parameter.AppendSql("     , SABUN = :SABUN                          ");
            //2020-06-17(가접수시 휴대폰 번호 환자마스터 업데이트 추가)
            if (!strHPhone.IsNullOrEmpty())
            {
                parameter.AppendSql("     , HPHONE = :HPHONE                    ");
            }
            parameter.AppendSql("     , GKIHO = :GKIHO                          ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                           ");

            parameter.Add("BUSENAME", strBuseName);
            if (!strGongjeng.IsNullOrEmpty())
            {
                parameter.Add("GONGJENG", strGongjeng);
            }
            parameter.Add("SABUN", strSaBun);
            if (!strHPhone.IsNullOrEmpty())
            {
                parameter.Add("HPHONE", strHPhone);
            }
            parameter.Add("GKIHO", strGkiho);
            parameter.Add("PTNO", strPtNo);

            return ExecuteNonQuery(parameter);
        }

        public HIC_PATIENT GetLtdCodebyPaNo(string argPaNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT LTDCODE FROM KOSMOS_PMPA.HIC_PATIENT WHERE PANO = :PANO     ");

            parameter.Add("PANO", argPaNo);

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public int UpdatePtNobyJumin2(string pANO, string argJumin)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_PATIENT     ");
            parameter.AppendSql("   SET PTNO   = :PTNO              ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2            ");

            parameter.Add("PTNO", pANO);
            parameter.Add("JUMIN2", argJumin);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_PATIENT> GetItembyName(string strJob, string strSName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO,SNAME,JUMIN,JUMIN2,Ptno                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT                                     ");
            if (strJob == "1")
            {
                parameter.AppendSql(" WHERE SNAME LIKE :SNAME                                       ");
            }
            else if (strJob == "2")
            {
                parameter.AppendSql(" WHERE JUMIN  = :SNAME                                         ");
                parameter.AppendSql(" order by jumin                                                ");
            }
            else if (strJob == "3")
            {
                parameter.AppendSql(" WHERE LTDCODE = :SNAME                                        ");
                parameter.AppendSql(" order by sname                                                ");
            }
            else if (strJob == "4")
            {
                parameter.AppendSql(" WHERE PANO = :SNAME                                           ");
            }

            parameter.AddLikeStatement("SNAME", strSName);

            return ExecuteReader<HIC_PATIENT>(parameter);
        }

        public long GetPanobyJumin(string strJumin)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(PANO) AS PANO       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2        ");
            parameter.AppendSql(" ORDER BY Pano                 ");

            parameter.Add("JUMIN2", strJumin);

            return ExecuteScalar<long>(parameter);
        }

        public HIC_PATIENT GetPatInfoByPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO,SNAME,JUMIN,SEX,MAILCODE,JUSO1,JUSO2,TEL,LTDCODE,JIKGBN,JIKJONG                     ");
            parameter.AppendSql("      ,SABUN, BUSENAME, GONGJENG, TO_CHAR(IPSADATE, 'YYYY-MM-DD') IPSADATE                      ");
            parameter.AppendSql("      ,TO_CHAR(BUSEIPSA, 'YYYY-MM-DD') BUSEIPSA, JISA, GKIHO, GBSUCHEP, PTNO                    ");
            parameter.AppendSql("      ,REMARK AS PAT_REMARK, KIHO, KOSMOS_PMPA.FC_HIC_LTDNAME(LTDCODE) AS LTDNAME               ");
            parameter.AppendSql("      ,BOGUNSO, YOUNGUPSO, LIVER2, GUMDAESANG, EMAIL, HPHONE, GBIEMUNJIN, GBSMS, TEL_CONFIRM    ");
            parameter.AppendSql("      ,JUMIN2, GBPRIVACY, BUILDNO, LTDCODE2, BIRTHDAY, GBBIRTH, LTDTEL, GAMCODE, RELIGION       ");
            parameter.AppendSql("      ,STARTDATE, LASTDATE, JINCOUNT, FAMILLY, GAMCODE2, SOSOK, VIPREMARK, GBJIKWON             ");
            parameter.AppendSql("      ,SEX || '/' || KOSMOS_OCS.FC_GET_AGE2(PTNO, TRUNC(SYSDATE)) AS S_AGE                      ");
            parameter.AppendSql("      ,GBFOREIGNER, ENAME, SNAME2, FOREIGNERNUM, GBPRIVACY_NEW, WORKER_ROLE, ISMANAGEOSHA       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT                                                                  ");
            parameter.AppendSql(" WHERE PTNO =:PTNO                                                                              ");
            parameter.AppendSql("   AND (SNAME IS NULL OR  SNAME <> '이중챠트')                                                  ");

            parameter.Add("PTNO", argPtno, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_PATIENT SET             ");
            parameter.AppendSql("       SNAME = '이중챠트'                       ");
            parameter.AppendSql(" WHERE PANO IN (SELECT PANO                    ");
            parameter.AppendSql("                  FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql("                 WHERE JUMIN2 = :JUMIN2        ");
            parameter.AppendSql("                   AND PANO <> :PANO)          ");

            parameter.Add("PANO", argPaNo);
            parameter.Add("JUMIN2", argJumin2);

            return ExecuteNonQuery(parameter);
        }

        public string GetPtnoByPano(long nPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PTNO FROM KOSMOS_PMPA.HIC_PATIENT  ");
            parameter.AppendSql(" WHERE PANO =:PANO                        ");

            parameter.Add("PANO", nPano);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_PATIENT GetPanoPtnoByLikeJuminSNameLtdCode(string argBIRTH, string argSNAME, long argLTDCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, PTNO FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql(" WHERE JUMIN LIKE :BIRTH                      ");
            parameter.AppendSql("   AND SNAME =:SNAME                           ");
            parameter.AppendSql("   AND LTDCODE =:LTDCODE                           ");
            parameter.AppendSql("   AND (SNAME IS NULL OR  SNAME <> '이중챠트') ");

            parameter.AddLikeStatement("BIRTH", argBIRTH);
            parameter.Add("SNAME", argSNAME);
            parameter.Add("LTDCODE", argLTDCODE);

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public HIC_PATIENT GetPanoPtnoByJumin2SName(string argJUMIN, string argSName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, PTNO FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql(" WHERE JUMIN2 =:JUMIN2                         ");
            parameter.AppendSql("   AND SNAME =:SNAME                         ");
            parameter.AppendSql("   AND (SNAME IS NULL OR  SNAME <> '이중챠트') ");

            parameter.Add("JUMIN2", argJUMIN);
            parameter.Add("SNAME", argSName);

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public string GetPtnoByJuminNo(string argJumin)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PTNO FROM KOSMOS_PMPA.HIC_PATIENT       ");
            parameter.AppendSql(" WHERE JUMIN2 =:JUMIN2                         ");
            parameter.AppendSql("   AND (SNAME IS NULL OR  SNAME <> '이중챠트')  ");

            parameter.Add("JUMIN2", argJumin);

            return ExecuteScalar<string>(parameter);
        }
        public string GetPanoByJuminNo(string argJumin)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO FROM KOSMOS_PMPA.HIC_PATIENT       ");
            parameter.AppendSql(" WHERE JUMIN2 =:JUMIN2                         ");
            parameter.AppendSql("   AND (SNAME IS NULL OR  SNAME <> '이중챠트')  ");

            parameter.Add("JUMIN2", argJumin);

            return ExecuteScalar<string>(parameter);
        }

        public string GetPanobyJumin2(string strJumin2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO FROM KOSMOS_PMPA.HIC_PATIENT       ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2                        ");

            parameter.Add("JUMIN2", strJumin2);

            return ExecuteScalar<string>(parameter);
        }

        public int InsertPatient(long nPano, string strJumin, string strJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_PATIENT    ");
            parameter.AppendSql("       (PANO, JUMIN, JUMIN2)           ");
            parameter.AppendSql("VALUES                                 ");
            parameter.AppendSql("       (:PANO, :JUMIN, :JUMIN2)        ");

            parameter.Add("PANO", nPano);
            parameter.Add("JUMIN", strJumin, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMIN2", strJumin2);

            return ExecuteNonQuery(parameter);
        }

        public HIC_PATIENT GetJusobyPano(long nPano, string argSName = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TEL, JUSO1, JUSO2, LTDCODE, JUMIN2, HPHONE      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT                         ");
            parameter.AppendSql(" WHERE PANO =:PANO                                    ");
            if (argSName != "")
            {
                parameter.AppendSql("   AND SNAME =:SNAME                              ");
            }
            parameter.Add("PANO", nPano);
            if (argSName != "")
            {
                parameter.Add("SNAME", argSName);
            }

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public List<HIC_PATIENT> GetHicListByItem(string strSName, string strPtno, long nLtdCode, string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT b.JUMIN, b.PTNO, b.SNAME, FC_HIC_LTDNAME(b.LTDCODE) AS LTDNAME, A.WRTNO    ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_JEPSU a                                                    ");
            parameter.AppendSql("       ,KOSMOS_PMPA.HIC_PATIENT b                                                  ");
            parameter.AppendSql("  WHERE a.JEPDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("    AND a.JEPDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("    AND a.DELDATE IS NULL                                                          ");
            parameter.AppendSql("    AND a.PANO = b.PANO(+)                                                         ");
            parameter.AppendSql("    AND b.SNAME <> '이중챠트'                                                      ");
            parameter.AppendSql("                                                                                   ");
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND b.SNAME LIKE :SNAME                   ");
            }

            if (!strPtno.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND b.PTNO = :PTNO                        ");
            }

            if (nLtdCode > 0)
            {
                parameter.AppendSql("   AND b.LTDCODE = :LTDCODE                        ");
            }

            parameter.AppendSql(" ORDER BY b.SNAME                    ");

            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);

            if (!strSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", strSName);
            }

            if (!strPtno.IsNullOrEmpty())
            {
                parameter.Add("PTNO", strPtno, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            if (nLtdCode > 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_PATIENT>(parameter);
        }

        public List<HIC_PATIENT> GetHeaListByItem(string strSName, string strPtno, long nLtdCode, string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT b.JUMIN, b.PTNO, b.SNAME, FC_HIC_LTDNAME(b.LTDCODE) AS LTDNAME, A.WRTNO    ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HEA_JEPSU a                                                    ");
            parameter.AppendSql("       ,KOSMOS_PMPA.HIC_PATIENT b                                                  ");
            parameter.AppendSql("  WHERE a.SDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("    AND a.SDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("    AND a.DELDATE IS NULL                                                          ");
            parameter.AppendSql("    AND a.PANO = b.PANO(+)                                                         ");
            parameter.AppendSql("    AND b.SNAME <> '이중챠트'                                                      ");
            parameter.AppendSql("                                                                                   ");
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND b.SNAME LIKE :SNAME                   ");
            }

            if (!strPtno.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND b.PTNO = :PTNO                        ");
            }

            if (nLtdCode > 0)
            {
                parameter.AppendSql("   AND b.LTDCODE = :LTDCODE                        ");
            }

            parameter.AppendSql(" ORDER BY b.SNAME                    ");

            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);

            if (!strSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", strSName);
            }

            if (!strPtno.IsNullOrEmpty())
            {
                parameter.Add("PTNO", strPtno, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            if (nLtdCode > 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_PATIENT>(parameter);
        }

        public HIC_PATIENT GetJumin2byPano(long fnPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT JUMIN2, PTNO                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT     ");
            parameter.AppendSql(" WHERE PANO =:PANO                ");

            parameter.Add("PANO", fnPano);

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public int UpdatebyPaNo(HIC_PATIENT item3, string strGbSuchup)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_PATIENT SET                     ");
            parameter.AppendSql("       GONGJENG  = :GONGJENG                           ");
            parameter.AppendSql("     , SABUN     = :SABUN                              ");
            parameter.AppendSql("     , IPSADATE  = TO_DATE(:IPSADATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("     , BUSENAME  = :BUSENAME                           ");
            parameter.AppendSql("     , BUSEIPSA  = TO_DATE(:BUSEIPSA, 'YYYY-MM-DD')    ");
            if (strGbSuchup == "1")
            {
                parameter.AppendSql("     , GBSUCHEP = 'Y'                              ");
            }
            else
            {
                parameter.AppendSql("     , GBSUCHEP = 'N'                              ");
            }
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");

            parameter.Add("GONGJENG", item3.GONGJENG, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("SABUN", item3.SABUN);
            parameter.Add("IPSADATE", item3.IPSADATE);
            parameter.Add("BUSENAME", item3.BUSENAME);
            parameter.Add("BUSEIPSA", item3.BUSEIPSA);
            //parameter.Add("GBSUCHEP", item3.GBSUCHEP, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("PANO", item3.PANO);

            return ExecuteNonQuery(parameter);
        }

        public long GetPanobyPaNo(long pANO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO FROM KOSMOS_PMPA.HIC_PATIENT           ");
            parameter.AppendSql(" WHERE Jumin2 IN (SELECT Jumin2                    ");
            parameter.AppendSql("                    FROM KOSMOS_PMPA.HIC_PATIENT   ");
            parameter.AppendSql("                   WHERE PANO = :PANO)             ");
            parameter.AppendSql(" AND SNAME <> '이중챠트'                            ");
            parameter.AppendSql(" ORDER BY Pano ASC                                 ");

            parameter.Add("PANO", pANO);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyJumin2(string strJumin2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(PANO) CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT                 ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2                        ");

            parameter.Add("JUMIN2", strJumin2);

            return ExecuteScalar<int>(parameter);
        }

        public HIC_PATIENT GetItembyPaNo(long fnPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT JUMIN2, PTNO,SNAME, SEX                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT                 ");
            parameter.AppendSql(" WHERE PANO = :PANO                            ");

            parameter.Add("PANO", fnPano);

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public HIC_PATIENT GetHphoneTelbyPano(long nPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT HPHONE, TEL                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT                 ");
            parameter.AppendSql(" WHERE PANO =:PANO                            ");

            parameter.Add("PANO", nPano);

            return ExecuteReaderSingle<HIC_PATIENT>(parameter);
        }

        public int UpDate(HIC_PATIENT item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_PATIENT         ");
            parameter.AppendSql("   SET SNAME       = :SNAME            ");
            parameter.AppendSql("      ,JUMIN       = :JUMIN            ");
            parameter.AppendSql("      ,SEX         = :SEX              ");
            parameter.AppendSql("      ,MAILCODE    = :MAILCODE         ");
            parameter.AppendSql("      ,JUSO1       = :JUSO1            ");
            parameter.AppendSql("      ,JUSO2       = :JUSO2            ");
            parameter.AppendSql("      ,TEL         = :TEL              ");
            parameter.AppendSql("      ,LTDCODE     = :LTDCODE          ");
            parameter.AppendSql("      ,JIKJONG     = :JIKJONG          ");
            parameter.AppendSql("      ,SABUN       = :SABUN            ");
            parameter.AppendSql("      ,BUSENAME    = :BUSENAME         ");
            parameter.AppendSql("      ,GONGJENG    = :GONGJENG         ");
            parameter.AppendSql("      ,IPSADATE    = TO_DATE(:IPSADATE, 'YYYY-MM-DD')         ");
            parameter.AppendSql("      ,BUSEIPSA    = TO_DATE(:BUSEIPSA, 'YYYY-MM-DD')         ");
            parameter.AppendSql("      ,JISA        = :JISA             ");
            parameter.AppendSql("      ,GKIHO       = :GKIHO            ");
            parameter.AppendSql("      ,GBSUCHEP    = :GBSUCHEP         ");
            parameter.AppendSql("      ,PTNO        = :PTNO             ");
            parameter.AppendSql("      ,REMARK      = :REMARK           ");
            parameter.AppendSql("      ,KIHO        = :KIHO             ");
            parameter.AppendSql("      ,BOGUNSO     = :BOGUNSO          ");
            parameter.AppendSql("      ,EMAIL       = :EMAIL            ");
            parameter.AppendSql("      ,HPHONE      = :HPHONE           ");
            parameter.AppendSql("      ,GBSMS       = :GBSMS            ");
            parameter.AppendSql("      ,JUMIN2      = :JUMIN2           ");
            parameter.AppendSql("      ,GBPRIVACY   = TO_DATE(:GBPRIVACY, 'YYYY-MM-DD')        ");
            parameter.AppendSql("      ,BUILDNO     = :BUILDNO          ");
            parameter.AppendSql("      ,BIRTHDAY    = :BIRTHDAY         ");
            parameter.AppendSql("      ,FAMILLY     = :FAMILLY          ");
            parameter.AppendSql("      ,VIPREMARK   = :VIPREMARK        ");
            parameter.AppendSql("      ,GBFOREIGNER =:GBFOREIGNER       ");
            parameter.AppendSql("      ,ENAME       =:ENAME             ");
            parameter.AppendSql("      ,SNAME2      =:SNAME2            ");
            parameter.AppendSql("      ,WEBSEND     =''             ");
            
            if (!item.SOSOK.IsNullOrEmpty())
            {
                parameter.AppendSql("      ,SOSOK         =:SOSOK           ");
            }
            parameter.AppendSql("      ,FOREIGNERNUM  =:FOREIGNERNUM    ");
            //parameter.AppendSql("      ,GBPRIVACY_NEW =:GBPRIVACY_NEW   ");
            //parameter.AppendSql(" WHERE PTNO          =:PTNO           ");
            parameter.AppendSql(" WHERE PANO          =:PANO           ");

            #region Query 변수대입
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("JUMIN", item.JUMIN);
            parameter.Add("SEX", item.SEX);
            parameter.Add("MAILCODE", item.MAILCODE);
            parameter.Add("JUSO1", item.JUSO1);
            parameter.Add("JUSO2", item.JUSO2);
            parameter.Add("TEL", item.TEL);
            parameter.Add("LTDCODE", item.LTDCODE);
            parameter.Add("JIKJONG", item.JIKJONG);
            parameter.Add("SABUN", item.SABUN);
            parameter.Add("BUSENAME", item.BUSENAME);
            parameter.Add("GONGJENG", item.GONGJENG);
            parameter.Add("IPSADATE", item.IPSADATE);
            parameter.Add("BUSEIPSA", item.BUSEIPSA);
            parameter.Add("JISA", item.JISA);
            parameter.Add("GKIHO", item.GKIHO);
            parameter.Add("GBSUCHEP", item.GBSUCHEP);
            parameter.Add("PTNO", item.PTNO);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("KIHO", item.KIHO);
            parameter.Add("BOGUNSO", item.BOGUNSO);
            parameter.Add("EMAIL", item.EMAIL);
            parameter.Add("HPHONE", item.HPHONE);
            parameter.Add("GBSMS", item.GBSMS);
            parameter.Add("JUMIN2", item.JUMIN2);
            parameter.Add("GBPRIVACY", item.GBPRIVACY);
            parameter.Add("BUILDNO", item.BUILDNO);
            parameter.Add("BIRTHDAY", item.BIRTHDAY);
            parameter.Add("FAMILLY", item.FAMILLY);
            parameter.Add("VIPREMARK", item.VIPREMARK);
            parameter.Add("PANO", item.PANO);
            parameter.Add("GBFOREIGNER", item.GBFOREIGNER);
            parameter.Add("ENAME", item.ENAME);
            parameter.Add("SNAME2", item.SNAME2);
            if (!item.SOSOK.IsNullOrEmpty())
            {
                parameter.Add("SOSOK", item.SOSOK);
            } 
            parameter.Add("FOREIGNERNUM", item.FOREIGNERNUM);
            //parameter.Add("GBPRIVACY_NEW", item.GBPRIVACY_NEW);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public List<HIC_PATIENT> GetPanobyItem(string sItem, string sGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, SNAME, JUMIN2,JUMIN, PTNO         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT                 ");
            if (sGubun == "1")
            {
                parameter.AppendSql(" WHERE PANO LIKE :ITEM                     ");
            }
            if (sGubun == "2")
            {
                parameter.AppendSql(" WHERE SNAME LIKE :ITEM                    ");
                parameter.AppendSql(" ORDER BY SNAME                            ");
            }
            if (sGubun == "3")
            {
                parameter.AppendSql(" WHERE JUMIN LIKE :ITEM                    ");
                parameter.AppendSql(" ORDER BY JUMIN                            ");
            }
            if (sGubun == "4")
            {
                parameter.AppendSql("  WHERE LTDCODE LIKE :ITEM                 ");
            }
            if (sGubun == "5")
            {
                parameter.AppendSql("  WHERE PTNO LIKE :ITEM                 ");
            }


            parameter.AddLikeStatement("ITEM", sItem);

            return ExecuteReader<HIC_PATIENT>(parameter);
        }

        public int UpdatePrivacyByPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_PATIENT SET         ");
            parameter.AppendSql(" GbPrivacy_NEW = SYSDATE                   ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                       ");

            parameter.Add("PTNO", argPtno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateJumin2ByPtno(string argPtno, string argJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_PATIENT SET         ");
            parameter.AppendSql(" JUMIN2 = :JUMIN2                          ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                       ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("JUMIN2", argJumin2);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateWebSendByPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_PATIENT SET         ");
            parameter.AppendSql(" WEBSEND = ''                               ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                       ");

            parameter.Add("PTNO", argPtno);

            return ExecuteNonQuery(parameter);
        }

        public long Read_HeaWrtNo()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT KOSMOS_PMPA.SEQ_HEAJEPNO.NEXTVAL HEAWRTNO FROM DUAL ");

            return ExecuteScalar<long>(parameter);
        }


    }
}
