namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_WAIT : BaseDto
    {
        
        /// <summary>
        /// 작업일자(YYYY-MM-DD)
        /// </summary>
		public string JOBDATE { get; set; } 

        /// <summary>
        /// 접수번호
        /// </summary>
		public long SEQNO { get; set; } 

        /// <summary>
        /// 주민등록번호(13자리)
        /// </summary>
		public string JUMIN { get; set; } 

        /// <summary>
        /// 성명(신규이면 공란)
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 작업구분(1.접수증인쇄 2.CALL 3.접수완료)
        /// </summary>
		public string GBJOB { get; set; } 

        /// <summary>
        /// 호출PC번호(NULL,1,2)
        /// </summary>
		public string PCNO { get; set; } 

        /// <summary>
        /// 접수증 인쇄시각(HH:MM)
        /// </summary>
		public string JEPTIME { get; set; } 

        /// <summary>
        /// 호출시각(HH:MM)
        /// </summary>
		public string CALLTIME { get; set; } 

        /// <summary>
        /// 접수시각(HH:MM)
        /// </summary>
		public string ENDTIME { get; set; } 

        /// <summary>
        /// 주민번호 암호화
        /// </summary>
		public string JUMIN2 { get; set; } 

        /// <summary>
        /// 예약구분(1.예약 2.2차재검 3.일반예약 ,NULL:일반수검자)
        /// </summary>
		public string GBYEYAK { get; set; } 

        /// <summary>
        /// 자동접수여부(Y.자동접수)
        /// </summary>
		public string GBAUTOJEP { get; set; } 

        /// <summary>
        /// 자동접수 검진종류
        /// </summary>
		public string GJJONG { get; set; } 

        /// <summary>
        /// 부서구분: 1.종합건진 2.일반건진
        /// </summary>
		public string GBBUSE { get; set; } 

        /// <summary>
        /// 2차재검자 접수증(바코드)인쇄 여부(Y.인쇄)
        /// </summary>
		public string SECONDPRINT { get; set; }

        /// <summary>
        /// 종합검진 총대기인원
        /// </summary>
        public long CNT { get; set; }

        public string RID { get; set; }

        //건진 순번대기표
        //인터넷 문진 작성 여부
        public string IEMUNYN { get; set; }

        public string GBDISPLAY1 { get; set; }

        public string GBDISPLAY2 { get; set; }

        public long HEACNT { get; set; }

        public long YEYAKCNT { get; set; }


        /// <summary>
        /// 건진 순번대기표
        /// </summary>
        public HIC_WAIT()
        {
        }
    }
}
