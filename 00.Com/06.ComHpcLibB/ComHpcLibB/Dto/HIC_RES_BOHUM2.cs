namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_RES_BOHUM2 : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 흉부질환 2차대상(Y/N)
        /// </summary>
		public string GBCHEST { get; set; } 

        /// <summary>
        /// 순환기계질환 2차 대상(Y/N)
        /// </summary>
		public string GBCYCLE { get; set; } 

        /// <summary>
        /// 고지혈증 2차대상(Y/N)
        /// </summary>
		public string GBGOJI { get; set; } 

        /// <summary>
        /// 간장질환 2차대상(Y/N)
        /// </summary>
		public string GBLIVER { get; set; } 

        /// <summary>
        /// 신장질환 2차대상(Y/N)
        /// </summary>
		public string GBKIDNEY { get; set; } 

        /// <summary>
        /// 빈혈증 2차 대상(Y/N)
        /// </summary>
		public string GBANEMIA { get; set; } 

        /// <summary>
        /// 당뇨질환 2차 대상(Y/N)
        /// </summary>
		public string GBDIABETES { get; set; } 

        /// <summary>
        /// 기타질환 2차 대상(Y/N)
        /// </summary>
		public string GBETC { get; set; } 

        /// <summary>
        /// 흉부방사선검사
        /// </summary>
		public string CHEST1 { get; set; } 

        /// <summary>
        /// 결핵균집균도말검사(1.음성 2.양성)
        /// </summary>
		public string CHEST2 { get; set; } 

        /// <summary>
        /// 폐결핵검사소견(1.정상 4.유질환)
        /// </summary>
		public string CHEST3 { get; set; } 

        /// <summary>
        /// 흉부검사소견(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
		public string CHEST_RES { get; set; } 

        /// <summary>
        /// 고혈압(혈압:최고)
        /// </summary>
		public string CYCLE1 { get; set; } 

        /// <summary>
        /// 고혈압(혈압:최저)
        /// </summary>
		public string CYCLE2 { get; set; } 

        /// <summary>
        /// 정밀안저검사(1.정상 2.안정도1~2 3.안전도3 4.안전도4)
        /// </summary>
		public string CYCLE3 { get; set; } 

        /// <summary>
        /// 심전도검사(아래참조)
        /// </summary>
		public string CYCLE4 { get; set; } 

        /// <summary>
        /// 고혈압검사소견(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
		public string CYCLE_RES { get; set; } 

        /// <summary>
        /// 고지혈증(트리그리세라이드)
        /// </summary>
		public string GOJI1 { get; set; } 

        /// <summary>
        /// 고지혈증(HDL콜레스테롤)
        /// </summary>
		public string GOJI2 { get; set; } 

        /// <summary>
        /// 고지혈증소견(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
		public string GOJI_RES { get; set; } 

        /// <summary>
        /// 간장질환(알부민)
        /// </summary>
		public string LIVER11 { get; set; } 

        /// <summary>
        /// 간장질환(총단백정량)
        /// </summary>
		public string LIVER12 { get; set; } 

        /// <summary>
        /// 간장질환(총빌리루빈)
        /// </summary>
		public string LIVER13 { get; set; } 

        /// <summary>
        /// 간장질환(직접빌리루빈)
        /// </summary>
		public string LIVER14 { get; set; } 

        /// <summary>
        /// 간장질환(알카리포스타파제)
        /// </summary>
		public string LIVER15 { get; set; } 

        /// <summary>
        /// 간장질환(유산탈수효소(LDH)
        /// </summary>
		public string LIVER16 { get; set; } 

        /// <summary>
        /// 간장질환(알파휘토단백) (아래참조)
        /// </summary>
		public string LIVER17 { get; set; } 

        /// <summary>
        /// 간장질환(간염검사항원) :1.음성 2.양성
        /// </summary>
		public string LIVER18 { get; set; } 

        /// <summary>
        /// 간장질환(간염검사항체):1.음성 2.양성
        /// </summary>
		public string LIVER19 { get; set; } 

        /// <summary>
        /// 간염검사결과: 1.간염보균자 2.면역자 3.접종대상자
        /// </summary>
		public string LIVER20 { get; set; } 

        /// <summary>
        /// 간장질환소견(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
		public string LIVER_RES { get; set; } 

        /// <summary>
        /// 신장질환(요침사현미경검사(RBC))
        /// </summary>
		public string KIDNEY1 { get; set; } 

        /// <summary>
        /// 신장질환(요침사현미경검사(WBC))
        /// </summary>
		public string KIDNEY2 { get; set; } 

        /// <summary>
        /// 신장질환(요소질소)
        /// </summary>
		public string KIDNEY3 { get; set; } 

        /// <summary>
        /// 신장질환(크레아티닌)
        /// </summary>
		public string KIDNEY4 { get; set; } 

        /// <summary>
        /// 신장질환(요산)
        /// </summary>
		public string KIDNEY5 { get; set; } 

        /// <summary>
        /// 신장질환소견(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
		public string KIDNEY_RES { get; set; } 

        /// <summary>
        /// 빈혈증(헤파토크리트)
        /// </summary>
		public string ANEMIA1 { get; set; } 

        /// <summary>
        /// 빈혈증(백혈구수)
        /// </summary>
		public string AMEMIA2 { get; set; } 

        /// <summary>
        /// 빈혈증(적혈구수)
        /// </summary>
		public string AMEMIA3 { get; set; } 

        /// <summary>
        /// 빈혈증소견(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
		public string AMEMIA_RES { get; set; } 

        /// <summary>
        /// 당뇨질환(혈당:최고) 식후
        /// </summary>
		public string DIABETES1 { get; set; } 

        /// <summary>
        /// 당뇨질환(혈당:최저) 식전
        /// </summary>
		public string DIABETES2 { get; set; } 

        /// <summary>
        /// 당뇨질환(정밀안저검사) ** 아래참조 **
        /// </summary>
		public string DIABETES3 { get; set; } 

        /// <summary>
        /// 당뇨질환소견(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
		public string DIABETES_RES { get; set; } 

        /// <summary>
        /// 기타질환 검사소견
        /// </summary>
		public string ETC_RES { get; set; } 

        /// <summary>
        /// 기타질환 검사내용
        /// </summary>
		public string ETC_EXAM { get; set; } 

        /// <summary>
        /// 종합판정(1.정상A 2.정상B 3.건강주의 4.유질환)
        /// </summary>
		public string PANJENG { get; set; } 

        /// <summary>
        /// 종합판정유질환(직업병D1):판정이 "5"인 경우(2002년검진용)
        /// </summary>
		public string PANJENG_D1 { get; set; } 

        /// <summary>
        /// 질병유소견자 일반질병
        /// </summary>
		public string PANJENG_SO1 { get; set; } 

        /// <summary>
        /// 질병유소견자 직업병
        /// </summary>
		public string PANJENG_SO2 { get; set; } 

        /// <summary>
        /// 판정일자
        /// </summary>
		public string PANJENGDATE { get; set; } 

        /// <summary>
        /// 결과 통보방법(1.사업장 2.주소지 3.내원)
        /// </summary>
		public string TONGBOGBN { get; set; } 

        /// <summary>
        /// 결과 통보일자
        /// </summary>
		public string TONGBODATE { get; set; } 

        /// <summary>
        /// 판정의사 면허번호
        /// </summary>
		public long PANJENGDRNO { get; set; } 

        /// <summary>
        /// 소견 및 조치사항
        /// </summary>
		public string SOGEN { get; set; } 

        /// <summary>
        /// 검진일자
        /// </summary>
		public string GUNDATE { get; set; } 

        /// <summary>
        /// 건강주의 사후관리소견
        /// </summary>
		public string PANJENG_SO3 { get; set; } 

        /// <summary>
        /// 종합판정유질환(직업병D1):판정이 "5"인 경우(2003년검진용)
        /// </summary>
		public string PANJENG_D11 { get; set; } 

        /// <summary>
        /// 종합판정유질환(직업병D1):판정이 "5"인 경우(2003년검진용)
        /// </summary>
		public string PANJENG_D12 { get; set; } 

        /// <summary>
        /// 결과지 인쇄(Y :인쇄, 기타: 미인쇄)
        /// </summary>
		public string GBPRINT { get; set; } 

        /// <summary>
        /// 업무적합성여부(**) 공단코드:13 일반+특수만 사용함
        /// </summary>
		public string WORKYN { get; set; } 

        /// <summary>
        /// 배양 및 약제감수성검사 (1:의뢰 2:미의뢰)
        /// </summary>
		public string CHEST4 { get; set; } 

        /// <summary>
        /// 유질환 - 일반질병 D2 (K,J,A,)
        /// </summary>
		public string PANJENG_D2 { get; set; } 

        /// <summary>
        /// 간장질환 - C형간염표면항체 검사-1일반, 2정밀
        /// </summary>
		public string LIVER21 { get; set; } 

        /// <summary>
        /// 간장질환 - C형간염표면항체 결과-1.음성 2.양성
        /// </summary>
		public string LIVER22 { get; set; } 

        /// <summary>
        /// 당뇨병-치료계획(아래참조)
        /// </summary>
		public string DIABETES_RES_CARE { get; set; } 

        /// <summary>
        /// 고혈압-치료계획(아래참조)
        /// </summary>
		public string CYCLE_RES_CARE { get; set; } 

        /// <summary>
        /// 인지기능판정(1:특이소견없음0-5점 ,2:인지기능저하~ 6점이상)
        /// </summary>
		public string T66_MEM { get; set; } 

        /// <summary>
        /// 흡연-니코틴 의존도
        /// </summary>
		public string T_SMOKE1 { get; set; } 

        /// <summary>
        /// 흡연-처방
        /// </summary>
		public string T_SMOKE2 { get; set; } 

        /// <summary>
        /// 흡연-평가점수
        /// </summary>
		public long T_SMOKE3 { get; set; } 

        /// <summary>
        /// 음주-평가
        /// </summary>
		public string T_DRINK1 { get; set; } 

        /// <summary>
        /// 음주-처방
        /// </summary>
		public string T_DRINK2 { get; set; } 

        /// <summary>
        /// 음주-평가점수
        /// </summary>
		public long T_DRINK3 { get; set; } 

        /// <summary>
        /// 운동-평가
        /// </summary>
		public string T_HELTH1 { get; set; } 

        /// <summary>
        /// 운동-처방(종류)
        /// </summary>
		public string T_HELTH2 { get; set; } 

        /// <summary>
        /// 운동-처방(시간)
        /// </summary>
		public string T_HELTH3 { get; set; } 

        /// <summary>
        /// 운동-처방(빈도,횟수)
        /// </summary>
		public string T_HELTH4 { get; set; } 

        /// <summary>
        /// 운동-평가점수
        /// </summary>
		public long T_HELTH5 { get; set; } 

        /// <summary>
        /// 영양-평가
        /// </summary>
		public string T_DIET1 { get; set; } 

        /// <summary>
        /// 영양-권장음식(유제품)
        /// </summary>
		public string T_DIET2 { get; set; } 

        /// <summary>
        /// 영양-권장음식(단백질류)
        /// </summary>
		public string T_DIET3 { get; set; } 

        /// <summary>
        /// 영양-권장음식(야채와과일)
        /// </summary>
		public string T_DIET4 { get; set; } 

        /// <summary>
        /// 영양-제한음식(지방)
        /// </summary>
		public string T_DIET5 { get; set; } 

        /// <summary>
        /// 영양-제한음식(단순당)
        /// </summary>
		public string T_DIET6 { get; set; } 

        /// <summary>
        /// 영양-제한음식(염분,소금)
        /// </summary>
		public string T_DIET7 { get; set; } 

        /// <summary>
        /// 영양-올바른식사습관(아침식사)
        /// </summary>
		public string T_DIET8 { get; set; } 

        /// <summary>
        /// 영양-올바른식사습관(골고루먹기)
        /// </summary>
		public string T_DIET9 { get; set; } 

        /// <summary>
        /// 영양-처방연계(영양교육)
        /// </summary>
		public string T_DIET10 { get; set; } 

        /// <summary>
        /// 영양-평가점수
        /// </summary>
		public long T_DIET11 { get; set; } 

        /// <summary>
        /// 비만-체질량지수
        /// </summary>
		public string T_BIMAN1 { get; set; } 

        /// <summary>
        /// 비만-허리둘레
        /// </summary>
		public string T_BIMAN2 { get; set; } 

        /// <summary>
        /// 비만-처방(1.식사량을 줄이십시오)
        /// </summary>
		public string T_BIMAN3 { get; set; } 

        /// <summary>
        /// 비만-처방(2.간식과 야식을 줄이십시오)
        /// </summary>
		public string T_BIMAN4 { get; set; } 

        /// <summary>
        /// 비만-처방(3.음주량과 횟수를 줄이십시오)
        /// </summary>
		public string T_BIMAN5 { get; set; } 

        /// <summary>
        /// 비만-처방(4.외식이나 패스트푸드를 줄이십시오)
        /// </summary>
		public string T_BIMAN6 { get; set; } 

        /// <summary>
        /// 비만-처방(5.운동처방을 참고하십시오)
        /// </summary>
		public string T_BIMAN7 { get; set; } 

        /// <summary>
        /// 비만-처방(6연계(비만클리닉))
        /// </summary>
		public string T_BIMAN8 { get; set; } 

        /// <summary>
        /// 비만-처방(7.기타)
        /// </summary>
		public string T_BIMAN9 { get; set; } 

        /// <summary>
        /// 우울증CES-D
        /// </summary>
		public string T_CESD { get; set; } 

        /// <summary>
        /// 노인성 우울증 GDS단축판
        /// </summary>
		public string T_GDS { get; set; } 

        /// <summary>
        /// 인지기능장애 KDSQ-C
        /// </summary>
		public string T_KDSQC { get; set; } 

        /// <summary>
        /// 생애상담여부(Y or Null)
        /// </summary>
		public string T_SANGDAM { get; set; } 

        /// <summary>
        /// 사후관리(R1-C)
        /// </summary>
		public string PANJENGSAHU1 { get; set; } 

        /// <summary>
        /// 사후관리(D2)
        /// </summary>
		public string PANJENGSAHU2 { get; set; } 

        /// <summary>
        /// 1차 건강진단 결과 요약(생애)
        /// </summary>
		public string T_SANGDAM_1 { get; set; } 

        /// <summary>
        /// 프린트 작업사번
        /// </summary>
		public long PRTSABUN { get; set; } 

        /// <summary>
        /// 종합소견(적극적인 관리):2015-01-08 추가
        /// </summary>
		public string SOGENB { get; set; } 

        /// <summary>
        /// 토.공휴일 가산료 여부(Y/N)
        /// </summary>
		public string GBGONGHU { get; set; }

        public string RID { get; set; }


        /// <summary>
        /// 2차 검진 결과
        /// </summary>
        public HIC_RES_BOHUM2()
        {
        }
    }
}
