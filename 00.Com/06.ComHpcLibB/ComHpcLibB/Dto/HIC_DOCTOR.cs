namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_DOCTOR : BaseDto
    {
        
        /// <summary>
        /// 의사사번
        /// </summary>
		public long SABUN { get; set; } 

        /// <summary>
        /// 의사성명
        /// </summary>
		public string DRNAME { get; set; } 

        /// <summary>
        /// 입사일자
        /// </summary>
		public DateTime? IPSADAY { get; set; } 

        /// <summary>
        /// 퇴사일자
        /// </summary>
		public DateTime? REDAY { get; set; } 

        /// <summary>
        /// 면허번호
        /// </summary>
		public string LICENCE { get; set; } 

        /// <summary>
        /// 등록일자
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 등록/변경자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 스케쥴 표시 과장
        /// </summary>
		public string SCHE { get; set; } 

        /// <summary>
        /// 상담실 방 번호
        /// </summary>
		public string ROOM { get; set; } 

        /// <summary>
        /// 판정유무
        /// </summary>
		public string PAN { get; set; } 

        /// <summary>
        /// 원내의사코드
        /// </summary>
		public string DRCODE { get; set; } 

        /// <summary>
        /// 구강검진
        /// </summary>
		public string GBDENT { get; set; } 

        /// <summary>
        /// 종검판정
        /// </summary>
		public string GBHEA { get; set; } 

        public long DRBUNHO { get; set; }


        /// <summary>
        /// 일반검진 판정의사 코드
        /// </summary>
        public HIC_DOCTOR()
        {
        }
    }
}
