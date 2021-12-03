namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class HIC_RESULT_HIS : BaseDto
    {   
        /// <summary>
        /// 변경일자 및 시각
        /// </summary>
		public string JOBTIME { get; set; } 

        /// <summary>
        /// 작업자사번
        /// </summary>
		public long JOBSABUN { get; set; } 

        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 검사코드
        /// </summary>
		public string EXCODE { get; set; } 

        /// <summary>
        /// 변경 전 결과
        /// </summary>
		public string RESULT_OLD { get; set; } 

        /// <summary>
        /// 변경 후 결과
        /// </summary>
		public string RESULT_NEW { get; set; } 

        
        /// <summary>
        /// 검사결과 변경 History
        /// </summary>
        public HIC_RESULT_HIS()
        {
        }
    }
}
