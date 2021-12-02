namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HIC_TCODE : BaseDto
    {

        /// <summary>
        /// 구분
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// 코드
        /// </summary>
        public string CODE { get; set; }

        /// <summary>
        /// 내역
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 구분
        /// </summary>
        public string JIK { get; set; }

        /// <summary>
        /// 인쇄순서
        /// </summary>
        public long PRINTRANKING { get; set; }

        /// <summary>
        /// 점수
        /// </summary>
        public long JUMSU { get; set; }


        public string UserId { get; set; }
    }
}
