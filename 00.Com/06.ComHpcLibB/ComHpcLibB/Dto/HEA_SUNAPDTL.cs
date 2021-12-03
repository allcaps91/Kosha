namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class HEA_SUNAPDTL : BaseDto
    {   
        /// <summary>
        /// 검진접수 일련번호
        /// </summary>	
		public long WRTNO { get; set; } 

        /// <summary>
        /// 묶음코드
        /// </summary>
		public string CODE { get; set; }

        /// <summary>
        /// 묶음코드
        /// </summary>
        public string GRPCODE { get; set; }

        /// <summary>
        /// 묶음코드
        /// </summary>
        public string GRPNAME { get; set; }

        /// <summary>
        /// 금액
        /// </summary>
        public long AMT { get; set; }

        /// <summary>
        /// 금액
        /// </summary>
        public long GAMAMT { get; set; }

        /// <summary>
        /// 부담율
        /// </summary>
        public string GBSELF { get; set; }

        /// <summary>
        /// 부담율
        /// </summary>
        public string BURATE { get; set; }

        /// <summary>
        /// 할인구분
        /// </summary>
        public string GBHALIN { get; set; } 

        /// <summary>
        /// 코드명
        /// </summary>
		public string CODENAME { get; set; }

        /// <summary>
        /// 코드명
        /// </summary>
		public string GAMNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string YNAME { get; set; }

        /// <summary>
        /// 종합검진 수납 상세내역
        /// </summary>
        public HEA_SUNAPDTL()
        {
        }
    }
}
