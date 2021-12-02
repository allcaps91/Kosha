namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// HEMS 전송 현재증상
    /// </summary>
    public class HIC_HEMS_SLNS02 : BaseDto
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
        /// 최초 검진일
        /// </summary>
        public string FRST_SLNS_DT { get; set; }

        /// <summary>
        /// 주민번호
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// 현재증상순번
        /// </summary>
        public long PAIN_SYMPTMS_SEQ { get; set; }

        /// <summary>
        /// 현재증상코드
        /// </summary>
        public string PAIN_SYMPTMS_CD { get; set; }

        /// <summary>
        /// 현재증상명
        /// </summary>
        public string PAIN_SYMPTMS_NM { get; set; }

        /// <summary>
        /// 현재증상비고
        /// </summary>
        public string PAIN_SYMPTMS_REMARK { get; set; }

        /// <summary>
        /// HEMS 번호
        /// </summary>
        public long HEMSNO { get; set; }

        /// <summary>
        /// HEMS 전송 현재증상
        /// </summary>
        public HIC_HEMS_SLNS02()
        {
        }
    }
}
