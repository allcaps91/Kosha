
namespace ComHpcLibB.Dto
{
    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    /// 
    using ComBase.Mvc;
    using System;


    public class HEA_RESV_SET_TIME : BaseDto
    {

        /// <summary>
        /// 
        /// </summary>
        public string SDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string STIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long INWON { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long GAINWON { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ENTTIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long ENTSABUN { get; set; }

        public string RID { get; set; }

    }
}
