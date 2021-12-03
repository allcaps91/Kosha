namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class EXAM_SPECMST : BaseDto
    {
        
        /// <summary>
        /// 검체번호
        /// </summary>
		public string SPECNO { get; set; } 

        /// <summary>
        /// 등록번호, 종합건진번호, 일반건진번호, 특수건진번호
        /// </summary>
		public string PANO { get; set; } 

        /// <summary>
        /// 환자구분(아래참조)
        /// </summary>
		public string BI { get; set; } 

        /// <summary>
        /// 이름
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 입원(I),외래(O)
        /// </summary>
		public string IPDOPD { get; set; } 

        /// <summary>
        /// 나이
        /// </summary>
		public long AGE { get; set; }

        /// <summary>
        /// 나이
        /// </summary>
        public string AGESEX { get; set; }

        /// <summary>
        /// 유아 개월수
        /// </summary>
        public long AGEMM { get; set; } 

        /// <summary>
        /// 성별(M:남,F:여)
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 진료과
        /// </summary>
		public string DEPTCODE { get; set; } 

        /// <summary>
        /// 병동
        /// </summary>
		public string WARD { get; set; } 

        /// <summary>
        /// 병실
        /// </summary>
		public long ROOM { get; set; } 

        /// <summary>
        /// 의사코드
        /// </summary>
		public string DRCODE { get; set; }

        /// <summary>
        /// 의사성명
        /// </summary>
		public string DRNAME { get; set; }
        
        /// <summary>
        /// 의사 Comment(형태:검사코드+의사컴멘트)
        /// </summary>
		public string DRCOMMENT { get; set; } 

        /// <summary>
        /// 응급여부(S:응급, R:Routine)
        /// </summary>
		public string STRT { get; set; } 

        /// <summary>
        /// BarCode에 인쇄될 문자
        /// </summary>
		public string WORKSTS { get; set; } 

        /// <summary>
        /// 검체코드
        /// </summary>
		public string SPECCODE { get; set; }

        /// <summary>
        /// 용기코드
        /// </summary>
		public string TUBE { get; set; } 

        /// <summary>
        /// 처방일
        /// </summary>
		public string BDATE { get; set; }

        public string BBDATE { get; set; }

        /// <summary>
        /// 채혈일시(일부스마트간호에서 UPDATE함)
        /// </summary>
		public string BLOODDATE { get; set; } 

        /// <summary>
        /// 접수일시
        /// </summary>
		public string RECEIVEDATE { get; set; } 

        /// <summary>
        /// 결과일시/취소일시
        /// </summary>
		public string RESULTDATE { get; set; } 

        /// <summary>
        /// 상태(아래참조)
        /// </summary>
		public string STATUS { get; set; } 

        /// <summary>
        /// 취소사유(Code로 관리)
        /// </summary>
		public string CANCEL { get; set; } 

        /// <summary>
        /// 출력횟수(검체별 결과가 완료된경우만 인쇄함)
        /// </summary>
		public long PRINT { get; set; } 

        /// <summary>
        /// 병리번호/취소자사번
        /// </summary>
		public string ANATNO { get; set; } 

        /// <summary>
        /// 오더코드
        /// </summary>
		public string GBORDERCODE { get; set; } 

        /// <summary>
        /// EMR(검사서식변환여부) 0.결과입력/변경 1.서식변환
        /// </summary>
		public string EMR { get; set; } 

        /// <summary>
        /// 오더시간(검사지시시각)(2005/08/25부터)
        /// </summary>
		public string ORDERDATE { get; set; } 

        /// <summary>
        /// 오더전송시간(2005/08/25부터)
        /// </summary>
		public string SENDDATE { get; set; } 

        /// <summary>
        /// 건진,종검 결과전송 Flag
        /// </summary>
		public string SENDFLAG { get; set; } 

        /// <summary>
        /// 응급실 등 과검사(Y.과검사)
        /// </summary>
		public string GB_GWAEXAM { get; set; } 

        /// <summary>
        /// 병동에서 검사항목 바코드출력할때 => Y 표시함.
        /// </summary>
		public string GBPRINT { get; set; } 

        /// <summary>
        /// 검진,종검 접수번호(결과전송용)
        /// </summary>
		public long HICNO { get; set; } 

        /// <summary>
        /// 결과 확인자
        /// </summary>
		public long VIEWID { get; set; } 

        /// <summary>
        /// 결과 확인일시
        /// </summary>
		public string VIEWDATE { get; set; } 

        /// <summary>
        /// ocs오더번호
        /// </summary>
		public long ORDERNO { get; set; } 

        /// <summary>
        /// 외부의뢰일자
        /// </summary>
		public string WDATE { get; set; } 

        /// <summary>
        /// 1.삼광의료재단 2.영대, 3.씨젠
        /// </summary>
		public string WGBN { get; set; } 

        /// <summary>
        /// 외부의뢰순서
        /// </summary>
		public long WSEQ { get; set; } 

        /// <summary>
        /// 1:일반, 2:조직
        /// </summary>
		public string WPART { get; set; } 

        /// <summary>
        /// HCV 신고일자( 감염관리 프로그램:EXINFECT에서 값발생)
        /// </summary>
		public string HCV { get; set; } 

        /// <summary>
        /// 1.보험, 2.비보험, 3.개별(엑셀자료)
        /// </summary>
		public string WGB { get; set; } 

        /// <summary>
        /// 외부(삼광) 단가(엑셀자료)
        /// </summary>
		public long WAMT { get; set; } 

        /// <summary>
        /// 외부(삼광) 항목코드(엑셀자료)
        /// </summary>
		public string WHCODE { get; set; } 

        /// <summary>
        /// 표준코드(엑셀자료)
        /// </summary>
		public string BCODE { get; set; } 

        /// <summary>
        /// 외부(삼광)검사명(엑셀자료)
        /// </summary>
		public string WEXNAME { get; set; } 

        /// <summary>
        /// 외부(삼광)검체명(엑셀자료)
        /// </summary>
		public string WSPECNAME { get; set; } 

        /// <summary>
        /// 외부(삼광)접수일(엑셀자료)
        /// </summary>
		public string WJDATE { get; set; } 

        /// <summary>
        /// HCV 참고사항(감염관리 프로그램:EXINFECT에서 값발생)
        /// </summary>
		public string HCVREMARK { get; set; } 

        /// <summary>
        /// EXAM_RESULT_IMG의 WRTNO 값
        /// </summary>
		public long IMGWRTNO { get; set; } 

        /// <summary>
        /// 과검사
        /// </summary>
		public string GB_GWAEXAM2 { get; set; } 

        /// <summary>
        /// MIC수가(B4062D) 발생요청일
        /// </summary>
		public string GBMIC { get; set; } 

        /// <summary>
        /// MIC수가 오더발생일 OR 수납일
        /// </summary>
		public string GBMICORDER { get; set; } 

        /// <summary>
        /// 외부의뢰 검체 건수
        /// </summary>
		public long WCNT { get; set; } 

        /// <summary>
        /// 검체접수
        /// </summary>
		public string JEPSUSABUN { get; set; } 

        /// <summary>
        /// 검체접수자명
        /// </summary>
		public string JEPSUNAME { get; set; } 

        /// <summary>
        /// 검사실검체접수자사번
        /// </summary>
		public string JEPSUSABUN2 { get; set; } 

        /// <summary>
        /// 검사실검체접수자명
        /// </summary>
		public string JEPSUNAME2 { get; set; } 

        /// <summary>
        /// 서브분류
        /// </summary>
		public string WPART_SUB { get; set; } 

        /// <summary>
        /// 체혈 ACT 사번 - 모바일에서 갱신
        /// </summary>
		public string ACTSABUN { get; set; } 

        /// <summary>
        /// 입력자
        /// </summary>
		public string INPS { get; set; } 

        /// <summary>
        /// 입력일시
        /// </summary>
		public string INPT_DT { get; set; } 

        /// <summary>
        /// 수정자
        /// </summary>
		public string UPPS { get; set; } 

        /// <summary>
        /// 수정일시
        /// </summary>
		public string UPDT { get; set; }

        /// <summary>
        /// 검사종류명
        /// </summary>
        public string EXNAME { get; set; }

        /// <summary>
        /// 검체번호 Master
        /// </summary>
        public EXAM_SPECMST()
        {
        }
    }
}
