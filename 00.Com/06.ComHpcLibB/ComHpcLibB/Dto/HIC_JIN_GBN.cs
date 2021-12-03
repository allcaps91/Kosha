namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JIN_GBN : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 구분(아래참조)
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 인쇄구분(Y,N)
        /// </summary>
		public string GBPRINT { get; set; } 

        /// <summary>
        /// 최종작업시간
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 최종작업자
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 진단판정의사
        /// </summary>
		public long PANJENGDRNO { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string ROWID { get; set; }

        /// <summary>
        /// 일반건진 진단서 구분
        /// </summary>
        public HIC_JIN_GBN()
        {
        }
    }
}
