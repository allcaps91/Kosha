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
    public class HicCharttransRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicCharttransRepository()
        {
        }


        public List<HIC_CHARTTRANS> GetAllbyTrDate(string strTRDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,SName,GjJong,TrList,TO_CHAR(EntTime,'HH24:MI') EntTime            ");
            parameter.AppendSql(" ,EntSabun,TO_CHAR(RecvTime,'HH24:MI') RecvTime,RecvSabun,Remark               ");
            parameter.AppendSql("  FROM ADMIN.HIC_CHARTTRANS                                              ");
            parameter.AppendSql("  WHERE  1=1                                                                   ");
            parameter.AppendSql("   AND TRDATE = TO_DATE(:TRDATE,'YYYY-MM-DD')                                  ");
            parameter.AppendSql(" ORDER BY ENTTIME DESC                                                         ");

            parameter.Add("TRDATE", strTRDATE);

            return ExecuteReader<HIC_CHARTTRANS>(parameter);
        }

        public HIC_CHARTTRANS GetAllbyWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,SName,GjJong,TrList,TO_CHAR(EntTime,'YYYY-MM-DD HH24:MI') EntTime,EntSabun    ");
            parameter.AppendSql("     , TO_CHAR(RecvTime,'YYYY-MM-DD HH24:MI') RecvTime,RecvSabun,Remark,ROWID              ");
            parameter.AppendSql("     , TO_CHAR(RemarkTime,'YYYY-MM-DD HH24:MI') REMARKTIME                                 ");
            parameter.AppendSql("  FROM ADMIN.HIC_CHARTTRANS                                                          ");
            parameter.AppendSql(" WHERE 1=1                                                                                 ");
            parameter.AppendSql("   AND WRTNO = :WRTNO                                                                      ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_CHARTTRANS>(parameter);
        }

        public int UpdaterevbTimeRecvSabunbyWrtno(long fnWrtNo, string idNumber)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_CHARTTRANS SET  ");
            parameter.AppendSql("       RECVTIME  = SYSDATE             ");
            parameter.AppendSql("     , RECVSABUN = :RECVSABUN          ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO              ");

            parameter.Add("WRTNO", fnWrtNo);
            parameter.Add("RECVSABUN", idNumber);

            return ExecuteNonQuery(parameter);
        }

        public string GetRowIdbyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                           ");
            parameter.AppendSql("  FROM ADMIN.HIC_CHARTTRANS      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_CHARTTRANS> GetItembyTrDate(string strFrDate, string strToDate, string strSname, long nWRTNO, string strOut, string strAmPm, string strNoTrans, string strJob)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(b.JepDate,'YYYY-MM-DD') JepDate,b.GjJong,b.GbChul                               ");
            parameter.AppendSql("     , a.WRTNO,b.SName,a.TrList,TO_CHAR(a.EntTime,'YYYY-MM-DD HH24:MI') EntTime,a.EntSabun     ");
            parameter.AppendSql("     , TO_CHAR(a.RecvTime,'YYYY-MM-DD HH24:MI') RecvTime,a.RecvSabun,a.Remark,a.ROWID          ");
            parameter.AppendSql("  FROM ADMIN.HIC_CHARTTRANS a, ADMIN.HIC_JEPSU b                                   ");
            parameter.AppendSql(" WHERE a.TrDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                              ");
            parameter.AppendSql("   AND a.TrDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                              ");
            parameter.AppendSql("   AND a.EntTime IS NOT NULL                                                                   ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                    ");
            if (strOut == "0")
            {
                parameter.AppendSql("   AND b.GbChul = 'N'                                                                      ");
            }
            else if (strOut == "1")
            {
                parameter.AppendSql("   AND b.GbChul = 'Y'                                                                      ");
            }
            if (strAmPm == "0")
            {
                parameter.AppendSql("   AND TO_CHAR(b.EntTime,'HH24:MI') <= '12:30'                                             ");
            }
            else if (strAmPm == "1")
            {
                parameter.AppendSql("   AND TO_CHAR(b.EntTime,'HH24:MI') >= '12:31'                                             ");
            }
            if (strNoTrans == "1")
            {
                parameter.AppendSql("   AND a.GjJong NOT IN ('55')                                                              "); //운전면허
            }
            else if (strNoTrans == "2")
            {
                parameter.AppendSql("   AND B.GjJong IN ('11','31')                                                             "); //순수일반 + 암검진
                parameter.AppendSql("   AND B.UCODES IS NULL                                                                    "); 
            }
            if (strJob == "3")
            {
                parameter.AppendSql("   AND a.RecvTime IS NULL                                                                  ");
            }
            if (!strSname.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME = :SNAME                                                                    ");
            }
            if (nWRTNO != 0)
            {
                parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                                    ");
            }
            parameter.AppendSql(" ORDER BY TrDate, SName                                                                        ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strSname.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strSname);
            }
            if (nWRTNO != 0)
            {
                parameter.Add("WRTNO", nWRTNO);
            }

            return ExecuteReader<HIC_CHARTTRANS>(parameter);
        }

        public int InsertAll(HIC_CHARTTRANS item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_CHARTTRANS                                             ");
            parameter.AppendSql("       (TRDATE, WRTNO, SNAME, GJJONG, TRLIST, REMARK, REMARKTIME)                  ");
            parameter.AppendSql("VALUES                                                                             ");
            parameter.AppendSql("       (TO_DATE(:TRDATE, 'yyyy-mm-dd'), :WRTNO, :SNAME, :GJJONG, :TRLIST, :REMARK  ");
            if (item.REMARKTIME == "SYSDATE")
            {
                parameter.AppendSql("     , SYSDATE)                                                                ");
            }
            else
            {
                parameter.AppendSql("     , '')                                                                     ");
            }

            parameter.Add("TRDATE", item.TRDATE);
            parameter.Add("WRTNO", item.WRTNO);            
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("GJJONG", item.GJJONG);
            parameter.Add("TRLIST", item.TRLIST);
            parameter.Add("REMARK", item.REMARK);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyRowId(string strROWID, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_CHARTTRANS SET  ");
            parameter.AppendSql("       REMARK     = ''                 ");
            if (strGubun == "Y")
            {
                parameter.AppendSql("     , REMARKTIME = SYSDATE        ");
            }
            else
            {
                parameter.AppendSql("     , REMARKTIME = ''             ");
            }
            parameter.AppendSql(" WHERE ROWID = :RID                    ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int DeletebyRowId(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HIC_CHARTTRANS      ");
            parameter.AppendSql(" WHERE ROWID = :RID                    ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public HIC_CHARTTRANS GetItembyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TrList,TO_CHAR(EntTime,'HH24:MI') EntTime,EntSabun          ");
            parameter.AppendSql("  FROM ADMIN.HIC_CHARTTRANS                                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                              ");
            parameter.AppendSql("   AND ENTSABUN IS NOT NULL                                        ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReaderSingle<HIC_CHARTTRANS>(parameter);
        }

        public List<HIC_CHARTTRANS> GetItembySysDate()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,SName,GjJong,TrList,TO_CHAR(EntTime,'HH24:MI') EntTime,EntSabun   ");
            parameter.AppendSql("     , TO_CHAR(RecvTime,'HH24:MI') RecvTime,RecvSabun                          ");
            parameter.AppendSql("  FROM ADMIN.HIC_CHARTTRANS                                              ");
            parameter.AppendSql(" WHERE TrDate = TRUNC(SYSDATE)                                                 ");
            parameter.AppendSql("   AND RecvTime IS NOT NULL                                                    ");
            parameter.AppendSql(" ORDER BY RecvTime DESC                                                        ");

            return ExecuteReader<HIC_CHARTTRANS>(parameter);
        }

        public int UpdaerecvSabunRecvTimebyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_CHARTTRANS SET      ");
            parameter.AppendSql("       RECVSABUN = 0                       ");
            parameter.AppendSql("     , RECVTIME  = ''                      ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO                  ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int InsertbySelect(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_CHARTTRANS_HIS                                                         ");
            parameter.AppendSql("       (WRTNO, TRDATE, SNAME, GJJONG, TRLIST, ENTTIME, ENTSABUN, RECVSABUN, RECVTIME, BACKUPTIME)  ");
            parameter.AppendSql("SELECT WRTNO, TRDATE, SNAME, GJJONG, TRLIST, ENTTIME, ENTSABUN, RECVSABUN, RECVTIME, SYSDATE       ");
            parameter.AppendSql("  FROM ADMIN.HIC_CHARTTRANS                                                                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                              ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_CHARTTRANS item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_CHARTTRANS                             ");
            parameter.AppendSql(" (WRTNO, TRDATE, SNAME, GJJONG, TRLIST, ENTTIME, ENTSABUN)         ");
            parameter.AppendSql("VALUES                                                             ");
            parameter.AppendSql(" (:WRTNO, :TRDATE, :SNAME, :GJJONG, :TRLIST, SYSDATE, :ENTSABUN)  ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("TRDATE", item.TRDATE.Substring(0, 10));
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("GJJONG", item.GJJONG);
            parameter.Add("TRLIST", item.TRLIST);
            //parameter.Add("ENTTIME", item.ENTTIME.Substring(0, 10));
            parameter.Add("ENTSABUN", item.ENTSABUN);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyWrtno(long nWRTNO, string strTRDATE, string strTRLIST, string strENTTIME, string strENTSABUN, string strREMARK)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_CHARTTRANS SET          ");
            if (strTRDATE != "")
            {
                parameter.AppendSql(" TRDATE = TO_DATE(:TRDATE, 'YYYY-MM-DD'),  ");
            }
            if (strREMARK != "" )
            {
                parameter.AppendSql(" REMARK = :REMARK,                         ");
            }
            parameter.AppendSql(" TRLIST = :TRLIST,                             ");
            parameter.AppendSql(" ENTTIME = SYSDATE,                            ");
            parameter.AppendSql(" ENTSABUN = :ENTSABUN                          ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                        ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("TRDATE", strTRDATE);
            parameter.Add("TRLIST", strTRLIST);
            //parameter.Add("ENTTIME", strENTTIME);
            parameter.Add("ENTSABUN", strENTSABUN);
            parameter.Add("REMARK", strREMARK); 

            return ExecuteNonQuery(parameter);
        }

        public int DeleteData(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HIC_CHARTTRANS  ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteNonQuery(parameter);
        }
    }
}
