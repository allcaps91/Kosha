namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_RES_SAHUSANGDAM : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 접수일자
        /// </summary>
		public DateTime? JEPDATE { get; set; } 

        /// <summary>
        /// 상담일자
        /// </summary>
		public DateTime? SDATE { get; set; } 

        /// <summary>
        /// 상담자 사번
        /// </summary>
		public long SABUN { get; set; } 

        /// <summary>
        /// 소견
        /// </summary>
		public string SOGEN { get; set; } 

        /// <summary>
        /// 판정구분(D1,D2)
        /// </summary>
		public string PANJENGGBN { get; set; } 

        /// <summary>
        /// 상담구분(1.내원 2.전화)
        /// </summary>
		public string GBN { get; set; } 

        /// <summary>
        /// 상담내역
        /// </summary>
		public string REMARK { get; set; }

        /// <summary>
        /// 상담내역
        /// </summary>
        public string ROWID { get; set; }




        /// <summary>
        /// 유소견자 (D1,D2) 사후관리 상담대장
        /// </summary>
        public HIC_RES_SAHUSANGDAM()
        {
        }
    }
}
