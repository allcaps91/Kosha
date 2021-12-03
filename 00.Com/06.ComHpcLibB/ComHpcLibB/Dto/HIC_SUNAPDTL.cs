namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class HIC_SUNAPDTL : BaseDto
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
        /// 유해물질
        /// </summary>
		public string UCODE { get; set; } 

        /// <summary>
        /// 금액
        /// </summary>
		public long AMT { get; set; } 

        /// <summary>
        /// 부담율
        /// </summary>
		public string GBSELF { get; set; }

        /// <summary>
        /// 묶음코드명(Hic_GROUPCODE)
        /// </summary>
        public string NAME { get; set; }

        public string RID { get; set; }

        public long COUNT { get; set; }

        public long CNT { get; set; }

        /// <summary>
        /// HIC_GROUPCODE 칼럼(검진동의서 출력 사용)
        /// </summary>
        public string GBSANGDAM { get; set; }


        public long JOHAPAMT { get; set; }
        public long DENTAMT { get; set; }
        public string MINDATE { get; set; }
        public string MAXDATE { get; set; }
        /// <summary>
        /// 수납 상세내역(검진항목별)
        /// </summary>
        public HIC_SUNAPDTL()
        {
        }
    }
}
