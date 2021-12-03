namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 건강운동지도자 양성교육
    /// </summary>
    public class HC_OSHA_CARD11_2 : BaseDto
    {
        
     
		public long ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SITE_ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long ESTIMATE_ID { get; set; }

        public string YEAR { get; set; }

        /// <summary>
        /// 이수자명
        /// </summary>
        public string NAME { get; set; } 

        /// <summary>
        /// 교육시작
        /// </summary>
		public string STARTDATE { get; set; } 

        /// <summary>
        /// 교육 종료
        /// </summary>
		public string ENDDATE { get; set; } 

        /// <summary>
        /// 교육과정 및 내용
        /// </summary>
		public string CONTENT { get; set; }

        public string PERIOD { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD11_2()
        {
        }
    }
}
