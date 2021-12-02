namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_RESV_SET : BaseDto
    {
        
        /// <summary>
        /// 수검일자
        /// </summary>
		public string SDATE { get; set; } 

        /// <summary>
        /// 검사코드(Hea_Code 13번 참조)
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 검사명
        /// </summary>
		public string EXAMNAME { get; set; } 

        /// <summary>
        /// 최종등록/변경사번
        /// </summary>
		public long ENTSABUN { get; set; }

        /// <summary>
        /// 작업자명
        /// </summary>
        public string JOBNAME { get; set; }

        /// <summary>
        /// 최종등록/변경시각
        /// </summary>
        public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 오전 가능 인원
        /// </summary>
		public long AMINWON { get; set; } 

        /// <summary>
        /// 오후 가능 인원
        /// </summary>
		public long PMINWON { get; set; } 

        /// <summary>
        /// 오전 가예약 인원
        /// </summary>
		public long GAINWONAM { get; set; } 

        /// <summary>
        /// 오후 가예약 인원
        /// </summary>
		public long GAINWONPM { get; set; } 

        /// <summary>
        /// 1.예약  2.가예약
        /// </summary>
		public string GBRESV { get; set; } 

        /// <summary>
        /// 요일
        /// </summary>
		public string YOIL { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }


        ///HEA_RESERVED 항목 추가 => HEA_RESERVED 테이블 없음
        /// <summary>
        /// YDATE
        /// </summary>
        //public string YDATE { get; set; }

        /// <summary>
        /// 종합검진 검사예약 가능인원 세팅 Table
        /// </summary>
        public HEA_RESV_SET()
        {
        }
    }
}
