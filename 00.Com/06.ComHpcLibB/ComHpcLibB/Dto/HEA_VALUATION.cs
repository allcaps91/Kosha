namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_VALUATION : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 흡연(1:현재하고있다, 2:안한다)
        /// </summary>
		public string SMOKE { get; set; } 

        /// <summary>
        /// 신체활동부족(1:규칙적으로 한다, 2:운동부족이다)
        /// </summary>
		public string ACTIVE { get; set; } 

        /// <summary>
        /// 가족력- 누구?
        /// </summary>
		public string GAJOK1 { get; set; } 

        /// <summary>
        /// 가족력- 몇세?
        /// </summary>
		public string GAJOK2 { get; set; } 

        /// <summary>
        /// 가족력(뇌졸중,협심증,심근경색증)발생 ex) 전부있을경우 111
        /// </summary>
		public string GAJOK3 { get; set; } 

        /// <summary>
        /// 표적장기(좌심실비대,요단백검사,죽상동맥경화증,고혈압성망막증)
        /// </summary>
		public string JANGI { get; set; } 

        /// <summary>
        /// 동반된질병상태(당뇨,뇌혈관 및 심혈관질환,신장질환,말초혈관질환)
        /// </summary>
		public string JIHWAN { get; set; } 

        /// <summary>
        /// 최종등록자
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 입력상태
        /// </summary>
		public string GBSTS { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string ROWID { get; set; }

        /// <summary>
        /// 뇌심혈관질환 발병위험도 평가자료 종합조사표
        /// </summary>
        public HEA_VALUATION()
        {
        }
    }
}
