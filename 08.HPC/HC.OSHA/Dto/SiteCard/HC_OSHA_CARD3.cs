namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// ����� ����ī�� 3. ����å���� �� ������Ȳ
    /// </summary>
    public class HC_OSHA_CARD3 : BaseDto
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

        /// <summary>
        /// å���ڱ���
        /// </summary>
        public string TASKTYPE { get; set; }

        public string YEAR { get; set; }

        /// <summary>
        /// ���� �Ǵ� ��������
        /// </summary>
        public string NAME { get; set; } 

        /// <summary>
        /// ��������
        /// </summary>
		public string DELACEDATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CERT { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string EDUCATION { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CARRER { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string EDUATIONNAME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string EDUCATIONSTARTDATE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string EDUCATIONENDDATE { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD3()
        {
        }
    }
}
