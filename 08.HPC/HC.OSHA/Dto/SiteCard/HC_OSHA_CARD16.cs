namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// ���ع���
    /// </summary>
    public class HC_OSHA_CARD16 : BaseDto
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
        /// ��ް���
        /// </summary>
        [MTSNotEmpty]
		public string TASKNAME { get; set; }

        /// <summary>
        /// �з�
        /// </summary>
        /// 
        [MTSNotEmpty]
        public string TASKTYPE { get; set; } 

        /// <summary>
        /// ������
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// ���䵵
        /// </summary>
		public string USAGE { get; set; } 

        /// <summary>
        /// �����
        /// </summary>
		public string QTY { get; set; } 

        /// <summary>
        /// ��������
        /// </summary>
		public string EXPOSURE { get; set; } 

        /// <summary>
        /// �������
        /// </summary>
		public string COSENESS { get; set; } 

        /// <summary>
        /// ��ȣ������
        /// </summary>
		public string PROTECTION { get; set; } 

        /// <summary>
        /// �Խÿ���
        /// </summary>
		public string ISMSDSPUBLISH { get; set; } 

        /// <summary>
        /// ���ǥ�� ����
        /// </summary>
		public string ISALET { get; set; } 

        /// <summary>
        /// �����ǽÿ���
        /// </summary>
		public string ISMSDSEDUCATION { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD16()
        {
        }
    }
}
