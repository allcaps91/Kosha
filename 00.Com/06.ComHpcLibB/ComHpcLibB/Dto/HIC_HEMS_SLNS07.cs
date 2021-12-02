namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// HEMS 전송 건강진단 현황
    /// </summary>
    /// 
    public class HIC_HEMS_SLNS07 : BaseDto
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
        /// 코드
        /// </summary>
        public long INDDIS_NO_SEQ { get; set; }

        /// <summary>
        /// 유해인자_구분 순번
        /// </summary>
        public long HRMFLNS_FACTR_SE_SEQ { get; set; }

        /// <summary>
        /// 유해인자_구분
        /// </summary>
        public string HRMFLNS_FACTR_SE { get; set; }

        /// <summary>
        /// 대상근로자(남)
        /// </summary>
        public long TRGTER_NMPR_MALE { get; set; }

        /// <summary>
        /// 대상근로자(여)
        /// </summary>
        public long TRGTER_NMPR_FEMALE { get; set; }

        /// <summary>
        /// 수진근로자(남)
        /// </summary>
        public long CLNIC_NMPR_MALE { get; set; }

        /// <summary>
        /// 수진근로자(여)
        /// </summary>
        public long CLNIC_NMPR_FEMALE { get; set; }

        /// <summary>
        /// 질병유소견자-일반질병(여)
        /// </summary>
        public long DISS_GNRL_NMPR_FEMALE { get; set; }

        /// <summary>
        /// 질병유소견자-일반질병(남)
        /// </summary>
        public long DISS_GNRL_NMPR_MALE { get; set; }

        /// <summary>
        /// 질병유소견자-직업병(남)
        /// </summary>
        public long DISS_OCCP_NMPR_MALE { get; set; }

        /// <summary>
        /// 질병유소견자-직업병(여)
        /// </summary>
        public long DISS_OCCP_NMPR_FEMALE { get; set; }

        /// <summary>
        /// 직업성요관찰자(남)
        /// </summary>
        public long OCCP_OBSRVR_NMPR_MALE { get; set; }

        /// <summary>
        /// 직업성요관찰자(여)
        /// </summary>
        public long OCCP_OBSRVR_NMPR_FEMALE { get; set; }

        /// <summary>
        /// HEMS 번호
        /// </summary>
        public long HEMSNO { get; set; }

        /// <summary>
        /// HEMS 전송 건강진단 현황
        /// </summary>
        public HIC_HEMS_SLNS07()
        {
        }
    }
}
