namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HC_SITE_VIEW : BaseDto, ISiteModel
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NAME_ALIAS { get; set; } 

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
        /// ������ȣ
        /// </summary>
		public string BizUPJONG { get; set; }
        /// <summary>
        /// �����ѱ�
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
		public string INSUARANCE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string INDUSTRIAL { get; set; } 

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
        /// �ֿ����ǰ
        /// </summary>        
        public string JEPUMLIST { get; set; }

        /// <summary>
        /// �����ڵ��
        /// </summary>        
        public string BIZUPJONGNAME { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public HC_SITE_VIEW()
        {
        }
    }
}
