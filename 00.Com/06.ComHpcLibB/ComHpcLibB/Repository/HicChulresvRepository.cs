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
    public class HicChulresvRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicChulresvRepository()
        {
        }

        public List<CalendarEventModel> SearchSchedule(CalendarSearchModel model)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT LTDCODE, RDATE, STARTTIME, LTDNAME, INWON, SPECIAL, REMARK  ");
            parameter.AppendSql("      ,GBCHANGE, RTIME                                             ");
            parameter.AppendSql("  FROM ADMIN.HIC_CHULRESV                                    ");
            parameter.AppendSql(" WHERE 1 = 1                                                       ");
            parameter.AppendSql("   AND RDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("   AND RDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                      ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                                              ");
            parameter.AppendSql("   AND DELDATE IS NULL                                             ");
            parameter.AppendSql(" ORDER BY RDATE, STARTTIME                                         ");

            parameter.Add("FDATE", model.StartDate);
            parameter.Add("TDATE", model.EndDate);
            parameter.Add("GUBUN", model.ViewGubun);

            return ExecuteReader<CalendarEventModel>(parameter);
        }

        public List<HIC_CHULRESV> GetListByDateGubun(string argStartDate, string argLastDate, string argGubun, long nLtdCode, string fstrDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(RDATE, 'YYYY-MM-DD') AS RDATE, LTDCODE              ");
            parameter.AppendSql("      ,ADMIN.FC_HIC_LTDNAME(LTDCODE) LTDNAME                 ");
            parameter.AppendSql("      ,RTIME, INWON, STARTTIME, SPECIAL, REMARK, JOBSABUN, ENTTIME ");
            parameter.AppendSql("      ,GBCHANGE, LTDNAME AS PLACE, ROWID AS RID                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_CHULRESV                                    ");
            parameter.AppendSql(" WHERE 1 = 1                                                       ");
            if (fstrDate.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND RDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                      ");
                parameter.AppendSql("   AND RDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                      ");
            }
            else
            {
                parameter.AppendSql("   AND RDATE = TO_DATE(:RDATE, 'YYYY-MM-DD')                      ");
            }

            if (nLtdCode > 0)
            {
                parameter.AppendSql("   AND LTDCODE =:LTDCODE                      ");
            }
            
            parameter.AppendSql("   AND GUBUN = :GUBUN                                              ");
            parameter.AppendSql("   AND DELDATE IS NULL                                             ");
            parameter.AppendSql(" ORDER BY RDATE, RTIME                                             ");

            if (fstrDate.IsNullOrEmpty())
            { 
                parameter.Add("FDATE", argStartDate);
                parameter.Add("TDATE", argLastDate);
            }
            else
            {
                parameter.Add("RDATE", fstrDate);
            }

            if (nLtdCode > 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            parameter.Add("GUBUN", argGubun);

            return ExecuteReader<HIC_CHULRESV>(parameter);
        }

        public void UpDate(HIC_CHULRESV item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" UPDATE ADMIN.HIC_CHULRESV                   ");
            parameter.AppendSql("    SET RDATE      =TO_DATE(:RDATE,'YYYY-MM-DD')   ");
            parameter.AppendSql("       ,LTDCODE    =:LTDCODE                       ");
            parameter.AppendSql("       ,LTDNAME    =:LTDNAME                       ");
            parameter.AppendSql("       ,RTIME      =:RTIME                         ");
            parameter.AppendSql("       ,INWON      =:INWON                         ");
            parameter.AppendSql("       ,STARTTIME  =:STARTTIME                     ");
            parameter.AppendSql("       ,SPECIAL    =:SPECIAL                       ");
            parameter.AppendSql("       ,REMARK     =:REMARK                        ");
            parameter.AppendSql("       ,GUBUN      =:GUBUN                         ");
            parameter.AppendSql("       ,JOBSABUN   =:JOBSABUN                      ");
            parameter.AppendSql("       ,ENTTIME    =SYSDATE                        ");
            parameter.AppendSql("       ,GBCHANGE   = 'Y'                           ");
            parameter.AppendSql("  WHERE ROWID =:RID                                ");

            parameter.Add("RDATE",item.RDATE);
            parameter.Add("LTDCODE",item.LTDCODE);
            parameter.Add("LTDNAME",item.LTDNAME);
            parameter.Add("RTIME",item.RTIME);
            parameter.Add("INWON",item.INWON);
            parameter.Add("STARTTIME",item.STARTTIME);
            parameter.Add("SPECIAL",item.SPECIAL);
            parameter.Add("REMARK",item.REMARK);
            parameter.Add("GUBUN",item.GUBUN);
            parameter.Add("JOBSABUN",item.JOBSABUN);
            parameter.Add("RID", item.RID);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_CHULRESV> GetCheckup(string dtpFDate, string strNextDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT LTDCODE,TO_CHAR(RDATE,'YYYY-MM-DD') RDATE  ");

            parameter.AppendSql("   FROM    ADMIN.HIC_CHULRESV");

            parameter.AppendSql("   WHERE RDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND RDATE <= TO_DATE(:STRNEXTDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND LTDCODE IS NOT NULL  ");
            parameter.AppendSql("   ORDER BY RDATE  ");


            parameter.Add("FDATE", dtpFDate);
            parameter.Add("STRNEXTDATE", strNextDate);

            return ExecuteReader<HIC_CHULRESV>(parameter);
        }

        public void Insert(HIC_CHULRESV item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" INSERT INTO ADMIN.HIC_CHULRESV (                                ");
            parameter.AppendSql("        RDATE, LTDCODE, LTDNAME, RTIME, INWON, STARTTIME, SPECIAL      ");
            parameter.AppendSql("       ,REMARK, JOBSABUN, ENTTIME, GUBUN                               ");
            parameter.AppendSql(" ) VALUES (                                                            ");
            parameter.AppendSql("        :RDATE, :LTDCODE, :LTDNAME, :RTIME, :INWON, :STARTTIME, :SPECIAL");
            parameter.AppendSql("       ,:REMARK, :JOBSABUN, SYSDATE, :GUBUN )                          ");

            parameter.Add("RDATE", item.RDATE);
            parameter.Add("LTDCODE", item.LTDCODE);
            parameter.Add("LTDNAME", item.LTDNAME);
            parameter.Add("RTIME", item.RTIME);
            parameter.Add("INWON", item.INWON);
            parameter.Add("STARTTIME", item.STARTTIME);
            parameter.Add("SPECIAL", item.SPECIAL);
            parameter.Add("REMARK", item.REMARK);            
            parameter.Add("JOBSABUN", item.JOBSABUN);
            parameter.Add("GUBUN", item.GUBUN);

            ExecuteNonQuery(parameter);
        }

        public int Delete(string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" UPDATE ADMIN.HIC_CHULRESV   ");
            parameter.AppendSql("    SET DELDATE = TRUNC(SYSDATE)   ");
            parameter.AppendSql("  WHERE ROWID =:RID                ");

            parameter.Add("RID", argRowid);

            return ExecuteNonQuery(parameter);
        }
    }
}
