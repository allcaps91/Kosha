namespace HC.OSHA.Site.Management.Model
{
    using ComBase.Mvc;
    using HC.Core.Site.Model;
    using System;


    /// <summary>
    /// ���ǰ��� �����
    /// </summary>
    public class HC_OSHA_SITE_MODEL : BaseDto, ISiteModel
    {
        
        /// <summary>
        /// ����ھ��̵�
        /// </summary>
		public long ID { get; set; }

        
        /// <summary>
        /// ����ڸ�
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
