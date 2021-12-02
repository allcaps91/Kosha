namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_RES_ETC : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 판정일자
        /// </summary>
		public string PANJENGDATE { get; set; } 

        /// <summary>
        /// 판정의사번호
        /// </summary>
		public long PANJENGDRNO { get; set; } 

        /// <summary>
        /// 검진일자
        /// </summary>
		public DateTime? GUNDATE { get; set; } 

        /// <summary>
        /// 판정구분(Y/N)
        /// </summary>
		public string GBPANJENG { get; set; } 

        /// <summary>
        /// 인쇄구분(Y/N)
        /// </summary>
		public string GBPRINT { get; set; } 

        /// <summary>
        /// 판정소견내역
        /// </summary>
		public string SOGEN { get; set; } 

        /// <summary>
        /// 통보일자
        /// </summary>
		public DateTime? TONGBODATE { get; set; } 

        /// <summary>
        /// 구분(1:혈액종합,2:69종판정,3:뇌심혈관계판정)
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 판정
        /// </summary>
		public string PAN { get; set; } 

        /// <summary>
        /// 조치
        /// </summary>
		public string JOCHI { get; set; } 

        /// <summary>
        /// 소견내역
        /// </summary>
		public string SOGENREMARK { get; set; } 

        /// <summary>
        /// 업무적합성
        /// </summary>
		public string WORKYN { get; set; } 

        /// <summary>
        /// 사후관리
        /// </summary>
		public string SAHUCODE { get; set; } 

        /// <summary>
        /// 프린트 작업사번
        /// </summary>
		public long PRTSABUN { get; set; } 

        
        /// <summary>
        /// 혈액종합검사
        /// </summary>
        public HIC_RES_ETC()
        {
        }
    }
}
