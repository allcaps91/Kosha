namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// �ǰ������
    /// </summary>
    public class HC_OSHA_CARD11_1 : BaseDto
    {
        
        /// <summary>
        /// �ǰ������ ����
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
        /// ��������
        /// </summary>
        public string PUBLISHDATE { get; set; } 

        /// <summary>
        /// ��������
        /// </summary>
		public string ACTDATE { get; set; } 

        /// <summary>
        /// �ü������Ȳ
        /// </summary>
		public string STATUS { get; set; } 

        /// <summary>
        /// ����
        /// </summary>
		public string CONTENT { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD11_1()
        {
        }
    }
}
