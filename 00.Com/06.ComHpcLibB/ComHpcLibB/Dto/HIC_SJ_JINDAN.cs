namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_SJ_JINDAN : BaseDto
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
        /// 건강구분
        /// </summary>
		public string PANJENG { get; set; } 

        /// <summary>
        /// 부서/공정
        /// </summary>
		public string BUSE { get; set; } 

        /// <summary>
        /// 유해인자
        /// </summary>
		public string YUHE { get; set; } 

        /// <summary>
        /// 표적장기
        /// </summary>
		public string JANGGI { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 인원수
        /// </summary>
        public long INWON { get; set; }


        /// <summary>
        /// 건강진단 사전조사표(과년도 특수건강진단결과 요약)
        /// </summary>
        public HIC_SJ_JINDAN()
        {
        }
    }
}
