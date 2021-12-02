namespace ComHpcLibB.Model
{
    using ComBase.Mvc;

    public class HEA_JEPSU_SUNAP : BaseDto
    {
        /// <summary>
        /// 성별
        /// </summary>
		public string SEX { get; set; }

        /// <summary>
        /// 나이
        /// </summary>
		public int AGE { get; set; }

        /// <summary>
        /// 접수일자 YYYY-MM-DD
        /// </summary>
		public string SDATE { get; set; }

        /// <summary>
        /// 접수일자 MM/DD
        /// </summary>
		public string SDATE_MMDD { get; set; }

        /// <summary>
        /// 수납일자
        /// </summary>
		public string SUDATE { get; set; }

        /// <summary>
        /// 수납일자 MM/DD
        /// </summary>
		public string SUDATE_MMDD { get; set; }

        /// <summary>
        /// 수검자명
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 등록번호
        /// </summary>
		public string PTNO { get; set; }

        /// <summary>
        /// 검진번호
        /// </summary>
		public long PANO { get; set; }

        /// <summary>
        /// 개별접수번호
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 그룹접수번호
        /// </summary>
		public long GWRTNO { get; set; }

        /// <summary>
        /// 사업장코드
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 사업장명
        /// </summary>
		public string LTDNAME { get; set; }

        /// <summary>
        /// 작업자사번
        /// </summary>
		public long JOBSABUN { get; set; }

        /// <summary>
        /// 작업자명
        /// </summary>
		public string JOBNAME { get; set; }

        /// <summary>
        /// 수납 Seq
        /// </summary>
		public int SEQNO { get; set; }

        /// <summary>
        /// 검진종류
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 검진종류명
        /// </summary>
		public string GJNAME { get; set; }

        /// <summary>
        /// 주민번호
        /// </summary>
        public string JUMINNO { get; set; }

        /// <summary>
        /// 총금액
        /// </summary>
		public long TOTAMT { get; set; }

        /// <summary>
        /// 조합부담금액
        /// </summary>
		public long JOHAPAMT { get; set; }

        /// <summary>
        /// 회사부담금액
        /// </summary>
		public long LTDAMT { get; set; }

        /// <summary>
        /// 본인부담금액
        /// </summary>
		public long BONINAMT { get; set; }

        /// <summary>
        /// 미수계정
        /// </summary>
		public string MISUGYE { get; set; }

        /// <summary>
        /// 미수금액
        /// </summary>
		public long MISUAMT { get; set; }

        /// <summary>
        /// 할인금액
        /// </summary>
		public long HALINAMT { get; set; }

        /// <summary>
        /// 할인계정
        /// </summary>
		public string HALINGYE { get; set; }

        /// <summary>
        /// 할인계정명
        /// </summary>
		public string HALINGYE_NM { get; set; }

        /// <summary>
        /// 본인부담금액
        /// </summary>
		public long SUNAPAMT { get; set; }

        /// <summary>
        /// 현금영수금액
        /// </summary>
		public long SUNAPAMT1 { get; set; }

        /// <summary>
        /// 카드영수금액
        /// </summary>
		public long SUNAPAMT2 { get; set; }

        /// <summary>
        ///  카드결제 여부 ◎
        /// </summary>
		public string GBCARD { get; set; }

        /// <summary>
        /// 비고
        /// </summary>
		public string REMARK { get; set; }

        /// <summary>
        /// 진행상태
        /// </summary>
		public string GBSTS { get; set; }

        /// <summary>
        /// 수납여부
        /// </summary>
		public string SUNAP { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SANGDAM_ONE { get; set; }
        
        public HEA_JEPSU_SUNAP()
        {
        }
    }
}
