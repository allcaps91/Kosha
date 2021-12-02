namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_MIR_CANCER_BO : BaseDto
    {

        /// <summary>
        /// 청구번호
        /// </summary>
        public long MIRNO { get; set; }

        /// <summary>
        /// 사업년도
        /// </summary>
        public string YEAR { get; set; }

        /// <summary>
        /// 검진구분(4.암검진)
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// 직역구분(J.지역 K.직장 G.공교)
        /// </summary>
        public string JOHAP { get; set; }

        /// <summary>
        /// 사업장기호
        /// </summary>
        public string KIHO { get; set; }

        /// <summary>
        /// 청구서번호(yymmdd01~99)
        /// </summary>
        public string JEPNO { get; set; }

        /// <summary>
        /// 통보건수
        /// </summary>
        public long JEPQTY { get; set; }

        /// <summary>
        /// 총청구금액
        /// </summary>
        public long TAMT { get; set; }

        /// <summary>
        /// 청구(접수)일자
        /// </summary>
        public DateTime? JEPDATE { get; set; }

        /// <summary>
        /// 위암:위장조영검사
        /// </summary>
        public long INWON01 { get; set; }

        /// <summary>
        /// 위암:위내시경검사
        /// </summary>
        public long INWON02 { get; set; }

        /// <summary>
        /// 위암:조직검사 1-3
        /// </summary>
        public long INWON03 { get; set; }

        /// <summary>
        /// 위암:조직검사 4-6
        /// </summary>
        public long INWON04 { get; set; }

        /// <summary>
        /// 위암:조직검사 7-9
        /// </summary>
        public long INWON05 { get; set; }

        /// <summary>
        /// 위암:조직검사 10-12
        /// </summary>
        public long INWON06 { get; set; }

        /// <summary>
        /// 위암:조직검사 13이상
        /// </summary>
        public long INWON07 { get; set; }

        /// <summary>
        /// 간암:의료급여-ALT
        /// </summary>
        public long INWON08 { get; set; }

        /// <summary>
        /// 간암:의료급여-B형간염항원
        /// </summary>
        public long INWON09 { get; set; }

        /// <summary>
        /// 간암:의료급여-C형간염항체
        /// </summary>
        public long INWON10 { get; set; }

        /// <summary>
        /// 간암:간초음파검사
        /// </summary>
        public long INWON11 { get; set; }

        /// <summary>
        /// 간암:혈청알파태아단백-RPHA
        /// </summary>
        public long INWON12 { get; set; }

        /// <summary>
        /// 입금일자
        /// </summary>
        public DateTime? IPGUMDATE { get; set; }

        /// <summary>
        /// 입금금액
        /// </summary>
        public long IPGUMAMT { get; set; }

        /// <summary>
        /// 간암:혈청알파태아단백-EIA
        /// </summary>
        public long INWON13 { get; set; }

        /// <summary>
        /// 접수 시작일자
        /// </summary>
        public DateTime? FRDATE { get; set; }

        /// <summary>
        /// 접수 종료일자
        /// </summary>
        public DateTime? TODATE { get; set; }

        /// <summary>
        /// 청구형성 건수
        /// </summary>
        public long BUILDCNT { get; set; }

        /// <summary>
        /// 청구형성 일자 및 시각
        /// </summary>
        public DateTime? BUILDDATE { get; set; }

        /// <summary>
        /// 청구형성 작업자 사번
        /// </summary>
        public long BUILDSABUN { get; set; }

        /// <summary>
        /// 사전점검상태(NULL:미실시 N.오류있음 Y.오류없음)
        /// </summary>
        public string GBERRCHK { get; set; }

        /// <summary>
        /// 대장암:분변잠혈반응검사-RPHA
        /// </summary>
        public long INWON14 { get; set; }

        /// <summary>
        /// 청구 파일 이름
        /// </summary>
        public string FILENAME { get; set; }

        /// <summary>
        /// 보건소청구 구분 (Y:보건소청구, C 마일리지 암, E 의료보험암)
        /// </summary>
        public string GBBOGUN { get; set; }

        /// <summary>
        /// 대장암:분변잠혈반응검사-분변혈색소정량법
        /// </summary>
        public long INWON15 { get; set; }

        /// <summary>
        /// 추가청구(Y)
        /// </summary>
        public string MIRGBN { get; set; }

        /// <summary>
        /// [위암]상담료
        /// </summary>
        public long SANGDAM1 { get; set; }

        /// <summary>
        /// [대장암]상담료
        /// </summary>
        public long SANGDAM2 { get; set; }

        /// <summary>
        /// [간암]상담료
        /// </summary>
        public long SANGDAM3 { get; set; }

        /// <summary>
        /// [유방암]상담료
        /// </summary>
        public long SANGDAM4 { get; set; }

        /// <summary>
        /// [자궁경부암]상담료
        /// </summary>
        public long SANGDAM5 { get; set; }

        /// <summary>
        /// 2차단독청구
        /// </summary>
        public string CHASU { get; set; }

        /// <summary>
        /// 대장암:대장이중조영검사
        /// </summary>
        public long INWON16 { get; set; }

        /// <summary>
        /// 대장암:대장내시경검사
        /// </summary>
        public long INWON17 { get; set; }

        /// <summary>
        /// 대장암:조직검사 1-3
        /// </summary>
        public long INWON18 { get; set; }

        /// <summary>
        /// 대장암:조직검사 4-6
        /// </summary>
        public long INWON19 { get; set; }

        /// <summary>
        /// 대장암:조직검사 7-9
        /// </summary>
        public long INWON20 { get; set; }

        /// <summary>
        /// 대장암:조직검사 10-12
        /// </summary>
        public long INWON21 { get; set; }

        /// <summary>
        /// 대장암:조직검사 13이상
        /// </summary>
        public long INWON22 { get; set; }

        /// <summary>
        /// 유방암:유방촬영
        /// </summary>
        public long INWON23 { get; set; }

        /// <summary>
        /// 자궁경부암:자궁경부세포검사
        /// </summary>
        public long INWON24 { get; set; }

        /// <summary>
        /// 암검진 실시인원
        /// </summary>
        public long INWON25 { get; set; }

        /// <summary>
        /// 청구번호 - 청구삭제시 갱신됨
        /// </summary>
        public long OLDMIRNO { get; set; }

        /// <summary>
        /// 생애구분 Y
        /// </summary>
        public string LIFE_GBN { get; set; }

        /// <summary>
        /// 공단청구번호
        /// </summary>
        public string NHICNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDCODE2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBJOHAP { get; set; }

        /// <summary>
        /// 토요일,공휴30%가산 건수
        /// </summary>
        public long INWON26 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long SANGDAM6 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long INWON27 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long INWON28 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ROWID { get; set; }


        /// <summary>
        /// 암검진 청구 (보건소용)
        /// </summary>
        public HIC_MIR_CANCER_BO()
        {
        }
    }
}
