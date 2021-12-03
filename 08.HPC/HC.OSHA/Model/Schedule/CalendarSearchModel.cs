using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComBase.Mvc;
using ComBase.Mvc.Validation;
using System;

namespace HC.OSHA.Model
{
    /// <summary>
    /// 달력 검색 조건
    /// </summary>
    public enum CalendarSearchType
    {
        ALL, VISIT, UNVISIT
    }
    public enum CalendarViewType
    {
        MONTH, DAY
    }
    public class CalendarSearchModel 
    {
        public CalendarSearchModel()
        {
          //  this.CalendarSearchType = CalendarSearchType.ALL;
        }
        /// <summary>
        /// 방문일정 검색조건
        /// </summary>
        public CalendarSearchType CalendarSearchType { get; set; }

        public CalendarViewType CalendarViewType { get; set; }
        /// <summary>
        /// 담당자
        /// </summary>
        public string VisitUser { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
