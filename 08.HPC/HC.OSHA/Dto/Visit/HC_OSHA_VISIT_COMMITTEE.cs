namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// ���������������ȸ
    /// </summary>
    public class HC_OSHA_VISIT_COMMITTEE : BaseDto
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
        /// ��������
        /// </summary>
		public string REGDATE { get; set; } 

        /// <summary>
        /// ��������
        /// </summary>
		public string METTINGTYPE { get; set; } 

        /// <summary>
        /// ȸ������
        /// </summary>
		public string MEETINGKIND { get; set; } 

        /// <summary>
        /// ����������
        /// </summary>
		public string MEETINGUSER { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_VISIT_COMMITTEE()
        {
        }
    }
}
