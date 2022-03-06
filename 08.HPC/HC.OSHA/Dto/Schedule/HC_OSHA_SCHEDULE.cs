namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// �湮����
    /// </summary>
    public class HC_OSHA_SCHEDULE : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SITE_ID { get; set; }
        public long ESTIMATE_ID { get; set; }
        public string SITE_NAME { get; set; }
        /// <summary>
        /// �޷� �̺�Ʈ �����Ͻ�
        /// </summary>
        public DateTime? EVENTSTARTDATETIME { get; set; }

        /// <summary>
        /// �޷� �̺�Ʈ �����Ͻ�
        /// </summary>
        public DateTime? EVENTENDDATETIME { get; set; } 
        /// <summary>
        /// �湮������
        /// </summary>
        public DateTime? VISITRESERVEDATE { get; set; }
        /// <summary>
        /// �湮���۽ð�
        /// </summary>
        [MTSNotEmpty]
        public string VISITSTARTTIME { get; set; }

        /// <summary>
        /// �湮�� �̸�
        /// </summary>
        public string VISITUSERNAME { get; set; }

        /// <summary>
        /// �湮���
        /// </summary>
        public string VISITPLACE { get; set; }

        public long VISIT_ID { get; set; }

        /// <summary>
        /// �湮�� ���̵�
        /// </summary>
        [MTSNotEmpty]
        public string VISITUSERID { get; set; } 

        /// <summary>
        /// ������ �̸�
        /// </summary>
		public string VISITMANAGERNAME { get; set; } 

        /// <summary>
        /// ������ ���̵�
        /// </summary>
		public string VISITMANAGERID { get; set; }
        /// <summary>
        /// �ο���
        /// </summary>
        public long WORKERCOUNT { get; set; }
        
        /// <summary>
        /// �������
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// ���ΰ�������Ͻ�
        /// </summary>
		public DateTime? INDOCPRINTDATETIME { get; set; } 

        /// <summary>
        /// �ܺΰ�������Ͻ�
        /// </summary>
		public DateTime? OUTDOCPRINTDATETIME { get; set; } 

        /// <summary>
        /// ���������Ͻ�
        /// </summary>
		public DateTime? SENDMAILDATETIME { get; set; }

        public string GBCHANGE { get; set; }


        /// <summary>
        /// 
        /// </summary>
		public string ISDELETED { get; set; }
        public DateTime? MODIFIED { get; set; }
        public string MODIFIEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }


        public string CHK { get; set; }
        public string COMPLETE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_SCHEDULE()
        {
        }
    }
}
