namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// ���蹰��
    /// </summary>
    public class HC_OSHA_CARD15 : BaseDto
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

        /// <summary>
        /// ��ް���
        /// </summary>
        [MTSNotEmpty]
        public string TASKNAME { get; set; }

        /// <summary>
        /// �з�
        /// </summary>
        [MTSNotEmpty]
        public string TASKTYPE { get; set; } 

        /// <summary>
        /// ������
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// ��޷�
        /// </summary>
		public string QTY { get; set; } 

        /// <summary>
        /// ��ġ��Ȳ
        /// </summary>
		public string STATUS { get; set; } 

        /// <summary>
        /// ��������
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// ��������
        /// </summary>
		public string MANAGESTATUS { get; set; }

        public string YEAR { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD15()
        {
        }
    }
}
