namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_GROUPCODE : BaseDto
    {
        
        /// <summary>
        /// 묶음코드
        /// </summary>
		public string CODE { get; set; } 

        /// <summary>
        /// 항목분류
        /// </summary>
		public string HANG { get; set; } 

        /// <summary>
        /// 검진종류(기타검진만 종류가 표시됨)
        /// </summary>
		public string JONG { get; set; } 

        /// <summary>
        /// 선택검사여부(Y.선택검사 N.필수검사)
        /// </summary>
		public string GBSELECT { get; set; } 

        /// <summary>
        /// 묶음코드명
        /// </summary>
		public string NAME { get; set; } 

        /// <summary>
        /// 취급물질명
        /// </summary>
		public string UCODE { get; set; } 

        /// <summary>
        /// 삭제일자
        /// </summary>
		public DateTime? DELDATE { get; set; } 

        /// <summary>
        /// 최종 변경일자
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 최종 변경자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 묶음코드 약어
        /// </summary>
		public string YNAME { get; set; } 

        /// <summary>
        /// 수가적용구분(아래참조)
        /// </summary>
		public string GBSUGA { get; set; } 

        /// <summary>
        /// 사용시작일자
        /// </summary>
		public DateTime? SDATE { get; set; } 

        /// <summary>
        /// 부담율설정
        /// </summary>
		public string GBSELF { get; set; } 

        /// <summary>
        /// 암검진구분 예)1,1,0,1,1,0 (위UGI,위GFS,간암,대장암,유방,자궁)
        /// </summary>
		public string GBAM { get; set; } 

        /// <summary>
        /// 검사정보
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 구강검진코드
        /// </summary>
		public string GBDENT { get; set; } 

        /// <summary>
        /// 검사명표기
        /// </summary>
		public string EXAMNAME { get; set; } 

        /// <summary>
        /// 접수증 출력 구분 예)1,1,0,1,1,0 (아래참조)
        /// </summary>
		public string GBPRINT { get; set; } 

        /// <summary>
        /// 2차 추가검사 선정항목(최대 6개까지 가능)
        /// </summary>
		public string REEXAM { get; set; } 

        /// <summary>
        /// 수납여부체크
        /// </summary>
		public string GBSUNAP { get; set; } 

        /// <summary>
        /// 건진판정 추가판정에 제외 여부(Y.제외)
        /// </summary>
		public string GBNOTADDPAN { get; set; } 

        /// <summary>
        /// 특수검진 표적장기별 상담여부
        /// </summary>
		public string GBSANGDAM { get; set; } 

        /// <summary>
        /// 폐활량문진표 표시여부(Y:대상)
        /// </summary>
		public string GBGUBUN1 { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IsDelete { get; set; }
        
        /// <summary>
        /// 묶음코드 명칭
        /// </summary>
        public HIC_GROUPCODE()
        {
        }
    }
}
