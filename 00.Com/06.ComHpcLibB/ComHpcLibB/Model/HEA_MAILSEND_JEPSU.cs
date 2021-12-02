namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;

    public class HEA_MAILSEND_JEPSU : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MAILCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string JUSO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SENDDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RECVDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RID { get; set; }

        public HEA_MAILSEND_JEPSU()
        {
        }
    }
}
