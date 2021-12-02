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
    public class HicSangdamWaitRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSangdamWaitRepository()
        {
        }

        public string GetNextRoomByWrtNo(long argWrtNo, string argRoom = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NEXTROOM                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            if (argRoom != "")
            {
                parameter.AppendSql("   AND GUBUN = :GUBUN              ");
            }
            parameter.AppendSql("   AND ENTTIME >= TRUNC(SYSDATE)       ");

            parameter.Add("WRTNO", argWrtNo);
            if (argRoom != "")
            {
                parameter.Add("GUBUN", argRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteScalar<string>(parameter);
        }

        public long GetMaxWaitNoByRoom(string argRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(WAITNO) + 1 WAIT            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND ENTTIME >= TRUNC(SYSDATE)       ");

            parameter.Add("GUBUN", argRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);            

            return ExecuteScalar<long>(parameter);
        }

        public string GetRoomNameByRoomCd(string argRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NEXTROOM                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE GUBUN = :ROOM                   ");
            parameter.AppendSql(" GROUP BY NEXTROOM                     ");

            parameter.Add("ROOM", argRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int GetCountbyWrtNoGubun(string gstrDrRoom, long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT     ");
            parameter.AppendSql(" WHERE CALLTIME IS NOT NULL                        ");
            parameter.AppendSql("   AND (GBCALL IS NULL or GBCALL = '')             ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                              ");
            parameter.AppendSql("   AND WRTNO = :WRTNO                              ");

            parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateCallTimeDisplaybyOnlyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_WAIT SET                    ");
            parameter.AppendSql("       CALLTIME = ''                                       ");
            parameter.AppendSql("     , DISPLAY  = ''                                       ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                                   ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public long GetMaxWaitNobyGubun(string strGubun, string strTemp = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(WaitNo) + 1 MaxNo           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND ENTTIME >= TRUNC(SYSDATE)       ");
            if (strTemp == "N")
            {
                parameter.AppendSql("   AND ENTTIME <= TRUNC(SYSDATE) + 1   ");
            }

            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<long>(parameter);
        }

        public List<HIC_SANGDAM_WAIT> GetItem()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN, WAITNO, PANO             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE ENTTIME >= TRUNC(SYSDATE)       ");
            parameter.AppendSql("   AND ENTTIME <= TRUNC(SYSDATE) + 1   ");
            parameter.AppendSql("   AND GBCALL IS NOT NULL              ");
            parameter.AppendSql(" GROUP By GUBUN, WAITNO, PANO          ");
            parameter.AppendSql(" ORDER BY GUBUN, WAITNO, PANO          ");

            return ExecuteReader<HIC_SANGDAM_WAIT>(parameter);
        }

        public int Insert(HIC_SANGDAM_WAIT item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SANGDAM_WAIT                                                   ");
            parameter.AppendSql("       (WRTNO, SNAME, SEX, AGE, GJJONG, GUBUN, ENTTIME, WAITNO, PANO, NEXTROOM)            ");
            parameter.AppendSql("VALUES                                                                                     ");   
            parameter.AppendSql("       (:WRTNO, :SNAME, :SEX, :AGE, :GJJONG, :GUBUN, SYSDATE, :WAITNO, :PANO, :NEXTROOM)   ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", item.AGE);
            parameter.Add("GJJONG", item.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GUBUN", item.GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WAITNO", item.WAITNO);
            parameter.Add("PANO", item.PANO);
            parameter.Add("NEXTROOM", item.NEXTROOM);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_SANGDAM_WAIT> GetItembyGubunEntTime(string gstrDrRoom, string strFrDate, string strToDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,WAITNO                                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT                            ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                                          ");
            parameter.AppendSql("   AND (GBCALL IS NULL OR GBCALL = '')                         ");
            parameter.AppendSql("   AND ENTTIME >= TO_DATE(:FRDATE, 'YYYY-MM-DD')               ");
            parameter.AppendSql("   AND ENTTIME <= TO_DATE(:TODATE, 'YYYY-MM-DD')               ");
            parameter.AppendSql("   AND WAITNO > 1                                              ");
            parameter.AppendSql(" ORDER BY WaitNo                                               ");

            parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReader<HIC_SANGDAM_WAIT>(parameter);
        }

        public List<HIC_SANGDAM_WAIT> GetWrtNoWaitNobyGubunEntTime(string gstrDrRoom, string strFrDate, string strToDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, WAITNO                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT                        ");
            parameter.AppendSql(" WHERE GUBUN  = :GUBUN                                     ");
            parameter.AppendSql("   AND ENTTIME >= TO_DATE(:FRDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND ENTTIME <= TO_DATE(:TODATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND WAITNO > 1                                          ");
            parameter.AppendSql(" ORDER BY WAITNO                                           ");

            parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReader<HIC_SANGDAM_WAIT>(parameter);
        }

        public List<HIC_SANGDAM_WAIT> GetNextRoombyWrtNoInGubun(long fnWRTNO, List<string> strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NEXTROOM                                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT                        ");
            parameter.AppendSql(" WHERE GUBUN IN (:GUBUN)                                   ");
            parameter.AppendSql("   AND WRTNO = :WRTNO                                      ");

            parameter.AddInStatement("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_SANGDAM_WAIT>(parameter);
        }

        public int InserWrtNoSNameGubunt(HIC_SANGDAM_WAIT item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SANGDAM_WAIT                           ");
            parameter.AppendSql("       (WRTNO,SNAME,GBCALL,GUBUN,ENTTIME,CALLTIME,WAITNO,DISPLAY)  ");
            parameter.AppendSql("VALUES                                                             ");
            parameter.AppendSql("       (:WRTNO, :SNAME, '', :GUBUN, SYSDATE, '', 0, '')            ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("GUBUN", item.GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public HIC_SANGDAM_WAIT Read_Sangdam_Wait_RegList(long argWrtNo, string strPart)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN, GBCALL, ROWID RID            ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_SangDam_WAIT        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                      ");
            parameter.AppendSql("   AND trunc(ENTTIME) >= trunc(SYSDATE)    ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("GUBUN", strPart, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_SANGDAM_WAIT>(parameter);
        }

        public HIC_SANGDAM_WAIT Read_Sangdam_View2(string strPart)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MAX(WaitNo) + 1 WAITNO          ");
            //parameter.AppendSql("     , ROWID RID                       ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND trunc(ENTTIME) >= trunc(SYSDATE)");

            parameter.Add("GUBUN", strPart, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_SANGDAM_WAIT>(parameter);
        }

        public List<HIC_SANGDAM_WAIT> Read_Sangdam_View3(string strPart)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Wrtno, Sname, Gjjong            ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_SangDam_WAIT    ");
            parameter.AppendSql(" WHERE GUBUN = :ROOMCD                 ");
            parameter.AppendSql("   AND (GBCALL IS NULL OR GBCALL = '') ");
            parameter.AppendSql(" ORDER By ENTTIME                      ");

            parameter.Add("ROOMCD", strPart, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_SANGDAM_WAIT>(parameter);
        }

        public List<HIC_SANGDAM_NEW> GetOnlyNextRoomByWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NEXTROOM                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND Gubun IN ('08','09')            ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_SANGDAM_NEW>(parameter);
        }

        public List<HIC_SANGDAM_WAIT> Read_Sangdam_Wait_List(string strPart)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME, WRTNO                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT        ");
            parameter.AppendSql(" WHERE TRUNC(ENTTIME) >= TRUNC(SYSDATE)    ");
            parameter.AppendSql("   AND (GBCALL IS NULL OR GBCALL = '')     ");
            if (!strPart.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GUBUN = :PART                   ");
            }
            parameter.AppendSql(" ORDER By WAITNO                           ");

            if (!strPart.IsNullOrEmpty())
            {
                parameter.Add("PART", strPart, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_SANGDAM_WAIT>(parameter);
        }

        public HIC_SANGDAM_WAIT Read_Sangdam_WrtNoCnt(string strRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(WRTNO) CNT                ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND trunc(ENTTIME) >= trunc(SYSDATE)");
            parameter.AppendSql("   AND(GBCALL IS NULL OR GBCALL = '')  ");

            parameter.Add("GUBUN", strRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_SANGDAM_WAIT>(parameter);
        }

        public int Insert_Sangdam_Wait3(HIC_SANGDAM_WAIT item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SANGDAM_WAIT       ");
            parameter.AppendSql("       (WRTNO, SNAME, SEX, AGE, GJJONG         ");
            parameter.AppendSql("     , GUBUN, ENTTIME,  WAITNO, PANO)          ");
            parameter.AppendSql("VALUES                                         ");
            parameter.AppendSql("       (:WRTNO, :SNAME, :SEX, :AGE, :GJJONG    ");
            parameter.AppendSql("     , :GUBUN, SYSDATE, :WAITNO, :PANO)        ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", item.AGE);
            parameter.Add("GJJONG", item.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GUBUN", item.GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WAITNO", item.WAITNO);
            parameter.Add("PANO", item.PANO);

            return ExecuteNonQuery(parameter);
        }

        public HIC_SANGDAM_WAIT Read_Sangdam_View(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME,SEX,AGE,GJJONG            ");
            parameter.AppendSql("     , ROWID RID                       ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReaderSingle<HIC_SANGDAM_WAIT>(parameter);
        }

        public int Delete_Sangdam_PreData(long argWrtNo, string strPart)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");
            parameter.AppendSql("   AND GUBUN    = :ROOMCD              ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("ROOMCD", strPart, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public HIC_SANGDAM_WAIT Read_Sangdam_EtcRoomReg(long argWrtNo, string strPart)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN,TO_CHAR(CALLTIME,'YYYY-MM-DD') CALLTIME   ");
            parameter.AppendSql("     , ROWID RID                                       ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_SANGDAM_WAIT                    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                                  ");
            parameter.AppendSql("   AND (GBCALL IS NULL OR GBCALL = '')                 ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("GUBUN", strPart, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_SANGDAM_WAIT>(parameter);
        }

        public HIC_SANGDAM_WAIT Read_Exam_Wait(string argDate, string hCROOM)//, string strJepsuGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT                            ");
            parameter.AppendSql(" WHERE ENTTIME >= TO_DATE(:SDATE, 'yyyy-MM-dd')                ");
            //parameter.AppendSql("   AND Gubun <> '12'                                           "); //상담대기
            if (!hCROOM.IsNullOrEmpty())
            {   
                parameter.AppendSql("   AND TRIM(GUBUN) = :GUBUN                                ");
            }
            parameter.AppendSql("   AND (GbCall IS NULL OR GbCall = '')                         ");
            //if (strJepsuGubun == "1")
            //{
            //    parameter.AppendSql("   AND GBCHUL = 'N'                                        ");
            //}
            //else if (strJepsuGubun == "2")
            //{
            //    parameter.AppendSql("   AND GBCHUL = 'Y'                                        ");
            //}

            parameter.Add("SDATE", argDate);
            if (!hCROOM.IsNullOrEmpty())
            {
                parameter.Add("GUBUN", hCROOM, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReaderSingle<HIC_SANGDAM_WAIT>(parameter);
        }

        public List<HIC_SANGDAM_WAIT> Read_Now_Wait(string fstrRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME, WRTNO                                ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_SangDam_WAIT                ");
            parameter.AppendSql(" WHERE TRUNC(ENTTIME) = TRUNC(SYSDATE)             ");
            if (!fstrRoom.IsNullOrEmpty() && fstrRoom != "ALL")
            {
                parameter.AppendSql("   AND GUBUN = :ROOMCD                         ");
            }
            //parameter.AppendSql("   AND Gubun <> '12'                               ");    //상담대기 제외
            parameter.AppendSql("   AND GBCALL IS NULL                              ");
            parameter.AppendSql(" ORDER BY WAITNO                                   ");

            if (!fstrRoom.IsNullOrEmpty() && fstrRoom != "ALL")
            {
                parameter.Add("ROOMCD", fstrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_SANGDAM_WAIT>(parameter);
        }

        public int DeletebyEntTime(string strDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT                   ");
            parameter.AppendSql(" WHERE ENTTIME = TO_DATE(:ENTTIME, 'YYYY-MM-DD HH24:MI')   ");

            parameter.Add("ENTTIME", strDate);

            return ExecuteNonQuery(parameter);
        }

        public int DeletebyGubunSName(string fstrRoom, string strSName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT       ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                          ");
            parameter.AppendSql("   AND SNAME = :SNAME                          ");

            parameter.Add("GUBUN", fstrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SNAME", strSName);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyGubunSName(string fstrRoom, string strName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT                ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                              ");
            parameter.AppendSql("   AND SNAME = :SNAME                              ");

            parameter.Add("GUBUN", fstrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SNAME", strName);

            return ExecuteScalar<int>(parameter);
        }

        public string GetNextRoomByWrtNoInGubun(long fnWRTNO, string[] strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NEXTROOM                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT                ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND GUBUN IN (:GUBUN)                           ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.AddInStatement("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_SANGDAM_WAIT> GetWrtNoWaitNo(string gstrDrRoom, string strFrDate, string strToDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, WAITNO                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT                ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                              ");
            parameter.AppendSql("   AND ENTTIME >= TO_DATE(:FRDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND ENTTIME <= TO_DATE(:TODATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND WAITNO > 1                                  ");
            parameter.AppendSql(" ORDER BY WaitNo                                   ");

            parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReader<HIC_SANGDAM_WAIT>(parameter);
        }

        public HIC_SANGDAM_WAIT GetWaitNobyGubun(string gstrDrRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WAITNO                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE GUBUN IN(:GUBUN)                ");
            parameter.AppendSql("   AND WAITNO = 2                      ");

            parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_SANGDAM_WAIT>(parameter);
        }

        public int DeletebyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT                       ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                         ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateCallTimebyWrtNoGubun(long nWrtNo, string gstrDrRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_WAIT SET                        ");
            parameter.AppendSql("       CALLTIME = SYSDATE                                      ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                         ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                                         ");
            parameter.AppendSql("   AND (GBCALL IS NULL or GBCALL = '')                         ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_SANGDAM_WAIT> GetWrtNobyGubunNotWrtNo(string gstrDrRoom, long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT        ");
            parameter.AppendSql(" WHERE CALLTIME IS NOT NULL                ");
            parameter.AppendSql("   AND (GBCALL IS NULL or GBCALL = '')     ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                     ");
            parameter.AppendSql("   AND WRTNO != :WRTNO                     ");

            parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HIC_SANGDAM_WAIT>(parameter);
        }

        public int UpdateWaitNobyWrtnoGubunEntTime(int nWaitNo, long wRTNO, string gstrDrRoom, string strFrDate1, string strToDate1)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_WAIT SET                    ");
            parameter.AppendSql("       WAITNO = :WAITNO                                    ");            
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                     ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                                     ");
            parameter.AppendSql("   AND ENTTIME >= TO_DATE(:FRDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND ENTTIME <= TO_DATE(:TODATE, 'YYYY-MM-DD')           ");

            parameter.Add("WAITNO", nWaitNo);
            parameter.Add("WRTNO", wRTNO);
            parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FRDATE", strFrDate1);
            parameter.Add("TODATE", strToDate1);

            return ExecuteNonQuery(parameter);
        }

        public HIC_SANGDAM_WAIT GetMaxWaitNobyDrRoom(string gstrDrRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WAITNO                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT                        ");
            parameter.AppendSql(" WHERE GUBUN  = :GUBUN                                     ");
            parameter.AppendSql("   AND WAITNO = 2                                          ");

            parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<HIC_SANGDAM_WAIT>(parameter);
        }

        public string GetGubunbyWrtNo(long gnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN                                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT                        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                      ");

            parameter.Add("WRTNO", gnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateWaitNoGubunbyWrtNo(string gstrDrRoom, long gnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_WAIT SET                    ");
            parameter.AppendSql("       WAITNO = '2'                                        ");
            parameter.AppendSql("     , GUBUN  = :GUBUN                                     ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                     ");

            parameter.Add("WRTNO", gnWRTNO);
            parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateWaitNobyWrtNo(int nWaitNo, long wRTNO, string gstrDrRoom, string strFrDate1, string strToDate1)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_WAIT SET                    ");
            parameter.AppendSql("       WAITNO = :WAITNO                                    ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                     ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                                     ");
            parameter.AppendSql("   AND ENTTIME >= TO_DATE(:FRDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND ENTTIME <= TO_DATE(:TODATE, 'YYYY-MM-DD')           ");

            parameter.Add("WAITNO", nWaitNo); 
            parameter.Add("WRTNO", wRTNO);
            parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FRDATE", strFrDate1);
            parameter.Add("TODATE", strToDate1);

            return ExecuteNonQuery(parameter);
        }

        public HIC_SANGDAM_WAIT GetMaxWaitNobyGubunWaitNo(string gstrDrRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WAITNO                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE GUBUN  = :GUBUN                 ");
            parameter.AppendSql("   AND WAITNO = 2                      ");

            parameter.Add("WRTNO", gstrDrRoom);

            return ExecuteScalar<HIC_SANGDAM_WAIT>(parameter);
        }

        public string GetGubunbyPaNoWrtNo(long gnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN FROM HIC_SANGDAM_WAIT WHERE WRTNO = :WRTNO    ");

            parameter.Add("WRTNO", gnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateCallTimebyWrtNo(string strSysDate, long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_WAIT SET                    ");
            parameter.AppendSql("       CALLTIME = TO_DATE(:SYSDATE, 'YYYY-MM-DD HH24:MI')  ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                                   ");
            parameter.AppendSql("   AND GUBUN IN('8', '9')                                  ");
            parameter.AppendSql("   AND(GBCALL IS NULL or GBCALL = '')                      ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("SYSDATE", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int DeletebyPaNo(long fnPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SANGDAM_WAIT");
            parameter.AppendSql(" WHERE PANO = :PANO                ");

            parameter.Add("PANO", fnPano);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateGbCallCallTimeGubunbyWrtNo(string gstrDrRoom, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       GBCALL   = 'Y'                      ");
            parameter.AppendSql("     , CallTime = SYSDATE                  ");
            parameter.AppendSql("     , GUBUN    = :GUBUN                   ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                   ");

            parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateGbCallGubunbyWrtNo(string gstrDrRoom, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       GBCALL   = 'Y'                      ");
            parameter.AppendSql("     , GUBUN    = :GUBUN                   ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                   ");

            parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateCallTimeDisplaybyWrtNo(List<string> strWrtNo, string strGubun = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       CALLTIME = ''                       ");
            parameter.AppendSql("     , DISPLAY  = ''                       ");
            parameter.AppendSql(" WHERE WRTNO IN (:WRTNO)                   ");
            if (strGubun != "NOT")
            {
                parameter.AppendSql("   AND GUBUN IN('8', '9')              ");
            }

            parameter.AddInStatement("WRTNO", strWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_SANGDAM_WAIT> GetWrtNobyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT            ");
            parameter.AppendSql(" WHERE WRTNO != :WRTNO                         ");
            parameter.AppendSql("   AND CALLTIME IS NOT NULL                    ");
            parameter.AppendSql("   AND (GBCALL IS NULL or GBCALL = '')         ");
            parameter.AppendSql("   AND GUBUN IN ('8','9')                      ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HIC_SANGDAM_WAIT>(parameter);
        }

        public int GetCountbyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND EntTime>=TRUNC(SYSDATE)         ");
            parameter.AppendSql("   AND GUBUN IN ('8','9')              "); //구강상담실

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyOnlyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_SANGDAM_WAIT> GetNextRoombyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NEXTROOM                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND GUBUN IN ('08','09')            ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_SANGDAM_WAIT>(parameter);
        }

        public int InsertCall(string gstrDrRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SANGDAM_WAIT                            ");
            parameter.AppendSql("       (WRTNO,SNAME,GBCALL,GUBUN,ENTTIME,CALLTIME,WAITNO,DISPLAY)   ");
            parameter.AppendSql("VALUES                                                              ");
            parameter.AppendSql("       (0, '{수검자호출}', '', :GUBUN, SYSDATE, '', 0, '')          ");

            parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int DeletebyGubun(string gstrDrRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SANGDAM_WAIT        ");
            parameter.AppendSql(" WHERE WRTNO = 0                           ");
            parameter.AppendSql("   AND SName IN ('{수검자호출}','{자리비움}')");
            parameter.AppendSql("   AND GUBUN= :GUBUN                       ");

            parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public long GetWaitbyGubunEntTime(string strRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(WaitNo) + 1 MaxNo                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT            ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                          ");
            parameter.AppendSql("   AND TRUNC(ENTTIME) >= TRUNC(SYSDATE)        ");

            parameter.Add("GUBUN", strRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<long>(parameter);
        }

        public int UpdateGbCallbyWrtNo(string gstrDrRoom, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       GBCALL   = 'Y'                      ");
            parameter.AppendSql("     , CALLTIME = SYSDATE                  ");
            if (!gstrDrRoom.IsNullOrEmpty())
            {
                parameter.AppendSql("     , GUBUN    = :GUBUN               ");
            }
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                   ");

            if (!gstrDrRoom.IsNullOrEmpty())
            {
                parameter.Add("GUBUN", gstrDrRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public string GetINextRoombyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NEXTROOM                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND GUBUN IN ('08','09')            "); //구강상담

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_SANGDAM_WAIT> GetItem_GbCallNull()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME, AGE, SEX, GUBUN, PANO, WAITNO    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT            ");
            parameter.AppendSql(" WHERE ENTTIME >= TRUNC(SYSDATE)               ");
            parameter.AppendSql("   AND ENTTIME <= TRUNC(SYSDATE) + 1           ");
            parameter.AppendSql("   AND GBCALL IS NULL                          ");
            parameter.AppendSql(" GROUP By SNAME, AGE, SEX, GUBUN, PANO, WAITNO ");
            parameter.AppendSql(" ORDER BY GUBUN, WAITNO                        ");

            return ExecuteReader<HIC_SANGDAM_WAIT>(parameter);
        }

        public int Delete_JepsuCancel()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE HIC_SANGDAM_WAIT                                    ");
            parameter.AppendSql(" WHERE WRTNO IN (SELECT a.WRTNO                            ");
            parameter.AppendSql("                   FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT a     ");
            parameter.AppendSql("                      , KOSMOS_PMPA.HIC_JEPSU        b     "); 
            parameter.AppendSql("                  WHERE a.ENTTIME >= TRUNC(SYSDATE)        ");
            parameter.AppendSql("                    AND a.ENTTIME <= TRUNC(SYSDATE) + 1    ");
            parameter.AppendSql("                    AND a.GBCALL IS NULL                   ");
            parameter.AppendSql("                    AND a.WRTNO=b.WRTNO(+)                 ");
            parameter.AppendSql("                    AND b.DelDate IS NOT NULL)             ");

            return ExecuteNonQuery(parameter);
        }

        public int UpdateWaitNobyPaNo(HIC_SANGDAM_WAIT item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       GUBUN  = :GUBUN                     ");
            parameter.AppendSql("       WAITNO = :WAITNO                    ");
            parameter.AppendSql(" WHERE PANO   = :PANO                      ");
            parameter.AppendSql("   AND ENTTIME >= TRUNC(SYSDATE)           ");
            parameter.AppendSql("   AND ENTTIME <= TRUNC(SYSDATE) + 1       ");

            parameter.Add("GUBUN", item.GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WAITNO", item.WAITNO);
            parameter.Add("PANO", item.PANO);

            return ExecuteNonQuery(parameter);
        }

        public HIC_SANGDAM_WAIT GetNextRoomByWrtNoGubun(long fnWRTNO, string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NEXTROOM, ROOMNAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                  ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_SANGDAM_WAIT>(parameter);
        }

        public int GetMaxWaitNobyRoomCd(string argRoom)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MAX(WaitNo) + 1 WAITNO          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND ENTTIME >= TRUNC(SYSDATE)       ");

            parameter.Add("GUBUN", argRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public HIC_SANGDAM_WAIT GetNextRoomGubunByWrtNo(long fWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN, NEXTROOM                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND EntTime>=TRUNC(SYSDATE)         ");

            parameter.Add("WRTNO", fWrtNo);

            return ExecuteReaderSingle<HIC_SANGDAM_WAIT>(parameter);
        }

        public int GetWaitCountbyRoomCd(string strRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND CallTime IS NULL                ");

            parameter.Add("GUBUN", strRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int Insert_Hic_Sangdam_Wait(long nWrtNo, string strSName, string strSex, long nAge, string strGjJong, string strRoom, long nWait, long argPano, string strNextRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SANGDAM_WAIT               ");
            parameter.AppendSql("       (WRTNO, SNAME, SEX, AGE, GJJONG                 ");
            parameter.AppendSql("      , GUBUN, ENTTIME, WAITNO, PANO, NEXTROOM)        ");
            parameter.AppendSql("VALUES                                                 ");
            parameter.AppendSql("       (:WRTNO, :SNAME, :SEX, :AGE, :GJJONG            ");
            parameter.AppendSql("      , :GUBUN, SYSDATE, :WAITNO, :PANO, :NEXTROOM)    ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("SNAME", strSName);
            parameter.Add("SEX", strSex, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", nAge);
            parameter.Add("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GUBUN", strRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WAITNO", nWait);
            parameter.Add("PANO", argPano);
            parameter.Add("NEXTROOM", strNextRoom);

            return ExecuteNonQuery(parameter);
        }

        public int Delete_Hic_Sangdam_Wait(long argPano, string strGubun = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE HIC_SANGDAM_WAIT                    ");
            parameter.AppendSql(" WHERE PANO  = :PANO                       ");
            if (strGubun == "Y")
            {
                parameter.AppendSql("   AND ENTTIME >= TRUNC(SYSDATE)       ");
                parameter.AppendSql("   AND ENTTIME <= TRUNC(SYSDATE) + 1   ");
            }

            parameter.Add("PANO", argPano);

            return ExecuteNonQuery(parameter);
        }

        public int Update_Patient_Call(long argPano, string argRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       GBCALL = 'Y'                        "); 
            parameter.AppendSql("     , CALLTIME = SYSDATE                  ");
            parameter.AppendSql("     , GUBUN = :GUBUN                      ");
            parameter.AppendSql(" WHERE PANO  = :PANO                       ");

            parameter.Add("PANO", argPano);
            parameter.Add("GUBUN", argRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }
    }
}
