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
    public class HicResDentalRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicResDentalRepository()
        {
        }

        public HIC_RES_DENTAL GetItemByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,USIK1,USIK2,USIK3,USIK4,USIK5,USIK6,GYELSON1,GYELSON2,GYELSON3                        ");
            parameter.AppendSql("     , CHIJU1, CHIJU2, CHIJU3, CHIJU4, CHIJU5, CHIJU6, CHIJU7, CHIJU8, CHIJU9, CHIJU10, BOCHUL1    ");
            parameter.AppendSql("     , BOCHUL2, BOCHUL3, BOCHUL4, BOCHUL5, BOCHUL6, BOCHUL7, BOCHUL8, BOCHUL9, BOCHUL10, BOCHUL11  ");
            parameter.AppendSql("     , BOCHUL12, OPDDNT, SCALING, DNTSTATUS, FOOD1, FOOD2, FOOD3, BRUSH11, BRUSH12, BRUSH13        ");
            parameter.AppendSql("     , BRUSH14, BRUSH15, BRUSH16, BRUSH21, JUNGSANG1, JUNGSANG2, JUNGSANG3, JUNGSANG4, JUNGSANG5   ");
            parameter.AppendSql("     , JUNGSANG6, JUNGSANG7, MUNJINETC, PANJENG1, PANJENG2, PANJENG3, PANJENG4, PANJENG5           ");
            parameter.AppendSql("     , PANJENG6, PANJENG7, PANJENG8, PANJENG9, PANJENG10, PANJENG11, PANJENG12                     ");
            parameter.AppendSql("     , TONGBOGBN, PANJENGDRNO, MIRNO, PANJENG13, MIRYN, T_HABIT1, T_HABIT2, T_HABIT3               ");
            parameter.AppendSql("     , T_HABIT4, T_STAT1, T_STAT2, T_STAT3, T_STAT4, T_STAT5, T_STAT6, T_FUNCTION1, T_FUNCTION2    ");
            parameter.AppendSql("     , T_FUNCTION3, T_FUNCTION4, T_FUNCTION5, T_JILBYUNG1, T_PAN1, T_PAN2, T_PAN3, T_PAN4          ");
            parameter.AppendSql("     , T_PAN5, T_PAN6, T_PAN7, T_PAN8, T_PAN9, T_PAN10, T_PAN_ETC, T40_PAN1, T40_PAN2, T40_PAN3    ");
            parameter.AppendSql("     , T40_PAN4, T40_PAN5, T40_PAN6, T_PANJENG1, T_PANJENG2, T_PANJENG3, T_PANJENG4, T_PANJENG5    ");
            parameter.AppendSql("     , T_PANJENG6, T_PANJENG7, T_PANJENG8, T_PANJENG9, T_PANJENG10, T_PANJENG_ETC, T_PANJENG_SOGEN ");
            parameter.AppendSql("     , GBPRINT, SANGDAM, BUSIK1, BUSIK2, BUSIK3, BUSIK4, BUSIK5, GYOMO1, GYOMO2, GYOMO3, GYOMO4    ");
            parameter.AppendSql("     , CHIJURESULT, CHIJUSTAT1, CHIJUSTAT2, CHIJUSTAT3, CHIJUSTAT4, CHIJUSTATETC, BUSIK0           ");
            parameter.AppendSql("     , GYOMO0, T_PAN11, T_HABIT5, T_HABIT6, T_HABIT7, T_HABIT8, T_HABIT9, T_JILBYUNG2, RES_MUNJIN  ");
            parameter.AppendSql("     , RES_JOCHI, RES_RESULT, T40_PAN1_NEW, T40_PAN2_NEW, T40_PAN3_NEW, T40_PAN4_NEW               ");
            parameter.AppendSql("     , T40_PAN5_NEW, T40_PAN6_NEW                                                                  ");
            parameter.AppendSql("     , TO_CHAR(PANJENGDATE, 'YYYY-MM-DD') PANJENGDATE                                              ");
            parameter.AppendSql("     , TO_CHAR(TONGBODATE, 'YYYY-MM-DD') TONGBODATE                                                ");
            parameter.AppendSql("     , ROWID RID                                                                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_DENTAL                                                                  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                                             ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReaderSingle<HIC_RES_DENTAL>(parameter);
        }

        public int InsertWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_RES_DENTAL         ");
            parameter.AppendSql("       (WRTNO)                                 ");
            parameter.AppendSql("VALUES                                         ");
            parameter.AppendSql("       (:WRTNO)                                ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_RES_DENTAL> GetItemsbyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PanjengDrno, TO_CHAR(PanjengDate,'YYYY-MM-DD') PanjengDate,'0' PrtSabun     ");
            parameter.AppendSql("     , TO_CHAR(TongboDate,'YYYY-MM-DD') TongboDate                                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_DENTAL                                                  ");
            parameter.AppendSql(" WHERE WRTNO =  :WRTNO                                                             ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_RES_DENTAL>(parameter);
        }

        public HIC_RES_DENTAL GetPanjengDateOpdDntDntStatusbyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PanjengDate,'YYYY-MM-DD') PanjengDate,OpdDnt,DntStatus              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_DENTAL                                                  ");
            parameter.AppendSql(" WHERE WRTNO =  :WRTNO                                                             ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReaderSingle<HIC_RES_DENTAL>(parameter);
        }

        public void UpdateTongBoInfobyWrtNo(long fnWRTNO, string strDate, string strGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_DENTAL SET                      ");
            if (!strDate.IsNullOrEmpty())
            {
                parameter.AppendSql("       TONGBODATE  = TO_DATE(:TONGBODATE, 'YYYY-MM-DD')");
                parameter.AppendSql("     , TONGBOGBN  = :TONGBOGBN                         ");
                parameter.AppendSql("     , GBPRINT = 'Y'                                   ");
            }
            else
            {
                parameter.AppendSql("       TONGBODATE  = ''                                ");
                parameter.AppendSql("     , TONGBOGBN  = ''                                 ");
                parameter.AppendSql("     , GBPRINT = 'N'                                   ");
            }

            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                     ");

            if (!strDate.IsNullOrEmpty())
            {
                parameter.Add("TONGBODATE", strDate);
                parameter.Add("TONGBOGBN", strGbn);
            }

            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public void UpdatePanjengInfobyWrtNo(long fnWRTNO, string strDate, long nDrNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_DENTAL SET                      ");
            if (nDrNO > 0)
            {
                parameter.AppendSql("     , TONGBODATE  = TO_DATE(:TONGBODATE, 'YYYY-MM-DD')    ");
                parameter.AppendSql("     , PANJENGDATE = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')   ");
            }
            else
            {
                parameter.AppendSql("     , GBPRINT  = 'N'    ");
                parameter.AppendSql("     , TONGBODATE  = ''    ");
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

        public int UpdateWrtNobyFWrtNo(long nWrtNo, long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_DENTAL SET  ");
            parameter.AppendSql("       WRTNO = :WRTNO                  ");
            parameter.AppendSql(" WHERE WRTNO = :FWRTNO                 ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("FWRTNO", fnWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UPdatePanjengDatebyWrtNo(string strPanjengDate, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_DENTAL SET                      ");
            parameter.AppendSql("       PANJENGDATE =TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                                   ");

            parameter.Add("PANJENGDATE", strPanjengDate);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateDentSogenbyWrtNo(string strSpcPanjeng, long nPanDrNo, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_DENTAL SET                          ");
            parameter.AppendSql("       DENTSOGEN       = :DENTSOGEN                            ");
            parameter.AppendSql("     , DENTDOCT        = :DENTDOCT                             ");
            parameter.AppendSql(" WHERE WRTNO           = :WRTNO                                ");

            parameter.Add("DENTSOGEN", strSpcPanjeng);
            parameter.Add("DENTDOCT", nPanDrNo);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateAllbyWrtNo(HIC_RES_DENTAL item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_DENTAL SET                          ");
            parameter.AppendSql("       T_PAN_ETC       = :T_PAN_ETC                            ");
            parameter.AppendSql("     , T40_PAN1        = :T40_PAN1                             ");
            parameter.AppendSql("     , T40_PAN2        = :T40_PAN2                             ");
            parameter.AppendSql("     , T40_PAN3        = :T40_PAN3                             ");
            parameter.AppendSql("     , T40_PAN4        = :T40_PAN4                             ");
            parameter.AppendSql("     , T40_PAN5        = :T40_PAN5                             ");
            parameter.AppendSql("     , T40_PAN6        = :T40_PAN6                             ");
            parameter.AppendSql("     , T40_PAN1_NEW    = :T40_PAN1_NEW                         ");
            parameter.AppendSql("     , T40_PAN2_NEW    = :T40_PAN2_NEW                         ");
            parameter.AppendSql("     , T40_PAN3_NEW    = :T40_PAN3_NEW                         ");
            parameter.AppendSql("     , T40_PAN4_NEW    = :T40_PAN4_NEW                         ");
            parameter.AppendSql("     , T40_PAN5_NEW    = :T40_PAN5_NEW                         ");
            parameter.AppendSql("     , T40_PAN6_NEW    = :T40_PAN6_NEW                         ");
            parameter.AppendSql("     , T_PANJENG1      = :T_PANJENG1                           ");
            parameter.AppendSql("     , T_PANJENG_ETC   = :T_PANJENG_ETC                        ");
            parameter.AppendSql("     , T_PANJENG_SOGEN = :T_PANJENG_SOGEN                      ");
            parameter.AppendSql("     , RES_MUNJIN      = :RES_MUNJIN                           ");
            parameter.AppendSql("     , RES_RESULT      = :RES_RESULT                           ");
            parameter.AppendSql("     , RES_JOCHI       = :RES_JOCHI                            ");
            parameter.AppendSql("     , SANGDAM         = :SANGDAM                              ");
            parameter.AppendSql("     , CHIJURESULT     = :CHIJURESULT                          ");
            parameter.AppendSql("     , CHIJUSTAT1      = :CHIJUSTAT1                           ");
            parameter.AppendSql("     , CHIJUSTAT2      = :CHIJUSTAT2                           ");
            parameter.AppendSql("     , CHIJUSTAT3      = :CHIJUSTAT3                           ");
            parameter.AppendSql("     , CHIJUSTAT4      = :CHIJUSTAT4                           ");
            parameter.AppendSql("     , CHIJUSTATETC    = :CHIJUSTATETC                         ");
            parameter.AppendSql("     , PANJENGDRNO     = :PANJENGDRNO                          ");
            parameter.AppendSql("     , PANJENGDATE     = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql(" WHERE WRTNO           = :WRTNO                                ");

            parameter.Add("T_PAN_ETC", item.T_PAN_ETC);
            parameter.Add("T40_PAN1", item.T40_PAN1);
            parameter.Add("T40_PAN2", item.T40_PAN2);
            parameter.Add("T40_PAN3", item.T40_PAN3);
            parameter.Add("T40_PAN4", item.T40_PAN4);
            parameter.Add("T40_PAN5", item.T40_PAN5);
            parameter.Add("T40_PAN6", item.T40_PAN6);
            parameter.Add("T40_PAN1_NEW", item.T40_PAN1_NEW);
            parameter.Add("T40_PAN2_NEW", item.T40_PAN2_NEW);
            parameter.Add("T40_PAN3_NEW", item.T40_PAN3_NEW);
            parameter.Add("T40_PAN4_NEW", item.T40_PAN4_NEW);
            parameter.Add("T40_PAN5_NEW", item.T40_PAN5_NEW);
            parameter.Add("T40_PAN6_NEW", item.T40_PAN6_NEW);
            parameter.Add("T_PANJENG1", item.T_PANJENG1);
            parameter.Add("T_PANJENG_ETC", item.T_PANJENG_ETC);
            parameter.Add("T_PANJENG_SOGEN", item.T_PANJENG_SOGEN);
            parameter.Add("RES_MUNJIN", item.RES_MUNJIN);
            parameter.Add("RES_RESULT", item.RES_RESULT);
            parameter.Add("RES_JOCHI", item.RES_JOCHI);
            parameter.Add("SANGDAM", item.SANGDAM);
            parameter.Add("CHIJURESULT", item.CHIJURESULT);
            parameter.Add("CHIJUSTAT1", item.CHIJUSTAT1);
            parameter.Add("CHIJUSTAT2", item.CHIJUSTAT2);
            parameter.Add("CHIJUSTAT3", item.CHIJUSTAT3);
            parameter.Add("CHIJUSTAT4", item.CHIJUSTAT4);
            parameter.Add("CHIJUSTATETC", item.CHIJUSTATETC);
            parameter.Add("PANJENGDRNO", item.PANJENGDRNO);
            parameter.Add("PANJENGDATE", item.PANJENGDATE);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateResultbyWrtNo(string strGbn, long fnWRTNO, string strResult, string strBusik, string strGyomo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_DENTAL SET  ");
            switch (strGbn)             
            {
                case "E1":
                    parameter.AppendSql("       BUSIK1  = :RESULT       ");
                    break;
                case "E2":
                    parameter.AppendSql("       BUSIK2  = :RESULT       ");
                    break;
                case "E3":
                    parameter.AppendSql("       BUSIK3  = :RESULT       ");
                    break;
                case "E4":
                    parameter.AppendSql("       BUSIK4  = :RESULT       ");
                    break;
                case "E5":
                    parameter.AppendSql("       BUSIK5  = :RESULT       ");
                    break;
                case "K1":
                    parameter.AppendSql("       GYOMO1  = :RESULT       ");
                    break;
                case "K2":
                    parameter.AppendSql("       GYOMO2  = :RESULT       ");
                    break;
                case "K3":
                    parameter.AppendSql("       GYOMO3  = :RESULT       ");
                    break;
                case "K4":
                    parameter.AppendSql("       GYOMO4  = :RESULT       ");
                    break;
                default:
                    break;
            }
            if (strBusik == "OK")
            {
                parameter.AppendSql("     , BUSIK0  = ''                ");
            }
            if (strGyomo == "OK")
            {
                parameter.AppendSql("     , GYOMO0  = ''                ");
            }
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO            ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("RESULT", strResult);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateBusikGyomobyWrtNo(string strGbn, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_DENTAL SET  ");
            if (strGbn == "E0")
            {
                parameter.AppendSql("       BUSIK0      = 'Y'           ");
                parameter.AppendSql("     , BUSIK1      = ''            ");
                parameter.AppendSql("     , BUSIK2      = ''            ");
                parameter.AppendSql("     , BUSIK3      = ''            ");
                parameter.AppendSql("     , BUSIK4      = ''            ");
                parameter.AppendSql("     , BUSIK5      = ''            ");
            }
            if (strGbn == "T0")
            {
                parameter.AppendSql("       GYOMO0      = 'Y'           ");
                parameter.AppendSql("     , GYOMO1      = ''            ");
                parameter.AppendSql("     , GYOMO2      = ''            ");
                parameter.AppendSql("     , GYOMO3      = ''            ");
                parameter.AppendSql("     , GYOMO4      = ''            ");
            }            
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO            ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public string GetRowIdbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_DENTAL              ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                         ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_RES_DENTAL GetPanjengDatebyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PANJENGDATE, 'YYYY-MM-DD') PANJENGDATE, PANJENGDRNO ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_DENTAL                                  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                             ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_RES_DENTAL>(parameter);
        }

        public HIC_RES_DENTAL GetItemByWrtnoPanjenGDrNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PANJENGDATE, 'YYYY-MM-DD') PANJENGDATE, PANJENGDRNO ");
            parameter.AppendSql("     , RES_JOCHI                                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_DENTAL                                  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                             ");
            parameter.AppendSql("   AND PANJENGDRNO > 0                                             "); //판정완료

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReaderSingle<HIC_RES_DENTAL>(parameter);
        }

        public int GetCountbyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_DENTAL  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO             ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateAll(HIC_RES_DENTAL item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_DENTAL SET  ");
            parameter.AppendSql("       OPDDNT      = :OPDDNT           ");
            parameter.AppendSql("     , DNTSTATUS   = :DNTSTATUS        ");
            parameter.AppendSql("     , T_HABIT1    = :T_HABIT1         ");
            parameter.AppendSql("     , T_HABIT2    = :T_HABIT2         ");
            parameter.AppendSql("     , T_HABIT4    = :T_HABIT4         ");
            parameter.AppendSql("     , T_HABIT5    = :T_HABIT5         ");
            parameter.AppendSql("     , T_HABIT6    = :T_HABIT6         ");
            parameter.AppendSql("     , T_HABIT7    = :T_HABIT7         ");
            parameter.AppendSql("     , T_HABIT8    = :T_HABIT8         ");
            parameter.AppendSql("     , T_HABIT9    = :T_HABIT9         ");
            parameter.AppendSql("     , T_STAT1     = :T_STAT1          ");
            parameter.AppendSql("     , T_STAT2     = :T_STAT2          ");
            parameter.AppendSql("     , T_FUNCTION1 = :T_FUNCTION1      ");
            parameter.AppendSql("     , T_JILBYUNG1 = :T_JILBYUNG1      ");
            parameter.AppendSql("     , T_JILBYUNG2 = :T_JILBYUNG2      ");
            parameter.AppendSql("     , MUNJINETC   = :MUNJINETC        ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO            ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("OPDDNT", item.OPDDNT, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("DNTSTATUS", item.DNTSTATUS, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT1", item.T_HABIT1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT2", item.T_HABIT2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT4", item.T_HABIT4, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT5", item.T_HABIT5, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT6", item.T_HABIT6, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT7", item.T_HABIT7, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT8", item.T_HABIT8, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT9", item.T_HABIT9, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT1", item.T_STAT1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT2", item.T_STAT2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_FUNCTION1", item.T_FUNCTION1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_JILBYUNG1", item.T_JILBYUNG1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_JILBYUNG2", item.T_JILBYUNG2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("MUNJINET", item.MUNJINETC);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_RES_DENTAL item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_RES_DENTAL                                                             ");
            parameter.AppendSql("       (WRTNO, OPDDNT, DNTSTATUS, T_HABIT1, T_HABIT2, T_HABIT4, T_HABIT5, T_HABIT6         ");
            parameter.AppendSql("     , T_HABIT7, T_HABIT8, T_HABIT9, T_STAT1, T_STAT2, T_FUNCTION1                                 ");
            parameter.AppendSql("     , T_JILBYUNG1, T_JILBYUNG2, MUNJINETC)                                                        ");
            parameter.AppendSql("VALUES                                                                                             ");
            parameter.AppendSql("       (:WRTNO, :OPDDNT, :DNTSTATUS, :T_HABIT1, :T_HABIT2, :T_HABIT4, :T_HABIT5, :T_HABIT6");
            parameter.AppendSql("     , :T_HABIT7, :T_HABIT8, :T_HABIT9, :T_STAT1, :T_STAT2, :T_FUNCTION1                           ");
            parameter.AppendSql("     , :T_JILBYUNG1, :T_JILBYUNG2, :MUNJINETC)                                                     ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("OPDDNT", item.OPDDNT, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("DNTSTATUS", item.DNTSTATUS, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT1", item.T_HABIT1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT2", item.T_HABIT2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT4", item.T_HABIT4, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT5", item.T_HABIT5, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT6", item.T_HABIT6, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT7", item.T_HABIT7, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT8", item.T_HABIT8, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_HABIT9", item.T_HABIT9, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT1", item.T_STAT1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_STAT2", item.T_STAT2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_FUNCTION1", item.T_FUNCTION1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_JILBYUNG1", item.T_JILBYUNG1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("T_JILBYUNG2", item.T_JILBYUNG2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("MUNJINETC", item.MUNJINETC);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_DENTAL SET  ");
            parameter.AppendSql("       GBMUNJIN2 = ''                  ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO              ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int DeletebyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_RES_DENTAL  ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO         ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public HIC_RES_DENTAL GetPanjengDrnoDate(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANJENGDRNO, PANJENGDATE    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_DENTAL  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReaderSingle<HIC_RES_DENTAL>(parameter);
        }

        public long GetPanjengDrNobyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANJENGDRNO                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_DENTAL  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO             ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteScalar<long>(parameter);
        }

        public int MunjinResultUpDate(HIC_RES_DENTAL item2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_DENTAL                      ");
            parameter.AppendSql("   SET PANJENGDATE = TO_DATE(:PANDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("     , T_PANJENG1  = :T_PANJENG1                       ");
            parameter.AppendSql("     , RES_MUNJIN  = :RES_MUNJIN                       ");
            parameter.AppendSql("     , RES_RESULT  = :RES_RESULT                       ");
            parameter.AppendSql("     , RES_JOCHI   = :RES_JOCHI                        ");
            parameter.AppendSql("     , SANGDAM     = :SANGDAM                          ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO                            ");

            #region Query 변수대입
            parameter.Add("PANDATE",   item2.PANJENGDATE);
            parameter.Add("T_PANJENG1",    item2.T_PANJENG1);
            parameter.Add("RES_MUNJIN",   item2.RES_MUNJIN);
            parameter.Add("RES_RESULT",   item2.RES_RESULT);
            parameter.Add("RES_JOCHI",   item2.RES_JOCHI);
            parameter.Add("SANGDAM", item2.SANGDAM);
            parameter.Add("WRTNO",     item2.WRTNO);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public int UpdateTongboByWrtno(long argWrtno,string argTongGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_DENTAL SET  ");
            parameter.AppendSql("       GBPRINT = 'Y'                   ");
            parameter.AppendSql("       , TONGBODATE = SYSDATE          ");
            parameter.AppendSql("       , TONGBOGBN = :TONGBOGBN        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                 ");

            parameter.Add("WRTNO", argWrtno);
            parameter.Add("TONGBOGBN", argTongGbn);

            return ExecuteNonQuery(parameter);
        }
    }
}
