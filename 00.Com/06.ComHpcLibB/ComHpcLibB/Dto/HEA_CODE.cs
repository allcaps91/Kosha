namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_CODE : BaseDto
    {
        
        /// <summary>
        /// 코드구분
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 코드명칭
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 삭제여부(삭제:Y)
        /// </summary>
		public string GBDEL { get; set; } 

        /// <summary>
        /// 구분1
        /// </summary>
		public string GUBUN1 { get; set; } 

        /// <summary>
        /// 구분2
        /// </summary>
		public string GUBUN2 { get; set; } 

        /// <summary>
        /// 구분3
        /// </summary>
		public string GUBUN3 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public decimal SORT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RID { get; set; }

        public string ORDYN { get; set; }



        /// <summary>
        /// 종합검진 기초코드
        /// </summary>
        public HEA_CODE()
        {
        }
    }
}
