namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class ETC_ALIMTALK : BaseDto
    {
        /// <summary>
        /// 작업일자
        /// </summary>
        public DateTime? JOBDATE { get; set; }

        /// <summary>
        /// 전송상태
        /// </summary>
        public string SENDFLAG { get; set; }

        /// <summary>
        /// 전송타입 (A:알림톡, F:친구톡, S:SMS, L:LMS )
        /// </summary>
        public string SENDTYPE { get; set; }

        /// <summary>
        /// 템플릿 코드
        /// </summary>
        public string TEMPCD { get; set; }

        /// <summary>
        /// 작업자 사번
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 작업일시
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 중계서버로 Data 전송일시
        /// </summary>
        public DateTime? REQUEST_DATE { get; set; }

        /// <summary>
        /// 중계서버 응답일시
        /// </summary>
        public DateTime? RESPONSE_DATE { get; set; }

        /// <summary>
        /// 중계서버 응답코드
        /// </summary>
        public string RESPONSE_CODE { get; set; }

        /// <summary>
        /// 메세지 전송 결과유형
        /// </summary>
        public string REPORT_TYPE { get; set; }

        /// <summary>
        /// 메세지 전송 결과 수신일시
        /// </summary>
        public DateTime? REPORT_DATE { get; set; }

        /// <summary>
        /// 메세지 전송 결과코드
        /// </summary>
        public string REPORT_CODE { get; set; }

        /// <summary>
        /// 메세지 도착(수신)시간
        /// </summary>
        public DateTime? ARRIVAL_DATE { get; set; }

        /// <summary>
        /// 등록번호
        /// </summary>
        public string PANO { get; set; }

        /// <summary>
        /// 수신자명
        /// </summary>
        public string SNAME { get; set; }

        /// <summary>
        /// 핸드폰번호
        /// </summary>
        public string HPHONE { get; set; }

        /// <summary>
        /// 회사명
        /// </summary>
        public string LTDNAME { get; set; }

        /// <summary>
        /// 진료과
        /// </summary>
        public string DEPTNAME { get; set; }

        /// <summary>
        /// 의사성명
        /// </summary>
        public string DRNAME { get; set; }

        /// <summary>
        /// 메세지전송 예약시간
        /// </summary>
        public string RDATE { get; set; }

        /// <summary>
        /// 회신번호
        /// </summary>
        public string RETTEL { get; set; }

        /// <summary>
        /// 중계서버 요청 ID
        /// </summary>
        public string REQUID { get; set; }

        /// <summary>
        /// 원내에서 발급하는 발송 UNIQUE ID (전화번호(11)+날짜시분초(14))
        /// </summary>
        public string SENDUID { get; set; }

        /// <summary>
        /// 웹 전송유무
        /// </summary>
        public string WEBSEND { get; set; }

        /// <summary>
        /// 건진/외래/기타 접수번호 (발신여부조회시 Key)
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 알림톡 부달시 SMS 대체발송 결과코드
        /// </summary>
        public string RESEND_REPORT_CODE { get; set; }

        /// <summary>
        /// 검진종류명
        /// </summary>
        public string GJNAME { get; set; }

        /// <summary>
        /// 즉시전송 접수건 삭제일자
        /// </summary>
        public DateTime? DELDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string QUESTTITLE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string QUESTMSG { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string QUESTLINK { get; set; }


        public ETC_ALIMTALK()
        {
        }
    }
}

