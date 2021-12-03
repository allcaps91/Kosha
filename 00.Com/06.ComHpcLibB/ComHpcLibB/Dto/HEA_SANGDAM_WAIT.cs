namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class HEA_SANGDAM_WAIT : BaseDto
    {   
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 수검자명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 성별
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 나이
        /// </summary>
		public long AGE { get; set; } 

        /// <summary>
        /// 검진종류
        /// </summary>
		public string GJJONG { get; set; } 

        /// <summary>
        /// 호출여부
        /// </summary>
		public string GBCALL { get; set; } 

        /// <summary>
        /// 상담실방번호
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 입력시각
        /// </summary>
		public string ENTTIME { get; set; } 

        /// <summary>
        /// 호출시각
        /// </summary>
		public string CALLTIME { get; set; } 

        /// <summary>
        /// 대기순번
        /// </summary>
		public long WAITNO { get; set; } 

        /// <summary>
        /// 구강판정에서 전달시 Y
        /// </summary>
		public string GBDENT { get; set; } 

        /// <summary>
        /// Y=수면내시경
        /// </summary>
		public string GBENDO { get; set; }

        /// <summary>
        /// count
        /// </summary>
        public long CNT { get; set; }

        /// <summary>
        /// count
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// ENTTIME
        /// </summary>
        public string ENT { get; set; }

        /// <summary>
        /// CALLTIME
        /// </summary>
        public string CALL { get; set; }

        public string PTNO { get; set; }
        

        /// <summary>
        /// 종합검진 상담대기 순번관리
        /// </summary>
        public HEA_SANGDAM_WAIT()
        {
        }
    }
}
