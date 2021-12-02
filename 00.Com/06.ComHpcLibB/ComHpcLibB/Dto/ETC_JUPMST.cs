namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class ETC_JUPMST : BaseDto
    {

        /// <summary>
        /// 발생일자
        /// </summary>
        public string BDATE { get; set; }

        /// <summary>
        /// 등록번호
        /// </summary>
        public string PTNO { get; set; }

        /// <summary>
        /// 성명
        /// </summary>
        public string SNAME { get; set; }

        /// <summary>
        /// 성별
        /// </summary>
        public string SEX { get; set; }

        /// <summary>
        /// 나이
        /// </summary>
        public long AGE { get; set; }

        /// <summary>
        /// Order코드
        /// </summary>
        public string ORDERCODE { get; set; }

        /// <summary>
        /// Order번호
        /// </summary>
        public long ORDERNO { get; set; }

        /// <summary>
        /// 입원외래구분
        /// </summary>
        public string GBIO { get; set; }

        /// <summary>
        /// 분류코드
        /// </summary>
        public string BUN { get; set; }

        /// <summary>
        /// 과코드
        /// </summary>
        public string DEPTCODE { get; set; }

        /// <summary>
        /// 의사코드
        /// </summary>
        public string DRCODE { get; set; }

        /// <summary>
        /// 비고
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 구분(1.미접수 2.예약 3.접수,완료 9.취소)
        /// </summary>
        public string GBJOB { get; set; }

        /// <summary>
        /// 예약일자
        /// </summary>
        public string RDATE { get; set; }

        /// <summary>
        /// 금액
        /// </summary>
        public long AMT { get; set; }

        /// <summary>
        /// 입력일자 + 시간
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 응급구분
        /// </summary>
        public string GBER { get; set; }

        /// <summary>
        /// 병실
        /// </summary>
        public string ROOMCODE { get; set; }

        /// <summary>
        /// ** 아래 참고
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// Portable    M : Portable
        /// </summary>
        public string GBPORT { get; set; }

        /// <summary>
        /// 시행의사(심전도실에서 입력)
        /// </summary>
        public string RESULTDRSABUN { get; set; }

        /// <summary>
        /// 오더시간(2005/08/25부터)
        /// </summary>
        public DateTime? ORDERDATE { get; set; }

        /// <summary>
        /// 전송시간(2005/08/25부터)
        /// </summary>
        public DateTime? SENDDATE { get; set; }

        /// <summary>
        /// 예약검사 일련번호
        /// </summary>
        public long EXAM_WRTNO { get; set; }

        /// <summary>
        /// 판독번호
        /// </summary>
        public long READ_WRTNO { get; set; }

        /// <summary>
        /// 결과 이미지
        /// </summary>
        public byte[] IMAGE { get; set; } 

        /// <summary>
        /// 이미지 크기
        /// </summary>
		public long IMAGE_SIZE { get; set; } 

        /// <summary>
        /// 건진번호 : hic_jepsu 의 wrtno로 사용
        /// </summary>
		public long TREATNO { get; set; } 

        /// <summary>
        /// 사용안함
        /// </summary>
		public string FILEPATH { get; set; } 

        /// <summary>
        /// 접수대기(*)
        /// </summary>
		public string GBSELECT { get; set; } 

        /// <summary>
        /// ECG 확인(*)
        /// </summary>
		public string GBECG { get; set; } 

        /// <summary>
        /// 카덱스 간호사 수동작업 (삭제 : D, 수동확인 : 1 =>하단 확인일자에 추가)
        /// </summary>
		public string CADEX_DEL { get; set; } 

        /// <summary>
        /// 확인일자시간
        /// </summary>
		public DateTime? CDATE { get; set; } 

        /// <summary>
        /// 확인자
        /// </summary>
		public long CSABUN { get; set; } 

        /// <summary>
        /// 인터페이스 전송 구분(아래참조)
        /// </summary>
		public string IMAGE_GBN { get; set; } 

        /// <summary>
        /// 스트레스소견
        /// </summary>
		public string STRESS_SOGEN { get; set; } 

        /// <summary>
        /// FTP 전송여부(Y)
        /// </summary>
		public string GBFTP { get; set; } 

        /// <summary>
        /// 도착시각여부(2015-01-21)
        /// </summary>
		public DateTime? ARRDATE { get; set; } 

        /// <summary>
        /// 검사시작시간(2015-01-21)
        /// </summary>
		public DateTime? STARTDATE { get; set; } 

        /// <summary>
        /// 예약확정 및 변경시간(2015-01-21)
        /// </summary>
		public DateTime? CRDATE { get; set; } 

        /// <summary>
        /// 구분2(심초음파 구분: 0,1)
        /// </summary>
		public string GUBUN2 { get; set; } 

        /// <summary>
        /// 도착 Y,N
        /// </summary>
		public string GUBUN3 { get; set; } 

        /// <summary>
        /// 미국마취 신체등급 BAS_BCODE GUBUN=`마취_신체등급(ASA)`
        /// </summary>
		public string ASA { get; set; } 

        /// <summary>
        /// 심전도,심초음파 응급CVR 대상 : Y
        /// </summary>
		public string CVR { get; set; } 

        /// <summary>
        /// 심전도,심초음파 응급CVR 대상 상세
        /// </summary>
		public string CVRDETAIL { get; set; } 

        /// <summary>
        /// CP배부 구분
        /// </summary>
		public string CP { get; set; } 

        /// <summary>
        /// ECG파일 교차점검 여부(Y=>파일정상)
        /// </summary>
		public string CHECK_FILE { get; set; }

        public string ROWID { get; set; }


        /// <summary>
        /// 기능부서 오더 접수 마스타
        /// </summary>
        public ETC_JUPMST()
        {
        }
    }
}
