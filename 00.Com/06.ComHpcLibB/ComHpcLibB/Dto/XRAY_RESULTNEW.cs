namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class XRAY_RESULTNEW : BaseDto
    {
        
        /// <summary>
        /// 결과지 일련번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 등록번호
        /// </summary>
		public string PANO { get; set; } 

        /// <summary>
        /// 판독일자
        /// </summary>
		public string READDATE { get; set; } 

        /// <summary>
        /// 촬영일자
        /// </summary>
		public string SEEKDATE { get; set; } 

        /// <summary>
        /// 종류(1.일반 2.특수 3.Sono 4.CT 5.MRI 6.RI 7.BMD C.심초음파,P:EEG)
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
        /// 협진의사 사번(1)
        /// </summary>
		public long XDRCODE2 { get; set; } 

        /// <summary>
        /// 협진의사 사번(2)
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
        /// 조직검사 확인완료 여부(Y/N/NULL)
        /// </summary>
		public string GBANAT { get; set; } 

        /// <summary>
        /// progress note 자동 전송 여부
        /// </summary>
		public string SENDEMR { get; set; } 

        /// <summary>
        /// progress note 자동 전송 여부
        /// </summary>
		public long SENDEMRNO { get; set; } 

        /// <summary>
        /// 선택진료구분(1,0)
        /// </summary>
		public string GBSPC { get; set; } 

        /// <summary>
        /// 작업자 2011-08-30 추가
        /// </summary>
		public long PART { get; set; } 

        /// <summary>
        /// 추가판독 결과1
        /// </summary>
		public string ADDENDUM1 { get; set; } 

        /// <summary>
        /// 추가판독 결과2
        /// </summary>
		public string ADDENDUM2 { get; set; } 

        /// <summary>
        /// 추가판독일시
        /// </summary>
		public DateTime? ADDDATE { get; set; } 

        /// <summary>
        /// 츄가판독의사
        /// </summary>
		public long ADDDRCODE { get; set; } 

        /// <summary>
        /// 인증번호
        /// </summary>
		public long CERTNO { get; set; } 

        /// <summary>
        /// 건강검진 기준 판정값(2014.12.5 추가)
        /// </summary>
		public string PANHIC { get; set; } 

        /// <summary>
        /// 판독시각(2015-05-27)
        /// </summary>
		public DateTime? READTIME { get; set; } 

        /// <summary>
        /// 작업자2 2016-03-09 추가
        /// </summary>
		public long PART2 { get; set; } 

        /// <summary>
        /// tmep
        /// </summary>
		public string TEMP { get; set; } 

        /// <summary>
        /// 입력자
        /// </summary>
		public string INPS { get; set; } 

        /// <summary>
        /// 최초시간
        /// </summary>
		public DateTime? INPT_DT { get; set; } 

        /// <summary>
        /// 변경자
        /// </summary>
		public string UPPS { get; set; } 

        /// <summary>
        /// 변경시간
        /// </summary>
		public DateTime? UP_DT { get; set; } 

        
        /// <summary>
        /// 방사선 판독결과 자료
        /// </summary>
        public XRAY_RESULTNEW()
        {
        }
    }
}
