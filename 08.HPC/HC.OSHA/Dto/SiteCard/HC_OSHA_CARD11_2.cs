namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// �ǰ�������� �缺����
    /// </summary>
    public class HC_OSHA_CARD11_2 : BaseDto
    {
        
     
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
        /// �̼��ڸ�
        /// </summary>
        public string NAME { get; set; } 

        /// <summary>
        /// ��������
        /// </summary>
		public string STARTDATE { get; set; } 

        /// <summary>
        /// ���� ����
        /// </summary>
		public string ENDDATE { get; set; } 

        /// <summary>
        /// �������� �� ����
        /// </summary>
		public string CONTENT { get; set; }

        public string PERIOD { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD11_2()
        {
        }
    }
}
