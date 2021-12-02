namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class XRAY_RESULTNEW_DR : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long DRWRTNO { get; set; } 

        /// <summary>
        /// 등록번호
        /// </summary>
		public string PANO { get; set; } 

        /// <summary>
        /// 판독일자
        /// </summary>
		public DateTime? READDATE { get; set; } 

        /// <summary>
        /// 촬영일자
        /// </summary>
		public DateTime? SEEKDATE { get; set; } 

        /// <summary>
        /// 종류(1.일반 2.특수 3.Sono 4.CT 5.MRI 6.RI 7.BMD C.심초음파)
        /// </summary>
		public string XJONG { get; set; } 

        /// <summary>
        /// 성명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 성별(M:남 F:여자)
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 나이
        /// </summary>
		public long AGE { get; set; } 

        /// <summary>
        /// 외래입원(I,O)
        /// </summary>
		public string IPDOPD { get; set; } 

        /// <summary>
        /// 의뢰과
        /// </summary>
		public string DEPTCODE { get; set; } 

        /// <summary>
        /// 의뢰의사
        /// </summary>
		public string DRCODE { get; set; } 

        /// <summary>
        /// 병동
        /// </summary>
		public string WARDCODE { get; set; } 

        /// <summary>
        /// 병실
        /// </summary>
		public string ROOMCODE { get; set; } 

        /// <summary>
        /// 판독의사 사번
        /// </summary>
		public long XDRCODE1 { get; set; } 

        /// <summary>
        /// 협진의사 사번(1)*사용않함*
        /// </summary>
		public long XDRCODE2 { get; set; } 

        /// <summary>
        /// 협진의사 사번(2)*사용않함*
        /// </summary>
		public long XDRCODE3 { get; set; } 

        /// <summary>
        /// 상병코드(1)
        /// </summary>
		public string ILLCODE1 { get; set; } 

        /// <summary>
        /// 상병코드(2)
        /// </summary>
		public string ILLCODE2 { get; set; } 

        /// <summary>
        /// 상병코드(3)
        /// </summary>
		public string ILLCODE3 { get; set; } 

        /// <summary>
        /// 의뢰 방사선코드(주 코드 1개만 보관됨)
        /// </summary>
		public string XCODE { get; set; } 

        /// <summary>
        /// 촬영명칭
        /// </summary>
		public string XNAME { get; set; } 

        /// <summary>
        /// 판독결과(1)
        /// </summary>
		public string RESULT { get; set; } 

        /// <summary>
        /// 판독결과(2)
        /// </summary>
		public string RESULT1 { get; set; } 

        /// <summary>
        /// 결과지인쇄횟수
        /// </summary>
		public long PRTCNT { get; set; } 

        /// <summary>
        /// 결과검색횟수
        /// </summary>
		public long VIEWCNT { get; set; } 

        /// <summary>
        /// 최종등록(변경)시각
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 판독완료여부(Y.판독완료 N.임시저장)
        /// </summary>
		public string APPROVE { get; set; } 

        /// <summary>
        /// 외주판독여부(Y.외주판독)
        /// </summary>
		public string GBOUT { get; set; } 

        /// <summary>
        /// 판독시작시간(환자선택시간)
        /// </summary>
		public DateTime? STIME { get; set; } 

        /// <summary>
        /// 판독종료시간(결과등록시간)
        /// </summary>
		public DateTime? ETIME { get; set; } 

        /// <summary>
        /// 판독(에코)1
        /// </summary>
		public string RESULTEC { get; set; } 

        /// <summary>
        /// 판독(에코)2
        /// </summary>
		public string RESULTEC1 { get; set; } 

        /// <summary>
        /// temp
        /// </summary>
		public string TEMP { get; set; } 

        
        /// <summary>
        /// 처방의 판독결과
        /// </summary>
        public XRAY_RESULTNEW_DR()
        {
        }
    }
}
