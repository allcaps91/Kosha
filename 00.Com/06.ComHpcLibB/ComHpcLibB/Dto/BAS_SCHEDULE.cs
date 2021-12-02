namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class BAS_SCHEDULE : BaseDto
    {
        
        /// <summary>
        /// 의사코드
        /// </summary>
		public string DRCODE { get; set; } 

        /// <summary>
        /// 진료일자
        /// </summary>
		public DateTime? SCHDATE { get; set; } 

        /// <summary>
        /// 일자구분(1.종일근무 2.오전근무 3.휴일)** 사용않함
        /// </summary>
		public string GBDAY { get; set; } 

        /// <summary>
        /// 오전:진료형태(아래참조):2006/07/21변경
        /// </summary>
		public string GBJIN { get; set; } 

        /// <summary>
        /// 접수마감(1.오전 2.오후 기타:접수가능)
        /// </summary>
		public string GBJINEND { get; set; } 

        /// <summary>
        /// 오후:진료형태(아래참조):2006/07/21추가
        /// </summary>
		public string GBJIN2 { get; set; } 

        /// <summary>
        /// 야간:진료형태(아래참조):2009/12/15 추가
        /// </summary>
		public string GBJIN3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ILJA { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CNT { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string ROWID { get; set; }

        /// <summary>
        /// 의사별 진료 스케쥴
        /// </summary>
        public BAS_SCHEDULE()
        {
        }
    }
}
