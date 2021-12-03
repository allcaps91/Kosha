namespace HC.OSHA.Visit.Schedule.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 방문내역
    /// </summary>
    public class HC_OSHA_VISIT : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        /// <summary>
        /// 일정 아이디
        /// </summary>
		public long SCHEDULE_ID { get; set; } 

        /// <summary>
        /// 방문일
        /// </summary>
		public DateTime? VISITDATETIME { get; set; } 

        /// <summary>
        /// 방문구분
        /// </summary>
		public string VISITTYPE { get; set; } 

        /// <summary>
        /// 방문시작시간
        /// </summary>
		public string STARTTIME { get; set; } 

        /// <summary>
        /// 방문종료시간
        /// </summary>
		public string ENDTIME { get; set; } 

        /// <summary>
        /// 소요시간,분 문자열
        /// </summary>
        public string TakeHourAndMinute { get; set; }
        /// <summary>
        /// 소요시간
        /// </summary>
        public long TAKEHOUR { get; set; } 

        /// <summary>
        /// 소요분
        /// </summary>
		public long TAKEMINUTE { get; set; } 

        /// <summary>
        /// 방문자 이름
        /// </summary>
		public string VISITUSERNAME { get; set; } 

        /// <summary>
        /// 방문자 아이디
        /// </summary>
		public string VISITUSER { get; set; } 

        /// <summary>
        /// 방문의사 이름
        /// </summary>
		public string VISITDOCTORNAME { get; set; } 

        /// <summary>
        /// 방문의사아이디
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
