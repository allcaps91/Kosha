namespace HC.OSHA.Model
{
    using ComBase.Mvc;
    using ComHpcLibB.Model;
    using HC.Core.Model;
    using System;
    
    
    /// <summary>
    /// 견적 + 계약
    /// </summary>
    public class HC_ESTIMATE_MODEL : BaseDto, IEstimateModel
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; }

        /// <summary>
        /// 견적일자
        /// </summary>
		public string ESTIMATEDATE { get; set; }

        /// <summary>
        /// 계약여부
        /// </summary>
        public string ISCONTRACT { get; set; }

        /// <summary>
        /// 계약일
        /// </summary>
		public string CONTRACTDATE { get; set; }

        /// <summary>
        /// 계약시작일
        /// </summary>
        public string CONTRACTSTARTDATE { get; set; }

        /// <summary>
        /// 계약종료일
        /// </summary>
        public string CONTRACTENDDATE { get; set; }

        /// <summary>
        /// 계약기간
        /// </summary>
        public string ContractPeriod { get; set; }
      

       
    }
}
