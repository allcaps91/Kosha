namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HIC_CHUKDTL_RESULT :BaseDto
    {
        /// <summary>
        /// 측정고유번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 입력구분
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// 입력순번
        /// </summary>
        public long SEQNO { get; set; }

        /// <summary>
        /// 부서명
        /// </summary>
        public string DEPT_NM { get; set; }

        /// <summary>
        /// 공정코드
        /// </summary>
        public string PROCS_CD { get; set; }

        /// <summary>
        /// 공정명
        /// </summary>
        public string PROCS_NM { get; set; }

        /// <summary>
        /// 단위작업장소명
        /// </summary>
        public string UNIT_WRKRUM_NM { get; set; }

        /// <summary>
        /// 취급물질코드
        /// </summary>
        public string CHMCLS_CD { get; set; }

        /// <summary>
        /// 취급물질명
        /// </summary>
        public string CHMCLS_NM { get; set; }

        /// <summary>
        /// 취급물질그룹코드
        /// </summary>
        public string UCODE_GROUP_CD { get; set; }

        /// <summary>
        /// 취급물질그룹순번
        /// </summary>
        public long UCODE_GROUP_SEQ { get; set; }

        /// <summary>
        /// 근로자수
        /// </summary>
        public string LABRR_CD { get; set; }

        /// <summary>
        /// 작업내용
        /// </summary>
        public string OPERT_CN { get; set; }

        /// <summary>
        /// 근로형태
        /// </summary>
        public string LABOR_CND { get; set; }

        /// <summary>
        /// 실근로시간
        /// </summary>
        public long LABOR_TIME { get; set; }

        /// <summary>
        /// 유해인자발생시간
        /// </summary>
        public long UCODE_EXPSR_TIME { get; set; }

        /// <summary>
        /// 유해인자발생주기
        /// </summary>
        public string UCODE_EXPSR_CYCLE { get; set; }

        /// <summary>
        /// 측정위치
        /// </summary>
        public string WEM_LC { get; set; }

        /// <summary>
        /// 근로자명
        /// </summary>
        public string LABRR_NM { get; set; }

        /// <summary>
        /// 측정시간_시작
        /// </summary>
        public string WEM_TIME_FROM { get; set; }

        /// <summary>
        /// 측정시간_종료
        /// </summary>
        public string WEM_TIME_TO { get; set; }

        /// <summary>
        /// 측정횟수
        /// </summary>
        public long WEM_CO { get; set; }

        /// <summary>
        /// 측정결과값_평균_기타
        /// </summary>
        public string WEM_VALUE_AVRG_ETC { get; set; }

        /// <summary>
        /// 측정결과값_평균
        /// </summary>
        public decimal WEM_VALUE_AVRG { get; set; }

        /// <summary>
        /// 측정결과값_전회_기타
        /// </summary>
        public string WEM_VALUE_PREV_ETC { get; set; }

        /// <summary>
        /// 측정결과값_전회
        /// </summary>
        public decimal WEM_VALUE_PREV { get; set; }

        /// <summary>
        /// 측정결과값_금회_기타
        /// </summary>
        public string WEM_VALUE_NOW_ETC { get; set; }

        /// <summary>
        /// 측정결과값_금회
        /// </summary>
        public decimal WEM_VALUE_NOW { get; set; }

        /// <summary>
        /// 노출기준
        /// </summary>
        public string EXPSR_STDR_default { get; set; }
        
        /// <summary>
        /// 노출기준
        /// </summary>
        public string EXPSR_STDR_VALUE { get; set; }

        /// <summary>
        /// 노출기준_구분
        /// </summary>
        public string EXPSR_STDR_SE { get; set; }

        /// <summary>
        /// 노출기준_단위
        /// </summary>
        public string EXPSR_STDR_UNIT { get; set; }

        /// <summary>
        /// 측정평가결과
        /// </summary>
        public string WEN_EVL_RESULT { get; set; }

        /// <summary>
        /// 분석방법
        /// </summary>
        public string ANALS_MTH_CD { get; set; }

        /// <summary>
        /// 채취방법명
        /// </summary>
        public string WEM_MTH_NM { get; set; }

        /// <summary>
        /// 분석방법명
        /// </summary>
        public string ANALS_MTH_NM { get; set; }

        /// <summary>
        /// 비고
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 입력시간
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 입력사번
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 삭제일자
        /// </summary>
        public DateTime? DELDATE { get; set; }

        /// <summary>
        /// Rowid
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string IsDelete { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string IsGroup { get; set; }

        /// <summary>
        /// 건강검진 자료사전 Table
        /// </summary>
        public HIC_CHUKDTL_RESULT()
        {
        }
    }
}
