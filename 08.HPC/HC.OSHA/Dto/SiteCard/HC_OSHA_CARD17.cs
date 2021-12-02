namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// ����� �������Ǳ���
    /// </summary>
    public class HC_OSHA_CARD17 : BaseDto
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
        /// ����
        /// </summary>
        [MTSNotEmpty]
		public string EDUDATE { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [MTSNotEmpty]
        public string EDUTYPE { get; set; } 

        /// <summary>
        /// ���
        /// </summary>
		public string EDUPLACE { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        [MTSNotEmpty]
        public string EDUUSAGE { get; set; } 

        /// <summary>
        /// �ǽ���
        /// </summary>
		public string EDUNAME { get; set; } 

        /// <summary>
        /// ����ο�
        /// </summary>
		public string TARGETCOUNT { get; set; } 

        /// <summary>
        /// �����ο�
        /// </summary>
		public string ACTCOUNT { get; set; } 

        /// <summary>
        /// ��������
        /// </summary>
		public string CONTENT { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD17()
        {
        }
    }
}
