namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class ETC_SMS : BaseDto
    {
        
        /// <summary>
        /// 전송 희망일자 및 시각(YYYY-MM-DD HH24:MI)
        /// </summary>
		public DateTime? JOBDATE { get; set; } 

        /// <summary>
        /// 등록번호
        /// </summary>
		public string PANO { get; set; } 

        /// <summary>
        /// 성명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 휴대폰번호
        /// </summary>
		public string HPHONE { get; set; } 

        /// <summary>
        /// 통보구분(아래참조)
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 진료과
        /// </summary>
		public string DEPTCODE { get; set; } 

        /// <summary>
        /// 의사코드
        /// </summary>
		public string DRCODE { get; set; } 

        /// <summary>
        /// 예약일시(YYYY-MM-DD HH24:MI)
        /// </summary>
		public DateTime? RTIME { get; set; } 

        /// <summary>
        /// 회신번호
        /// </summary>
		public string RETTEL { get; set; } 

        /// <summary>
        /// 전송시각
        /// </summary>
		public DateTime? SENDTIME { get; set; } 

        /// <summary>
        /// 전송메세지
        /// </summary>
		public string SENDMSG { get; set; } 

        /// <summary>
        /// 등록자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 등록일자 및 시각
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// JobDate 일자 백업
        /// </summary>
		public DateTime? SEND_CNT { get; set; } 

        /// <summary>
        /// 1차 전송시간 백업
        /// </summary>
		public DateTime? SENDTIMEBACK { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string SENDMSGBACK { get; set; } 

        /// <summary>
        /// 참고사항
        /// </summary>
		public string BIGO { get; set; } 

        /// <summary>
        /// 전송상태
        /// </summary>
		public string STATE { get; set; } 

        /// <summary>
        /// PUSH 구분
        /// </summary>
		public string GBPUSH { get; set; }

        /// <summary>
        /// 전송여부
        /// </summary>
        public string ISSEND { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
		public string RID { get; set; }

        /// <summary>
        /// 휴대폰 문자메세지 전송
        /// </summary>
        public ETC_SMS()
        {
        }
    }
}
