namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class BAS_BCODE_ECG : BaseDto
    {
        
        /// <summary>
        /// 코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 결과
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 순서
        /// </summary>
		public long SORT { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
		public string ROWID { get; set; }        


        /// <summary>
        /// ecg 결과코드집
        /// </summary>
        public BAS_BCODE_ECG()
        {
        }
    }
}
