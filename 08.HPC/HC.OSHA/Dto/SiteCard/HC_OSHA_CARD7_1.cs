namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    public class HC_OSHA_CARD7_1 : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long SITE_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long ESTIMATE_ID { get; set; }

        public string YEAR { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string MEETDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ISREGULAR { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CONTENT { get; set; }

    
    }
}
