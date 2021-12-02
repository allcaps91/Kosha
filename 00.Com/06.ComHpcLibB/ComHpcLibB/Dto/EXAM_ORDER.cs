namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class EXAM_ORDER : BaseDto
    {
        
        /// <summary>
        /// 입원,외래(I.입원 O.외래)
        /// </summary>
		public string IPDOPD { get; set; } 

        /// <summary>
        /// 진료일자
        /// </summary>
		public DateTime? BDATE { get; set; } 

        /// <summary>
        /// 수납일자
        /// </summary>
		public DateTime? ACTDATE { get; set; } 

        /// <summary>
        /// 등록번호
        /// </summary>
		public string PANO { get; set; } 

        /// <summary>
        /// 환자종류
        /// </summary>
		public string BI { get; set; } 

        /// <summary>
        /// 환자성명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 나이
        /// </summary>
		public long AGE { get; set; } 

        /// <summary>
        /// 유아 개월수
        /// </summary>
		public long AGEMM { get; set; } 

        /// <summary>
        /// 성별(M:남, F:여)
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 진료과
        /// </summary>
		public string DEPTCODE { get; set; } 

        /// <summary>
        /// 의사코드
        /// </summary>
		public string DRCODE { get; set; } 

        /// <summary>
        /// 병동
        /// </summary>
		public string WARD { get; set; } 

        /// <summary>
        /// 병실
        /// </summary>
		public long ROOM { get; set; } 

        /// <summary>
        /// 검사코드(외래일때는 수가코드)
        /// </summary>
		public string MASTERCODE { get; set; } 

        /// <summary>
        /// 수가코드 수량(외래에서만 사용)
        /// </summary>
		public long QTY { get; set; } 

        /// <summary>
        /// 검체코드
        /// </summary>
		public string SPECCODE { get; set; } 

        /// <summary>
        /// 응급여부(S.응급 E.STAT, R.Routine)
        /// </summary>
		public string STRT { get; set; } 

        /// <summary>
        /// 의사 Comment
        /// </summary>
		public string DRCOMMENT { get; set; } 

        /// <summary>
        /// 검체번호(연속검사10회까지 10Byte*10개)
        /// </summary>
		public string SPECNO { get; set; } 

        /// <summary>
        /// 취소사유
        /// </summary>
		public string CANCEL { get; set; } 

        /// <summary>
        /// 입원검사 ORDER번호
        /// </summary>
		public long ORDERNO { get; set; } 

        /// <summary>
        /// 검사지시 일시(2005/08/25부터)
        /// </summary>
		public DateTime? ORDERDATE { get; set; } 

        /// <summary>
        /// 오더전송 일시(2005/08/25부터)
        /// </summary>
		public DateTime? SENDDATE { get; set; } 

        /// <summary>
        /// 예약일자 ( 2012/04/01 부터)-외래
        /// </summary>
		public DateTime? RDATE { get; set; } 

        /// <summary>
        /// 취소일자 ( 2012/04/01 부터)-외래
        /// </summary>
		public DateTime? CDATE { get; set; } 

        /// <summary>
        /// 수납환불일자 ( 2012/04/01 부터)-외래
        /// </summary>
		public DateTime? SDATE { get; set; } 

        /// <summary>
        /// 조
        /// </summary>
		public string PART { get; set; } 

        /// <summary>
        /// 접수일자(바코드발행일) (2012/04/01 부터)-외래
        /// </summary>
		public DateTime? JDATE { get; set; } 

        /// <summary>
        /// 검사실 검수자 사번
        /// </summary>
		public string PART2 { get; set; } 

        /// <summary>
        /// 검사실 검수일자
        /// </summary>
		public DateTime? GDATE { get; set; } 

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
        /// 외래,입원 검사 ORDER 내역
        /// </summary>
        public EXAM_ORDER()
        {
        }
    }
}
