namespace ComHpcLibB.Dto
{

    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HIC_PRIVACY_ACCEPT_NEW : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string GJYEAR { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FILENAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ENTDATE { get; set; }
    }
}
