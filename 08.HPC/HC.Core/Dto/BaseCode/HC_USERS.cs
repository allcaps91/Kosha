namespace HC.Core.Dto
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

        /// <summary>
        /// 자격번호
        /// </summary>
        public string CERTNO { get; set; }

        public string PASSHASH256 { get; set; }
        public string JUMIN3 { get; set; }
        public IsActive IsActive { get; set; }
        public IsDeleted ISDELETED { get; set; }
        public DateTime? MODIFIED { get; set; }
        public string MODIFIEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }
        public string SEQ_WORD { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public HC_USER()
        {
        }
    }
}
