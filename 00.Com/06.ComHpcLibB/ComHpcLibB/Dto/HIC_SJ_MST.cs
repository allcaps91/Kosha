namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_SJ_MST : BaseDto
    {
        
        /// <summary>
        /// 검진년도
        /// </summary>
		public string GJYEAR { get; set; } 

        /// <summary>
        /// 회사코드
        /// </summary>
		public long LTDCODE { get; set; } 

        /// <summary>
        /// 접수(협의)일자
        /// </summary>
		public string JEPDATE { get; set; } 

        /// <summary>
        /// 1.신규사업장 2.기존사업장
        /// </summary>
		public string GBNEWLTD { get; set; } 

        /// <summary>
        /// 1.전화 2.방문
        /// </summary>
		public string GBTEL { get; set; } 

        /// <summary>
        /// 알특 인원수
        /// </summary>
		public long INWON1 { get; set; } 

        /// <summary>
        /// 특수 인원수
        /// </summary>
		public long INWON2 { get; set; } 

        /// <summary>
        /// 일반 인원수
        /// </summary>
		public long INWON3 { get; set; } 

        /// <summary>
        /// 사전조사 접수자
        /// </summary>
		public string JEPNAME { get; set; } 

        /// <summary>
        /// 검진의뢰자
        /// </summary>
		public string DAMNAME { get; set; } 

        /// <summary>
        /// 협의 주요내용
        /// </summary>
		public string JOSAREMARK { get; set; } 

        /// <summary>
        /// 검진실시 시작 예정일
        /// </summary>
		public string RFDATE { get; set; } 

        /// <summary>
        /// 검진실시 종료 예정일
        /// </summary>
		public string RTDATE { get; set; } 

        /// <summary>
        /// 검진장소: 내원여부(Y/N)
        /// </summary>
		public string PLACE1 { get; set; } 

        /// <summary>
        /// 검진장소: 출장여부(Y/N)
        /// </summary>
		public string PLACE2 { get; set; } 

        /// <summary>
        /// 출장장소(1.회의실 2.식당 3.기타)
        /// </summary>
		public string PLACE3 { get; set; } 

        /// <summary>
        /// 검진명단수령일자
        /// </summary>
		public string MDATE { get; set; } 

        /// <summary>
        /// 검진준비물 우편수령 여부(Y/N)
        /// </summary>
		public string GBMAIL1 { get; set; } 

        /// <summary>
        /// 검진준비물 내월수령 여부(Y/N)
        /// </summary>
		public string GBMAIL2 { get; set; } 

        /// <summary>
        /// 기타확인사항(MSDS)(Y/N)
        /// </summary>
		public string GBJUNBI1 { get; set; } 

        /// <summary>
        /// 기타확인사항(작업환경측정결과)(Y/N)
        /// </summary>
		public string GBJUNBI2 { get; set; } 

        /// <summary>
        /// 기타확인사항(특수건강진단결과)(Y/N)
        /// </summary>
		public string GBJUNBI3 { get; set; } 

        /// <summary>
        /// 기타확인사항(근무및조직도)(Y/N)
        /// </summary>
		public string GBJUNBI4 { get; set; } 

        /// <summary>
        /// 이전 검진기관
        /// </summary>
		public string OLDHOSPITAL { get; set; } 

        /// <summary>
        /// 이전 검진시작일자
        /// </summary>
		public string OLDFDATE { get; set; } 

        /// <summary>
        /// 이전 검진종료일자
        /// </summary>
		public string OLDTDATE { get; set; } 

        /// <summary>
        /// 이전 일특 인원수
        /// </summary>
		public long OLDINWON1 { get; set; } 

        /// <summary>
        /// 이전 특수 인원수
        /// </summary>
		public long OLDINWON2 { get; set; } 

        /// <summary>
        /// 이전 일반 인원수
        /// </summary>
		public long OLDINWON3 { get; set; } 

        /// <summary>
        /// 최근 측정기관
        /// </summary>
		public string CHUKHOSPITAL { get; set; } 

        /// <summary>
        /// 최근 측정일자
        /// </summary>
		public string CHUKDATE1 { get; set; } 

        /// <summary>
        /// 이전 측정일자
        /// </summary>
		public string CHUKDATE2 { get; set; } 

        /// <summary>
        /// 노출기준 초과여부
        /// </summary>
		public string CHUKNOCHUL { get; set; } 

        /// <summary>
        /// 초과 공정
        /// </summary>
		public string CHUKCHOGWA { get; set; } 

        /// <summary>
        /// 주요 유해인자
        /// </summary>
		public string CHUKYUHE { get; set; } 

        /// <summary>
        /// 최근 변경사항1
        /// </summary>
		public string REMARK1 { get; set; } 

        /// <summary>
        /// 최근 변경사항2
        /// </summary>
		public string REMARK2 { get; set; } 

        /// <summary>
        /// 최종 등록/변경 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 최종 등록/변경 일시
        /// </summary>
		public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
		public string RID { get; set; }
        

        /// <summary>
        /// 건강진단 사전조사표 마스타
        /// </summary>
        public HIC_SJ_MST()
        {
        }
    }
}
