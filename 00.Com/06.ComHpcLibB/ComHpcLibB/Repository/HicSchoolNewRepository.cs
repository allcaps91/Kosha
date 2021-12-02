namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicSchoolNewRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSchoolNewRepository()
        {
        }

        public int UpdatebyWrtNo(HIC_SCHOOL_NEW item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SCHOOL_NEW SET              ");
            parameter.AppendSql("       DPAN1 = :DPAN1                              ");
            parameter.AppendSql("     , DPAN2 = :DPAN2                              ");
            parameter.AppendSql("     , DPAN3 = :DPAN3                              ");
            parameter.AppendSql("     , DPAN4 = :DPAN4                              ");
            parameter.AppendSql("     , DPAN5 = :DPAN5                              ");
            parameter.AppendSql("     , DPAN6 = :DPAN6                              ");
            parameter.AppendSql("     , DPAN7 = :DPAN7                              ");
            parameter.AppendSql("     , DPAN8 = :DPAN8                              ");
            parameter.AppendSql("     , DPAN9 = :DPAN9                              ");
            parameter.AppendSql("     , DPAN10 = :DPAN10                            ");
            parameter.AppendSql("     , DPAN11 = :DPAN11                            ");
            parameter.AppendSql("     , DPANSOGEN = :DPANSOGEN                      ");
            parameter.AppendSql("     , DPANJOCHI = :DPANJOCHI                      ");
            parameter.AppendSql("     , DPAN12 = :DPAN12                            ");
            parameter.AppendSql("     , DPAN13 = :DPAN13                            ");
            parameter.AppendSql("     , DPANDATE = SYSDATE                          ");
            parameter.AppendSql("     , DPANDRNO = :DPANDRNO                        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("DPAN1", item.DPAN1);
            parameter.Add("DPAN2", item.DPAN2);
            parameter.Add("DPAN3", item.DPAN3);
            parameter.Add("DPAN4", item.DPAN4);
            parameter.Add("DPAN5", item.DPAN5);
            parameter.Add("DPAN6", item.DPAN6);
            parameter.Add("DPAN7", item.DPAN7);
            parameter.Add("DPAN8", item.DPAN8);
            parameter.Add("DPAN9", item.DPAN9);
            parameter.Add("DPAN10", item.DPAN10);
            parameter.Add("DPAN11", item.DPAN11);
            parameter.Add("DPANSOGEN", item.DPANSOGEN);
            parameter.Add("DPANJOCHI", item.DPANJOCHI);
            parameter.Add("DPAN12", item.DPAN12);
            parameter.Add("DPAN13", item.DPAN13);
            parameter.Add("DPANDRNO", item.DPANDRNO);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public void UpDateItembyItem(HIC_SCHOOL_NEW nHSN)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SCHOOL_NEW SET              ");
            parameter.AppendSql("      ,JUMIN   = :JUMIN                            ");
            parameter.AppendSql("      ,JUMIN2  = :JUMIN2                           ");
            parameter.AppendSql("      ,SNAME   = :SNAME                            ");
            parameter.AppendSql("      ,SDATE   = TO_DATE(:SDATE, 'YYYY-MM-DD')     ");
            parameter.AppendSql("      ,SEX     = :SEX                              ");
            parameter.AppendSql("      ,GBN     = :GBN                              ");
            parameter.AppendSql("      ,CLASS   = :CLASS                            ");
            parameter.AppendSql("      ,BAN     = :BAN                              ");
            parameter.AppendSql("      ,BUN     = :BUN                              ");
            parameter.AppendSql("      ,LTDCODE = :LTDCODE                          ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                            ");

            parameter.Add("JUMIN",  nHSN.JUMIN);
            parameter.Add("JUMIN2", nHSN.JUMIN2);
            parameter.Add("SNAME",  nHSN.SNAME);
            parameter.Add("SDATE",  nHSN.SDATE);
            parameter.Add("SEX",    nHSN.SEX);
            parameter.Add("GBN",    nHSN.GBN);
            parameter.Add("CLASS",  nHSN.CLASS);
            parameter.Add("BAN",    nHSN.BAN);
            parameter.Add("BUN",    nHSN.BUN);
            parameter.Add("LTDCODE",nHSN.LTDCODE);
            parameter.Add("WRTNO",  nHSN.WRTNO);

            ExecuteNonQuery(parameter);
        }

        public HIC_SCHOOL_NEW GetDPPanDrNoPPanDrNobyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DPanDrno,PPanDrno,GbDntPrt,GbPanPrt,PrtSabun    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SCHOOL_NEW                      ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                                ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_SCHOOL_NEW>(parameter);
        }

        public HIC_SCHOOL_NEW GetItembyWrtNoSingle(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PanjengDrno, TO_CHAR(PanjengDate,'YYYY-MM-DD') PanjengDate,'0' PrtSabun ");
            parameter.AppendSql("     , TO_CHAR(TongboDate,'YYYY-MM-DD') TongboDate                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SCHOOL_NEW                                              ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                                                        ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_SCHOOL_NEW>(parameter);
        }

        public int InsertWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SCHOOL_NEW     ");
            parameter.AppendSql("       (WRTNO)                             ");
            parameter.AppendSql("VALUES                                     ");
            parameter.AppendSql("       (:WRTNO)                            ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyPpanBWrtNo(HIC_SCHOOL_NEW item)
        {
            MParameter parameter = CreateParameter();
            
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SCHOOL_NEW SET      ");
            parameter.AppendSql("       PPANB1  = :PPANB1                   ");
            parameter.AppendSql("     , PPANB2  = :PPANB2                   ");
            parameter.AppendSql("     , PPANC4  = :PPANC4                   ");
            parameter.AppendSql("     , PPANC6  = :PPANC6                   ");
            parameter.AppendSql("     , PPANC7  = :PPANC7                   ");
            parameter.AppendSql("     , PPANC8  = :PPANC8                   ");
            parameter.AppendSql("     , PPANC9  = :PPANC9                   ");
            parameter.AppendSql("     , PPAND1  = :PPAND1                   ");
            parameter.AppendSql("     , PPAND2  = :PPAND2                   ");
            parameter.AppendSql("     , PPAND3  = :PPAND3                   ");
            parameter.AppendSql("     , PPAND4  = :PPAND4                   ");
            parameter.AppendSql("     , PPAND5  = :PPAND5                   ");
            parameter.AppendSql("     , PPAND6  = :PPAND6                   ");
            parameter.AppendSql("     , PPANK1  = :PPANK1                   ");
            parameter.AppendSql("     , PPANK2  = :PPANK2                   ");
            parameter.AppendSql("     , PPANK3  = :PPANK3                   ");
            parameter.AppendSql("     , PPANK4  = :PPANK4                   ");
            parameter.AppendSql("     , SANGDAM = :SANGDAM                  ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                    ");

            parameter.Add("PPANB1", item.PPANB1);
            parameter.Add("PPANB2", item.PPANB2);
            parameter.Add("PPANC4", item.PPANC4);
            parameter.Add("PPANC6", item.PPANC6);
            parameter.Add("PPANC7", item.PPANC7);
            parameter.Add("PPANC8", item.PPANC8);
            parameter.Add("PPANC9", item.PPANC9);
            parameter.Add("PPAND1", item.PPAND1);
            parameter.Add("PPAND2", item.PPAND2);
            parameter.Add("PPAND3", item.PPAND3);
            parameter.Add("PPAND4", item.PPAND4);
            parameter.Add("PPAND5", item.PPAND5);
            parameter.Add("PPAND6", item.PPAND6);
            parameter.Add("PPANK1", item.PPANK1);
            parameter.Add("PPANK2", item.PPANK2);
            parameter.Add("PPANK3", item.PPANK3);
            parameter.Add("PPANK4", item.PPANK4);
            parameter.Add("SANGDAM", item.SANGDAM);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebySchPanWrtNo(HIC_SCHOOL_NEW item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SCHOOL_NEW SET          ");
            parameter.AppendSql("       SCHPAN1     = :SCHPAN1                  ");
            parameter.AppendSql("     , SCHPAN2     = :SCHPAN2                  ");
            parameter.AppendSql("     , SCHPAN3     = :SCHPAN3                  ");
            parameter.AppendSql("     , SCHPAN4     = :SCHPAN4                  ");
            parameter.AppendSql("     , SCHPAN5     = :SCHPAN5                  ");
            parameter.AppendSql("     , SCHPAN6     = :SCHPAN6                  ");
            parameter.AppendSql("     , SCHPAN7     = :SCHPAN7                  ");
            parameter.AppendSql("     , SCHPAN8     = :SCHPAN8                  ");
            parameter.AppendSql("     , SCHPAN9     = :SCHPAN9                  ");
            parameter.AppendSql("     , SCHPAN10    = :SCHPAN10                 ");
            parameter.AppendSql("     , SCHPAN11    = :SCHPAN11                 ");
            parameter.AppendSql("     , REMARK      = :REMARK                   ");
            parameter.AppendSql("     , GBSTS       = :GBSTS                    ");
            parameter.AppendSql("     , SANGDAMDRNO = :SANGDAMDRNO              ");
            parameter.AppendSql("     , ENTSABUN    = :ENTSABUN                 ");
            parameter.AppendSql("     , ENTTIME     = SYSDATE                   ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO                    ");

            parameter.Add("SCHPAN1" , item.SCHPAN1);
            parameter.Add("SCHPAN2" , item.SCHPAN2);
            parameter.Add("SCHPAN3" , item.SCHPAN3);
            parameter.Add("SCHPAN4" , item.SCHPAN4);
            parameter.Add("SCHPAN5" , item.SCHPAN5);
            parameter.Add("SCHPAN6" , item.SCHPAN6);
            parameter.Add("SCHPAN7" , item.SCHPAN7);
            parameter.Add("SCHPAN8" , item.SCHPAN8);
            parameter.Add("SCHPAN9", item.SCHPAN9);
            parameter.Add("SCHPAN10", item.SCHPAN10);
            parameter.Add("SCHPAN11", item.SCHPAN11);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("GBSTS", item.GBSTS);
            parameter.Add("SANGDAMDRNO", item.SANGDAMDRNO);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyRowId(HIC_SCHOOL_NEW item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SCHOOL_NEW SET                      ");
            parameter.AppendSql("       WRTNO       = :WRTNO                                ");
            parameter.AppendSql("     , JUMIN       = :JUMIN                                ");
            parameter.AppendSql("     , JUMIN2      = :JUMIN2                               ");
            parameter.AppendSql("     , SNAME       = :SNAME                                ");
            parameter.AppendSql("     , SDATE       = TO_DATE(:SDATE, 'YYYY-MM-DD')         ");
            parameter.AppendSql("     , RDATE       = TO_DATE(:RDATE, 'YYYY-MM-DD')         ");
            parameter.AppendSql("     , SEX         = :SEX                                  ");
            parameter.AppendSql("     , GBN         = :GBN                                  ");
            parameter.AppendSql("     , CLASS       = :CLASS                                ");
            parameter.AppendSql("     , BAN         = :BAN                                  ");
            parameter.AppendSql("     , BUN         = :BUN                                  ");
            parameter.AppendSql("     , LTDCODE     = :LTDCODE                              ");
            parameter.AppendSql("     , PPANA1      = :PPANA1                               ");
            parameter.AppendSql("     , PPANA2      = :PPANA2                               ");
            parameter.AppendSql("     , PPANA3      = :PPANA3                               ");
            parameter.AppendSql("     , PPANA4      = :PPANA4                               ");
            parameter.AppendSql("     , PPANB1      = :PPANB1                               ");
            parameter.AppendSql("     , PPANB2      = :PPANB2                               ");
            parameter.AppendSql("     , PPANC1      = :PPANC1                               ");
            parameter.AppendSql("     , PPANC2      = :PPANC2                               ");
            parameter.AppendSql("     , PPANC3      = :PPANC3                               ");
            parameter.AppendSql("     , PPANC4      = :PPANC4                               ");
            parameter.AppendSql("     , PPANC5      = :PPANC5                               ");
            parameter.AppendSql("     , PPANC6      = :PPANC6                               ");
            parameter.AppendSql("     , PPANC7      = :PPANC7                               ");
            parameter.AppendSql("     , PPANC8      = :PPANC8                               ");
            parameter.AppendSql("     , PPANC9      = :PPANC9                               ");
            parameter.AppendSql("     , PPAND1      = :PPAND1                               ");
            parameter.AppendSql("     , PPAND2      = :PPAND2                               ");
            parameter.AppendSql("     , PPAND3      = :PPAND3                               ");
            parameter.AppendSql("     , PPAND4      = :PPAND4                               ");
            parameter.AppendSql("     , PPAND5      = :PPAND5                               ");
            parameter.AppendSql("     , PPAND6      = :PPAND6                               ");
            parameter.AppendSql("     , PPANE1      = :PPANE1                               ");
            parameter.AppendSql("     , PPANE2      = :PPANE2                               ");
            parameter.AppendSql("     , PPANE3      = :PPANE3                               ");
            parameter.AppendSql("     , PPANE4      = :PPANE4                               ");
            parameter.AppendSql("     , PPANF1      = :PPANF1                               ");
            parameter.AppendSql("     , PPANF2      = :PPANF2                               ");
            parameter.AppendSql("     , PPANF3      = :PPANF3                               ");
            parameter.AppendSql("     , PPANF4      = :PPANF4                               ");
            parameter.AppendSql("     , PPANF5      = :PPANF5                               ");
            parameter.AppendSql("     , PPANF6      = :PPANF6                               ");
            parameter.AppendSql("     , PPANG1      = :PPANG1                               ");
            parameter.AppendSql("     , PPANH1      = :PPANH1                               ");
            parameter.AppendSql("     , PPANJ1      = :PPANJ1                               ");
            parameter.AppendSql("     , PPANK1      = :PPANK1                               ");
            parameter.AppendSql("     , PPANK2      = :PPANK2                               ");
            parameter.AppendSql("     , PPANK3      = :PPANK3                               ");
            parameter.AppendSql("     , PPANK4      = :PPANK4                               ");
            parameter.AppendSql("     , PPANREMARK1 = :PPANREMARK1                          ");
            parameter.AppendSql("     , PPANREMARK2 = :PPANREMARK2                          ");
            parameter.AppendSql("     , GBPAN       = :GBPAN                                ");
            parameter.AppendSql("     , PPANDRNO    = :PPANDRNO                             ");
            parameter.AppendSql("     , ENTTIME     = SYSDATE                               ");
            parameter.AppendSql("     , ENTSABUN    = :ENTSABUN                             ");
            parameter.AppendSql(" WHERE ROWID       = :RID                                  ");

            parameter.Add("DPAN1", item.DPAN1);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("JUMIN", item.JUMIN);
            parameter.Add("JUMIN2", item.JUMIN2);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SDATE", item.SDATE);
            parameter.Add("RDATE", item.RDATE);
            parameter.Add("SEX", item.SEX);
            parameter.Add("GBN", item.GBN);
            parameter.Add("CLASS", item.CLASS);
            parameter.Add("BAN", item.BAN);
            parameter.Add("BUN", item.BUN);
            parameter.Add("LTDCODE", item.LTDCODE);
            parameter.Add("PPANA1", item.PPANA1);
            parameter.Add("PPANA2", item.PPANA2);
            parameter.Add("PPANA3", item.PPANA3);
            parameter.Add("PPANA4", item.PPANA4);
            parameter.Add("PPANB1", item.PPANB1);
            parameter.Add("PPANB2", item.PPANB2);
            parameter.Add("PPANC1", item.PPANC1);
            parameter.Add("PPANC2", item.PPANC2);
            parameter.Add("PPANC3", item.PPANC3);
            parameter.Add("PPANC4", item.PPANC4);
            parameter.Add("PPANC5", item.PPANC5);
            parameter.Add("PPANC6", item.PPANC6);
            parameter.Add("PPANC7", item.PPANC7);
            parameter.Add("PPANC8", item.PPANC8);
            parameter.Add("PPANC9", item.PPANC9);
            parameter.Add("PPAND1", item.PPAND1);
            parameter.Add("PPAND2", item.PPAND2);
            parameter.Add("PPAND3", item.PPAND3);
            parameter.Add("PPAND4", item.PPAND4);
            parameter.Add("PPAND5", item.PPAND5);
            parameter.Add("PPAND6", item.PPAND6);
            parameter.Add("PPANE1", item.PPANE1);
            parameter.Add("PPANE2", item.PPANE2);
            parameter.Add("PPANE3", item.PPANE3);
            parameter.Add("PPANE4", item.PPANE4);
            parameter.Add("PPANF1", item.PPANF1);
            parameter.Add("PPANF2", item.PPANF2);
            parameter.Add("PPANF3", item.PPANF3);
            parameter.Add("PPANF4", item.PPANF4);
            parameter.Add("PPANF5", item.PPANF5);
            parameter.Add("PPANF6", item.PPANF6);
            parameter.Add("PPANG1", item.PPANG1);
            parameter.Add("PPANH1", item.PPANH1);
            parameter.Add("PPANJ1", item.PPANJ1);
            parameter.Add("PPANK1", item.PPANK1);
            parameter.Add("PPANK2", item.PPANK2);
            parameter.Add("PPANK3", item.PPANK3);
            parameter.Add("PPANK4", item.PPANK4);
            parameter.Add("PPANREMARK1", item.PPANREMARK1);
            parameter.Add("PPANREMARK2", item.PPANREMARK2);
            parameter.Add("PPANDRNO", item.PPANDRNO);
            parameter.Add("GBPAN", item.GBPAN);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("RID", item.ROWID);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_SCHOOL_NEW item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO HIC_SCHOOL_NEW                                                                         ");
            parameter.AppendSql("       (WRTNO, JUMIN, JUMIN2, SNAME, SDATE, RDATE, SEX, GBN, CLASS, BAN, BUN, LTDCODE              ");
            parameter.AppendSql("     , PPANA1,PPANA2,PPANA3,PPANA4,PPANB1,PPANB2,PPANC1,PPANC2,PPANC3,PPANC4,PPANC5                ");
            parameter.AppendSql("     , PPANC6,PPANC7,PPANC8,PPANC9,PPAND1,PPAND2,PPAND3,PPAND4,PPAND5,PPAND6,PPANE1                ");
            parameter.AppendSql("     , PPANE2,PPANE3,PPANE4,PPANF1,PPANF2,PPANF3,PPANF4,PPANF5,PPANF6,PPANG1,PPANH1                ");
            parameter.AppendSql("     , PPANJ1,PPANK1,PPANK2,PPANK3,PPANK4,PPanRemark1,PPanRemark2,PPANDRNO,GbPan,ENTTIME,ENTSABUN) ");
            parameter.AppendSql("VALUES                                                                                             ");
            parameter.AppendSql("       (:WRTNO, :JUMIN, :JUMIN2, :SNAME, TO_DATE(:SDATE, 'YYYY-MM-DD')                             ");
            parameter.AppendSql("     , TO_DATE(:RDATE, 'YYYY-MM-DD'), :SEX, :GBN, :CLASS, :BAN, :BUN, :LTDCODE                     ");
            parameter.AppendSql("     , :PPANA1,:PPANA2,:PPANA3,:PPANA4,:PPANB1,:PPANB2,:PPANC1,:PPANC2,:PPANC3,:PPANC4,:PPANC5     ");
            parameter.AppendSql("     , :PPANC6,:PPANC7,:PPANC8,:PPANC9,:PPAND1,:PPAND2,:PPAND3,:PPAND4,:PPAND5,:PPAND6,:PPANE1     ");
            parameter.AppendSql("     , :PPANE2,:PPANE3,:PPANE4,:PPANF1,:PPANF2,:PPANF3,:PPANF4,:PPANF5,:PPANF6,:PPANG1,:PPANH1     ");
            parameter.AppendSql("     , :PPANJ1,:PPANK1,:PPANK2,:PPANK3,:PPANK4,:PPanRemark1,:PPanRemark2,:PPANDRNO,:GbPan,SYSDATE,:ENTSABUN) ");

            parameter.Add("DPAN1", item.DPAN1);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("JUMIN", item.JUMIN);
            parameter.Add("JUMIN2", item.JUMIN2);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SDATE", item.SDATE);
            parameter.Add("RDATE", item.RDATE);
            parameter.Add("SEX", item.SEX);
            parameter.Add("GBN", item.GBN);
            parameter.Add("CLASS", item.CLASS);
            parameter.Add("BAN", item.BAN);
            parameter.Add("BUN", item.BUN);
            parameter.Add("LTDCODE", item.LTDCODE);
            parameter.Add("PPANA1", item.PPANA1);
            parameter.Add("PPANA2", item.PPANA2);
            parameter.Add("PPANA3", item.PPANA3);
            parameter.Add("PPANA4", item.PPANA4);
            parameter.Add("PPANB1", item.PPANB1);
            parameter.Add("PPANB2", item.PPANB2);
            parameter.Add("PPANC1", item.PPANC1);
            parameter.Add("PPANC2", item.PPANC2);
            parameter.Add("PPANC3", item.PPANC3);
            parameter.Add("PPANC4", item.PPANC4);
            parameter.Add("PPANC5", item.PPANC5);
            parameter.Add("PPANC6", item.PPANC6);
            parameter.Add("PPANC7", item.PPANC7);
            parameter.Add("PPANC8", item.PPANC8);
            parameter.Add("PPANC9", item.PPANC9);
            parameter.Add("PPAND1", item.PPAND1);
            parameter.Add("PPAND2", item.PPAND2);
            parameter.Add("PPAND3", item.PPAND3);
            parameter.Add("PPAND4", item.PPAND4);
            parameter.Add("PPAND5", item.PPAND5);
            parameter.Add("PPAND6", item.PPAND6);
            parameter.Add("PPANE1", item.PPANE1);
            parameter.Add("PPANE2", item.PPANE2);
            parameter.Add("PPANE3", item.PPANE3);
            parameter.Add("PPANE4", item.PPANE4);
            parameter.Add("PPANF1", item.PPANF1);
            parameter.Add("PPANF2", item.PPANF2);
            parameter.Add("PPANF3", item.PPANF3);
            parameter.Add("PPANF4", item.PPANF4);
            parameter.Add("PPANF5", item.PPANF5);
            parameter.Add("PPANF6", item.PPANF6);
            parameter.Add("PPANG1", item.PPANG1);
            parameter.Add("PPANH1", item.PPANH1);
            parameter.Add("PPANJ1", item.PPANJ1);
            parameter.Add("PPANK1", item.PPANK1);
            parameter.Add("PPANK2", item.PPANK2);
            parameter.Add("PPANK3", item.PPANK3);
            parameter.Add("PPANK4", item.PPANK4);
            parameter.Add("PPANREMARK1", item.PPANREMARK1);
            parameter.Add("PPANREMARK2", item.PPANREMARK2);
            parameter.Add("PPANDRNO", item.PPANDRNO);
            parameter.Add("GBPAN", item.GBPAN);
            parameter.Add("ENTSABUN", item.ENTSABUN);

            return ExecuteNonQuery(parameter);
        }

        public string GetRowIdbyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SCHOOL_NEW      ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_SCHOOL_NEW GetDPanDrNobySDate(string strFrDate, string strToDate, long nLtdCode1, string strClass)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DPanDrno, PPanDrno FROM KOSMOS_PMPA.HIC_SCHOOL_NEW  ");
            parameter.AppendSql(" WHERE SDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')             ");
            parameter.AppendSql("   AND SDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')             ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                  ");
            parameter.AppendSql("   AND CLASS   = :CLASS                                    ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("LTDCODE", nLtdCode1);
            parameter.Add("CLASS", strClass);

            return ExecuteReaderSingle<HIC_SCHOOL_NEW>(parameter);
        }

        public int UpdateGbMirPrintbyWrtNo(List<string> fstrWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SCHOOL_NEW SET  ");
            parameter.AppendSql("       GBMIRPRINT = 'Y'                ");
            parameter.AppendSql(" WHERE WRTNO     IN (:WRTNO)           ");

            parameter.AddInStatement("WRTNO", fstrWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateGbDntPrtbyWrtNo(string idNumber, long nWrtNo, string strGbGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SCHOOL_NEW SET  ");
            if (strGbGubun == "PAN")
            {
                parameter.AppendSql("       GBPANPRT = 'Y'              ");
            }
            else if (strGbGubun == "DNT")
            {
                parameter.AppendSql("       GBDNTPRT = 'Y'              ");
            }
            else if (strGbGubun == "PANDNT")
            {
                parameter.AppendSql("       GBDNTPRT = 'Y'              ");
                parameter.AppendSql("     , GBDNTPRT = 'Y'              ");
            }
            parameter.AppendSql("     , PRTSABUN = :PRTSABUN            ");
            parameter.AppendSql("     , TONGBODATE = TRUNC(SYSDATE)     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("PRTSABUN", idNumber);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_SCHOOL_NEW> GetItembyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,SNAME,TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, TO_CHAR(RDATE,'YYYY-MM-DD') RDATE        ");
            parameter.AppendSql("     , SEX,GBN,CLASS,BAN,BUN,LTDCODE,DMUN1,DMUN2                                               ");
            parameter.AppendSql("     , DMUN3,DMUN4,DMUN5,DMUN6,DMUN7,DMUN8,DMUN9,DMUN10,DMUNREMARK,DPAN1,DPAN2,DPAN3           "); 
            parameter.AppendSql("     , DPAN4,DPAN5,DPAN6,DPAN7,DPAN8,DPAN9,DPAN10,DPAN11,DPAN12,DPAN13, DPANSOGEN,DPANJOCHI    ");
            parameter.AppendSql("     , DPANDRNO, PMUNA1,PMUNA2,PMUNA3,PMUNA4,PMUNA5,PMUNA6,PMUNA7,PMUNB1,PMUNB2,PMUNB3,PMUNB4  ");
            parameter.AppendSql("     , PMUNB5,PMUNB6,PMUNB7,PMUNC1,PMUNC2,PMUNC3,PMUNC4,PMUNC5,PMUNC6,PMUNC7,PMUND1            ");
            parameter.AppendSql("     , PMUND2,PMUND3,PMUND4,PMUND5,PMUND6,PMUND7,PMUND8,PMUND9,PMUNREMARK1,PMUNREMARK2,PPANA1  ");
            parameter.AppendSql("     , PPANA2,PPANA3,PPANA4,PPANB1,PPANB2,PPANC1,PPANC2,PPANC3,PPANC4,PPANC5,PPANC6            ");
            parameter.AppendSql("     , PPANC7,PPANC8,PPANC9,PPAND1,PPAND2,PPAND3,PPAND4,PPAND5,PPAND6,PPANE1,PPANE2            ");
            parameter.AppendSql("     , PPANE3,PPANE4,PPANF1,PPANF2,PPANF3,PPANF4,PPANF5,PPANF6,PPANG1,PPANH1,PPANJ1            ");
            parameter.AppendSql("     , PPANK1 , PPANK2, PPANK3, PPANK4,GbPan, PPANDRNO, PPANF7, PPANF8 PPANF9                  ");
            parameter.AppendSql("     , PPANREMARK1, PPANREMARK2, SANGDAM                                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SCHOOL_NEW                                                              ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                                         ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_SCHOOL_NEW>(parameter);
        }

        public HIC_SCHOOL_NEW GetPanjengDateByWrtno(long argWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(DPanDate,'YYYY-MM-DD') PanjengDate,DPanDrNo PanjengDrNo     ");
            parameter.AppendSql(" ,TO_CHAR(TongboDate,'YYYY-MM-DD') TongboDate,PrtSabun                     ");
            parameter.AppendSql(" FROM HIC_SCHOOL_NEW                                                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                      ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteReaderSingle<HIC_SCHOOL_NEW>(parameter);
        }
    }
}
