namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;


    /// <summary>
    /// 
    /// </summary>
    public class OCS_SUBCODE : BaseDto
    {
        
        /// <summary>
        /// 오더코드
        /// </summary>
		public string ORDERCODE { get; set; } 

        /// <summary>
        /// 일련번호
        /// </summary>
		public long SEQNO { get; set; } 

        /// <summary>
        /// SUB명칭
        /// </summary>
		public string SUBNAME { get; set; } 

        /// <summary>
        /// 수가코드
        /// </summary>
		public string SUCODE { get; set; } 

        /// <summary>
        /// 검사코드
        /// </summary>
		public string ITEMCD { get; set; } 

        /// <summary>
        /// 삭제일자
        /// </summary>
		public string DELDATE { get; set; } 

        /// <summary>
        /// 팍스전송명칭 사용안함
        /// </summary>
		public string PACSNAME { get; set; } 

        /// <summary>
        /// 팍스검사코드
        /// </summary>
		public string PACS_CODE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CORDERCODE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CSUCODE { get; set; } 

        
        /// <summary>
        /// OCS 오더코드 SUB
        /// </summary>
        public OCS_SUBCODE()
        {
        }
    }
}
