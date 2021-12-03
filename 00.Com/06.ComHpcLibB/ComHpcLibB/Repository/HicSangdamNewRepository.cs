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
    public class HicSangdamNewRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSangdamNewRepository()
        {
        }

        public HIC_SANGDAM_NEW SelOneData(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,GJJONG,GJCHASU,GBSTS,PANO,JEPDATE,HABIT1,HABIT2,HABIT3,HABIT4                             ");
            parameter.AppendSql("      ,HABIT5, JINCHAL1, JINCHAL2, GBSIKSA, T_STAT01, T_STAT02, T_STAT11, T_STAT12, T_STAT21           ");
            parameter.AppendSql("      ,T_STAT22, T_STAT31, T_STAT32, T_STAT41, T_STAT42, T_STAT51, T_STAT52, T_STAT52_TEC              ");
            parameter.AppendSql("      ,MUN_GAJOK, MUN_GIINSUNG, JENGSANG, JIN_01, JIN_02, JIN_03, JIN_04, REMARK, PTNO, SANGDAMDRNO    ");
            parameter.AppendSql("      ,ENTSABUN, ENTTIME, T_STAT61, T_STAT62, DIABETES_1, DIABETES_2, CYCLE_1, CYCLE_2, SCHPAN1        ");
            parameter.AppendSql("      ,SCHPAN2, SCHPAN3, SCHPAN4, SCHPAN5, SCHPAN6, SCHPAN7, SCHPAN8, SCHPAN9, SCHPAN10, SCHPAN11      ");
            parameter.AppendSql("      ,T_STAT71, T_STAT72, AMSANGDAM, MUN_OLDMSYM, PJSANGDAM, GBCHK                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_NEW                                                                     ");
            parameter.AppendSql("  WHERE 1 = 1                                                                                          ");
            parameter.AppendSql("    AND WRTNO  =:WRTNO                                                                                 ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReaderSingle<HIC_SANGDAM_NEW>(parameter);
        }

        public int UpdatebyWrtNo(HIC_SANGDAM_NEW item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_NEW SET ");
            parameter.AppendSql("       HABIT1       = :HABIT1          ");
            parameter.AppendSql("     , HABIT2       = :HABIT2          ");
            parameter.AppendSql("     , HABIT3       = :HABIT3          ");
            parameter.AppendSql("     , HABIT4       = :HABIT4          ");
            parameter.AppendSql("     , HABIT5       = :HABIT5          ");
            parameter.AppendSql("     , JINCHAL1     = :JINCHAL1        ");
            parameter.AppendSql("     , JINCHAL2     = :JINCHAL2        ");
            parameter.AppendSql("     , T_STAT01     = :T_STAT01        ");
            parameter.AppendSql("     , T_STAT02     = :T_STAT02        ");
            parameter.AppendSql("     , T_STAT11     = :T_STAT11        ");
            parameter.AppendSql("     , T_STAT12     = :T_STAT12        ");
            parameter.AppendSql("     , T_STAT21     = :T_STAT21        ");
            parameter.AppendSql("     , T_STAT22     = :T_STAT22        ");
            parameter.AppendSql("     , T_STAT31     = :T_STAT31        ");
            parameter.AppendSql("     , T_STAT32     = :T_STAT32        ");
            parameter.AppendSql("     , T_STAT41     = :T_STAT41        ");
            parameter.AppendSql("     , T_STAT42     = :T_STAT42        ");
            parameter.AppendSql("     , T_STAT51     = :T_STAT51        ");
            parameter.AppendSql("     , T_STAT52     = :T_STAT52        ");
            if (!item.T_STAT52_TEC.IsNullOrEmpty())
            {
                parameter.AppendSql("     , T_STAT52_TEC = :T_STAT52_TEC");
            }
            parameter.AppendSql("     , T_STAT61     = :T_STAT61        ");
            parameter.AppendSql("     , T_STAT62     = :T_STAT62        ");
            parameter.AppendSql("     , T_STAT71     = :T_STAT71        ");
            parameter.AppendSql("     , T_STAT72     = :T_STAT72        ");
            parameter.AppendSql("     , GBSIKSA      = :GBSIKSA         ");
            parameter.AppendSql("     , MUN_OLDMSYM  = :MUN_OLDMSYM     ");
            parameter.AppendSql("     , MUN_GAJOK    = :MUN_GAJOK       ");
            parameter.AppendSql("     , MUN_GIINSUNG = :MUN_GIINSUNG    ");
            parameter.AppendSql("     , JIN_01       = :JIN_01          ");
            parameter.AppendSql("     , JIN_02       = :JIN_02          ");
            parameter.AppendSql("     , JIN_03       = :JIN_03          ");
            parameter.AppendSql("     , JIN_04       = :JIN_04          ");
            parameter.AppendSql("     , JENGSANG     = :JENGSANG        ");
            parameter.AppendSql("     , PJSANGDAM    = :PJSANGDAM       ");
            parameter.AppendSql("     , REMARK       = :REMARK          ");
            parameter.AppendSql("     , SANGDAMDRNO  = :SANGDAMDRNO     ");
            parameter.AppendSql("     , GBSTS        = :GBSTS           ");
            parameter.AppendSql("     , ENTSABUN     = :ENTSABUN        ");
            parameter.AppendSql("     , ENTTIME      = SYSDATE          ");
            parameter.AppendSql(" WHERE WRTNO        = :WRTNO           ");

            parameter.Add("HABIT1", item.HABIT1);
            parameter.Add("HABIT2", item.HABIT2);
            parameter.Add("HABIT3", item.HABIT3);
            parameter.Add("HABIT4", item.HABIT4);
            parameter.Add("HABIT5", item.HABIT5);
            parameter.Add("JINCHAL1", item.JINCHAL1);
            parameter.Add("JINCHAL2", item.JINCHAL2);
            parameter.Add("T_STAT01", item.T_STAT01);
            parameter.Add("T_STAT02", item.T_STAT02);
            parameter.Add("T_STAT11", item.T_STAT11);
            parameter.Add("T_STAT12", item.T_STAT12);
            parameter.Add("T_STAT21", item.T_STAT21);
            parameter.Add("T_STAT22", item.T_STAT22);
            parameter.Add("T_STAT31", item.T_STAT31);
            parameter.Add("T_STAT32", item.T_STAT32);
            parameter.Add("T_STAT41", item.T_STAT41);
            parameter.Add("T_STAT42", item.T_STAT42);
            parameter.Add("T_STAT51", item.T_STAT51);
            parameter.Add("T_STAT52", item.T_STAT52);
            if (!item.T_STAT52_TEC.IsNullOrEmpty())
            {
                parameter.Add("T_STAT52_TEC", item.T_STAT52_TEC);
            }
            parameter.Add("T_STAT61", item.T_STAT61);
            parameter.Add("T_STAT62", item.T_STAT62);
            parameter.Add("T_STAT71", item.T_STAT71);
            parameter.Add("T_STAT72", item.T_STAT72);
            parameter.Add("GBSIKSA", item.GBSIKSA);
            parameter.Add("MUN_OLDMSYM", item.MUN_OLDMSYM);
            parameter.Add("MUN_GAJOK", item.MUN_GAJOK);
            parameter.Add("MUN_GIINSUNG", item.MUN_GIINSUNG);
            parameter.Add("JIN_01", item.JIN_01);
            parameter.Add("JIN_02", item.JIN_02);
            parameter.Add("JIN_03", item.JIN_03);
            parameter.Add("JIN_04", item.JIN_04);
            parameter.Add("JENGSANG", item.JENGSANG);
            parameter.Add("PJSANGDAM", item.PJSANGDAM);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("SANGDAMDRNO", item.SANGDAMDRNO);
            parameter.Add("GBSTS", item.GBSTS);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyWrtNoSangdamDrNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_NEW     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND SANGDAMDRNO > 0                 ");    

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<int>(parameter);
        }

        public void UpDateSangdamDrno(long nDrno, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_NEW SET         ");
            if (nDrno > 0)
            {
                parameter.AppendSql("       SANGDAMDRNO = :SANGDAMDRNO          ");
            }
            else
            {
                parameter.AppendSql("       SANGDAMDRNO = 0                     ");
            }
            
            parameter.AppendSql(" WHERE WRTNO =:WRTNO         ");

            if (nDrno > 0)
            {
                parameter.Add("SANGDAMDRNO", nDrno);
            }
            
            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public HIC_SANGDAM_NEW GetPjSangdambyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PJSANGDAM                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_NEW                 ");
            parameter.AppendSql("  WHERE  WRTNO  =:WRTNO                            ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_SANGDAM_NEW>(parameter);
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_NEW SET         ");
            parameter.AppendSql("       PANO = :PANO                            ");
            parameter.AppendSql(" WHERE PANO IN (SELECT PANO                    ");
            parameter.AppendSql("                  FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql("                 WHERE JUMIN2 = :JUMIN2        ");
            parameter.AppendSql("                   AND PANO <> :PANO)          ");

            parameter.Add("PANO", argPaNo);
            parameter.Add("JUMIN2", argJumin2);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateWrtNoGjJongbyWrtNo(long nWrtNo, string strJong, long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_NEW SET     ");
            parameter.AppendSql("       WRTNO = :WRTNO                      ");
            parameter.AppendSql("     , GJJONG = :GJJONG                    ");
            parameter.AppendSql(" WHERE WRTNO  = :FWRTNO                    ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("FWRTNO", fnWrtNo);
            parameter.Add("GJJONG", strJong);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateHabitGbChkbyWrtNo(HIC_SANGDAM_NEW item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_NEW SET     ");
            parameter.AppendSql("       HABIT1 = :HABIT1                    ");
            parameter.AppendSql("     , HABIT2 = :HABIT2                    ");
            parameter.AppendSql("     , HABIT3 = :HABIT3                    ");
            parameter.AppendSql("     , HABIT4 = :HABIT4                    ");
            parameter.AppendSql("     , GBCHK  = :GBCHK                     ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                     ");

            parameter.Add("HABIT1", item.HABIT1);
            parameter.Add("HABIT2", item.HABIT2);
            parameter.Add("HABIT3", item.HABIT3);
            parameter.Add("HABIT4", item.HABIT4);
            parameter.Add("GBCHK", item.GBCHK);
            parameter.Add("WRTNO",  item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_NEW     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<int>(parameter);
        }

        public long GetSangdamDrNobyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SANGDAMDRNO                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_NEW     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<long>(parameter);
        }

        public HIC_SANGDAM_NEW GetSangdamDrNoAmSangdambyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SANGDAMDRNO, AMSANGDAM, GBSTS   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_NEW     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_SANGDAM_NEW>(parameter);
        }

        public HIC_SANGDAM_NEW GetAllbyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, GJJONG, GJCHASU, GBSTS, PANO, JEPDATE, HABIT1, HABIT2, HABIT3, HABIT4    ");
            parameter.AppendSql("     , HABIT5, JINCHAL1, JINCHAL2, GBSIKSA, T_STAT01, T_STAT02, T_STAT11, T_STAT12     ");
            parameter.AppendSql("     , T_STAT21, T_STAT22, T_STAT31, T_STAT32, T_STAT41, T_STAT42, T_STAT51, T_STAT52  ");
            parameter.AppendSql("     , T_STAT52_TEC, MUN_GAJOK, MUN_GIINSUNG, JENGSANG, JIN_01, JIN_02, JIN_03, JIN_04 ");
            parameter.AppendSql("     , REMARK, PTNO, SANGDAMDRNO, ENTSABUN, ENTTIME, T_STAT61, T_STAT62, DIABETES_1    ");
            parameter.AppendSql("     , DIABETES_2, CYCLE_1, CYCLE_2, SCHPAN1, SCHPAN2, SCHPAN3, SCHPAN4, SCHPAN5       ");
            parameter.AppendSql("     , SCHPAN6, SCHPAN7, SCHPAN8, SCHPAN9, SCHPAN10, SCHPAN11, T_STAT71, T_STAT72      ");
            parameter.AppendSql("     , AMSANGDAM, MUN_OLDMSYM, PJSANGDAM, GBCHK                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_NEW                                                     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                  ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReaderSingle<HIC_SANGDAM_NEW>(parameter);
        }

        public HIC_SANGDAM_NEW GetItembyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBSTS,REMARK,PJSANGDAM    ");
            parameter.AppendSql("     , HABIT1, HABIT2, HABIT3, HABIT4, ROWID RID      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_NEW         ");
            parameter.AppendSql(" WHERE WRTNO  =:WRTNO                      ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_SANGDAM_NEW>(parameter);
        }

        public int UpdateRemarkbyWrtNo(HIC_SANGDAM_NEW item2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_NEW SET     ");
            parameter.AppendSql("       REMARK      = :REMARK               ");
            parameter.AppendSql("     , SANGDAMDRNO = :SANGDAMDRNO          ");
            parameter.AppendSql("     , GBSTS       = :GBSTS                ");
            parameter.AppendSql("     , ENTSABUN    = :ENTSABUN             ");
            parameter.AppendSql("     , ENTTIME     = SYSDATE               ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO                ");

            parameter.Add("REMARK", item2.REMARK);
            parameter.Add("SANGDAMDRNO", item2.SANGDAMDRNO);
            parameter.Add("GBSTS", item2.GBSTS, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", item2.ENTSABUN);
            parameter.Add("WRTNO", item2.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatSchPanbyWrtNo(HIC_SANGDAM_NEW item2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_NEW SET     ");
            parameter.AppendSql("       SCHPAN1     = :SCHPAN1              ");
            parameter.AppendSql("     , SCHPAN2     = :SCHPAN2              ");
            parameter.AppendSql("     , SCHPAN3     = :SCHPAN3              ");
            parameter.AppendSql("     , SCHPAN4     = :SCHPAN4              ");
            parameter.AppendSql("     , SCHPAN5     = :SCHPAN5              ");
            parameter.AppendSql("     , SCHPAN6     = :SCHPAN6              ");
            parameter.AppendSql("     , SCHPAN7     = :SCHPAN7              ");
            parameter.AppendSql("     , SCHPAN8     = :SCHPAN8              ");
            parameter.AppendSql("     , SCHPAN9     = :SCHPAN9              ");
            parameter.AppendSql("     , SCHPAN10    = :SCHPAN10             ");
            parameter.AppendSql("     , SCHPAN11    = :SCHPAN11             ");
            parameter.AppendSql("     , REMARK      = :REMARK               ");
            parameter.AppendSql("     , GBSTS       = :GBSTS                ");
            parameter.AppendSql("     , SANGDAMDRNO = :SANGDAMDRNO          ");
            parameter.AppendSql("     , ENTSABUN    = :ENTSABUN             ");
            parameter.AppendSql("     , ENTTIME     = SYSDATE               ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO                ");

            parameter.Add("SCHPAN1", item2.SCHPAN1);
            parameter.Add("SCHPAN2", item2.SCHPAN2);
            parameter.Add("SCHPAN3", item2.SCHPAN3);
            parameter.Add("SCHPAN4", item2.SCHPAN4);
            parameter.Add("SCHPAN5", item2.SCHPAN5);
            parameter.Add("SCHPAN6", item2.SCHPAN6);
            parameter.Add("SCHPAN7", item2.SCHPAN7);
            parameter.Add("SCHPAN8", item2.SCHPAN8);
            parameter.Add("SCHPAN9", item2.SCHPAN9);
            parameter.Add("SCHPAN10", item2.SCHPAN10);
            parameter.Add("SCHPAN11", item2.SCHPAN11);
            parameter.Add("REMARK", item2.REMARK);
            parameter.Add("GBSTS", item2.GBSTS);
            parameter.Add("SANGDAMDRNO", item2.SANGDAMDRNO);
            parameter.Add("ENTSABUN", item2.ENTSABUN);
            parameter.Add("WRTNO", item2.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateJinchal2byWrtNo(HIC_SANGDAM_NEW item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_NEW SET     ");
            parameter.AppendSql("       JINCHAL2    = :JINCHAL2             ");
            parameter.AppendSql("     , GBSIKSA     = :GBSIKSA              ");
            parameter.AppendSql("     , AMSANGDAM   = :AMSANGDAM            ");
            parameter.AppendSql("     , REMARK      = :REMARK               ");
            parameter.AppendSql("     , SANGDAMDRNO = :SANGDAMDRNO          ");
            parameter.AppendSql("     , GBSTS       = :GBSTS                ");
            parameter.AppendSql("     , ENTSABUN    = :ENTSABUN             ");
            parameter.AppendSql("     , ENTTIME     = SYSDATE               ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO                ");

            parameter.Add("JINCHAL2",    item.JINCHAL2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSIKSA",     item.GBSIKSA, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AMSANGDAM",   item.AMSANGDAM);
            parameter.Add("REMARK",      item.REMARK);
            parameter.Add("SANGDAMDRNO", item.SANGDAMDRNO);
            parameter.Add("GBSTS",       item.GBSTS, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN",    item.ENTSABUN);
            parameter.Add("WRTNO",       item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateDiabetesbyWrtNo(HIC_SANGDAM_NEW item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_NEW SET     ");
            parameter.AppendSql("       DIABETES_1  = :DIABETES_1           ");
            parameter.AppendSql("     , DIABETES_2  = :DIABETES_2           ");
            parameter.AppendSql("     , CYCLE_1     = :CYCLE_1              ");
            parameter.AppendSql("     , CYCLE_2     = :CYCLE_2              ");
            parameter.AppendSql("     , GBSIKSA     = :GBSIKSA              ");
            parameter.AppendSql("     , REMARK      = :REMARK               ");
            parameter.AppendSql("     , GBSTS       = :GBSTS                ");
            parameter.AppendSql("     , SANGDAMDRNO = :SANGDAMDRNO          ");
            parameter.AppendSql("     , ENTSABUN    = :ENTSABUN             ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO                ");

            parameter.Add("DIABETES_1", item.DIABETES_1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DIABETES_2", item.DIABETES_2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("CYCLE_1", item.CYCLE_1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("CYCLE_2", item.CYCLE_2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSIKSA", item.GBSIKSA, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("GBSTS", item.GBSTS, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SANGDAMDRNO", item.SANGDAMDRNO);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_SANGDAM_NEW item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SANGDAM_NEW                            ");
            parameter.AppendSql("       (WRTNO,GJJONG,GJCHASU,JEPDATE,PANO,PTNO,GBSTS)              ");
            parameter.AppendSql("VALUES                                                             ");
            parameter.AppendSql("       (:WRTNO,:GJJONG,:GJCHASU,TO_DATE(:JEPDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("       , :PANO,:PTNO,:GBSTS)                                       ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("GJJONG", item.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJCHASU", item.GJCHASU, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", item.JEPDATE);
            parameter.Add("PANO", item.PANO);
            parameter.Add("PTNO", item.PTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSTS", item.GBSTS, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public HIC_SANGDAM_NEW GetGbStsRowIdbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBSTS, ROWID RID                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_NEW             ");
            parameter.AppendSql(" WHERE WRTNO  =:WRTNO                          ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_SANGDAM_NEW>(parameter);
        }

        public string GetRemarkbyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT REMARK  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_NEW             ");
            parameter.AppendSql(" WHERE WRTNO  =:WRTNO                          ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_SANGDAM_NEW GetHabitbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT HABIT1, HABIT2, HABIT3, HABIT4, HABIT5  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_NEW             ");
            parameter.AppendSql(" WHERE WRTNO  =:WRTNO                          ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_SANGDAM_NEW>(parameter);
        }

        public HIC_SANGDAM_NEW GetGbstsRowIdbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBSTS,ROWID                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_NEW ");
            parameter.AppendSql(" WHERE WRTNO  =:WRTNO              ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_SANGDAM_NEW>(parameter);
        }

        public int UpdateHabitbyWrtNo(HIC_RES_BOHUM1 item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SANGDAM_NEW SET ");
            parameter.AppendSql("       HABIT1   = :HABIT1              ");
            parameter.AppendSql("     , HABIT2   = :HABIT2              ");
            parameter.AppendSql("     , HABIT3   = :HABIT3              ");
            parameter.AppendSql("     , HABIT4   = :HABIT4              ");
            parameter.AppendSql("     , HABIT5   = :HABIT5              ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN            ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE              ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");

            parameter.Add("HABIT1", item.HABIT1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT2", item.HABIT2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT3", item.HABIT3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT4", item.HABIT4, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT5", item.HABIT5, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public string GetRowIdbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID RID                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_NEW     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_SANGDAM_NEW> GetPanoCntbyDrNo(string strDrNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Pano,COUNT(*) CNT               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SANGDAM_NEW     ");
            parameter.AppendSql(" WHERE JEPDATE = TRUNC(SYSDATE)        ");
            parameter.AppendSql("   AND SANGDAMDRNO = :DRNO             ");
            parameter.AppendSql(" GROUP BY PANO                         ");

            parameter.Add("DRNO", strDrNo);

            return ExecuteReader<HIC_SANGDAM_NEW>(parameter);
        }
    }
}
