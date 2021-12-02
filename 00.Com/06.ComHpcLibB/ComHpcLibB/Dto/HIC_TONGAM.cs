namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_TONGAM : BaseDto
    {
        /// <summary>
        /// 통계일자
        /// </summary>
        public string TDATE { get; set; }

        /// <summary>
        /// 검진종류
        /// </summary>
        public string GJJONG { get; set; }

        /// <summary>
        /// 인원종류
        /// </summary>
        public string GBINWON { get; set; }

        /// <summary>
        /// 검진차수
        /// </summary>
        public string CHASU { get; set; }

        /// <summary>
        /// 출장구분
        /// </summary>
        public string GBCHUL { get; set; }

        /// <summary>
        /// 접수건수
        /// </summary>
        public long JEPCNT { get; set; }

        /// <summary>
        /// 위(UGI)
        /// </summary>
        public long CNT1 { get; set; }

        /// <summary>
        /// 위(GFS)
        /// </summary>
        public long CNT2 { get; set; }

        /// <summary>
        /// 대장암
        /// </summary>
        public long CNT3 { get; set; }

        /// <summary>
        /// 간암
        /// </summary>
        public long CNT4 { get; set; }

        /// <summary>
        /// 유방암
        /// </summary>
        public long CNT5 { get; set; }

        /// <summary>
        /// 자궁경부암
        /// </summary>
        public long CNT6 { get; set; }

        /// <summary>
        /// 구분
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// 최종작업일자
        /// </summary>
        public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// 최종작업자
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long CNT7 { get; set; }

        public long SUBTOT { get; set; }

        public string YYMM { get; set; }

        public HIC_TONGAM()
        {
        }
    }
}
