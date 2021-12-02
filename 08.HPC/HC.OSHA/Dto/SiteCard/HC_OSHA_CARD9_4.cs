namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 9. ���輺�� ������
    /// </summary>
    public class HC_OSHA_CARD9_4 : BaseDto
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
        /// ������ ����
        /// </summary>
		public string STARTDATE { get; set; }

        public string PERIOD { get; set; }

        /// <summary>
        /// ������ ����
        /// </summary>
        public string ENDDATE { get; set; } 

        /// <summary>
        /// ������ �� ��������
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// �ֿ������ó���
        /// </summary>
		public string CONTENT { get; set; } 

        /// <summary>
        /// ��������
        /// </summary>
		public string REMARK { get; set; }

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
