namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_SCHOOL_NEW : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 주민번호
        /// </summary>
		public string JUMIN { get; set; } 

        /// <summary>
        /// 성명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 검사일자
        /// </summary>
		public string SDATE { get; set; } 

        /// <summary>
        /// 판정일자
        /// </summary>
		public string RDATE { get; set; } 

        /// <summary>
        /// 성별
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 구분(1:초등,2:중,고등)
        /// </summary>
		public string GBN { get; set; } 

        /// <summary>
        /// 학년
        /// </summary>
		public long CLASS { get; set; } 

        /// <summary>
        /// 학반
        /// </summary>
		public long BAN { get; set; } 

        /// <summary>
        /// 학번
        /// </summary>
		public long BUN { get; set; } 

        /// <summary>
        /// 회사코드
        /// </summary>
		public long LTDCODE { get; set; } 

        /// <summary>
        /// 구강문진-1.치아부러짐
        /// </summary>
		public string DMUN1 { get; set; } 

        /// <summary>
        /// 구강문진-2.
        /// </summary>
		public string DMUN2 { get; set; } 

        /// <summary>
        /// 구강문진-3.
        /// </summary>
		public string DMUN3 { get; set; } 

        /// <summary>
        /// 구강문진-4.
        /// </summary>
		public string DMUN4 { get; set; } 

        /// <summary>
        /// 구강문진-5.
        /// </summary>
		public string DMUN5 { get; set; } 

        /// <summary>
        /// 구강문진-6.
        /// </summary>
		public string DMUN6 { get; set; } 

        /// <summary>
        /// 구강문진-7.
        /// </summary>
		public string DMUN7 { get; set; } 

        /// <summary>
        /// 구강문진-8.
        /// </summary>
		public string DMUN8 { get; set; } 

        /// <summary>
        /// 구강문진-9.
        /// </summary>
		public string DMUN9 { get; set; } 

        /// <summary>
        /// 구강문진-10.
        /// </summary>
		public string DMUN10 { get; set; } 

        /// <summary>
        /// 구강문진 의사에게 하고싶은 말
        /// </summary>
		public string DMUNREMARK { get; set; } 

        /// <summary>
        /// 구강판정-우식치아
        /// </summary>
		public string DPAN1 { get; set; } 

        /// <summary>
        /// 구강판정-우식발생위험치아
        /// </summary>
		public string DPAN2 { get; set; } 

        /// <summary>
        /// 구강판정-결손치아
        /// </summary>
		public string DPAN3 { get; set; } 

        /// <summary>
        /// 구강판정-구내염및연조직질환
        /// </summary>
		public string DPAN4 { get; set; } 

        /// <summary>
        /// 구강판정-부정교합
        /// </summary>
		public string DPAN5 { get; set; } 

        /// <summary>
        /// 구강판정-구강위생상태
        /// </summary>
		public string DPAN6 { get; set; } 

        /// <summary>
        /// 구강판정-그밖의치아상태
        /// </summary>
		public string DPAN7 { get; set; } 

        /// <summary>
        /// 구강판정-치주질환 -중고등항목
        /// </summary>
		public string DPAN8 { get; set; } 

        /// <summary>
        /// 구강판정-악관절이상-중고등항목
        /// </summary>
		public string DPAN9 { get; set; } 

        /// <summary>
        /// 구강판정-치아마모증-고등항목
        /// </summary>
		public string DPAN10 { get; set; } 

        /// <summary>
        /// 구강판정-제3대구치(사랑니)-고등항목
        /// </summary>
		public string DPAN11 { get; set; } 

        /// <summary>
        /// 구강판정 종합소견
        /// </summary>
		public string DPANSOGEN { get; set; } 

        /// <summary>
        /// 구강판정 조치사항
        /// </summary>
		public string DPANJOCHI { get; set; } 

        /// <summary>
        /// 구강판정 의사
        /// </summary>
		public long DPANDRNO { get; set; } 

        /// <summary>
        /// 문진-고혈압,뇌졸증
        /// </summary>
		public string PMUNA1 { get; set; } 

        /// <summary>
        /// 문진-협심증,심근경색,심부전
        /// </summary>
		public string PMUNA2 { get; set; } 

        /// <summary>
        /// 문진-당뇨병
        /// </summary>
		public string PMUNA3 { get; set; } 

        /// <summary>
        /// 문진-암
        /// </summary>
		public string PMUNA4 { get; set; } 

        /// <summary>
        /// 문진-간질환
        /// </summary>
		public string PMUNA5 { get; set; } 

        /// <summary>
        /// 문진-결핵
        /// </summary>
		public string PMUNA6 { get; set; } 

        /// <summary>
        /// 문진-정신질환
        /// </summary>
		public string PMUNA7 { get; set; } 

        /// <summary>
        /// 문진-소화기계
        /// </summary>
		public string PMUNB1 { get; set; } 

        /// <summary>
        /// 문진-호흡기계
        /// </summary>
		public string PMUNB2 { get; set; } 

        /// <summary>
        /// 문진-눈귀
        /// </summary>
		public string PMUNB3 { get; set; } 

        /// <summary>
        /// 문진-피부
        /// </summary>
		public string PMUNB4 { get; set; } 

        /// <summary>
        /// 문진-순환기계
        /// </summary>
		public string PMUNB5 { get; set; } 

        /// <summary>
        /// 문진-근골격계
        /// </summary>
		public string PMUNB6 { get; set; } 

        /// <summary>
        /// 문진-그밖의질환
        /// </summary>
		public string PMUNB7 { get; set; } 

        /// <summary>
        /// 문진-전신상태
        /// </summary>
		public string PMUNC1 { get; set; } 

        /// <summary>
        /// 문진-호흡기
        /// </summary>
		public string PMUNC2 { get; set; } 

        /// <summary>
        /// 문진-순환기
        /// </summary>
		public string PMUNC3 { get; set; } 

        /// <summary>
        /// 문진-소화기
        /// </summary>
		public string PMUNC4 { get; set; } 

        /// <summary>
        /// 문진-정신건강
        /// </summary>
		public string PMUNC5 { get; set; } 

        /// <summary>
        /// 문진-혈액
        /// </summary>
		public string PMUNC6 { get; set; } 

        /// <summary>
        /// 문진-그밖의 증상
        /// </summary>
		public string PMUNC7 { get; set; } 

        /// <summary>
        /// 문진-식생활
        /// </summary>
		public string PMUND1 { get; set; } 

        /// <summary>
        /// 문진-개인위생
        /// </summary>
		public string PMUND2 { get; set; } 

        /// <summary>
        /// 문진-안전,운동
        /// </summary>
		public string PMUND3 { get; set; } 

        /// <summary>
        /// 문진-TV,인터넷
        /// </summary>
		public string PMUND4 { get; set; } 

        /// <summary>
        /// 문진-가정 및 학교생활,인터넷
        /// </summary>
		public string PMUND5 { get; set; } 

        /// <summary>
        /// 문진-약물,가정및학교생활
        /// </summary>
		public string PMUND6 { get; set; } 

        /// <summary>
        /// 문진-정서,약물
        /// </summary>
		public string PMUND7 { get; set; } 

        /// <summary>
        /// 문진-부모님,성
        /// </summary>
		public string PMUND8 { get; set; } 

        /// <summary>
        /// 의사선생님에게 하고싶은말
        /// </summary>
		public string PMUNREMARK1 { get; set; } 

        /// <summary>
        /// 상담을 받고 싶다
        /// </summary>
		public string PMUNREMARK2 { get; set; } 

        /// <summary>
        /// 신체발달-키
        /// </summary>
		public string PPANA1 { get; set; } 

        /// <summary>
        /// -몸무게
        /// </summary>
		public string PPANA2 { get; set; } 

        /// <summary>
        /// 비만도-체질량지수
        /// </summary>
		public string PPANA3 { get; set; } 

        /// <summary>
        /// 비만도-상대체중
        /// </summary>
		public string PPANA4 { get; set; } 

        /// <summary>
        /// 판정-근골격 및 척추
        /// </summary>
		public string PPANB1 { get; set; } 

        /// <summary>
        /// 근골격 및 척추 기타
        /// </summary>
		public string PPANB2 { get; set; } 

        /// <summary>
        /// 눈-나안시력
        /// </summary>
		public string PPANC1 { get; set; } 

        /// <summary>
        /// 눈-교정시력
        /// </summary>
		public string PPANC2 { get; set; } 

        /// <summary>
        /// 눈-색각유무
        /// </summary>
		public string PPANC3 { get; set; } 

        /// <summary>
        /// 눈-안질환
        /// </summary>
		public string PPANC4 { get; set; } 

        /// <summary>
        /// 귀-청력
        /// </summary>
		public string PPANC5 { get; set; } 

        /// <summary>
        /// 귀-귓병
        /// </summary>
		public string PPANC6 { get; set; } 

        /// <summary>
        /// 콧병
        /// </summary>
		public string PPANC7 { get; set; } 

        /// <summary>
        /// 판목병
        /// </summary>
		public string PPANC8 { get; set; } 

        /// <summary>
        /// 피부병
        /// </summary>
		public string PPANC9 { get; set; } 

        /// <summary>
        /// 기관능력-호흡기
        /// </summary>
		public string PPAND1 { get; set; } 

        /// <summary>
        /// -순환기
        /// </summary>
		public string PPAND2 { get; set; } 

        /// <summary>
        /// -비뇨기
        /// </summary>
		public string PPAND3 { get; set; } 

        /// <summary>
        /// -소화기
        /// </summary>
		public string PPAND4 { get; set; } 

        /// <summary>
        /// -신경계
        /// </summary>
		public string PPAND5 { get; set; } 

        /// <summary>
        /// -기타
        /// </summary>
		public string PPAND6 { get; set; } 

        /// <summary>
        /// 소변검사-요단백
        /// </summary>
		public string PPANE1 { get; set; } 

        /// <summary>
        /// -요잠혈
        /// </summary>
		public string PPANE2 { get; set; } 

        /// <summary>
        /// -요당
        /// </summary>
		public string PPANE3 { get; set; } 

        /// <summary>
        /// -요PH
        /// </summary>
		public string PPANE4 { get; set; } 

        /// <summary>
        /// 혈액검사-혈당(식전)
        /// </summary>
		public string PPANF1 { get; set; } 

        /// <summary>
        /// -총콜레스테롤
        /// </summary>
		public string PPANF2 { get; set; } 

        /// <summary>
        /// -AST
        /// </summary>
		public string PPANF3 { get; set; } 

        /// <summary>
        /// -ALT
        /// </summary>
		public string PPANF4 { get; set; } 

        /// <summary>
        /// -혈색소
        /// </summary>
		public string PPANF5 { get; set; } 

        /// <summary>
        /// -혈액형검사
        /// </summary>
		public string PPANF6 { get; set; } 

        /// <summary>
        /// 흉부방사선검사
        /// </summary>
		public string PPANG1 { get; set; } 

        /// <summary>
        /// 간염검사
        /// </summary>
		public string PPANH1 { get; set; } 

        /// <summary>
        /// 혈압
        /// </summary>
		public string PPANJ1 { get; set; } 

        /// <summary>
        /// 진촬및상담-과거병력
        /// </summary>
		public string PPANK1 { get; set; } 

        /// <summary>
        /// -생활습관
        /// </summary>
		public string PPANK2 { get; set; } 

        /// <summary>
        /// -외상및후유증
        /// </summary>
		public string PPANK3 { get; set; } 

        /// <summary>
        /// -일반상태
        /// </summary>
		public string PPANK4 { get; set; } 

        /// <summary>
        /// 판정의사
        /// </summary>
		public long PPANDRNO { get; set; } 

        /// <summary>
        /// 최종작업일자
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 최종작업자
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 구강인쇄구분
        /// </summary>
		public string GBDNTPRT { get; set; } 

        /// <summary>
        /// 판정인쇄구분
        /// </summary>
		public string GBPANPRT { get; set; } 

        /// <summary>
        /// 종합판정(1.정상A,2.정상B,3.질환의심)
        /// </summary>
		public string GBPAN { get; set; } 

        /// <summary>
        /// 문진-상담받고 싶다
        /// </summary>
		public string PMUND9 { get; set; } 

        /// <summary>
        /// 종합판정
        /// </summary>
		public string PPANREMARK1 { get; set; } 

        /// <summary>
        /// 조치사항
        /// </summary>
		public string PPANREMARK2 { get; set; } 

        /// <summary>
        /// 청구인쇄구분
        /// </summary>
		public string GBMIRPRINT { get; set; } 

        /// <summary>
        /// 임상진찰시 참고사항(상담내역)
        /// </summary>
		public string SANGDAM { get; set; } 

        /// <summary>
        /// 구강판정일자
        /// </summary>
		public DateTime? DPANDATE { get; set; } 

        /// <summary>
        /// 치아상태-종합판정
        /// </summary>
		public string DPAN12 { get; set; } 

        /// <summary>
        /// 구강상태-종합판정
        /// </summary>
		public string DPAN13 { get; set; } 

        /// <summary>
        /// 통보일자
        /// </summary>
		public DateTime? TONGBODATE { get; set; } 

        /// <summary>
        /// 프린트 작업사번
        /// </summary>
		public long PRTSABUN { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string JUMIN2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string LTDCODE2 { get; set; }

        public string SCHPAN1 { get; set; }
        public string SCHPAN2{ get; set; }
        public string SCHPAN3{ get; set; }
        public string SCHPAN4{ get; set; }
        public string SCHPAN5{ get; set; }
        public string SCHPAN6{ get; set; }
        public string SCHPAN7{ get; set; }
        public string SCHPAN8{ get; set; }
        public string SCHPAN9 { get; set; }
        public string SCHPAN10{ get; set; }
        public string SCHPAN11 { get; set; }
        public string REMARK { get; set; }
        public string GBSTS { get; set; }
        public long SANGDAMDRNO { get; set; }

        public string ROWID { get; set; }

        public long PANJENGDRNO { get; set; }

        public string PPANF7 { get; set; }

        public string PPANF8 { get; set; }

        public string PPANF9 { get; set; }
        public string PANJENGDATE { get; set; }

        /// <summary>
        /// 2006년 학생신체검사 new 테이블
        /// </summary>
        public HIC_SCHOOL_NEW()
        {
        }
    }
}
