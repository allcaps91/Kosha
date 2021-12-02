namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class CARD_APPROV_CENTER : BaseDto
    {
        
        /// <summary>
        /// 신용카드 승인일련번호(1~9999999999)
        /// </summary>
		public long CARDSEQNO { get; set; } 

        /// <summary>
        /// 회계일자
        /// </summary>
		public string ACTDATE { get; set; } 

        /// <summary>
        /// 거래일련번호
        /// </summary>
		public long CDSEQNO { get; set; } 

        /// <summary>
        /// 병록번호
        /// </summary>
		public string PANO { get; set; } 

        /// <summary>
        /// 건진번호
        /// </summary>
		public long HPANO { get; set; } 

        /// <summary>
        /// 건진접수번호
        /// </summary>
		public long HWRTNO { get; set; } 

        /// <summary>
        /// 주민등록번호(뒤 6자리 별표처리)
        /// </summary>
		public string JUMIN { get; set; } 

        /// <summary>
        /// 수진자명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 외래/입원
        /// </summary>
		public string GBIO { get; set; } 

        /// <summary>
        /// 진료과
        /// </summary>
		public string DEPTCODE { get; set; } 

        /// <summary>
        /// 카드수납구분
        /// </summary>
		public string PCODE { get; set; } 

        /// <summary>
        /// 입력조
        /// </summary>
		public string PART { get; set; } 

        /// <summary>
        /// 영수증일련번호
        /// </summary>
		public long SEQNO { get; set; } 

        /// <summary>
        /// 거래구분(1:승인요청및요청응답  2:취소승인요청및응답)
        /// </summary>
		public string TRANHEADER { get; set; } 

        /// <summary>
        /// 단말기번호
        /// </summary>
		public string TEMINALID { get; set; } 

        /// <summary>
        /// 거래일시(Date & Time)
        /// </summary>
		public DateTime? TRANDATE { get; set; } 

        /// <summary>
        /// 입력구분(S:Swipe  K:Key_In)
        /// </summary>
		public string INPUTMETHOD { get; set; } 

        /// <summary>
        /// 할부개월수
        /// </summary>
		public string INSTPERIOD { get; set; } 

        /// <summary>
        /// 총금액
        /// </summary>
		public long TRADEAMT { get; set; } 

        /// <summary>
        /// 세금
        /// </summary>
		public long TRXAMT { get; set; } 

        /// <summary>
        /// 봉사표
        /// </summary>
		public long SERVICEAMT { get; set; } 

        /// <summary>
        /// 승인일시(Date & Time)
        /// </summary>
		public string APPROVDATE { get; set; } 

        /// <summary>
        /// 발급사명
        /// </summary>
		public string FINAME { get; set; } 

        /// <summary>
        /// 매입사명
        /// </summary>
		public string ACCEPTERNAME { get; set; } 

        /// <summary>
        /// 매입처코드
        /// </summary>
		public string FICODE { get; set; } 

        /// <summary>
        /// 가맹점호
        /// </summary>
		public string ACCOUNTNO { get; set; } 

        /// <summary>
        /// (원)승인번호
        /// </summary>
		public string ORIGINNO { get; set; } 

        /// <summary>
        /// 원거래일자
        /// </summary>
		public string ORIGINDATE { get; set; } 

        /// <summary>
        /// 카드번호(암호화)
        /// </summary>
		public string CARDNO { get; set; } 

        /// <summary>
        /// 유효기간(암호화)
        /// </summary>
		public string PERIOD { get; set; } 

        /// <summary>
        /// 전체카드번호(암호화)
        /// </summary>
		public string FULLCARDNO { get; set; } 

        /// <summary>
        /// 비고
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 입력자사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 입력일시(Date & Time)
        /// </summary>
		public DateTime? ENTERDATE { get; set; } 

        /// <summary>
        /// 1.카드 , 2.현금
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 센터구분(1:건진, 2:종검)
        /// </summary>
		public string CENTERGBN { get; set; } 

        /// <summary>
        /// 현금취소사유 (1:거래취소,2:오류발급,3:기타)
        /// </summary>
		public string CASHCAN { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GBTAX { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// IC칩 거래
        /// </summary>
		public string IC { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GUBUN1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string ORIGINNO2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FRDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TODATE { get; set; }

        public string GBRECORD { get; set; }

        public string COMPANYNAME { get; set; }
        
        public long TAMT { get; set; }



        /// <summary>
        /// 건강증진센터 신용카드 승인 테이블
        /// </summary>
        public CARD_APPROV_CENTER()
        {
        }
    }
}
