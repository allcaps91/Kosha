namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_WAIT_ROOM : BaseDto
    {
        
        /// <summary>
        /// 방번호
        /// </summary>
		public string ROOM { get; set; } 

        /// <summary>
        /// 상담/검사실 명칭
        /// </summary>
		public string ROOMNAME { get; set; } 

        /// <summary>
        /// 오전 사용여부(Y/N)
        /// </summary>
		public string AMUSE { get; set; } 

        /// <summary>
        /// 오후 사용여부(Y/N)
        /// </summary>
		public string PMUSE { get; set; } 

        /// <summary>
        /// 오전 채용상담 여부(Y/N)
        /// </summary>
		public string AMSANG { get; set; } 

        /// <summary>
        /// 오후 채용상담 여부(Y/N)
        /// </summary>
		public string PMSANG { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 건진대기순번(방번호 설정)
        /// </summary>
        public HIC_WAIT_ROOM()
        {
        }
    }
}
