namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_SANGDAM_WAIT : BaseDto
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
		public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// 입력시각
        /// </summary>
        public string ENT { get; set; }

        /// <summary>
        /// 호출시각
        /// </summary>
        public DateTime? CALLTIME { get; set; }

        /// <summary>
        /// 호출시각
        /// </summary>
        public string CALL { get; set; }

        /// <summary>
        /// 대기순번
        /// </summary>
        public long WAITNO { get; set; } 

        /// <summary>
        /// 안내표시구분
        /// </summary>
		public string DISPLAY { get; set; } 

        /// <summary>
        /// 검진번호
        /// </summary>
		public long PANO { get; set; } 

        /// <summary>
        /// 다음 검사실(컴마로 구분됨)
        /// </summary>
		public string NEXTROOM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long CNT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 일반검진 상담 대기순번
        /// </summary>
        public HIC_SANGDAM_WAIT()
        {
        }
    }
}
