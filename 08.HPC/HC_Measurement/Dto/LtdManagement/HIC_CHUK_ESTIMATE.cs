namespace HC_Measurement.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// HIC_CHUK_ESTIMATE, HIC_CHUK_ESTIMATE_DTL 통합
    /// </summary>
    public class HIC_CHUK_ESTIMATE : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long SEQNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MCODE_NM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SUCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SUNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long QTY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBHALIN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long HALINAMT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long AMT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long PER { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long HALINAMT1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long HALINAMT2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? DELDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long TOTAMT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long BASEAMT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long CHARGEAMT { get; set; }

        /// <summary>
        /// 측정방법 코드
        /// </summary>
        public string CHKWAY_CD { get; set; }

        /// <summary>
        /// 측정방법명
        /// </summary>
        public string CHKWAY_NM { get; set; }

        /// <summary>
        /// 분석방법명
        /// </summary>
        public string ANALWAY_NM { get; set; }

        /// <summary>
        /// 견적서 이메일 전송일자
        /// </summary>
        public string SENDTIME { get; set; }

        /// <summary>
        /// 견적서 출력일자
        /// </summary>
        public string PRINTDATE { get; set; }

        /// <summary>
        /// 이메일 전송 보건담당자(사업장)
        /// </summary>
        public string LTD_MANAGER { get; set; }

        /// <summary>
        /// 이메일 전송 주소
        /// </summary>
        public string LTD_EMAILA_DDRESS { get; set; }
        
        /// <summary>
        /// Rowid
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string IsDelete { get; set; }

        /// <summary>
        /// 건강검진 자료사전 Table
        /// </summary>
        public HIC_CHUK_ESTIMATE()
        {
        }
    }
}
