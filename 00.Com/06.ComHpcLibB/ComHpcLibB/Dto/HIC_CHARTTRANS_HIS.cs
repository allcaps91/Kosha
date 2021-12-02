namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_CHARTTRANS_HIS : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 인계일자
        /// </summary>
		public DateTime? TRDATE { get; set; } 

        /// <summary>
        /// 성명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 검진종류
        /// </summary>
		public string GJJONG { get; set; } 

        /// <summary>
        /// 인계항목(아래참조)
        /// </summary>
		public string TRLIST { get; set; } 

        /// <summary>
        /// 삭제여부(Y OR NULL)
        /// </summary>
		public string GBDEL { get; set; } 

        /// <summary>
        /// 인계일시
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 인계자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 인수일시
        /// </summary>
		public DateTime? RECVTIME { get; set; } 

        /// <summary>
        /// 인수자 사번
        /// </summary>
		public long RECVSABUN { get; set; } 

        /// <summary>
        /// 백업일자 및 시각
        /// </summary>
		public DateTime? BACKUPTIME { get; set; } 

        /// <summary>
        /// 참고사항
        /// </summary>
		public string REMARK { get; set; } 

        
        /// <summary>
        /// 문진표(차트) 인수인계 변경내역
        /// </summary>
        public HIC_CHARTTRANS_HIS()
        {
        }
    }
}
