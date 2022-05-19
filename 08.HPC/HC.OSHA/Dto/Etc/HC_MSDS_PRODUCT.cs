namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    using System.Drawing;

    /// <summary>
    /// 
    /// </summary>
    public class HC_MSDS_PRODUCT : BaseDto
    {

        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 제품명
        /// </summary>
        [MTSNotEmpty]
        public string PRODUCTNAME { get; set; }

        /// <summary>
        /// 용도
        /// </summary>
        public string USAGE { get; set; }

        /// <summary>
        /// 제조회사명
        /// </summary>
        public string MANUFACTURER { get; set; }

        /// <summary>
        /// MSDS코드
        /// </summary>
        public string MSDSCODE { get; set; }

        /// <summary>
        /// 물질목록
        /// </summary>
        public string MSDSLIST { get; set; }

        public DateTime? MODIFIED { get; set; }
        public string MODIFEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HC_MSDS_PRODUCT()
        {
        }
    }
}
