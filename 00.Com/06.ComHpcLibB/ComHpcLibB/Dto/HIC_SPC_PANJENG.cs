namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_SPC_PANJENG : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 접수(검진)일자
        /// </summary>
        //public DateTime? JEPDATE { get; set; } 
        public string JEPDATE { get; set; }

        /// <summary>
        /// 회사코드
        /// </summary>
        public long LTDCODE { get; set; } 

        /// <summary>
        /// 판정일자
        /// </summary>
		public string PANJENGDATE { get; set; } 

        /// <summary>
        /// 판정의사 면허번호
        /// </summary>
		public long PANJENGDRNO { get; set; } 

        /// <summary>
        /// 취급물질코드(**)
        /// </summary>
		public string MCODE { get; set; } 

        /// <summary>
        /// 판정(아래참조)
        /// </summary>
		public string PANJENG { get; set; } 

        /// <summary>
        /// 소견코드(**)       공단코드:27
        /// </summary>
		public string SOGENCODE { get; set; } 

        /// <summary>
        /// 조치코드(**)       공단코드:14
        /// </summary>
		public string JOCHICODE { get; set; } 

        /// <summary>
        /// 업무적합성여부(**) 공단코드:13
        /// </summary>
		public string WORKYN { get; set; } 

        /// <summary>
        /// 사후관리코드(**)   공단코드:32
        /// </summary>
		public string SAHUCODE { get; set; } 

        /// <summary>
        /// 질병코드
        /// </summary>
		public string MSYMCODE { get; set; } 

        /// <summary>
        /// 질병분류코드
        /// </summary>
		public string MBUN { get; set; } 

        /// <summary>
        /// 유해인자(**)       공단코드:09
        /// </summary>
		public string UCODE { get; set; } 

        /// <summary>
        /// 소견 상세내역
        /// </summary>
		public string SOGENREMARK { get; set; } 

        /// <summary>
        /// 조치 상세내역
        /// </summary>
		public string JOCHIREMARK { get; set; } 

        /// <summary>
        /// 추가검사(R.판정자)
        /// </summary>
		public string REEXAM { get; set; } 

        /// <summary>
        /// 최종 변경/등록자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 최종 변경 일자 및 시각
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 삭제일자
        /// </summary>
		public string DELDATE { get; set; } 

        /// <summary>
        /// 표적장기선정코드
        /// </summary>
		public string ORCODE { get; set; } 

        /// <summary>
        /// 표적장기선정사유코드
        /// </summary>
		public string ORSAYUCODE { get; set; } 

        /// <summary>
        /// 사후관리 내역
        /// </summary>
		public string SAHUREMARK { get; set; } 

        /// <summary>
        /// 판정차수(1=1차,2=2차)
        /// </summary>
		public string GJCHASU { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE2 { get; set; } 

        /// <summary>
        /// 야간작업 표적장기 구분
        /// </summary>
		public string PYOJANGGI { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// count
        /// </summary>
		public long CNT { get; set; }

        /// <summary>
        /// count
        /// </summary>
		public string PYOJANGGi { get; set; }

        public long D1 { get; set; }

        public long D2 { get; set; }


        /// <summary>
        /// 특수검진 취급물질별 판정내역
        /// </summary>
        public HIC_SPC_PANJENG()
        {
        }
    }
}
