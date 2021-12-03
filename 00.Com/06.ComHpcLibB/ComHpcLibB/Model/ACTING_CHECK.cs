namespace ComHpcLibB.Model
{
    using ComBase;
    using ComBase.Mvc;
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class ACTING_CHECK : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string HAROOM { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string HEAPART { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ENTPART { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EXCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RESULT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ACTIVE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string HNAME { get; set; }

        public string CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PART { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string HCROOM { get; set; }

        public ACTING_CHECK()
        {
        }
    }
}
