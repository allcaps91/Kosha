namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;

    public class BTN_PANSET :BaseDto
    {
        public int INDEX { get; set; }
        public string JONG { get; set; }
        public string NAME { get; set; }
        public long WRTNO { get; set; }
        public BTN_PANSET()
        {

        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class PAN_PATLIST_SEARCH : BaseDto
    {
        /// <summary>
        /// 조회 구분 (좌측 탭) (HIC, SPC, REC, CAN, SCHOOL, ETC, TOTAL)
        /// </summary>
		public string JOBGUBUN { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
		public string FRDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string TODATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// 문진Viewer 실행여부
        /// </summary>
		public bool MUNJINVIEW { get; set; }

        /// <summary>
        /// (공백) : 전체 || 0 : 상담대기 || 1 : 미판정 || 2 : 판정
        /// </summary>
		public string JOB { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long SABUN { get; set; }        

        /// <summary>
        /// 
        /// </summary>
		public long IDNUMBER { get; set; }

        /// <summary>
        /// 로그인 한 판정의사 LICENCE
        /// </summary>
		public long LICENSE { get; set; }

        /// <summary>
        /// 조회 할 판정의사 LICENCE 
        /// </summary>
		public string LICENSE_VIEW { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SORT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PANJENGDRNO { get; set; }

        

        /// <summary>
        /// 
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string CHUL { get; set; }

        public string GBAUTOPAN { get; set; }

        public string B02_PANJENG_DRNO { get; set; }

        public long WRTNO { get; set; }

        public PAN_PATLIST_SEARCH()
        {
        }
    }
}
