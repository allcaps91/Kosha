namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// ������ �����
    /// </summary>
    public class HC_OSHA_CARD10 : BaseDto
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
        /// �Խ�����
        /// </summary>
        [MTSNotEmpty]
		public string PUBLISHDATE { get; set; } 

        /// <summary>
        /// ��ǥ�ð�
        /// </summary>
		public string GOAL { get; set; } 

        /// <summary>
        /// �޼��ð�
        /// </summary>
		public string COMPLETE { get; set; } 

        /// <summary>
        /// ��ǥ�޼�
        /// </summary>
		public string STATUS { get; set; }

        public DateTime? MODIFIED { get; set; }

        public string MODIFIEDUSER { get; set; }

        public DateTime? CREATED { get; set; }

        public string CREATEDUSER { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD10()
        {
        }
    }
}
