namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_HYANG : BaseDto
    {
        
        /// <summary>
        /// 등록번호, 이월작업시 00000000
        /// </summary>
		public string PTNO { get; set; } 

        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 성명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 약품종류(1.향정  2.마약)
        /// </summary>
		public string JONG { get; set; } 

        /// <summary>
        /// 보험자격
        /// </summary>
		public string BI { get; set; } 

        /// <summary>
        /// 병동
        /// </summary>
		public string WARDCODE { get; set; } 

        /// <summary>
        /// 호실
        /// </summary>
		public long ROOMCODE { get; set; } 

        /// <summary>
        /// 처방일자
        /// </summary>
		public DateTime? BDATE { get; set; } 

        /// <summary>
        /// 수검일자
        /// </summary>
		public DateTime? SDATE { get; set; } 

        /// <summary>
        /// 전송일자(수납처리)
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 삭제일자
        /// </summary>
		public DateTime? DELDATE { get; set; } 

        /// <summary>
        /// 진료과
        /// </summary>
		public string DEPTCODE { get; set; } 

        /// <summary>
        /// 의사사번
        /// </summary>
		public string DRSABUN { get; set; } 

        /// <summary>
        /// 입원/외래
        /// </summary>
		public string IO { get; set; } 

        /// <summary>
        /// 수가코드
        /// </summary>
		public string SUCODE { get; set; } 

        /// <summary>
        /// 수량
        /// </summary>
		public string QTY { get; set; } 

        /// <summary>
        /// 실수량
        /// </summary>
		public double REALQTY { get; set; } 

        /// <summary>
        /// 날수
        /// </summary>
		public long NAL { get; set; } 

        /// <summary>
        /// 용법
        /// </summary>
		public string DOSCODE { get; set; } 

        /// <summary>
        /// 병명
        /// </summary>
		public string REMARK1 { get; set; } 

        /// <summary>
        /// 주요증상
        /// </summary>
		public string REMARK2 { get; set; } 

        /// <summary>
        /// 일련번호
        /// </summary>
		public long SEQNO { get; set; } 

        /// <summary>
        /// 출력여부 출력시 Y로 입력함
        /// </summary>
		public string PRINT { get; set; } 

        /// <summary>
        /// 오더번호
        /// </summary>
		public long ORDERNO { get; set; } 

        /// <summary>
        /// 반납여부(1.반납)
        /// </summary>
		public string BANBUN { get; set; } 

        /// <summary>
        /// 반납사유
        /// </summary>
		public string BANREMARK { get; set; } 

        /// <summary>
        /// 마약 대장 출력시 순번임.
        /// </summary>
		public string NO1 { get; set; } 

        /// <summary>
        /// 재고
        /// </summary>
		public double JEGO { get; set; } 

        /// <summary>
        /// 점검완료 "1"
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 조회 순서
        /// </summary>
		public long GBSORT { get; set; } 

        /// <summary>
        /// ABC원가 진료 과장
        /// </summary>
		public string DRCODE { get; set; } 

        /// <summary>
        /// 이월수량
        /// </summary>
		public double IWOLQTY { get; set; } 

        /// <summary>
        /// 상병코드
        /// </summary>
		public string ILLCODE { get; set; } 

        /// <summary>
        /// 간호사 사번
        /// </summary>
		public string NRSABUN { get; set; } 

        /// <summary>
        /// 파우더여부
        /// </summary>
		public string POWDER { get; set; } 

        /// <summary>
        /// 성별
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 나이
        /// </summary>
		public long AGE { get; set; } 

        /// <summary>
        /// 주민번호
        /// </summary>
		public string JUMIN { get; set; } 

        /// <summary>
        /// 주소
        /// </summary>
		public string JUSO { get; set; } 

        /// <summary>
        /// 전자서명 번호
        /// </summary>
		public long CERTNO { get; set; } 

        /// <summary>
        /// 입력수량
        /// </summary>
		public double ENTQTY { get; set; } 

        /// <summary>
        /// 오더전송위치(opd, ero: 외래)
        /// </summary>
		public string ORDERSITE { get; set; } 

        /// <summary>
        /// 내시경실 수기 처방 인쇄 일시
        /// </summary>
		public DateTime? JDATE_ENDO { get; set; } 

        /// <summary>
        /// 사용수량
        /// </summary>
		public double ENTQTY2 { get; set; } 

        /// <summary>
        /// 주민번호 암호화
        /// </summary>
		public string JUMIN2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long CHASU { get; set; }

        public string ROWID { get; set; }

        public string RID { get; set; }

        /// <summary>
        /// 건진센터 향정약품 관리 Table
        /// </summary>
        public HIC_HYANG()
        {
        }
    }
}
