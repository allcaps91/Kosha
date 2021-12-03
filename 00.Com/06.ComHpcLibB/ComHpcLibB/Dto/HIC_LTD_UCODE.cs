namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_LTD_UCODE : BaseDto
    {
        
        /// <summary>
        /// 회사코드
        /// </summary>
		public long LTDCODE { get; set; } 

        /// <summary>
        /// 1.배치전 2.특수 3.일특
        /// </summary>
		public string JONG { get; set; }

        /// <summary>
        /// 검진종류 이름
        /// </summary>
        public string JONG_NM { get; set; }

        /// <summary>
        /// 담당업무명
        /// </summary>
        public string JOBNAME { get; set; } 

        /// <summary>
        /// 취급물질코드(컴마로 구분됨)
        /// </summary>
		public string UCODES { get; set; }

        /// <summary>
        /// 취급물질코드_VIEW
        /// </summary>
        public string VUCODE { get; set; }

        /// <summary>
        /// 직종코드(공단코드)
        /// </summary>
        public string JIKJONG { get; set; } 

        /// <summary>
        /// 공정코드(공단코드)
        /// </summary>
		public string GONGJENG { get; set; } 

        /// <summary>
        /// 최종 작업자 사번
        /// </summary>
		public long JOBSABUN { get; set; } 

        /// <summary>
        /// 최종 변경일시(YYYY-MM-DD HH24:MI)
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string SCODES { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long LTDCODE1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE2 { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }


        /// <summary>
        /// 회사의 부서별 취급물질코드 관리
        /// </summary>
        public HIC_LTD_UCODE()
        {
        }
    }
}
