namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicResSpecialRepository : BaseRepository
    {   
        /// <summary>
        /// 
        /// </summary>
        public HicResSpecialRepository()
        {
        }
        
        public string Read_Res_Special(long WRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                   ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_RES_SPECIAL ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", WRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int Delete(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_RES_SPECIAL    ");
            parameter.AppendSql("  WHERE WRTNO = :argWRTNO              ");

            #region Query 변수대입
            parameter.Add("argWRTNO", argWRTNO);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public HIC_RES_SPECIAL GetItemByWrtno(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, JEPDATE, MUNJINDATE, PANJENGDATE, PANJENGDRNO, UCODECNT, UCODENAME, SEXAMNAME, GBPRINT           ");
            parameter.AppendSql("     , GBPANJENGR, GBSTS, GBSPC, GBOHMS, SABUN, BUSE, HNAME, IPSADATE, SUCHUPYN, GONGJENG, JIKJONG             ");
            parameter.AppendSql("     , JENIPDATE, PGIGAN_YY, PGIGAN_MM, PTIME, OLDGONG1, OLDMCODE1, OLDYEAR1, OLDDAYTIME1, OLDGONG2            ");
            parameter.AppendSql("     , OLDMCODE2, OLDYEAR2, OLDDAYTIME2, OLDGONG3, OLDMCODE3, OLDYEAR3, OLDDAYTIME3, HABIT1, HABIT2, HABIT3    ");
            parameter.AppendSql("     , HABIT4, HABIT5, GBHUYU, GBSANGTAE, OLDMYEAR1, OLDMYEAR2, OLDMYEAR3, OLDMYEAR4, OLDMYEAR5, OLDETCMSYM    ");
            parameter.AppendSql("     , MUN_GAJOK, MUN_GIINSUNG, JIN_NEURO, JIN_HEAD, JIN_SKIN, JIN_CHEST, JENGSANG, MUNJIN_A, MUNJIN_B         ");
            parameter.AppendSql("     , MUNJIN_C, MUNJIN_D, MUNJIN_E, MUNJIN_F, MUNJIN_G, MUNJIN_H, MUNJIN_I, MUNJIN_J, MUNJIN_K, MUNJIN_L      ");
            parameter.AppendSql("     , MUNJIN_M, MUNJIN_N, HEIGHT, WEIGHT, BIMAN, ABORH, GUNJINGBN, DENTSOGEN, DENTDOCT, XRAYGBN, XRAYNO       ");
            parameter.AppendSql("     , XRAYNO2, JINDRNO, JINREMARK, TONGBODATE, MUNJINT_A, MUNJINT_B, MUNJINT_C, MUNJINT_D, MUNJINT_E          ");
            parameter.AppendSql("     , MUNJINT_F, MUNJINT_G, MUNJINT_H, MUNJINT_I, MUNJINT_J, MUNJINT_K, HSTAT, MCODE_STAT, GBCAPACITY         ");
            parameter.AppendSql("     , OLDJILHWAN, OLDJILHWAN_ETC, GBSUSUL, GBSUSUL_ETC, GBDRUG, GBDRUG_ETC, SMOKEYEAR, GBSMOKE, GBDENTY       ");
            parameter.AppendSql("     , DYSPNEA, SANGDAM, GBSIKSA, ADDSOGEN, OLDPGIGAN1, OLDPGIGAN2, OLDPGIGAN3, PRTSABUN, NATIONAL             ");
            parameter.AppendSql("     , MUN_OLDMSYM, GBGEUMGI, GBSMOKE1, POSTURE, SERIALNO, COOPERATE, PSOGEN, PETCSOGEN, PFTDATE, PFTSABUN     ");
            parameter.AppendSql("     , BMI, ROWID RID                                                                                          ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_RES_SPECIAL                                                                             ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                                          ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReaderSingle<HIC_RES_SPECIAL>(parameter);
        }

        public int UpdateGbOhmabyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET     ");
            parameter.AppendSql("       GbOHMS = 'Y'                        ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                     ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public void UpDaterJinDrnoByWrtno(long nDrNO, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET     ");
            if (nDrNO > 0)
            {
                parameter.AppendSql("       JINDRNO = :JINDRNO              ");
            }
            else
            {
                parameter.AppendSql("       JINDRNO = 0                     ");
            }
            
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                     ");

            if (nDrNO > 0)
            {
                parameter.Add("JINDRNO", nDrNO);
            }
            
            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public int UpdateAllbyWrtNo(HIC_RES_SPECIAL item2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET     ");
            parameter.AppendSql("       GBHUYU       = :GBHUYU              "); 
            parameter.AppendSql("     , GBSANGTAE    = :GBSANGTAE           ");
            parameter.AppendSql("     , HABIT1       = :HABIT1              ");
            parameter.AppendSql("     , HABIT2       = :HABIT2              ");
            parameter.AppendSql("     , HABIT3       = :HABIT3              ");
            parameter.AppendSql("     , HABIT4       = :HABIT4              ");
            parameter.AppendSql("     , HABIT5       = :HABIT5              ");
            parameter.AppendSql("     , MUN_OLDMSYM  = :MUN_OLDMSYM         ");
            parameter.AppendSql("     , MUN_GAJOK    = :MUN_GAJOK           ");
            parameter.AppendSql("     , MUN_GIINSUNG = :MUN_GIINSUNG        ");
            parameter.AppendSql("     , JIN_NEURO    = :JIN_NEURO           ");
            parameter.AppendSql("     , JIN_HEAD     = :JIN_HEAD            ");
            parameter.AppendSql("     , JIN_SKIN     = :JIN_SKIN            ");
            parameter.AppendSql("     , JIN_CHEST    = :JIN_CHEST           ");
            parameter.AppendSql("     , JENGSANG     = :JENGSANG            ");
            parameter.AppendSql("     , JINDRNO      = :JINDRNO             ");
            parameter.AppendSql(" WHERE WRTNO        = :WRTNO               ");

            parameter.Add("GBHUYU", item2.GBHUYU, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSANGTAE", item2.GBSANGTAE);
            parameter.Add("HABIT1", item2.HABIT1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT2", item2.HABIT2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT3", item2.HABIT3, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT4", item2.HABIT4, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT5", item2.HABIT5, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("MUN_OLDMSYM", item2.MUN_OLDMSYM);
            parameter.Add("MUN_GAJOK", item2.MUN_GAJOK);
            parameter.Add("MUN_GIINSUNG", item2.MUN_GIINSUNG);
            parameter.Add("JIN_NEURO", item2.JIN_NEURO);
            parameter.Add("JIN_HEAD", item2.JIN_HEAD);
            parameter.Add("JIN_SKIN", item2.JIN_SKIN);
            parameter.Add("JIN_CHEST", item2.JIN_CHEST);
            parameter.Add("JENGSANG", item2.JENGSANG);
            parameter.Add("JINDRNO", item2.JINDRNO);
            parameter.Add("WRTNO", item2.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public void UpdatePanjengInfobyWrtNo(long fnWRTNO, string strDate, long nDrNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET                      ");
            if (nDrNO > 0)
            {
                parameter.AppendSql("     PANJENGDATE = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')   ");
            }
            else
            {
                parameter.AppendSql("     PANJENGDATE = ''   ");
            }

            parameter.AppendSql("     , PANJENGDRNO = :PANJENGDRNO                          ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                     ");

            if (nDrNO > 0)
            {
                parameter.Add("PANJENGDATE", strDate);
            }

            parameter.Add("PANJENGDRNO", nDrNO);
            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public void UpDateGbSpcByWrtno(string argGbSpc, long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL        ");
            parameter.AppendSql("   SET GBSPC =:GBSPC                      ");
            parameter.AppendSql(" WHERE WRTNO =:WRTNO                     ");

            parameter.Add("GBSPC", argGbSpc);
            parameter.Add("WRTNO", nWRTNO);

            ExecuteNonQuery(parameter);
        }

        public int InsertWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_RES_SPECIAL        ");
            parameter.AppendSql("       (WRTNO)                                 ");
            parameter.AppendSql("VALUES                                         ");
            parameter.AppendSql("       (:WRTNO)                                ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public void UpDateMunjinbyItem(HIC_RES_SPECIAL nHRS)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET     ");
            parameter.AppendSql("       GBOHMS      = :GBOHMS             ");
            parameter.AppendSql("     , GBSPC       = :GBSPC              ");
            parameter.AppendSql("     , UCODECNT    = :UCODECNT           ");
            parameter.AppendSql("     , SABUN       = :SABUN              ");
            parameter.AppendSql("     , BUSE        = :BUSE               ");
            parameter.AppendSql("     , IPSADATE    = TO_DATE(:IPSADATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("     , SUCHUPYN    = :SUCHUPYN           ");
            parameter.AppendSql("     , GONGJENG    = :GONGJENG           ");
            parameter.AppendSql("     , JIKJONG     = :JIKJONG            ");
            parameter.AppendSql("     , JENIPDATE   = TO_DATE(:JENIPDATE, 'YYYY-MM-DD')          ");
            parameter.AppendSql("     , PTIME       = :PTIME              ");
            parameter.AppendSql("     , OLDGONG1    = :OLDGONG1           ");
            parameter.AppendSql("     , OLDMCODE1   = :OLDMCODE1          ");
            parameter.AppendSql("     , OLDYEAR1    = :OLDYEAR1           ");
            parameter.AppendSql("     , OLDDAYTIME1 = :OLDDAYTIME1        ");
            parameter.AppendSql("     , NATIONAL    = :NATIONAL           ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO               ");

            parameter.Add("GBOHMS",     nHRS.GBOHMS);
            parameter.Add("GBSPC",      nHRS.GBSPC);
            parameter.Add("UCODECNT",   nHRS.UCODECNT);
            parameter.Add("SABUN",      nHRS.SABUN);
            parameter.Add("BUSE",       nHRS.BUSE);
            parameter.Add("IPSADATE",   nHRS.IPSADATE);
            parameter.Add("SUCHUPYN",   nHRS.SUCHUPYN);
            parameter.Add("GONGJENG",   nHRS.GONGJENG);
            parameter.Add("JIKJONG",    nHRS.JIKJONG);
            parameter.Add("JENIPDATE",  nHRS.JENIPDATE);
            parameter.Add("PTIME",      nHRS.PTIME);
            parameter.Add("OLDGONG1",   nHRS.OLDGONG1);
            parameter.Add("OLDMCODE1",  nHRS.OLDMCODE1);
            parameter.Add("OLDYEAR1",   nHRS.OLDYEAR1);
            parameter.Add("OLDDAYTIME1",nHRS.OLDDAYTIME1);
            parameter.Add("NATIONAL",   nHRS.NATIONAL);
            parameter.Add("WRTNO",      nHRS.WRTNO);

            ExecuteNonQuery(parameter);
        }

        public int UpdateWrtNobyFWrtNo(long nWrtNo, long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET     ");
            parameter.AppendSql("       WRTNO = :WRTNO                      ");
            parameter.AppendSql(" WHERE WRTNO = :FWRTNO                     ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("FWRTNO", fnWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateUcodesbyRowId(string strRowId, string strUCodes)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET     ");
            parameter.AppendSql("       UCODENAME = :UCODENAME              ");
            parameter.AppendSql(" WHERE ROWID     = :RID                    ");

            parameter.Add("RID", strRowId);
            parameter.Add("UCODENAME", strUCodes);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatePanjengDrNoDatebyWrtNo(long fnWrtNo, long nPanDrNo1)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET     ");
            parameter.AppendSql("       PANJENGDRNO = :PANJENGDRNO          ");
            parameter.AppendSql("     , PANJENGDATE = TRUNC(SYSDATE)        ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                   ");

            parameter.Add("PANJENGDRNO", nPanDrNo1);            
            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatePanjengDatebyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET     ");
            parameter.AppendSql("       PANJENGDATE = TRUNC(SYSDATE)        ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                   ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public HIC_RES_SPECIAL GetPanjengDrNoDatebyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANJENGDRNO,TO_CHAR(PANJENGDATE,'YYYY-MM-DD') PANJENGDATE   ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_RES_SPECIAL                                 ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                              ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReaderSingle<HIC_RES_SPECIAL>(parameter);
        }

        public int UPdateSpecialbyWrtNo(HIC_RES_SPECIAL item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET     ");
            parameter.AppendSql("       WEIGHT          = :WEIGHT           "); 
            parameter.AppendSql("     , HEIGHT          = :HEIGHT           ");
            parameter.AppendSql("     , GBCAPACITY      = :GBCAPACITY       ");
            parameter.AppendSql("     , OLDJILHWAN      = :OLDJILHWAN       ");
            parameter.AppendSql("     , OLDJILHWAN_ETC  = :OLDJILHWAN_ETC   ");
            parameter.AppendSql("     , GBSUSUL         = :GBSUSUL          ");
            parameter.AppendSql("     , GBSUSUL_ETC     = :GBSUSUL_ETC      ");
            parameter.AppendSql("     , GBDRUG          = :GBDRUG           ");
            parameter.AppendSql("     , GBDRUG_ETC      = :GBDRUG_ETC       ");
            parameter.AppendSql("     , GBSMOKE         = :GBSMOKE          ");
            parameter.AppendSql("     , SMOKEYEAR       = :SMOKEYEAR        ");
            parameter.AppendSql("     , GBDENTY         = :GBDENTY          ");
            parameter.AppendSql("     , DYSPNEA         = :DYSPNEA          ");
            parameter.AppendSql("     , GBGEUMGI        = :GBGEUMGI         ");
            parameter.AppendSql("     , GBSMOKE1        = :GBSMOKE1         ");
            parameter.AppendSql("     , POSTURE         = :POSTURE          ");
            parameter.AppendSql("     , SERIALNO        = :SERIALNO         ");
            parameter.AppendSql("     , COOPERATE       = :COOPERATE        ");
            parameter.AppendSql("     , PSOGEN          = :PSOGEN           ");
            parameter.AppendSql("     , PETCSOGEN       = :PETCSOGEN        ");
            parameter.AppendSql("     , PFTDATE         = :PFTDATE          ");
            parameter.AppendSql("     , PFTSABUN        = :PFTSABUN         ");
            parameter.AppendSql("     , BMI             = :BMI              ");
            parameter.AppendSql(" WHERE WRTNO           = :WRTNO            ");

            parameter.Add("WEIGHT", item.WEIGHT);
            parameter.Add("HEIGHT", item.HEIGHT);
            parameter.Add("GBCAPACITY", item.GBCAPACITY);
            parameter.Add("OLDJILHWAN", item.OLDJILHWAN);
            parameter.Add("OLDJILHWAN_ETC", item.OLDJILHWAN_ETC);
            parameter.Add("GBSUSUL", item.GBSUSUL);
            parameter.Add("GBSUSUL_ETC", item.GBSUSUL_ETC);
            parameter.Add("GBDRUG", item.GBDRUG);
            parameter.Add("GBDRUG_ETC", item.GBDRUG_ETC);
            parameter.Add("SMOKEYEAR", item.SMOKEYEAR);
            parameter.Add("GBSMOKE", item.GBSMOKE);
            parameter.Add("GBDENTY", item.GBDENTY);
            parameter.Add("DYSPNEA", item.DYSPNEA);
            parameter.Add("GBGEUMGI", item.GBGEUMGI);
            parameter.Add("GBSMOKE1", item.GBSMOKE1);
            parameter.Add("POSTURE", item.POSTURE);
            parameter.Add("SERIALNO", item.SERIALNO);
            parameter.Add("COOPERATE", item.COOPERATE);
            parameter.Add("PSOGEN", item.PSOGEN);
            parameter.Add("PETCSOGEN", item.PETCSOGEN);
            parameter.Add("PFTDATE", item.PFTDATE);
            parameter.Add("PFTSABUN", item.PFTSABUN);
            parameter.Add("BMI", item.BMI);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                      ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_RES_SPECIAL         ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<int>(parameter);
        }

        public HIC_RES_SPECIAL GetDentSogenbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DENTSOGEN, DENTDOCT                 ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_RES_SPECIAL         ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_RES_SPECIAL>(parameter);
        }

        public int Insert(HIC_RES_SPECIAL nHRS)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO HIC_RES_SPECIAL                                                                            ");
            parameter.AppendSql(" (WRTNO, JEPDATE, UCODENAME, SEXAMNAME, SABUN                                                          ");
            parameter.AppendSql(" ,BUSE, IPSADATE, SUCHUPYN, JIKJONG, JENIPDATE)                                                        ");
            parameter.AppendSql(" VALUES (                                                                                              ");
            parameter.AppendSql(" :WRTNO, TO_DATE(:JEPDATE,'YYYY-MM-DD'), :UCODENAME, :SEXAMNAME, :SABUN                                ");
            parameter.AppendSql(" ,:BUSENAME, TO_DATE(:IPSADATE,'YYYY-MM-DD'), :GBSUCHEP, :JIKJONG, TO_DATE(:BUSEIPSA,'YYYY-MM-DD'))    ");

            #region Query 변수대입
            parameter.Add("WRTNO", nHRS.WRTNO);
            parameter.Add("JEPDATE", nHRS.JEPDATE);
            parameter.Add("UCODENAME", nHRS.UCODENAME);
            parameter.Add("SEXAMNAME", nHRS.SEXAMNAME);
            parameter.Add("SABUN", nHRS.SABUN);
            parameter.Add("BUSENAME", nHRS.BUSE);
            parameter.Add("IPSADATE", nHRS.IPSADATE);
            parameter.Add("GBSUCHEP", nHRS.SUCHUPYN);
            parameter.Add("JIKJONG", nHRS.JIKJONG);
            parameter.Add("BUSEIPSA", nHRS.JENIPDATE);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public string GetRowIdbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                               ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_RES_SPECIAL         ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdatebyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET     ");
            parameter.AppendSql("       GbOHMS = ''                         ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                     ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public HIC_RES_SPECIAL GetPanjengDatebyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(PANJENGDATE,'YYYY-MM-DD') PANJENGDATE   ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_RES_SPECIAL                     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_RES_SPECIAL>(parameter);
        }

        public string GetGbOhmsbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GbOHMS                      ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_RES_SPECIAL ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_RES_SPECIAL GetItembyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MUNJIN_A,MUNJIN_B,MUNJIN_C,MUNJIN_D,MUNJIN_E,MUNJIN_F,MUNJIN_G              ");
            parameter.AppendSql("     , MUNJIN_H,MUNJIN_I,MUNJIN_J,MUNJIN_K,MUNJIN_L,MUNJIN_M,MUNJIN_N              ");
            parameter.AppendSql("     , HEIGHT,WEIGHT,GBCAPACITY,OLDJILHWAN,OLDJILHWAN_ETC,GBSUSUL,GBSUSUL_ETC      ");
            parameter.AppendSql("     , GBDRUG,GBDRUG_ETC,SMOKEYEAR,GBSMOKE,GBDENTY,DYSPNEA, GBGEUMGI, GBSMOKE1     ");
            parameter.AppendSql("     , POSTURE, SERIALNO, COOPERATE, PSOGEN, PETCSOGEN                             ");
            parameter.AppendSql("     , TO_CHAR(PFTDATE, 'YYYY-MM-DD') PFTDATE, PFTSABUN, BMI                       ");
            parameter.AppendSql("     , TO_CHAR(MunjinDate, 'YYYY-MM-DD') MUNJINDATE, GBSPC                         ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_RES_SPECIAL                                                 "); 
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                              ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReaderSingle<HIC_RES_SPECIAL>(parameter);
        }

        public int UpdateLungbyWrtNo(HIC_RES_SPECIAL item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET         ");
            parameter.AppendSql("       WEIGHT         = :WEIGHT                ");
            parameter.AppendSql("     , HEIGHT         = :HEIGHT                ");
            parameter.AppendSql("     , GBCAPACITY     = :GBCAPACITY            ");
            parameter.AppendSql("     , OLDJILHWAN     = :OLDJILHWAN            ");
            parameter.AppendSql("     , OLDJILHWAN_ETC = :OLDJILHWAN_ETC        ");
            parameter.AppendSql("     , GBSUSUL        = :GBSUSUL               ");
            parameter.AppendSql("     , GBSUSUL_ETC    = :GBSUSUL_ETC           ");
            parameter.AppendSql("     , GBDRUG         = :GBDRUG                ");
            parameter.AppendSql("     , GBDRUG_ETC     = :GBDRUG_ETC            ");
            parameter.AppendSql("     , SMOKEYEAR      = :SMOKEYEAR             ");
            parameter.AppendSql("     , GBSMOKE        = :GBSMOKE               ");
            parameter.AppendSql("     , GBDENTY        = :GBDENTY               ");
            parameter.AppendSql("     , DYSPNEA        = :DYSPNEA               ");
            parameter.AppendSql("     , GBGEUMGI       = :GBGEUMGI              ");
            parameter.AppendSql("     , GBSMOKE1       = :GBSMOKE1              ");
            parameter.AppendSql(" WHERE WRTNO          = :WRTNO                 ");

            parameter.Add("WEIGHT", item.WEIGHT);
            parameter.Add("HEIGHT", item.HEIGHT);
            parameter.Add("GBCAPACITY", item.GBCAPACITY);
            parameter.Add("OLDJILHWAN", item.OLDJILHWAN);
            parameter.Add("OLDJILHWAN_ETC", item.OLDJILHWAN_ETC);
            parameter.Add("GBSUSUL", item.GBSUSUL);
            parameter.Add("GBSUSUL_ETC", item.GBSUSUL_ETC);
            parameter.Add("GBDRUG", item.GBDRUG);
            parameter.Add("GBDRUG_ETC", item.GBDRUG_ETC);
            parameter.Add("SMOKEYEAR", item.SMOKEYEAR);
            parameter.Add("GBSMOKE", item.GBSMOKE);
            parameter.Add("GBDENTY", item.GBDENTY);
            parameter.Add("DYSPNEA", item.DYSPNEA);
            parameter.Add("GBGEUMGI", item.GBGEUMGI);
            parameter.Add("GBSMOKE1", item.GBSMOKE1);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateMunjinToKbyWrtNo(long nWrtNo, string strMunjinT1, string strMunjinT2, string strMunjinT3, string strMunjinT4, string strMunjinT5
                                        , string strMunjinT6, string strMunjinT7, string strMunjinT8, string strMunjinT9, string strMunjinT10, string strMunjinT11)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET     ");
            parameter.AppendSql("       MUNJINT_A = :MUNJINT_A                ");
            parameter.AppendSql("     , MUNJINT_B = :MUNJINT_B                ");
            parameter.AppendSql("     , MUNJINT_C = :MUNJINT_C                ");
            parameter.AppendSql("     , MUNJINT_D = :MUNJINT_D                ");
            parameter.AppendSql("     , MUNJINT_E = :MUNJINT_E                ");
            parameter.AppendSql("     , MUNJINT_F = :MUNJINT_F                ");
            parameter.AppendSql("     , MUNJINT_G = :MUNJINT_G                ");
            parameter.AppendSql("     , MUNJINT_H = :MUNJINT_H                ");
            parameter.AppendSql("     , MUNJINT_I = :MUNJINT_I                ");
            parameter.AppendSql("     , MUNJINT_J = :MUNJINT_J                ");
            parameter.AppendSql("     , MUNJINT_K = :MUNJINT_K                ");
            //parameter.AppendSql("     , MUNJIN_L = :MUNJIN_L                ");
            //parameter.AppendSql("     , MUNJIN_M = :MUNJIN_M                ");
            //parameter.AppendSql("     , MUNJIN_N = :MUNJIN_N                ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                   ");


            parameter.Add("MUNJINT_A", strMunjinT1);
            parameter.Add("MUNJINT_B", strMunjinT2);
            parameter.Add("MUNJINT_C", strMunjinT3);
            parameter.Add("MUNJINT_D", strMunjinT4);
            parameter.Add("MUNJINT_E", strMunjinT5);
            parameter.Add("MUNJINT_F", strMunjinT6);
            parameter.Add("MUNJINT_G", strMunjinT7);
            parameter.Add("MUNJINT_H", strMunjinT8);
            parameter.Add("MUNJINT_I", strMunjinT9);
            parameter.Add("MUNJINT_J", strMunjinT10);
            parameter.Add("MUNJINT_K", strMunjinT11);

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateMunjinbyWrtNo(long nWrtNo, string strMunjin1, string strMunjin2, string strMunjin3, string strMunjin4, string strMunjin5, string strMunjin6, string strMunjin7
                                       , string strMunjin8, string strMunjin9, string strMunjin10, string strMunjin11, string strMunjin12, string strMunjin13, string strMunjin14)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET     ");
            parameter.AppendSql("       MUNJIN_A = :MUNJIN_A                ");
            parameter.AppendSql("     , MUNJIN_B = :MUNJIN_B                ");
            parameter.AppendSql("     , MUNJIN_C = :MUNJIN_C                ");
            parameter.AppendSql("     , MUNJIN_D = :MUNJIN_D                ");
            parameter.AppendSql("     , MUNJIN_E = :MUNJIN_E                ");
            parameter.AppendSql("     , MUNJIN_F = :MUNJIN_F                ");
            parameter.AppendSql("     , MUNJIN_G = :MUNJIN_G                ");
            parameter.AppendSql("     , MUNJIN_H = :MUNJIN_H                ");
            parameter.AppendSql("     , MUNJIN_I = :MUNJIN_I                ");
            parameter.AppendSql("     , MUNJIN_J = :MUNJIN_J                ");
            parameter.AppendSql("     , MUNJIN_K = :MUNJIN_K                ");
            //parameter.AppendSql("     , MUNJIN_L = :MUNJIN_L                ");
            //parameter.AppendSql("     , MUNJIN_M = :MUNJIN_M                ");
            //parameter.AppendSql("     , MUNJIN_N = :MUNJIN_N                ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                   ");


            parameter.Add("MUNJIN_A", strMunjin1);
            parameter.Add("MUNJIN_B", strMunjin2);
            parameter.Add("MUNJIN_C", strMunjin3);
            parameter.Add("MUNJIN_D", strMunjin4);
            parameter.Add("MUNJIN_E", strMunjin5);
            parameter.Add("MUNJIN_F", strMunjin6);
            parameter.Add("MUNJIN_G", strMunjin7);
            parameter.Add("MUNJIN_H", strMunjin8);
            parameter.Add("MUNJIN_I", strMunjin9);
            parameter.Add("MUNJIN_J", strMunjin10);
            parameter.Add("MUNJIN_K", strMunjin11);
            //parameter.Add("MUNJIN_L", strMunjin12);
            //parameter.Add("MUNJIN_M", strMunjin13);
            //parameter.Add("MUNJIN_N", strMunjin14);

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateAllbyWrtNo(HIC_RES_SPECIAL item, string argGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET                     ");
            if (argGbn == "저장")
            {
                parameter.AppendSql("       MUNJINDATE = SYSDATE                            ");
            }
            else
            {
                parameter.AppendSql("    MUNJINDATE = ''                                    ");
            }
            parameter.AppendSql("     , GBOHMS       = :GBOHMS                              ");
            parameter.AppendSql("     , GBSPC        = :GBSPC                               ");
            parameter.AppendSql("     , UCODECNT     = :UCODECNT                            ");
            parameter.AppendSql("     , UCODENAME    = :UCODENAME                           ");
            parameter.AppendSql("     , SABUN        = :SABUN                               ");
            parameter.AppendSql("     , BUSE         = :BUSE                                ");
            parameter.AppendSql("     , HNAME        = :HNAME                               ");
            parameter.AppendSql("     , IPSADATE     = TO_DATE(:IPSADATE, 'YYYY-MM-DD')     ");
            parameter.AppendSql("     , SUCHUPYN     = :SUCHUPYN                            ");
            parameter.AppendSql("     , GONGJENG     = :GONGJENG                            ");
            parameter.AppendSql("     , JENIPDATE    = TO_DATE(:JENIPDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("     , PGIGAN_YY    = :PGIGAN_YY                           ");
            parameter.AppendSql("     , PGIGAN_MM    = :PGIGAN_MM                           ");
            parameter.AppendSql("     , OLDGONG1     = :OLDGONG1                            ");
            parameter.AppendSql("     , OLDMCODE1    = :OLDMCODE1                           ");
            parameter.AppendSql("     , OLDYEAR1     = :OLDYEAR1                            ");
            parameter.AppendSql("     , OLDPGIGAN1   = :OLDPGIGAN1                          ");
            parameter.AppendSql("     , OLDDAYTIME1  = :OLDDAYTIME1                         ");
            parameter.AppendSql("     , OLDGONG2     = :OLDGONG2                            ");
            parameter.AppendSql("     , OLDMCODE2    = :OLDMCODE2                           ");
            parameter.AppendSql("     , OLDYEAR2     = :OLDYEAR2                            ");
            parameter.AppendSql("     , OLDPGIGAN2   = :OLDPGIGAN2                          ");
            parameter.AppendSql("     , OLDDAYTIME2  = :OLDDAYTIME2                         ");
            parameter.AppendSql("     , OLDGONG3     = :OLDGONG3                            ");
            parameter.AppendSql("     , OLDMCODE3    = :OLDMCODE3                           ");
            parameter.AppendSql("     , OLDYEAR3     = :OLDYEAR3                            ");
            parameter.AppendSql("     , OLDPGIGAN3   = :OLDPGIGAN3                          ");
            parameter.AppendSql("     , OLDDAYTIME3  = :OLDDAYTIME3                         ");
            parameter.AppendSql("     , HABIT1       = :HABIT1                              ");
            parameter.AppendSql("     , HABIT2       = :HABIT2                              ");
            parameter.AppendSql("     , HABIT3       = :HABIT3                              ");
            parameter.AppendSql("     , HABIT4       = :HABIT4                              ");
            parameter.AppendSql("     , HABIT5       = :HABIT5                              ");
            parameter.AppendSql("     , GBHUYU       = :GBHUYU                              ");
            parameter.AppendSql("     , GBSANGTAE    = :GBSANGTAE                           ");
            parameter.AppendSql("     , OLDMYEAR1    = :OLDMYEAR1                           ");
            parameter.AppendSql("     , OLDMYEAR2    = :OLDMYEAR2                           ");
            parameter.AppendSql("     , OLDMYEAR3    = :OLDMYEAR3                           ");
            parameter.AppendSql("     , OLDMYEAR4    = :OLDMYEAR4                           ");
            parameter.AppendSql("     , OLDMYEAR5    = :OLDMYEAR5                           ");
            parameter.AppendSql("     , OLDETCMSYM   = :OLDETCMSYM                          ");
            parameter.AppendSql("     , MUN_GAJOK    = :MUN_GAJOK                           ");
            parameter.AppendSql("     , MUN_GIINSUNG = :MUN_GIINSUNG                        ");
            parameter.AppendSql("     , JIN_NEURO    = :JIN_NEURO                           ");
            parameter.AppendSql("     , JIN_HEAD     = :JIN_HEAD                            ");
            parameter.AppendSql("     , JIN_SKIN     = :JIN_SKIN                            ");
            parameter.AppendSql("     , JIN_CHEST    = :JIN_CHEST                           ");
            parameter.AppendSql("     , JENGSANG     = :JENGSANG                            ");
            parameter.AppendSql("     , DENTSOGEN    = :DENTSOGEN                           ");
            parameter.AppendSql("     , DENTDOCT     = :DENTDOCT                            ");
            parameter.AppendSql("     , JINDRNO      = :JINDRNO                             ");
            parameter.AppendSql("     , HSTAT        = :HSTAT                               "); //작업중 건강문제
            parameter.AppendSql("     , MCODE_STAT   = :MCODE_STAT                          "); //작업중 취급물질 건강문제
            parameter.AppendSql("     , JINREMARK    = :JINREMARK                           ");
            parameter.AppendSql(" WHERE WRTNO        = :WRTNO                               ");

            parameter.Add("UCODECNT"    , item.UCODECNT    );
            parameter.Add("GBOHMS"      , item.GBOHMS, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSPC"       , item.GBSPC, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("UCODENAME"   , item.UCODENAME   );
            parameter.Add("SABUN"       , item.SABUN       );
            parameter.Add("BUSE"        , item.BUSE        );
            parameter.Add("HNAME"       , item.HNAME       );
            parameter.Add("IPSADATE"    , item.IPSADATE    );
            parameter.Add("SUCHUPYN"    , item.SUCHUPYN    );
            parameter.Add("GONGJENG"    , item.GONGJENG    );
            parameter.Add("JENIPDATE"   , item.JENIPDATE   );
            parameter.Add("PGIGAN_YY"   , item.PGIGAN_YY   );
            parameter.Add("PGIGAN_MM"   , item.PGIGAN_MM   );
            parameter.Add("OLDGONG1"    , item.OLDGONG1    );
            parameter.Add("OLDMCODE1"   , item.OLDMCODE1   );
            parameter.Add("OLDYEAR1"    , item.OLDYEAR1    );
            parameter.Add("OLDPGIGAN1"  , item.OLDPGIGAN1  );
            parameter.Add("OLDDAYTIME1" , item.OLDDAYTIME1 );
            parameter.Add("OLDGONG2"    , item.OLDGONG2    );
            parameter.Add("OLDMCODE2"   , item.OLDMCODE2   );
            parameter.Add("OLDYEAR2"    , item.OLDYEAR2    );
            parameter.Add("OLDPGIGAN2"  , item.OLDPGIGAN2  );
            parameter.Add("OLDDAYTIME2" , item.OLDDAYTIME2 );
            parameter.Add("OLDGONG3"    , item.OLDGONG3    );
            parameter.Add("OLDMCODE3"   , item.OLDMCODE3   );
            parameter.Add("OLDYEAR3"    , item.OLDYEAR3    );
            parameter.Add("OLDPGIGAN3"  , item.OLDPGIGAN3  );
            parameter.Add("OLDDAYTIME3" , item.OLDDAYTIME3 );
            parameter.Add("HABIT1"      , item.HABIT1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT2"      , item.HABIT2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT3"      , item.HABIT3, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT4"      , item.HABIT4, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("HABIT5"      , item.HABIT5, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GBHUYU"      , item.GBHUYU, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSANGTAE"   , item.GBSANGTAE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("OLDMYEAR1"   , item.OLDMYEAR1   );
            parameter.Add("OLDMYEAR2"   , item.OLDMYEAR2   );
            parameter.Add("OLDMYEAR3"   , item.OLDMYEAR3   );
            parameter.Add("OLDMYEAR4"   , item.OLDMYEAR4   );
            parameter.Add("OLDMYEAR5"   , item.OLDMYEAR5   );
            parameter.Add("OLDETCMSYM"  , item.OLDETCMSYM  );
            parameter.Add("MUN_GAJOK"   , item.MUN_GAJOK   );
            parameter.Add("MUN_GIINSUNG", item.MUN_GIINSUNG);
            parameter.Add("JIN_NEURO"   , item.JIN_NEURO   );
            parameter.Add("JIN_HEAD"    , item.JIN_HEAD    );
            parameter.Add("JIN_SKIN"    , item.JIN_SKIN    );
            parameter.Add("JIN_CHEST"   , item.JIN_CHEST   );
            parameter.Add("JENGSANG"    , item.JENGSANG    );
            parameter.Add("DENTSOGEN"   , item.DENTSOGEN   );
            parameter.Add("DENTDOCT"    , item.DENTDOCT    );
            parameter.Add("JINDRNO"     , item.JINDRNO     );
            parameter.Add("HSTAT"       , item.HSTAT, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("MCODE_STAT"  , item.MCODE_STAT, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("JINREMARK"   , item.JINREMARK   );
            parameter.Add("WRTNO"       , item.WRTNO       );

            return ExecuteNonQuery(parameter);
        }

        public string GetTmunAllbyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MUNJINT_A || MUNJINT_B || MUNJINT_C || MUNJINT_D || MUNJINT_E || MUNJINT_F  ");
            parameter.AppendSql("       || MUNJINT_G || MUNJINT_H || MUNJINT_I || MUNJINT_J || MUNJINT_K AS TMunAll ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_SPECIAL                                                 ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                              ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public int UCodeCount_UpDate(long argWRTNO, int nUCodeCNT)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL     ");
            parameter.AppendSql("   SET UCODECNT   = :UCodeCNT          ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO             ");

            #region Query 변수대입
            parameter.Add("UCodeCNT", nUCodeCNT);
            parameter.Add("WRTNO", argWRTNO);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public int UCodeName_UpDate(string strRowid, string uCODES)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL     ");
            parameter.AppendSql("   SET UCODENAME   = :UCODENAME        ");
            parameter.AppendSql(" WHERE ROWID       = :RID              ");

            #region Query 변수대입
            parameter.Add("UCODENAME", uCODES);
            parameter.Add("RID", strRowid);
            #endregion
            return ExecuteNonQuery(parameter);
        }
        public int UpdatePRINTbyWrtNo(long argWRTNO, long argSABUN)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET ");
            parameter.AppendSql("      GBPRINT = 'Y'                    ");
            parameter.AppendSql("      ,TONGBODATE = TRUNC(SYSDATE)     ");
            parameter.AppendSql("      ,PRTSABUN = :SABUN               ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", argWRTNO);
            parameter.Add("SABUN", argSABUN);

            return ExecuteNonQuery(parameter);
        }
        public int UpdateDentSogenbyWrtNo(string strSpcPanjeng, long nPanDrNo, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_SPECIAL SET                         ");
            parameter.AppendSql("       DENTSOGEN       = :DENTSOGEN                            ");
            parameter.AppendSql("     , DENTDOCT        = :DENTDOCT                             ");
            parameter.AppendSql(" WHERE WRTNO           = :WRTNO                                ");

            parameter.Add("DENTSOGEN", strSpcPanjeng);
            parameter.Add("DENTDOCT", nPanDrNo);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public HIC_RES_SPECIAL GetPanTongDateByWrtno(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PANJENGDATE,'YYYY-MM-DD') PANJENGDATE,PANJENGDRNO,  ");
            parameter.AppendSql(" TO_CHAR(TONGBODATE,'YYYY-MM-DD') TONGBODATE,PRTSABUN              ");
            parameter.AppendSql(" From KOSMOS_PMPA.HIC_RES_SPECIAL                                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                              ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_RES_SPECIAL>(parameter);
        }
    }
}
