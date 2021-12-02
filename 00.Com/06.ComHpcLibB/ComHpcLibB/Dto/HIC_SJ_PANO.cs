namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_SJ_PANO : BaseDto
    {
        
        /// <summary>
        /// 건진년도
        /// </summary>
		public string GJYEAR { get; set; } 

        /// <summary>
        /// 회사코드
        /// </summary>
		public long LTDCODE { get; set; } 

        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 부서/공정
        /// </summary>
		public string BUSE { get; set; } 

        /// <summary>
        /// 측정결과
        /// </summary>
		public string CHUKRESULT { get; set; } 

        /// <summary>
        /// 전년도판정결과
        /// </summary>
		public string PANJENG { get; set; } 

        /// <summary>
        /// 삭제여부(Y.삭제)
        /// </summary>
		public string GBDEL { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }


        /// <summary>
        /// 건강진단 사전조사 대상자명단
        /// </summary>
        public HIC_SJ_PANO()
        {
        }
    }
}
