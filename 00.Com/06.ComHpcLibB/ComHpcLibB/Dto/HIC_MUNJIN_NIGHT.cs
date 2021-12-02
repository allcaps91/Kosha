namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_MUNJIN_NIGHT : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 불면증지수 문진데이타
        /// </summary>
		public string ITEM1_DATA { get; set; } 

        /// <summary>
        /// 불면증지수 점수
        /// </summary>
		public long ITEM1_JEMSU { get; set; } 

        /// <summary>
        /// 불면증지수 판정값
        /// </summary>
		public string ITEM1_PANJENG { get; set; } 

        /// <summary>
        /// 수면의질 문진데이타(1~18번) 컴마로 구분됨
        /// </summary>
		public string ITEM2_DATA { get; set; } 

        /// <summary>
        /// 수면의질 점수
        /// </summary>
		public long ITEM2_JEMSU { get; set; } 

        /// <summary>
        /// 수면의질 판정값
        /// </summary>
		public string ITEM2_PANJENG { get; set; } 

        /// <summary>
        /// 주간졸림 문진데이타
        /// </summary>
		public string ITEM3_DATA { get; set; } 

        /// <summary>
        /// 주간졸림 점수
        /// </summary>
		public long ITEM3_JEMSU { get; set; } 

        /// <summary>
        /// 주간졸림 판정값
        /// </summary>
		public string ITEM3_PANJENG { get; set; } 

        /// <summary>
        /// 최종 변경일자
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 최종 변경자 사번
        /// </summary>
		public long ENTSABUN { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public long RID { get; set; }

        /// <summary>
        /// 위장관질환(공통) 문진데이타
        /// </summary>
        public string ITEM4_DATA { get; set; }

        /// <summary>
        /// 위장관질환(공통) 점수
        /// </summary>
        public long ITEM4_JEMSU { get; set; }

        /// <summary>
        /// 위장관질환(공통) 판정값
        /// </summary>
        public string ITEM4_PANJENG { get; set; }
        /// <summary>
        /// 유방암(여성) 문진데이타
        /// </summary>
        /// 
		public string ITEM5_DATA { get; set; }

        /// <summary>
        /// 유방암(여성) 점수
        /// </summary>
        public long ITEM5_JEMSU { get; set; }

        /// <summary>
        /// 유방암(여성) 판정값
        /// </summary>
        public string ITEM5_PANJENG { get; set; }
        
        /// <summary>
        /// 야간작업 문진표
        /// </summary>
        /// 
        public HIC_MUNJIN_NIGHT()
        {
        }
    }
}
