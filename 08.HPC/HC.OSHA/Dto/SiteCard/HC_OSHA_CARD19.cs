using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 사업장카드 위탁업무수행일지
    /// </summary>
    public class HC_OSHA_CARD19 : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long SITE_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long ESTIMATE_ID { get; set; }

        /// <summary>
        /// 방문일자
        /// </summary>
        public DateTime? REGDATE { get; set; }
        /// <summary>
        /// 자격
        /// </summary>
        public string CERT { get; set; }
        /// <summary>
        /// 성명
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 업무수행내용
        /// </summary>
        public string CONTENT { get; set; }
        /// <summary>
        /// 문제점 및 개선지도사항
        /// </summary>
        public string OPINION { get; set; }

        /// <summary>
        /// 개선:0, 진행:1, 미개선:2
        /// </summary>
        public string STATUS { get; set; }

        /// <summary>
        /// 서명
        /// </summary>
        public string SITESIGN { get; set; }
        /// <summary>
        /// 서명여부
        /// </summary>
        public string ISSIGN { get; set; }
    }
}
