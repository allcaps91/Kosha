namespace HC.Core.BaseCode.Management.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Enums;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HC_USER : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string UserId { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string Name { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string Role { get; set; }

        public string Dept { get; set; }
        public IsActive IsActive { get; set; }
        public IsDeleted ISDELETED { get; set; }
        public DateTime? MODIFIED { get; set; }
        public string MODIFIEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HC_USER()
        {
        }
    }
}
