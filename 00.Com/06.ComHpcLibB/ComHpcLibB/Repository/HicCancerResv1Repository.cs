namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicCancerResv1Repository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicCancerResv1Repository()
        {
        }

        public int Insert(HIC_CANCER_RESV1 item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_CANCER_RESV1                                                   ");
            parameter.AppendSql("       (JOBDATE, WEEK, UGI, GFS, GFSH, MAMMO, RECTUM, SONO, WOMB, WOMB1, UGI1, GFS1        ");
            parameter.AppendSql("     , GFSH1, MAMMO1, RECTUM1, SONO1, BOHUM, BOHUM1, CT, CT1, REMARK, ENTSABUN             ");
            parameter.AppendSql("     , ENTTIME, LUNG_SANGDAM, LUNG_SANGDAM1 )                                              ");
            parameter.AppendSql("VALUES                                                                                     ");
            parameter.AppendSql("       (TO_DATE(:JOBDATE, 'YYYY-MM-DD'), :WEEK, :UGI, :GFS, :GFSH, :MAMMO, :RECTUM, :SONO  ");
            parameter.AppendSql("     , :WOMB, :WOMB1, :UGI1, :GFS1, :GFSH1, :MAMMO1, :RECTUM1, :SONO1, :BOHUM, :BOHUM1     ");
            parameter.AppendSql("     , :CT, :CT1, :REMARK, :ENTSABUN, SYSDATE, :LUNG_SANGDAM, :LUNG_SANGDAM1 )             ");

            parameter.Add("JOBDATE", item.JOBDATE);
            parameter.Add("WEEK", item.WEEK);
            parameter.Add("UGI", item.UGI);
            parameter.Add("GFS", item.GFS);
            parameter.Add("GFSH", item.GFSH);
            parameter.Add("MAMMO", item.MAMMO);
            parameter.Add("RECTUM", item.RECTUM);
            parameter.Add("SONO", item.SONO);
            parameter.Add("WOMB", item.WOMB);
            parameter.Add("WOMB1", item.WOMB1);
            parameter.Add("UGI1", item.UGI1);
            parameter.Add("GFS1", item.GFS1);
            parameter.Add("GFSH1", item.GFSH1);
            parameter.Add("MAMMO1", item.MAMMO1);
            parameter.Add("RECTUM1", item.RECTUM1);
            parameter.Add("SONO1", item.SONO1);
            parameter.Add("BOHUM", item.BOHUM);
            parameter.Add("BOHUM1", item.BOHUM1);
            parameter.Add("CT", item.CT);
            parameter.Add("CT1", item.CT1);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("LUNG_SANGDAM", item.LUNG_SANGDAM);
            parameter.Add("LUNG_SANGDAM1", item.LUNG_SANGDAM1);

            return ExecuteNonQuery(parameter);
        }

        public HIC_CANCER_RESV1 GetItembyJobDate(string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT UGI,UGI1,GFS,GFS1,GFSH,GFSH1               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV1               ");
            parameter.AppendSql(" WHERE JOBDATE = TO_DATE(:JOBDATE,'YYYY-MM-DD')   ");

            parameter.Add("JOBDATE", strDate);

            return ExecuteReaderSingle<HIC_CANCER_RESV1>(parameter);
        }

        public List<HIC_CANCER_RESV1> GetItembyJobDate(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JOBDATE, 'YYYY-MM-DD') JOBDATE, WEEK, UGI, GFS,GFSH, MAMMO, RECTUM          ");
            parameter.AppendSql("     , SONO,WOMB,UGI1, GFS1,GFSH1, MAMMO1                                                  ");
            parameter.AppendSql("     , RECTUM1, SONO1,WOMB1,BOHUM,BOHUM1, CT, CT1, REMARK, REMARK1, ENTSABUN, ENTTIME, ROWID        ");
            parameter.AppendSql("     , LUNG_SANGDAM, LUNG_SANGDAM1                                                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_RESV1                                                        ");
            parameter.AppendSql(" WHERE JOBDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                            ");
            parameter.AppendSql("   AND JOBDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')                                            ");
            parameter.AppendSql(" ORDER BY JobDate                                                                          ");
            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteReader<HIC_CANCER_RESV1>(parameter);
        }

        public int Update2(HIC_CANCER_RESV1 item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CANCER_RESV1 SET                            ");
            parameter.AppendSql("       UGI1          = :UGI1                                       ");
            parameter.AppendSql("     , GFS1          = :GFS1                                       ");
            parameter.AppendSql("     , GFSH1         = :GFSH1                                      ");
            parameter.AppendSql("     , MAMMO1        = :MAMMO1                                     ");
            parameter.AppendSql("     , RECTUM1       = :RECTUM1                                    ");
            parameter.AppendSql("     , SONO1         = :SONO1                                      ");
            parameter.AppendSql("     , WOMB1         = :WOMB1                                      ");
            parameter.AppendSql("     , BOHUM1        = :BOHUM1                                     ");
            parameter.AppendSql("     , CT1           = :CT1                                        ");
            parameter.AppendSql("     , REMARK1       = :REMARK1                                    ");
            parameter.AppendSql("     , ENTSABUN      = :ENTSABUN                                   ");
            parameter.AppendSql("     , ENTTIME       = SYSDATE                                     ");
            parameter.AppendSql("     , LUNG_SANGDAM1 = :LUNG_SANGDAM1                              ");
            parameter.AppendSql(" WHERE ROWID         = :RID                                        ");

            parameter.Add("UGI1", item.UGI1);
            parameter.Add("GFS1", item.GFS1);
            parameter.Add("GFSH1", item.GFSH1);
            parameter.Add("MAMMO1", item.MAMMO1);
            parameter.Add("RECTUM1", item.RECTUM1);
            parameter.Add("SONO1", item.SONO1);
            parameter.Add("WOMB1", item.WOMB1);
            parameter.Add("BOHUM1", item.BOHUM1);
            parameter.Add("CT1", item.CT1);
            parameter.Add("REMARK1", item.REMARK1);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("LUNG_SANGDAM1", item.LUNG_SANGDAM1);
            parameter.Add("RID", item.ROWID);

            return ExecuteNonQuery(parameter);
        }

        public int Update1(HIC_CANCER_RESV1 item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CANCER_RESV1 SET                        ");
            parameter.AppendSql("       UGI          = :UGI                                     ");
            parameter.AppendSql("     , GFS          = :GFS                                     ");
            parameter.AppendSql("     , GFSH         = :GFSH                                    ");
            parameter.AppendSql("     , MAMMO        = :MAMMO                                   ");
            parameter.AppendSql("     , RECTUM       = :RECTUM                                  ");
            parameter.AppendSql("     , SONO         = :SONO                                    ");
            parameter.AppendSql("     , WOMB         = :WOMB                                    ");
            parameter.AppendSql("     , BOHUM        = :BOHUM                                   ");
            parameter.AppendSql("     , CT           = :CT                                      ");
            parameter.AppendSql("     , REMARK       = :REMARK                                  ");
            parameter.AppendSql("     , ENTSABUN     = :ENTSABUN                                ");
            parameter.AppendSql("     , ENTTIME      = SYSDATE                                  ");
            parameter.AppendSql("     , LUNG_SANGDAM = :LUNG_SANGDAM                            ");
            parameter.AppendSql(" WHERE ROWID        = :RID                                     ");

            parameter.Add("UGI", item.UGI);
            parameter.Add("GFS", item.GFS);
            parameter.Add("GFSH", item.GFSH);
            parameter.Add("MAMMO", item.MAMMO);
            parameter.Add("RECTUM", item.RECTUM);
            parameter.Add("SONO", item.SONO);
            parameter.Add("WOMB", item.WOMB); 
            parameter.Add("BOHUM", item.BOHUM);
            parameter.Add("CT", item.CT);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("LUNG_SANGDAM", item.LUNG_SANGDAM);
            parameter.Add("RID", item.ROWID);

            return ExecuteNonQuery(parameter);
        }
    }
}
