namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_MIR_DENTAL : BaseDto
    {

        /// <summary>
        /// 청구번호
        /// </summary>
        public long MIRNO { get; set; }

        /// <summary>
        /// 사업연도
        /// </summary>
        public string YEAR { get; set; }

        /// <summary>
        /// 청구구분(3.구강검사)
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
        public string JEPDATE { get; set; }

        /// <summary>
        /// 삭제일자
        /// </summary>
        public string DELDATE { get; set; }

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
        /// 토.공휴일 가산료 건수
        /// </summary>
        public long HUQTY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDCODE2 { get; set; }

        /// <summary>
        /// 조합구분(1.지역 2.공교 3.직장 4.급여 0.공통)
        /// </summary>
        public string GBJOHAP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ROWID { get; set; }


        /// <summary>
        /// 구강검사 청구서
        /// </summary>
        public HIC_MIR_DENTAL()
        {
        }
    }
}
