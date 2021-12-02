namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 9. ���輺�� ���������� ����
    /// </summary>
    public class HC_OSHA_CARD9_5 : BaseDto
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
        /// ������û��
        /// </summary>
		public string REQUESTDATE { get; set; } 

        /// <summary>
        /// ����������
        /// </summary>
		public string APPROVEDATE { get; set; } 

        /// <summary>
        /// ��������
        /// </summary>
        [MTSNotEmpty]
		public string ISAPPROVE { get; set; }

        public string ISAPPROVE_TEXT { get; set; }

        /// <summary>
        /// ������ȿ�Ⱑ
        /// </summary>
        public string STARTDATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string ENDDATE { get; set; }

        public string PERIOD { get; set; }
        /// <summary>
        /// ��Ÿ
        /// </summary>
        public string REMARK { get; set; }


       
        public DateTime? MODIFIED { get; set; }
        
		public string MODIFIEDUSER { get; set; }

		public DateTime? CREATED { get; set; }

		public string CREATEDUSER { get; set; }
    }
}
