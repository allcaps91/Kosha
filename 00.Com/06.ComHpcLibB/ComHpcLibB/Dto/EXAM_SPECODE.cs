namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class EXAM_SPECODE : BaseDto
    {
        
        /// <summary>
        /// 각 기초코드의 구분자(11-21,71) 아래참조
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 각항목의 코드,장비코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 코드내용,장비에서 나오는 코드
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 내용의약어,검사실에서 사용하는 코드
        /// </summary>
		public string YNAME { get; set; } 

        /// <summary>
        /// Work Station의 Group 표시, 검체의 색상 표시
        /// </summary>
		public string WSGROUP { get; set; } 

        /// <summary>
        /// Work Station의 검사장비 표시(aaa,bbb,ccc식으로 저장)
        /// </summary>
		public string WSEQU { get; set; } 

        /// <summary>
        /// SORT 우선순위
        /// </summary>
		public long SORT { get; set; } 

        /// <summary>
        /// 삭제일자
        /// </summary>
		public DateTime? DELDATE { get; set; } 

        /// <summary>
        /// 사용여부
        /// </summary>
		public string USE_YN { get; set; } 

        /// <summary>
        /// 입력자
        /// </summary>
		public string INPS { get; set; } 

        /// <summary>
        /// 입력일시
        /// </summary>
		public DateTime? INPT_DT { get; set; } 

        /// <summary>
        /// 수정자
        /// </summary>
		public string UPPS { get; set; } 

        /// <summary>
        /// 수정일시
        /// </summary>
		public DateTime? UPDT { get; set; } 

        
        /// <summary>
        /// 임상병리 일반코드
        /// </summary>
        public EXAM_SPECODE()
        {
        }
    }
}
