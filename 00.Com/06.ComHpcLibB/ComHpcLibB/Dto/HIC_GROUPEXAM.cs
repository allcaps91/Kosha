namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_GROUPEXAM : BaseDto
    {
        
        /// <summary>
        /// 금액코드
        /// </summary>
		public string GROUPCODE { get; set; } 

        /// <summary>
        /// 검사코드
        /// </summary>
		public string EXCODE { get; set; } 

        /// <summary>
        /// 수가구분(아래참조)
        /// </summary>
		public string SUGAGBN { get; set; } 

        /// <summary>
        /// 순위
        /// </summary>
		public long SEQNO { get; set; } 

        /// <summary>
        /// 최종작업사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 최종작업일자
        /// </summary>
		public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 묶음코드별 검사코드 설정
        /// </summary>
        public HIC_GROUPEXAM()
        {
        }
    }
}
