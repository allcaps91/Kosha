namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// HEMS 전송 판정내역
    /// </summary>
    /// 
    public class HIC_HEMS_SLNS05 : BaseDto
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
        /// 유해물질
        /// </summary>
        public string HRMFLNS_FACTR_CD { get; set; }

        /// <summary>
        /// 표적 장기 코드
        /// </summary>
        public string TARGT_INORG_CD { get; set; }

        /// <summary>
        /// 좌우 구분 코드
        /// </summary>
        public string SWING_SE { get; set; }

        /// <summary>
        /// 2차 검진인지 1차인지 구분
        /// </summary>
        public string SLNS_SCD_CD { get; set; }

        /// <summary>
        /// 1차 소견 코드
        /// </summary>
        public string FIRST_OPIN_CD { get; set; }

        /// <summary>
        /// 1차 소견 명
        /// </summary>
        public string FIRST_OPIN_NM { get; set; }

        /// <summary>
        /// 1차 판정 코드
        /// </summary>
        public string FIRST_JDGMNT_CD { get; set; }

        /// <summary>
        /// 1차 조치 코드
        /// </summary>
        public string FIRST_MANAGT_CD { get; set; }

        /// <summary>
        /// 1차 조치 명
        /// </summary>
        public string FIRST_MANAGT_NM { get; set; }

        /// <summary>
        /// 2차 소견 코드
        /// </summary>
        public string SCD_OPIN_CD { get; set; }

        /// <summary>
        /// 2차 소견 명
        /// </summary>
        public string SCD_OPIN_NM { get; set; }

        /// <summary>
        /// 2차 판정 코드
        /// </summary>
        public string SCD_JDGMNT_CD { get; set; }

        /// <summary>
        /// 2차 조치 명
        /// </summary>
        public string SCD_MANAGT_NM { get; set; }

        /// <summary>
        /// 2차 조치 코드
        /// </summary>
        public string SCD_MANAGT_CD { get; set; }

        /// <summary>
        /// 질병 코드
        /// </summary>
        public string DISS_CD { get; set; }

        /// <summary>
        /// 사후관리조치코드1
        /// </summary>
        public string POSMANT_MANAGT_CD_1 { get; set; }

        /// <summary>
        /// 사후관리조치코드2
        /// </summary>
        public string POSMANT_MANAGT_CD_2 { get; set; }

        /// <summary>
        /// 사후관리조치코드3
        /// </summary>
        public string POSMANT_MANAGT_CD_3 { get; set; }

        /// <summary>
        /// 사후관리조치내용1
        /// </summary>
        public string POSMANT_MANAGT_CN_1 { get; set; }

        /// <summary>
        /// 사후관리조치내용2
        /// </summary>
        public string POSMANT_MANAGT_CN_2 { get; set; }

        /// <summary>
        /// 사후관리조치내용3
        /// </summary>
        public string POSMANT_MANAGT_CN_3 { get; set; }

        /// <summary>
        /// 사후관리조치_추적검사일시1
        /// </summary>
        public string POSMANT_MANAGT_DT_1 { get; set; }

        /// <summary>
        /// 사후관리조치_추적검사일시2
        /// </summary>
        public string POSMANT_MANAGT_DT_2 { get; set; }

        /// <summary>
        /// 사후관리조치_추적검사일시3
        /// </summary>
        public string POSMANT_MANAGT_DT_3 { get; set; }

        /// <summary>
        /// 업무적합성 코드
        /// </summary>
        public string JOB_STBLT_CD { get; set; }

        /// <summary>
        /// 1차 R일때 소견 (삭제)
        /// </summary>
        public string BFE_OPIN_CD { get; set; }

        /// <summary>
        /// 판정 비고
        /// </summary>
        public string JDGMNT_REMARK { get; set; }

        /// <summary>
        /// 기타소견(비출력용)
        /// </summary>
        public string ETC_REMARK { get; set; }

        /// <summary>
        /// 표적 장기 선정 사유
        /// </summary>
        public string TARGT_INORG_RESN { get; set; }

        /// <summary>
        /// 주기 단축 사유 코드
        /// </summary>
        public string CYCLE_SHRTEN_CD { get; set; }

        /// <summary>
        /// 주기 단축 사유
        /// </summary>
        public string CYCLE_SHRTEN_RESN { get; set; }

        /// <summary>
        /// 주기 단축 일
        /// </summary>
        public string CYCLE_SHRTEN_DT { get; set; }

        /// <summary>
        /// HEMS 번호
        /// </summary>
        public long HEMSNO { get; set; }

        /// <summary>
        /// HEMS 전송 판정내역
        /// </summary>
        public HIC_HEMS_SLNS05()
        {
        }
    }
}
