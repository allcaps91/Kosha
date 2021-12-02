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
    public class HicResBohum2Repository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicResBohum2Repository()
        {
        }

        public HIC_RES_BOHUM2 GetCountbyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT WRTNO, GBCHEST, GBCYCLE, GBGOJI, GBLIVER, GBKIDNEY, GBANEMIA, GBDIABETES                       ");
            parameter.AppendSql("      , GBETC, CHEST1, CHEST2, CHEST3, CHEST_RES, CYCLE1, CYCLE2, CYCLE3, CYCLE4, CYCLE_RES, GOJI1     ");
            parameter.AppendSql("      , GOJI2, GOJI_RES, LIVER11, LIVER12, LIVER13, LIVER14, LIVER15, LIVER16, LIVER17, LIVER18        ");
            parameter.AppendSql("      , LIVER19, LIVER20, LIVER_RES, KIDNEY1, KIDNEY2, KIDNEY3, KIDNEY4, KIDNEY5, KIDNEY_RES           ");
            parameter.AppendSql("      , ANEMIA1, AMEMIA2, AMEMIA3, AMEMIA_RES, DIABETES1, DIABETES2, DIABETES3, DIABETES_RES           ");
            parameter.AppendSql("      , ETC_RES, ETC_EXAM, PANJENG, PANJENG_D1, PANJENG_SO1, PANJENG_SO2, PANJENGDATE, TONGBOGBN       ");
            parameter.AppendSql("      , TONGBODATE, PANJENGDRNO, SOGEN, GUNDATE, PANJENG_SO3, PANJENG_D11, PANJENG_D12                 ");
            parameter.AppendSql("      , GBPRINT, WORKYN, CHEST4, PANJENG_D2, LIVER21, LIVER22, DIABETES_RES_CARE, CYCLE_RES_CARE       ");
            parameter.AppendSql("      , T66_MEM, T_SMOKE1, T_SMOKE2, T_SMOKE3, T_DRINK1, T_DRINK2, T_DRINK3, T_HELTH1, T_HELTH2        ");
            parameter.AppendSql("      , T_HELTH3, T_HELTH4, T_HELTH5, T_DIET1, T_DIET2, T_DIET3, T_DIET4, T_DIET5, T_DIET6             ");
            parameter.AppendSql("      , T_DIET7, T_DIET8, T_DIET9, T_DIET10, T_DIET11, T_BIMAN1, T_BIMAN2, T_BIMAN3, T_BIMAN4          ");
            parameter.AppendSql("      , T_BIMAN5, T_BIMAN6, T_BIMAN7, T_BIMAN8, T_BIMAN9, T_CESD, T_GDS, T_KDSQC, T_SANGDAM            ");
            parameter.AppendSql("      , PANJENGSAHU1, PANJENGSAHU2, T_SANGDAM_1, PRTSABUN, SOGENB, GBGONGHU,ROWID RID                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM2                                                                      ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                                                 ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReaderSingle<HIC_RES_BOHUM2>(parameter);
        }

        public int UpdateCycleDiabetesbyWrtNo(HIC_RES_BOHUM2 item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM2 SET                      ");            
            parameter.AppendSql("       CYCLE1 = :CYCLE1                                    ");            
            parameter.AppendSql("     , CYCLE2 = :CYCLE2                                    ");            
            parameter.AppendSql("     , CYCLE3 = :CYCLE3                                    ");            
            parameter.AppendSql("     , CYCLE4 = :CYCLE4                                    ");
            if (!item.T_SANGDAM_1.IsNullOrEmpty())
            {
                parameter.AppendSql("     , T_SANGDAM_1 = :T_SANGDAM_1                      ");
            }
            parameter.AppendSql("     , DIABETES1 = :DIABETES1                              ");
            parameter.AppendSql("     , DIABETES2 = :DIABETES2                              ");
            parameter.AppendSql("     , DIABETES3 = :DIABETES3                              ");
            parameter.AppendSql("     , GBGONGHU  = :GBGONGHU                               ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                     ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("CYCLE1", item.CYCLE1);
            parameter.Add("CYCLE2", item.CYCLE2);
            parameter.Add("CYCLE3", item.CYCLE3);
            parameter.Add("CYCLE4", item.CYCLE4);
            if (!item.T_SANGDAM_1.IsNullOrEmpty())
            {
                parameter.Add("T_SANGDAM_1", item.T_SANGDAM_1);
            }
            parameter.Add("DIABETES1", item.DIABETES1);
            parameter.Add("DIABETES2", item.DIABETES2);
            parameter.Add("DIABETES3", item.DIABETES3);
            parameter.Add("GBGONGHU", item.GBGONGHU);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public void UpdateTongBoInfobyWrtNo(long fnWRTNO, string strDate, string strGbn, string strSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM2 SET                      ");
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

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM2 SET                      ");
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

        public int UpdateTSangdam1byWrtNo(string strTsangdam1, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM2 SET  ");
            parameter.AppendSql("       T_SANGDAM_1 = :T_SANGDAM_1      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("T_SANGDAM_1", strTsangdam1);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public string GetTSangdam1byWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT T_SANGDAM_1                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM2   ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO              ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int DeletebyWrtNo(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_RES_BOHUM2  ");
            parameter.AppendSql(" WHERE WRTNO  =:WRTNO              ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateWrtNobyFWrtNo(long nWrtNo, long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM2 SET  ");
            parameter.AppendSql("       WRTNO = :WRTNO                  ");
            parameter.AppendSql(" WHERE WRTNO = :FWRTNO                 ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("FWRTNO", fnWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateDiabetesbyWrtNo(HIC_SANGDAM_NEW item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM2 SET          ");
            parameter.AppendSql("       DIABETES_1  = :DIABETES_1               ");
            parameter.AppendSql("     , DIABETES_2  = :DIABETES_2               ");
            parameter.AppendSql("     , CYCLE_1     = :CYCLE_1                  ");
            parameter.AppendSql("     , CYCLE_2     = :CYCLE_2                  ");
            parameter.AppendSql("     , GBSIKSA     = :GBSIKSA                  ");
            parameter.AppendSql("     , REMARK      = :REMARK                   ");
            parameter.AppendSql("     , GBSTS       = :GBSTS                    ");
            parameter.AppendSql("     , SANGDAMDRNO = :SANGDAMDRNO              ");
            parameter.AppendSql("     , ENTSABUN    = :ENTSABUN                 ");
            parameter.AppendSql("     , ENTTIME     = SYSDATE                   ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO                    ");


            parameter.Add("DIABETES_1", item.DIABETES_1);
            parameter.Add("DIABETES_2", item.DIABETES_2);
            parameter.Add("CYCLE_1", item.CYCLE_1);
            parameter.Add("CYCLE_2", item.CYCLE_2);
            parameter.Add("GBSIKSA", item.GBSIKSA);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("GBSTS", item.GBSTS);
            parameter.Add("SANGDAMDRNO", item.SANGDAMDRNO);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyWrtNo(HIC_RES_BOHUM2 item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RES_BOHUM2 SET          ");
            parameter.AppendSql("       DIABETES_RES      = :DIABETES_RES       ");
            parameter.AppendSql("     , DIABETES_RES_CARE = :DIABETES_RES_CARE  ");
            parameter.AppendSql("     , CYCLE_RES         = :CYCLE_RES          ");
            parameter.AppendSql("     , CYCLE_RES_CARE    = :CYCLE_RES_CARE     ");
            parameter.AppendSql("     , T_SANGDAM         = :T_SANGDAM          ");
            parameter.AppendSql(" WHERE WRTNO             = :WRTNO              ");

            parameter.Add("DIABETES_RES", item.DIABETES_RES);
            parameter.Add("DIABETES_RES_CARE", item.DIABETES_RES_CARE);
            parameter.Add("CYCLE_RES", item.CYCLE_RES);
            parameter.Add("CYCLE_RES_CARE", item.CYCLE_RES_CARE);
            parameter.Add("T_SANGDAM", item.T_SANGDAM);
            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public string GetT_SangdambyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT T_SANGDAM                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM2  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO             ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int GetCountDoublebyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT COUNT('X') CNT             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM2  ");
            parameter.AppendSql("   AND WRTNO  = :WRTNO             ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<int>(parameter);
        }
    }
}
