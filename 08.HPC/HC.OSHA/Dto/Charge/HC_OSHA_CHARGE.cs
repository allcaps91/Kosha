namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Enums;
    using ComBase.Mvc.Validation;
    using System;
    using System.Drawing;


    /// <summary>
    /// 
    /// </summary>
    public class HC_OSHA_CHARGE : BaseDto
    {
        public long ID { get; set; }
        public long SITE_ID { get; set; }
        public long ESTIMATE_ID { get; set; }
        public long VISIT_ID { get; set; }
        public long VISIT_PRICE_ID { get; set; }
        public DateTime CHARGEDATE { get; set; }
        public long WORKERCOUNT { get; set; }
        public long UNITPRICE { get; set; }
        public long TOTALPRICE { get; set; }
      //  public string ISVISIT { get; set; }

        /// <summary>
        /// 청구 완료여부
        /// </summary>
        public string ISCOMPLETE { get; set; }
        public string REMARK { get; set; }

        public IsDeleted ISDELETED { get; set; }

        public DateTime? MODIFIED { get; set; }
        public string MODIFIEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }
    }
}
