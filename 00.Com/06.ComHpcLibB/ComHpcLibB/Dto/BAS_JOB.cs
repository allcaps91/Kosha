namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class BAS_JOB : BaseDto
    {
        
        /// <summary>
        /// 작업일자 (회계일자)
        /// </summary>
		public DateTime? JOBDATE { get; set; } 

        /// <summary>
        /// 휴일여부,"*" 공휴일 OR 일요일
        /// </summary>
		public string HOLYDAY { get; set; } 

        /// <summary>
        /// 외래마감여부,"S" 마감시작 "*"마감완료
        /// </summary>
		public string OPDMAGAM { get; set; } 

        /// <summary>
        /// 입원마감여부,"S" 마감시작 "*"마감완료
        /// </summary>
		public string IPDMAGAM { get; set; } 

        /// <summary>
        /// ARC 작업여부,"S" 마감시작 "*"마감완료
        /// </summary>
		public string ARCJOB { get; set; } 

        /// <summary>
        /// 심평원자료 연동
        /// </summary>
		public string HIRAJOB { get; set; } 

        /// <summary>
        /// 임시공휴일 "*"
        /// </summary>
		public string TEMPHOLYDAY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ILJA { get; set; }

        /// <summary>
        /// 마감일자 관리
        /// </summary>
        public BAS_JOB()
        {
        }
    }
}
