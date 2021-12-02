namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_CRESV : BaseDto
    {

        /// <summary>
        /// 연번
        /// </summary>
        public string SEQNO { get; set; }

        /// <summary>
        /// 구분 ( 1: 일반 , 2: 5인미만 , 3 : 국고 )
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// 측정일자
        /// </summary>
        public string RDATE { get; set; }

        /// <summary>
        /// 회사코드
        /// </summary>
        public long LTDCODE { get; set; }

        /// <summary>
        /// 출발시간
        /// </summary>
        public string STIME { get; set; }

        /// <summary>
        /// 방문자
        /// </summary>
        public string MAN { get; set; }

        /// <summary>
        /// 동행자
        /// </summary>
        public string MAN1 { get; set; }

        /// <summary>
        /// 참고사항
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 작업자사번
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// 최종작업 시간
        /// </summary>
        public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDCODE2 { get; set; }


public HIC_CRESV()
        {
        }
    }
}
