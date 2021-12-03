namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicWaitRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicWaitRepository()
        {
        }

        public List<HIC_WAIT> GetReExambyDate()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Jumin2,GjJong,ROWID FROM HIC_WAIT       ");
            parameter.AppendSql(" WHERE JobDate = trunc(SYSDATE)                ");
            parameter.AppendSql("   AND GbYeyak = '2'                           "); //2차재검
            parameter.AppendSql("   AND (SecondPrint IS NULL OR SecondPrint='') ");

            return ExecuteReader<HIC_WAIT>(parameter);
        }

        public List<HIC_WAIT> GetItembyJobDate(string gstrSysDate, bool bChk, string argBuse)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SEQNO, SNAME, JUMIN2, JEPTIME               ");
            parameter.AppendSql("  FROM ADMIN.HIC_WAIT                        ");
            parameter.AppendSql(" WHERE JOBDATE = :JOBDATE                          ");
            parameter.AppendSql("   AND GBBUSE  = :GBBUSE                           "); 
            if (!bChk)
            {
                parameter.AppendSql("   AND (CALLTIME IS NULL OR CALLTIME = '')     ");
            }
            parameter.AppendSql(" ORDER By SEQNO                                    ");

            parameter.Add("JOBDATE", gstrSysDate, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBBUSE", argBuse);

            return ExecuteReader<HIC_WAIT>(parameter);
        }

        public HIC_WAIT GetItemByJobDateGbDisplay(string argDate, string argDisplay)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBDISPLAY2, GBJOB, SEQNO, PCNO, SNAME   ");
            parameter.AppendSql("  FROM ADMIN.HIC_WAIT_PC                 ");
            parameter.AppendSql(" WHERE JOBDATE = :JOBDATE                      ");
            parameter.AppendSql("   AND GBDISPLAY2 = :GBDISPLAY2                ");
            parameter.AppendSql("   AND PcNo <= 4                               "); //종합검진

            parameter.Add("JOBDATE", argDate, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBDISPLAY2", argDisplay, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_WAIT>(parameter);
        }

        public void UpDateDisplay(string argDisplay, string argJobDate, string argPcNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_WAIT_PC     ");
            parameter.AppendSql("   SET GBDISPLAY2 =:GBDISPLAY2     ");
            parameter.AppendSql(" WHERE JOBDATE =:JOBDATE           ");
            parameter.AppendSql("   AND PCNO =:PCNO                 ");

            parameter.Add("GBDISPLAY2", argDisplay);
            parameter.Add("JOBDATE", argJobDate);
            parameter.Add("PCNO", argPcNo);

            ExecuteNonQuery(parameter);
        }

        public void DeleteByJobDate(string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HIC_WAIT_PC    ");
            parameter.AppendSql(" WHERE JOBDATE <=:JOBDATE       ");

            parameter.Add("JOBDATE", strDate);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_WAIT> GetItembyJobDate1(string gstrSysDate, string strMundate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.JOBDATE, A.SEQNO, A.JUMIN, A.SNAME, A.GBJOB                       ");
            parameter.AppendSql("     , A.PCNO, A.JEPTIME, A.CALLTIME, A.ENDTIME, A.JUMIN2                  ");
            parameter.AppendSql("     , A.GBYEYAK, A.GBAUTOJEP, A.GJJONG, A.GBBUSE                          ");
            parameter.AppendSql("     , A.SECONDPRINT, A.SOYOTIME                                           ");
            parameter.AppendSql("     , (SELECT CASE WHEN COUNT('X') > 0 THEN '●' ELSE '' END IEMUNYN       ");
            parameter.AppendSql("          FROM ADMIN.HIC_IE_MUNJIN_NEW                               ");
            parameter.AppendSql("         WHERE PTNO = b.PTNO AND MUNDATE >= :MUNDATE) IEMUNYN              ");
            parameter.AppendSql("  FROM ADMIN.HIC_WAIT    a                                           ");
            parameter.AppendSql("     , ADMIN.HIC_PATIENT b                                           ");
            parameter.AppendSql(" WHERE A.JOBDATE = :JOBDATE                                                ");
            parameter.AppendSql("   AND A.JUMIN2  = b.JUMIN2(+)                                             ");
            parameter.AppendSql("   AND A.GBJOB   = '1'                                                     ");
            parameter.AppendSql("   AND A.GBBUSE  = '2'                                                     "); //일반검진
            parameter.AppendSql(" ORDER BY A.SEQNO                                                          ");

            parameter.Add("JOBDATE", gstrSysDate);
            parameter.Add("MUNDATE", strMundate);

            return ExecuteReader<HIC_WAIT>(parameter);
        }

        public HIC_WAIT GetCountItemByJobDateGbJob(string argDate, string argJob)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(ROWID) AS CNT                             ");
            parameter.AppendSql("       ,SUM(DECODE(GbBuse,'1',1,0)) AS HEACNT          ");
            parameter.AppendSql("       ,SUM(DECODE(GBYEYAK,'1',1,'2',1,0)) AS YEYAKCNT ");
            parameter.AppendSql("  FROM ADMIN.HIC_WAIT                            ");
            parameter.AppendSql(" WHERE JOBDATE = :JOBDATE                              ");
            parameter.AppendSql("   AND CALLTIME IS NULL                                ");
            parameter.AppendSql("   AND GBJOB = :GBJOB                                  ");
            
            parameter.Add("JOBDATE", argDate, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBJOB", argJob, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_WAIT>(parameter);
        }

        public long GetMaxSeqnoByJobDate(string argDate, string argJob)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MAX(SEQNO) SEQNO                ");
            parameter.AppendSql("  FROM ADMIN.HIC_WAIT            ");
            parameter.AppendSql(" WHERE JOBDATE = :JOBDATE              ");
            if (argJob == "예약")
            {
                parameter.AppendSql("   AND GBYEYAK IN ('1','2')        ");
            }
            else if (argJob == "일반")
            {
                parameter.AppendSql("   AND SEQNO >= 101                ");
            }
            
            parameter.Add("JOBDATE", argDate);

            return ExecuteScalar<long>(parameter);
        }

        public void UpDateCompleteOK(string argTime, string jUMINNO2, string argDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_WAIT    ");
            parameter.AppendSql("   SET ENDTIME =:ENDTIME       ");
            parameter.AppendSql("     , GBJOB = '3'             ");    //접수완료
            parameter.AppendSql(" WHERE JOBDATE =:JOBDATE       ");
            parameter.AppendSql("   AND JUMIN2 =:JUMIN2         ");

            parameter.Add("ENDTIME", argTime);
            parameter.Add("JOBDATE", argDate);
            parameter.Add("JUMIN2", jUMINNO2);

            ExecuteNonQuery(parameter);
        }

        public long GetSeqNoByJobDateJumin(string strCurDate, string argJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SEQNO                   ");
            parameter.AppendSql("  FROM ADMIN.HIC_WAIT    ");
            parameter.AppendSql(" WHERE JOBDATE = :JOBDATE      ");
            parameter.AppendSql("   AND JUMIN2 =:JUMIN2         ");

            parameter.Add("JOBDATE", strCurDate);
            parameter.Add("JUMIN2", argJumin2);

            return ExecuteScalar<long>(parameter);
        }

        public long GetMaxSeqnoByJobDateGbBuse(string strCurDate, string argBuse, string argEndo, long argCount)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MAX(SEQNO) SEQNO        ");
            parameter.AppendSql("  FROM ADMIN.HIC_WAIT    ");
            parameter.AppendSql(" WHERE JOBDATE = :JOBDATE      ");
            parameter.AppendSql("   AND GBBUSE =:GBBUSE         ");
            if (argEndo =="OK")
            {
                parameter.AppendSql("   AND SEQNO < :SEQNO          ");
            }

            else
            {
                parameter.AppendSql("   AND SEQNO >= :SEQNO          ");
            }

            parameter.Add("JOBDATE", strCurDate);
            parameter.Add("GBBUSE", argBuse);
            parameter.Add("SEQNO", argCount);


            return ExecuteScalar<long>(parameter);
        }

        public void InsertData(HIC_WAIT nHW)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_WAIT (                                         ");
            parameter.AppendSql("       JOBDATE,SEQNO,JUMIN,JUMIN2,SNAME,GBJOB,GBYEYAK,GBAUTOJEP            ");
            parameter.AppendSql("       ,GJJONG,JEPTIME,CALLTIME,ENDTIME,GBBUSE                             ");
            parameter.AppendSql(" ) VALUES (                                                                ");
            parameter.AppendSql("       :JOBDATE,:SEQNO,:JUMIN,:JUMIN2,:SNAME,:GBJOB,:GBYEYAK,:GBAUTOJEP    ");
            parameter.AppendSql("      ,:GJJONG,:JEPTIME,:CALLTIME,:ENDTIME,:GBBUSE )                       ");

            parameter.Add("JOBDATE",    nHW.JOBDATE);
            parameter.Add("SEQNO",      nHW.SEQNO);
            parameter.Add("JUMIN",      nHW.JUMIN); 
            parameter.Add("JUMIN2",     nHW.JUMIN2);
            parameter.Add("SNAME",      nHW.SNAME);
            parameter.Add("GBJOB",      nHW.GBJOB);
            parameter.Add("GBYEYAK",    nHW.GBYEYAK);
            parameter.Add("GBAUTOJEP",  nHW.GBAUTOJEP);
            parameter.Add("GJJONG",     nHW.GJJONG);
            parameter.Add("JEPTIME",    nHW.JEPTIME);
            parameter.Add("CALLTIME",   nHW.CALLTIME);
            parameter.Add("ENDTIME",    nHW.ENDTIME);
            parameter.Add("GBBUSE",     nHW.GBBUSE);

            ExecuteNonQuery(parameter);
        }

        public long GetMaxSeqNoByGbYeyakAge(string strDate, string strGbYeyak, int nAge, string strGBGFS, string strGBCT, string strJONG)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MAX(SEQNO) SEQNO        ");
            parameter.AppendSql("  FROM ADMIN.HIC_WAIT    ");
            parameter.AppendSql(" WHERE JOBDATE = :JOBDATE      ");

            //if (strGbYeyak == "1" && (strGBGFS == "Y" || strGBCT =="Y"))
            //{
            //    parameter.AppendSql("  AND GBYEYAK IN ('1')     ");
            //    parameter.AppendSql("  AND SEQNO >= 101         ");
            //    parameter.AppendSql("  AND SEQNO <= 150         ");
            //}
            //else if (strGbYeyak == "1" || strGbYeyak == "2" || strGbYeyak == "3")
            //{
            //    parameter.AppendSql("  AND GBYEYAK IN ('1', '2', '3')   ");
            //    parameter.AppendSql("  AND SEQNO >= 151                 ");
            //    parameter.AppendSql("  AND SEQNO <= 300                 ");
            //}
            //else if (nAge >= 5 && nAge <= 17 )
            //{
            //    parameter.AppendSql("  AND SEQNO >= 501                 ");
            //}
            //else
            //{
            //    parameter.AppendSql("  AND SEQNO >= 301                 ");
            //    parameter.AppendSql("  AND SEQNO <= 499                 ");
            //}


            //NEW
            if (strGbYeyak == "1" && (strGBGFS == "Y" || strGBCT == "Y"))
            {
                parameter.AppendSql("  AND GBYEYAK IN ('1')     ");
                parameter.AppendSql("  AND SEQNO >= 101         ");
                parameter.AppendSql("  AND SEQNO <= 200         ");
            }
            else if (strJONG == "16" || strJONG == "28")
            {
                parameter.AppendSql("  AND SEQNO >= 201         ");
                parameter.AppendSql("  AND SEQNO <= 300         ");
            }
            else if (strGbYeyak == "1" || strGbYeyak == "2" || strGbYeyak == "3")
            {
                parameter.AppendSql("  AND GBYEYAK IN ('1', '2', '3')   ");
                parameter.AppendSql("  AND SEQNO >= 301                 ");
                parameter.AppendSql("  AND SEQNO <= 999                 ");
            }
            else if (nAge >= 5 && nAge <= 17)
            {
                parameter.AppendSql("  AND SEQNO >= 2001                 ");
            }
            else
            {
                parameter.AppendSql("  AND SEQNO >= 1001                 ");
                parameter.AppendSql("  AND SEQNO <= 1999                 ");
            }


            parameter.Add("JOBDATE", strDate);

            return ExecuteScalar<long>(parameter);
        }

        public string GetRowidByJobDateJumin2(string strDate, string strJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID AS RID                ");
            parameter.AppendSql("  FROM ADMIN.HIC_WAIT        ");
            parameter.AppendSql(" WHERE JOBDATE = :JOBDATE          ");
            parameter.AppendSql("   AND JUMIN2 = :JUMIN2            "); 

            parameter.Add("JOBDATE", strDate);
            parameter.Add("JUMIN2", strJumin2);

            return ExecuteScalar<string>(parameter);
        }

        public void InsertHicWaitPcRow(string strWaitPcNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_WAIT_PC (              ");
            parameter.AppendSql("       JOBDATE,PCNO,GBDISPLAY1,GBDISPLAY2,GBJOB    ");
            parameter.AppendSql(" ) VALUES (                                        ");
            parameter.AppendSql("       :JOBDATE,:PCNO, 'N', 'N', '0'  )            ");

            parameter.Add("JOBDATE", DateTime.Now.ToShortDateString());
            parameter.Add("PCNO", strWaitPcNo);

            ExecuteNonQuery(parameter);
        }

        public string GetRowidByJobDatePcNo(string strWaitPcNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID AS RID                  ");
            parameter.AppendSql("  FROM ADMIN.HIC_WAIT_PC       ");
            parameter.AppendSql(" WHERE JOBDATE = :JOBDATE            ");
            parameter.AppendSql("   AND PCNO = :PCNO                  "); //종합건진
            
            parameter.Add("JOBDATE", DateTime.Now.ToShortDateString());
            parameter.Add("PCNO", strWaitPcNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public void UpDateCall(string strPcNo, string argJob, int nSeqNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_WAIT        ");
            parameter.AppendSql("   SET GBJOB    =:GBJOB            ");
            parameter.AppendSql("     , PCNO     =:PCNO             ");
            parameter.AppendSql("     , CALLTIME =:CALLTIME         ");
            parameter.AppendSql(" WHERE JOBDATE  =:JOBDATE          ");
            parameter.AppendSql("   AND SEQNO    =:SEQNO            ");

            parameter.Add("GBJOB", argJob);
            parameter.Add("PCNO", strPcNo);
            parameter.Add("CALLTIME", clsPublic.GstrSysTime);
            parameter.Add("JOBDATE", DateTime.Now.ToShortDateString());
            parameter.Add("SEQNO", nSeqNo);

            ExecuteNonQuery(parameter);
        }

        public void UpDateCallWaitPC(string strPcNo, string argJob, string argBuse, string strName, int nSeqNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_WAIT_PC     ");
            parameter.AppendSql("   SET GBDISPLAY1 = 'N'            ");
            parameter.AppendSql("     , GBDISPLAY2 = 'N'            ");
            parameter.AppendSql("     , GBJOB    =:GBJOB            ");
            parameter.AppendSql("     , GBBUSE   =:GBBUSE           ");
            parameter.AppendSql("     , SEQNO    =:SEQNO            ");
            parameter.AppendSql("     , SNAME    =:SNAME            ");
            parameter.AppendSql("     , CALLTIME =:CALLTIME          ");
            parameter.AppendSql(" WHERE JOBDATE  =:JOBDATE          ");
            parameter.AppendSql("   AND PCNO     =:PCNO             ");

            parameter.Add("GBJOB", argJob);
            parameter.Add("GBBUSE", argBuse);
            parameter.Add("SEQNO", nSeqNo);
            parameter.Add("SNAME", strName);
            parameter.Add("JOBDATE", DateTime.Now.ToShortDateString());
            parameter.Add("CALLTIME", clsPublic.GstrSysTime);
            parameter.Add("PCNO", strPcNo);

            ExecuteNonQuery(parameter);
        }

        public void UpDateReCallWaitPC(string strPcNo, string argJob, string argBuse)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_WAIT_PC     ");
            parameter.AppendSql("   SET GBDISPLAY1 = 'N'            ");
            parameter.AppendSql("     , GBDISPLAY2 = 'N'            ");
            parameter.AppendSql("     , GBJOB    =:GBJOB            ");
            parameter.AppendSql("     , GBBUSE   =:GBBUSE           ");
            parameter.AppendSql("     , CALLTIME =:CALLTIME          ");
            parameter.AppendSql(" WHERE JOBDATE  =:JOBDATE          ");
            parameter.AppendSql("   AND PCNO     =:PCNO             ");

            parameter.Add("GBJOB", argJob);
            parameter.Add("GBBUSE", argBuse);
            parameter.Add("JOBDATE", DateTime.Now.ToShortDateString());
            parameter.Add("CALLTIME", clsPublic.GstrSysTime);
            parameter.Add("PCNO", strPcNo);

            ExecuteNonQuery(parameter);
        }

        public int GetCountbyJobDate(string argGbBuse)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(PCNO) CNT                  ");
            parameter.AppendSql("  FROM ADMIN.HIC_WAIT            ");
            parameter.AppendSql(" WHERE JOBDATE = :JOBDATE              ");
            parameter.AppendSql("   AND GBBUSE  = :GBBUSE               "); 
            parameter.AppendSql("   AND CallTime IS NULL                ");
            parameter.AppendSql(" ORDER BY SeqNo                        "); //일반검진

            parameter.Add("GBBUSE", argGbBuse);
            parameter.Add("JOBDATE", DateTime.Now.ToShortDateString());

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_WAIT> GetItembyJobDate(string argBuse)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JOBDATE, SEQNO, JUMIN, SNAME, GBJOB ");
            parameter.AppendSql("     , PCNO, JEPTIME, CALLTIME, ENDTIME    ");
            parameter.AppendSql("     , JUMIN2, GBYEYAK, GBAUTOJEP, GJJONG  ");
            parameter.AppendSql("     , GBBUSE, SECONDPRINT, SOYOTIME       ");
            parameter.AppendSql("  FROM ADMIN.HIC_WAIT                ");
            parameter.AppendSql(" WHERE JobDate =:JobDate                   ");
            parameter.AppendSql("   AND GbJob  = '1'                        ");
            parameter.AppendSql("   AND GbBuse = :GbBuse                    "); //일반건진
            parameter.AppendSql(" ORDER BY SeqNo                            "); //일반검진

            parameter.Add("JobDate", DateTime.Now.ToShortDateString());
            parameter.Add("GbBuse", argBuse);

            return ExecuteReader<HIC_WAIT>(parameter);
        }

        public HIC_WAIT GetItembyJobDate2(string gstrSysDate, string argGbUse)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(SNAME) CNT                        ");
            parameter.AppendSql(" FROM ADMIN.HIC_WAIT                     ");
            parameter.AppendSql(" WHERE JOBDATE = :JOBDATE                      ");
            parameter.AppendSql(" AND GBJOB = '1'                               ");
            parameter.AppendSql(" AND GBBUSE  = :GBBUSE                         "); //종합검진

            parameter.Add("JOBDATE", DateTime.Now.ToShortDateString());
            parameter.Add("GBBUSE", argGbUse);

            return ExecuteReaderSingle <HIC_WAIT>(parameter);
        }

        public int Update_SecondPrint(string sRid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_WAIT    ");
            parameter.AppendSql("   SET SECONDPRINT = 'Y'       ");
            parameter.AppendSql(" WHERE ROWID       = :RID      ");
            
            parameter.Add("RID", sRid);

            return ExecuteNonQuery(parameter);
        }


        public int DeleteBySeqNo(long nSeqNo, string strJOBDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HIC_WAIT                    ");
            parameter.AppendSql(" WHERE 1=1                                     ");
            parameter.AppendSql(" AND JOBDATE = :JOBDATE                        ");
            parameter.AppendSql(" AND SEQNO = :SEQNO                            ");

            parameter.Add("SEQNO", nSeqNo);
            parameter.Add("JOBDATE", strJOBDATE);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_WAIT> GetListbyJobdateJumin(string strJOBDATE, string strJUMIN)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID RID                               ");
            parameter.AppendSql(" FROM ADMIN.HIC_WAIT                     ");
            parameter.AppendSql(" WHERE JOBDATE = :JOBDATE                      ");
            parameter.AppendSql(" AND JUMIN2 = :JUMIN                           ");

            parameter.Add("JOBDATE", strJOBDATE);
            parameter.Add("JUMIN", strJUMIN);

            return ExecuteReader<HIC_WAIT>(parameter);
        }

    }
}
