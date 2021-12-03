namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 9. ���輺�� ������
    /// </summary>
    public class HC_OSHA_CARD9_2 : BaseDto
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
        /// ����۾� �Ǵ� ����
        /// </summary>
		public string WORKNAME { get; set; }
        public string PERIOD { get; set; }
        
        /// <summary>
        /// �ǽñⰣ
        /// </summary>
		public string STARTDATE { get; set; } 

        /// <summary>
        /// �ǽñⰣ
        /// </summary>
		public string ENDDATE { get; set; } 

        /// <summary>
        /// �ǽð��
        /// </summary>
		public string CONTENT { get; set; } 

        /// <summary>
        /// �ǽ���1 ��å
        /// </summary>
		public string GRADE1 { get; set; } 

        /// <summary>
        /// �ǽ���1 ����
        /// </summary>
		public string NAME1 { get; set; } 

        /// <summary>
        /// �ǽ���2 ��å
        /// </summary>
		public string GRADE2 { get; set; } 

        /// <summary>
        /// �ǽ���2 ����
        /// </summary>
		public string NAME2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? MODIFIED { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string MODIFIEDUSER { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? CREATED { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string CREATEDUSER { get; set; }
    }
}
