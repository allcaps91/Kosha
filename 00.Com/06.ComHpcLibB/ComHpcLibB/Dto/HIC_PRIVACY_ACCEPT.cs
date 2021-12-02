namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HIC_PRIVACY_ACCEPT : BaseDto
    {
        /// <summary>
        /// 검진년도
        /// </summary>
        public string GJYEAR { get; set; }

        /// <summary>
        /// 등록번호
        /// </summary>
        public string PTNO { get; set; }

        /// <summary>
        /// 수검자명
        /// </summary>
        public string SNAME { get; set; }

        /// <summary>
        /// 파일명
        /// </summary>
        public string FILENAME { get; set; }

        /// <summary>
        /// 입력일자
        /// </summary>
        public string ENTDATE { get; set; }
    }
}
