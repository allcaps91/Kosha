namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class ETC_ALIMTALK_RESULT : BaseDto
    {
        /// <summary>
        /// 1: 중계서버 응답값,  2: 메세지 전송 결과값
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// 결과코드
        /// </summary>
        public string CODE { get; set; }

        /// <summary>
        /// 메시지
        /// </summary>
        public string MESSAGE { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public string BIGO { get; set; }


        public ETC_ALIMTALK_RESULT()
        {
        }
    }
}
