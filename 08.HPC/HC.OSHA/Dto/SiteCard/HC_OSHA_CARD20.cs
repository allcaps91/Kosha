namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// ����� ������
    /// </summary>
    public class HC_OSHA_CARD20 : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SITE_ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long ESTIMATE_ID { get; set; }

        public string YEAR { get; set; }

        /// <summary>
        /// ���Ϲݱ�
        /// </summary>
        public string QUARTER { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string STATISFACTION { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NAME { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string SITESIGN { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string ISSIGN { get; set; }

        public DateTime? MODIFIED { get; set; }
        public string MODIFIEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }
       
    }
}
