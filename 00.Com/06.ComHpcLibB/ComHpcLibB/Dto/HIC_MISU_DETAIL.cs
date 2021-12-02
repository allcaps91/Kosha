namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_MISU_DETAIL : BaseDto
    {

        /// <summary>
        /// 접수번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 미수 일련번호
        /// </summary>
        public long MISUNO { get; set; }

        /// <summary>
        /// 검진구분(H:일반건진 T:종합검진)
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// 검진종류
        /// </summary>
        public string GJJONG { get; set; }

        /// <summary>
        /// 회사코드
        /// </summary>
        public long LTDCODE { get; set; }

        /// <summary>
        /// 미수종류(HIC_MISU_MST 미수종류 참조)
        /// </summary>
        public string MISUJONG { get; set; }

        /// <summary>
        /// 발생일자
        /// </summary>
        public string BDATE { get; set; }

        /// <summary>
        /// 일력일자
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 작업사번
        /// </summary>
        public long ENTJOBSABUN { get; set; }

        /// <summary>
        /// 미수금액
        /// </summary>
        public long MISUAMT { get; set; }

        /// <summary>
        /// 입금금액
        /// </summary>
        public long IPGUMAMT { get; set; }

        /// <summary>
        /// 감액
        /// </summary>
        public long GAMAMT { get; set; }

        /// <summary>
        /// 반송금액
        /// </summary>
        public long BANAMT { get; set; }

        /// <summary>
        /// 삭감금액
        /// </summary>
        public long SAKAMT { get; set; }

        /// <summary>
        /// 할인금액
        /// </summary>
        public long HALAMT { get; set; }

        /// <summary>
        /// 기타금액
        /// </summary>
        public long ETCAMT { get; set; }

        /// <summary>
        /// 잔액
        /// </summary>
        public long JANAMT { get; set; }

        /// <summary>
        /// 담당자명
        /// </summary>
        public string DAMNAME { get; set; }

        /// <summary>
        /// 비고
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 공단접수번호
        /// </summary>
        public long CNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDCODE2 { get; set; }


public HIC_MISU_DETAIL()
                    {
                    }
    }
}
