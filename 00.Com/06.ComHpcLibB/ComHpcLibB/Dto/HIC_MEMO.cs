namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_MEMO : BaseDto
    {
        /// <summary>
        /// 검진종류
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 종검 접수번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 건진 등록번호
        /// </summary>
        public long PANO { get; set; } 

        /// <summary>
        /// 메모내역
        /// </summary>
		public string MEMO { get; set; } 

        /// <summary>
        /// 삭제일자
        /// </summary>
		public DateTime? DELDATE { get; set; } 

        /// <summary>
        /// 등록일시
        /// </summary>
		public string ENTTIME { get; set; } 

        /// <summary>
        /// 등록자 사번
        /// </summary>
		public long JOBSABUN { get; set; }

        /// <summary>
        /// 등록자 성명
        /// </summary>
		public string JOBNAME { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
		public string PTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBDEL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 1: 일반검진, 2: 종합검진, 3: 전체 구분
        /// </summary>
        public string JOBGBN { get; set; }
        public string GUBUN1 { get; set; }

        /// <summary>
        /// 일반건진 환자별 메모내역
        /// </summary>
        public HIC_MEMO()
        {
        }
    }
}
