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
    public class HicResBohum1Repository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicResBohum1Repository()
        {
        }
        public HIC_RES_BOHUM1 GetItemByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, HEIGHT, WEIGHT, BIMAN, EYE_L, EYE_R, EAR_L, EAR_R, BLOOD_H, BLOOD_L, URINE1              ");
            parameter.AppendSql("     , URINE2, URINE3, URINE4, BLOOD1, BLOOD2, BLOOD3, BLOOD4, BLOOD5, BLOOD6, LIVER1, LIVER2          ");
            parameter.AppendSql("     , LIVER3, XRAYGBN, XRAYRES, EKG, CYTO1, CYTO2, EXAMFLAG, SICK11, SICK12, SICK13, SICK21, SICK22   ");
            parameter.AppendSql("     , SICK23, SICK31, SICK32, SICK33, GAJOK1, GAJOK2, GAJOK3, GAJOK4, GAJOK5, GAJOK6, ROSICK          ");
            parameter.AppendSql("     , ROSICKNAME, SIKSENG, DRINK1, DRINK2, SMOKING1, SMOKING2, SMOKING3, SPORTS, ANNOUNE              ");
            parameter.AppendSql("     , WOMAN1, WOMAN2, WOMAN3, MUNJINFLAG, MUNJINENTDATE, MUNJINENTSABUN, OLDBYENG, OLDBYENG1          ");
            parameter.AppendSql("     , OLDBYENG2, OLDBYENG3, OLDBYENG4, OLDBYENG5, OLDBYENG6, OLDBYENG7, HABIT, HABIT1, HABIT2         ");
            parameter.AppendSql("     , HABIT3, HABIT4, HABIT5, JINCHAL1, JINCHAL2, PANJENG, PANJENGB1, PANJENGB2, PANJENGB3            ");
            parameter.AppendSql("     , PANJENGB4, PANJENGB5, PANJENGB6, PANJENGB7, PANJENGB8, PANJENGR1, PANJENGR2, PANJENGR3          ");
            parameter.AppendSql("     , PANJENGR4, PANJENGR5, PANJENGR6, PANJENGR7, PANJENGR8, PANJENGR9, PANJENGR10, PANJENGR11        ");
            parameter.AppendSql("     , PANJENGETC, TO_CHAR(PANJENGDATE,'YYYY-MM-DD') PANJENGDATE, GUNDATE, TONGBOGBN, TONGBODATE       ");
            parameter.AppendSql("     , PANJENGDRNO, SOGEN, XRAYNO                                                                      ");
            parameter.AppendSql("     , IPSADATE, WOMB01, WOMB02, WOMB03, WOMB04, WOMB05, WOMB06, WOMB07, WOMB08, WOMB09, WOMB10        ");
            parameter.AppendSql("     , WOMB11, OLDBYENGNAME, GBPRINT, MUNJINDRNO, JINREMARK, GBPANJENG, PANJENGB9, PANJENGB_ETC        ");
            parameter.AppendSql("     , PANJENGR_ETC, ADDSOGEN, SMOKING4, SMOKING5, PANJENGB_ETC_DTL, WAIST, T_STAT01, T_STAT02         ");
            parameter.AppendSql("     , T_STAT11, T_STAT12, T_STAT21, T_STAT22, T_STAT31, T_STAT32, T_STAT41, T_STAT42, T_STAT51        ");
            parameter.AppendSql("     , T_STAT52, T_GAJOK1, T_GAJOK2, T_GAJOK3, T_GAJOK4, T_GAJOK5, T_BLIVER, T_SMOKE1, T_SMOKE2        ");
            parameter.AppendSql("     , T_SMOKE3, T_SMOKE4, T_SMOKE5, T_DRINK1, T_DRINK2, T_ACTIVE1, T_ACTIVE2, T_ACTIVE3               ");
            parameter.AppendSql("     , T40_FEEL1, T40_FEEL2, T40_FEEL3, T40_FEEL4, T66_INJECT, T66_STAT1, T66_STAT2, T66_STAT3         ");
            parameter.AppendSql("     , T66_STAT4, T66_STAT5, T66_STAT6, T66_FEEL1, T66_FEEL2, T66_FEEL3, T66_MEMORY1, T66_MEMORY2      ");
            parameter.AppendSql("     , T66_MEMORY3, T66_MEMORY4, T66_MEMORY5, T66_FALL, T66_URO, PANJENGC1, PANJENGC2                  ");
            parameter.AppendSql("     , PANJENGC3, PANJENGC4, PANJENGD11, PANJENGD12, PANJENGD13, PANJENGD21, PANJENGD22                ");
            parameter.AppendSql("     , PANJENGD23, PANJENGSAHU, PANJENGC5, SANGDAM, SANGDAM2, GBSIKSA, T40_FEEL, T66_STAT              ");
            parameter.AppendSql("     , FOOT1, FOOT2, BALANCE, OSTEO, LIFESOGEN, T_STAT61, T_STAT62, PANJENGU1, PANJENGU2, PANJENGU3    ");
            parameter.AppendSql("     , PANJENGU4, PANJENGSAHU2, PANJENGSAHU3, WORKYN, PRTSABUN, T_STAT71, T_STAT72, SOGENB             ");
            parameter.AppendSql("     , GBGONGHU, TMUN0001, TMUN0002, TMUN0003, TMUN0004, TMUN0005, TMUN0006, TMUN0007, TMUN0008        ");
            parameter.AppendSql("     , TMUN0009, TMUN0010, TMUN0011, TMUN0012, TMUN0013, TMUN0014, TMUN0015, TMUN0016, TMUN0017        ");
            parameter.AppendSql("     , TMUN0018, TMUN0019, TMUN0020, TMUN0021, TMUN0022, TMUN0023, TMUN0024, TMUN0025, TMUN0026        ");
            parameter.AppendSql("     , TMUN0027, TMUN0028, TMUN0029, TMUN0030, TMUN0031, TMUN0032, TMUN0033, TMUN0034, TMUN0035        ");
            parameter.AppendSql("     , TMUN0036, TMUN0037, TMUN0038, TMUN0039, TMUN0040, TMUN0041, TMUN0042, TMUN0043, TMUN0044        ");
            parameter.AppendSql("     , TMUN0045, TMUN0046, TMUN0047, TMUN0048, TMUN0049, TMUN0050, TMUN0051, TMUN0052, TMUN0053        ");
            parameter.AppendSql("     , TMUN0054, TMUN0055, TMUN0056, TMUN0057, TMUN0058, TMUN0059, TMUN0060, TMUN0061, TMUN0062        ");
            parameter.AppendSql("     , TMUN0063, TMUN0064, TMUN0065, TMUN0066, TMUN0067, TMUN0068, TMUN0069, TMUN0070, TMUN0071        ");
            parameter.AppendSql("     , TMUN0072, TMUN0073, TMUN0074, TMUN0075, TMUN0076, TMUN0077, TMUN0078, TMUN0079, TMUN0080        ");
            parameter.AppendSql("     , TMUN0081, TMUN0082, TMUN0083, TMUN0084, TMUN0085, TMUN0086, TMUN0087, TMUN0088, TMUN0089        ");
            parameter.AppendSql("     , TMUN0090, TMUN0091, TMUN0092, TMUN0093, TMUN0094, TMUN0095, PANJENGC, PANJENGD, SLIP_SMOKE      ");
            parameter.AppendSql("     , SLIP_DRINK, SLIP_ACTIVE, SLIP_FOOD, SLIP_BIMAN, SOGENC, SOGEND, SLIP_PHQ, SLIP_KDSQ             ");
            parameter.AppendSql("     , SLIP_OLDMAN, SLIP_LIFESOGEN1, SLIP_LIFESOGEN2, PANJENGB10, PANJENGR12, TMUN0096                 ");
            parameter.AppendSql("     , TMUN0097, TMUN0098, TMUN0099, TMUN0100, TMUN0101, TMUN0102, TMUN0103, TMUN0104, SIM_RESULT1     ");
            parameter.AppendSql("     , SIM_RESULT2, SIM_RESULT3, TMUN0105, TMUN0106, TMUN0107, TMUN0108, TMUN0109, TMUN0110, TMUN0111  ");
            parameter.AppendSql("     , TMUN0112, TMUN0113, TMUN0114, TMUN0115, TMUN0116, TMUN0117, TMUN0118, TMUN0119, TMUN0120        ");
            parameter.AppendSql("     , TMUN0121, TMUN0122, TMUN0123, TMUN0124, TMUN0125, TMUN0126, TMUN0127, TMUN0128, ROWID RID       ");    
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1                                                                      ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                           ");
            parameter.AppendSql("   AND WRTNO  = :WRTNO                                                                                 ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReaderSingle<HIC_RES_BOHUM1>(parameter);
        }

        public int UpdateAll(HIC_RES_BOHUM1 item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET      ");
            parameter.AppendSql("       HEIGHT   = :HEIGHT                  ");
            parameter.AppendSql("     , WEIGHT   = :WEIGHT                  ");
            parameter.AppendSql("     , WAIST    = :WAIST                   ");
            parameter.AppendSql("     , BIMAN    = :BIMAN                   ");
            parameter.AppendSql("     , EYE_L    = :EYE_L                   ");
            parameter.AppendSql("     , EYE_R    = :EYE_R                   ");
            parameter.AppendSql("     , EAR_L    = :EAR_L                   ");
            parameter.AppendSql("     , EAR_R    = :EAR_R                   ");
            parameter.AppendSql("     , BLOOD_H  = :BLOOD_H                 ");
            parameter.AppendSql("     , BLOOD_L  = :BLOOD_L                 ");
            parameter.AppendSql("     , URINE1   = :URINE1                  ");
            parameter.AppendSql("     , URINE2   = :URINE2                  ");
            parameter.AppendSql("     , URINE3   = :URINE3                  ");
            parameter.AppendSql("     , URINE4   = :URINE4                  ");
            parameter.AppendSql("     , BLOOD1   = :BLOOD1                  ");
            parameter.AppendSql("     , BLOOD2   = :BLOOD2                  ");
            parameter.AppendSql("     , BLOOD3   = :BLOOD3                  ");
            parameter.AppendSql("     , BLOOD4   = :BLOOD4                  ");
            parameter.AppendSql("     , BLOOD5   = :BLOOD5                  ");
            parameter.AppendSql("     , BLOOD6   = :BLOOD6                  ");
            parameter.AppendSql("     , LIVER1   = :LIVER1                  ");
            parameter.AppendSql("     , LIVER2   = :LIVER2                  ");
            parameter.AppendSql("     , LIVER3   = :LIVER3                  ");
            parameter.AppendSql("     , XRAYGBN  = :XRAYGBN                 ");
            parameter.AppendSql("     , XRAYRES  = :XRAYRES                 ");
            parameter.AppendSql("     , FOOT1    = :FOOT1                   ");
            parameter.AppendSql("     , FOOT2    = :FOOT2                   ");
            parameter.AppendSql("     , BALANCE  = :BALANCE                 ");
            parameter.AppendSql("     , OSTEO    = :OSTEO                   ");
            parameter.AppendSql("     , EKG      = :EKG                     ");
            parameter.AppendSql("     , GBGONGHU = :GBGONGHU                ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                   ");

            parameter.Add("HEIGHT", item.HEIGHT);
            parameter.Add("WEIGHT", item.WEIGHT);
            parameter.Add("WAIST", item.WAIST);
            parameter.Add("BIMAN", item.BIMAN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EYE_L", item.EYE_L);
            parameter.Add("EYE_R", item.EYE_R);
            parameter.Add("EAR_L", item.EAR_L, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EAR_R", item.EAR_R, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BLOOD_H", item.BLOOD_H);
            parameter.Add("BLOOD_L", item.BLOOD_L);
            parameter.Add("URINE1", item.URINE1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("URINE2", item.URINE2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("URINE3", item.URINE3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("URINE4", item.URINE4);
            parameter.Add("BLOOD1", item.BLOOD1);
            parameter.Add("BLOOD2", item.BLOOD2);
            parameter.Add("BLOOD3", item.BLOOD3);
            parameter.Add("BLOOD4", item.BLOOD4);
            parameter.Add("BLOOD5", item.BLOOD5);
            parameter.Add("BLOOD6", item.BLOOD6);
            parameter.Add("LIVER1", item.LIVER1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LIVER2", item.LIVER2);
            parameter.Add("LIVER3", item.LIVER3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("XRAYGBN", item.XRAYGBN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("XRAYRES", item.XRAYRES, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FOOT1", item.FOOT1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FOOT2", item.FOOT2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BALANCE", item.BALANCE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("OSTEO", item.OSTEO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EKG", item.EKG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBGONGHU", item.GBGONGHU, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public HIC_RES_BOHUM1 GetPanjengDatebyWrtno(long wRTNO, string strChasu)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PanjengDate,'YYYY-MM-DD') PANJENGDATE                                                   ");
            if (strChasu == "1")
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1                                                                  ");
            }
            else
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM2                                                                  ");
            }
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                                                 ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReaderSingle<HIC_RES_BOHUM1>(parameter);
        }

        public long GetPanjengDrNobyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANJENGDRNO                                                                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1                                                                      ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                                                 ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteScalar<long>(parameter);
        }

        public void UpdateTongBoInfobyWrtNo(long fnWRTNO, string strDate, string strGbn, string strSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET                      ");
            if (!strDate.IsNullOrEmpty())
            {
                parameter.AppendSql("       TONGBODATE  = TO_DATE(:TONGBODATE, 'YYYY-MM-DD')");
                parameter.AppendSql("     , TONGBOGBN  = :TONGBOGBN                         ");
                parameter.AppendSql("     , GBPRINT = 'Y'                                   ");
                parameter.AppendSql("     , PRTSABUN  = :PRTSABUN                           ");
            }
            else
            {
                parameter.AppendSql("       TONGBODATE  = ''                                ");
                parameter.AppendSql("     , TONGBOGBN  = ''                                 ");
                parameter.AppendSql("     , GBPRINT = 'N'                                   ");
                parameter.AppendSql("     , PRTSABUN  = ''                                  ");
            }

            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                     ");

            if (!strDate.IsNullOrEmpty())
            {
                parameter.Add("TONGBODATE", strDate);
                parameter.Add("TONGBOGBN", strGbn);
            }

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("PRTSABUN", strSabun);

            ExecuteNonQuery(parameter);
        }

        public void UpdatePanjengInfobyWrtNo(long fnWRTNO, string strDate, long nDrNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET                      ");
            if (nDrNO > 0)
            {
                parameter.AppendSql("     TONGBODATE  = TO_DATE(:TONGBODATE, 'YYYY-MM-DD')    ");
                parameter.AppendSql("     , PANJENGDATE = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')   ");
            }
            else
            {
                parameter.AppendSql("     TONGBODATE  = ''    ");
                parameter.AppendSql("     , PANJENGDATE = ''   ");
            }
            
            parameter.AppendSql("     , PANJENGDRNO = :PANJENGDRNO                          ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                     ");

            if (nDrNO > 0)
            {
                parameter.Add("TONGBODATE", strDate);
                parameter.Add("PANJENGDATE", strDate);
            }
            
            parameter.Add("PANJENGDRNO", nDrNO);
            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public void UpdateNotPanjengbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1          ");
            parameter.AppendSql("   SET GBPRINT     = 'N'                       ");
            parameter.AppendSql("     , TONGBODATE  = ''                     ");
            parameter.AppendSql("     , TONGBOGBN   = ''                     ");
            parameter.AppendSql("     , PRTSABUN    = ''                     ");
            parameter.AppendSql("     , GBPANJENG   = ''                     ");
            parameter.AppendSql("     , PANJENG     = ''                     ");
            parameter.AppendSql("     , PANJENGDATE = ''                     ");
            parameter.AppendSql("     , PANJENGDRNO = ''                     ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                     ");

            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public int UpdateWrtNobyFWrtNo(long nWrtNo, long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET      ");
            parameter.AppendSql("       WRTNO  = :WRTNO                     ");
            parameter.AppendSql(" WHERE WRTNO  = :FWRTNO                    ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("FWRTNO", fnWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateOnlyuHabitbyWrtNo(HIC_RES_BOHUM1 item1)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET                          ");
            parameter.AppendSql("       HABIT1           = :HABIT1                              ");
            parameter.AppendSql("     , HABIT2           = :HABIT2                              ");
            parameter.AppendSql("     , HABIT3           = :HABIT3                              ");
            parameter.AppendSql("     , HABIT4           = :HABIT4                              ");
            parameter.AppendSql(" WHERE WRTNO            = :WRTNO                               ");

            parameter.Add("HABIT1", item1.HABIT1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT2", item1.HABIT2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT3", item1.HABIT3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT4", item1.HABIT4, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", item1.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateHicPanjengbyWrtNo(HIC_RES_BOHUM1 item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET                          ");
            parameter.AppendSql("       PANJENG          = :PANJENG                             ");
            parameter.AppendSql("     , PANJENGB1        = :PANJENGB1                           ");
            parameter.AppendSql("     , PANJENGB2        = :PANJENGB2                           ");
            parameter.AppendSql("     , PANJENGB3        = :PANJENGB3                           ");
            parameter.AppendSql("     , PANJENGB4        = :PANJENGB4                           ");
            parameter.AppendSql("     , PANJENGB5        = :PANJENGB5                           ");
            parameter.AppendSql("     , PANJENGB6        = :PANJENGB6                           ");
            parameter.AppendSql("     , PANJENGB7        = :PANJENGB7                           ");
            parameter.AppendSql("     , PANJENGB8        = :PANJENGB8                           ");
            parameter.AppendSql("     , PANJENGB9        = :PANJENGB9                           ");
            parameter.AppendSql("     , PANJENGB10       = :PANJENGB10                          ");
            parameter.AppendSql("     , PANJENGR1        = :PANJENGR1                           ");
            parameter.AppendSql("     , PANJENGR2        = :PANJENGR2                           ");
            parameter.AppendSql("     , PANJENGR3        = :PANJENGR3                           ");
            parameter.AppendSql("     , PANJENGR4        = :PANJENGR4                           ");
            parameter.AppendSql("     , PANJENGR5        = :PANJENGR5                           ");
            parameter.AppendSql("     , PANJENGR6        = :PANJENGR6                           ");
            parameter.AppendSql("     , PANJENGR7        = :PANJENGR7                           ");
            parameter.AppendSql("     , PANJENGR8        = :PANJENGR8                           ");
            parameter.AppendSql("     , PANJENGR9        = :PANJENGR9                           ");
            parameter.AppendSql("     , PANJENGR10       = :PANJENGR10                          ");
            parameter.AppendSql("     , PANJENGR11       = :PANJENGR11                          ");
            parameter.AppendSql("     , PANJENGR12       = :PANJENGR12                          ");
            parameter.AppendSql("     , PANJENGU1        = :PANJENGU1                           ");
            parameter.AppendSql("     , PANJENGU2        = :PANJENGU2                           ");
            parameter.AppendSql("     , PANJENGU3        = :PANJENGU3                           ");
            parameter.AppendSql("     , PANJENGU4        = :PANJENGU4                           ");
            parameter.AppendSql("     , PANJENGD11       = :PANJENGD11                          ");
            parameter.AppendSql("     , PANJENGD12       = :PANJENGD12                          ");
            parameter.AppendSql("     , PANJENGD13       = :PANJENGD13                          ");
            parameter.AppendSql("     , PANJENGD21       = :PANJENGD21                          ");
            parameter.AppendSql("     , PANJENGD22       = :PANJENGD22                          ");
            parameter.AppendSql("     , PANJENGD23       = :PANJENGD23                          ");
            parameter.AppendSql("     , PANJENGSAHU      = :PANJENGSAHU                         ");
            parameter.AppendSql("     , PANJENGSAHU2     = :PANJENGSAHU2                        ");
            parameter.AppendSql("     , PANJENGSAHU3     = :PANJENGSAHU3                        ");
            parameter.AppendSql("     , WORKYN           = :WORKYN                              ");
            parameter.AppendSql("     , PANJENGB_ETC     = :PANJENGB_ETC                        ");
            parameter.AppendSql("     , PANJENGB_ETC_DTL = :PANJENGB_ETC_DTL                    ");
            parameter.AppendSql("     , PANJENGR_ETC     = :PANJENGR_ETC                        ");
            parameter.AppendSql("     , LIVER3           = :LIVER3                              ");
            parameter.AppendSql("     , T40_FEEL         = :T40_FEEL                            ");
            parameter.AppendSql("     , T66_STAT         = :T66_STAT                            ");
            //parameter.AppendSql("     , LIFESOGEN        = :LIFESOGEN                           ");
            //parameter.AppendSql("     , PANJENGETC       = :PANJENGETC                          ");
            parameter.AppendSql("     , SOGEN            = :SOGEN                               ");
            parameter.AppendSql("     , SOGENB           = :SOGENB                              ");
            parameter.AppendSql("     , SOGENC           = :SOGENC                              ");
            parameter.AppendSql("     , SOGEND           = :SOGEND                              ");
            parameter.AppendSql("     , PANJENGDATE      = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("     , PANJENGDRNO      = :PANJENGDRNO                         ");
            parameter.AppendSql(" WHERE WRTNO            = :WRTNO                               ");

            parameter.Add("PANJENG", item.PANJENG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGB1", item.PANJENGB1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGB2", item.PANJENGB2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGB3", item.PANJENGB3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGB4", item.PANJENGB4, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGB5", item.PANJENGB5, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGB6", item.PANJENGB6, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGB7", item.PANJENGB7, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGB8", item.PANJENGB8, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGB9", item.PANJENGB9, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGB10", item.PANJENGB10, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGR1", item.PANJENGR1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGR2", item.PANJENGR2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGR3", item.PANJENGR3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGR4", item.PANJENGR4, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGR5", item.PANJENGR5, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGR6", item.PANJENGR6, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGR7", item.PANJENGR7, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGR8", item.PANJENGR8, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGR9", item.PANJENGR9, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGR10", item.PANJENGR10, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGR11", item.PANJENGR11, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGR12", item.PANJENGR12, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGU1", item.PANJENGU1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGU2", item.PANJENGU2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGU3", item.PANJENGU3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGU4", item.PANJENGU4, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGD11", item.PANJENGD11, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGD12", item.PANJENGD12, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGD13", item.PANJENGD13, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGD21", item.PANJENGD21, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGD22", item.PANJENGD22, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGD23", item.PANJENGD23, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGSAHU", item.PANJENGSAHU);
            parameter.Add("PANJENGSAHU2", item.PANJENGSAHU2);
            parameter.Add("PANJENGSAHU3", item.PANJENGSAHU3);
            parameter.Add("WORKYN", item.WORKYN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGB_ETC", item.PANJENGB_ETC);
            parameter.Add("PANJENGB_ETC_DTL", item.PANJENGB_ETC_DTL, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGR_ETC", item.PANJENGR_ETC);
            parameter.Add("LIVER3", item.LIVER3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T40_FEEL", item.T40_FEEL, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T66_STAT", item.T66_STAT, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            //parameter.Add("LIFESOGEN", item.LIFESOGEN);
            //parameter.Add("PANJENGETC", item.PANJENGETC, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SOGEN", item.SOGEN);
            parameter.Add("SOGENB", item.SOGENB);
            parameter.Add("SOGENC", item.SOGENC);
            parameter.Add("SOGEND", item.SOGEND);
            parameter.Add("PANJENGDATE", item.PANJENGDATE);
            parameter.Add("PANJENGDRNO", item.PANJENGDRNO);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateLifebyWrtNo(HIC_RES_BOHUM1 item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET      ");
            parameter.AppendSql("       SLIP_SMOKE      = :SLIP_SMOKE       ");
            parameter.AppendSql("     , SLIP_DRINK      = :SLIP_DRINK       ");
            parameter.AppendSql("     , SLIP_ACTIVE     = :SLIP_ACTIVE      ");
            parameter.AppendSql("     , SLIP_FOOD       = :SLIP_FOOD        ");
            parameter.AppendSql("     , SLIP_BIMAN      = :SLIP_BIMAN       ");
            parameter.AppendSql("     , SLIP_LIFESOGEN1 = :SLIP_LIFESOGEN1  ");
            parameter.AppendSql("     , SLIP_LIFESOGEN2 = :SLIP_LIFESOGEN2  ");
            parameter.AppendSql(" WHERE WRTNO           = :WRTNO            ");

            parameter.Add("SLIP_SMOKE", item.SLIP_SMOKE);
            parameter.Add("SLIP_DRINK", item.SLIP_DRINK);
            parameter.Add("SLIP_ACTIVE", item.SLIP_ACTIVE);
            parameter.Add("SLIP_FOOD", item.SLIP_FOOD);
            parameter.Add("SLIP_BIMAN", item.SLIP_BIMAN);
            parameter.Add("SLIP_LIFESOGEN1", item.SLIP_LIFESOGEN1);
            parameter.Add("SLIP_LIFESOGEN2", item.SLIP_LIFESOGEN2);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_RES_BOHUM1> GetAllByWrtno(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, HEIGHT, WEIGHT, BIMAN, EYE_L, EYE_R, EAR_L, EAR_R, BLOOD_H, BLOOD_L, URINE1              ");
            parameter.AppendSql("     , URINE2, URINE3, URINE4, BLOOD1, BLOOD2, BLOOD3, BLOOD4, BLOOD5, BLOOD6, LIVER1, LIVER2          ");
            parameter.AppendSql("     , LIVER3, XRAYGBN, XRAYRES, EKG, CYTO1, CYTO2, EXAMFLAG, SICK11, SICK12, SICK13, SICK21, SICK22   ");
            parameter.AppendSql("     , SICK23, SICK31, SICK32, SICK33, GAJOK1, GAJOK2, GAJOK3, GAJOK4, GAJOK5, GAJOK6, ROSICK          ");
            parameter.AppendSql("     , ROSICKNAME, SIKSENG, DRINK1, DRINK2, SMOKING1, SMOKING2, SMOKING3, SPORTS, ANNOUNE              ");
            parameter.AppendSql("     , WOMAN1, WOMAN2, WOMAN3, MUNJINFLAG, MUNJINENTDATE, MUNJINENTSABUN, OLDBYENG, OLDBYENG1          ");
            parameter.AppendSql("     , OLDBYENG2, OLDBYENG3, OLDBYENG4, OLDBYENG5, OLDBYENG6, OLDBYENG7, HABIT, HABIT1, HABIT2         ");
            parameter.AppendSql("     , HABIT3, HABIT4, HABIT5, JINCHAL1, JINCHAL2, PANJENG, PANJENGB1, PANJENGB2, PANJENGB3            ");
            parameter.AppendSql("     , PANJENGB4, PANJENGB5, PANJENGB6, PANJENGB7, PANJENGB8, PANJENGR1, PANJENGR2, PANJENGR3          ");
            parameter.AppendSql("     , PANJENGR4, PANJENGR5, PANJENGR6, PANJENGR7, PANJENGR8, PANJENGR9, PANJENGR10, PANJENGR11        ");
            parameter.AppendSql("     , PANJENGETC, TO_CHAR(PANJENGDATE,'YYYY-MM-DD') PANJENGDATE, GUNDATE, TONGBOGBN, TONGBODATE       ");
            parameter.AppendSql("     , PANJENGDRNO, SOGEN, XRAYNO                                                                      ");
            parameter.AppendSql("     , IPSADATE, WOMB01, WOMB02, WOMB03, WOMB04, WOMB05, WOMB06, WOMB07, WOMB08, WOMB09, WOMB10        ");
            parameter.AppendSql("     , WOMB11, OLDBYENGNAME, GBPRINT, MUNJINDRNO, JINREMARK, GBPANJENG, PANJENGB9, PANJENGB_ETC        ");
            parameter.AppendSql("     , PANJENGR_ETC, ADDSOGEN, SMOKING4, SMOKING5, PANJENGB_ETC_DTL, WAIST, T_STAT01, T_STAT02         ");
            parameter.AppendSql("     , T_STAT11, T_STAT12, T_STAT21, T_STAT22, T_STAT31, T_STAT32, T_STAT41, T_STAT42, T_STAT51        ");
            parameter.AppendSql("     , T_STAT52, T_GAJOK1, T_GAJOK2, T_GAJOK3, T_GAJOK4, T_GAJOK5, T_BLIVER, T_SMOKE1, T_SMOKE2        ");
            parameter.AppendSql("     , T_SMOKE3, T_SMOKE4, T_SMOKE5, T_DRINK1, T_DRINK2, T_ACTIVE1, T_ACTIVE2, T_ACTIVE3               ");
            parameter.AppendSql("     , T40_FEEL1, T40_FEEL2, T40_FEEL3, T40_FEEL4, T66_INJECT, T66_STAT1, T66_STAT2, T66_STAT3         ");
            parameter.AppendSql("     , T66_STAT4, T66_STAT5, T66_STAT6, T66_FEEL1, T66_FEEL2, T66_FEEL3, T66_MEMORY1, T66_MEMORY2      ");
            parameter.AppendSql("     , T66_MEMORY3, T66_MEMORY4, T66_MEMORY5, T66_FALL, T66_URO, PANJENGC1, PANJENGC2                  ");
            parameter.AppendSql("     , PANJENGC3, PANJENGC4, PANJENGD11, PANJENGD12, PANJENGD13, PANJENGD21, PANJENGD22                ");
            parameter.AppendSql("     , PANJENGD23, PANJENGSAHU, PANJENGC5, SANGDAM, SANGDAM2, GBSIKSA, T40_FEEL, T66_STAT              ");
            parameter.AppendSql("     , FOOT1, FOOT2, BALANCE, OSTEO, LIFESOGEN, T_STAT61, T_STAT62, PANJENGU1, PANJENGU2, PANJENGU3    ");
            parameter.AppendSql("     , PANJENGU4, PANJENGSAHU2, PANJENGSAHU3, WORKYN, PRTSABUN, T_STAT71, T_STAT72, SOGENB             ");
            parameter.AppendSql("     , GBGONGHU, TMUN0001, TMUN0002, TMUN0003, TMUN0004, TMUN0005, TMUN0006, TMUN0007, TMUN0008        ");
            parameter.AppendSql("     , TMUN0009, TMUN0010, TMUN0011, TMUN0012, TMUN0013, TMUN0014, TMUN0015, TMUN0016, TMUN0017        ");
            parameter.AppendSql("     , TMUN0018, TMUN0019, TMUN0020, TMUN0021, TMUN0022, TMUN0023, TMUN0024, TMUN0025, TMUN0026        ");
            parameter.AppendSql("     , TMUN0027, TMUN0028, TMUN0029, TMUN0030, TMUN0031, TMUN0032, TMUN0033, TMUN0034, TMUN0035        ");
            parameter.AppendSql("     , TMUN0036, TMUN0037, TMUN0038, TMUN0039, TMUN0040, TMUN0041, TMUN0042, TMUN0043, TMUN0044        ");
            parameter.AppendSql("     , TMUN0045, TMUN0046, TMUN0047, TMUN0048, TMUN0049, TMUN0050, TMUN0051, TMUN0052, TMUN0053        ");
            parameter.AppendSql("     , TMUN0054, TMUN0055, TMUN0056, TMUN0057, TMUN0058, TMUN0059, TMUN0060, TMUN0061, TMUN0062        ");
            parameter.AppendSql("     , TMUN0063, TMUN0064, TMUN0065, TMUN0066, TMUN0067, TMUN0068, TMUN0069, TMUN0070, TMUN0071        ");
            parameter.AppendSql("     , TMUN0072, TMUN0073, TMUN0074, TMUN0075, TMUN0076, TMUN0077, TMUN0078, TMUN0079, TMUN0080        ");
            parameter.AppendSql("     , TMUN0081, TMUN0082, TMUN0083, TMUN0084, TMUN0085, TMUN0086, TMUN0087, TMUN0088, TMUN0089        ");
            parameter.AppendSql("     , TMUN0090, TMUN0091, TMUN0092, TMUN0093, TMUN0094, TMUN0095, PANJENGC, PANJENGD, SLIP_SMOKE      ");
            parameter.AppendSql("     , SLIP_DRINK, SLIP_ACTIVE, SLIP_FOOD, SLIP_BIMAN, SOGENC, SOGEND, SLIP_PHQ, SLIP_KDSQ             ");
            parameter.AppendSql("     , SLIP_OLDMAN, SLIP_LIFESOGEN1, SLIP_LIFESOGEN2, PANJENGB10, PANJENGR12, TMUN0096                 ");
            parameter.AppendSql("     , TMUN0097, TMUN0098, TMUN0099, TMUN0100, TMUN0101, TMUN0102, TMUN0103, TMUN0104, SIM_RESULT1     ");
            parameter.AppendSql("     , SIM_RESULT2, SIM_RESULT3,ROWID RID                                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1                                                                      ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                           ");
            parameter.AppendSql("   AND WRTNO  = :WRTNO                                                                                 ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReader<HIC_RES_BOHUM1>(parameter);
        }

        public int UpdateGbPanjengbyWrtNo(long fnWrtNo, string strOK)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET      ");
            if (strOK == "OK")
            {
                parameter.AppendSql("       GBPANJENG      = 'Y'            ");
            }
            else
            {
                parameter.AppendSql("       GBPANJENG      = ''             ");
            }
            parameter.AppendSql(" WHERE WRTNO           = :WRTNO            ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public string GetRowIdbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1          ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                     ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateAllbyWrtNo(HIC_RES_BOHUM1 item1)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET  ");
            parameter.AppendSql("       JINCHAL1     = :JINCHAL1        ");
            parameter.AppendSql("     , JINCHAL2     = :JINCHAL2        ");
            parameter.AppendSql("     , HABIT        = :HABIT           ");
            parameter.AppendSql("     , HABIT1       = :HABIT1          ");
            parameter.AppendSql("     , HABIT2       = :HABIT2          ");
            parameter.AppendSql("     , HABIT3       = :HABIT3          ");
            parameter.AppendSql("     , HABIT4       = :HABIT4          ");
            parameter.AppendSql("     , HABIT5       = :HABIT5          ");
            parameter.AppendSql("     , OLDBYENG     = :OLDBYENG        ");
            parameter.AppendSql("     , OLDBYENG1    = :OLDBYENG1       ");
            parameter.AppendSql("     , OLDBYENG2    = :OLDBYENG2       ");
            parameter.AppendSql("     , OLDBYENG3    = :OLDBYENG3       ");
            parameter.AppendSql("     , OLDBYENG4    = :OLDBYENG4       ");
            parameter.AppendSql("     , OLDBYENG5    = :OLDBYENG5       ");
            parameter.AppendSql("     , OLDBYENG6    = :OLDBYENG6       ");
            parameter.AppendSql("     , OLDBYENG7    = :OLDBYENG7       ");
            parameter.AppendSql("     , OLDBYENGNAME = :OLDBYENGNAME    ");
            parameter.AppendSql("     , MUNJINDRNO   = :MUNJINDRNO      ");
            parameter.AppendSql(" WHERE WRTNO        = :WRTNO           ");

            parameter.Add("JINCHAL1", item1.JINCHAL1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JINCHAL2", item1.JINCHAL2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT", item1.HABIT, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT1", item1.HABIT1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT2", item1.HABIT2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT3", item1.HABIT3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT4", item1.HABIT4, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT5", item1.HABIT5, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("OLDBYENG" , item1.OLDBYENG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("OLDBYENG1", item1.OLDBYENG1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("OLDBYENG2", item1.OLDBYENG2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("OLDBYENG3", item1.OLDBYENG3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("OLDBYENG4", item1.OLDBYENG4, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("OLDBYENG5", item1.OLDBYENG5, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("OLDBYENG6", item1.OLDBYENG6, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("OLDBYENG7", item1.OLDBYENG7, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("OLDBYENGNAME", item1.OLDBYENGNAME);
            parameter.Add("MUNJINDRNO", item1.MUNJINDRNO);
            parameter.Add("WRTNO", item1.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateHabitbyWrtNo(HIC_RES_BOHUM1 item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET  ");
            parameter.AppendSql("       HABIT1   = :HABIT1              ");
            parameter.AppendSql("     , HABIT2   = :HABIT2              ");
            parameter.AppendSql("     , HABIT3   = :HABIT3              ");
            parameter.AppendSql("     , HABIT4   = :HABIT4              ");
            parameter.AppendSql("     , HABIT5   = :HABIT5              ");
            parameter.AppendSql("     , T40_FEEL = :T40_FEEL            ");
            parameter.AppendSql("     , T66_STAT = :T66_STAT            ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");

            parameter.Add("HABIT1", item.HABIT1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT2", item.HABIT2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT3", item.HABIT3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT4", item.HABIT4, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT5", item.HABIT5, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T40_FEEL", item.T40_FEEL, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T66_STAT", item.T66_STAT, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateSlipbyWrtNo(string strSLIP1, string strSLIP2, string strSLIP3, string strSLIP4, string strSLIP5, string strSLIP6, string strSLIP7, long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET      ");
            parameter.AppendSql("       SLIP_SMOKE      = :SLIP_SMOKE       ");
            parameter.AppendSql("     , SLIP_DRINK      = :SLIP_DRINK       ");
            parameter.AppendSql("     , SLIP_ACTIVE     = :SLIP_ACTIVE      ");
            parameter.AppendSql("     , SLIP_FOOD       = :SLIP_FOOD        ");
            parameter.AppendSql("     , SLIP_BIMAN      = :SLIP_BIMAN       ");
            parameter.AppendSql("     , SLIP_LIFESOGEN1 = :SLIP_LifeSogen1  ");
            parameter.AppendSql("     , SLIP_LIFESOGEN2 = :SLIP_LifeSogen2  ");
            parameter.AppendSql(" WHERE WRTNO           = :WRTNO            ");
                        
            parameter.Add("SLIP_SMOKE", strSLIP1);
            parameter.Add("SLIP_DRINK", strSLIP2);
            parameter.Add("SLIP_ACTIVE", strSLIP3);
            parameter.Add("SLIP_FOOD", strSLIP4);
            parameter.Add("SLIP_BIMAN", strSLIP5);
            parameter.Add("SLIP_LIFESOGEN1", strSLIP6);
            parameter.Add("SLIP_LIFESOGEN2", strSLIP7);
            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public HIC_RES_BOHUM1 GetIetmbyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT T_SMOKE1,TMUN0003,SLIP_SMOKE,SLIP_DRINK,SLIP_ACTIVE,SLIP_FOOD,SLIP_BIMAN    ");
            parameter.AppendSql("     , SLIP_LIFESOGEN1,SLIP_LIFESOGEN2                                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1                                                  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                             ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReaderSingle<HIC_RES_BOHUM1>(parameter);
        }

        public HIC_RES_BOHUM1 GetItemByWrtnoPanjeng(long nWrtNo, List<string> strDAT, string strChkFirst1, string strChkFirst2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,Panjeng,Panjengb1,Panjengb2,Panjengb3                     ");
            parameter.AppendSql("     , Panjengb4,Panjengb5,Panjengb6,Panjengb7,Panjengb8               ");
            parameter.AppendSql("     , Panjengr1,Panjengr2,Panjengr3,Panjengr4,Panjengr5               ");
            parameter.AppendSql("     , Panjengr6,Panjengr7,Panjengr8,Panjengr9,Panjengr10,Panjengr11   ");
            parameter.AppendSql("     , TO_CHAR(PanjengDate,'YYYY-MM-DD') PanjengDate                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1                                      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                  ");
            if (strDAT.Count > 0)
            {
                parameter.AppendSql("   AND PANJENG IN (:PANJENG)                                       ");
            }
            //정상B 항목 선택
            switch (strChkFirst1)
            {
                case "1":
                    parameter.AppendSql("   AND PANJENGB1 = '1'                                         "); //비만관리
                    break;
                case "2":
                    parameter.AppendSql("   AND PANJENGB2 = '1'                                         "); //혈압
                    break;
                case "3":
                    parameter.AppendSql("   AND PANJENGB3 = '1'                                         "); //콜레스트롤
                    break;
                case "4":
                    parameter.AppendSql("   AND PANJENGB4 = '1'                                         "); //간기능
                    break;
                case "5":
                    parameter.AppendSql("   AND PANJENGB5 = '1'                                         "); //당뇨
                    break;
                case "6":
                    parameter.AppendSql("   AND PANJENGB6 = '1'                                         "); //신장기능
                    break;
                case "7":
                    parameter.AppendSql("   AND PANJENGB7 = '1'                                         "); //빈혈
                    break;
                case "8":
                    parameter.AppendSql("   AND PANJENGB8 = '1'                                         "); //부인과
                    break;
                default:
                    break;
            }
            //질환의심 항목 선택
            switch (strChkFirst2)
            {
                case "01":
                    parameter.AppendSql("   AND PANJENGR1 = '1'                                         "); //폐결핵의심
                    break;
                case "02":
                    parameter.AppendSql("   AND PANJENGR2 = '1'                                         "); //기타흉부
                    break;
                case "03":
                    parameter.AppendSql("   AND PANJENGR3 = '1'                                         "); //고혈압
                    break;
                case "04":
                    parameter.AppendSql("   AND PANJENGR4 = '1'                                         "); //고지혈
                    break;
                case "05":
                    parameter.AppendSql("   AND PANJENGR5 = '1'                                         "); //간장질환
                    break;
                case "06":
                    parameter.AppendSql("   AND PANJENGR6 = '1'                                         "); //당뇨질환
                    break;
                case "07":
                    parameter.AppendSql("   AND PANJENGR7 = '1'                                         "); //신장질환
                    break;
                case "08":
                    parameter.AppendSql("   AND PANJENGR8 = '1'                                         "); //빈혈증
                    break;
                case "09":
                    parameter.AppendSql("   AND PANJENGR9 = '1'                                         "); //부인과질환
                    break;
                case "10":
                    parameter.AppendSql("   AND PANJENGR10 = '1'                                        "); //자궁경부암
                    break;
                case "11":
                    parameter.AppendSql("   AND PANJENGR11 = '1'                                        "); //기타질환
                    break;
                default:
                    break;
            }


            parameter.Add("WRTNO", nWrtNo);
            if (strDAT.Count > 0)
            {
                parameter.AddInStatement("PANJENG", strDAT, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReaderSingle<HIC_RES_BOHUM1>(parameter);
        }

        public HIC_RES_BOHUM1 GetPanjengDatebyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GbPanjeng,TO_CHAR(PanjengDate,'YYYY-MM-DD') PanjengDate     ");
            parameter.AppendSql("     , TO_CHAR(TongboDate,'YYYY-MM-DD') TongboDate                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1                                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_RES_BOHUM1>(parameter);
        }

        public HIC_RES_BOHUM1 GetMunjinDrNobyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MunjinDrno                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_RES_BOHUM1>(parameter);
        }

        public int UpdateMunjinDrNobyWrtNo(long gnHicLicense, long nWRTNO, string argMunDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET      ");
            
            if (gnHicLicense > 0)
            {
                parameter.AppendSql("       MUNJINDRNO = :MUNJINDRNO            ");

                if (!argMunDate.IsNullOrEmpty())
                {
                    parameter.AppendSql("     , MUNJINENTDATE = TO_DATE(:MUNJINENTDATE, 'YYYY-MM-DD')            ");
                }
            }
            else
            {
                parameter.AppendSql("       MUNJINENTDATE = ''            ");
                parameter.AppendSql("     , MUNJINDRNO = 0            ");
            }
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO                 ");

            if (gnHicLicense > 0)
            {
                parameter.Add("MUNJINDRNO", gnHicLicense);

                if (!argMunDate.IsNullOrEmpty())
                {
                    parameter.Add("MUNJINENTDATE", argMunDate);
                }
            }

            parameter.Add("WRTNO", nWRTNO);
            
            return ExecuteNonQuery(parameter);
        }

        public string GetCountLife1thbyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SOGEN                                                                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1                                                              ");
            parameter.AppendSql(" WHERE WRTNO IN (                                                                              ");
            parameter.AppendSql("             SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU                                           ");
            parameter.AppendSql("                   WHERE (PANO,GjYear) IN (SELECT PANO,GjYear FROM KOSMOS_PMPA.HIC_JEPSU       ");
            parameter.AppendSql("                                      WHERE WRTNO = :WRTNO )                                   ");
            parameter.AppendSql("            AND GJJONG IN ('41','42','43')                                                     ");
            parameter.AppendSql("      AND DELDATE IS NULL                                                                      ");
            parameter.AppendSql("      AND GBSTS <> 'D'                                                                         ");
            parameter.AppendSql("      AND GJCHASU ='1' )                                                                       ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int GetCountbyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1  ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND WRTNO  = :WRTNO             ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateOldByengbyWrtNo(long wRTNO, string strOldByeng1)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET  ");
            parameter.AppendSql("       OLDBYENG = :OLDBYENG            ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");

            parameter.Add("WRTNO", wRTNO);
            parameter.Add("OLDBYENG", strOldByeng1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateResultbyWrtNo(HIC_RES_BOHUM1 item, string argGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET      ");
            parameter.AppendSql("       T_STAT01   = :T_STAT01              ");
            parameter.AppendSql("     , T_STAT02   = :T_STAT02              ");
            parameter.AppendSql("     , T_STAT11   = :T_STAT11              ");
            parameter.AppendSql("     , T_STAT12   = :T_STAT12              ");
            parameter.AppendSql("     , T_STAT21   = :T_STAT21              ");
            parameter.AppendSql("     , T_STAT22   = :T_STAT22              ");
            parameter.AppendSql("     , T_STAT31   = :T_STAT31              ");
            parameter.AppendSql("     , T_STAT32   = :T_STAT32              ");
            parameter.AppendSql("     , T_STAT41   = :T_STAT41              ");
            parameter.AppendSql("     , T_STAT42   = :T_STAT42              ");
            parameter.AppendSql("     , T_STAT51   = :T_STAT51              ");
            parameter.AppendSql("     , T_STAT52   = :T_STAT52              ");
            parameter.AppendSql("     , T_STAT61   = :T_STAT61              ");
            parameter.AppendSql("     , T_STAT62   = :T_STAT62              ");
            parameter.AppendSql("     , T_GAJOK1   = :T_GAJOK1              ");
            parameter.AppendSql("     , T_GAJOK2   = :T_GAJOK2              ");
            parameter.AppendSql("     , T_GAJOK3   = :T_GAJOK3              ");
            parameter.AppendSql("     , T_GAJOK4   = :T_GAJOK4              ");
            parameter.AppendSql("     , T_GAJOK5   = :T_GAJOK5              ");
            parameter.AppendSql("     , T_BLIVER   = :T_BLIVER              ");
            parameter.AppendSql("     , T_SMOKE1   = :T_SMOKE1              ");
            parameter.AppendSql("     , T_SMOKE2   = :T_SMOKE2              ");
            parameter.AppendSql("     , T_SMOKE3   = :T_SMOKE3              ");
            parameter.AppendSql("     , T_SMOKE4   = :T_SMOKE4              ");
            parameter.AppendSql("     , T_SMOKE5   = :T_SMOKE5              ");
            parameter.AppendSql("     , T_DRINK1   = :T_DRINK1              ");
            parameter.AppendSql("     , T_DRINK2   = :T_DRINK2              ");
            parameter.AppendSql("     , T_ACTIVE1  = :T_ACTIVE1             ");
            parameter.AppendSql("     , T_ACTIVE2  = :T_ACTIVE2             ");
            parameter.AppendSql("     , T_ACTIVE3  = :T_ACTIVE3             ");
            parameter.AppendSql("     , T66_INJECT = :T66_INJECT            ");
            parameter.AppendSql("     , T66_STAT1  = :T66_STAT1             ");
            parameter.AppendSql("     , T66_STAT2  = :T66_STAT2             ");
            parameter.AppendSql("     , T66_STAT3  = :T66_STAT3             ");
            parameter.AppendSql("     , T66_STAT4  = :T66_STAT4             ");
            parameter.AppendSql("     , T66_STAT5  = :T66_STAT5             ");
            parameter.AppendSql("     , T66_STAT6  = :T66_STAT6             ");
            parameter.AppendSql("     , T66_FALL   = :T66_FALL              ");
            parameter.AppendSql("     , T66_URO    = :T66_URO               ");
            parameter.AppendSql("     , TMUN0001   = :TMUN0001              ");
            parameter.AppendSql("     , TMUN0002   = :TMUN0002              ");
            parameter.AppendSql("     , TMUN0003   = :TMUN0003              ");
            parameter.AppendSql("     , TMUN0004   = :TMUN0004              ");
            parameter.AppendSql("     , TMUN0005   = :TMUN0005              ");
            parameter.AppendSql("     , TMUN0006   = :TMUN0006              ");
            parameter.AppendSql("     , TMUN0007   = :TMUN0007              ");
            parameter.AppendSql("     , TMUN0008   = :TMUN0008              ");
            parameter.AppendSql("     , TMUN0009   = :TMUN0009              ");
            parameter.AppendSql("     , TMUN0010   = :TMUN0010              ");
            parameter.AppendSql("     , TMUN0011   = :TMUN0011              ");
            parameter.AppendSql("     , TMUN0012   = :TMUN0012              ");
            parameter.AppendSql("     , TMUN0013   = :TMUN0013              ");
            //2019년도 변경사항
            parameter.AppendSql("     , TMUN0096   = :TMUN0096              ");
            parameter.AppendSql("     , TMUN0097   = :TMUN0097              ");
            parameter.AppendSql("     , TMUN0098   = :TMUN0098              ");
            parameter.AppendSql("     , TMUN0099   = :TMUN0099              ");
            parameter.AppendSql("     , TMUN0100   = :TMUN0100              ");
            parameter.AppendSql("     , TMUN0101   = :TMUN0101              ");
            parameter.AppendSql("     , TMUN0102   = :TMUN0102              ");
            parameter.AppendSql("     , TMUN0103   = :TMUN0103              ");
            parameter.AppendSql("     , TMUN0104   = :TMUN0104              ");
            //2022-02-03(보통,최대음주 추가)
            parameter.AppendSql("     , TMUN0125   = :TMUN0125              ");
            parameter.AppendSql("     , TMUN0126   = :TMUN0126              ");
            parameter.AppendSql("     , TMUN0127   = :TMUN0127              ");
            parameter.AppendSql("     , TMUN0128   = :TMUN0128              ");

            if (argGbn == "저장")
            {
                parameter.AppendSql("     , MUNJINFLAG = 'Y'                ");
            }
            else
            {
                parameter.AppendSql("     , MUNJINFLAG = 'N'                ");
            }
            parameter.AppendSql("     , MUNJINENTDATE  = SYSDATE            ");
            parameter.AppendSql("     , MUNJINENTSABUN = :SABUN             ");
            parameter.AppendSql(" WHERE WRTNO          = :WRTNO             ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("T_STAT01", item.T_STAT01, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT02", item.T_STAT02, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT11", item.T_STAT11, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT12", item.T_STAT12, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT21", item.T_STAT21, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT22", item.T_STAT22, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT31", item.T_STAT31, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT32", item.T_STAT32, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT41", item.T_STAT41, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT42", item.T_STAT42, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT51", item.T_STAT51, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT52", item.T_STAT52, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT61", item.T_STAT61, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT62", item.T_STAT62, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_GAJOK1", item.T_GAJOK1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_GAJOK2", item.T_GAJOK2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_GAJOK3", item.T_GAJOK3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_GAJOK4", item.T_GAJOK4, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_GAJOK5", item.T_GAJOK5, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_BLIVER", item.T_BLIVER, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_SMOKE1", item.T_SMOKE1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_SMOKE2", item.T_SMOKE2);
            parameter.Add("T_SMOKE3", item.T_SMOKE3);
            parameter.Add("T_SMOKE4", item.T_SMOKE4);
            parameter.Add("T_SMOKE5", item.T_SMOKE5);
            parameter.Add("T_DRINK1", item.T_DRINK1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_DRINK2", item.T_DRINK2);
            parameter.Add("T_ACTIVE1", item.T_ACTIVE1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_ACTIVE2", item.T_ACTIVE2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T_ACTIVE3", item.T_ACTIVE3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T66_INJECT", item.T66_INJECT, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T66_STAT1", item.T66_STAT1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T66_STAT2", item.T66_STAT2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T66_STAT3", item.T66_STAT3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T66_STAT4", item.T66_STAT4, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T66_STAT5", item.T66_STAT5, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T66_STAT6", item.T66_STAT6, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T66_FALL", item.T66_FALL, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("T66_URO", item.T66_URO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("TMUN0001", item.TMUN0001);
            parameter.Add("TMUN0002", item.TMUN0002);
            parameter.Add("TMUN0003", item.TMUN0003);
            parameter.Add("TMUN0004", item.TMUN0004);
            parameter.Add("TMUN0005", item.TMUN0005);
            parameter.Add("TMUN0006", item.TMUN0006);
            parameter.Add("TMUN0007", item.TMUN0007);
            parameter.Add("TMUN0008", item.TMUN0008);
            parameter.Add("TMUN0009", item.TMUN0009);
            parameter.Add("TMUN0010", item.TMUN0010);
            parameter.Add("TMUN0011", item.TMUN0011);
            parameter.Add("TMUN0012", item.TMUN0012);
            parameter.Add("TMUN0013", item.TMUN0013);
            parameter.Add("TMUN0096", item.TMUN0096);
            parameter.Add("TMUN0097", item.TMUN0097);
            parameter.Add("TMUN0098", item.TMUN0098);
            parameter.Add("TMUN0099", item.TMUN0099);
            parameter.Add("TMUN0100", item.TMUN0100);
            parameter.Add("TMUN0101", item.TMUN0101);
            parameter.Add("TMUN0102", item.TMUN0102);
            parameter.Add("TMUN0103", item.TMUN0103);
            parameter.Add("TMUN0104", item.TMUN0104);
            parameter.Add("TMUN0125", item.TMUN0125);
            parameter.Add("TMUN0126", item.TMUN0126);
            parameter.Add("TMUN0127", item.TMUN0127);
            parameter.Add("TMUN0128", item.TMUN0128);
            parameter.Add("SABUN", item.SABUN); 

            return ExecuteNonQuery(parameter);
        }

        public int Insert(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_RES_BOHUM1     ");
            parameter.AppendSql("       (WRTNO)                             ");
            parameter.AppendSql("VALUES                                     ");
            parameter.AppendSql("       (:WRTNO)                            ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int DeletebyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_RES_BOHUM1  ");            
            parameter.AppendSql(" WHERE WRTNO  =:WRTNO              ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateLiverCbyWrtNo(HIC_RES_BOHUM1 item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET  ");
            parameter.AppendSql("       TMUN0105     = :TMUN0105        ");
            parameter.AppendSql("       ,TMUN0106     = :TMUN0106        ");
            parameter.AppendSql("       ,TMUN0107     = :TMUN0107        ");
            parameter.AppendSql("       ,TMUN0108     = :TMUN0108        ");
            parameter.AppendSql("       ,TMUN0109     = :TMUN0109        ");
            parameter.AppendSql("       ,TMUN0110     = :TMUN0110        ");
            parameter.AppendSql("       ,TMUN0111     = :TMUN0111        ");
            parameter.AppendSql("       ,TMUN0112     = :TMUN0112        ");
            parameter.AppendSql("       ,TMUN0113     = :TMUN0113        ");
            parameter.AppendSql("       ,TMUN0114     = :TMUN0114        ");
            parameter.AppendSql("       ,TMUN0115     = :TMUN0115        ");
            parameter.AppendSql("       ,TMUN0116     = :TMUN0116        ");
            parameter.AppendSql("       ,TMUN0117     = :TMUN0117        ");
            parameter.AppendSql("       ,TMUN0118     = :TMUN0118        ");
            parameter.AppendSql("       ,TMUN0119     = :TMUN0119        ");
            parameter.AppendSql("       ,TMUN0120     = :TMUN0120        ");
            parameter.AppendSql("       ,TMUN0121     = :TMUN0121        ");
            parameter.AppendSql("       ,TMUN0122     = :TMUN0122        ");
            parameter.AppendSql("       ,TMUN0123     = :TMUN0123        ");
            parameter.AppendSql("       ,TMUN0124     = :TMUN0124        ");
            parameter.AppendSql(" WHERE WRTNO        = :WRTNO           ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("TMUN0105", item.TMUN0105);
            parameter.Add("TMUN0106", item.TMUN0106);
            parameter.Add("TMUN0107", item.TMUN0107);
            parameter.Add("TMUN0108", item.TMUN0108);
            parameter.Add("TMUN0109", item.TMUN0109);
            parameter.Add("TMUN0110", item.TMUN0110);
            parameter.Add("TMUN0111", item.TMUN0111);
            parameter.Add("TMUN0112", item.TMUN0112);
            parameter.Add("TMUN0113", item.TMUN0113);
            parameter.Add("TMUN0114", item.TMUN0114);
            parameter.Add("TMUN0115", item.TMUN0115);
            parameter.Add("TMUN0116", item.TMUN0116);
            parameter.Add("TMUN0117", item.TMUN0117);
            parameter.Add("TMUN0118", item.TMUN0118);
            parameter.Add("TMUN0119", item.TMUN0119);
            parameter.Add("TMUN0120", item.TMUN0120);
            parameter.Add("TMUN0121", item.TMUN0121);
            parameter.Add("TMUN0122", item.TMUN0122);
            parameter.Add("TMUN0123", item.TMUN0123);
            parameter.Add("TMUN0124", item.TMUN0124);

            return ExecuteNonQuery(parameter);
        }


        public string GetItemBywrtno(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1      ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                 ");
            parameter.AppendSql(" AND TMUN0105 IS NOT NULL              ");
            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_RES_BOHUM1 GetTongBoPanjengDateByWrtno(long argWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(TONGBODATE,'YYYY-MM-DD') TONGBODATE     ");
            parameter.AppendSql("     , TO_CHAR(PANJENGDATE,'YYYY-MM-DD') PANJENGDATE   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1                      ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                 ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteReaderSingle<HIC_RES_BOHUM1>(parameter);
        }

    }
}
