namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// HEMS 전송 개인별 유해인자
    /// </summary>
    /// 
    public class HIC_HEMS_SLNS10 : BaseDto
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
        /// 취급 물질 순번
        /// </summary>
        public long HRMFLNS_FACTR_CDSEQ { get; set; }

        /// <summary>
        /// 유해물질
        /// </summary>
        public string HRMFLNS_FACTR_CD { get; set; }

        /// <summary>
        /// 건강관리 수첩 여부 (1.소지, 0.해당없음)
        /// </summary>
        public string HM_PKTBUK_YN { get; set; }

        /// <summary>
        /// 해당사업장 취급 여부
        /// </summary>
        public string BIZ_TRTMNT_YN { get; set; }

        /// <summary>
        /// HEMS 번호
        /// </summary>
        public long HEMSNO { get; set; }

        /// <summary>
        /// HEMS 전송 개인별 유해인자
        /// </summary>
        public HIC_HEMS_SLNS10()
        {
        }
    }   
}
