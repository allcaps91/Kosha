namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicWaitRoomRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicWaitRoomRepository()
        {
        }

        public string GetRoomNamebyRoom(string strRoom)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RoomName                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_WAIT_ROOM   ");
            parameter.AppendSql(" WHERE ROOM = :ROOM                ");

            parameter.Add("ROOM", strRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }
        public HIC_WAIT_ROOM GetItemByRoom(string strRoom)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROOM, ROOMNAME              ");
            parameter.AppendSql(" ,AMUSE, PMUSE, AMSANG, PMSANG     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_WAIT_ROOM   ");
            parameter.AppendSql(" WHERE ROOM = :ROOM                ");

            parameter.Add("ROOM", strRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_WAIT_ROOM>(parameter);
        }

        public List<HIC_WAIT_ROOM> GetRoombyRoomCode(List<string> strTemp, string strSysTime)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Room, RoomName, AmSang, PmSang  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_WAIT_ROOM       ");
            parameter.AppendSql(" WHERE Room IN (:ROOM)                 ");
            if (string.Compare(strSysTime, "13:00") < 0)
            {
                parameter.AppendSql("   AND AmUse = 'Y'                 ");
            }
            else
            {
                parameter.AppendSql("   AND PmUse = 'Y'                 ");
            }

            parameter.AddInStatement("ROOM", strTemp, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_WAIT_ROOM>(parameter);
        }

        public HIC_WAIT_ROOM GetRoomRoomNamebyRoom(string strNextRoom)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROOM, ROOMNAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_WAIT_ROOM   ");
            parameter.AppendSql(" WHERE ROOM = :ROOM                ");

            parameter.Add("ROOM", strNextRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_WAIT_ROOM>(parameter);
        }

        public string GetRoomRoomNameByRoomCd(string strCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RoomName FROM KOSMOS_PMPA.HIC_WAIT_ROOM             ");
            parameter.AppendSql(" WHERE ROOM = :ROOMCD                                      ");

            parameter.Add("ROOMCD", strCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdatebyRoom(HIC_WAIT_ROOM item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_WAIT_ROOM SET       ");
            parameter.AppendSql("       AMUSE     = :AMUSE                  ");
            parameter.AppendSql("     , PMUSE     = :PMUSE                  ");
            parameter.AppendSql("     , AmSang    = :AMSANG                 ");
            parameter.AppendSql("     , PmSang    = :PMSANG                 ");
            parameter.AppendSql(" WHERE ROOM      = :ROOM                   ");

            parameter.Add("ROOM", item.ROOM);
            parameter.Add("AMUSE", item.AMUSE);
            parameter.Add("PMUSE", item.PMUSE);
            parameter.Add("AMSANG", item.AMSANG);
            parameter.Add("PMSANG", item.PMSANG);

            return ExecuteNonQuery(parameter);
        }

        public int Update(HIC_WAIT_ROOM item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_WAIT_ROOM SET       ");
            parameter.AppendSql("       ROOM      = :ROOM                   ");  
            parameter.AppendSql("     , ROOMNAME  = :ROOMNAME               ");
            parameter.AppendSql("     , AMUSE     = :AMUSE                  ");
            parameter.AppendSql("     , PMUSE     = :PMUSE                  ");
            parameter.AppendSql("     , AMSANG    = :AMSANG                 ");
            parameter.AppendSql("     , PMSANG    = :PMSANG                 ");
            parameter.AppendSql(" WHERE ROWID     = :RID                    ");

            parameter.Add("ROOM", item.ROOM);
            parameter.Add("ROOMNAME", item.ROOMNAME);
            parameter.Add("AMUSE", item.AMUSE);
            parameter.Add("PMUSE", item.PMUSE);
            parameter.Add("AMSANG", item.AMSANG);
            parameter.Add("PMSANG", item.PMSANG);
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public int InsertAll(HIC_WAIT_ROOM item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_WAIT_ROOM      ");
            parameter.AppendSql("       (ROOM, ROOMNAME, AMUSE, PMUSE)      ");
            parameter.AppendSql("VALUES                                     ");
            parameter.AppendSql("       (:ROOM, :ROOMNAME, :AMUSE, :PMUSE)  ");

            parameter.Add("ROOM", item.ROOM);
            parameter.Add("ROOMNAME", item.ROOMNAME);
            parameter.Add("AMUSE", item.AMUSE);
            parameter.Add("PMUSE", item.PMUSE);

            return ExecuteNonQuery(parameter);
        }

        public string GetCountbyRoomCd(string strRoom)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID RID                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_WAIT_ROOM   ");
            parameter.AppendSql(" WHERE ROOM = :ROOM                ");

            parameter.Add("ROOM", strRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_WAIT_ROOM> GetAll()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROOM, ROOMNAME, AMUSE, PMUSE, AMSANG, PMSANG    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_WAIT_ROOM                       ");
            parameter.AppendSql("  WHERE ROOM NOT IN ('00')                             ");
            parameter.AppendSql(" ORDER BY Room                                         ");

            return ExecuteReader<HIC_WAIT_ROOM>(parameter);
        }
    }
}
