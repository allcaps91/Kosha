namespace HC_Measurement.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HIC_CHUK_WORKER : BaseDto
    {

        /// <summary>
        /// 일련번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 작업자명
        /// </summary>
        public string WORKER_NAME { get; set; }

        /// <summary>
        /// 작업사번
        /// </summary>
        public long WORKER_SABUN { get; set; }

        /// <summary>
        /// 역할
        /// </summary>
        public string ROLE { get; set; }

        /// <summary>
        /// 자격번호
        /// </summary>
        public string CERTNO { get; set; }

        /// <summary>
        /// 작성일자
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 작성자
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 비고
        /// </summary>
        public string BIGO { get; set; }

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
        public HIC_CHUK_WORKER()
        {
        }
    }
}
