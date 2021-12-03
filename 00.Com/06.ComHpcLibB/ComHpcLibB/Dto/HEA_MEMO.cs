namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_MEMO : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 메모내용
        /// </summary>
		public string MEMO { get; set; } 

        /// <summary>
        /// 입력시각
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 작업사번
        /// </summary>
		public long JOBSABUN { get; set; }

        /// <summary>
        /// 작업자명
        /// </summary>
        public string JOBNAME { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 종합검진 수검자 메모사항
        /// </summary>
        public HEA_MEMO()
        {
        }
    }
}
