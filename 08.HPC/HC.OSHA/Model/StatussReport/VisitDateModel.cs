using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HC.OSHA.Model
{
    public class VisitDateModel : BaseDto
    {

        public string VisitYear { get; set; }

        /// <summary>
        /// 방문일자
        /// </summary>
        public string VisitDate { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 사업장보고서 아이디
        /// </summary>
        public long ID { get; set; }
    }
}
