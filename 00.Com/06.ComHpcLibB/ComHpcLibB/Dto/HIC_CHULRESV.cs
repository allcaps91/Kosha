namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_CHULRESV : BaseDto
    {
        
        /// <summary>
        /// 예약일자
        /// </summary>
		public DateTime? RDATE { get; set; } 

        /// <summary>
        /// 회사코드
        /// </summary>
		public string LTDCODE { get; set; } 

        /// <summary>
        /// 회사명칭 또는 출장장소. 기존 컬럼 LTDNAME을 PLACE 로 변환하여 사용
        /// </summary>
		public string PLACE { get; set; }

        /// <summary>
        /// 회사명칭 
        /// </summary>
        public string LTDNAME { get; set; }

        /// <summary>
        /// 검진시간(ex:10:00-12:00)
        /// </summary>
        public string RTIME { get; set; } 

        /// <summary>
        /// 인원
        /// </summary>
		public long INWON { get; set; } 

        /// <summary>
        /// 병원출발시간(ex:10:30)
        /// </summary>
		public string STARTTIME { get; set; } 

        /// <summary>
        /// 검진종류 (예..공무원 (군인))
        /// </summary>
		public string SPECIAL { get; set; } 

        /// <summary>
        /// 기타 참고사항
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 최종 등록자 사번
        /// </summary>
		public long JOBSABUN { get; set; } 

        /// <summary>
        /// 최종 등록/변경 시간
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 수정했을때(Y) , NULL
        /// </summary>
		public string GBCHANGE { get; set; } 

        /// <summary>
        /// 1.출장 2.내원 및 학생
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 삭제일자
        /// </summary>
		public DateTime? DELDATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 출장검진 예약 내역
        /// </summary>
        public HIC_CHULRESV()
        {
        }
    }
}
