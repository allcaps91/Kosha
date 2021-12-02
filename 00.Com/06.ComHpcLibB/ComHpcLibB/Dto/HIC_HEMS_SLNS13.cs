namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// HEMS 전송 검진정보
    /// </summary>
    /// 
    public class HIC_HEMS_SLNS13 : BaseDto
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
        /// 검진일
        /// </summary>
        public string SLNS_DT { get; set; }

        /// <summary>
        /// 검진 장소 (1.출장, 2.내원)
        /// </summary>
        public string SLNS_PLACE { get; set; }

        /// <summary>
        /// 검진 공휴일 진료 유무
        /// </summary>
        public string SLNS_HDAYYN { get; set; }

        /// <summary>
        /// 검사 번호
        /// </summary>
        public string SLNS_NO { get; set; }

        /// <summary>
        /// 차트 번호
        /// </summary>
        public string CHT_NO { get; set; }

        /// <summary>
        /// 흉부 촬영 번호
        /// </summary>
        public string BRST_POTOGRFNO { get; set; }

        /// <summary>
        /// 흉부 촬영 방법 (1.간촬, 2직촬,3.Full Pacrs, 4.CR,DR)
        /// </summary>
        public string POTOGRFNO_MTH { get; set; }

        /// <summary>
        /// 흉부 촬영 매수
        /// </summary>
        public string POTOGRFNO_NMRS { get; set; }

        /// <summary>
        /// 판독의사 이름
        /// </summary>
        public string POTOGRFNO_DOCTRNM { get; set; }

        /// <summary>
        /// 판독의사  면허
        /// </summary>
        public string POTOGRFNO_DOCTRLCN { get; set; }

        /// <summary>
        /// 치과 의사 이름
        /// </summary>
        public string DSGR_DOCTRNM { get; set; }

        /// <summary>
        /// 치과 의사 면허
        /// </summary>
        public string DSGR_DOCTRLCN { get; set; }

        /// <summary>
        /// 판정 의사 이름
        /// </summary>
        public string DOCTR_NM { get; set; }

        /// <summary>
        /// 판정 의사 면허 번호
        /// </summary>
        public string DOCTR_LCNSE { get; set; }

        /// <summary>
        /// 필름 종류 (1.70mm, 2.100mm)
        /// </summary>
        public string FILM_KND { get; set; }

        /// <summary>
        /// 조영제 사용 유무 (사용 : 1 미사용 NULL)
        /// </summary>
        public string MEDIA_YN { get; set; }

        /// <summary>
        /// HEMS 번호
        /// </summary>
        public long HEMSNO { get; set; }

        /// <summary>
        /// 일반검진 공단 청구여부(1:청구, 0:미청구)
        /// </summary>
        public string GLHMN_RQEST_YN { get; set; }

        /// <summary>
        /// HEMS 전송 검진정보
        /// </summary>
        public HIC_HEMS_SLNS13()
        {
        }
    }
}
