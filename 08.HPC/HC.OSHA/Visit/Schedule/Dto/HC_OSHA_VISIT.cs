namespace HC.OSHA.Visit.Schedule.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// �湮����
    /// </summary>
    public class HC_OSHA_VISIT : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        /// <summary>
        /// ���� ���̵�
        /// </summary>
		public long SCHEDULE_ID { get; set; } 

        /// <summary>
        /// �湮��
        /// </summary>
		public DateTime? VISITDATETIME { get; set; } 

        /// <summary>
        /// �湮����
        /// </summary>
		public string VISITTYPE { get; set; } 

        /// <summary>
        /// �湮���۽ð�
        /// </summary>
		public string STARTTIME { get; set; } 

        /// <summary>
        /// �湮����ð�
        /// </summary>
		public string ENDTIME { get; set; } 

        /// <summary>
        /// �ҿ�ð�,�� ���ڿ�
        /// </summary>
        public string TakeHourAndMinute { get; set; }
        /// <summary>
        /// �ҿ�ð�
        /// </summary>
        public long TAKEHOUR { get; set; } 

        /// <summary>
        /// �ҿ��
        /// </summary>
		public long TAKEMINUTE { get; set; } 

        /// <summary>
        /// �湮�� �̸�
        /// </summary>
		public string VISITUSERNAME { get; set; } 

        /// <summary>
        /// �湮�� ���̵�
        /// </summary>
		public string VISITUSER { get; set; } 

        /// <summary>
        /// �湮�ǻ� �̸�
        /// </summary>
		public string VISITDOCTORNAME { get; set; } 

        /// <summary>
        /// �湮�ǻ���̵�
        /// </summary>
		public string VISITDOCTOR { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string ISFEE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string ISKUKGO { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string ISDELETED { get; set; }

        public DateTime? MODIFIED { get; set; }
        public string MODIFIEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_VISIT()
        {
        }
    }
}
