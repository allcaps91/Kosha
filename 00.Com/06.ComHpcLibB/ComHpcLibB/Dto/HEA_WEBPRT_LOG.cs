namespace ComHpcLibB
{

    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HEA_WEBPRT_LOG : BaseDto
    {
        /// <summary>
        /// 서버 전송요청 일시
        /// </summary>
        public string REQDATE { get; set; }

        /// <summary>
        /// 종검,일반건진 접수번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 성명
        /// </summary>
        public string SNAME { get; set; }

        /// <summary>
        /// 건진종류
        /// </summary>
        public string GJJONG { get; set; }

        /// <summary>
        /// 직원여부(Y/N)
        /// </summary>
        public string GBJIKWON { get; set; }

        /// <summary>
        /// 등록번호
        /// </summary>
        public string PTNO { get; set; }

        /// <summary>
        /// 검진일자
        /// </summary>
        public string SDATE { get; set; }

        /// <summary>
        /// SMS발송 휴대폰번호
        /// </summary>
        public string HPHONE { get; set; }

        /// <summary>
        /// 이메일
        /// </summary>
        public string EMAIL { get; set; }

        /// <summary>
        /// 결과지서버 전송완료 일시
        /// </summary>
        public string SENDTIME { get; set; }

        /// <summary>
        /// 결과지 파일명
        /// </summary>
        public string FILEPATH { get; set; }

        /// <summary>
        /// 전송요청자 사번
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GUBUN { get; set; }

    }
}
