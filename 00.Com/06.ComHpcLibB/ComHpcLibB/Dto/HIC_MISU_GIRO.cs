namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_MISU_GIRO : BaseDto
    {

        /// <summary>
        /// 지로의뢰서 일련번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 발행일자
        /// </summary>
        public string BDATE { get; set; }

        /// <summary>
        /// 납기일
        /// </summary>
        public string MDATE { get; set; }

        /// <summary>
        /// 발행부서(1.건강증진센타 2.원무과 3.기타부서)
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
        /// 발행일시
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 발행자 사번
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 취소일자
        /// </summary>
        public string DELDATE { get; set; }

        /// <summary>
        /// 취소자 사번
        /// </summary>
        public long DELSABUN { get; set; }

        /// <summary>
        /// 취소 사유
        /// </summary>
        public string DELREMARK { get; set; }




public HIC_MISU_GIRO()
        {
        }
    }
}
