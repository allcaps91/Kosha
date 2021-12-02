namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_BOGENLTD : BaseDto
    {



        /// <summary>
        /// 회사코드
        /// </summary>
        public long LTDCODE { get; set; }

        /// <summary>
        /// 최종계약일자
        /// </summary>
        public DateTime? GEDATE { get; set; }

        /// <summary>
        /// 계약인원
        /// </summary>
        public long INWON { get; set; }

        /// <summary>
        /// 계약단가
        /// </summary>
        public double PRICE { get; set; }

        /// <summary>
        /// 계약금액
        /// </summary>
        public long AMT { get; set; }

        /// <summary>
        /// 종전 인원
        /// </summary>
        public long OLDINWON { get; set; }

        /// <summary>
        /// 종전 단가
        /// </summary>
        public long OLDPRICE { get; set; }

        /// <summary>
        /// 종전 금액
        /// </summary>
        public long OLDAMT { get; set; }

        /// <summary>
        /// 계약취소일자
        /// </summary>
        public DateTime? DELDATE { get; set; }

        /// <summary>
        /// 최종 작업자 사번
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// 최종 변경 일시
        /// </summary>
        public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// 사용여부
        /// </summary>
        public string USE { get; set; }

        /// <summary>
        /// 단가금액(인원*단가)
        /// </summary>
        public long PRAMT { get; set; }

        /// <summary>
        /// 종전 단가금액(인원*단가)
        /// </summary>
        public long OLDPRAMT { get; set; }

        /// <summary>
        /// 정액계약여부
        /// </summary>
        public string FIX { get; set; }

        /// <summary>
        /// 출력시 계산서 인원&단가 표시여부
        /// </summary>
        public string BILL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDCODE2 { get; set; }

        public HIC_BOGENLTD()
        {
        }
    }
}
