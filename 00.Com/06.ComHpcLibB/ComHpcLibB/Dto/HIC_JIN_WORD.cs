
namespace ComHpcLibB.Dto
{

    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HIC_JIN_WORD : BaseDto
    {

        /// <summary>
        /// 
        /// </summary>
        public string JDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WORD1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WORD2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ENTDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long ENTSABUN { get; set; }

    }
}
