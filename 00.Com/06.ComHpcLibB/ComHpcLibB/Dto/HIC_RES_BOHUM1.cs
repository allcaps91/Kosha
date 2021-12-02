namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_RES_BOHUM1 : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 신장(cm)
        /// </summary>
		public long HEIGHT { get; set; } 

        /// <summary>
        /// 체중(Kg)
        /// </summary>
		public long WEIGHT { get; set; } 

        /// <summary>
        /// 비만도(1.저체중,2.정상체중,3.비만1단계,4.비만2단계,5.비만3단계)
        /// </summary>
		public string BIMAN { get; set; } 

        /// <summary>
        /// 시력(좌)
        /// </summary>
		public double EYE_L { get; set; } 

        /// <summary>
        /// 시력(우)
        /// </summary>
		public double EYE_R { get; set; } 

        /// <summary>
        /// 청력(좌)
        /// </summary>
		public string EAR_L { get; set; } 

        /// <summary>
        /// 청력(우)
        /// </summary>
		public string EAR_R { get; set; } 

        /// <summary>
        /// 혈압(최고)
        /// </summary>
		public long BLOOD_H { get; set; } 

        /// <summary>
        /// 혈압(최저)
        /// </summary>
		public long BLOOD_L { get; set; } 

        /// <summary>
        /// 요검사(요당): (아래참조)
        /// </summary>
		public string URINE1 { get; set; } 

        /// <summary>
        /// 요검사(요단백): (아래참조)
        /// </summary>
		public string URINE2 { get; set; } 

        /// <summary>
        /// 요검사(요잠혈): (아래참조)
        /// </summary>
		public string URINE3 { get; set; } 

        /// <summary>
        /// 요PH
        /// </summary>
		public double URINE4 { get; set; } 

        /// <summary>
        /// 혈액검사(혈색소)
        /// </summary>
		public double BLOOD1 { get; set; } 

        /// <summary>
        /// 혈액검사(혈당:식전)
        /// </summary>
		public long BLOOD2 { get; set; } 

        /// <summary>
        /// 혈액검사(총콜레스테롤)
        /// </summary>
		public long BLOOD3 { get; set; } 

        /// <summary>
        /// 혈액검사(혈청지오티)
        /// </summary>
		public long BLOOD4 { get; set; } 

        /// <summary>
        /// 혈액검사(혈청지피티)
        /// </summary>
		public long BLOOD5 { get; set; } 

        /// <summary>
        /// 혈액검사(감마지피티)
        /// </summary>
		public long BLOOD6 { get; set; } 

        /// <summary>
        /// 간염검사(항원)
        /// </summary>
		public string LIVER1 { get; set; } 

        /// <summary>
        /// 간염검사(항체)
        /// </summary>
		public string LIVER2 { get; set; } 

        /// <summary>
        /// 간염검사 결과
        /// </summary>
		public string LIVER3 { get; set; } 

        /// <summary>
        /// 흉부방사선 구분(0.간접촬영 1.직접촬영)
        /// </summary>
		public string XRAYGBN { get; set; } 

        /// <summary>
        /// 흉부방사선 결과
        /// </summary>
		public string XRAYRES { get; set; } 

        /// <summary>
        /// 심전도검사(아래참조)
        /// </summary>
		public string EKG { get; set; } 

        /// <summary>
        /// 자궁질도말검사(유형별)
        /// </summary>
		public string CYTO1 { get; set; } 

        /// <summary>
        /// 자궁질도말검사(검사결과)
        /// </summary>
		public string CYTO2 { get; set; } 

        /// <summary>
        /// 검사결과 완료여부
        /// </summary>
		public string EXAMFLAG { get; set; } 

        /// <summary>
        /// 과거,현재 질환명①
        /// </summary>
		public string SICK11 { get; set; } 

        /// <summary>
        /// 발병년도①
        /// </summary>
		public string SICK12 { get; set; } 

        /// <summary>
        /// 치료상태① (1.완치 2.치료중)
        /// </summary>
		public string SICK13 { get; set; } 

        /// <summary>
        /// 과거,현재 질환명②
        /// </summary>
		public string SICK21 { get; set; } 

        /// <summary>
        /// 발병년도②
        /// </summary>
		public string SICK22 { get; set; } 

        /// <summary>
        /// 치료상태② (1.완치 2.치료중)
        /// </summary>
		public string SICK23 { get; set; } 

        /// <summary>
        /// 과거,현재 질환명③
        /// </summary>
		public string SICK31 { get; set; } 

        /// <summary>
        /// 발병년도③
        /// </summary>
		public string SICK32 { get; set; } 

        /// <summary>
        /// 치료상태③ (1.완치 2.치료중)
        /// </summary>
		public string SICK33 { get; set; } 

        /// <summary>
        /// 가족질환(1.간장질환)  1.없다 2.있다
        /// </summary>
		public string GAJOK1 { get; set; } 

        /// <summary>
        /// 가족질환(2.고혈압)
        /// </summary>
		public string GAJOK2 { get; set; } 

        /// <summary>
        /// 가족질환(3.뇌졸증)
        /// </summary>
		public string GAJOK3 { get; set; } 

        /// <summary>
        /// 가족질환(4.심장병)
        /// </summary>
		public string GAJOK4 { get; set; } 

        /// <summary>
        /// 가족질환(5.당뇨병)
        /// </summary>
		public string GAJOK5 { get; set; } 

        /// <summary>
        /// 가족질환(6.암)
        /// </summary>
		public string GAJOK6 { get; set; } 

        /// <summary>
        /// 의심질환(1.없다 2.있다)
        /// </summary>
		public string ROSICK { get; set; } 

        /// <summary>
        /// 의심질환(질환명)
        /// </summary>
		public string ROSICKNAME { get; set; } 

        /// <summary>
        /// 식성(1.채식 2.채식,육식 3.육식)
        /// </summary>
		public string SIKSENG { get; set; } 

        /// <summary>
        /// 음주습관(아래참조)
        /// </summary>
		public string DRINK1 { get; set; } 

        /// <summary>
        /// 1회 음주량(N.음주않함 1.반병 2.1병 3.1병반 4.2병이상)
        /// </summary>
		public string DRINK2 { get; set; } 

        /// <summary>
        /// 담배(1.피우지 않는다 2.과거에피움 3.현재 피움)
        /// </summary>
		public string SMOKING1 { get; set; } 

        /// <summary>
        /// 1일 담배량(아래참조)
        /// </summary>
		public string SMOKING2 { get; set; } 

        /// <summary>
        /// 담배를 피운 기간(아래참조)
        /// </summary>
		public string SMOKING3 { get; set; } 

        /// <summary>
        /// 일주일에 운동 횟수(아래참조)
        /// </summary>
		public string SPORTS { get; set; } 

        /// <summary>
        /// 정신적육체적 피로상태(아래참조)
        /// </summary>
		public string ANNOUNE { get; set; } 

        /// <summary>
        /// 생리기간 이상출혈(N.남자 1.예 2.아니오)
        /// </summary>
		public string WOMAN1 { get; set; } 

        /// <summary>
        /// 분비물 냄새(N.남자 1.예 2.아니오)
        /// </summary>
		public string WOMAN2 { get; set; } 

        /// <summary>
        /// 결혼연령(아래참조)
        /// </summary>
		public string WOMAN3 { get; set; } 

        /// <summary>
        /// 문진상태(N.미입력 Y.입력완료)
        /// </summary>
		public string MUNJINFLAG { get; set; } 

        /// <summary>
        /// 최종 문진 등록일자 및 시각
        /// </summary>
		public DateTime? MUNJINENTDATE { get; set; } 

        /// <summary>
        /// 최종 문진 등록자 사번
        /// </summary>
		public long MUNJINENTSABUN { get; set; } 

        /// <summary>
        /// 과거병력 유무(1.유 2.무)
        /// </summary>
		public string OLDBYENG { get; set; } 

        /// <summary>
        /// 병력유형:1.간장질환(1.해당 0.미해당)
        /// </summary>
		public string OLDBYENG1 { get; set; } 

        /// <summary>
        /// 병력유형:2.고혈압  (1.해당 0.미해당)
        /// </summary>
		public string OLDBYENG2 { get; set; } 

        /// <summary>
        /// 병력유형:3.뇌졸증  (1.해당 0.미해당)
        /// </summary>
		public string OLDBYENG3 { get; set; } 

        /// <summary>
        /// 병력유형:4.심장병  (1.해당 0.미해당)
        /// </summary>
		public string OLDBYENG4 { get; set; } 

        /// <summary>
        /// 병력유형:5.당뇨병  (1.해당 0.미해당)
        /// </summary>
		public string OLDBYENG5 { get; set; } 

        /// <summary>
        /// 병력유형:6.암      (1.해당 0.미해당)
        /// </summary>
		public string OLDBYENG6 { get; set; } 

        /// <summary>
        /// 병력유형:7.기타    (1.해당 0.미해당)
        /// </summary>
		public string OLDBYENG7 { get; set; } 

        /// <summary>
        /// 생활습관(1.양호 2.개선필요)
        /// </summary>
		public string HABIT { get; set; } 

        /// <summary>
        /// 개선필요:1.음주 (1.해당 0.미해당)
        /// </summary>
		public string HABIT1 { get; set; } 

        /// <summary>
        /// 개선필요:2.흡연 (1.해당 0.미해당)
        /// </summary>
		public string HABIT2 { get; set; } 

        /// <summary>
        /// 개선필요:3.운동 (1.해당 0.미해당)
        /// </summary>
		public string HABIT3 { get; set; } 

        /// <summary>
        /// 개선필요:4.체중 (1.해당 0.미해당)
        /// </summary>
		public string HABIT4 { get; set; } 

        /// <summary>
        /// 개선필요:5.음식 (1.해당 0.미해당)
        /// </summary>
		public string HABIT5 { get; set; }

        /// <summary>
        /// 작업사번
        /// </summary>
		public string ENTSABUN { get; set; }        

        /// <summary>
        /// 진찰소견:외상,휴유증(1.무 2.유)
        /// </summary>
		public string JINCHAL1 { get; set; } 

        /// <summary>
        /// 진찰소견::일반상태(1.양호 2.보통 3.불량)
        /// </summary>
		public string JINCHAL2 { get; set; } 

        /// <summary>
        /// 종합판정(아래참조)
        /// </summary>
		public string PANJENG { get; set; } 

        /// <summary>
        /// 정상B:1.비만관리    (1.해당 0.미해당)
        /// </summary>
		public string PANJENGB1 { get; set; } 

        /// <summary>
        /// 정상B:2.혈압관리    (1.해당 0.미해당)
        /// </summary>
		public string PANJENGB2 { get; set; } 

        /// <summary>
        /// 정상B:3.콜레스테롤관리((1.해당 0.미해당)
        /// </summary>
		public string PANJENGB3 { get; set; } 

        /// <summary>
        /// 정상B:4.간기능관리  (1.해당 0.미해당)
        /// </summary>
		public string PANJENGB4 { get; set; } 

        /// <summary>
        /// 정상B:5.당뇨관리    (1.해당 0.미해당)
        /// </summary>
		public string PANJENGB5 { get; set; } 

        /// <summary>
        /// 정상B:6.신장기능관리(1.해당 0.미해당)
        /// </summary>
		public string PANJENGB6 { get; set; } 

        /// <summary>
        /// 정상B:7.빈혈관리    (1.해당 0.미해당)
        /// </summary>
		public string PANJENGB7 { get; set; } 

        /// <summary>
        /// 정상B:8.골다공증관리-(생애66여)(1.해당 0.미해당)
        /// </summary>
		public string PANJENGB8 { get; set; } 

        /// <summary>
        /// R1:1.폐결핵의심(1.해당 0.미해당)
        /// </summary>
		public string PANJENGR1 { get; set; } 

        /// <summary>
        /// R1:2.기타흉부질환 (1.해당 0.미해당)
        /// </summary>
		public string PANJENGR2 { get; set; } 

        /// <summary>
        /// R2.1.고혈압(1.해당 0.미해당)
        /// </summary>
		public string PANJENGR3 { get; set; } 

        /// <summary>
        /// R1:3.이상지질혈증의심(1.해당 0.미해당)
        /// </summary>
		public string PANJENGR4 { get; set; } 

        /// <summary>
        /// R1:4.간장질환의심(1.해당 0.미해당)
        /// </summary>
		public string PANJENGR5 { get; set; } 

        /// <summary>
        /// R2:2.당뇨병(1.해당 0.미해당)
        /// </summary>
		public string PANJENGR6 { get; set; } 

        /// <summary>
        /// R1:5.신장질환의심(1.해당 0.미해당)
        /// </summary>
		public string PANJENGR7 { get; set; } 

        /// <summary>
        /// R1:6.빈혈증의심(1.해당 0.미해당)
        /// </summary>
		public string PANJENGR8 { get; set; } 

        /// <summary>
        /// R1:7.골다공증관리(66여)(1.해당 0.미해당)
        /// </summary>
		public string PANJENGR9 { get; set; } 

        /// <summary>
        /// R1:8.기타질환의심(1.해당 0.미해당)
        /// </summary>
		public string PANJENGR10 { get; set; } 

        /// <summary>
        /// R1:9.비만관리(1.해당 0.미해당) - 2010년
        /// </summary>
		public string PANJENGR11 { get; set; }

        /// <summary>
        /// 기타질환
        /// </summary>
        public string PANJENGETC { get; set; } 

        /// <summary>
        /// 판정일자
        /// </summary>
		public string PANJENGDATE { get; set; } 

        /// <summary>
        /// 건진일자
        /// </summary>
		public string GUNDATE { get; set; } 

        /// <summary>
        /// 통보방법(1.사업장 2.주소지 3.내원)
        /// </summary>
		public string TONGBOGBN { get; set; } 

        /// <summary>
        /// 통보일자
        /// </summary>
		public string  TONGBODATE { get; set; } 

        /// <summary>
        /// 판정의사 면허번호
        /// </summary>
		public long PANJENGDRNO { get; set; } 

        /// <summary>
        /// 소견및조치사항(바로조치/종합소견(질환의심)
        /// </summary>
		public string SOGEN { get; set; } 

        /// <summary>
        /// 흉부방사선번호(추가)
        /// </summary>
		public string XRAYNO { get; set; } 

        /// <summary>
        /// 입사일자(추가)
        /// </summary>
		public string IPSADATE { get; set; } 

        /// <summary>
        /// 검체상태 (1.적절,2.부적절)
        /// </summary>
		public string WOMB01 { get; set; } 

        /// <summary>
        /// 자궁경부 선상피세포 (1.유,2.무)
        /// </summary>
		public string WOMB02 { get; set; } 

        /// <summary>
        /// 유형별진단 (1.음성,2.상피세포,3.기타)
        /// </summary>
		public string WOMB03 { get; set; } 

        /// <summary>
        /// 편평상피 세포이상 (참고사항 참조)
        /// </summary>
		public string WOMB04 { get; set; } 

        /// <summary>
        /// 선상피세포이상 (참고사항 참조)
        /// </summary>
		public string WOMB05 { get; set; } 

        /// <summary>
        /// 선상피세포이상 기타
        /// </summary>
		public string WOMB06 { get; set; } 

        /// <summary>
        /// 추가소견(참고사항 참조)
        /// </summary>
		public string WOMB07 { get; set; } 

        /// <summary>
        /// 추가소견 기타
        /// </summary>
		public string WOMB08 { get; set; } 

        /// <summary>
        /// 자궁도말검사 종합판정
        /// </summary>
		public string WOMB09 { get; set; } 

        /// <summary>
        /// 의심()개월후 재검대상
        /// </summary>
		public string WOMB10 { get; set; } 

        /// <summary>
        /// 기타질환()치료대상
        /// </summary>
		public string WOMB11 { get; set; } 

        /// <summary>
        /// 병력유형:7.기타 질환명
        /// </summary>
		public string OLDBYENGNAME { get; set; } 

        /// <summary>
        /// 결과지 인쇄(Y :인쇄, 기타: 미인쇄)
        /// </summary>
		public string GBPRINT { get; set; } 

        /// <summary>
        /// 임상진찰 의사면허번호
        /// </summary>
		public long MUNJINDRNO { get; set; } 

        /// <summary>
        /// 임상진찰 참고사항
        /// </summary>
		public string JINREMARK { get; set; } 

        /// <summary>
        /// 판정완료여부(Y.판정완료)
        /// </summary>
		public string GBPANJENG { get; set; } 

        /// <summary>
        /// 정상B:9.기타질환관리    (1.해당 0.미해당)
        /// </summary>
		public string PANJENGB9 { get; set; } 

        /// <summary>
        /// 정상B:기타질환관리
        /// </summary>
		public string PANJENGB_ETC { get; set; } 

        /// <summary>
        /// 질환의심:기타질환의심
        /// </summary>
		public string PANJENGR_ETC { get; set; } 

        /// <summary>
        /// 추가(선택)검사 판정소견
        /// </summary>
		public string ADDSOGEN { get; set; } 

        /// <summary>
        /// 흡연 시작년도(2005)
        /// </summary>
		public string SMOKING4 { get; set; } 

        /// <summary>
        /// 금연 시작년도(2006)
        /// </summary>
		public string SMOKING5 { get; set; } 

        /// <summary>
        /// 판정B기타질환관리세부
        /// </summary>
		public string PANJENGB_ETC_DTL { get; set; } 

        /// <summary>
        /// 허리둘레(cm)
        /// </summary>
		public long WAIST { get; set; } 

        /// <summary>
        /// 현재상태-뇌졸중 진단여부 2009년 (1.해당 2.미해당)
        /// </summary>
		public string T_STAT01 { get; set; } 

        /// <summary>
        /// 현재상태-뇌졸중 약물여부 2009년
        /// </summary>
		public string T_STAT02 { get; set; } 

        /// <summary>
        /// 현재상태-심장병 진단여부 2009년
        /// </summary>
		public string T_STAT11 { get; set; } 

        /// <summary>
        /// 현재상태-심장병 약물여부 2009년
        /// </summary>
		public string T_STAT12 { get; set; } 

        /// <summary>
        /// 현재상태-고혈압 진단여부 2009년
        /// </summary>
		public string T_STAT21 { get; set; } 

        /// <summary>
        /// 현재상태-고혈압 약물여부 2009년
        /// </summary>
		public string T_STAT22 { get; set; } 

        /// <summary>
        /// 현재상태-당뇨병 진단여부 2009년
        /// </summary>
		public string T_STAT31 { get; set; } 

        /// <summary>
        /// 현재상태-당뇨병 약물여부 2009년
        /// </summary>
		public string T_STAT32 { get; set; } 

        /// <summary>
        /// 현재상태-고지혈증 진단여부 2009년
        /// </summary>
		public string T_STAT41 { get; set; } 

        /// <summary>
        /// 현재상태-고지혈증 약물여부 2009년
        /// </summary>
		public string T_STAT42 { get; set; } 

        /// <summary>
        /// 현재상태-기타 진단여부 2009년
        /// </summary>
		public string T_STAT51 { get; set; } 

        /// <summary>
        /// 현재상태-기타 약물여부 2009년
        /// </summary>
		public string T_STAT52 { get; set; } 

        /// <summary>
        /// 가족질환-뇌졸중 2009년 (1.해당 2.미해당)
        /// </summary>
		public string T_GAJOK1 { get; set; } 

        /// <summary>
        /// 가족질환-심장병 2009년
        /// </summary>
		public string T_GAJOK2 { get; set; } 

        /// <summary>
        /// 가족질환-고혈압 2009년
        /// </summary>
		public string T_GAJOK3 { get; set; } 

        /// <summary>
        /// 가족질환-당뇨병 2009년
        /// </summary>
		public string T_GAJOK4 { get; set; } 

        /// <summary>
        /// 가족질환-기타 2009년
        /// </summary>
		public string T_GAJOK5 { get; set; } 

        /// <summary>
        /// B형간염 항원보유자 2009년(1.예 2.아니오 3.모름)
        /// </summary>
		public string T_BLIVER { get; set; } 

        /// <summary>
        /// 흡연-담배피운적? 2009년
        /// </summary>
		public string T_SMOKE1 { get; set; } 

        /// <summary>
        /// 흡연-과거흡연-몇년흡연 2009년
        /// </summary>
		public long T_SMOKE2 { get; set; } 

        /// <summary>
        /// 흡연-과거흡연-흡연량 2009년
        /// </summary>
		public long T_SMOKE3 { get; set; } 

        /// <summary>
        /// 흡연-몇년째? 2009년
        /// </summary>
		public long T_SMOKE4 { get; set; } 

        /// <summary>
        /// 흡연-흡연량? 2009년
        /// </summary>
		public long T_SMOKE5 { get; set; } 

        /// <summary>
        /// 음주-1주평균? 2009년
        /// </summary>
		public string T_DRINK1 { get; set; } 

        /// <summary>
        /// 음주-하루에얼마? 2009년
        /// </summary>
		public long T_DRINK2 { get; set; } 

        /// <summary>
        /// 신체활동-격렬한운동 20분이상? 2009년
        /// </summary>
		public string T_ACTIVE1 { get; set; } 

        /// <summary>
        /// 중간운동 하루 30분이상 ? 2009년
        /// </summary>
		public string T_ACTIVE2 { get; set; } 

        /// <summary>
        /// 총30분걷기몇일? 2009년
        /// </summary>
		public string T_ACTIVE3 { get; set; } 

        /// <summary>
        /// 만40세-괴롭게느껴진다 2009년
        /// </summary>
		public string T40_FEEL1 { get; set; } 

        /// <summary>
        /// 만40세-식욕이없다 2009년
        /// </summary>
		public string T40_FEEL2 { get; set; } 

        /// <summary>
        /// 만40세-울적하다 2009년
        /// </summary>
		public string T40_FEEL3 { get; set; } 

        /// <summary>
        /// 만40세-우울했다 2009년
        /// </summary>
		public string T40_FEEL4 { get; set; } 

        /// <summary>
        /// 만66세-독감접종 2009년
        /// </summary>
		public string T66_INJECT { get; set; } 

        /// <summary>
        /// 만66세-혼자식사 2009년
        /// </summary>
		public string T66_STAT1 { get; set; } 

        /// <summary>
        /// 만66세-혼자 옷입기 2009년
        /// </summary>
		public string T66_STAT2 { get; set; } 

        /// <summary>
        /// 만66세-혼자 대소변 2009년
        /// </summary>
		public string T66_STAT3 { get; set; } 

        /// <summary>
        /// 만66세-혼자 목욕 2009년
        /// </summary>
		public string T66_STAT4 { get; set; } 

        /// <summary>
        /// 만66세-혼자 식사 2009년
        /// </summary>
		public string T66_STAT5 { get; set; } 

        /// <summary>
        /// 만66세-혼자 외출 2009년
        /// </summary>
		public string T66_STAT6 { get; set; } 

        /// <summary>
        /// 만66세-의욕이 떨어짐 2009년
        /// </summary>
		public string T66_FEEL1 { get; set; } 

        /// <summary>
        /// 만66세-쓸모없는 느낌 2009년
        /// </summary>
		public string T66_FEEL2 { get; set; } 

        /// <summary>
        /// 만66세-희망이없다 2009년
        /// </summary>
		public string T66_FEEL3 { get; set; } 

        /// <summary>
        /// 만66세-기억력이 동료보다 못하다 2009년
        /// </summary>
		public string T66_MEMORY1 { get; set; } 

        /// <summary>
        /// 만66세-기억력이 나빠졌다 2009년
        /// </summary>
		public string T66_MEMORY2 { get; set; } 

        /// <summary>
        /// 만66세-기억력이 문제가된다 2009년
        /// </summary>
		public string T66_MEMORY3 { get; set; } 

        /// <summary>
        /// 만66세-기억력떨어진것 남도 안다 2009년
        /// </summary>
		public string T66_MEMORY4 { get; set; } 

        /// <summary>
        /// 만66세-예전보도 서툴다 2009년
        /// </summary>
		public string T66_MEMORY5 { get; set; } 

        /// <summary>
        /// 만66세-낙상경험 2009년
        /// </summary>
		public string T66_FALL { get; set; } 

        /// <summary>
        /// 만66세-배뇨장애 2009년
        /// </summary>
		public string T66_URO { get; set; } 

        /// <summary>
        /// C:1.폐결핵 - 사용안함
        /// </summary>
		public string PANJENGC1 { get; set; } 

        /// <summary>
        /// C:2.간장질환 - 사용안함
        /// </summary>
		public string PANJENGC2 { get; set; } 

        /// <summary>
        /// C:3.빈혈증 - 사용안함
        /// </summary>
		public string PANJENGC3 { get; set; } 

        /// <summary>
        /// C:4.기타질환 - 사용안함
        /// </summary>
		public string PANJENGC4 { get; set; } 

        /// <summary>
        /// D1:직업병1
        /// </summary>
		public string PANJENGD11 { get; set; } 

        /// <summary>
        /// D1:직업병2
        /// </summary>
		public string PANJENGD12 { get; set; } 

        /// <summary>
        /// D1:직업병3
        /// </summary>
		public string PANJENGD13 { get; set; } 

        /// <summary>
        /// D2:일반질병1
        /// </summary>
		public string PANJENGD21 { get; set; } 

        /// <summary>
        /// D2:일반질병2
        /// </summary>
		public string PANJENGD22 { get; set; } 

        /// <summary>
        /// D2:일반질병3
        /// </summary>
		public string PANJENGD23 { get; set; } 

        /// <summary>
        /// 1차판정 사후관리(R1-C)
        /// </summary>
		public string PANJENGSAHU { get; set; } 

        /// <summary>
        /// C:5.비민관리 - 사용안함
        /// </summary>
		public string PANJENGC5 { get; set; } 

        /// <summary>
        /// 1차상담내역
        /// </summary>
		public string SANGDAM { get; set; } 

        /// <summary>
        /// 2차상담내역(2차검진시 1차 접수번호에 저장)
        /// </summary>
		public string SANGDAM2 { get; set; } 

        /// <summary>
        /// 검진일에 식사여부(Y: 했음, N or NULL : 안했음)
        /// </summary>
		public string GBSIKSA { get; set; } 

        /// <summary>
        /// 생애-우울증판정(1:없음, 2: 2차대상)
        /// </summary>
		public string T40_FEEL { get; set; } 

        /// <summary>
        /// 생애-인지기능장애판정(1:없음, 2: 2차대상)
        /// </summary>
		public string T66_STAT { get; set; } 

        /// <summary>
        /// 생애-하지기능(일어나 3m 돌아와 앉기)
        /// </summary>
		public string FOOT1 { get; set; } 

        /// <summary>
        /// 생애-보행장애유무(1.유  2.무)
        /// </summary>
		public string FOOT2 { get; set; } 

        /// <summary>
        /// 생애-평형성(한다리로서기)
        /// </summary>
		public string BALANCE { get; set; } 

        /// <summary>
        /// 생애-여성골밀도검사(만66세여성)
        /// </summary>
		public string OSTEO { get; set; } 

        /// <summary>
        /// 생애-권고사항
        /// </summary>
		public string LIFESOGEN { get; set; } 

        /// <summary>
        /// 현재상태-폐결핵 진단여부 2010년
        /// </summary>
		public string T_STAT61 { get; set; } 

        /// <summary>
        /// 현재상태-폐결핵 약물여부 2010년
        /// </summary>
		public string T_STAT62 { get; set; } 

        /// <summary>
        /// U:1.고혈압 (1.해당  0.미해당)
        /// </summary>
		public string PANJENGU1 { get; set; } 

        /// <summary>
        /// U:2.당뇨병 (1.해당  0.미해당)
        /// </summary>
		public string PANJENGU2 { get; set; } 

        /// <summary>
        /// U:3.이상지질혈증 (1.해당  0.미해당)
        /// </summary>
		public string PANJENGU3 { get; set; } 

        /// <summary>
        /// U:4.폐결핵 (1.해당  0.미해당)
        /// </summary>
		public string PANJENGU4 { get; set; } 

        /// <summary>
        /// 1차판정 사후관리(D-D2)
        /// </summary>
		public string PANJENGSAHU2 { get; set; } 

        /// <summary>
        /// 1차판정 사후관리(D1)
        /// </summary>
		public string PANJENGSAHU3 { get; set; } 

        /// <summary>
        /// 업무적합성(000)
        /// </summary>
		public string WORKYN { get; set; } 

        /// <summary>
        /// 프린트 작업사번
        /// </summary>
		public long PRTSABUN { get; set; } 

        /// <summary>
        /// 현재상태-간장질환 진단여부 2009년
        /// </summary>
		public string T_STAT71 { get; set; } 

        /// <summary>
        /// 현재상태-간장질환 약물여부 2009년
        /// </summary>
		public string T_STAT72 { get; set; } 

        /// <summary>
        /// 2015-01-01 적극적인 관리/종합소견(유질환)
        /// </summary>
		public string SOGENB { get; set; } 

        /// <summary>
        /// 토.공휴일 가산료 여부(Y/N)
        /// </summary>
		public string GBGONGHU { get; set; } 

        /// <summary>
        /// 통합문진 전자담배 경험 (1.예,2아니오)
        /// </summary>
		public string TMUN0001 { get; set; } 

        /// <summary>
        /// 통합문진 최근 한달 전자담배 경험 (1.아니오 2월1-2일 3.월2-9일 4.월10-2
        /// </summary>
		public string TMUN0002 { get; set; } 

        /// <summary>
        /// 통합문진 술을 마시는 횟수(1.일주일,2.한달 3.1년 4.술을 마시지 않는다)
        /// </summary>
		public string TMUN0003 { get; set; } 

        /// <summary>
        /// 통합문진 술을 마시는 횟수
        /// </summary>
		public string TMUN0004 { get; set; } 

        /// <summary>
        /// 통합문진 보통주량1 (술종류;주량;단위)
        /// </summary>
		public string TMUN0005 { get; set; } 

        /// <summary>
        /// 통합문진 보통주량2 (술종류;주량;단위)
        /// </summary>
		public string TMUN0006 { get; set; } 

        /// <summary>
        /// 통합문진 보통주량3 (술종류;주량;단위)
        /// </summary>
		public string TMUN0007 { get; set; } 

        /// <summary>
        /// 통합문진 최대주량1 (술종류;주량;단위)
        /// </summary>
		public string TMUN0008 { get; set; } 

        /// <summary>
        /// 통합문진 최대주량2 (술종류;주량;단위)
        /// </summary>
		public string TMUN0009 { get; set; } 

        /// <summary>
        /// 통합문진 최대주량3 (술종류;주량;단위)
        /// </summary>
		public string TMUN0010 { get; set; } 

        /// <summary>
        /// 통합문진 고강도 신체활동 시간(시간:분)
        /// </summary>
		public string TMUN0011 { get; set; } 

        /// <summary>
        /// 통합문진 중강도 신체활동 시간(시간:분)
        /// </summary>
		public string TMUN0012 { get; set; } 

        /// <summary>
        /// 통합문진 노인기능평가 2.폐렴예방접종 (1.예,2아니오)
        /// </summary>
		public string TMUN0013 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TMUN0014 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TMUN0015 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TMUN0016 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TMUN0017 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TMUN0018 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TMUN0019 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TMUN0020 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TMUN0021 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TMUN0022 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TMUN0023 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string TMUN0024 { get; set; } 

        /// <summary>
        /// 기타 술종류
        /// </summary>
		public string TMUN0025 { get; set; } 

        /// <summary>
        /// 소주 잔
        /// </summary>
		public string TMUN0026 { get; set; } 

        /// <summary>
        /// 소주 병
        /// </summary>
		public string TMUN0027 { get; set; } 

        /// <summary>
        /// 소주 캔
        /// </summary>
		public string TMUN0028 { get; set; } 

        /// <summary>
        /// 소주 cc
        /// </summary>
		public string TMUN0029 { get; set; } 

        /// <summary>
        /// 맥주 잔
        /// </summary>
		public string TMUN0030 { get; set; } 

        /// <summary>
        /// 맥주 병
        /// </summary>
		public string TMUN0031 { get; set; } 

        /// <summary>
        /// 맥주 캔
        /// </summary>
		public string TMUN0032 { get; set; } 

        /// <summary>
        /// 맥주 cc
        /// </summary>
		public string TMUN0033 { get; set; } 

        /// <summary>
        /// 양주 잔
        /// </summary>
		public string TMUN0034 { get; set; } 

        /// <summary>
        /// 양주 병
        /// </summary>
		public string TMUN0035 { get; set; } 

        /// <summary>
        /// 양주 캔
        /// </summary>
		public string TMUN0036 { get; set; } 

        /// <summary>
        /// 양주 cc
        /// </summary>
		public string TMUN0037 { get; set; } 

        /// <summary>
        /// 막걸리 잔
        /// </summary>
		public string TMUN0038 { get; set; } 

        /// <summary>
        /// 막걸리 병
        /// </summary>
		public string TMUN0039 { get; set; } 

        /// <summary>
        /// 막걸리 캔
        /// </summary>
		public string TMUN0040 { get; set; } 

        /// <summary>
        /// 막걸리 cc
        /// </summary>
		public string TMUN0041 { get; set; } 

        /// <summary>
        /// 기타 잔
        /// </summary>
		public string TMUN0042 { get; set; } 

        /// <summary>
        /// 기타 병
        /// </summary>
		public string TMUN0043 { get; set; } 

        /// <summary>
        /// 기타 캔
        /// </summary>
		public string TMUN0044 { get; set; } 

        /// <summary>
        /// 기타 cc
        /// </summary>
		public string TMUN0045 { get; set; } 

        /// <summary>
        /// 기타 술종류
        /// </summary>
		public string TMUN0046 { get; set; } 

        /// <summary>
        /// 고강동 운동 - 1주일에 하는
        /// </summary>
		public string TMUN0047 { get; set; } 

        /// <summary>
        /// 고강도 운동 시간
        /// </summary>
		public string TMUN0048 { get; set; } 

        /// <summary>
        /// 고강도 운동 분
        /// </summary>
		public string TMUN0049 { get; set; } 

        /// <summary>
        /// 중강도 운동 - 1주일에 하는
        /// </summary>
		public string TMUN0050 { get; set; } 

        /// <summary>
        /// 중강도 운동 시간
        /// </summary>
		public string TMUN0051 { get; set; } 

        /// <summary>
        /// 중강도 운동 분
        /// </summary>
		public string TMUN0052 { get; set; } 

        /// <summary>
        /// 근력운동
        /// </summary>
		public string TMUN0053 { get; set; } 

        /// <summary>
        /// 노인기능평가 2.폐렴예방접종 (1.예,2아니오)
        /// </summary>
		public string TMUN0054 { get; set; } 

        /// <summary>
        /// 흡연-니코틴 의존도
        /// </summary>
		public string TMUN0055 { get; set; } 

        /// <summary>
        /// 흡연-처방
        /// </summary>
		public string TMUN0056 { get; set; } 

        /// <summary>
        /// 흡연-평가점수
        /// </summary>
		public long TMUN0057 { get; set; } 

        /// <summary>
        /// 음주-평가
        /// </summary>
		public string TMUN0058 { get; set; } 

        /// <summary>
        /// 음주-처방
        /// </summary>
		public string TMUN0059 { get; set; } 

        /// <summary>
        /// 음주-평가점수
        /// </summary>
		public long TMUN0060 { get; set; } 

        /// <summary>
        /// 운동-평가
        /// </summary>
		public string TMUN0061 { get; set; } 

        /// <summary>
        /// 운동-처방(종류)
        /// </summary>
		public string TMUN0062 { get; set; } 

        /// <summary>
        /// 운동-처방(시간)
        /// </summary>
		public string TMUN0063 { get; set; } 

        /// <summary>
        /// 운동-처방(빈도,횟수)
        /// </summary>
		public string TMUN0064 { get; set; } 

        /// <summary>
        /// 운동-평가점수
        /// </summary>
		public long TMUN0065 { get; set; } 

        /// <summary>
        /// 영양-평가
        /// </summary>
		public string TMUN0066 { get; set; } 

        /// <summary>
        /// 영양-권장음식(유제품)
        /// </summary>
		public string TMUN0067 { get; set; } 

        /// <summary>
        /// 영양-권장음식(단백질류)
        /// </summary>
		public string TMUN0068 { get; set; } 

        /// <summary>
        /// 영양-권장음식(야채와과일)
        /// </summary>
		public string TMUN0069 { get; set; } 

        /// <summary>
        /// 영양-제한음식(지방)
        /// </summary>
		public string TMUN0070 { get; set; } 

        /// <summary>
        /// 영양-제한음식(단순당)
        /// </summary>
		public string TMUN0071 { get; set; } 

        /// <summary>
        /// 영양-제한음식(염분,소금)
        /// </summary>
		public string TMUN0072 { get; set; } 

        /// <summary>
        /// 영양-올바른식사습관(아침식사)
        /// </summary>
		public string TMUN0073 { get; set; } 

        /// <summary>
        /// 영양-올바른식사습관(골고루먹기)
        /// </summary>
		public string TMUN0074 { get; set; } 

        /// <summary>
        /// 영양-처방연계(영양교육)
        /// </summary>
		public string TMUN0075 { get; set; } 

        /// <summary>
        /// 영양-평가점수
        /// </summary>
		public long TMUN0076 { get; set; } 

        /// <summary>
        /// 비만-체질량지수
        /// </summary>
		public string TMUN0077 { get; set; } 

        /// <summary>
        /// 비만-허리둘레
        /// </summary>
		public string TMUN0078 { get; set; } 

        /// <summary>
        /// 비만-처방(1.식사량을 줄이십시오)
        /// </summary>
		public string TMUN0079 { get; set; } 

        /// <summary>
        /// 비만-처방(2.간식과 야식을 줄이십시오)
        /// </summary>
		public string TMUN0080 { get; set; } 

        /// <summary>
        /// 비만-처방(3.음주량과 횟수를 줄이십시오)
        /// </summary>
		public string TMUN0081 { get; set; } 

        /// <summary>
        /// 비만-처방(4.외식이나 패스트푸드를 줄이십시오)
        /// </summary>
		public string TMUN0082 { get; set; } 

        /// <summary>
        /// 비만-처방(5.운동처방을 참고하십시오)
        /// </summary>
		public string TMUN0083 { get; set; } 

        /// <summary>
        /// 비만-처방(6연계(비만클리닉))
        /// </summary>
		public string TMUN0084 { get; set; } 

        /// <summary>
        /// 비만-처방(7.기타)
        /// </summary>
		public string TMUN0085 { get; set; } 

        /// <summary>
        /// 우울증-평가점수
        /// </summary>
		public long TMUN0086 { get; set; } 

        /// <summary>
        /// 우울증-평가
        /// </summary>
		public string TMUN0087 { get; set; } 

        /// <summary>
        /// 우울증-처방
        /// </summary>
		public string TMUN0088 { get; set; } 

        /// <summary>
        /// 인지기능-평가점수
        /// </summary>
		public long TMUN0089 { get; set; } 

        /// <summary>
        /// 인지기능-평가
        /// </summary>
		public string TMUN0090 { get; set; } 

        /// <summary>
        /// 인지기능-처방
        /// </summary>
		public string TMUN0091 { get; set; } 

        /// <summary>
        /// 종합소견-의심질환소견<사용안함>
        /// </summary>
		public string TMUN0092 { get; set; } 

        /// <summary>
        /// 종합소견-유질환소견<사용안함>
        /// </summary>
		public string TMUN0093 { get; set; } 

        /// <summary>
        /// 종합소견-생활습관관리<사용안함>
        /// </summary>
		public string TMUN0094 { get; set; } 

        /// <summary>
        /// 종합소견-기타소견<사용안함>
        /// </summary>
		public string TMUN0095 { get; set; } 

        /// <summary>
        /// 종합소견-생활습관관리<사용안함>
        /// </summary>
		public string PANJENGC { get; set; } 

        /// <summary>
        /// 종합소견-기타소견<사용안함>
        /// </summary>
		public string PANJENGD { get; set; } 

        /// <summary>
        /// 생활습관처방전-흡연
        /// </summary>
		public string SLIP_SMOKE { get; set; } 

        /// <summary>
        /// 생활습관처방전-음주
        /// </summary>
		public string SLIP_DRINK { get; set; } 

        /// <summary>
        /// 생활습관처방전-운동
        /// </summary>
		public string SLIP_ACTIVE { get; set; } 

        /// <summary>
        /// 생활습관처방전-영양
        /// </summary>
		public string SLIP_FOOD { get; set; } 

        /// <summary>
        /// 생활습관처방전-비만
        /// </summary>
		public string SLIP_BIMAN { get; set; } 

        /// <summary>
        /// 종합소견-생활습관관리
        /// </summary>
		public string SOGENC { get; set; } 

        /// <summary>
        /// 종합소견-기타소견
        /// </summary>
		public string SOGEND { get; set; } 

        /// <summary>
        /// 생활습관판정-우울증
        /// </summary>
		public string SLIP_PHQ { get; set; } 

        /// <summary>
        /// 생활습관판정-인지기능
        /// </summary>
		public string SLIP_KDSQ { get; set; } 

        /// <summary>
        /// 생활습관판정-노인신체기능
        /// </summary>
		public string SLIP_OLDMAN { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string SLIP_LIFESOGEN1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string SLIP_LIFESOGEN2 { get; set; } 

        /// <summary>
        /// 정상B:9. 비활동성 폐결핵(0.미해당 1.해당)
        /// </summary>
		public string PANJENGB10 { get; set; } 

        /// <summary>
        /// R1:12. 난청(0.미해당 1.해당)
        /// </summary>
		public string PANJENGR12 { get; set; } 

        /// <summary>
        /// 일반담배 금연전 기간
        /// </summary>
		public string TMUN0096 { get; set; } 

        /// <summary>
        /// 궐련형 전자담배 흡연 여부
        /// </summary>
		public string TMUN0097 { get; set; } 

        /// <summary>
        /// 궐련형 전자담배 흡연 기간
        /// </summary>
		public string TMUN0098 { get; set; } 

        /// <summary>
        /// 궐련형 전자담배 흡연량
        /// </summary>
		public string TMUN0099 { get; set; } 

        /// <summary>
        /// 궐련형 전자담배 금연 기간
        /// </summary>
		public string TMUN0100 { get; set; } 

        /// <summary>
        /// 궐련형 전자담배 금연전 흡연량
        /// </summary>
		public string TMUN0101 { get; set; } 

        /// <summary>
        /// 궐련형 전자담배 금연전 기간
        /// </summary>
		public string TMUN0102 { get; set; } 

        /// <summary>
        /// 일반담배 흡연 여부
        /// </summary>
		public string TMUN0103 { get; set; } 

        /// <summary>
        /// 전자담배 흡연 여부
        /// </summary>
		public string TMUN0104 { get; set; } 

        /// <summary>
        /// 심뇌혈관질환(발생위험도) 0.00배
        /// </summary>
		public string SIM_RESULT1 { get; set; } 

        /// <summary>
        /// 심뇌혈관질환(발생확률) 0.0배
        /// </summary>
		public string SIM_RESULT2 { get; set; } 

        /// <summary>
        /// 심뇌혈관질환(심혈관나이) 00세
        /// </summary>
		public string SIM_RESULT3 { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        public long SABUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0105 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0106 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0107 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0108 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0109 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0110 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0111 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0112 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0113 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0114 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0115 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0116 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0117 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0118 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0119 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0120 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0121 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0122 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0123 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TMUN0124 { get; set; }


        /// <summary>
        /// 통합문진 보통주량4 (술종류;주량;단위)
        /// </summary>
		public string TMUN0125 { get; set; }

        /// <summary>
        /// 통합문진 보통주량5 (술종류;주량;단위)
        /// </summary>
		public string TMUN0126 { get; set; }

        /// <summary>
        /// 통합문진 최대주량4 (술종류;주량;단위)
        /// </summary>
        public string TMUN0127 { get; set; }

        /// <summary>
        /// 통합문진 최대주량5 (술종류;주량;단위)
        /// </summary>
        public string TMUN0128 { get; set; }








        /// <summary>
        /// 건강검진1차 문진 및 판정결과
        /// </summary>
        public HIC_RES_BOHUM1()
        {
        }
    }
}
