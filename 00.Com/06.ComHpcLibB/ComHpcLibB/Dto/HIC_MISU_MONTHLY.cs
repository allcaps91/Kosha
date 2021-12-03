namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_MISU_MONTHLY : BaseDto
    {


        /// <summary>
        /// 년월
        /// </summary>
        public string YYMM { get; set; }

        /// <summary>
        /// 미수번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 이월금액
        /// </summary>
        public long IWOLAMT { get; set; }

        /// <summary>
        /// 당월발생액
        /// </summary>
        public long MISUAMT { get; set; }

        /// <summary>
        /// 당월입금액
        /// </summary>
        public long IPGUMAMT { get; set; }

        /// <summary>
        /// 수수료
        /// </summary>
        public long FEEAMT { get; set; }

        /// <summary>
        /// 당월할인액
        /// </summary>
        public long GAMAMT { get; set; }

        /// <summary>
        /// 당월삭감액
        /// </summary>
        public long SAKAMT { get; set; }

        /// <summary>
        /// 당월반송액
        /// </summary>
        public long BANAMT { get; set; }

        /// <summary>
        /// 월말잔액
        /// </summary>
        public long JANAMT { get; set; }

        /// <summary>
        /// 기타입금액
        /// </summary>
        public long ETCIPGUM { get; set; }

        /// <summary>
        /// 현금입금액
        /// </summary>
        public long CASHAMT { get; set; }

        /// <summary>
        /// 청구차액금액
        /// </summary>
        public long MIRCHAAMT { get; set; }

        /// <summary>
        /// 카드입금
        /// </summary>
        public long CARDAMT { get; set; }

        /// <summary>
        /// 통계형성시각
        /// </summary>
        public DateTime? ENTDATE { get; set; }

    public HIC_MISU_MONTHLY()
        {
        }
    }
}
