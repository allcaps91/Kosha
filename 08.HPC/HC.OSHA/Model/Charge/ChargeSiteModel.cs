namespace HC.OSHA.Model
{
    using ComBase.Mvc;
    using HC.Core.Model;
    using System;


    /// <summary>
    /// 청구 사이트
    /// </summary>
    public class ChargeSiteModel : BaseDto
    {
        public long SITEID { get; set; }
        public long PARENTSITE_ID { get; set; }
        public string SITENAME { get; set; }
    
        public long UNITPRICE { get; set; }

        public long TOTALPRICE { get; set; }
        /// <summary>
        /// 계약일
        /// </summary>
        public string CONTRACTDATE { get; set; }
        

        //발생일자
        public string BDATE { get; set; }
        public string DAMNAME { get; set; }
        public string REMARK { get; set; }
        /// <summary>
        /// 미수번호
        /// </summary>
        public long WRTNO { get; set; }
        /// <summary>
        /// 미수번호
        /// </summary>
        public long ESTIMATE_ID { get; set; }
    }
}
