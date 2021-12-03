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
    public class HicCancerResv2Repository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicCancerResv2Repository()
        {
        }

        public List<HIC_CANCER_RESV2> GetItembyRTime(string strFDate, string strTDate, string strJong, string strName)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(RTime,'MM/DD') RTime, Jumin2, SName, Tel, Hphone, GbAm1, GbAm2      ");
            parameter.AppendSql("     , GbAm3, GbAm4, GbUgi, GbGfs, GbMammo, GbRecutm, GbSono,GbWomb, GBCT, REMARK  ");
            parameter.AppendSql("     , TO_CHAR(JepDate,'YYYY-MM-DD') JepDate, EntSabun, EntTime, ROWID             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV2                                                ");
            parameter.AppendSql(" WHERE RTime >= TO_DATE(:FRDATE, 'YYYY-MM-DD HH24:MI')                             ");
            parameter.AppendSql("   AND RTime <= TO_DATE(:TODATE, 'YYYY-MM-DD HH24:MI')                             ");
            if (strJong == "1")
            {
                parameter.AppendSql("   AND GbUgi = 'Y'                                                             ");
            }                          
            else if (strJong == "2")   
            {                          
                parameter.AppendSql("   AND GbGfs = 'Y'                                                             ");
            }                          
            else if (strJong == "3")   
            {                          
                parameter.AppendSql("   AND GbMammo = 'Y'                                                           ");
            }                          
            else if (strJong == "4")   
            {                          
                parameter.AppendSql("   AND GbRecutm = 'Y'                                                          ");
            }                          
            else if (strJong == "5")   
            {                          
                parameter.AppendSql("   AND GbSono = 'Y'                                                            ");
            }                          
            else if (strJong == "6")   
            {                          
                parameter.AppendSql("   AND GbWomb = 'Y'                                                            ");
            }
            else if (strJong == "7")
            {
                parameter.AppendSql("   AND GBCT = 'Y'                                                              ");
            }
            if (!strName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SNAME like :SNAME                                                       ");
            }
            parameter.AppendSql(" ORDER BY RTime                                                                    ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            if (!strName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strName);
            }

            return ExecuteReader<HIC_CANCER_RESV2>(parameter);
        }

        public int Update(HIC_CANCER_RESV2 item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CANCER_RESV2 SET                        ");
            //parameter.AppendSql("       RTIME           = TO_DATE(:RTIME, 'YYYY-MM-DD HH24:MI') ");
            parameter.AppendSql("       RTIME           = :RTIME                                ");
            parameter.AppendSql("     , JUMIN           = :JUMIN                                ");
            parameter.AppendSql("     , JUMIN2          = :JUMIN2                               ");
            parameter.AppendSql("     , SNAME           = :SNAME                                ");
            parameter.AppendSql("     , TEL             = :TEL                                  ");
            parameter.AppendSql("     , HPHONE          = :HPHONE                               ");
            parameter.AppendSql("     , GBAM1           = :GBAM1                                ");
            parameter.AppendSql("     , GBAM2           = :GBAM2                                ");
            parameter.AppendSql("     , GBAM3           = :GBAM3                                ");
            parameter.AppendSql("     , GBAM4           = :GBAM4                                ");
            parameter.AppendSql("     , GBUGI           = :GBUGI                                ");
            parameter.AppendSql("     , GBGFS           = :GBGFS                                ");
            parameter.AppendSql("     , GBGFSH          = :GBGFSH                               ");
            parameter.AppendSql("     , GBMAMMO         = :GBMAMMO                              ");
            parameter.AppendSql("     , GBRECUTM        = :GBRECUTM                             ");
            parameter.AppendSql("     , GBSONO          = :GBSONO                               ");
            parameter.AppendSql("     , GBWOMB          = :GBWOMB                               ");
            parameter.AppendSql("     , GBBOHUM         = :GBBOHUM                              ");
            parameter.AppendSql("     , GBCOLON         = :GBCOLON                              ");
            parameter.AppendSql("     , GBCT            = :GBCT                                 ");
            parameter.AppendSql("     , GBLUNG_SANGDAM  = :GBLUNG_SANGDAM                       ");
            parameter.AppendSql("     , PANO            = :PANO                                 ");
            parameter.AppendSql("     , REMARK          = :REMARK                               ");
            parameter.AppendSql("     , SMSOK           = :SMSOK                                ");
            parameter.AppendSql("     , SDOCT           = :SDOCT                                ");
            parameter.AppendSql("     , ENTTIME         = SYSDATE                               ");
            parameter.AppendSql("     , ENTSABUN        = :ENTSABUN                             ");
            parameter.AppendSql(" WHERE ROWID           = :RID                                  ");

            parameter.Add("RTIME", item.RTIME);
            parameter.Add("JUMIN", item.JUMIN);
            parameter.Add("JUMIN2", item.JUMIN2);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("TEL", item.TEL);
            parameter.Add("HPHONE", item.HPHONE);
            parameter.Add("GBAM1", item.GBAM1);
            parameter.Add("GBAM2", item.GBAM2);
            parameter.Add("GBAM3", item.GBAM3);
            parameter.Add("GBAM4", item.GBAM4);
            parameter.Add("GBUGI", item.GBUGI);
            parameter.Add("GBGFS", item.GBGFS);
            parameter.Add("GBGFSH", item.GBGFSH);
            parameter.Add("GBMAMMO", item.GBMAMMO);
            parameter.Add("GBRECUTM", item.GBRECUTM);
            parameter.Add("GBSONO", item.GBSONO);
            parameter.Add("GBWOMB", item.GBWOMB);
            parameter.Add("GBBOHUM", item.GBBOHUM);
            parameter.Add("GBCOLON", item.GBCOLON);
            parameter.Add("GBCT", item.GBCT);
            parameter.Add("GBLUNG_SANGDAM", item.GBLUNG_SANGDAM);
            parameter.Add("PANO", item.PANO);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("SMSOK", item.SMSOK);
            parameter.Add("SDOCT", item.SDOCT);
            parameter.Add("ENTSABUN", item.ENTSABUN);            
            parameter.Add("RID", item.ROWID);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_CANCER_RESV2> GetListByRTimeJumin(string argDate, string argJumin)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RTIME, TO_CHAR(RTIME,'YYYY-MM-DD') RDATE                                    ");
            parameter.AppendSql("      ,JUMIN, SNAME, TEL, HPHONE, GBAM1, GBAM2                                     ");
            parameter.AppendSql("      ,GBAM3, GBAM4, GBUGI, GBGFS,GBGFSH, GBMAMMO, GBRECUTM, GBSONO,GBWOMB         ");
            parameter.AppendSql("      ,GBBOHUM,GBCT, REMARK,PANO, TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE            ");
            parameter.AppendSql("      ,ENTSABUN, ENTTIME, ROWID                                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV2                                                ");
            parameter.AppendSql(" WHERE RTIME >= TO_DATE(:RTIME,'YYYY-MM-DD')                                       ");
            parameter.AppendSql("   AND JUMIN2 =:JUMIN2                                                             ");
            parameter.AppendSql(" ORDER BY RTIME                                                                    ");

            parameter.Add("RTIME", argDate);
            parameter.Add("JUMIN2", argJumin);

            return ExecuteReader<HIC_CANCER_RESV2>(parameter);
        }

        public List<HIC_CANCER_RESV2> GetItembyRTime6(string strAmPm, string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SNAME, JUMIN2, GBAM1, GBAM2, GBAM3, GBAM4, GBUGI, GBGFS, GBGFSH             ");
            parameter.AppendSql("     , GBMAMMO, GBRECUTM, GBSONO, GBWOMB, GBBOHUM, GBCOLON, GBCT, SDOCT, REMARK    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV2                                                ");
            parameter.AppendSql(" WHERE RTime >= TO_DATE(:FRDATE, 'YYYY-MM-DD HH24:MI')                             ");
            parameter.AppendSql("   AND RTime <= TO_DATE(:TODATE, 'YYYY-MM-DD HH24:MI')                             ");
            if (strAmPm == "AM")
            {
                parameter.AppendSql("   AND TO_CHAR(RTime, 'HH24:MI') <= '12:30'                                    ");
            }
            else if (strAmPm == "PM")
            {
                parameter.AppendSql("   AND TO_CHAR(RTime, 'HH24:MI') > '12:30'                                     ");
            }
            parameter.AppendSql(" ORDER BY SName                                                                    ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteReader<HIC_CANCER_RESV2>(parameter);
        }

        public List<HIC_CANCER_RESV2> GetItembyRTime5(string strTempFDate, string strTempTDate, string strSort)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(RTIME,'YYYY/MM/DD HH24:MI') RTIME                                                       ");
            parameter.AppendSql("     , TO_CHAR(RTIME,'HH24:MI') RTIME_TIME, JUMIN2, SNAME, TEL, HPHONE, GBAM1, GBAM2,GBCT              ");
            parameter.AppendSql("     , GBAM3, GBAM4, GBUGI,GBGFS,GBGFSH,GBMAMMO, GBRECUTM, GBSONO,GBWOMB,GBBOHUM,GBCOLON,REMARK,PANO   ");
            parameter.AppendSql("     , TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,SDOCT,ENTSABUN, ENTTIME, ROWID, GBLUNG_SANGDAM            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV2                                                                    ");
            parameter.AppendSql(" WHERE RTime >= TO_DATE(:FRDATE, 'YYYY-MM-DD HH24:MI')                                                 ");
            parameter.AppendSql("   AND RTime <= TO_DATE(:TODATE, 'YYYY-MM-DD HH24:MI')                                                 ");
            if (strSort == "1")
            {
                parameter.AppendSql(" ORDER BY SName,RTime                                                                              ");
            }
            else if (strSort == "2")
            {
                parameter.AppendSql(" ORDER BY RTime,SName                                                                              ");
            }

            parameter.Add("FRDATE", strTempFDate);
            parameter.Add("TODATE", strTempTDate);

            return ExecuteReader<HIC_CANCER_RESV2>(parameter);
        }

        public List<HIC_CANCER_RESV2> GetItembyRTime4(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(RTIME,'YYYY-MM-DD') RTIME,TO_CHAR(RTIME,'HH24:MI') AMPM, GBUGI, GBGFS           ");
            parameter.AppendSql("     , GBMAMMO, GBRECUTM, GBSONO,GBWOMB,GBBOHUM,GBGFSH,GBCOLON,GBCT,SDOCT, GBLUNG_SANGDAM      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV2                                                            ");
            parameter.AppendSql(" WHERE RTime >= TO_DATE(:FRDATE ,'YYYY-MM-DD')                                                 ");
            parameter.AppendSql("   AND RTime <= TO_DATE(:TODATE ,'YYYY-MM-DD HH24:MI')                                         ");
            parameter.AppendSql(" ORDER BY RTime                                                                                ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteReader<HIC_CANCER_RESV2>(parameter);
        }

        public int UpdateRemark(string strRemark, string strRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CANCER_RESV2 SET    ");
            parameter.AppendSql("       REMARK = :REMARK                    ");
            parameter.AppendSql(" WHERE ROWID  = :RID                       ");

            parameter.Add("REMARK", strRemark);
            parameter.Add("RID", strRowId);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_CANCER_RESV2 item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_CANCER_RESV2                                                           ");
            parameter.AppendSql("       (RTIME , JUMIN, SNAME, TEL, HPHONE, GBAM1, GBAM2, GBAM3, GBAM4                              ");
            parameter.AppendSql("     , GBUGI,GBGFS,GBGFSH,GBMAMMO,GBRECUTM,GBSONO,GBWOMB,GBBOHUM,GBCOLON,GBCT,GBLUNG_SANGDAM, PANO ");
            parameter.AppendSql("     , REMARK,SMSOK,SDOCT,ENTTIME,ENTSABUN,JUMIN2 )                                                ");
            parameter.AppendSql(" VALUES                                                                                            ");
            //parameter.AppendSql("       (TO_DATE(:RTIME, 'YYYY-MM-DD HH24:MI:SS')                                                   ");
            parameter.AppendSql("       (:RTIME                                                                                     ");
            parameter.AppendSql("     , :JUMIN, :SNAME, :TEL, :HPHONE, :GBAM1, :GBAM2, :GBAM3, :GBAM4                               ");
            parameter.AppendSql("     , :GBUGI, :GBGFS, :GBGFSH, :GBMAMMO, :GBRECUTM, :GBSONO, :GBWOMB, :GBBOHUM, :GBCOLON          ");
            parameter.AppendSql("     , :GBCT, :GBLUNG_SANGDAM, :PANO, :REMARK, :SMSOK, :SDOCT, SYSDATE, :ENTSABUN, :JUMIN2)        ");
            
            parameter.Add("RTIME", item.RTIME);
            parameter.Add("JUMIN", item.JUMIN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMIN2", item.JUMIN2);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("TEL", item.TEL);
            parameter.Add("HPHONE", item.HPHONE);
            parameter.Add("GBAM1", item.GBAM1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBAM2", item.GBAM2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBAM3", item.GBAM3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBAM4", item.GBAM4, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBUGI", item.GBUGI, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBGFS", item.GBGFS, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBGFSH", item.GBGFSH, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBMAMMO", item.GBMAMMO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBRECUTM", item.GBRECUTM, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSONO", item.GBSONO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBWOMB", item.GBWOMB, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBBOHUM", item.GBBOHUM, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBCOLON", item.GBCOLON, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBCT", item.GBCT, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBLUNG_SANGDAM", item.GBLUNG_SANGDAM);
            parameter.Add("PANO", item.PANO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("SMSOK", item.SMSOK, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SDOCT", item.SDOCT);
            parameter.Add("ENTSABUN", item.ENTSABUN);            

            return ExecuteNonQuery(parameter);
        }

        public string GetRowIdbyJumin2RTime(string strJumin, string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV2                            ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2                                        ");
            parameter.AppendSql("   AND RTIME >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                 ");
            parameter.AppendSql("   AND RTIME <= TO_DATE(:TODATE, 'YYYY-MM-DD HH24:MI')         ");
            parameter.AppendSql(" ORDER BY EntTime DESC                                         ");

            parameter.Add("JUMIN2", strJumin);
            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_CANCER_RESV2> GetItembyLtdCodeRTime(string strLtdCode, string strFDate, string strTDate, string strYYMM)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(RTime,'YYYY/MM/DD HH24:MI') RTIME                                                       ");
            parameter.AppendSql("     , TO_CHAR(RTIME, 'HH24:MI') RTIME_TIME, Jumin2, SName, Tel, Hphone, GbAm1, GbAm2                  ");
            parameter.AppendSql("     , GbAm3, GbAm4, GbUgi,GbGfs,GbGfsH,GbMammo, GbRecutm, GbSono,GbWomb,GbBohum,GbColon,Remark,Pano   ");
            parameter.AppendSql("     , TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,SDoct,EntSabun, EntTime, ROWID                            ");
            parameter.AppendSql("     , GBCT, GBLUNG_SANGDAM                                                                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV2                                                                    ");
            parameter.AppendSql(" WHERE Jumin2 IN (SELECT Jumin2 FROM HIC_PATIENT WHERE LtdCode = :LTDCODE)                             ");
            if (strYYMM == "전체")
            {
                parameter.AppendSql("   AND RTime>=TRUNC(SYSDATE - 30)                                                                  ");
            }
            else
            {
                parameter.AppendSql("   AND RTime >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                     ");
                parameter.AppendSql("   AND RTime <= TO_DATE(:TODATE, 'YYYY-MM-DD HH24:MI')                                             ");
            }
            parameter.AppendSql(" ORDER BY SName, RTime                                                                                 ");

            parameter.Add("LTDCODE", strLtdCode);
            if (strYYMM != "전체")
            {
                parameter.Add("FRDATE", strFDate);
                parameter.Add("TODATE", strTDate);
            }
                    

            return ExecuteReader<HIC_CANCER_RESV2>(parameter);
        }

        public HIC_CANCER_RESV2 GetItembyRowId(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBUGI, GBGFS,GBGFSH, GBMAMMO, GBRECUTM, GBSONO, GBWOMB, GBBOHUM, GBCOLON, GBCT  ");
            parameter.AppendSql("     , SMSOK,ENTSABUN,TO_CHAR(RTime,'YYYY-MM-DD') RTIME                                ");
            parameter.AppendSql("     , TO_CHAR(RTime, 'HH24:MI') AMPM                                                  ");
            parameter.AppendSql("     , SDOCT, GBLUNG_SANGDAM                                                           ");
            parameter.AppendSql("     , JUMIN2, SNAME, TEL, HPHONE, GBAM1, GBAM2                                        ");
            parameter.AppendSql("     , GBAM3, GBAM4, GBUGI, GBGFS, GBGFSH, GBMAMMO, GBRECUTM                           ");
            parameter.AppendSql("     , REMARK, PANO, ENTTIME                                                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV2                                                    ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                                    ");

            parameter.Add("RID", strROWID);

            return ExecuteReaderSingle<HIC_CANCER_RESV2>(parameter);
        }

        public List<HIC_CANCER_RESV2> GetRTimebyJumin2RTime(string strJumin, string strSDate, string strEDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID, SName,TO_CHAR(RTime,'MM/DD') RTime, TO_CHAR(RTime,'YYYY-MM-DD') RTime1   ");
            parameter.AppendSql("     , TO_CHAR(RTime, 'YYYY-MM-DD HH24:MI') RTIMESYSDATE                               ");
            parameter.AppendSql("     , GBUGI, GBGFS, GBGFSH, GBMAMMO, GBCT, GBRECUTM, GBCOLON, GBSONO, GBWOMB, GBBOHUM ");
            parameter.AppendSql("     , GBLUNG_SANGDAM                                                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV2                                                    ");
            parameter.AppendSql(" WHERE JUMIN2 = :JUMIN2                                                                ");
            parameter.AppendSql("   AND RTIME >= TO_DATE(:FRDATE, 'YYYY-MM-DD HH24:MI')                                 ");
            parameter.AppendSql("   AND RTIME <= TO_DATE(:TODATE, 'YYYY-MM-DD HH24:MI')                                 ");
            parameter.AppendSql("ORDER BY RTime                                                                         ");

            parameter.Add("JUMIN2", strJumin);
            parameter.Add("FRDATE", strSDate);
            parameter.Add("TODATE", strEDate);

            return ExecuteReader<HIC_CANCER_RESV2>(parameter);
        }

        public List<HIC_CANCER_RESV2> GetItembyRTimeHPhone(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PANO, SNAME, HPHONE,TO_CHAR(RTIME,'YYYY-MM-DD HH24:MI') RTIME               ");
            parameter.AppendSql("     , TO_CHAR(RTIME, 'HH24:MI') RTIME2                                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV2                                                ");
            parameter.AppendSql(" WHERE RTIME >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND RTIME <  TO_DATE(:TODATE, 'YYYY-MM-DD HH24:MI')                                     ");
            parameter.AppendSql("   AND SUBSTR(HPhone,1,3) IN ('010','011','016','017','018','019')                 ");
            parameter.AppendSql(" ORDER BY RTIME, SNAME, HPHONE                                                     ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteReader<HIC_CANCER_RESV2>(parameter);
        }

        public List<HIC_CANCER_RESV2> GetItembyEntTime(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Pano,SName,HPhone, TO_CHAR(RTIME,'YYYY-MM-DD HH24:MI') RTIME                ");
            parameter.AppendSql("     , GBUGI,GBGFS,GBMAMMO, GBRECUTM,GBSONO,GBWOMB,GBBOHUM,GBGFSH,GBCOLON,GBCT     ");
            parameter.AppendSql("     , TO_CHAR(RTIME, 'HH24:MI') RTIME2                                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV2                                                ");
            parameter.AppendSql(" WHERE ENTTIME >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("   AND ENTTIME <  TO_DATE(:TODATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("   AND SUBSTR(HPhone,1,3) IN ('010','011','016','017','018','019')                 ");
            parameter.AppendSql("   AND RTIME >= TRUNC(SYSDATE)                                                     ");
            parameter.AppendSql(" ORDER BY RTime,SName,HPhone                                                       ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteReader<HIC_CANCER_RESV2>(parameter);
        }

        public int UpdateRTimeEntTimeEntSabunbyRowId(string strDelDate, string idNumber, string strRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CANCER_RESV2                        ");
            parameter.AppendSql("   SET RTIME    = TO_DATE(:RTIME, 'YYYY-MM-DD HH24:MI')    ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE                                  ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                                ");
            parameter.AppendSql(" WHERE ROWID    = :RID                                     ");

            parameter.Add("RTIME", strDelDate);
            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("RID", strRowId);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_CANCER_RESV2> GetItembyRTime2(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(RTime,'YYYY-MM-DD HH24:MI') RTime, Jumin2, SName, Tel, Hphone, GbAm1, GbAm2     ");
            parameter.AppendSql("     , GbAm3, GbAm4, GbUgi, GbGfs, GbMammo, GbRecutm, GbSono,GbWomb, Remark,Pano               ");
            parameter.AppendSql("     , TO_CHAR(JepDate,'YYYY-MM-DD') JepDate, EntSabun, EntTime, ROWID                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV2                                                            ");
            parameter.AppendSql(" WHERE RTime >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                 ");
            parameter.AppendSql("   AND RTime <  TO_DATE(:TODATE ,'YYYY-MM-DD')                                                 ");
            parameter.AppendSql(" ORDER BY SNAME, JUMIN2                                                                        ");
            
            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteReader<HIC_CANCER_RESV2>(parameter);
        }

        public List<HIC_CANCER_RESV2> GetItembyRTime3(string strAMPM, string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JUMIN2, GBUGI, GBGFS, GBGFSH                                                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV2                                                            ");
            if (strAMPM == "AM")
            {
                parameter.AppendSql(" WHERE RTime >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                             ");
                parameter.AppendSql("   AND RTime <= TO_DATE(:TODATE, 'YYYY-MM-DD HH24:MI')                                     ");
            }
            else if (strAMPM == "PM")
            {
                parameter.AppendSql(" WHERE RTime >= TO_DATE(:FRDATE, 'YYYY-MM-DD HH24:MI')                                     ");
                parameter.AppendSql("   AND RTime <= TO_DATE(:TODATE, 'YYYY-MM-DD HH24:MI')                                     ");
            }


            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteReader<HIC_CANCER_RESV2>(parameter);
        }

        public HIC_CANCER_RESV2 GetItembyJumin(string strJumin, string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RTIME,JUMIN,SNAME,TEL,HPHONE,GBAM1,GBAM2,GBAM3,GBAM4,GBUGI,GBGFS,GBMAMMO                ");
            parameter.AppendSql("      ,GBRECUTM, GBSONO, REMARK, JEPDATE, ENTSABUN, ENTTIME, PANO, GBWOMB, SMSOK, JUMIN2       ");
            parameter.AppendSql("      ,GBBOHUM, GBGFSH, SDOCT, GBCOLON, GBCT, GBLUNG_SANGDAM                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV2                                                            ");
            parameter.AppendSql(" WHERE 1=1                                                                                     ");
            parameter.AppendSql("   AND RTime >= TO_DATE(:FRDATE ,'YYYY-MM-DD')                                                 ");
            parameter.AppendSql("   AND RTime <  TO_DATE(:TODATE ,'YYYY-MM-DD')                                                 ");
            parameter.AppendSql("   AND JUMIN2 = :JUMIN                                                                         ");

            parameter.Add("JUMIN", strJumin);
            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteReaderSingle<HIC_CANCER_RESV2>(parameter);
        }

        public HIC_CANCER_RESV2 GetItemByPtnoRTime(string strPtno, string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SDOCT                                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV2                            ");
            parameter.AppendSql(" WHERE 1=1                                                     ");
            parameter.AppendSql("   AND PANO  = :PTNO                                           ");
            parameter.AppendSql("   AND RTIME >= TO_DATE(:RDATE, 'YYYY-MM-DD')                  ");
            parameter.AppendSql(" ORDER BY RTIME                                                ");

            parameter.Add("PTNO", strPtno);
            parameter.Add("RDATE", strDate);

            return ExecuteReaderSingle<HIC_CANCER_RESV2>(parameter);
        }
    }
}
