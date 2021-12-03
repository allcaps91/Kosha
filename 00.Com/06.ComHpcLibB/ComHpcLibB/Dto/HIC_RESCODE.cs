namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;


    /// <summary>
    /// 
    /// </summary>    
    public class HIC_RESCODE : BaseDto
    {   
        /// <summary>
        /// 결과값코드
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 결과값
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 비정상여부(비정상:*)
        /// </summary>
		public string GBFLAG { get; set; } 

        /// <summary>
        /// 일반건진 개인표 출력시 마크되는 값
        /// </summary>
		public string MARK { get; set; } 

        /// <summary>
        /// 우선순위
        /// </summary>
		public string RANK { get; set; }
        
        /// <summary>
        /// CODE.NAME(콤보 스타일)
        /// </summary>
		public string CODENAME { get; set; }

        /// <summary>
        /// 검사 결과값 코드
        /// </summary>
        public HIC_RESCODE()
        {
        }
    }
}
