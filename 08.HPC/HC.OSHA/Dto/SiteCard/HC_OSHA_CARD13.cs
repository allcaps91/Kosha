namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// ��������ī�� 13. ��ȣ��
    /// </summary>
    public class HC_OSHA_CARD13 : BaseDto
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
        /// ��ȣ����
        /// </summary>
        public string NAME { get; set; } 

        /// <summary>
        /// ���޴�� �۾���
        /// </summary>
		public string TARGET { get; set; } 

        /// <summary>
        /// �ٷ��ڼ�
        /// </summary>
		public string WORKERCOUNT { get; set; } 

        /// <summary>
        /// ���޼���
        /// </summary>
		public string TARGETCOUNT { get; set; } 

        /// <summary>
        /// �����հݹ�ȣ
        /// </summary>
		public string PASSNUMBER { get; set; } 

        /// <summary>
        /// ��������
        /// </summary>
		public string REMARK { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD13()
        {
        }
    }
}
