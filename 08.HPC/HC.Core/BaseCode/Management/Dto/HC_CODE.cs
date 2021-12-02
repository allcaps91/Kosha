namespace HC.Core.BaseCode.Management.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HC_CODE : BaseDto
    {
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [MTSNotEmpty]
		public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MTSNotEmpty]
        public string CodeName { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GroupCode { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GroupCodeName { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SortSeq { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string Extend1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string Extend2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string IsActive { get; set; }

        /// <summary>
        /// 
        /// </summary>
     
        public string Description { get; set; } 

        public DateTime Modified { get; set; }

        public string ModifiedUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public HC_CODE()
        {
            IsActive = "Y";
        }
    }
}
