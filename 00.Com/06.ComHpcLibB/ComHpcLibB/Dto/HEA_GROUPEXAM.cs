namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_GROUPEXAM : BaseDto
    {
        
        /// <summary>
        /// 금액코드
        /// </summary>
		public string GROUPCODE { get; set; } 

        /// <summary>
        /// 검사코드
        /// </summary>
		public string EXCODE { get; set; } 

        /// <summary>
        /// 수가구분(아래참조) : 사용않함
        /// </summary>
		public string SUGAGBN { get; set; } 

        /// <summary>
        /// 순위
        /// </summary>
		public string SEQNO { get; set; } 

        /// <summary>
        /// 최종입력사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 최종입력일자
        /// </summary>
		public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 최종입력사번
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 종합검진 그룹코드별 검사항목
        /// </summary>
        public HEA_GROUPEXAM()
        {
        }
    }
}
