namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// ��������ī�� 4.��������(1)
    /// </summary>
    public class HC_OSHA_CARD4_1 : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long ESTIMATE_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MTSNotEmpty]
        public string TASKDIAGRAM { get; set; }
        public string YEAR { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD4_1()
        {
        }
    }
}
