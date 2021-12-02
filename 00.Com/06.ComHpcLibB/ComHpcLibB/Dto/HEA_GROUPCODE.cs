namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HEA_GROUPCODE : BaseDto
    {
        
        /// <summary>
        /// 묶음코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 사용가능 검진종류(**.공통사용)
        /// </summary>
		public string JONG { get; set; } 

        /// <summary>
        /// 묶음코드명
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 묶음코드 약어
        /// </summary>
		public string YNAME { get; set; } 

        /// <summary>
        /// 성별구분(M.남자 F.여자 *.공통)
        /// </summary>
		public string GBSEX { get; set; } 

        /// <summary>
        /// 선택검사여부(Y.선택검사 N.필수검사)
        /// </summary>
		public string GBSELECT { get; set; } 

        /// <summary>
        /// 기본 부담율(1.본인 2.회사 3.본인.회사50%)
        /// </summary>
		public string BURATE { get; set; } 

        /// <summary>
        /// 접수시 부담율 변경가능(Y.가능 N.불가능)
        /// </summary>
		public string GBSELF { get; set; } 

        /// <summary>
        /// 감액가능(Y.감액가능 N.감액불가)
        /// </summary>
		public string GBGAM { get; set; } 

        /// <summary>
        /// 회사코드(회사종검만 해당)
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 조회용 LTDCODE
        /// </summary>
		public string V_LTDCODE { get; set; }
        
        /// <summary>
        /// 회사계약일자
        /// </summary>
		public DateTime? SDATE { get; set; } 

        /// <summary>
        /// 삭제일자
        /// </summary>
		public DateTime? DELDATE { get; set; } 

        /// <summary>
        /// 현재 적용금액
        /// </summary>
		public long AMT { get; set; } 

        /// <summary>
        /// 적용 기준일자
        /// </summary>
		public DateTime? SUDATE { get; set; } 

        /// <summary>
        /// 종전 적용금액
        /// </summary>
		public long OLDAMT { get; set; } 

        /// <summary>
        /// 최종 변경일자
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 최종 변경자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 유형(1~~~)
        /// </summary>
		public string TYPE { get; set; } 

        /// <summary>
        /// 접수증출력 구분(1,1,0,0 ...)
        /// </summary>
		public string GBPRINT { get; set; } 

        /// <summary>
        /// 검사명 표기
        /// </summary>
		public string EXAMNAME { get; set; } 

        /// <summary>
        /// 수면코드 연동
        /// </summary>
		public string ENDOCODE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE2 { get; set; }

        /// <summary>
        /// 회사명
        /// </summary>
        public string LTDNAME { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string GBSUGA { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }


        /// <summary>
        /// 종합검진 그룹코드
        /// </summary>
        public HEA_GROUPCODE()
        {
        }
    }
}
