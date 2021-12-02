namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 사업장카드 응급의료체계
    /// </summary>
    public class HC_OSHA_CARD21 : BaseDto
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

        public string YEAR { get; set; }

        /// <summary>
        /// 지정의료기관유무
        /// </summary>
        public string ISHOSPITAL { get; set; } 

        /// <summary>
        /// 지정의료기관
        /// </summary>
		public string HOSPITALNAME { get; set; } 

        /// <summary>
        /// 소재지
        /// </summary>
		public string ADDRESS { get; set; } 

        /// <summary>
        /// 연락처
        /// </summary>
		public string TEL { get; set; } 

        /// <summary>
        /// 응급처치담당자지정 유무
        /// </summary>
		public string ISMANAGER { get; set; } 

        /// <summary>
        /// 응급처치담당자
        /// </summary>
		public string MANAGERNAME { get; set; } 

        /// <summary>
        /// 구급함 유무
        /// </summary>
		public string ISAIDKIT { get; set; } 

        /// <summary>
        /// 구급함  형태
        /// </summary>
		public string AIDKITTYPE { get; set; } 

        /// <summary>
        /// 비고
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 위생기구 체크박스
        /// </summary>
		public string CLEAN1CHECKBOX { get; set; } 

        /// <summary>
        /// 위생기구 기타
        /// </summary>
		public string CLEAN1ETC { get; set; } 

        /// <summary>
        /// 위생물품체크박스
        /// </summary>
		public string CLEAN2CHECKBOX { get; set; } 

        /// <summary>
        /// 위생물품 기타
        /// </summary>
		public string CLEAN2ETC { get; set; } 

        /// <summary>
        /// 외용약 체크박스
        /// </summary>
		public string CLEAN3CHECKBOX { get; set; } 

        /// <summary>
        /// 외용약 기타
        /// </summary>
		public string CLEAN3ETC { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD21()
        {
        }
    }
}
