namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class EXAM_MASTER_SUB : BaseDto
    {
        
        /// <summary>
        /// 마스터 코드
        /// </summary>
		public string MASTERCODE { get; set; } 

        /// <summary>
        /// 각항목 구분자(아래 참고사항 확인)
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// SORT번호(001-999)
        /// </summary>
		public long SORT { get; set; } 

        /// <summary>
        /// 각항목 코드,Comment
        /// </summary>
		public string NORMAL { get; set; } 

        /// <summary>
        /// 성별(M:남, F:여)
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 나이(From)
        /// </summary>
		public long AGEFROM { get; set; } 

        /// <summary>
        /// 나이(To)
        /// </summary>
		public long AGETO { get; set; } 

        /// <summary>
        /// Reference Value(From)
        /// </summary>
		public double REFVALFROM { get; set; } 

        /// <summary>
        /// Reference Value(To)
        /// </summary>
		public double REFVALTO { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public DateTime? EXPIREDATE { get; set; } 

        
        /// <summary>
        /// 검사코드 SUB 마스타
        /// </summary>
        public EXAM_MASTER_SUB()
        {
        }
    }
}
