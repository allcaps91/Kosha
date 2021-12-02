namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_EXCEL : BaseDto
    {

        /// <summary>
        /// RowSelection
        /// </summary>
        public bool CHK { get; set; }

        /// <summary>
        /// 검진연도
        /// </summary>
        public string YEAR { get; set; } 

        /// <summary>
        /// 회사코드
        /// </summary>
		public long LTDCODE { get; set; } 

        /// <summary>
        /// 부서명
        /// </summary>
		public string LTDBUSE { get; set; } 

        /// <summary>
        /// 사번
        /// </summary>
		public string LTDSABUN { get; set; } 

        /// <summary>
        /// 직책
        /// </summary>
		public string JIKNAME { get; set; } 

        /// <summary>
        /// 직원성명
        /// </summary>
		public string JNAME { get; set; } 

        /// <summary>
        /// 관계
        /// </summary>
		public string REL { get; set; } 

        /// <summary>
        /// 수검자명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 종검등록번호
        /// </summary>
		public long PANO { get; set; } 

        /// <summary>
        /// 주민등록번호(암호화)
        /// </summary>
		public string AES_JUMIN { get; set; } 

        /// <summary>
        /// 성별
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 나이
        /// </summary>
		public long AGE { get; set; } 

        /// <summary>
        /// 휴대폰
        /// </summary>
		public string HPHONE { get; set; } 

        /// <summary>
        /// 사무실 또는 집 전화번호
        /// </summary>
		public string TEL { get; set; } 

        /// <summary>
        /// 종검유형
        /// </summary>
		public string GJTYPE { get; set; } 

        /// <summary>
        /// 증번호
        /// </summary>
		public string GKIHO { get; set; } 

        /// <summary>
        /// 입사일자
        /// </summary>
		public string IPSADATE { get; set; } 

        /// <summary>
        /// 사무직(1),비사무직(2)
        /// </summary>
		public string GBSAMU { get; set; } 

        /// <summary>
        /// 교대근무 여부
        /// </summary>
		public string GBNIGHT { get; set; } 

        /// <summary>
        /// 일반건진 제외
        /// </summary>
		public string GBNONHIC { get; set; } 

        /// <summary>
        /// 희망병원명
        /// </summary>
		public string HOSPITAL { get; set; } 

        /// <summary>
        /// 특수검진 취급물질명
        /// </summary>
		public string MCODES { get; set; } 

        /// <summary>
        /// 회사부담 선택검사 가능 여부
        /// </summary>
		public string GBLTDADDEXAM { get; set; } 

        /// <summary>
        /// 본인추가부담 선택검사명
        /// </summary>
		public string BONINADDEXAM { get; set; } 

        /// <summary>
        /// 회사부담 선택검사명
        /// </summary>
		public string LTDADDEXAM { get; set; } 

        /// <summary>
        /// 가족 본인부담 선택검사명
        /// </summary>
		public string GAJOKADDEXAM { get; set; } 

        /// <summary>
        /// 주소
        /// </summary>
		public string JUSO { get; set; } 

        /// <summary>
        /// 희망일자
        /// </summary>
		public string HDATE { get; set; } 

        /// <summary>
        /// 오전/오후
        /// </summary>
		public string AMPM { get; set; } 

        /// <summary>
        /// 예약일자
        /// </summary>
		public string RDATE { get; set; } 

        /// <summary>
        /// 검진일자
        /// </summary>
		public string SDATE { get; set; } 

        /// <summary>
        /// 참고사향
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 생년월일
        /// </summary>
		public string BIRTH { get; set; } 

        /// <summary>
        /// 공단검진 포함여부(포함,제외)
        /// </summary>
		public string GBNHIC { get; set; } 

        /// <summary>
        /// 주민등록번호 제공여부(1.엑셀제공 2.자동찾기 3.자료없음)
        /// </summary>
		public string GBJUMIN { get; set; } 

        /// <summary>
        /// 등록번호
        /// </summary>
		public string PTNO { get; set; } 

        /// <summary>
        /// 공단자격조회(1차,위,대장,...)
        /// </summary>
		public string NHICINFO { get; set; } 

        /// <summary>
        /// 등록 사번
        /// </summary>
		public long ENTSABUN { get; set; }

        /// <summary>
        /// 등록 일시(MM/DD)
        /// </summary>
        public string ENTDATE { get; set; }

        /// <summary>
        /// 등록 일시(yyyy-mm-dd hh24:mi:ss)
        /// </summary>
        public string ENTDATE1 { get; set; }

        /// <summary>
        /// 메모
        /// </summary>
        public string MEMO { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long PANO2 { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
		public string RID { get; set; }

        /// <summary>
        /// CNT
        /// </summary>
		public long CNT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string LTDNAME { get; set; }

        /// <summary>
        /// 수정일시
        /// </summary>
		public string UPDATETIME { get; set; }        

        /// <summary>
        /// 수정사번
        /// </summary>
        public string MODIFIEDUSER { get; set; }

        /// <summary>
        /// 휴대폰
        /// </summary>
        public string BUSENAME { get; set; }

        /// <summary>
        /// 종합검진 예약용 엑셀 자료
        /// </summary>
        public HEA_EXCEL()
        {
        }
    }
}
