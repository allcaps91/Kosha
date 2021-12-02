namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    using System.Drawing;


    /// <summary>
    /// 
    /// </summary>
    public class HC_SITE_PRODUCT : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        [MTSZeroAttribute(Message = "사업장이 없습니다")]
		public long SITE_ID { get; set; }


        /// <summary>
        /// 제품명
        /// </summary>
        [MTSNotEmpty]
        public string PRODUCTNAME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string PROCESS { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string USAGE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string MANUFACTURER { get; set; } 

        /// <summary>
        /// 월취급량
        /// </summary>
		public string MONTHLYAMOUNT { get; set; }
        /// <summary>
        /// 단위
        /// </summary>
        public string UNIT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string REVISIONDATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GHSPICTURE { get; set; } 


        public Bitmap GhsImage { get; set; }

        public DateTime? MODIFIED { get; set; }
        public string MODIFEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HC_SITE_PRODUCT()
        {
        }
    }
}
