using ComBase.Mvc;
using ComBase.Mvc.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{
    /// <summary>
    /// 질병유소견자 대행 빌드 DTO
    /// </summary>
    public class HIC_OSHA_PANJEONG : BaseDto
    {
        public string WORKER_ID { get; set; }
        public long SITE_ID { get; set; }
        public string SITENAME { get; set; }

        public string YEAR { get; set; }
        public string PANO { get; set; }
        public string TASK { get; set; }
        public string NAME { get; set; }
        public string SEX { get; set; }
        public string AGE { get; set; }
        public string INJA { get; set; }
        public string JIPYO { get; set; }
        public string JOBYEAR { get; set; }
        public string PANJEONG { get; set; }
        public string ISSPECIAL { get; set; }
        public string OPINION { get; set; }
        public string RESULT { get; set; }
        public string GRADE { get; set; }
        public DateTime CREATED { get; set; }
    }
}
