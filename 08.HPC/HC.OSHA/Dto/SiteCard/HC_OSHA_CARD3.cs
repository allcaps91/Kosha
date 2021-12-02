namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 사업장 관리카드 3. 관리책임자 및 선임현황
    /// </summary>
    public class HC_OSHA_CARD3 : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SITE_ID { get; set; }
        public long ESTIMATE_ID { get; set; }

        /// <summary>
        /// 책임자구분
        /// </summary>
        public string TASKTYPE { get; set; }

        public string YEAR { get; set; }

        /// <summary>
        /// 성명 또는 대행기관명
        /// </summary>
        public string NAME { get; set; } 

        /// <summary>
        /// 선임일자
        /// </summary>
		public string DELACEDATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CERT { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string EDUCATION { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CARRER { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string EDUATIONNAME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string EDUCATIONSTARTDATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string EDUCATIONENDDATE { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD3()
        {
        }
    }
}
