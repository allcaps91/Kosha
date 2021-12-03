namespace HC_Measurement.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HIC_CHUKDTL_LOCATION : BaseDto
    {
        /// <summary>
        /// 일력번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 순번
        /// </summary>
        public long SEQNO { get; set; }

        /// <summary>
        /// 파일명
        /// </summary>
        public string FILENAME { get; set; }

        /// <summary>
        /// 파일설명
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 이미지 Data
        /// </summary>
        public byte[] IMAGEDATA { get; set; }

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
        public HIC_CHUKDTL_LOCATION()
        {
        }
    }
}
