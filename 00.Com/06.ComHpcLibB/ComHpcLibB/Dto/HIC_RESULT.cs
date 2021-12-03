namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_RESULT : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 묶음코드
        /// </summary>
		public string GROUPCODE { get; set; } 

        /// <summary>
        /// 검사부서(1.신체계측 2.임상병리과 3.방사선과 4.기타검사)
        /// </summary>
		public string PART { get; set; } 

        /// <summary>
        /// 검사코드
        /// </summary>
		public string EXCODE { get; set; } 

        /// <summary>
        /// 검사결과분류코드(기타는 NULL)
        /// </summary>
		public string RESCODE { get; set; } 

        /// <summary>
        /// 검사결과
        /// </summary>
		public string RESULT { get; set; } 

        /// <summary>
        /// 검사결과판정(B.정상B,R.질환의심,NULL:정상)
        /// </summary>
		public string PANJENG { get; set; } 

        /// <summary>
        /// ABC시행부서
        /// </summary>
		public string OPER_DEPT { get; set; } 

        /// <summary>
        /// ABC시행의사(사번)
        /// </summary>
		public string OPER_DCT { get; set; } 

        /// <summary>
        /// Acting 여부 (Y: 액팀함, Null: 안함)
        /// </summary>
		public string ACTIVE { get; set; } 

        /// <summary>
        /// 작업자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 작업날짜
        /// </summary>
		public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string ROWID { get; set; }

        /// <summary>
        /// HANME
        /// </summary>
        public string HNAME { get; set; }

        /// <summary>
        /// CNT
        /// </summary>
        public long CNT { get; set; }

        /// <summary>
        /// 작업날짜
        /// </summary>
		public DateTime? MINDATE { get; set; }

        /// <summary>
        /// 작업날짜
        /// </summary>
		public DateTime? MAXDATE { get; set; }

        /// <summary>
        /// HANME
        /// </summary>
        public string RID { get; set; }


        /// <summary>
        /// 일반검진 검사항목별 결과
        /// </summary>
        public HIC_RESULT()
        {
        }
    }
}
