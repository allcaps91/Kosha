namespace HC.OSHA.Visit.Management.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// ���Ǳ�������
    /// </summary>
    public class HC_OSHA_VISIT_EDU : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        /// <summary>
        /// �������̵�
        /// </summary>
		public long SITE_ID { get; set; } 

        /// <summary>
        /// ������
        /// </summary>
		public string EDUDATE { get; set; } 

        /// <summary>
        /// ����
        /// </summary>
		public string EDUTYPE { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public string TARGET { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string TITLE { get; set; } 

        /// <summary>
        /// �������
        /// </summary>
		public string LOCATION { get; set; } 

        /// <summary>
        /// �ǽ������̵�
        /// </summary>
		public string EDUUSERID { get; set; } 

        /// <summary>
        /// �ǽ��ڸ�
        /// </summary>
		public string EDUUSERNAME { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_VISIT_EDU()
        {
        }
    }
}
