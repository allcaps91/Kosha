namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class BAS_SUT : BaseDto
    {
        
        /// <summary>
        /// 수가코드
        /// </summary>
		public string SUCODE { get; set; } 

        /// <summary>
        /// 분류코드
        /// </summary>
		public string BUN { get; set; } 

        /// <summary>
        /// 누적코드
        /// </summary>
		public string NU { get; set; } 

        /// <summary>
        /// 수가구분 (A항),Record구분1단순,2복합,3Routine
        /// </summary>
		public string SUGBA { get; set; } 

        /// <summary>
        /// 수가구분 (B항),비고참조
        /// </summary>
		public string SUGBB { get; set; } 

        /// <summary>
        /// 수가구분 (C항),심야여부,증류수사용여부
        /// </summary>
		public string SUGBC { get; set; } 

        /// <summary>
        /// 수가구분 (D항),검사체감 종목수
        /// </summary>
		public string SUGBD { get; set; } 

        /// <summary>
        /// 수가구분 (E항),기술료 가산여부
        /// </summary>
		public string SUGBE { get; set; } 

        /// <summary>
        /// 수가구분 (F항),급여,비급여 구분
        /// </summary>
		public string SUGBF { get; set; } 

        /// <summary>
        /// 수가구분 (G항),입력항목구분
        /// </summary>
		public string SUGBG { get; set; } 

        /// <summary>
        /// 수가구분 (H항),특진료가산여부
        /// </summary>
		public string SUGBH { get; set; } 

        /// <summary>
        /// 수가구분 (I항),소아,심야 같이 적용하는 항목
        /// </summary>
		public string SUGBI { get; set; } 

        /// <summary>
        /// 수가구분 (J항) 9.외부의뢰검사
        /// </summary>
		public string SUGBJ { get; set; } 

        /// <summary>
        /// 수가구분 (K항) 감액대상 제외(0.감액 1.제외)
        /// </summary>
		public string SUGBK { get; set; } 

        /// <summary>
        /// 일반수가
        /// </summary>
		public long IAMT { get; set; } 

        /// <summary>
        /// 자보수가
        /// </summary>
		public long TAMT { get; set; } 

        /// <summary>
        /// 보험수가
        /// </summary>
		public long BAMT { get; set; } 

        /// <summary>
        /// 수가변경일자2
        /// </summary>
		public DateTime? SUDATE { get; set; } 

        /// <summary>
        /// 일반수가2
        /// </summary>
		public long OLDIAMT { get; set; } 

        /// <summary>
        /// 자보수가2
        /// </summary>
		public long OLDTAMT { get; set; } 

        /// <summary>
        /// 보험수가2
        /// </summary>
		public long OLDBAMT { get; set; } 

        /// <summary>
        /// 일일한도량(입력시 Check되는 Max수량)
        /// </summary>
		public long DAYMAX { get; set; } 

        /// <summary>
        /// 최대복용량(I항과 연계하여 관리되는 Max수량)
        /// </summary>
		public long TOTMAX { get; set; } 

        /// <summary>
        /// Next Key(품명 코드),BAS_SUN(Sunext)
        /// </summary>
		public string SUNEXT { get; set; } 

        /// <summary>
        /// 삭제일자(사용중지일자)
        /// </summary>
		public DateTime? DELDATE { get; set; } 

        /// <summary>
        /// 수가구분(L항):0.협약수가적용 1.실구입가
        /// </summary>
		public string SUGBL { get; set; } 

        /// <summary>
        /// 수가변경일자3
        /// </summary>
		public DateTime? SUDATE3 { get; set; } 

        /// <summary>
        /// 일반수가3
        /// </summary>
		public long IAMT3 { get; set; } 

        /// <summary>
        /// 자보수가3
        /// </summary>
		public long TAMT3 { get; set; } 

        /// <summary>
        /// 보험수가3
        /// </summary>
		public long BAMT3 { get; set; } 

        /// <summary>
        /// 수가변경일자4
        /// </summary>
		public DateTime? SUDATE4 { get; set; } 

        /// <summary>
        /// 일반수가4
        /// </summary>
		public long IAMT4 { get; set; } 

        /// <summary>
        /// 자보수가4
        /// </summary>
		public long TAMT4 { get; set; } 

        /// <summary>
        /// 보험수가4
        /// </summary>
		public long BAMT4 { get; set; } 

        /// <summary>
        /// 수가변경일자5
        /// </summary>
		public DateTime? SUDATE5 { get; set; } 

        /// <summary>
        /// 일반수가5
        /// </summary>
		public long IAMT5 { get; set; } 

        /// <summary>
        /// 자보수가5
        /// </summary>
		public long TAMT5 { get; set; } 

        /// <summary>
        /// 보험수가5
        /// </summary>
		public long BAMT5 { get; set; } 

        /// <summary>
        /// 수가코드 new
        /// </summary>
		public string CSUNEXT { get; set; } 

        /// <summary>
        /// 품목코드 new
        /// </summary>
		public string CSUCODE { get; set; } 

        /// <summary>
        /// 분류코드 new
        /// </summary>
		public string CBUN { get; set; } 

        
        /// <summary>
        /// 수가 Terminal (행위입력시 Read)
        /// </summary>
        public BAS_SUT()
        {
        }
    }
}
