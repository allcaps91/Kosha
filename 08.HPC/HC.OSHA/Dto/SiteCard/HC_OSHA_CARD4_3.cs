namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;


    /// <summary>
    /// 사업장관리카드 4.업무개요(2)관리감독자
    /// </summary>
    public class HC_OSHA_CARD4_3 : BaseDto
    { /// <summary>
      /// 
      /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long ESTIMATE_ID { get; set; }

     
        public string NAME { get; set; }
        /// <summary>
        /// 선임일자
        /// </summary>
        public string DECLAREDATE { get; set; }
        public string GRADE { get; set; }
        public string TASKNAME { get; set; }
        public string EDUNAME { get; set; }
        public string EDUDATE { get; set; }
    }
}
