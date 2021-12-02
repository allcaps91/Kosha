namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_BORESV : BaseDto
    {
        /// <summary>
        /// 방문일자
        /// </summary>
        public string RDATE { get; set; }

        /// <summary>
        /// 회사코드
        /// </summary>
        public long LTDCODE { get; set; }

        /// <summary>
        /// 방문시간(12:00~13:00)
        /// </summary>
        public string RTIME { get; set; }

        /// <summary>
        /// 인원
        /// </summary>
        public long INWON { get; set; }

        /// <summary>
        /// 출발시간
        /// </summary>
        public string STIME { get; set; }

        /// <summary>
        /// 도착시간
        /// </summary>
        public string STIME1 { get; set; }

        /// <summary>
        /// 방문당담자
        /// </summary>
        public string MAN { get; set; }

        /// <summary>
        /// 동행방문자
        /// </summary>
        public string MAN1 { get; set; }

        /// <summary>
        /// 참고사항, 비고
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 최종 작업자사번
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// 최종 작업시간
        /// </summary>
        public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// 변경확인
        /// </summary>   
        public string GBCHANGE { get; set; }

        /// <summary>
        /// 방문담당자 사번 2011-11-10
        /// </summary>
        public string MAN_SABUN { get; set; }

        /// <summary>
        /// 담당자구분(1.의사 2.간호사 3.산업위생)
        /// </summary>
        public string MAN_GBN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDCODE2 { get; set; }


        public HIC_BORESV()
                    {
                    }
    }
}
