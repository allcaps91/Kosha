namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// HEMS 전송 과거 병력
    /// </summary>
    /// 
    public class HIC_HEMS_SLNS04 : BaseDto
    {
        /// <summary>
        /// 특검사업년도
        /// </summary>
        public string SLNS_YEAR { get; set; }

        /// <summary>
        /// 검진기관코드
        /// </summary>
        public string HOS_CODE { get; set; }

        /// <summary>
        /// 산재번호
        /// </summary>
        public string INDDIS_NO { get; set; }

        /// <summary>
        /// 개시번호
        /// </summary>
        public string INDOPEN_NO { get; set; }

        /// <summary>
        /// 순번
        /// </summary>
        public long INDDIS_NO_SEQ { get; set; }

        /// <summary>
        /// 최초_검진일
        /// </summary>
        public string FRST_SLNS_DT { get; set; }

        /// <summary>
        /// 인적키(주민번호)
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// 과거병력순번
        /// </summary>
        public long HSTCS_SEQ { get; set; }

        /// <summary>
        /// 과거병력 코드
        /// </summary>
        public string HSTCS_CD { get; set; }

        /// <summary>
        /// 과거병력 비고
        /// </summary>
        public string HSTCS_REMARK { get; set; }

        /// <summary>
        /// 과거병력 명
        /// </summary>
        public string HSTCS_NM { get; set; }

        /// <summary>
        /// HEMS 번호
        /// </summary>
        public long HEMSNO { get; set; }

        /// <summary>
        /// HEMS 전송 과거 병력
        /// </summary>
        public HIC_HEMS_SLNS04()
        {
        }
    }
}
