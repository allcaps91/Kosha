namespace HC.OSHA.Visit.Schedule.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 방문일정
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
        [MTSZero(Message = " 사업장을 선택하세요 ")]
		public long SITE_ID { get; set; }
        public long ESTIMATE_ID { get; set; }
        public string SITE_NAME { get; set; }
        /// <summary>
        /// 달력 이벤트 시작일시
        /// </summary>
        
        public DateTime? EVENTSTARTDATETIME { get; set; }

        /// <summary>
        /// 달력 이벤트 종료일시
        /// </summary>
        public DateTime? EVENTENDDATETIME { get; set; } 
        /// <summary>
        /// 방문예정일
        /// </summary>
        public string VISITRESERVEDATE { get; set; }
        /// <summary>
        /// 방문시작시간
        /// </summary>
        [MTSNotEmpty]
        public string VISITSTARTTIME { get; set; }

        /// <summary>
        /// 방문종료시간
        /// </summary>
        [MTSNotEmpty]
        public string VISITENDTIME { get; set; } 

        /// <summary>
        /// 출발시간
        /// </summary>
		public string DEPARTUREDATETIME { get; set; } 

        /// <summary>
        /// 도착시간
        /// </summary>
		public string ARRIVALTIME { get; set; } 

        /// <summary>
        /// 방문자 이름
        /// </summary>
		public string VISITUSERNAME { get; set; } 

        /// <summary>
        /// 방문자 아이디
        /// </summary>
		public string VISITUSERID { get; set; } 

        /// <summary>
        /// 동행자 이름
        /// </summary>
		public string VISITMANAGERNAME { get; set; } 

        /// <summary>
        /// 동행자 아이디
        /// </summary>
		public string VISITMANAGERID { get; set; }
        /// <summary>
        /// 인원수
        /// </summary>
        public int WORKERCOUNT { get; set; }
        
        /// <summary>
        /// 참고사항
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 내부공문출력일시
        /// </summary>
		public DateTime? INDOCPRINTDATETIME { get; set; } 

        /// <summary>
        /// 외부공문출력일시
        /// </summary>
		public DateTime? OUTDOCPRINTDATETIME { get; set; } 

        /// <summary>
        /// 메일전송일시
        /// </summary>
		public DateTime? SENDMAILDATETIME { get; set; } 

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
        public HC_OSHA_SCHEDULE()
        {
        }
    }
}
