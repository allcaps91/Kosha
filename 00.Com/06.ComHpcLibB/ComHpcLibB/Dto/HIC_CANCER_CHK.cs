namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_CANCER_CHK : BaseDto
    {
        
        /// <summary>
        /// 등록번호
        /// </summary>
		public long PANO { get; set; } 

        /// <summary>
        /// 검진년도
        /// </summary>
		public string YEAR { get; set; } 

        /// <summary>
        /// 참고사항
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 작업자
        /// </summary>
		public long JOBSABUN { get; set; } 

        /// <summary>
        /// 작업시간
        /// </summary>
		public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// 작업시간
        /// </summary>
		public string ROWID { get; set; }       
        
        /// <summary>
        /// 암검진 미실시 조치사항
        /// </summary>
        public HIC_CANCER_CHK()
        {
        }
    }
}
