namespace HC.OSHA.Site.Management.Model
{
    using ComBase.Mvc;
    using HC.Core.Site.Model;
    using System;


    /// <summary>
    /// 보건관리 사업장
    /// </summary>
    public class HC_OSHA_SITE_MODEL : BaseDto, ISiteModel
    {
        
        /// <summary>
        /// 사업자아이디
        /// </summary>
		public long ID { get; set; }

        
        /// <summary>
        /// 사업자명
        /// </summary>
		public string NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string TEL { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string FAX { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EMAIL { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ADDRESS { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BIZNUMBER { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BIZTYPE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BIZJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BIZKIHO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? BIZCREATEDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string INSURANCE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string INDUSTRIALNUMBER { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BIZJIDOWON { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string LABOR { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string CEONAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ISACTIVE { get; set; }

        
        public HC_OSHA_SITE_MODEL()
        {
        }
    }
}
