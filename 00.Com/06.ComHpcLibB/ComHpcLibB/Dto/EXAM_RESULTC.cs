namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class EXAM_RESULTC : BaseDto
    {
        
        /// <summary>
        /// 검체번호
        /// </summary>
		public string SPECNO { get; set; } 

        /// <summary>
        /// 결과 입력한 Work Station
        /// </summary>
		public string RESULTWS { get; set; } 

        /// <summary>
        /// 장비코드
        /// </summary>
		public string EQUCODE { get; set; } 

        /// <summary>
        /// 일련번호(결과조회,인쇄시 사용)
        /// </summary>
		public string SEQNO { get; set; } 

        /// <summary>
        /// 등록번호, 종합건진번호, 일반건진번호, 특수건진번호
        /// </summary>
		public string PANO { get; set; } 

        /// <summary>
        /// 검사코드(모 코드)
        /// </summary>
		public string MASTERCODE { get; set; } 

        /// <summary>
        /// 검사코드(모 코드,자 코드)
        /// </summary>
		public string SUBCODE { get; set; } 

        /// <summary>
        /// H:Header, V:Verify, Y:결과O, N:결과X
        /// </summary>
		public string STATUS { get; set; } 

        /// <summary>
        /// 결과
        /// </summary>
		public string RESULT { get; set; } 

        /// <summary>
        /// 결과일시
        /// </summary>
		public DateTime? RESULTDATE { get; set; } 

        /// <summary>
        /// 검사자사번
        /// </summary>
		public long RESULTSABUN { get; set; } 

        /// <summary>
        /// Reference
        /// </summary>
		public string REFER { get; set; } 

        /// <summary>
        /// Panic
        /// </summary>
		public string PANIC { get; set; } 

        /// <summary>
        /// Delta
        /// </summary>
		public string DELTA { get; set; } 

        /// <summary>
        /// 결과단위
        /// </summary>
		public string UNIT { get; set; } 

        /// <summary>
        /// 사용않함(99.6.15)
        /// </summary>
		public long PRINT { get; set; } 

        /// <summary>
        /// 병리번호
        /// </summary>
		public string ANATNO { get; set; } 

        /// <summary>
        /// 사용않됨(99.9.13)
        /// </summary>
		public string GBORDER { get; set; } 

        /// <summary>
        /// 결과 전송된 인터페이스 장비코드
        /// </summary>
		public string EQUCODE_INTER { get; set; } 

        /// <summary>
        /// helpcode (균주코드)
        /// </summary>
		public string HCODE { get; set; } 

        /// <summary>
        /// 1.보험, 2.비보험, 3.개별(엑셀자료)
        /// </summary>
		public string WGB { get; set; } 

        /// <summary>
        /// 외부(삼광) 항목코드(엑셀자료)
        /// </summary>
		public string WHCODE { get; set; } 

        /// <summary>
        /// 외부(삼광) 단가(엑셀자료)
        /// </summary>
		public long WAMT { get; set; } 

        /// <summary>
        /// 외부(삼광)검사명(엑셀자료)
        /// </summary>
		public string WEXAMNAME { get; set; } 

        /// <summary>
        /// 외부(삼광)검체명(엑셀자료)
        /// </summary>
		public string WSPECNAME { get; set; } 

        /// <summary>
        /// 외부(삼광)접수일(엑셀자료)
        /// </summary>
		public string WJDATE { get; set; } 

        /// <summary>
        /// 표준코드
        /// </summary>
		public string BCODE { get; set; } 

        /// <summary>
        /// 이미지결과 WRTNO
        /// </summary>
		public long IMGWRTNO { get; set; } 

        /// <summary>
        /// critical Value (=C)
        /// </summary>
		public string CV { get; set; } 

        /// <summary>
        /// TAT시간
        /// </summary>
		public string TAT_DT { get; set; } 

        /// <summary>
        /// TAT입력자
        /// </summary>
		public string TAT_INPT { get; set; } 

        /// <summary>
        /// TAT지연사유
        /// </summary>
		public string TAT_CASE { get; set; } 

        /// <summary>
        /// TAT지연사유
        /// </summary>
		public string TAT_CASE_CD { get; set; } 

        /// <summary>
        /// 재검전사유코드
        /// </summary>
		public string RE_RESULT_CASE_CD { get; set; } 

        /// <summary>
        /// 재검전사번
        /// </summary>
		public string RE_RESULT_INPT { get; set; } 

        /// <summary>
        /// 재검전사유
        /// </summary>
		public string RE_RESULT_CASE { get; set; } 

        /// <summary>
        /// 재검전시간
        /// </summary>
		public DateTime? RE_RESULT_DT { get; set; } 

        /// <summary>
        /// 재검전결과
        /// </summary>
		public string RE_RESULT { get; set; } 

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
        /// 
        /// </summary>
		public string RE_RESULT_TO_DO { get; set; } 

        /// <summary>
        /// 삼광코드
        /// </summary>
		public string SMCODE { get; set; }

        /// <summary>
        /// 삼광코드
        /// </summary>
        public string EXAMNAME { get; set; }


        /// <summary>
        /// 검체별 검사내역 및 결과
        /// </summary>
        public EXAM_RESULTC()
        {
        }
    }
}
