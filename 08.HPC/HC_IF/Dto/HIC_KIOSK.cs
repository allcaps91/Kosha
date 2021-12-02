namespace HC_IF.Dto
{
    using ComBase.Mvc;

    public class HIC_KIOSK : BaseDto
    {
        /// <summary>
        /// 예약접수 번호
        /// </summary>
		public long YNO { get; set; }

        /// <summary>
        /// 예약자명
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 나이 성별  ex: 66세/남
        /// </summary>
		public string AGE { get; set; }

        /// <summary>
        /// 등록번호
        /// </summary>
		public string PTNO { get; set; }

        /// <summary>
        /// 주민등록번호
        /// </summary>
		public string JUMIN { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
		public string RID { get; set; }


        public HIC_KIOSK()
        {
        }
    }
}
