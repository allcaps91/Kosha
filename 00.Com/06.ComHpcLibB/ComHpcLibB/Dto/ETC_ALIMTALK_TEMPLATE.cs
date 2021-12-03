namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>   
    public class ETC_ALIMTALK_TEMPLATE : BaseDto
    {
        /// <summary>
        /// 템플릿 코드 (알림톡 승인후 자동생성)
        /// </summary>
        public string TEMPCD { get; set; }

        /// <summary>
        /// 템플릿 제목
        /// </summary>
        public string TITLE { get; set; }

        /// <summary>
        /// 알림톡 내용
        /// </summary>
        public string MESSAGE { get; set; }

        /// <summary>
        /// 삭제일자
        /// </summary>
        public DateTime? DELDATE { get; set; }

        /// <summary>
        /// 등록일자
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 등록사번
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 템플릿 신청상태 (0: 등록, 1: 승인신청, 2: 승인완료)
        /// </summary>
        public string GBSTS { get; set; }

        /// <summary>
        /// 비고
        /// </summary>
        public string BIGO { get; set; }

        /// <summary>
        /// 전송시점 (0: 즉시, 1: 당일, 2: 지정)
        /// </summary>
        public string SENDGAP { get; set; }

        /// <summary>
        /// 전송 희망 지정일수
        /// </summary>
        public long SENDDAY { get; set; }

        /// <summary>
        /// 전송 희망 지정일수 전후 (0: 전, 1: 후)
        /// </summary>
        public string SENDBF { get; set; }

        /// <summary>
        /// 전송 희망 시각
        /// </summary>
        public string SENDTIME { get; set; }

        /// <summary>
        /// 전송기준 상세설명
        /// </summary>
        public string SENDSTD { get; set; }

        /// <summary>
        /// 카카오톡 전송 실패시 SMS 전송문자
        /// </summary>
        public string SENDSMS { get; set; }

        public ETC_ALIMTALK_TEMPLATE()
        {
        }

    }
}
