using ComBase.Mvc;

namespace ComHpcLibB.Model
{
    public class GROUPCODE_EXAM_DISPLAY : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
		public long GWRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GROUPCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GROUPCODENAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EXCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PART { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ENTPART { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string HEAPART { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RESCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long AMT { get; set; }
        public long GAMAMT { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
		public string EXNAME { get; set; }

        /// <summary>
        /// 기타검사 구분 (종검용)
        /// </summary>
		public string ETCEXAM { get; set; }

        /// <summary>
        /// 검사추가여부 (Y / Null or N)
        /// </summary>
		public string GBADD { get; set; }
        public string HANG { get; set; }
        public string GUBUN { get; set; }

        public GROUPCODE_EXAM_DISPLAY()
        {
        }
    }
}
