namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_IE_MUNJIN_NEW : BaseDto
    {
        
        /// <summary>
        /// 인터넷문진 작성일자
        /// </summary>
		public string MUNDATE { get; set; } 

        /// <summary>
        /// 성명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 생년월일(주민번호 앞6자리)
        /// </summary>
		public string BIRTH { get; set; } 

        /// <summary>
        /// 휴대폰번호
        /// </summary>
		public string HPHONE { get; set; } 

        /// <summary>
        /// 등록번호
        /// </summary>
		public string PTNO { get; set; } 

        /// <summary>
        /// 문진표 폼목록
        /// </summary>
		public string RECVFORM { get; set; } 

        /// <summary>
        /// 1차검진  여부(Y/N)
        /// </summary>
		public string GBMUN1 { get; set; } 

        /// <summary>
        /// 암검진   여부(Y/N)
        /// </summary>
		public string GBMUN2 { get; set; } 

        /// <summary>
        /// 특수검진 여부(Y/N)
        /// </summary>
		public string GBMUN3 { get; set; } 

        /// <summary>
        /// 학생검진 여부(Y/N)
        /// </summary>
		public string GBMUN4 { get; set; } 

        /// <summary>
        /// 2차검진  여부(Y/N)
        /// </summary>
		public string GBMUN5 { get; set; } 

        /// <summary>
        /// 나이
        /// </summary>
		public long AGE { get; set; } 

        /// <summary>
        /// 성별
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 우편번호
        /// </summary>
		public string MAILCODE { get; set; } 

        /// <summary>
        /// 주소1
        /// </summary>
		public string JUSO1 { get; set; } 

        /// <summary>
        /// 주소2
        /// </summary>
		public string JUSO2 { get; set; } 

        /// <summary>
        /// 회사명/학교명
        /// </summary>
		public string LTDNAME { get; set; } 

        /// <summary>
        /// 집전화번호
        /// </summary>
		public string TEL { get; set; } 

        /// <summary>
        /// 학교구분(1.초등학교 2.중학교 3.고등학교)
        /// </summary>
		public string GBSCHOOL { get; set; } 

        /// <summary>
        /// 학년
        /// </summary>
		public long CLASS { get; set; } 

        /// <summary>
        /// 반
        /// </summary>
		public long BAN { get; set; } 

        /// <summary>
        /// 번호
        /// </summary>
		public long BUN { get; set; } 

        /// <summary>
        /// 인터넷문진 결과값
        /// </summary>
		public string MUNJINRES { get; set; } 

        /// <summary>
        /// 이름,생년월일,성별,휴대폰번호 기본정보(암호화)
        /// </summary>
		public string INJEKDATA { get; set; } 

        /// <summary>
        /// 우편번호,주소1,주소2,성별,회사명,학생정보
        /// </summary>
		public string JUSODATA { get; set; } 

        /// <summary>
        /// 인터넷문진 결과값1(원본)
        /// </summary>
		public string WEBDATA1 { get; set; } 

        /// <summary>
        /// 인터넷문진 결과값2(원본)
        /// </summary>
		public string WEBDATA2 { get; set; } 

        /// <summary>
        /// 인터넷문진 결과값3(원본)
        /// </summary>
		public string WEBDATA3 { get; set; } 

        /// <summary>
        /// 문진결과 DB 전송 여부(Y/N)
        /// </summary>
		public string GBDBSEND { get; set; } 

        /// <summary>
        /// 1차검진  접수번호
        /// </summary>
		public long WRTNO1 { get; set; } 

        /// <summary>
        /// 암검진   접수번호
        /// </summary>
		public long WRTNO2 { get; set; } 

        /// <summary>
        /// 특수검진 접수번호
        /// </summary>
		public long WRTNO3 { get; set; } 

        /// <summary>
        /// 학생검진 접수번호
        /// </summary>
		public long WRTNO4 { get; set; } 

        /// <summary>
        /// 2차검진  접수번호
        /// </summary>
		public long WRTNO5 { get; set; } 

        /// <summary>
        /// 문진번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 학생검진 HTML 서버에 FTP로 전송 여부(Y/NULL)
        /// </summary>
		public string GBWEBHTML { get; set; } 

        /// <summary>
        /// 웹서버에 원본 삭제작업 여부(Y/NULL)
        /// </summary>
		public string GBWEBDELETE { get; set; } 

        /// <summary>
        /// 인터넷문진 결과값1
        /// </summary>
		public string MUNJINRES1 { get; set; }

        public string ROWID { get; set; }

        /// <summary>
        /// 인터넷 문진표(신규)
        /// </summary>
        public HIC_IE_MUNJIN_NEW()
        {
        }
    }
}
