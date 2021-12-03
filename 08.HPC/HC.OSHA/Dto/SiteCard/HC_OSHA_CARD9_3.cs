namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 9. ���輺�� ����
    /// </summary>
    public class HC_OSHA_CARD9_3 : BaseDto
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
        /// ����� ��������
        /// </summary>
		public string SITESTARTDATE { get; set; } 

        /// <summary>
        /// ����� ���� ����
        /// </summary>
		public string SITEENDDATE { get; set; }

        public string SITE_PERIOD { get; set; }
        public string SITE_GRADE_NAME { get; set; }
        /// <summary>
        /// ����� ������ ��å
        /// </summary>
        public string SITEGRADE { get; set; } 

        /// <summary>
        /// ����� ������ ����
        /// </summary>
		public string SITENAME { get; set; } 

        /// <summary>
        /// �򰡴���� ����
        /// </summary>
		public string TESTSTARTDATE { get; set; } 

        /// <summary>
        /// �򰡴���� ���� ����
        /// </summary>
		public string TESTENDDATE { get; set; }

        public string TEST_PERIOD { get; set; }
        public string TEST_GRADE_NAME { get; set; }

        /// <summary>
        /// �򰡴���� ��å
        /// </summary>
		public string TESTGRADE { get; set; } 

        /// <summary>
        /// �򰡴���� �̸�
        /// </summary>
		public string TESTNAME { get; set; }

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
