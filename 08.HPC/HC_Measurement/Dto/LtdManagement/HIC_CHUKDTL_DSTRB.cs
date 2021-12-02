namespace HC_Measurement.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HIC_CHUKDTL_DSTRB : BaseDto
    {
        /// <summary>
        /// 일력번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 유해요인 분포실태
        /// </summary>
        public byte[] REMARK { get; set; }

        /// <summary>
        /// 입력일자
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// Rowid
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string IsDelete { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string IsActive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HIC_CHUKDTL_DSTRB()
        {
        }
    }
}
