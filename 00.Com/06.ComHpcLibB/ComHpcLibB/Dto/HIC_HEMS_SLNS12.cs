namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// HEMS 전송 인적정보
    /// </summary>
    /// 
    public class HIC_HEMS_SLNS12 : BaseDto
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
        /// 이름
        /// </summary>
        public string INSPTN_NM { get; set; }

        /// <summary>
        /// 일반검진 공단 청구여부 (1: 청구 0:미청구)
        /// </summary>
        public string GLHMN_RQEST_YN { get; set; }

        /// <summary>
        /// 일반검진 가입 유형 (1:직장 2:지역 3:급여 4:기타)
        /// </summary>
        public string GLHMN_SSBYP { get; set; }

        /// <summary>
        /// 특수검진 공단 청구여부 (1: 청구 0:미청구)
        /// </summary>
        public string SLNS_RQEST_YN { get; set; }

        /// <summary>
        /// 특수검진 수검여부 (1.수검, 0.미수검)
        /// </summary>
        public string SLNS_YN { get; set; }

        /// <summary>
        /// 배치전 수검여부 (1.수검, 0.미수검)
        /// </summary>
        public string BEPOSTNG_SLNS_YN { get; set; }

        /// <summary>
        /// 수시 수검 여부 (1.수검, 0.미수검)
        /// </summary>
        public string ANYTM_SLNS_YN { get; set; }

        /// <summary>
        /// 임시검진 (1.수검, 0.미수검)
        /// </summary>
        public string TMPR_SLNS_YN { get; set; }

        /// <summary>
        /// 건강관리 수첩 여부 (1.소지, 0.해당없음)
        /// </summary>
        public string HM_PKTBUK_YN { get; set; }

        /// <summary>
        /// 전화번호
        /// </summary>
        public string TELNO { get; set; }

        /// <summary>
        /// 핸드폰
        /// </summary>
        public string CELNO { get; set; }

        /// <summary>
        /// 이 메일
        /// </summary>
        public string EMAIL_ADRES { get; set; }

        /// <summary>
        /// 사원번호
        /// </summary>
        public string EMPLNO { get; set; }

        /// <summary>
        /// 직종 형태 (1.사무직, 2.생산직(비사무직))
        /// </summary>
        public string JSSFC_SSBYP { get; set; }

        /// <summary>
        /// 입사일자
        /// </summary>
        public string ECNY_DT { get; set; }

        /// <summary>
        /// 전입일자
        /// </summary>
        public string TRNSFRN_DT { get; set; }

        /// <summary>
        /// 폭로기간 (00년00일)
        /// </summary>
        public string EXPSR_PD { get; set; }

        /// <summary>
        /// 1일 폭로시간
        /// </summary>
        public string DAY_EXPSR_TIME { get; set; }

        /// <summary>
        /// 공정코드
        /// </summary>
        public string PROCS_CD { get; set; }

        /// <summary>
        /// 공정명
        /// </summary>
        public string PROCS_NM { get; set; }

        /// <summary>
        /// 직종코드
        /// </summary>
        public string JSSFC_CD { get; set; }

        /// <summary>
        /// 직종명
        /// </summary>
        public string JSSFC_NM { get; set; }

        /// <summary>
        /// 부서1
        /// </summary>
        public string DEPT_NM1 { get; set; }

        /// <summary>
        /// 부서2
        /// </summary>
        public string DEPT_NM2 { get; set; }

        /// <summary>
        /// 우편번호
        /// </summary>
        public string INSPTN_ZIP { get; set; }

        /// <summary>
        /// 주소1
        /// </summary>
        public string INSPTN_ADRES1 { get; set; }

        /// <summary>
        /// 주소2
        /// </summary>
        public string INSPTN_ADRES2 { get; set; }

        /// <summary>
        /// 국가코드 (외국인일 경우)
        /// </summary>
        public string NATION_CD { get; set; }

        /// <summary>
        /// 가족력
        /// </summary>
        public string FAMILY_HSTCS { get; set; }

        /// <summary>
        /// 업무기인성
        /// </summary>
        public string JOB_CAUSE { get; set; }

        /// <summary>
        /// 흡연,음주
        /// </summary>
        public string SMKNG_DRNKG { get; set; }

        /// <summary>
        /// 치과 특검 청구 여부 (1: 청구 0:미청구)
        /// </summary>
        public string DSGR_RQEST_YN { get; set; }

        /// <summary>
        /// 사업장 사후관리 검사항목 제공 동의
        /// </summary>
        public string POSMANT_PROVD_YN { get; set; }

        /// <summary>
        /// 환자 비고
        /// </summary>
        public string SLNS_REMARK { get; set; }

        /// <summary>
        /// 남여구분 남자=M 여자=W
        /// </summary>
        public string SEXDSTN { get; set; }

        /// <summary>
        /// 비고
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// HEMS 번호
        /// </summary>
        public long HEMSNO { get; set; }

        /// <summary>
        /// 일반검진 동시 미실시 사유코드
        /// </summary>
        public string GENR_SMTM_NAT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CNSTRC_DELBR_CNFIRM { get; set; }

        /// <summary>
        /// HEMS 전송 인적정보
        /// </summary>
        public HIC_HEMS_SLNS12()
        {
        }
    }
}
