namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicXMunjinRepository : BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicXMunjinRepository()
        {
        }

        public int Update(HIC_X_MUNJIN item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_X_MUNJIN    ");
            parameter.AppendSql("   SET JINGBN   = :JINGBN          ");
            parameter.AppendSql("     , XP1      = :XP1             ");
            parameter.AppendSql("     , XPJONG   = :XPJONG          ");
            parameter.AppendSql("     , XPLACE   = :XPLACE          ");
            parameter.AppendSql("     , XREMARK  = :XREMARK         ");
            parameter.AppendSql("     , XMUCH    = :XMUCH           ");
            parameter.AppendSql("     , XTERM    = :XTERM           ");
            parameter.AppendSql("     , XTERM1   = :XTERM1          ");
            parameter.AppendSql("     , XJUNGSAN = :XJUNGSAN        ");
            parameter.AppendSql("     , MUN1     = :MUN1            ");
            parameter.AppendSql("     , JUNGSAN1 = :JUNGSAN1        ");
            parameter.AppendSql("     , JUNGSAN2 = :JUNGSAN2        ");
            parameter.AppendSql("     , JUNGSAN3 = :JUNGSAN3        ");
            parameter.AppendSql("     , SANGDAM  = :SANGDAM         ");
            parameter.AppendSql("     , MUNDRNO  = :MUNDRNO         ");
            parameter.AppendSql(" WHERE ROWID    = :RID             ");

            parameter.Add("JINGBN", item.JINGBN);
            parameter.Add("XP1", item.XP1);
            parameter.Add("XPJONG", item.XPJONG);
            parameter.Add("XPLACE", item.XPLACE);
            parameter.Add("XREMARK", item.XREMARK);
            parameter.Add("XMUCH", item.XMUCH);
            parameter.Add("XTERM", item.XTERM);
            parameter.Add("XTERM1", item.XTERM1);
            parameter.Add("XJUNGSAN", item.XJUNGSAN);
            parameter.Add("MUN1", item.MUN1);
            parameter.Add("JUNGSAN1", item.JUNGSAN1);
            parameter.Add("JUNGSAN2", item.JUNGSAN2);
            parameter.Add("JUNGSAN3", item.JUNGSAN3);
            parameter.Add("SANGDAM", item.SANGDAM);
            parameter.Add("MUNDRNO", item.MUNDRNO);
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        internal void UpDateMunDrnoByWrtno(long nDrNO, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_X_MUNJIN    ");
            if (nDrNO > 0)
            {
                parameter.AppendSql("   SET  MUNDRNO  = :MUNDRNO         ");
            }
            else
            {
                parameter.AppendSql("   SET  MUNDRNO  = 0         ");
            }
            
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO             ");

            if (nDrNO > 0)
            {
                parameter.Add("MUNDRNO", nDrNO);
            }
            
            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public string GetJinGbnbyWrtNo(string wRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JINGBN FROM KOSMOS_PMPA.HIC_X_MUNJIN    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int Insert(string strJepDate, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_X_MUNJIN               ");
            parameter.AppendSql("       (WRTNO, JEPDATE)                            ");
            parameter.AppendSql("VALUES                                             ");
            parameter.AppendSql("       (:WRTNO, TO_DATE(:JEPDATE, 'YYYY-MM-DD'))   ");

            parameter.Add("JEPDATE", strJepDate);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateJepDatebyWrtNo(string strJepDate, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_X_MUNJIN SET                ");
            parameter.AppendSql("       JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                            ");

            parameter.Add("JEPDATE", strJepDate);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X')CNT FROM KOSMOS_PMPA.HIC_X_MUNJIN     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdatebyWrtNo(HIC_X_MUNJIN item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_X_MUNJIN SET    ");
            parameter.AppendSql("       JINGBN   = :JINGBN              ");
            parameter.AppendSql("     , XP1      = :XP1                 ");
            parameter.AppendSql("     , XPJONG   = :XPJONG              ");
            parameter.AppendSql("     , XPLACE   = :XPLACE              ");
            parameter.AppendSql("     , XREMARK  = :XREMARK             ");
            parameter.AppendSql("     , XMUCH    = :XMUCH               ");
            parameter.AppendSql("     , XTERM    = :XTERM               ");
            parameter.AppendSql("     , XTERM1   = :XTERM1              ");
            parameter.AppendSql("     , XJUNGSAN = :XJUNGSAN            ");
            parameter.AppendSql("     , MUN1     = :MUN1                ");
            parameter.AppendSql("     , JUNGSAN1 = :JUNGSAN1            ");
            parameter.AppendSql("     , JUNGSAN2 = :JUNGSAN2            ");
            parameter.AppendSql("     , JUNGSAN3 = :JUNGSAN3            ");
            parameter.AppendSql("     , SANGDAM  = :SANGDAM             ");
            parameter.AppendSql("     , MUNDRNO  = :MUNDRNO             ");
            parameter.AppendSql("     , JILBYUNG = :JILBYUNG            ");
            parameter.AppendSql("     , BLOOD1   = :BLOOD1              ");
            parameter.AppendSql("     , BLOOD2   = :BLOOD2              ");
            parameter.AppendSql("     , BLOOD3   = :BLOOD3              ");
            parameter.AppendSql("     , SKIN1    = :SKIN1               ");
            parameter.AppendSql("     , SKIN2    = :SKIN2               ");
            parameter.AppendSql("     , SKIN3    = :SKIN3               ");
            parameter.AppendSql("     , NERVOUS1 = :NERVOUS1            ");
            parameter.AppendSql("     , EYE1     = :EYE1                ");
            parameter.AppendSql("     , EYE2     = :EYE2                ");
            parameter.AppendSql("     , CANCER1  = :CANCER1             ");
            parameter.AppendSql("     , GAJOK    = :GAJOK               ");
            parameter.AppendSql("     , BLOOD    = :BLOOD               ");
            parameter.AppendSql("     , NERVOUS2 = :NERVOUS2            ");
            parameter.AppendSql("     , CANCER2  = :CANCER2             ");
            parameter.AppendSql("     , SYMPTON  = :SYMPTON             ");
            parameter.AppendSql("     , JIKJONG1 = :JIKJONG1            ");
            parameter.AppendSql("     , JIKJONG2 = :JIKJONG2            ");
            parameter.AppendSql("     , JIKJONG3 = :JIKJONG3            ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");

            parameter.Add("JINGBN", item.JINGBN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("XP1", item.XP1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("XPJONG", item.XPJONG);
            parameter.Add("XPLACE", item.XPLACE);
            parameter.Add("XREMARK", item.XREMARK);
            parameter.Add("XMUCH", item.XMUCH);
            parameter.Add("XTERM", item.XTERM);
            parameter.Add("XTERM1", item.XTERM1);
            parameter.Add("XJUNGSAN", item.XJUNGSAN);
            parameter.Add("MUN1", item.MUN1);
            parameter.Add("JUNGSAN1", item.JUNGSAN1);
            parameter.Add("JUNGSAN2", item.JUNGSAN2);
            parameter.Add("JUNGSAN3", item.JUNGSAN3);
            parameter.Add("SANGDAM", item.SANGDAM);
            parameter.Add("MUNDRNO", item.MUNDRNO);
            parameter.Add("JILBYUNG", item.JILBYUNG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BLOOD1", item.BLOOD1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BLOOD2", item.BLOOD2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BLOOD3", item.BLOOD3);
            parameter.Add("SKIN1", item.SKIN1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SKIN2", item.SKIN2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SKIN3", item.SKIN3);
            parameter.Add("NERVOUS1", item.NERVOUS1);
            parameter.Add("EYE1", item.EYE1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EYE2", item.EYE2);
            parameter.Add("CANCER1", item.CANCER1);
            parameter.Add("GAJOK", item.GAJOK, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BLOOD", item.BLOOD);
            parameter.Add("NERVOUS2", item.NERVOUS2);
            parameter.Add("CANCER2", item.CANCER2);
            parameter.Add("SYMPTON", item.SYMPTON);
            parameter.Add("JIKJONG1", item.JIKJONG1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JIKJONG2", item.JIKJONG2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JIKJONG3", item.JIKJONG3);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public HIC_X_MUNJIN GetItembyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,JinGbn, XP1,XPJONG,XPLACE,XREMARK,XTERM,XTERM1,XMUCH,XJUNGSAN ");
            parameter.AppendSql("     , MUN1,JUNGSAN1, MunDrNo                                              ");
            parameter.AppendSql("     , TO_CHAR(JepDate,'YYYY-MM-DD') JepDate                               ");
            parameter.AppendSql("     , TO_CHAR(PanjengDate,'YYYY-MM-DD') PanjengDate, PANJENG, PanjengDrno ");
            parameter.AppendSql("     , JUNGSAN2 , JUNGSAN3, STS ,Pan, Sogen, Jochi,WorkYN,SahuCode,SangDam ");
            parameter.AppendSql("     , JILBYUNG,BLOOD1,BLOOD2,BLOOD3,SKIN1,SKIN2,SKIN3,NERVOUS1,EYE1,EYE2  ");
            parameter.AppendSql("     , CANCER1, GAJOK, BLOOD , NERVOUS2, CANCER2, SYMPTON, JIKJONG1        ");
            parameter.AppendSql("     , JIKJONG2, JIKJONG3, GBPRINT                                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_X_MUNJIN                                            ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                      ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReaderSingle<HIC_X_MUNJIN>(parameter);
        }

        public HIC_X_MUNJIN GetMunDrNobyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MunDrno, ROWID FROM KOSMOS_PMPA.HIC_X_MUNJIN        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                      ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_X_MUNJIN>(parameter);
        }

        public int SaveXMunjin(long nWrtNo, string strJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("MERGE INTO KOSMOS_PMPA.HIC_X_MUNJIN a                  ");
            parameter.AppendSql("using dual d                                           ");
            parameter.AppendSql("   on (a.WRTNO     = :WRTNO)                           ");
            parameter.AppendSql(" when matched then                                     ");
            parameter.AppendSql("  update set                                           ");
            parameter.AppendSql("         JEPDATE   = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql(" when not matched then                                 ");
            parameter.AppendSql("  insert                                               ");
            parameter.AppendSql("         (WRTNO                                        ");
            parameter.AppendSql("        , JEPDATE)                                     ");
            parameter.AppendSql(" VALUES                                                ");
            parameter.AppendSql("         (:WRTNO                                       ");
            parameter.AppendSql("        , TO_DATE(:JEPDATE, 'YYYY-MM-DD'))             ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("GUNDATE", strJepDate);

            return ExecuteNonQuery(parameter);
        }


        public int UpdatePrtinfobyWrtNo(string argSabun, string argTongboDate, long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_X_MUNJIN SET                        ");
            parameter.AppendSql("       GBPRINT = 'Y'                                       ");
            parameter.AppendSql("       , PRTSABUN = :SABUN                                 ");
            parameter.AppendSql("       , TONGBODATE = TO_DATE(:TONGBODATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                                    ");

            parameter.Add("TONGBODATE", argTongboDate);
            parameter.Add("SABUN", argSabun);
            parameter.Add("WRTNO", argWrtno);

            return ExecuteNonQuery(parameter);
        }
    }
}
