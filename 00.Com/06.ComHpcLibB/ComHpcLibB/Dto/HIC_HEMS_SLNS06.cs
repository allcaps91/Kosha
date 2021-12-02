namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// HEMS 전송 검사결과
    /// </summary>
    /// 
    public class HIC_HEMS_SLNS06 : BaseDto
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
        /// 1,2 차 구분
        /// </summary>
        public string SLNS_KND { get; set; }

        /// <summary>
        /// 검사항목 코드
        /// </summary>
        public string INSPCT_CD { get; set; }

        /// <summary>
        /// 결과
        /// </summary>
        public string INSPCT_RESULT { get; set; }

        /// <summary>
        /// 결과 코드
        /// </summary>
        public string INSPCT_RESULT_CD { get; set; }

        /// <summary>
        /// 결과 판정 (0: 결과코드 이상, 1: 로우, 2: 하이)
        /// </summary>
        public string INSPCT_RESULT_JDGMNT { get; set; }

        /// <summary>
        /// 결과값 유형 코드
        /// </summary>
        public string RESULT_TY_CD { get; set; }

        /// <summary>
        /// 임상참고치(미니멈)
        /// </summary>
        public string CLINC_REFER_LOW { get; set; }

        /// <summary>
        /// 임상참고치(맥스)
        /// </summary>
        public string CLINC_REFER_HGH { get; set; }

        /// <summary>
        /// 수가
        /// </summary>
        public long SLNS_MCCHRG { get; set; }

        /// <summary>
        /// 청구제외
        /// </summary>
        public string EXCEPT_FOR_CLAIM { get; set; }

        /// <summary>
        /// 고가검사 유무
        /// </summary>
        public string HGH_MCCHRG_CHK { get; set; }

        /// <summary>
        /// 고가검사 코드
        /// </summary>
        public string HGH_MCCHRG_CD { get; set; }

        /// <summary>
        /// 고가 검사 사유
        /// </summary>
        public string HGH_MCCHRG_RESN { get; set; }

        /// <summary>
        /// 해제 유무
        /// </summary>
        public string NOT_INSPCT_CHK { get; set; }

        /// <summary>
        /// 해제 사유
        /// </summary>
        public string NOT_INSPCT_RESN { get; set; }

        /// <summary>
        /// 결과_비고
        /// </summary>
        public string INSPCT_REMARK { get; set; }

        /// <summary>
        /// 수첩 물질 검사일 경우
        /// </summary>
        public string HM_PKTBUK_KND { get; set; }

        /// <summary>
        /// 필수검사항목여부 1.필수, 0.선택
        /// </summary>
        public string REQUIRE_CHK { get; set; }

        /// <summary>
        /// 검사단위 (유형코드일 경우 NULL)
        /// </summary>
        public string INSPTN_UNIT { get; set; }

        /// <summary>
        /// HEMS 번호
        /// </summary>
        public long HEMSNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string INSPCT_RESULT_JDGMNT_DET { get; set; }

        /// <summary>
        /// HEMS 전송 검사결과
        /// </summary>
        public HIC_HEMS_SLNS06()
        {
        }
    }
}
