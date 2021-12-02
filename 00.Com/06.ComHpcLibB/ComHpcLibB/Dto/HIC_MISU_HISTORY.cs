namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_MISU_HISTORY : BaseDto
    {

        /// <summary>
        /// 작업일자 및 시각
        /// </summary>
        public DateTime? JOBDATE { get; set; }

        /// <summary>
        /// 작업자 사번
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// 작업구분(1.신규등록 2.변경전 3.변경후 4.삭제)
        /// </summary>
        public string JOBGBN { get; set; }

        /// <summary>
        /// 미수번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 미수변동일자
        /// </summary>
        public DateTime? BDATE { get; set; }

        /// <summary>
        /// 회사코드
        /// </summary>
        public long LTDCODE { get; set; }

        /// <summary>
        /// 계정코드
        /// </summary>
        public string GEACODE { get; set; }

        /// <summary>
        /// 금액
        /// </summary>
        public long SLIPAMT { get; set; }

        /// <summary>
        /// 참고사항
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 등록일자 및 시각
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 작업자 사번
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDCODE2 { get; set; }


        public HIC_MISU_HISTORY() { }
    }
}
