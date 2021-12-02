namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_X_MUNJIN : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 접수일자
        /// </summary>
		public DateTime? JEPDATE { get; set; } 

        /// <summary>
        /// 진단구분 (1.신규,2.정기)
        /// </summary>
		public string JINGBN { get; set; } 

        /// <summary>
        /// 방사선 피폭증상의 유무
        /// </summary>
		public string XP1 { get; set; } 

        /// <summary>
        /// 피폭방사선 종류
        /// </summary>
		public string XPJONG { get; set; } 

        /// <summary>
        /// 방사선 작업 장소
        /// </summary>
		public string XPLACE { get; set; } 

        /// <summary>
        /// 방사선 작업내용
        /// </summary>
		public string XREMARK { get; set; } 

        /// <summary>
        /// 방사선 작업기간
        /// </summary>
		public string XTERM { get; set; } 

        /// <summary>
        /// 방사선작업종사기간
        /// </summary>
		public string XTERM1 { get; set; } 

        /// <summary>
        /// 방사선집적선량
        /// </summary>
		public string XMUCH { get; set; } 

        /// <summary>
        /// 기타 방사선에 의한 피폭증상
        /// </summary>
		public string XJUNGSAN { get; set; } 

        /// <summary>
        /// 말초혈액중의 혈색소량
        /// </summary>
		public string MUN1 { get; set; } 

        /// <summary>
        /// 증상 : 말초혈액중의 백혈구상
        /// </summary>
		public string JUNGSAN1 { get; set; } 

        /// <summary>
        /// 증상 : 눈
        /// </summary>
		public string JUNGSAN2 { get; set; } 

        /// <summary>
        /// 증상 : 피부
        /// </summary>
		public string JUNGSAN3 { get; set; } 

        /// <summary>
        /// 최종 작업 시간
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 최종 작업자
        /// </summary>
		public long JOBSABUN { get; set; } 

        /// <summary>
        /// 상태
        /// </summary>
		public string STS { get; set; } 

        /// <summary>
        /// 판정의사번호
        /// </summary>
		public long PANJENGDRNO { get; set; } 

        /// <summary>
        /// 판정일자
        /// </summary>
		public DateTime? PANJENGDATE { get; set; } 

        /// <summary>
        /// 통보일자
        /// </summary>
		public DateTime? TONGBODATE { get; set; } 

        /// <summary>
        /// 판정
        /// </summary>
		public string PANJENG { get; set; } 

        /// <summary>
        /// 인쇄구분
        /// </summary>
		public string GBPRINT { get; set; } 

        /// <summary>
        /// 판정
        /// </summary>
		public string PAN { get; set; } 

        /// <summary>
        /// 조치내역
        /// </summary>
		public string JOCHI { get; set; } 

        /// <summary>
        /// 소견내역
        /// </summary>
		public string SOGEN { get; set; } 

        /// <summary>
        /// 문진의사명
        /// </summary>
		public long MUNDRNO { get; set; } 

        /// <summary>
        /// 업무적합성여부(**) 공단코드:13
        /// </summary>
		public string WORKYN { get; set; } 

        /// <summary>
        /// 사후관리코드(**)   공단코드:32
        /// </summary>
		public string SAHUCODE { get; set; } 

        /// <summary>
        /// 상담내용
        /// </summary>
		public string SANGDAM { get; set; } 

        /// <summary>
        /// 프린트 작업사번
        /// </summary>
		public long PRTSABUN { get; set; } 

        /// <summary>
        /// 과거질병력(0:없음, 1:있음) - 2013.11.25 추가
        /// </summary>
		public string JILBYUNG { get; set; } 

        /// <summary>
        /// 혈액관련질환(빈혈)    - 2013.11.25 추가
        /// </summary>
		public string BLOOD1 { get; set; } 

        /// <summary>
        /// 혈액관련질환(백혈병)  - 2013.11.25 추가
        /// </summary>
		public string BLOOD2 { get; set; } 

        /// <summary>
        /// 혈액관련질환(기타)    - 2013.11.25 추가
        /// </summary>
		public string BLOOD3 { get; set; } 

        /// <summary>
        /// 피부질환(아토피)      - 2013.11.25 추가
        /// </summary>
		public string SKIN1 { get; set; } 

        /// <summary>
        /// 피부질환(습진)        - 2013.11.25 추가
        /// </summary>
		public string SKIN2 { get; set; } 

        /// <summary>
        /// 피부질환(기타)        - 2013.11.25 추가
        /// </summary>
		public string SKIN3 { get; set; } 

        /// <summary>
        /// 신경계질환(질환명)    - 2013.11.25 추가
        /// </summary>
		public string NERVOUS1 { get; set; } 

        /// <summary>
        /// 눈질환(백내장)        - 2013.11.25 추가
        /// </summary>
		public string EYE1 { get; set; } 

        /// <summary>
        /// 눈질환(기타)          - 2013.11.25 추가
        /// </summary>
		public string EYE2 { get; set; } 

        /// <summary>
        /// 암(질환명)            - 2013.11.25 추가
        /// </summary>
		public string CANCER1 { get; set; } 

        /// <summary>
        /// 가족력(0:없음, 1:있음) - 2013.11.25 추가
        /// </summary>
		public string GAJOK { get; set; } 

        /// <summary>
        /// 혈액관련질환명        - 2013.11.25 추가
        /// </summary>
		public string BLOOD { get; set; } 

        /// <summary>
        /// 신경계질환명          - 2013.11.25 추가
        /// </summary>
		public string NERVOUS2 { get; set; } 

        /// <summary>
        /// 암 질환명             - 2013.11.25 추가
        /// </summary>
		public string CANCER2 { get; set; } 

        /// <summary>
        /// 최근 특이증상         - 2013.11.25 추가
        /// </summary>
		public string SYMPTON { get; set; } 

        /// <summary>
        /// 현재직종(비파괴검사)  - 2013.11.25 추가
        /// </summary>
		public string JIKJONG1 { get; set; } 

        /// <summary>
        /// 현재직종(방사선사)    - 2013.11.25 추가
        /// </summary>
		public string JIKJONG2 { get; set; } 

        /// <summary>
        /// 현재직종(기타)        - 2013.11.25 추가
        /// </summary>
		public string JIKJONG3 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string REEXAM { get; set; }

        public string RID { get; set; }
        


        /// <summary>
        /// 방사선 작업종사자 문진 및 판정
        /// </summary>
        public HIC_X_MUNJIN()
        {
        }
    }
}
