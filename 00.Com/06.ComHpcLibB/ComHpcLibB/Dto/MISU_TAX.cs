namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class MISU_TAX :BaseDto
    {
        /// <summary>
        /// 세금계산서 발급연도
        /// </summary>
        public string GJYEAR { get; set; }

        /// <summary>
        /// 세금계산서 일련번호
        /// </summary>
        public string WRTNO { get; set; }

        /// <summary>
        /// 발행일자
        /// </summary>
        public string BDATE { get; set; }

        /// <summary>
        /// 발행부서(1,건강증진센터 2,원무과 3,기타부서)
        /// </summary>
        public string GBBUSE { get; set; }

        /// <summary>
        /// 미수종류(아래참조)
        /// </summary>
        public string MJONG { get; set; }

        /// <summary>
        /// 회사코드
        /// </summary>
        public string LTDCODE { get; set; }

        /// <summary>
        /// 회사명
        /// </summary>
        public string LTDNAME { get; set; }

        /// <summary>
        /// 사업자등록번호
        /// </summary>
        public string LTDNO { get; set; }

        /// <summary>
        /// 대표자성명
        /// </summary>
        public string DAEPYONAME { get; set; }

        /// <summary>
        /// 회사주소
        /// </summary>
        public string LTDJUSO { get; set; }

        /// <summary>
        /// 업태
        /// </summary>
        public string UPTAE { get; set; }

        /// <summary>
        /// 업종
        /// </summary>
        public string JONGMOK { get; set; }

        /// <summary>
        /// 미수관리 번호(WRTNO)
        /// </summary>
        public long MISUNO { get; set; }

        /// <summary>
        /// 청구번호
        /// </summary>
        public string MIRNO { get; set; }

        /// <summary>
        /// 금액
        /// </summary>
        public long AMT { get; set; }

        /// <summary>
        /// 발급일시
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 발급자 사번
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 취소일자
        /// </summary>
        public DateTime? DELDATE { get; set; }

        /// <summary>
        /// 취소자 사번
        /// </summary>
        public long DELSABUN { get; set; }

        /// <summary>
        /// 취소 사유
        /// </summary>
        public string DELREMARK { get; set; }

        /// <summary>
        /// 미수내역
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 품목(2002.1.10 추가)
        /// </summary>
        public string PUMMOK { get; set; }

        /// <summary>
        /// 전자계산서여부(Y.전자계산서)
        /// </summary>
        public string GBTRB { get; set; }

        /// <summary>
        /// 전자계산서 발급번호
        /// </summary>
        public long TRBNO { get; set; }

        /// <summary>
        /// 전자계산서 취소번호
        /// </summary>
        public long TRBNO2 { get; set; }

        /// <summary>
        /// 국세청 취소계산서 발생일자
        /// </summary>
        public DateTime? BMDATE { get; set; }

        /// <summary>
        /// 대행회사코드
        /// </summary>
        public string LTDCODE2 { get; set; }

        /// <summary>
        /// 대행회사명
        /// </summary>
        public string LTDNAME2 { get; set; }

        /// <summary>
        /// 대행회사 사업자등록번호
        /// </summary>
        public string LTDNO2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UPTAE2 { get; set; }

        /// <summary>
        /// 대행외사업종
        /// </summary>
        public string JONGMOK2 { get; set; }

        public long GAMT{ get; set; }
        public string LTD { get; set; }
        public string LTD2 { get; set; }
        public string NAME { get; set; }
        public string NAME2 { get; set; }

        public string TAXWRTNO { get; set; }
        public MISU_TAX()
        {

        }
    }
}
