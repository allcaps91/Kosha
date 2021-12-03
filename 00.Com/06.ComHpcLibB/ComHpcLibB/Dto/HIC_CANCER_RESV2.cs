namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_CANCER_RESV2 : BaseDto
    {
        
        /// <summary>
        /// 예약일자 및 시각
        /// </summary>
		public DateTime? RTIME { get; set; }

        public DateTime? RTIME1 { get; set; }

        public DateTime? RTIME2 { get; set; }

        public DateTime? RTIMESYSDATE { get; set; }

        public string RTIME_TIME { get; set; }

        public string RDATE { get; set; }

        public string AMPM { get; set; }
        
        /// <summary>
        /// 주민등록번호
        /// </summary>
        public string JUMIN { get; set; } 

        /// <summary>
        /// 성명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 전화번호
        /// </summary>
		public string TEL { get; set; } 

        /// <summary>
        /// 휴대폰번호
        /// </summary>
		public string HPHONE { get; set; } 

        /// <summary>
        /// 위암 검사여부(Y/N)
        /// </summary>
		public string GBAM1 { get; set; } 

        /// <summary>
        /// 대장암 검사여부(Y/N)
        /// </summary>
		public string GBAM2 { get; set; } 

        /// <summary>
        /// 간암 검사여부(Y/N)
        /// </summary>
		public string GBAM3 { get; set; } 

        /// <summary>
        /// 유방암 검사여부(Y/N)
        /// </summary>
		public string GBAM4 { get; set; } 

        /// <summary>
        /// 위장조영(UGI) 검사여부(Y/N)
        /// </summary>
		public string GBUGI { get; set; } 

        /// <summary>
        /// 내시경검사 여부(Y/N)
        /// </summary>
		public string GBGFS { get; set; } 

        /// <summary>
        /// 유방검사 여부(Y/N)
        /// </summary>
		public string GBMAMMO { get; set; } 

        /// <summary>
        /// 분변검사 여부(Y/N)
        /// </summary>
		public string GBRECUTM { get; set; } 

        /// <summary>
        /// 초음파검사 여부(Y/N)
        /// </summary>
		public string GBSONO { get; set; } 

        /// <summary>
        /// 참고사항
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 검사접수일자
        /// </summary>
		public DateTime? JEPDATE { get; set; } 

        /// <summary>
        /// 최종 등록/변경자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 최종 등록/변경   시각
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 병원등록번호
        /// </summary>
		public string PANO { get; set; } 

        /// <summary>
        /// 자궁경부암 여부(Y/N)
        /// </summary>
		public string GBWOMB { get; set; } 

        /// <summary>
        /// 문자전송 동의(Y)
        /// </summary>
		public string SMSOK { get; set; } 

        /// <summary>
        /// 주민번호 암호화
        /// </summary>
		public string JUMIN2 { get; set; } 

        /// <summary>
        /// 1차검진(Y/N)
        /// </summary>
		public string GBBOHUM { get; set; } 

        /// <summary>
        /// 내시경검사(종검) 여부(Y/N)
        /// </summary>
		public string GBGFSH { get; set; } 

        /// <summary>
        /// 희망의사 성명
        /// </summary>
		public string SDOCT { get; set; } 

        /// <summary>
        /// 본관 대장내시경 여부(Y/N)
        /// </summary>
		public string GBCOLON { get; set; } 

        /// <summary>
        /// CT검사 여부(Y/N)
        /// </summary>
		public string GBCT { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GBLUNG_SANGDAM { get; set; } 

        public string ROWID { get; set; }

        public long LTDCODE { get; set; }
        


        /// <summary>
        /// 암건진 예약자 명단
        /// </summary>
        public HIC_CANCER_RESV2()
        {
        }
    }
}
