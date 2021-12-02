namespace HC_Tong.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_TONGDAILY : BaseDto
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
        /// 차수(1.1차 2.2차 3.기타)
        /// </summary>
        public string CHASU { get; set; }

        /// <summary>
        /// 출장검진(Y.출장 N.내원)
        /// </summary>
        public string GBCHUL { get; set; }

        /// <summary>
        /// 접수인원수
        /// </summary>
        public long JEPCNT { get; set; }

        /// <summary>
        /// 당일수납 총검진비
        /// </summary>
        public long TOTAMT { get; set; }

        /// <summary>
        /// 당일수납 조합부담액
        /// </summary>
        public long JOHAPAMT { get; set; }

        /// <summary>
        /// 당일수납 회사부담액
        /// </summary>
        public long LTDAMT { get; set; }

        /// <summary>
        /// 당일수납 본인부담액
        /// </summary>
        public long BONINAMT { get; set; }

        /// <summary>
        /// 당일수납 할인금액
        /// </summary>
        public long HALINAMT { get; set; }

        /// <summary>
        /// 당일수납 미수금액
        /// </summary>
        public long MISUAMT { get; set; }

        /// <summary>
        /// 당일수금 현금입금액
        /// </summary>
        public long SUNAPAMT { get; set; }

        /// <summary>
        /// 통계구분(1.검진종류별 2.인원통계종류별,3.종검현금예약선수,4.예약취소)
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// 당일 예약선수금 수납액
        /// </summary>
        public long YEYAKAMT { get; set; }

        /// <summary>
        /// 당일 예약선수금 대체액
        /// </summary>
        public long YDAECHE { get; set; }

        /// <summary>
        /// 최종 작업 시간
        /// </summary>
        public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// 최종 작업자 사번
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// 구분1(1: 독감)
        /// </summary>
        public string GUBUN1 { get; set; }

        /// <summary>
        /// 당일 카드금액
        /// </summary>
        public long SUNAPAMT2 { get; set; }

        /// <summary>
        /// 당일수납 보건소금액
        /// </summary>
        public long BOGENAMT { get; set; }



        public string GJONG { get; set; }
        public long SLIPAMT { get; set; }
        public string GEACODE { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public HIC_TONGDAILY()
        {
        }
    }
}
