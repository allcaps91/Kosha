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
    public class HicIeMunjinNewRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicIeMunjinNewRepository()
        {
        }

        public HIC_IE_MUNJIN_NEW GetItembyPtNoJepDateGjJong(string fstrPtno, string strFrDate, string strToDate, string fstrGjJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MunDate,RecvForm,ROWID FROM HIC_IE_MUNJIN_NEW   ");
            parameter.AppendSql(" WHERE PTNO    = :PTNO                                 ");
            parameter.AppendSql("   AND MUNDATE >= :FRDATE                              ");
            parameter.AppendSql("   AND MUNDATE <= :TODATE                              ");
            switch (fstrGjJong)
            {
                case "11":
                case "12":
                case "13":
                case "14":
                case "41":
                case "42":
                case "43":
                    parameter.AppendSql("   AND GBMUN1 = 'Y'                            "); //1차
                    break;
                case "31":
                case "35":
                    parameter.AppendSql("   AND GBMUN2 = 'Y'                            "); //암검진
                    break;
                case "56":
                    parameter.AppendSql("   AND GBMUN4 = 'Y'                            "); //학생검진
                    break;
                case "16":
                case "17":
                case "18":
                case "19":
                case "44":
                case "45":
                case "46":
                    parameter.AppendSql("   AND GBMUN5 = 'Y'                            "); //2차
                    break;
                case "21":
                case "22":
                case "23":
                case "24":
                case "25":
                case "26":
                    parameter.AppendSql("   AND GBMUN3 = 'Y'                            "); //특수
                    break;
                default:
                    break;
            }
            parameter.AppendSql(" ORDER BY MunDate DESC                                 ");

            parameter.Add("PTNO", fstrPtno);
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReaderSingle<HIC_IE_MUNJIN_NEW>(parameter);
        }

        public int UpdatePtNobyRowId(string strPtNo, string rOWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_IE_MUNJIN_NEW SET   ");
            parameter.AppendSql("       PTNO   = :PTNO                      ");
            parameter.AppendSql(" WHERE ROWID  = :RID                       ");

            parameter.Add("PANO", strPtNo);
            parameter.Add("RID", rOWID);

            return ExecuteNonQuery(parameter);
        }

        public void UpDateReset(long argIEMunno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_IE_MUNJIN_NEW   ");
            parameter.AppendSql("   SET GBDBSEND = ''                   ");
            parameter.AppendSql("     , WRTNO1 = ''                     ");
            parameter.AppendSql("     , WRTNO2 = ''                     ");
            parameter.AppendSql("     , WRTNO3 = ''                     ");
            parameter.AppendSql("     , WRTNO4 = ''                     ");
            parameter.AppendSql("     , WRTNO5 = ''                     ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                 ");

            parameter.Add("WRTNO", argIEMunno);

            ExecuteNonQuery(parameter);
        }

        public HIC_IE_MUNJIN_NEW GetItembyWrtNoMunDateLike(long iEMUNNO, string argMunDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MUNDATE,WRTNO,RECVFORM,ROWID    ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW   ");
            parameter.AppendSql(" WHERE WRTNO =:WRTNO                   ");
            parameter.AppendSql("   AND MUNDATE LIKE :MUNDATE           ");

            parameter.Add("WRTNO", iEMUNNO);
            parameter.AddLikeStatement("MUNDATE", argMunDate);

            return ExecuteReaderSingle<HIC_IE_MUNJIN_NEW>(parameter);
        }

        public int GetCountbyPtNo(string pTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                    ");
            parameter.AppendSql("   AND MUNDATE >= TRUNC(SYSDATE) -180  ");
            parameter.AppendSql("   AND WebData1 IS NOT NULL            ");

            parameter.Add("PTNO", pTNO);

            return ExecuteScalar<int>(parameter);
        }

        public HIC_IE_MUNJIN_NEW GetMunjinResbyWrtNo2(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME, PTNO, MUNJINRES              ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW       ");
            parameter.AppendSql(" WHERE WRTNO2 =:WRTNO2                     ");
            parameter.AppendSql(" ORDER BY MUNDATE DESC                     ");

            parameter.Add("WRTNO2", nWRTNO);

            return ExecuteReaderSingle<HIC_IE_MUNJIN_NEW>(parameter);
        }

        public string GetResvFormByPtno(string argPtno, string argDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RECVFORM                        ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW   ");
            parameter.AppendSql(" WHERE PTNO =:PTNO                     ");
            parameter.AppendSql("   AND MUNDATE >= :MUNDATE             ");
            parameter.AppendSql("  ORDER By MUNDATE DESC                ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("MUNDATE", argDate);

            return ExecuteScalar<string>(parameter);
        }

        public int GetCountbyPtNoGjJong(string pTNO, string strGjJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                    ");
            parameter.AppendSql("   AND MunDate >= TRUNC(SYSDATE) -180  ");
            parameter.AppendSql("   AND WebData1 IS NOT NULL            ");
            if (strGjJong == "31" || strGjJong == "35")
            {
                parameter.AppendSql("   AND RecvForm LIKE '%12003%'     ");
            }
            else
            {
                parameter.AppendSql("   AND RecvForm LIKE '%12001%'     ");
            }

            parameter.Add("PTNO", pTNO);

            return ExecuteScalar<int>(parameter);
        }

        public string GetResvFormByWrtno(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RECVFORM                        ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW   ");
            parameter.AppendSql(" WHERE WRTNO =:WRTNO                   ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_IE_MUNJIN_NEW GetItembyPtNoMunDate(string pTNO, string strDate, string argJob = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,RECVFORM,ROWID, GBMUN1,GBMUN2, MUNDATE     ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW           ");
            parameter.AppendSql(" WHERE PTNO =:PTNO                             ");
            parameter.AppendSql("   AND MUNDATE >= :MUNDATE                     ");
            parameter.AppendSql("   AND WEBDATA1 IS NOT NULL                    ");
            if (!argJob.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND (WRTNO1 IS NULL AND WRTNO2 IS NULL AND WRTNO3 IS NULL AND WRTNO4 IS NULL AND WRTNO5 IS NULL) ");
            }
            parameter.AppendSql("   ORDER BY MUNDATE DESC                       ");

            parameter.Add("PTNO", pTNO);
            parameter.Add("MUNDATE", strDate);

            return ExecuteReaderSingle<HIC_IE_MUNJIN_NEW>(parameter);
        }

        public HIC_IE_MUNJIN_NEW GetItembySNamePtnoMunDate(string sNAME, string pTNO, string jEPDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MUNDATE,WRTNO,RECVFORM,ROWID    ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW   ");
            parameter.AppendSql(" WHERE SNAME =:SNAME                   ");
            parameter.AppendSql("   AND PTNO =:PTNO                     ");
            parameter.AppendSql("   AND MUNDATE <= :MUNDATE             ");
            parameter.AppendSql(" ORDER BY MUNDATE DESC                 ");

            parameter.Add("SNAME", sNAME);
            parameter.Add("PTNO", pTNO);
            parameter.Add("MUNDATE", jEPDATE);

            return ExecuteReaderSingle<HIC_IE_MUNJIN_NEW>(parameter);
        }

        public HIC_IE_MUNJIN_NEW GetRecvFormbyMunDatePtNo(string strMunDate, string pTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RECVFORM, MUNDATE                   ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW       ");
            parameter.AppendSql(" WHERE MUNDATE >= :MUNDATE                 ");
            parameter.AppendSql("   AND PTNO  = :PTNO                       ");
            parameter.AppendSql(" ORDER BY MUNDATE DESC                     ");

            parameter.Add("PTNO", pTNO);
            parameter.Add("MUNDATE", strMunDate);

            return ExecuteReaderSingle<HIC_IE_MUNJIN_NEW>(parameter);
        }

        public HIC_IE_MUNJIN_NEW GetItembyPtnoMundate2(string argPtno, string strDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,MUNDATE,SNAME,BIRTH,PTNO,RECVFORM,MUNJINRES,GBMUN4,ROWID  ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW                                   ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                                                   ");
            parameter.AppendSql("   AND MUNDATE  >= :MUNDATE                                            ");
            parameter.AppendSql("   AND WEBDATA1 IS NOT NULL                                            ");
            parameter.AppendSql(" ORDER BY MUNDATE DESC                                                 ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("MUNDATE", strDate);

            return ExecuteReaderSingle<HIC_IE_MUNJIN_NEW>(parameter);
        }

        public string GetMunDatebyPtNo(string fstrPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MUNDATE                                     ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW               ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                               ");
            parameter.AppendSql(" ORDER BY MUNDATE DESC                             ");

            parameter.Add("PTNO", fstrPtno);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_IE_MUNJIN_NEW GetWrtNoRecvFormbyRowId(string strROWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, RECVFORM                             ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW               ");
            parameter.AppendSql(" WHERE ROWID  = :RID                               ");

            parameter.Add("RID", strROWID);

            return ExecuteReaderSingle<HIC_IE_MUNJIN_NEW>(parameter);
        }

        public List<HIC_IE_MUNJIN_NEW> GetItembyWrtNoMunDate(long fnWrtNo, string strFrDate, string strToDate, string fstrPtno, string strSname, string strLtdName, string fstrGjJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MUNDATE, SNAME, BIRTH, AGE, SEX, GBMUN1, GBMUN2, GBMUN3, GBMUN4, GBMUN5             ");
            parameter.AppendSql("     , MAILCODE, JUSO1, JUSO2, LTDNAME, TEL, HPHONE, CLASS, BAN, BUN, PTNO, RECVFORM,ROWID ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW                                                       ");
            if (fnWrtNo > 0)
            {
                parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                                 ");
            }
            else
            {
                parameter.AppendSql(" WHERE MunDate >= :FRDATE                                                              ");
                parameter.AppendSql("   AND MunDate <= :TODATE                                                              ");
            }

            if (fstrGjJong != "56" && !fstrGjJong.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND PTNO = :PTNO                                                                    ");
            }

            if (!strSname.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SNAME LIKE :SNAME                                                               ");
            }

            if (!strLtdName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND LTDNAME LIKE :LTDNAME                                                           ");
            }
            parameter.AppendSql(" ORDER BY MunDate DESC                                                                     ");

            if (fnWrtNo > 0)
            {
                parameter.Add("WRTNO", fnWrtNo);
            }
            else
            {
                parameter.Add("FRDATE", strFrDate);
                parameter.Add("TODATE", strToDate);
            }            
            if (fstrGjJong != "56" && !fstrGjJong.IsNullOrEmpty())
            {
                parameter.Add("PTNO", fstrPtno);
            }
            if (!strSname.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", strSname);
            }
            if (!strLtdName.IsNullOrEmpty())
            {
                parameter.Add("LTDNAME", strLtdName);
            }

            return ExecuteReader<HIC_IE_MUNJIN_NEW>(parameter);
        }

        public HIC_IE_MUNJIN_NEW GetItembyRowId(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MUNDATE, INJEKDATA, RECVFORM                ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW               ");
            parameter.AppendSql(" WHERE ROWID  = :ROWID                             ");

            parameter.Add("ROWID", strROWID);

            return ExecuteScalar<HIC_IE_MUNJIN_NEW>(parameter);
        }

        public HIC_IE_MUNJIN_NEW GetCountbyPtNoMunDate(string fstrPtno, string gstrSysDate, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MUNJINRES                                   ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW               ");
            parameter.AppendSql(" WHERE PTNO  = :PTNO                               ");
            parameter.AppendSql("   AND MUNDATE >= :SYSDT                           ");
            parameter.AppendSql("   AND MUNDATE <= :JEPDATE                         ");
            parameter.AppendSql("   AND MunjinRes > ' '                             ");
            parameter.AppendSql("   AND WebData1 IS NOT NULL                        ");
            parameter.AppendSql("   AND RecvForm LIKE '%30001%'                     ");
            parameter.AppendSql(" ORDER BY MunDate DESC                             ");

            parameter.Add("PTNO", fstrPtno);
            parameter.Add("SYSDT", gstrSysDate);
            parameter.Add("JEPDATE", fstrJepDate);

            return ExecuteReaderSingle<HIC_IE_MUNJIN_NEW>(parameter);
        }

        public string GetRecvFormbyWrtNo(long iEMUNNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RECVFORM                            ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW       ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                     ");

            parameter.Add("WRTNO", iEMUNNO);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_IE_MUNJIN_NEW GetMunjinResbyWrtNo1(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MUNJINRES                           ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW       ");
            parameter.AppendSql(" WHERE WRTNO1  = :WRTNO                     ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReaderSingle<HIC_IE_MUNJIN_NEW>(parameter);
        }

        public int GetwebHtmlbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WEBHTML                             ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN           ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                     ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyJumin(string strJumin, string strJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN                   ");
            parameter.AppendSql(" WHERE JUMIN2  = :JUMIN                            ");
            parameter.AppendSql("   AND ENTDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");

            parameter.Add("JUMIN", strJumin, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", strJepDate); 

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT FROM ADMIN.HIC_IE_MUNJIN WHERE WRTNO = :WRTNO  ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public HIC_IE_MUNJIN_NEW GetItembyPtnoMundate(string strPtno, string strMundate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                                       ");
            parameter.AppendSql("  FROM ADMIN.HIC_IE_MUNJIN_NEW               ");
            parameter.AppendSql(" WHERE 1 =1                                        ");
            parameter.AppendSql("   AND PTNO = :PTNO                                ");
            parameter.AppendSql("   AND MUNDATE >= :MUNDATE                         ");
            parameter.AppendSql("   AND WEBDATA1 IS NOT NULL                        ");

            parameter.Add("PTNO", strPtno);
            parameter.Add("MUNDATE", strMundate);

            return ExecuteReaderSingle<HIC_IE_MUNJIN_NEW>(parameter);
        }
    }
}
