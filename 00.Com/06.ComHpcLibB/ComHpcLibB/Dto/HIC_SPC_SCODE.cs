namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_SPC_SCODE : BaseDto
    {
        
        /// <summary>
        /// 코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 판정코드(1,2,..)
        /// </summary>
		public string PANJENG { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string PANJENG2 { get; set; }

        /// <summary>
        /// 질병분류(표준코드:질병분류 참조)
        /// </summary>
        public string DBUN { get; set; }

        /// <summary>
        /// 질병분류코드
        /// </summary>
        public string DBUN_NM { get; set; }

        /// <summary>
        /// 소견명칭
        /// </summary>
        public string NAME { get; set; } 

        /// <summary>
        /// 취급물질코드
        /// </summary>
		public string MCODE { get; set; }

        /// <summary>
        /// 취급물질코드
        /// </summary>
        public string MCODE_NM { get; set; }

        /// <summary>
        /// 삭제일자
        /// </summary>
        public DateTime? DELDATE { get; set; } 

        /// <summary>
        /// 최종 변경/등록 일자
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 최종 변경/등록 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 공단코드
        /// </summary>
		public string GCODE { get; set; } 

        /// <summary>
        /// 소견 상세내역
        /// </summary>
		public string SOGENREMARK { get; set; } 

        /// <summary>
        /// 조치 상세내역
        /// </summary>
		public string JOCHIREMARK { get; set; } 

        /// <summary>
        /// 업무적합성여부(**) 공단코드:13
        /// </summary>
		public string WORKYN { get; set; } 

        /// <summary>
        /// 사후관리코드(**)   공단코드:32
        /// </summary>
		public string SAHUCODE { get; set; } 

        /// <summary>
        /// 추가검사(R.판정자)
        /// </summary>
		public string REEXAM { get; set; } 

        /// <summary>
        /// 자동판정 여부(Y.자동판정)
        /// </summary>
		public string GBAUTOPAN { get; set; } 

        /// <summary>
        /// 우선순위
        /// </summary>
		public long SORT { get; set; }

        /// <summary>
        /// 우선순위
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 자동판정 적용기준
        /// </summary>
        public string AUTOPANGBN { get; set; }

        /// <summary>
        /// 특수검진 소견코드
        /// </summary>
        public HIC_SPC_SCODE()
        {
        }
    }
}
