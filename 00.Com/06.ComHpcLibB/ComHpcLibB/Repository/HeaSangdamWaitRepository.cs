namespace ComHpcLibB.Repository
{
    using ComBase;
    using ComBase.Mvc;
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using System.Data;
    using ComBase.Controls;

    /// <summary>
    /// 
    /// </summary>
    public class HeaSangdamWaitRepository : BaseRepository
    {
        public Dictionary<string, object> Sangdam_Wait_Update(long WRTNO, string GUBUN)
        {
            MParameter parameter = CreateParameter();
            parameter.commandType = CommandType.StoredProcedure;
            parameter.AppendSql("ADMIN.PC_HIC_WAIT_PATIENT_CLEAR");

            parameter.AddProcIn("IN_WRTNO", WRTNO);
            parameter.AddProcIn("IN_ROOMCD", GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteProc(parameter); 
        }

        //public int Update_Patient_Call(long WRTNO, string ROOMCD)
        public int Update_Patient_Call(long WRTNO, List<string> ROOMCD)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HEA_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       CALLTIME = SYSDATE                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");
            //parameter.AppendSql("   AND GUBUN = :GUBUN                      ");
            parameter.AppendSql("   AND GUBUN IN (:GUBUN)                   ");
            parameter.AppendSql("   AND(GBCALL IS NULL or GBCALL = '')      ");

            parameter.Add("WRTNO", WRTNO);
            //parameter.Add("GUBUN", ROOMCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("GUBUN", ROOMCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_SANGDAM_WAIT> Read_Now_Wait(string ROOMCD)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Sname,Wrtno                                 ");
            parameter.AppendSql("  From ADMIN.HEA_SangDam_WAIT                ");            
            parameter.AppendSql(" WHERE TRUNC(ENTTIME) = TRUNC(SYSDATE)             ");
            if (!ROOMCD.IsNullOrEmpty() && ROOMCD != "ALL")
            {
                parameter.AppendSql("   AND GUBUN = :ROOMCD                         ");
            }
            parameter.AppendSql("   AND Gubun <> '12'                               ");    //상담대기 제외
            parameter.AppendSql("   AND GbCall IS NULL                              ");
            parameter.AppendSql(" ORDER By WaitNo                                   ");

            if (!ROOMCD.IsNullOrEmpty() && ROOMCD != "ALL")
            {
                parameter.Add("ROOMCD", ROOMCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            
            return ExecuteReader<HEA_SANGDAM_WAIT>(parameter);
        }

        
        public List<HEA_SANGDAM_WAIT> Etc_ExamRoom_RegConfirm(long WRTNO, string ROOMCD)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN                           ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            if (!ROOMCD.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GUBUN != :ROOMCD            ");
            }
            parameter.AppendSql("   AND(GBCALL IS NULL OR GBCALL = '')  ");

            parameter.Add("WRTNO", WRTNO);
            if (!ROOMCD.IsNullOrEmpty())
            {
                parameter.Add("ROOMCD", ROOMCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HEA_SANGDAM_WAIT>(parameter);
        }

        public int GetRowIdbyWrtNo(long nWrtNo, string strFrDate, string strToDate, string[] strEndo_Room)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                                       ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT                ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND ENTTIME >= TO_DATE(:FRDATE, 'yyyy-MM-dd')   ");
            parameter.AppendSql("   AND ENTTIME <= TO_DATE(:TODATE, 'yyyy-MM-dd')   ");
            parameter.AppendSql("   AND (GBCALL IS NULL OR GBCALL = '')             ");
            parameter.AppendSql("   AND GUBUN IN (:GUBUN)                           ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.AddInStatement("GUBUN", strEndo_Room, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public HEA_SANGDAM_WAIT GetGBCallbyWrtNo(long nWrtNo, string strFrDate, string strToDate, List<string> strEndo_Room)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBCALL                                              ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT                        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                      ");
            parameter.AppendSql("   AND ENTTIME >= TO_DATE(:FRDATE, 'yyyy-MM-dd') - 0.99999 ");
            parameter.AppendSql("   AND ENTTIME <= TO_DATE(:TODATE, 'yyyy-MM-dd') + 0.99999 ");
            parameter.AppendSql("   AND (GBCALL IS NULL OR GBCALL = '')                     ");
            parameter.AppendSql("   AND GUBUN IN (:GUBUN)                                   ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.AddInStatement("GUBUN", strEndo_Room, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<HEA_SANGDAM_WAIT>(parameter);
        }

        public HEA_SANGDAM_WAIT GetGbCallbyWrtNoEntTime(long nWrtNo, string strFrDate, string strToDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID, GBCALL                                   ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT                        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                      ");
            parameter.AppendSql("   AND ENTTIME >= TO_DATE(:FRDATE, 'yyyy-MM-dd')           ");
            parameter.AppendSql("   AND ENTTIME <  TO_DATE(:TODATE, 'yyyy-MM-dd')           ");
            parameter.AppendSql("   AND GUBUN = '12'                                        ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReaderSingle<HEA_SANGDAM_WAIT>(parameter);
        }

        public int Delete_Sangdam_Wait(long WRTNO, string ROOMCD)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE ADMIN.HEA_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND GUBUN != :GUBUN                 ");
            parameter.AppendSql("   AND(GBCALL IS NULL or GBCALL = '')  ");

            parameter.Add("WRTNO", WRTNO);
            parameter.Add("GUBUN", ROOMCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public HEA_SANGDAM_WAIT GetCountWaitbyEndoRoom(string strPtNo, string eNDO_ROOM)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                       ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE ENTTIME = TRUNC(SYSDATE)        ");
            parameter.AppendSql("   AND WRTNO   = :WRTNO                ");
            parameter.AppendSql("   AND GUBUN   = :GUBUN                ");

            parameter.Add("WRTNO", strPtNo);
            parameter.Add("GUBUN", eNDO_ROOM, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_SANGDAM_WAIT>(parameter);
        }

        public int GetRowIdbyEndoRoom(long fnWRTNO, string dENT_ROOM)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                ");
            parameter.AppendSql("   AND ENTTIME >= TRUNC(SYSDATE)       ");
            parameter.AppendSql("   AND GUBUN   = :GUBUN                ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("GUBUN", dENT_ROOM, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyEntTimeGubun(string strFrDate, string strToDate, string strRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(WRTNO) CNT                                ");
            parameter.AppendSql("  FROM ADMIN.HEA_SANGDAM_WAIT                    ");
            parameter.AppendSql(" WHERE ENTTIME >= TO_DATE(:FRDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND ENTTIME <  TO_DATE(:TODATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                                  ");
            parameter.AppendSql("   AND (GbCall IS NULL OR GbCall = '')                 ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GUBUN", strRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public HEA_SANGDAM_WAIT GetGbCallGbDentbyEntTimeGubun(string strFrDate, string strToDate, string strRoom, long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBCALL, GBDENT                                  ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT                    ");
            parameter.AppendSql(" WHERE ENTTIME BETWEEN TO_DATE(:FRDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("                   AND TO_DATE(:TODATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND GUBUN   = :GUBUN                                ");
            parameter.AppendSql("   AND WRTNO   = :WRTNO                                ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GUBUN", strRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReaderSingle<HEA_SANGDAM_WAIT>(parameter);
        }

        public string GetGBCallbyWrtNoGubun(long fnWRTNO, string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBCALL                          ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                ");
            parameter.AppendSql("   AND GUBUN   = :GUBUN                ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int GetCountbyWrtNoGubun(long nWrtNo, List<string> strACT_SET_Data)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND GUBUN IN (:GUBUN)               ");
            parameter.AppendSql("   AND GbCall = 'Y'                    ");
            parameter.AppendSql("   AND CallTime>=TRUNC(SYSDATE)        ");
            
            parameter.Add("WRTNO", nWrtNo);
            parameter.AddInStatement("GUBUN", strACT_SET_Data, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateGbCallbyWrtNoGubun(string strGbCall, long nWRTNO, string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HIC_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       GBCALL = :GBCALL                    ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                     ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                     ");

            parameter.Add("GBCALL", strGbCall);
            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_SANGDAM_WAIT> GetItembyGubunEntTime(string strGubun, string strFrDate, string strToDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Wrtno,Sname,GbEndo,TO_CHAR(ENTTIME,'HH24:MI') ENT, TO_CHAR(CALLTIME, 'HH24:MI') CALL    ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT                                                            ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                                                                          ");
            parameter.AppendSql("   AND (GBCALL IS NULL OR GBCALL = '')                                                         ");
            parameter.AppendSql("   AND ENTTIME >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                               ");
            parameter.AppendSql("   AND ENTTIME <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                               ");
            parameter.AppendSql(" ORDER By WaitNo                                                                               ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_SANGDAM_WAIT>(parameter);
        }

        public int UpdateCallTimebyWrtNo(string strWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HIC_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       CALLTIME = ''                       ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                   ");
            parameter.AppendSql("   AND GUBUN IN ('12','13')                ");

            parameter.Add("WRTNO", strWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateCallTimebyWrtNoGubun(string strWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HIC_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       CALLTIME = SYSDATE                  ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                   ");
            parameter.AppendSql("   AND GUBUN IN ('12','13')                ");
            parameter.AppendSql("   AND (GBCALL IS NULL or GBCALL = '')     ");

            parameter.Add("WRTNO", strWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_SANGDAM_WAIT> GetWrtNobyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, GBCALL                   ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO != :WRTNO                 ");
            parameter.AppendSql("   AND CALLTIME IS NOT NULL            ");
            parameter.AppendSql("   AND (GBCALL IS NULL or GBCALL = '') ");
            parameter.AppendSql("   AND GUBUN IN ('12','13')            ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HEA_SANGDAM_WAIT>(parameter);
        }

        public HEA_SANGDAM_WAIT GetGBCallbyWrtNoInGubun(long fnWRTNO, string[] strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBCALL                                  ");
            parameter.AppendSql("     , TO_CHAR(ENTTIME,'YYYY-MM-DD') ENTTIME   ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT            ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                        ");
            parameter.AppendSql("   AND GUBUN   IN (:GUBUN)                     ");
            parameter.AppendSql(" ORDER BY ENTTIME DESC                         ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.AddInStatement("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_SANGDAM_WAIT>(parameter);
        }

        public List<HEA_SANGDAM_WAIT> GetcountbyWrtNoRoom(long fnWRTNO, string dENT_ROOM)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                           ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE GUBUN = :DENT_ROOM              ");
            parameter.AppendSql("   AND GBCALL IS NULL                  ");
            parameter.AppendSql("   AND CALLTIME IS NOT NULL            ");
            parameter.AppendSql("   AND WRTNO <> :WRTNO                 ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("GUBUN", dENT_ROOM, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_SANGDAM_WAIT>(parameter);
        }

        public int Update_CallTimeGbCall(string strRowId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HEA_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       CALLTIME = SYSDATE                  ");
            parameter.AppendSql("     , GBCALL   = 'Y'                      ");
            parameter.AppendSql(" WHERE ROWID    = :RID                     ");

            parameter.Add("RID", strRowId);

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_SANGDAM_WAIT> GetItembyRoomCd(string eNDO_ROOM)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WAITNO, WRTNO, SNAME, TO_CHAR(ENTTIME,'HH24:MI') ENTTIME, ROWID RID ");
            parameter.AppendSql("     , WRTNO PTNO                                                          ");
            parameter.AppendSql("  FROM ADMIN.HEA_SANGDAM_WAIT                                        ");
            parameter.AppendSql(" WHERE ENTTIME >= TRUNC(SYSDATE)                                           ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                                                      ");
            parameter.AppendSql("   AND CALLTIME IS NULL                                                    ");
            parameter.AppendSql("   AND GBCALL IS NULL                                                      ");
            parameter.AppendSql(" ORDER BY WAITNO                                                           ");

            parameter.Add("GUBUN", eNDO_ROOM, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_SANGDAM_WAIT>(parameter);
        }

        public int UpdateSangdam_GbEndo(HEA_SANGDAM_WAIT item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HEA_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       GBENDO   = :GBENDO                  ");
            parameter.AppendSql("     , WAITNO   = :WAITNO                  ");
            parameter.AppendSql("     , GBCALL   = :GBCALL                  ");
            parameter.AppendSql("     , CALLTIME = :CALLTIME                ");
            parameter.AppendSql(" WHERE ROWID    = :RID                     ");

            parameter.Add("GBENDO", item.GBENDO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WAITNO", item.WAITNO);
            parameter.Add("GBCALL", item.GBCALL, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("CALLTIME", item.CALLTIME);
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_SANGDAM_WAIT> GetSangdamWaitInfobyEndoRoom(string eNDO_ROOM, string strSysDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME, TO_CHAR(ENTTIME,'HH24:MI:SS') EntTime, WRTNO     ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT                            ");
            parameter.AppendSql(" WHERE ENTTIME >= TRUNC(SYSDATE)                               ");
            //parameter.AppendSql(" WHERE ENTTIME >= TO_DATE(:ENTTIME, 'yyyy-MM-dd') - 0.99999    ");
            //parameter.AppendSql("   AND ENTTIME <= TO_DATE(:ENTTIME, 'yyyy-MM-dd') + 0.99999    ");
            parameter.AppendSql("   AND (GBCALL IS NULL OR GBCALL = '')                         ");
            parameter.AppendSql("   AND GUBUN = :ENDO_ROOM                                      ");
            parameter.AppendSql(" ORDER BY WAITNO                                               ");

            //parameter.Add("ENTTIME", strSysDate);
            parameter.Add("ENDO_ROOM", eNDO_ROOM, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_SANGDAM_WAIT>(parameter);
        }

        public HEA_SANGDAM_WAIT GetItembyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT AGE, WAITNO, SNAME, SEX, GJJONG     ");
            parameter.AppendSql("  FROM ADMIN.HEA_SANGDAM_WAIT        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");
            parameter.AppendSql(" ORDER BY WAITNO DESC                      ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HEA_SANGDAM_WAIT>(parameter);
        }

        public int UpdateSangdamWaitNo(long nWait, long nWRTNO, string eNDO_ROOM)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HIC_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       CALLTIME = SYSDATE                  ");
            parameter.AppendSql(" WHERE WAITNO   = :WAITNO                  ");
            if (nWRTNO != 0)
            {
                parameter.AppendSql("   AND WRTNO    = :WRTNO               ");
            }
            parameter.AppendSql("   AND GUBUN    = :GUBUN                   ");

            parameter.Add("WAITNO", nWait);
            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("GUBUN", eNDO_ROOM, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateWaitNo(long nWait, string strRowId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HEA_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       WAITNO = :WAITNO                    ");
            parameter.AppendSql(" WHERE ROWID  = :RID                       ");

            parameter.Add("WAITNO", nWait);
            parameter.Add("RID", strRowId);

            return ExecuteNonQuery(parameter);
        }

        public int InsertSangdamWait(HEA_SANGDAM_WAIT item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.HEA_SANGDAM_WAIT                               ");
            parameter.AppendSql("       (WRTNO, SNAME, SEX, AGE, GJJONG, GUBUN, ENTTIME, WAITNO)        ");
            parameter.AppendSql("VALUES                                                                 ");
            parameter.AppendSql("       (:WRTNO, :SNAME, :SEX, :AGE, :GJJONG, :GUBUN, SYSDATE, :WAITNO) ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", item.AGE);
            parameter.Add("GJJONG", item.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GUBUN", item.GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WAITNO", item.WAITNO);

            return ExecuteNonQuery(parameter);
        }

        public int InsertSangdamWait_GbEndo(HEA_SANGDAM_WAIT item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.HEA_SANGDAM_WAIT                                       ");
            parameter.AppendSql("       (WRTNO, SNAME, SEX, AGE, GJJONG, GUBUN, ENTTIME, WAITNO, GBENDO)        ");
            parameter.AppendSql("VALUES                                                                         ");
            parameter.AppendSql("       (:WRTNO, :SNAME, :SEX, :AGE, :GJJONG, :GUBUN, SYSDATE, :WAITNO, :GBENDO)");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", item.AGE);
            parameter.Add("GJJONG", item.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GUBUN", item.GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WAITNO", item.WAITNO);
            parameter.Add("GBENDO", item.GBENDO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int DeleteSangdamWait(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE ADMIN.HEA_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND GUBUN IN ('12', '13', '14')     ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int InsertSangdamHis(HEA_SANGDAM_HIS item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.HEA_SANGDAM_HIS                                            ");
            parameter.AppendSql("       (WRTNO, SNAME, SEX, AGE, GJJONG, GUBUN, ENTTIME, WAITNO, ENTGUBUN)          ");
            parameter.AppendSql("VALUES                                                                             ");
            parameter.AppendSql("       (:WRTNO, :SNAME, :SEX, :AGE, :GJJONG, :GUBUN, SYSDATE, :WAITNO, :ENTGUBUN)  ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", item.AGE);
            parameter.Add("GJJONG", item.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GUBUN", item.GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WAITNO", item.WAITNO);
            parameter.Add("ENTGUBUN", item.ENTGUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public long GetWaitNobyEndoRoom(string eNDO_ROOM, string strSysDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(WAITNO) + 1 WAIT                        ");
            parameter.AppendSql("  FROM ADMIN.HEA_SANGDAM_WAIT                ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                              ");
            parameter.AppendSql("   AND ENTTIME >= TO_DATE(:TODATE, 'YYYY-MM-DD')   ");

            parameter.Add("GUBUN", eNDO_ROOM, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("TODATE", strSysDate);

            return ExecuteScalar<long>(parameter);
        }

        public int Update_Sangdam_GbCall(long WRTNO, string[] ROOMCD)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HEA_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       GBCALL = 'Y'                        ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                     ");
            parameter.AppendSql("   AND GUBUN  IN (:GUBUN)                  ");

            parameter.Add("WRTNO", WRTNO);
            parameter.AddInStatement("GUBUN", ROOMCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_SANGDAM_WAIT> Read_Sangdam_GbCall(long WRTNO, string ROOMCD)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBCALL                          ");
            parameter.AppendSql("  FROM ADMIN.HEA_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");            
            parameter.AppendSql("   AND ENTTIME >= TRUNC(SYSDATE)       ");
            if (!ROOMCD.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GUBUN = :ROOMCD             ");
            }

            parameter.Add("WRTNO", WRTNO);
            if (!ROOMCD.IsNullOrEmpty())
            {
                parameter.Add("ROOMCD", ROOMCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HEA_SANGDAM_WAIT>(parameter);
        }

        public int Update_Sangdam_CallTime(long WRTNO, string ROOMCD)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HEA_SANGDAM_WAIT SET    ");
            parameter.AppendSql("       CALLTIME = ''                       ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                   ");
            if (ROOMCD != "")
            {
                parameter.AppendSql("   AND GUBUN    = :ROOMCD              ");
            }

            parameter.Add("WRTNO", WRTNO);
            if (ROOMCD != "")
            {
                parameter.Add("ROOMCD", ROOMCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteNonQuery(parameter);
        }

        public int Insert_Sangdam_Wait(HEA_SANGDAM_WAIT item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.HEA_SANGDAM_WAIT               ");
            parameter.AppendSql("       (WRTNO, SNAME, SEX, AGE, GJJONG, GBCALL         ");
            parameter.AppendSql("     , GUBUN, ENTTIME, CALLTIME, WAITNO)               ");
            parameter.AppendSql("VALUES                                                 ");
            parameter.AppendSql("       (:WRTNO, :SNAME, :SEX, :AGE, :GJJONG, :GBCALL   ");
            parameter.AppendSql("     , :GUBUN, SYSDATE, SYSDATE, :WAITNO)              ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", item.AGE);
            parameter.Add("GJJONG", item.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBCALL", item.GBCALL, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GUBUN", item.GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WAITNO", item.WAITNO);

            return ExecuteNonQuery(parameter);
        }

        public int Insert_Sangdam_Wait2(HEA_SANGDAM_WAIT item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.HEA_SANGDAM_WAIT               ");
            parameter.AppendSql("       (WRTNO, SNAME, SEX, AGE, GJJONG                 ");
            parameter.AppendSql("     , GUBUN, ENTTIME, CALLTIME, WAITNO)               ");
            parameter.AppendSql("VALUES                                                 ");
            parameter.AppendSql("       (:WRTNO, :SNAME, :SEX, :AGE, :GJJONG            ");
            parameter.AppendSql("     , :GUBUN, SYSDATE, SYSDATE, :WAITNO)              ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", item.AGE);
            parameter.Add("GJJONG", item.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GUBUN", item.GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WAITNO", item.WAITNO);
            return ExecuteNonQuery(parameter);
        }

        public int Insert_Sangdam_Wait3(HEA_SANGDAM_WAIT item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.HEA_SANGDAM_WAIT       ");
            parameter.AppendSql("       (WRTNO, SNAME, SEX, AGE, GJJONG         ");
            parameter.AppendSql("     , GUBUN, ENTTIME,  WAITNO)                ");
            parameter.AppendSql("VALUES                                         ");
            parameter.AppendSql("       (:WRTNO, :SNAME, :SEX, :AGE, :GJJONG    ");
            parameter.AppendSql("     , :GUBUN, SYSDATE, :WAITNO)               ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", item.AGE);
            parameter.Add("GJJONG", item.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GUBUN", item.GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WAITNO", item.WAITNO);
            return ExecuteNonQuery(parameter);
        }

        public HEA_SANGDAM_WAIT Read_Sangdam_WaitNo(string ROOMCD)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(WaitNo) + 1 WAITNO              ");
            parameter.AppendSql("  From ADMIN.HEA_SangDam_WAIT        ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                      ");
            parameter.AppendSql("   AND trunc(ENTTIME) >= trunc(SYSDATE)    ");

            parameter.Add("GUBUN", ROOMCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_SANGDAM_WAIT>(parameter);
        }

        public List<HEA_SANGDAM_WAIT> Read_Sangdam_Wait_List(string PART, string GbGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME, WRTNO                        ");
            parameter.AppendSql("  FROM ADMIN.HEA_SANGDAM_WAIT        ");
            parameter.AppendSql(" WHERE TRUNC(ENTTIME) >= TRUNC(SYSDATE)    ");
            parameter.AppendSql("   AND (GBCALL IS NULL OR GBCALL = '')     ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                      ");
            if (GbGubun == "")
            {
                parameter.AppendSql("   AND Gubun <> '12'                   "); //상담대기 제외
            }
            parameter.AppendSql(" ORDER By WaitNo                           ");

            parameter.Add("GUBUN", PART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_SANGDAM_WAIT>(parameter);
        }

        public HEA_SANGDAM_WAIT Read_Sangdam_Wait_RegList(long WRTNO, string ROOMCD)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN, GBCALL, ROWID RID            ");
            parameter.AppendSql("  From ADMIN.HEA_SangDam_WAIT        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                      ");
            parameter.AppendSql("   AND trunc(ENTTIME) >= trunc(SYSDATE)    ");

            parameter.Add("WRTNO", WRTNO);
            parameter.Add("GUBUN", ROOMCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_SANGDAM_WAIT>(parameter);
        }
        
        public HEA_SANGDAM_WAIT Read_Sangdam_EtcRoomReg(long WRTNO, string ROOMCD)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN,TO_CHAR(CALLTIME,'YYYY-MM-DD') CallTime   ");
            parameter.AppendSql("     , ROWID RID                                       ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT                    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");
            parameter.AppendSql("   AND GUBUN != :GUBUN                                 ");
            parameter.AppendSql("   AND (GBCALL IS NULL OR GBCALL = '')                 ");

            parameter.Add("WRTNO", WRTNO);
            parameter.Add("GUBUN", ROOMCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_SANGDAM_WAIT>(parameter);
        }
        
        public int Delete_Sangdam_PreData(long WRTNO, string ROOMCD)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE ADMIN.HEA_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");
            parameter.AppendSql("   AND GUBUN    != :ROOMCD             ");
            parameter.AppendSql("   AND GBCALL IS NULL                  ");

            parameter.Add("WRTNO", WRTNO);
            parameter.Add("ROOMCD", ROOMCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }
        
        public HEA_SANGDAM_WAIT Read_Sangdam_View(long WRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME,SEX,AGE,GJJONG            ");
            parameter.AppendSql("     , ROWID RID                       ");
            parameter.AppendSql("  From ADMIN.HEA_SangDam_WAIT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", WRTNO);

            return ExecuteReaderSingle<HEA_SANGDAM_WAIT>(parameter);
        }

        public HEA_SANGDAM_WAIT Read_Sangdam_View2(string GUBUN)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(WaitNo) + 1 WAITNO          ");
            //parameter.AppendSql("     , ROWID RID                       ");
            parameter.AppendSql("  From ADMIN.HEA_SangDam_WAIT    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND trunc(ENTTIME) >= trunc(SYSDATE) ");

            parameter.Add("GUBUN", GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_SANGDAM_WAIT>(parameter);
        }

        public List<HEA_SANGDAM_WAIT> Read_Sangdam_View3(string ROOMCD)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Wrtno, Sname, Gjjong            ");
            parameter.AppendSql("  From ADMIN.HEA_SangDam_WAIT    ");
            parameter.AppendSql(" WHERE GUBUN = :ROOMCD                 ");
            parameter.AppendSql("   AND Gubun <> '12'                   "); //상담대기 제외
            parameter.AppendSql("   AND (GBCALL IS NULL OR GBCALL = '') ");
            parameter.AppendSql(" ORDER By ENTTIME                      ");

            parameter.Add("ROOMCD", ROOMCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_SANGDAM_WAIT>(parameter);
        }

        public HEA_SANGDAM_WAIT Read_Sangdam_WrtNoCnt(string GUBUN)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(WRTNO) CNT                ");
            parameter.AppendSql("  From ADMIN.HEA_SANGDAM_WAIT    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND trunc(ENTTIME) >= trunc(SYSDATE)");
            parameter.AppendSql("   AND(GBCALL IS NULL OR GBCALL = '')  ");

            parameter.Add("GUBUN", GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_SANGDAM_WAIT>(parameter);
        }
    }
}
