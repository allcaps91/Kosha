namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// HEMS 전송 사업장 마스터
    /// </summary>

    public class HIC_HEMS_SLNS01 : BaseDto
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
        /// 컴코드
        /// </summary>
        public string BIZ_HTH_NO { get; set; }

        /// <summary>
        /// 사업장명
        /// </summary>
        public string BIZ_NM { get; set; }

        /// <summary>
        /// 사업장_관리노동관서코드
        /// </summary>
        public string BIZ_MNG_LABOFFICE { get; set; }

        /// <summary>
        /// 사업장_관리지도원코드
        /// </summary>
        public string BIZ_MNG_AGENT { get; set; }

        /// <summary>
        /// 사업장_대표자명
        /// </summary>
        public string BIZ_RPRSNTV { get; set; }

        /// <summary>
        /// 사업장_우편번호
        /// </summary>
        public string BIZ_ZIP { get; set; }

        /// <summary>
        /// 사업장_주소1
        /// </summary>
        public string BIZ_ADRES_1 { get; set; }

        /// <summary>
        /// 사업장_주소2
        /// </summary>
        public string BIZ_ADRES_2 { get; set; }

        /// <summary>
        /// 사업장_전화번호
        /// </summary>
        public string BIZ_TELNO { get; set; }

        /// <summary>
        /// 사업장_FAX번호
        /// </summary>
        public string BIZ_FXNUM { get; set; }

        /// <summary>
        /// 사업장_업종코드
        /// </summary>
        public string BIZ_INDUTY { get; set; }

        /// <summary>
        /// 사업장_업태
        /// </summary>
        public string BIZCND { get; set; }

        /// <summary>
        /// 사업장_담당자_직위
        /// </summary>
        public string BIZ_CHARGER_OFCPS { get; set; }

        /// <summary>
        /// 사업장_담당자명
        /// </summary>
        public string BIZ_CHARGER_NM { get; set; }

        /// <summary>
        /// 사업장_주생산품
        /// </summary>
        public string BIZ_MAIN_PRODUCT { get; set; }

        /// <summary>
        /// 총 남자 인원
        /// </summary>
        public long TOT_NMPR_MALE { get; set; }

        /// <summary>
        /// 총 여자 인원
        /// </summary>
        public long TOT_NMPR_FEMALE { get; set; }

        /// <summary>
        /// 검진의사
        /// </summary>
        public string TCNXPRT_NM { get; set; }

        /// <summary>
        /// 설립일자
        /// </summary>
        public string FOND_DT { get; set; }

        /// <summary>
        /// 사업장 규모
        /// </summary>
        public string BIZ_SCALE { get; set; }

        /// <summary>
        /// 지원여부
        /// </summary>
        public string SUPPORT { get; set; }

        /// <summary>
        /// 총 검진 인원
        /// </summary>
        public long TOT_SLNS_NMPR { get; set; }

        /// <summary>
        /// 지원 인원
        /// </summary>
        public long TOT_SLNS_SPORT { get; set; }

        /// <summary>
        /// 수첩 인원
        /// </summary>
        public long TOT_SLNS_PKTBUK { get; set; }

        /// <summary>
        /// 대상근로자 남자 인원
        /// </summary>
        public long TRGTER_NMPR_MALE { get; set; }

        /// <summary>
        /// 대상근로자 여자 인원
        /// </summary>
        public long TRGTER_NMPR_FEMALE { get; set; }

        /// <summary>
        /// 비고
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// Hems 번호
        /// </summary>
        public long HEMSNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BPLC_SENDNG_DE { get; set; }

        /// <summary>
        /// HEMS 전송 사업장 마스터
        /// </summary>
        public HIC_HEMS_SLNS01()
        {
        }
    }
}
