namespace HC_IF.Dto
{
    using ComBase.Mvc;

    public class HEA_KIOSK : BaseDto
    {
        /// <summary>
        /// 예약자명
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 검진종류
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 나이
        /// </summary>
		public long AGE { get; set; }

        /// <summary>
        /// 성별
        /// </summary>
		public string SEX { get; set; }

        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 검진번호
        /// </summary>
		public long PANO { get; set; }

        /// <summary>
        /// 등록번호
        /// </summary>
		public string PTNO { get; set; }

        /// <summary>
        /// 사업장기호
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 사업장명
        /// </summary>
		public string LTDNAME { get; set; }

        /// <summary>
        /// 주민등록번호
        /// </summary>
		public string JUMIN { get; set; }

        /// <summary>
        /// 주소
        /// </summary>
		public string JUSO { get; set; }

        /// <summary>
        /// 휴대폰번호
        /// </summary>
		public string TEL { get; set; }

        /// <summary>
        /// 휴대폰번호
        /// </summary>
		public string HPHONE { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
		public string RID { get; set; }

        /// <summary>
        /// 버튼위치 (배열순서)
        /// </summary>
		public long INDX { get; set; }

        /// <summary>
        /// 수검상태
        /// </summary>
		public string GBSTS { get; set; }

        public HEA_KIOSK()
        {
        }
    }
}
