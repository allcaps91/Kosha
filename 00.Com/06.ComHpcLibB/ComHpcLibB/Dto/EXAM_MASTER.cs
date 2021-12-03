namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;


    /// <summary>
    /// 
    /// </summary>
    public class EXAM_MASTER : BaseDto
    {
        
        /// <summary>
        /// Master Key
        /// </summary>
		public string MASTERCODE { get; set; } 

        /// <summary>
        /// 검사명:통상명칭
        /// </summary>
		public string EXAMNAME { get; set; } 

        /// <summary>
        /// 검사명:Full Name
        /// </summary>
		public string EXAMFNAME { get; set; } 

        /// <summary>
        /// 검사명:연보에 실린 이름
        /// </summary>
		public string EXAMYNAME { get; set; } 

        /// <summary>
        /// Work Station 1
        /// </summary>
		public string WSCODE1 { get; set; } 

        /// <summary>
        /// 검사 항목 결과 위치
        /// </summary>
		public string WSCODE1POS { get; set; } 

        /// <summary>
        /// Work Station 2
        /// </summary>
		public string WSCODE2 { get; set; } 

        /// <summary>
        /// 검사 항목 결과 위치
        /// </summary>
		public string WSCODE2POS { get; set; } 

        /// <summary>
        /// Work Station 3
        /// </summary>
		public string WSCODE3 { get; set; } 

        /// <summary>
        /// 검사 항목 결과 위치
        /// </summary>
		public string WSCODE3POS { get; set; } 

        /// <summary>
        /// Work Station 4
        /// </summary>
		public string WSCODE4 { get; set; } 

        /// <summary>
        /// 검사 항목 결과 위치
        /// </summary>
		public string WSCODE4POS { get; set; } 

        /// <summary>
        /// Work Station 5
        /// </summary>
		public string WSCODE5 { get; set; } 

        /// <summary>
        /// 검사 항목 결과 위치
        /// </summary>
		public string WSCODE5POS { get; set; } 

        /// <summary>
        /// Turn Around Time(일)
        /// </summary>
		public string TURNTIME1 { get; set; } 

        /// <summary>
        /// Turn Around Time(분)
        /// </summary>
		public string TURNTIME2 { get; set; } 

        /// <summary>
        /// 검사요일(일월화수목금토) (Y:검사, N:검사안함)
        /// </summary>
		public string EXAMWEEK { get; set; } 

        /// <summary>
        /// 용기종류코드
        /// </summary>
		public string TUBECODE { get; set; } 

        /// <summary>
        /// 검체종류코드
        /// </summary>
		public string SPECCODE { get; set; } 

        /// <summary>
        /// 채혈량코드
        /// </summary>
		public string VOLUMECODE { get; set; } 

        /// <summary>
        /// 장비코드
        /// </summary>
		public string EQUCODE1 { get; set; } 

        /// <summary>
        /// 장비코드
        /// </summary>
		public string EQUCODE2 { get; set; } 

        /// <summary>
        /// 장비코드
        /// </summary>
		public string EQUCODE3 { get; set; } 

        /// <summary>
        /// 장비코드
        /// </summary>
		public string EQUCODE4 { get; set; } 

        /// <summary>
        /// 결과단위코드
        /// </summary>
		public string UNITCODE { get; set; } 

        /// <summary>
        /// Data Type
        /// </summary>
		public string DATATYPE { get; set; } 

        /// <summary>
        /// Data Length
        /// </summary>
		public string DATALENGTH { get; set; } 

        /// <summary>
        /// 결과입력 유무(1:결과 입력 안함, 0:결과 입력 함)
        /// </summary>
		public string RESULTIN { get; set; } 

        /// <summary>
        /// Panic Value(From)
        /// </summary>
		public double PANICFROM { get; set; } 

        /// <summary>
        /// Panic Value(To)
        /// </summary>
		public double PANICTO { get; set; } 

        /// <summary>
        /// Delta Value(-)
        /// </summary>
		public double DELTAM { get; set; } 

        /// <summary>
        /// Delta value(+)
        /// </summary>
		public double DELTAP { get; set; } 

        /// <summary>
        /// DD, DP, RD, RP
        /// </summary>
		public string DDDPRDRP { get; set; } 

        /// <summary>
        /// Barcode 출력명
        /// </summary>
		public string BCODENAME { get; set; } 

        /// <summary>
        /// Barcode 인쇄장수
        /// </summary>
		public long BCODEPRINT { get; set; } 

        /// <summary>
        /// KeyPad Position(Diff용)
        /// </summary>
		public string KEYPAD { get; set; } 

        /// <summary>
        /// 연속검사(1.연속검사 0.연속검사아님)
        /// </summary>
		public long SERIES { get; set; } 

        /// <summary>
        /// Pending Checking 여부(1:Check, 0:Check 안함)
        /// </summary>
		public string PENDING { get; set; } 

        /// <summary>
        /// 사용종료일(삭제일자)
        /// </summary>
		public string ENDDATE { get; set; } 

        /// <summary>
        /// 변경일자
        /// </summary>
		public string MODIFYDATE { get; set; } 

        /// <summary>
        /// 변경자사번
        /// </summary>
		public long MODIFYID { get; set; } 

        /// <summary>
        /// SUB코드여부(1:Sub, 0:Mother) 화면에는 선택숨김
        /// </summary>
		public string SUB { get; set; } 

        /// <summary>
        /// 수가코드
        /// </summary>
		public string SUCODE { get; set; } 

        /// <summary>
        /// 수가코드 분류
        /// </summary>
		public string SUBUN { get; set; } 

        /// <summary>
        /// 모코드를 서브코드로 가진 것을 표시(1:존재, 0:없음)
        /// </summary>
		public string MOTHER { get; set; } 

        /// <summary>
        /// Help코드 존여여부(1.존재 0.없슴)
        /// </summary>
		public string HELPCODE { get; set; } 

        /// <summary>
        /// 오더SLIP의 Slip종류(0010-0050)
        /// </summary>
		public string SLIPNO { get; set; } 

        /// <summary>
        /// 연보 통계용 분류코드(C,F,H,...)
        /// </summary>
		public string TONGBUN { get; set; } 

        /// <summary>
        /// 연보 통계시 분류별 일련번호(우선순위)
        /// </summary>
		public long TONGSEQNO { get; set; } 

        /// <summary>
        /// 사용안함.
        /// </summary>
		public string GBTAX { get; set; } 

        /// <summary>
        /// 조직검사 TAT 소요시간
        /// </summary>
		public string GBTAT { get; set; } 

        /// <summary>
        /// 바코드 개별 발행(1:묶음 발행 않함)
        /// </summary>
		public string PIECE { get; set; } 

        /// <summary>
        /// 정상인참고치( * )
        /// </summary>
		public string GBBASE { get; set; } 

        /// <summary>
        /// 표준코드
        /// </summary>
		public string BCODE { get; set; } 

        /// <summary>
        /// 외부단가
        /// </summary>
		public long WAMT { get; set; } 

        /// <summary>
        /// 외부의뢰(1.보험,2.비보험,3.개별)
        /// </summary>
		public string WGB { get; set; } 

        /// <summary>
        /// 외부항목코드
        /// </summary>
		public string WHCODE { get; set; } 

        /// <summary>
        /// vutek 항생제 약어
        /// </summary>
		public string VITEKCODE { get; set; } 

        /// <summary>
        /// critical Value min 값
        /// </summary>
		public double CVMIN { get; set; } 

        /// <summary>
        /// critical Value Max 값
        /// </summary>
		public double CVMAX { get; set; } 

        /// <summary>
        /// critical Value (Negative, Positive, Reactive)
        /// </summary>
		public string CVVALUE { get; set; } 

        /// <summary>
        /// 통계제외(Y)
        /// </summary>
		public string TONGDISP { get; set; } 

        /// <summary>
        /// TLA 여부
        /// </summary>
		public string GBTLA { get; set; } 

        /// <summary>
        /// TLA 순서
        /// </summary>
		public string GBTLASORT { get; set; } 

        /// <summary>
        /// 병원체검사결과신고용 코드
        /// </summary>
		public string ACODE { get; set; } 

        /// <summary>
        /// TAT분류
        /// </summary>
		public string TAT_CLS { get; set; } 

        /// <summary>
        /// TAT사용여부
        /// </summary>
		public string TAT_YN { get; set; } 

        /// <summary>
        /// TAT일반시간
        /// </summary>
		public string TAT_MIN_01 { get; set; } 

        /// <summary>
        /// TAT응급실시간
        /// </summary>
		public string TAT_MIN_02 { get; set; } 

        /// <summary>
        /// TAT원격지시간
        /// </summary>
		public string TAT_MIN_03 { get; set; } 

        /// <summary>
        /// TAT응급검사
        /// </summary>
		public string TAT_MIN_04 { get; set; } 

        /// <summary>
        /// TAT 공휴
        /// </summary>
		public string TAT_HOLD { get; set; } 

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
        /// 
        /// </summary>
		public string TAT_DAY { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TAT_ER_YN { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TAT_WON_YN { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TAT_EM_YN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EXAMCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string CHKGWA { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string WSGRP_TITLE { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int CNT { get; set; }

        /// <summary>
        /// 검사코드 Master
        /// </summary>
        public EXAM_MASTER()
        {
        }
    }
}
