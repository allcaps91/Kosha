namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class XRAY_PACS_EXAM: BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string EXAMCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EXAMNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SHORTNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MODALITY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SECTCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EXAMNAME2 { get; set; }

    }
}
