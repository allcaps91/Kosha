namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// HEMS 전송 진찰
    /// </summary>
    /// 
    public class HIC_HEMS_SLNS11 : BaseDto
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
        /// 진찰코드(1:신경계, 2:이비인후과, 3:안과, 4:피부과, 5.진동장애, 6:기타)
        /// </summary>
        public string MDCXM_CD { get; set; }

        /// <summary>
        /// 진찰내용
        /// </summary>
        public string MDCXM_CN { get; set; }

        /// <summary>
        /// HEMS 번호
        /// </summary>
        public long HEMSNO { get; set; }

        /// <summary>
        /// HEMS 전송 진찰
        /// </summary>
        public HIC_HEMS_SLNS11()
        {
        }
    }
}
