namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_CHUKMST : BaseDto
    {
        
        /// <summary>
        /// 마스타 일련번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 측정년도
        /// </summary>
		public string YEAR { get; set; } 

        /// <summary>
        /// 반기(1.상반기 2.하반기)
        /// </summary>
		public string BANGI { get; set; } 

        /// <summary>
        /// 회사코드
        /// </summary>
		public long LTDCODE { get; set; } 

        /// <summary>
        /// 측정인원
        /// </summary>
		public long INWON { get; set; } 

        /// <summary>
        /// 시작일자
        /// </summary>
		public string SDATE { get; set; } 

        /// <summary>
        /// 종료일자
        /// </summary>
		public string EDATE { get; set; } 

        /// <summary>
        /// 측정일수
        /// </summary>
		public long ILSU { get; set; } 

        /// <summary>
        /// 전회측정일
        /// </summary>
		public string OLDDATE { get; set; } 

        /// <summary>
        /// 측정시작시간
        /// </summary>
		public string STIME { get; set; } 

        /// <summary>
        /// 측정마침시간
        /// </summary>
		public string ETIME { get; set; } 

        /// <summary>
        /// 소요일수
        /// </summary>
		public long SOILSU { get; set; } 

        /// <summary>
        /// 소요시간(시간)
        /// </summary>
		public long SOTIME1 { get; set; } 

        /// <summary>
        /// 소요시간(분)
        /// </summary>
		public long SOTIME2 { get; set; } 

        /// <summary>
        /// 작업장 기온
        /// </summary>
		public string TEMPERATURE { get; set; } 

        /// <summary>
        /// 작업장 습도
        /// </summary>
		public string HUMIDITY { get; set; } 

        /// <summary>
        /// 측정금액
        /// </summary>
		public long AMT1 { get; set; } 

        /// <summary>
        /// 분석금액
        /// </summary>
		public long AMT2 { get; set; } 

        /// <summary>
        /// 미수등록일자
        /// </summary>
		public string MISUDATE { get; set; } 

        /// <summary>
        /// 미수등록번호
        /// </summary>
		public long MISUNO { get; set; } 

        /// <summary>
        /// 최종작업자사번
        /// </summary>
		public long JOBSABUN { get; set; } 

        /// <summary>
        /// 최종등록일시
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE2 { get; set; } 

        
        /// <summary>
        /// 작업환경측정 마스타
        /// </summary>
        public HIC_CHUKMST()
        {
        }
    }
}
