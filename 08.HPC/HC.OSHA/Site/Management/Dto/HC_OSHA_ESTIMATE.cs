namespace HC.OSHA.Site.Management.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Enums;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// ���ǰ������� ���� ���̺�
    /// </summary>
    public class HC_OSHA_ESTIMATE : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        
        [MTSDecimalMinAttribute(Min = 0)]
        public long OSHA_SITE_ID { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [MTSNotEmpty]
        public string ESTIMATEDATE { get; set; }

        /// <summary>
        /// �����밡��
        /// </summary>
        [MTSNotEmpty]
        public string STARTDATE { get; set; }

        /// <summary>
        /// �ѱٷ��ڼ�
        /// </summary>
        [MTSDecimalMinAttribute(Min = 0)]
        public long WORKERTOTALCOUNT { get; set; }

        /// <summary>
        /// ��ǥ ������
        /// </summary>
        [MTSDecimalMinAttribute(Min = 0)]
        public long OFFICIALFEE { get; set; }

        /// <summary>
        /// ����� ������
        /// </summary>
        [MTSDecimalMinAttribute(Min = 0)]
        public long SITEFEE { get; set; }

        /// <summary>
        /// ���� ������
        /// </summary>
        [MTSDecimalMinAttribute(Min = 0)]
        public long MONTHLYFEE { get; set; }

        /// <summary>
        /// ������������
        /// </summary>
        [MTSNotEmpty]
        public string FEETYPE { get; set; } 

        /// <summary>
        /// �μ��Ͻ�
        /// </summary>
		public DateTime? PRINTDATE { get; set; } 

        /// <summary>
        /// ���������Ͻ�
        /// </summary>
		public DateTime? SENDMAILDATE { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        public string REMARK { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public IsDeleted ISDELETED { get; set; }

        public DateTime? MODIFIED { get; set; }
        public string MODIFIEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_ESTIMATE()
        {
        }
    }
}
