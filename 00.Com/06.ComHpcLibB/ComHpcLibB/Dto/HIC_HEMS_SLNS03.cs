namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// HEMS 전송 과거직력
    /// </summary>
    /// 
    public class HIC_HEMS_SLNS03 : BaseDto
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
        /// 최초검진일
        /// </summary>
        public string FRST_SLNS_DT { get; set; }

        /// <summary>
        /// 주민번호
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// 과거직력순번
        /// </summary>
        public long WORK_PD_SEQ { get; set; }

        /// <summary>
        /// 공정코드
        /// </summary>
        public string PROCS_CD { get; set; }

        /// <summary>
        /// 노출기간 FROM
        /// </summary>
        public string WORK_PD_FROM { get; set; }

        /// <summary>
        /// 노출기간 TO
        /// </summary>
        public string WORK_PD_TO { get; set; }

        /// <summary>
        /// 근무년수
        /// </summary>
        public string WORK_PD_YEARS { get; set; }

        /// <summary>
        /// 공정명
        /// </summary>
        public string PROCS_NM { get; set; }

        /// <summary>
        /// HEMS 번호
        /// </summary>
        public long HEMSNO { get; set; }


        /// <summary>
        /// HEMS 전송 과거직력
        /// </summary>
        public HIC_HEMS_SLNS03()
        {
        }
    }
}
