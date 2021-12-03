using ComBase.Mvc;
using ComBase.Mvc.Enums;

namespace ComHpcLibB.Model
{
    public class READ_SUNAP_ITEM : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
		public string ISDELTE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GRPCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GRPNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long AMT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long OLDAMT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBSELF { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JEPBUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SUDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RID { get; set; }

        /// <summary>
        /// 유해물질
        /// </summary>
		public string UCODE { get; set; }

        /// <summary>
        /// 추가여부
        /// </summary>
		public string ADDGBN { get; set; }

        /// <summary>
        /// 암종류
        /// </summary>
		public string GBAM { get; set; }

        /// <summary>
        /// 할인구분
        /// </summary>
		public string GBHALIN { get; set; }

        /// <summary>
        /// 추가여부
        /// </summary>
		public string GBSELECT { get; set; }

        /// <summary>
        /// HANG
        /// </summary>
		public string HANG { get; set; }

        /// <summary>
        /// HANG
        /// </summary>
		public string ENDOCODE { get; set; }

        /// <summary>
        /// HANG
        /// </summary>
		public string BURATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long BONINAMT { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public long LTDAMT { get; set; }

        public READ_SUNAP_ITEM()
        {
        }
    }
}
