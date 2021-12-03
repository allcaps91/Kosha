using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComBase.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComHpcLibB.Repository
{
    public class HeaResvCalRepository :BaseRepository
    {
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
    }
}
