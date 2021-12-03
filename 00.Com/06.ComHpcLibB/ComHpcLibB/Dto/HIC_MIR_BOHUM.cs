namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_MIR_BOHUM : BaseDto
    {

        /// <summary>
        /// 청구번호(일련번호)
        /// </summary>
        public long MIRNO { get; set; }

        /// <summary>
        /// 사업년도
        /// </summary>
        public string YEAR { get; set; }

        /// <summary>
        /// 검진구분(1.직장 2.지역,직장피부양자)
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// 직역구분(J.지역 K.직장 G.공교 T.공단-통합)
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
        /// 결과통보건수
        /// </summary>
        public long JEPQTY { get; set; }

        /// <summary>
        /// 총청구금액
        /// </summary>
        public long TAMT { get; set; }

        /// <summary>
        /// 청구(접수)일자
        /// </summary>
        public string JEPDATE { get; set; }

        /// <summary>
        /// 1차 통보건수
        /// </summary>
        public long ONE_QTY { get; set; }

        /// <summary>
        /// 1차 총 청구금액
        /// </summary>
        public long ONE_TAMT { get; set; }

        /// <summary>
        /// 1차 진찰및상담
        /// </summary>
        public long ONE_INWON011 { get; set; }

        /// <summary>
        /// 1차 흉부방사선검사
        /// </summary>
        public long ONE_INWON012 { get; set; }

        /// <summary>
        /// 1차 요검사
        /// </summary>
        public long ONE_INWON021 { get; set; }

        /// <summary>
        /// 1차 혈색소
        /// </summary>
        public long ONE_INWON022 { get; set; }

        /// <summary>
        /// 1차 식전혈당
        /// </summary>
        public long ONE_INWON031 { get; set; }

        /// <summary>
        /// 1차 총콜레스테롤
        /// </summary>
        public long ONE_INWON032 { get; set; }

        /// <summary>
        /// 1차 HDL콜레스테롤
        /// </summary>
        public long ONE_INWON041 { get; set; }

        /// <summary>
        /// 1차 트리글리세라이드
        /// </summary>
        public long ONE_INWON042 { get; set; }

        /// <summary>
        /// 1차 AST(SGOT)
        /// </summary>
        public long ONE_INWON051 { get; set; }

        /// <summary>
        /// 1차 ALT(SGPT)
        /// </summary>
        public long ONE_INWON052 { get; set; }

        /// <summary>
        /// 1차 감마지티피
        /// </summary>
        public long ONE_INWON061 { get; set; }

        /// <summary>
        /// 1차 혈청크레아티닌
        /// </summary>
        public long ONE_INWON062 { get; set; }

        /// <summary>
        /// 1차 간염항원일반
        /// </summary>
        public long ONE_INWON071 { get; set; }

        /// <summary>
        /// 1차 간염항원정밀
        /// </summary>
        public long ONE_INWON072 { get; set; }

        /// <summary>
        /// 1차 간염항체일반
        /// </summary>
        public long ONE_INWON081 { get; set; }

        /// <summary>
        /// 1차 간염항체정밀
        /// </summary>
        public long ONE_INWON082 { get; set; }

        /// <summary>
        /// 1차 양방사선골밀도
        /// </summary>
        public long ONE_INWON091 { get; set; }

        /// <summary>
        /// 1차 양방사선말단골밀도
        /// </summary>
        public long ONE_INWON092 { get; set; }

        /// <summary>
        /// 1차 적량적전산화단층
        /// </summary>
        public long ONE_INWON101 { get; set; }

        /// <summary>
        /// 1차 초음파골밀도
        /// </summary>
        public long ONE_INWON102 { get; set; }

        /// <summary>
        /// 1차 노인신체기능검사
        /// </summary>
        public long ONE_INWON111 { get; set; }

        /// <summary>
        /// 1차 LDL콜레스테롤(추가분)
        /// </summary>
        public long ONE_INWON112 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long ONE_INWON121 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long ONE_INWON122 { get; set; }

        /// <summary>
        /// 2차 통보건수
        /// </summary>
        public long TWO_QTY { get; set; }

        /// <summary>
        /// 2차 총 청구금액
        /// </summary>
        public long TWO_TAMT { get; set; }

        /// <summary>
        /// 1차 건강진단 결과 사후상담
        /// </summary>
        public long TWO_INWON01 { get; set; }

        /// <summary>
        /// 2차 식전혈당(혈액화확분석기)
        /// </summary>
        public long TWO_INWON02 { get; set; }

        /// <summary>
        /// 2차 식전혈당(자가혈당측정기)
        /// </summary>
        public long TWO_INWON03 { get; set; }

        /// <summary>
        /// 2차 기본(상담)
        /// </summary>
        public long TWO_INWON04 { get; set; }

        /// <summary>
        /// 2차 흡연
        /// </summary>
        public long TWO_INWON05 { get; set; }

        /// <summary>
        /// 2차 음주
        /// </summary>
        public long TWO_INWON06 { get; set; }

        /// <summary>
        /// 2차 운동
        /// </summary>
        public long TWO_INWON07 { get; set; }

        /// <summary>
        /// 2차 영양
        /// </summary>
        public long TWO_INWON08 { get; set; }

        /// <summary>
        /// 2차 비만
        /// </summary>
        public long TWO_INWON09 { get; set; }

        /// <summary>
        /// 2차 우울증(CES-D)
        /// </summary>
        public long TWO_INWON10 { get; set; }

        /// <summary>
        /// 2차 우울증(GDS)
        /// </summary>
        public long TWO_INWON11 { get; set; }

        /// <summary>
        /// 2차 인지기능검사(치매)(KDSQ-C)
        /// </summary>
        public long TWO_INWON12 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON13 { get; set; }

        /// <summary>
        /// 입금일자
        /// </summary>
        public string IPGUMDATE { get; set; }

        /// <summary>
        /// 입금금액
        /// </summary>
        public long IPGUMAMT { get; set; }

        /// <summary>
        /// 접수 시작일자
        /// </summary>
        public string FRDATE { get; set; }

        /// <summary>
        /// 접수 종료일자
        /// </summary>
        public string TODATE { get; set; }

        /// <summary>
        /// 비용청구서 형성 일자 및 시각
        /// </summary>
        public string BUILDDATE { get; set; }

        /// <summary>
        /// 비용청구서 형성자 사번
        /// </summary>
        public long BUILDSABUN { get; set; }

        /// <summary>
        /// 청구형성 건수
        /// </summary>
        public long BUILDCNT { get; set; }

        /// <summary>
        /// 사전점검상태(NULL:미실시 N.오류있음 Y.오류없음)
        /// </summary>
        public string GBERRCHK { get; set; }

        /// <summary>
        /// 청구파일이름
        /// </summary>
        public string FILENAME { get; set; }

        /// <summary>
        /// 2차단독청구시 사용(Y)
        /// </summary>
        public string CHASU { get; set; }

        /// <summary>
        /// 추가청구(Y)
        /// </summary>
        public string MIRGBN { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON21 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON22 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON23 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON24 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON25 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON26 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON27 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON28 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON29 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON30 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON31 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON32 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON33 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON34 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON35 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON36 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON37 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON38 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON39 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON40 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON41 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON14 { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public long TWO_INWON15 { get; set; }

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
        /// 1차 토.공휴일 가산료
        /// </summary>
        public long ONE_INWON013 { get; set; }

        /// <summary>
        /// 2차 토.공휴일 가산료
        /// </summary>
        public long TWO_INWON16 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDCODE2 { get; set; }

        /// <summary>
        /// 조합구분(1.지역 2.공교 3.직장 4.급여 0.공통)
        /// </summary>
        public string GBJOHAP { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
		public string ROWID { get; set; }

        /// <summary>
        /// 건강검진 비용청구서
        /// </summary>
        public HIC_MIR_BOHUM()
        {
        }
    }
}
