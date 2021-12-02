namespace HC_Measurement.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HIC_CHK_UCODE : BaseDto
    {

        /// <summary>
        /// 유해요인 코드
        /// </summary>
        public string CODE { get; set; }

        /// <summary>
        /// 유해요인명
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 상세설명
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 순번
        /// </summary>
        public long SORT { get; set; }

        /// <summary>
        /// 사용유무
        /// </summary>
        public string GBUSE { get; set; }

        /// <summary>
        /// 입력사번
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 일력일자
        /// </summary>
        public DateTime? ENTDATE { get; set; }


        /// <summary>
        /// Rowid
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 건강검진 자료사전 Table
        /// </summary>
        public HIC_CHK_UCODE()
        {
        }
    }
}
