namespace ComHpcLibB.Model
{


    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HIC_CANCER_NEW_JEPSU_PATIENT : BaseDto
    {

        /// <summary>
        /// 접수번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 위장조영촬영 병형
        /// </summary>
        public string STOMACH_S { get; set; }

        /// <summary>
        /// 위장조영촬영 병형기타
        /// </summary>
        public string STOMACH_B { get; set; }

        /// <summary>
        /// 위장조영촬영 부위
        /// </summary>
        public string STOMACH_P { get; set; }

        /// <summary>
        /// 위장조영촬영 부위기타
        /// </summary>
        public string STOMACH_PETC { get; set; }

        /// <summary>
        /// 위내시경 병형
        /// </summary>
        public string STOMACH_SENDO { get; set; }

        /// <summary>
        /// 위내시경 병형기타
        /// </summary>
        public string STOMACH_BENDO { get; set; }

        /// <summary>
        /// 위내시경 부위
        /// </summary>
        public string STOMACH_PENDO { get; set; }

        /// <summary>
        /// 위내시경 부위기타
        /// </summary>
        public string STOMACH_ENDOETC { get; set; }

        /// <summary>
        /// 위경검사실시여부
        /// </summary>
        public string S_ENDOGBN { get; set; }

        /// <summary>
        /// 조직검사실시여부
        /// </summary>
        public string S_ANATGBN { get; set; }

        /// <summary>
        /// 위암조직검사결과
        /// </summary>
        public string S_ANAT { get; set; }

        /// <summary>
        /// 위암조직검사결과 기타
        /// </summary>
        public string S_ANATETC { get; set; }

        /// <summary>
        /// 위암종합판정
        /// </summary>
        public string S_PANJENG { get; set; }

        /// <summary>
        /// 위암 의심()개월후 재검대상
        /// </summary>
        public string S_MONTH { get; set; }

        /// <summary>
        /// 기타질환()치료대상
        /// </summary>
        public string S_JILETC { get; set; }

        /// <summary>
        /// 위암검진장소 구분
        /// </summary>
        public string S_PLACE { get; set; }

        /// <summary>
        /// 분별잠혈반응검사
        /// </summary>
        public string COLON_RESULT { get; set; }

        /// <summary>
        /// 결장단순조영촬영,내시경검사 기 실시여부
        /// </summary>
        public string COLONGBN { get; set; }

        /// <summary>
        /// 결장단순조영촬영 병형
        /// </summary>
        public string COLON_S { get; set; }

        /// <summary>
        /// 결장단순조영촬영 병형기타
        /// </summary>
        public string COLON_B { get; set; }

        /// <summary>
        /// 결장단순조영촬영 부위
        /// </summary>
        public string COLON_P { get; set; }

        /// <summary>
        /// 결직장,S상결장경 검사방법
        /// </summary>
        public string COLON_ENDOGBN { get; set; }

        /// <summary>
        /// 결직장,S상결장경 검사 병형
        /// </summary>
        public string COLON_SENDO { get; set; }

        /// <summary>
        /// 결직장,S상결장경 검사 병형기타
        /// </summary>
        public string COLON_BENDO { get; set; }

        /// <summary>
        /// 결직장,S상결장경 검사 부위
        /// </summary>
        public string COLON_PENDO { get; set; }

        /// <summary>
        /// 결직장,S상결장경 검사 부위기타
        /// </summary>
        public string COLON_ENDOETC { get; set; }

        /// <summary>
        /// 결장단순조영촬영 내시경검사 여부
        /// </summary>
        public string C_ENDOGBN { get; set; }

        /// <summary>
        /// 결직장,S상결장경 조직검사 여부
        /// </summary>
        public string C_ANATGBN { get; set; }

        /// <summary>
        /// 대장암 조직검사결과
        /// </summary>
        public string C_ANAT { get; set; }

        /// <summary>
        /// 대장암 조직검사결과 기타
        /// </summary>
        public string C_ANATETC { get; set; }

        /// <summary>
        /// 대장암종합판정
        /// </summary>
        public string C_PANJENG { get; set; }

        /// <summary>
        /// 대장암 의심()개월후 재검대상
        /// </summary>
        public string C_MONTH { get; set; }

        /// <summary>
        /// 대장암 기타질환()치료대상
        /// </summary>
        public string C_JILETC { get; set; }

        /// <summary>
        /// 대장암 검진장소 구분
        /// </summary>
        public string C_PLACE { get; set; }

        /// <summary>
        /// 간암 초음파검사 병형
        /// </summary>
        public string LIVER_S { get; set; }

        /// <summary>
        /// 초음파검사 병형 기타 (x)
        /// </summary>
        public string LIVER_B { get; set; }

        /// <summary>
        /// 초음파검사 병변부위
        /// </summary>
        public string LIVER_P { get; set; }

        /// <summary>
        /// 간초음파 병변 크기
        /// </summary>
        public string LIVER_SIZE { get; set; }

        /// <summary>
        /// 간초음파 감암형
        /// </summary>
        public string LIVER_LSTYLE { get; set; }

        /// <summary>
        /// 간초음파소견-양성질환 2009
        /// </summary>
        public string LIVER_VIOLATE { get; set; }

        /// <summary>
        /// 간초음파소견-기타 2009
        /// </summary>
        public string LIVER_DISEASSE { get; set; }

        /// <summary>
        /// 간초음파소견-기타내역 2009
        /// </summary>
        public string LIVER_ETC { get; set; }

        /// <summary>
        /// 간암종합판정
        /// </summary>
        public string LIVER_PANJENG { get; set; }

        /// <summary>
        /// 간암 기타질환()치료대상
        /// </summary>
        public string LIVER_JILETC { get; set; }

        /// <summary>
        /// 간암 검진장소 구분
        /// </summary>
        public string LIVER_PLACE { get; set; }

        /// <summary>
        /// 유방암 단순촬영 판독소견
        /// </summary>
        public string BREAST_S { get; set; }

        /// <summary>
        /// 유방암 단수촬영 부위
        /// </summary>
        public string BREAST_P { get; set; }

        /// <summary>
        /// 유방암 단순촬영 부위 기타(우)
        /// </summary>
        public string BREAST_ETC { get; set; }

        /// <summary>
        /// 유방암 실질분포량 2009
        /// </summary>
        public string B_ANATGBN { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string B_ANAT { get; set; }

        /// <summary>
        /// 유방암 단순촬영 부위 기타(좌)
        /// </summary>
        public string B_ANATETC { get; set; }

        /// <summary>
        /// 유방암 종합판정
        /// </summary>
        public string B_PANJENG { get; set; }

        /// <summary>
        /// 유방암 의심()개월후재검대상
        /// </summary>
        public string B_MONTH { get; set; }

        /// <summary>
        /// 유방암 기타질환()치료대상
        /// </summary>
        public string B_JILETC { get; set; }

        /// <summary>
        /// 유방암 검진장소 구분
        /// </summary>
        public string B_PLACE { get; set; }

        /// <summary>
        /// 신장
        /// </summary>
        public long HEIGHT { get; set; }

        /// <summary>
        /// 체중 ,2009년부터 체중감소 kg들어감
        /// </summary>
        public long WEIGHT { get; set; }

        /// <summary>
        /// 검사종류 위암
        /// </summary>
        public string GBSTOMACH { get; set; }

        /// <summary>
        /// 검사종류 간암
        /// </summary>
        public string GBLIVER { get; set; }

        /// <summary>
        /// 검사종류 대장암
        /// </summary>
        public string GBRECTUM { get; set; }

        /// <summary>
        /// 검사종류 유방암
        /// </summary>
        public string GBBREAST { get; set; }

        /// <summary>
        /// 과거병력:위십이지장궤양 유무
        /// </summary>
        public string SICK11 { get; set; }

        /// <summary>
        /// 과거병력:위십이지장궤양 발병년도
        /// </summary>
        public string SICK12 { get; set; }

        /// <summary>
        /// 과거병력:위수술(절제술) 유무
        /// </summary>
        public string SICK21 { get; set; }

        /// <summary>
        /// 과거병력:위수술(절제술) 발병년도
        /// </summary>
        public string SICK22 { get; set; }

        /// <summary>
        /// 과거병력:B형간염보균상태 유무
        /// </summary>
        public string SICK31 { get; set; }

        /// <summary>
        /// 과거병력:B형간염보균상태 발병년도
        /// </summary>
        public string SICK32 { get; set; }

        /// <summary>
        /// 과거병력:간염 유무
        /// </summary>
        public string SICK41 { get; set; }

        /// <summary>
        /// 과거병력:간염 발병년도
        /// </summary>
        public string SICK42 { get; set; }

        /// <summary>
        /// 과거병력:간경변 유무
        /// </summary>
        public string SICK51 { get; set; }

        /// <summary>
        /// 과거병력:간경변 발병년도
        /// </summary>
        public string SICK52 { get; set; }

        /// <summary>
        /// 과거병력:대장종(폴립,혹) 유무
        /// </summary>
        public string SICK61 { get; set; }

        /// <summary>
        /// 과거병력:대장종(폴립,혹) 발병년도
        /// </summary>
        public string SICK62 { get; set; }

        /// <summary>
        /// 과거병력:궤양성대장염 유무
        /// </summary>
        public string SICK71 { get; set; }

        /// <summary>
        /// 과거병력:궤양성대장염 발병년도
        /// </summary>
        public string SICK72 { get; set; }

        /// <summary>
        /// 과거병력:양성유방질환 유무
        /// </summary>
        public string SICK81 { get; set; }

        /// <summary>
        /// 과거병력:양성유방질환 발병년도
        /// </summary>
        public string SICK82 { get; set; }

        /// <summary>
        /// 자주발생한증상:불면증
        /// </summary>
        public string JUNGSANG01 { get; set; }

        /// <summary>
        /// 자주발생한증상:피로
        /// </summary>
        public string JUNGSANG02 { get; set; }

        /// <summary>
        /// 자주발생한증상:기침
        /// </summary>
        public string JUNGSANG03 { get; set; }

        /// <summary>
        /// 자주발생한증상:가래
        /// </summary>
        public string JUNGSANG04 { get; set; }

        /// <summary>
        /// 자주발생한증상:흉부통증
        /// </summary>
        public string JUNGSANG05 { get; set; }

        /// <summary>
        /// 자주발생한증상:현기증
        /// </summary>
        public string JUNGSANG06 { get; set; }

        /// <summary>
        /// 자주발생한증상:호흡곤란
        /// </summary>
        public string JUNGSANG07 { get; set; }

        /// <summary>
        /// 자주발생한증상:장복부통증
        /// </summary>
        public string JUNGSANG08 { get; set; }

        /// <summary>
        /// 자주발생한증상:복부팽만감
        /// </summary>
        public string JUNGSANG09 { get; set; }

        /// <summary>
        /// 자주발생한증상:소화불량
        /// </summary>
        public string JUNGSANG10 { get; set; }

        /// <summary>
        /// 자주발생한증상:설사
        /// </summary>
        public string JUNGSANG11 { get; set; }

        /// <summary>
        /// 자주발생한증상:변비
        /// </summary>
        public string JUNGSANG12 { get; set; }

        /// <summary>
        /// 자주발생한증상:혈변
        /// </summary>
        public string JUNGSANG13 { get; set; }

        /// <summary>
        /// 자주발생한증상:하복부통증
        /// </summary>
        public string JUNGSANG14 { get; set; }

        /// <summary>
        /// 자주발생한증상:체중변화(최근1년간 3kg이상 변화)
        /// </summary>
        public string JUNGSANG15 { get; set; }

        /// <summary>
        /// 가족력유무
        /// </summary>
        public string GAJOK1 { get; set; }

        /// <summary>
        /// 가족력기타
        /// </summary>
        public string GAJOKETC { get; set; }

        /// <summary>
        /// 음주습관
        /// </summary>
        public string DRINK1 { get; set; }

        /// <summary>
        /// 1회음주량
        /// </summary>
        public string DRINK2 { get; set; }

        /// <summary>
        /// 흡연
        /// </summary>
        public string SMOKING1 { get; set; }

        /// <summary>
        /// 하루흡연량
        /// </summary>
        public string SMOKING2 { get; set; }

        /// <summary>
        /// 초경연령
        /// </summary>
        public string WOMAN1 { get; set; }

        /// <summary>
        /// 폐경여부
        /// </summary>
        public string WOMAN2 { get; set; }

        /// <summary>
        /// 폐경연령
        /// </summary>
        public string WOMAN3 { get; set; }

        /// <summary>
        /// 여성호로몬투약 유무
        /// </summary>
        public string WOMAN4 { get; set; }

        /// <summary>
        /// 여성호로몬 사용연령
        /// </summary>
        public string WOMAN5 { get; set; }

        /// <summary>
        /// 여성호로몬 사용기간
        /// </summary>
        public string WOMAN6 { get; set; }

        /// <summary>
        /// 출산여부
        /// </summary>
        public string WOMAN7 { get; set; }

        /// <summary>
        /// 출산횟수
        /// </summary>
        public string WOMAN8 { get; set; }

        /// <summary>
        /// 첫출산연령
        /// </summary>
        public string WOMAN9 { get; set; }

        /// <summary>
        /// 모유기간
        /// </summary>
        public string WOMAN10 { get; set; }

        /// <summary>
        /// 통보방법
        /// </summary>
        public string TONGBOGBN { get; set; }

        /// <summary>
        /// 통보일자
        /// </summary>
        public string TONGBODATE { get; set; }

        /// <summary>
        /// 판정의사번호
        /// </summary>
        public long PANJENGDRNO { get; set; }

        /// <summary>
        /// 판정소견 -2008년에 사용안함
        /// </summary>
        public string SOGEN { get; set; }

        /// <summary>
        /// 검진일자
        /// </summary>
        public string GUNDATE { get; set; }

        /// <summary>
        /// 진찰료포함여부 - 2007년이후 사용안함
        /// </summary>
        public string JINCHALGBN { get; set; }

        /// <summary>
        /// 판정의사명- 2007년이후 사용안함
        /// </summary>
        public string DRNAME { get; set; }

        /// <summary>
        /// 알파휘토단백-RPHA법(1.음성, 2.양성)
        /// </summary>
        public string LIVER_RPHA { get; set; }

        /// <summary>
        /// 알파휘토단백-EIA법
        /// </summary>
        public string LIVER_EIA { get; set; }

        /// <summary>
        /// 결과지 인쇄여부(인쇄 : Y )
        /// </summary>
        public string GBPRINT { get; set; }

        /// <summary>
        /// 결장단순조영촬영 부위기타
        /// </summary>
        public string COLON_PETC { get; set; }

        /// <summary>
        /// 자궁경부암 검체상태 (1.적절 ,2.부적절)
        /// </summary>
        public string WOMB01 { get; set; }

        /// <summary>
        /// 자궁경암 선상피세포 (1.유, 2.무)
        /// </summary>
        public string WOMB02 { get; set; }

        /// <summary>
        /// 자궁경부암 유형별진단(1.음성, 2.상피세포, 3.기타)
        /// </summary>
        public string WOMB03 { get; set; }

        /// <summary>
        /// 자궁경부암 편평상피 세포이상(참고사항 참조)
        /// </summary>
        public string WOMB04 { get; set; }

        /// <summary>
        /// 자궁경부암 선상피세포이상 (참고사항 참조)
        /// </summary>
        public string WOMB05 { get; set; }

        /// <summary>
        /// 자궁경부암 선상피세포이상 기타
        /// </summary>
        public string WOMB06 { get; set; }

        /// <summary>
        /// 자궁경부암 추가소견 (참고사항 참조)
        /// </summary>
        public string WOMB07 { get; set; }

        /// <summary>
        /// 자궁경부암 추가소견 기타
        /// </summary>
        public string WOMB08 { get; set; }

        /// <summary>
        /// 자궁경부암 종합판정 (참고사항 참조)
        /// </summary>
        public string WOMB09 { get; set; }

        /// <summary>
        /// 자궁경부암 종합판정 기타
        /// </summary>
        public string WOMB10 { get; set; }

        /// <summary>
        /// 자궁경부암 유형별진단 기타-기타(자궁내막세포출현등)
        /// </summary>
        public string WOMB11 { get; set; }

        /// <summary>
        /// 검사종류 자궁경부암
        /// </summary>
        public string GBWOMB { get; set; }

        /// <summary>
        /// 자궁경부암검진장소 구분
        /// </summary>
        public string WOMB_PLACE { get; set; }

        /// <summary>
        /// 생리이상출혈 2005년적용 (1.예, 2.아니오)
        /// </summary>
        public string WOMAN11 { get; set; }

        /// <summary>
        /// 비정형 편평상피세포 위험구분
        /// </summary>
        public string WOMAN12 { get; set; }

        /// <summary>
        /// 결혼연령 2005년적용 (참고사항 참조)
        /// </summary>
        public string WOMAN13 { get; set; }

        /// <summary>
        /// 과거병력:산부인과 질환 유무
        /// </summary>
        public string SICK91 { get; set; }

        /// <summary>
        /// 과거병력:산부인과 질환 발병년도
        /// </summary>
        public string SICK92 { get; set; }

        /// <summary>
        /// 간암 - ALT (0-9999)
        /// </summary>
        public string LIVER_NEW_ALT { get; set; }

        /// <summary>
        /// 간암 - B형간염항원 (1.음성, 2.양성)
        /// </summary>
        public string LIVER_NEW_B { get; set; }

        /// <summary>
        /// 간암 - ALT 및 B형간염항원 결과 (아래참조)
        /// </summary>
        public string LIVER_NEW_BRESULT { get; set; }

        /// <summary>
        /// 간암 - C형감염항체 (1.음성, 2.양성)
        /// </summary>
        public string LIVER_NEW_C { get; set; }

        /// <summary>
        /// 간암 - C형간염항체 결과(1.정상,2.암검진대상,3.암검진대상에서제외)
        /// </summary>
        public string LIVER_NEW_CRESULT { get; set; }

        /// <summary>
        /// 3.과거병력-위암
        /// </summary>
        public string NEW_SICK01 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK02 { get; set; }

        /// <summary>
        /// 가족관계
        /// </summary>
        public string NEW_SICK03 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK04 { get; set; }

        /// <summary>
        /// 3.과거병력-유방암
        /// </summary>
        public string NEW_SICK06 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK07 { get; set; }

        /// <summary>
        /// 가족관계
        /// </summary>
        public string NEW_SICK08 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK09 { get; set; }

        /// <summary>
        /// 3.과거병력-대장암
        /// </summary>
        public string NEW_SICK11 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK12 { get; set; }

        /// <summary>
        /// 가족관계
        /// </summary>
        public string NEW_SICK13 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK14 { get; set; }

        /// <summary>
        /// 3.과거병력-자궁경부암
        /// </summary>
        public string NEW_SICK16 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK17 { get; set; }

        /// <summary>
        /// 가족관계
        /// </summary>
        public string NEW_SICK18 { get; set; }

        /// <summary>
        /// 1.현재 신체 불편증상여부
        /// </summary>
        public string NEW_SICK19 { get; set; }

        /// <summary>
        /// 1.현재 신체 불편증상내역
        /// </summary>
        public string NEW_SICK20 { get; set; }

        /// <summary>
        /// 3.과거병력-간암
        /// </summary>
        public string NEW_SICK21 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK22 { get; set; }

        /// <summary>
        /// 가족관계
        /// </summary>
        public string NEW_SICK23 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK24 { get; set; }

        /// <summary>
        /// 6.간질환여부
        /// </summary>
        public string NEW_SICK25 { get; set; }

        /// <summary>
        /// 3.과거병력-기타
        /// </summary>
        public string NEW_SICK26 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK27 { get; set; }

        /// <summary>
        /// 가족관계
        /// </summary>
        public string NEW_SICK28 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK29 { get; set; }

        /// <summary>
        /// 기타암명칭
        /// </summary>
        public string NEW_SICK30 { get; set; }

        /// <summary>
        /// 4.만성염증성 대장질환
        /// </summary>
        public string NEW_SICK31 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK32 { get; set; }

        /// <summary>
        /// 대장이중조영 용종크기-2009년도
        /// </summary>
        public string NEW_SICK33 { get; set; }

        /// <summary>
        /// 대장내시경 용종절제여부-2009년도
        /// </summary>
        public string NEW_SICK34 { get; set; }

        /// <summary>
        /// 5.선종성 대장용종
        /// </summary>
        public string NEW_SICK36 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK37 { get; set; }

        /// <summary>
        /// 대장내시경 대장용종크기 - 2009년도
        /// </summary>
        public string NEW_SICK38 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK39 { get; set; }

        /// <summary>
        /// 7.김치제외 채소 먹는양?
        /// </summary>
        public string NEW_SICK41 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK42 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK43 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK44 { get; set; }

        /// <summary>
        /// 8.일주일에 과일 먹는횟수?
        /// </summary>
        public string NEW_SICK46 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK47 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK48 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK49 { get; set; }

        /// <summary>
        /// 9.일주일에 고기먹는 횟수?
        /// </summary>
        public string NEW_SICK51 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK52 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK53 { get; set; }

        /// <summary>
        /// 위내시경 용종개수 - 2009년
        /// </summary>
        public string NEW_SICK54 { get; set; }

        /// <summary>
        /// 10.음식의 간은 어떻게 느끼십니가?
        /// </summary>
        public string NEW_SICK56 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK57 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_SICK58 { get; set; }

        /// <summary>
        /// 대장내시경 용종개수 - 2009년
        /// </summary>
        public string NEW_SICK59 { get; set; }

        /// <summary>
        /// 위장질환 - 2009년
        /// </summary>
        public string NEW_SICK61 { get; set; }

        /// <summary>
        /// 대장 항문질환 - 2009년
        /// </summary>
        public string NEW_SICK62 { get; set; }

        /// <summary>
        /// 2009 위암조직진단시-암
        /// </summary>
        public string NEW_SICK63 { get; set; }

        /// <summary>
        /// 2009 위암조직진단
        /// </summary>
        public string NEW_SICK64 { get; set; }

        /// <summary>
        /// 2009 위암조직진단시-암기타
        /// </summary>
        public string NEW_SICK66 { get; set; }

        /// <summary>
        /// 2009 위암조직진단시기타
        /// </summary>
        public string NEW_SICK67 { get; set; }

        /// <summary>
        /// 2009 위암조직진단시기타-기타
        /// </summary>
        public string NEW_SICK68 { get; set; }

        /// <summary>
        /// 2009 대장암조직진단시-암
        /// </summary>
        public string NEW_SICK69 { get; set; }

        /// <summary>
        /// 2009 대장암조직진단
        /// </summary>
        public string NEW_SICK71 { get; set; }

        /// <summary>
        /// 2009 대장암조직진단시-암기타
        /// </summary>
        public string NEW_SICK72 { get; set; }

        /// <summary>
        /// 2009 대장암조직진단시기타
        /// </summary>
        public string NEW_SICK73 { get; set; }

        /// <summary>
        /// 2009 대장암조직진단시기타-기타
        /// </summary>
        public string NEW_SICK74 { get; set; }

        /// <summary>
        /// B형간염접종여부
        /// </summary>
        public string NEW_B_SICK01 { get; set; }

        /// <summary>
        /// 몇번 맞았나?
        /// </summary>
        public string NEW_B_SICK02 { get; set; }

        /// <summary>
        /// 언제 맞았나 1
        /// </summary>
        public string NEW_B_SICK03 { get; set; }

        /// <summary>
        /// 언제 맞았나 2
        /// </summary>
        public string NEW_B_SICK04 { get; set; }

        /// <summary>
        /// 언제 맞았나 3
        /// </summary>
        public string NEW_B_SICK05 { get; set; }

        /// <summary>
        /// B형간염접종여부 - 모름
        /// </summary>
        public string NEW_B_SICK06 { get; set; }

        /// <summary>
        /// 침 맞은 경험
        /// </summary>
        public string NEW_N_SICK01 { get; set; }

        /// <summary>
        /// 침 맞은 년도
        /// </summary>
        public string NEW_N_SICK02 { get; set; }

        /// <summary>
        /// 침 맞은 횟수
        /// </summary>
        public string NEW_N_SICK03 { get; set; }

        /// <summary>
        /// 수혈받은 경험
        /// </summary>
        public string NEW_S_SICK01 { get; set; }

        /// <summary>
        /// 수술받은 년도
        /// </summary>
        public string NEW_S_SICK02 { get; set; }

        /// <summary>
        /// 몇번 수술?
        /// </summary>
        public string NEW_S_SICK03 { get; set; }

        /// <summary>
        /// 총수혈량?
        /// </summary>
        public string NEW_S_SICK04 { get; set; }

        /// <summary>
        /// 위투시촬영 유무
        /// </summary>
        public string NEW_CAN_01 { get; set; }

        /// <summary>
        /// 위투시촬영 횟수?
        /// </summary>
        public string NEW_CAN_02 { get; set; }

        /// <summary>
        /// 위투시촬영 마지막받은 년도?
        /// </summary>
        public string NEW_CAN_03 { get; set; }

        /// <summary>
        /// 위투시촬영 최종결과
        /// </summary>
        public string NEW_CAN_04 { get; set; }

        /// <summary>
        /// 위내시경 유무
        /// </summary>
        public string NEW_CAN_06 { get; set; }

        /// <summary>
        /// 위내시경 횟수?
        /// </summary>
        public string NEW_CAN_07 { get; set; }

        /// <summary>
        /// 위내시경 마지막받은 년도?
        /// </summary>
        public string NEW_CAN_08 { get; set; }

        /// <summary>
        /// 위내시경 최종결과
        /// </summary>
        public string NEW_CAN_09 { get; set; }

        /// <summary>
        /// 흉부나복부의 CT,MRI 유무
        /// </summary>
        public string NEW_CAN_11 { get; set; }

        /// <summary>
        /// 흉부나복부의 CT,MRI 횟수?
        /// </summary>
        public string NEW_CAN_12 { get; set; }

        /// <summary>
        /// 흉부나복부의 CT,MRI 마지막받은 년도?
        /// </summary>
        public string NEW_CAN_13 { get; set; }

        /// <summary>
        /// 흉부나복부의 CT,MRI 최종결과
        /// </summary>
        public string NEW_CAN_14 { get; set; }

        /// <summary>
        /// 간초음파 유무
        /// </summary>
        public string NEW_CAN_16 { get; set; }

        /// <summary>
        /// 간초음파 횟수?
        /// </summary>
        public string NEW_CAN_17 { get; set; }

        /// <summary>
        /// 간초음파 마지막받은 년도?
        /// </summary>
        public string NEW_CAN_18 { get; set; }

        /// <summary>
        /// 간초음파 최종결과
        /// </summary>
        public string NEW_CAN_19 { get; set; }

        /// <summary>
        /// 대변잠혈검사 유무
        /// </summary>
        public string NEW_CAN_21 { get; set; }

        /// <summary>
        /// 대변잠혈검사 횟수?
        /// </summary>
        public string NEW_CAN_22 { get; set; }

        /// <summary>
        /// 대변잠혈검사 마지막받은 년도?
        /// </summary>
        public string NEW_CAN_23 { get; set; }

        /// <summary>
        /// 대변잠혈검사 최종결과
        /// </summary>
        public string NEW_CAN_24 { get; set; }

        /// <summary>
        /// 대장내시경 유무
        /// </summary>
        public string NEW_CAN_26 { get; set; }

        /// <summary>
        /// 대장내시경 횟수?
        /// </summary>
        public string NEW_CAN_27 { get; set; }

        /// <summary>
        /// 대장내시경 마지막받은 년도?
        /// </summary>
        public string NEW_CAN_28 { get; set; }

        /// <summary>
        /// 대장내시경 최종결과
        /// </summary>
        public string NEW_CAN_29 { get; set; }

        /// <summary>
        /// 13.하루 30분 이상 운동 일주일에 몇?
        /// </summary>
        public string NEW_HARD { get; set; }

        /// <summary>
        /// 결혼상태?
        /// </summary>
        public string NEW_MARRIED { get; set; }

        /// <summary>
        /// 최종학력?
        /// </summary>
        public string NEW_SCHOOL { get; set; }

        /// <summary>
        /// 직업?
        /// </summary>
        public string NEW_WORK01 { get; set; }

        /// <summary>
        /// 직업 기타
        /// </summary>
        public string NEW_WORK02 { get; set; }

        /// <summary>
        /// 11.흡연상태?
        /// </summary>
        public string NEW_SMOKE01 { get; set; }

        /// <summary>
        /// 금연 몇년?
        /// </summary>
        public string NEW_SMOKE02 { get; set; }

        /// <summary>
        /// 12.흡연량? 몇세부터
        /// </summary>
        public string NEW_SMOKE03 { get; set; }

        /// <summary>
        /// 흡연량? 총 몇년간
        /// </summary>
        public string NEW_SMOKE04 { get; set; }

        /// <summary>
        /// 흡연량? 하루평균개피?
        /// </summary>
        public string NEW_SMOKE05 { get; set; }

        /// <summary>
        /// 14.음주상태?
        /// </summary>
        public string NEW_DRINK01 { get; set; }

        /// <summary>
        /// 금주 몇년?
        /// </summary>
        public string NEW_DRINK02 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_DRINK03 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_DRINK04 { get; set; }

        /// <summary>
        /// 15.음주량? 음주횟수?
        /// </summary>
        public string NEW_DRINK05 { get; set; }

        /// <summary>
        /// 한번 음주시 평균?
        /// </summary>
        public string NEW_DRINK06 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_DRINK07 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_DRINK08 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_DRINK09 { get; set; }

        /// <summary>
        /// 17.초경연령?
        /// </summary>
        public string NEW_WOMAN01 { get; set; }

        /// <summary>
        /// 초경연령 만 몇세?
        /// </summary>
        public string NEW_WOMAN02 { get; set; }

        /// <summary>
        /// 2007년 신규-진촬상담여부(ex.1^^1^^0^^0^^1^^)위.대장.간.유방.자궁
        /// </summary>
        public string NEW_WOMAN03 { get; set; }

        /// <summary>
        /// 18.생리여부?
        /// </summary>
        public string NEW_WOMAN11 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_WOMAN12 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_WOMAN13 { get; set; }

        /// <summary>
        /// 폐경연령?
        /// </summary>
        public string NEW_WOMAN14 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_WOMAN15 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_WOMAN16 { get; set; }

        /// <summary>
        /// 유방암소견 기타()-2009년
        /// </summary>
        public string NEW_WOMAN17 { get; set; }

        /// <summary>
        /// 19.여성호르몬 여부
        /// </summary>
        public string NEW_WOMAN18 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_WOMAN19 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_WOMAN20 { get; set; }

        /// <summary>
        /// 20.출산몇명?
        /// </summary>
        public string NEW_WOMAN21 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_WOMAN22 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_WOMAN23 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_WOMAN24 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_WOMAN25 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_WOMAN26 { get; set; }

        /// <summary>
        /// 21.과거에 유방에 양성 종양진단여부?
        /// </summary>
        public string NEW_WOMAN27 { get; set; }

        /// <summary>
        /// 22.피임약 복용여부?
        /// </summary>
        public string NEW_WOMAN31 { get; set; }

        /// <summary>
        /// 판정면허번호1-위암
        /// </summary>
        public string NEW_WOMAN32 { get; set; }

        /// <summary>
        /// 판정면허번호2-대장
        /// </summary>
        public string NEW_WOMAN33 { get; set; }

        /// <summary>
        /// 판정면허번호3-간
        /// </summary>
        public string NEW_WOMAN34 { get; set; }

        /// <summary>
        /// 판정면허번호4-유방
        /// </summary>
        public string NEW_WOMAN35 { get; set; }

        /// <summary>
        /// 판정면허번호5-자궁
        /// </summary>
        public string NEW_WOMAN36 { get; set; }

        /// <summary>
        /// 모유수유기간 2009년
        /// </summary>
        public string NEW_WOMAN41 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_WOMAN42 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_WOMAN43 { get; set; }

        /// <summary>
        /// 16.암검사 목적 - 상부위장관조영술
        /// </summary>
        public string NEW_CAN_WOMAN01 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_CAN_WOMAN02 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_CAN_WOMAN03 { get; set; }

        /// <summary>
        /// 16.암검사 목적 - 위내시경
        /// </summary>
        public string NEW_CAN_WOMAN04 { get; set; }

        /// <summary>
        /// 16.암검사 목적 - 분변 잠혈검사
        /// </summary>
        public string NEW_CAN_WOMAN06 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_CAN_WOMAN07 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_CAN_WOMAN08 { get; set; }

        /// <summary>
        /// 16.암검사 목적 - 대장이중조영술
        /// </summary>
        public string NEW_CAN_WOMAN09 { get; set; }

        /// <summary>
        /// 16.암검사 목적 - 대장 내시경
        /// </summary>
        public string NEW_CAN_WOMAN11 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_CAN_WOMAN12 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_CAN_WOMAN13 { get; set; }

        /// <summary>
        /// 16.암검사 목적 - 간초음파
        /// </summary>
        public string NEW_CAN_WOMAN14 { get; set; }

        /// <summary>
        /// 23.암검사 목적 - 유방단순촬영
        /// </summary>
        public string NEW_CAN_WOMAN16 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_CAN_WOMAN17 { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string NEW_CAN_WOMAN18 { get; set; }

        /// <summary>
        /// 23.암검사 목적 - 자궁경부암 검사
        /// </summary>
        public string NEW_CAN_WOMAN19 { get; set; }

        /// <summary>
        /// 암검진 청구제외구분(ex..10101)
        /// </summary>
        public string CAN_MIRGBN { get; set; }

        /// <summary>
        /// 위장조영촬영 권고사항
        /// </summary>
        public string S_SOGEN { get; set; }

        /// <summary>
        /// 분변잠혈 권고사항
        /// </summary>
        public string C_SOGEN { get; set; }

        /// <summary>
        /// 간암 권고사항
        /// </summary>
        public string L_SOGEN { get; set; }

        /// <summary>
        /// 유방암 권고사항
        /// </summary>
        public string B_SOGEN { get; set; }

        /// <summary>
        /// 자궁경부암 권고사항
        /// </summary>
        public string W_SOGEN { get; set; }

        /// <summary>
        /// 위암 판정일자
        /// </summary>
        public string S_PANJENGDATE { get; set; }

        /// <summary>
        /// 대장암 판정일자
        /// </summary>
        public string C_PANJENGDATE { get; set; }

        /// <summary>
        /// 간암 판정일자
        /// </summary>
        public string L_PANJENGDATE { get; set; }

        /// <summary>
        /// 유방암 판정일자
        /// </summary>
        public string B_PANJENGDATE { get; set; }

        /// <summary>
        /// 자궁경부암 판정일자
        /// </summary>
        public string W_PANJENGDATE { get; set; }

        /// <summary>
        /// 상부소화관내시경 권고사항
        /// </summary>
        public string S_SOGEN2 { get; set; }

        /// <summary>
        /// 결장이중조영 권고사항
        /// </summary>
        public string C_SOGEN2 { get; set; }

        /// <summary>
        /// 결장경 권고사항
        /// </summary>
        public string C_SOGEN3 { get; set; }

        /// <summary>
        /// 암판정 상담내역
        /// </summary>
        public string SANGDAM { get; set; }

        /// <summary>
        /// 프린트 작업사번
        /// </summary>
        public long PRTSABUN { get; set; }

        /// <summary>
        /// 유방암 단순촬영 부위 기타2(우)
        /// </summary>
        public string BREAST_ETC2 { get; set; }

        /// <summary>
        /// 유방암 단순촬영 부위 기타3(우)
        /// </summary>
        public string BREAST_ETC3 { get; set; }

        /// <summary>
        /// 유방암 단순촬영 부위 기타3(좌)
        /// </summary>
        public string B_ANATETC3 { get; set; }

        /// <summary>
        /// 유방암 단순촬영 부위 기타2(좌)
        /// </summary>
        public string B_ANATETC2 { get; set; }

        /// <summary>
        /// 판정완료유무
        /// </summary>
        public string PANJENGYN { get; set; }

        /// <summary>
        /// 자궁경부암 중복자궁(1.해당없음 2.해당)
        /// </summary>
        public string WOMB12 { get; set; }

        /// <summary>
        /// 위조직진단
        /// </summary>
        public string RESULT0001 { get; set; }

        /// <summary>
        /// 맹장삽입여부
        /// </summary>
        public string RESULT0002 { get; set; }

        /// <summary>
        /// 장정결도
        /// </summary>
        public string RESULT0003 { get; set; }

        /// <summary>
        /// 검사소견
        /// </summary>
        public string RESULT0004 { get; set; }

        /// <summary>
        /// 간암1cm미만 종괴 병변위치
        /// </summary>
        public string RESULT0005 { get; set; }

        /// <summary>
        /// 간암1cm이상 종괴 병변위치
        /// </summary>
        public string RESULT0006 { get; set; }

        /// <summary>
        /// 병변크기1
        /// </summary>
        public string RESULT0007 { get; set; }

        /// <summary>
        /// 병변크기2
        /// </summary>
        public string RESULT0008 { get; set; }

        /// <summary>
        /// 병변크기3
        /// </summary>
        public string RESULT0009 { get; set; }

        /// <summary>
        /// 간암기타소견
        /// </summary>
        public string RESULT0010 { get; set; }

        /// <summary>
        /// 간암기타소견 - 6.담낭이상
        /// </summary>
        public string RESULT0011 { get; set; }

        /// <summary>
        /// 간암기타소견 - 7.기타
        /// </summary>
        public string RESULT0012 { get; set; }

        /// <summary>
        /// 유방촬영검사(1.양측 2.편측(우) 3.편측(좌)
        /// </summary>
        public string RESULT0013 { get; set; }

        /// <summary>
        /// 자궁-비정상 선상피세표 (1.일반 2.종양성)
        /// </summary>
        public string RESULT0014 { get; set; }

        /// <summary>
        /// 간암 - 간낭종
        /// </summary>
        public string RESULT0015 { get; set; }

        /// <summary>
        /// 위암 - 이물제거술
        /// </summary>
        public string RESULT0016 { get; set; }

        /// <summary>
        /// 위암 - 판독의사
        /// </summary>
        public long PANJENGDRNO1 { get; set; }

        /// <summary>
        /// 위암 - 검사의사
        /// </summary>
        public long PANJENGDRNO2 { get; set; }

        /// <summary>
        /// 위암 - 병리진단의사
        /// </summary>
        public long PANJENGDRNO3 { get; set; }

        /// <summary>
        /// 대장암 - 판독의사
        /// </summary>
        public long PANJENGDRNO4 { get; set; }

        /// <summary>
        /// 대장암 - 검사의사
        /// </summary>
        public long PANJENGDRNO5 { get; set; }

        /// <summary>
        /// 대장암 - 병리진단의사
        /// </summary>
        public long PANJENGDRNO6 { get; set; }

        /// <summary>
        /// 간암 - 검사의사
        /// </summary>
        public long PANJENGDRNO7 { get; set; }

        /// <summary>
        /// 유방암 - 판독의사
        /// </summary>
        public long PANJENGDRNO8 { get; set; }

        /// <summary>
        /// 자궁경부암 - 검체채취의사
        /// </summary>
        public long PANJENGDRNO9 { get; set; }

        /// <summary>
        /// 자궁경부암 - 병리진단의사
        /// </summary>
        public long PANJENGDRNO10 { get; set; }

        /// <summary>
        /// 폐암 - 판독의사
        /// </summary>
        public long PANJENGDRNO11 { get; set; }

        /// <summary>
        /// 3.과거병력-폐암
        /// </summary>
        public string NEW_SICK75 { get; set; }

        /// <summary>
        /// 과거병력-폐암(가족관계)
        /// </summary>
        public string NEW_SICK76 { get; set; }

        /// <summary>
        /// 8.폐질환 여부
        /// </summary>
        public string NEW_SICK77 { get; set; }

        /// <summary>
        /// 4.폐암-흉부CT 검사내역
        /// </summary>
        public string NEW_SICK78 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBLUNG { get; set; }

    /// <summary>
    /// 
        /// </summary>
        public string L_PANJENGDATE1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long LUNG_RESULT001 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT002 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT003 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT004 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT005 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT006 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT007 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT008 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT009 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT010 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT011 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT012 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT013 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT014 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT015 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT016 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT017 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT018 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT019 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT020 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT021 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT022 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT023 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT024 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT025 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT026 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT027 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT028 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT029 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT030 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT031 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT032 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT033 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT034 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT035 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT036 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT037 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT038 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT039 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT040 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT041 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT042 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT043 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT044 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT045 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT046 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT047 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT048 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT049 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT050 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT051 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT052 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT053 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT054 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT055 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT056 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT057 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT058 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT059 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT060 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT061 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT062 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT063 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT064 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT065 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT066 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT067 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT068 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT069 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT070 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT071 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT072 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT073 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT074 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT075 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT076 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT077 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT078 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_PLACE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NEW_WOMAN37 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT079 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_RESULT080 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_SANGDAM1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_SANGDAM2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_SANGDAM3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LUNG_SANGDAM4 { get; set; }




        /// <summary>
        /// 등록번호
        /// </summary>
        public long PANO { get; set; }

        /// <summary>
        /// 성명
        /// </summary>
        public string SNAME { get; set; }

        /// <summary>
        /// 주민등록번호
        /// </summary>
        public string JUMIN { get; set; }

        /// <summary>
        /// 성별(M/F)
        /// </summary>
        public string SEX { get; set; }

        /// <summary>
        /// 우편번호
        /// </summary>
        public string MAILCODE { get; set; }

        /// <summary>
        /// 주소①
        /// </summary>
        public string JUSO1 { get; set; }

        /// <summary>
        /// 주소②
        /// </summary>
        public string JUSO2 { get; set; }

        /// <summary>
        /// 전화번호
        /// </summary>
        public string TEL { get; set; }

        /// <summary>
        /// 회사코드
        /// </summary>
        public long LTDCODE { get; set; }

        /// <summary>
        /// 직종구분(1.생산직 2.특수직 3.사무직)
        /// </summary>
        public string JIKGBN { get; set; }

        /// <summary>
        /// 직종(공단코드)
        /// </summary>
        public string JIKJONG { get; set; }

        /// <summary>
        /// 사원번호
        /// </summary>
        public string SABUN { get; set; }

        /// <summary>
        /// 부서명칭
        /// </summary>
        public string BUSENAME { get; set; }

        /// <summary>
        /// 공정코드(공단코드)
        /// </summary>
        public string GONGJENG { get; set; }

        /// <summary>
        /// 회사 입사일자
        /// </summary>
        public string IPSADATE { get; set; }

        /// <summary>
        /// 현직 전입일자
        /// </summary>
        public string BUSEIPSA { get; set; }

        /// <summary>
        /// 건강보험 지사코드
        /// </summary>
        public string JISA { get; set; }

        /// <summary>
        /// 증번호
        /// </summary>
        public string GKIHO { get; set; }

        /// <summary>
        /// 수첩소지여부(Y/N)
        /// </summary>
        public string GBSUCHEP { get; set; }

        /// <summary>
        /// 원무행정의 환자 등록번호
        /// </summary>
        public string PTNO { get; set; }

        /// <summary>
        /// 참고사항
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 사업장기호
        /// </summary>
        public string KIHO { get; set; }

        /// <summary>
        /// 보건소
        /// </summary>
        public string BOGUNSO { get; set; }

        /// <summary>
        /// 영업소
        /// </summary>
        public string YOUNGUPSO { get; set; }

        /// <summary>
        /// 2차간염대상자
        /// </summary>
        public string LIVER2 { get; set; }

        /// <summary>
        /// 검진대상    예) 1.직장가입자 2.지역,피부양자
        /// </summary>
        public string GUMDAESANG { get; set; }

        /// <summary>
        /// E-mail(문진표 작성시 동의한 사람만 입력)
        /// </summary>
        public string EMAIL { get; set; }

        /// <summary>
        /// 핸드폰번호
        /// </summary>
        public string HPHONE { get; set; }

        /// <summary>
        /// 인터넷문진여부(Y:했음 / N or Null:해당없음)
        /// </summary>
        public string GBIEMUNJIN { get; set; }

        /// <summary>
        /// 문자수신동의여부(Y:동의함 / N or Null:동의안함)
        /// </summary>
        public string GBSMS { get; set; }

        /// <summary>
        /// 핸드폰번호가 잘못되어 있는 경우 N 로 표시됨
        /// </summary>
        public string TEL_CONFIRM { get; set; }

        /// <summary>
        /// 주민번호 암호화
        /// </summary>
        public string JUMIN2 { get; set; }

        /// <summary>
        /// 개인정보동의서(동의일자)
        /// </summary>
        public string GBPRIVACY { get; set; }

        /// <summary>
        /// 우편번호용 건물번호
        /// </summary>
        public string BUILDNO { get; set; }

        /// <summary>
        /// <사용안함>
        /// </summary>
        public string LTDCODE2 { get; set; }

        /// <summary>
        /// 생일
        /// </summary>
        public string BIRTHDAY { get; set; }

        /// <summary>
        /// 생일구분(1.양력 2.음력)
        /// </summary>
        public string GBBIRTH { get; set; }

        /// <summary>
        /// 회사 전화번호
        /// </summary>
        public string LTDTEL { get; set; }

        /// <summary>
        /// 감액계정(최종 사용된것)
        /// </summary>
        public string GAMCODE { get; set; }

        /// <summary>
        /// 종교구분코드(아래참조)
        /// </summary>
        public string RELIGION { get; set; }

        /// <summary>
        /// 최초검진일자
        /// </summary>
        public string STARTDATE { get; set; }

        /// <summary>
        /// 최종검진일자
        /// </summary>
        public string LASTDATE { get; set; }

        /// <summary>
        /// 검진횟수
        /// </summary>
        public long JINCOUNT { get; set; }

        /// <summary>
        /// 가족성명
        /// </summary>
        public string FAMILLY { get; set; }

        /// <summary>
        /// 감액계정2 (최종 사용된것)
        /// </summary>
        public string GAMCODE2 { get; set; }

        /// <summary>
        /// 시청/교육청 검진확인서용 소속명칭(00초등학교,남구청)
        /// </summary>
        public string SOSOK { get; set; }

        /// <summary>
        /// VIP관련 참고사항
        /// </summary>
        public string VIPREMARK { get; set; }

        /// <summary>
        /// 직원본인여부(Y.직원 N.배우자 등 가족)
        /// </summary>
        public string GBJIKWON { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBFOREIGNER { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ENAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SNAME2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FOREIGNERNUM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBPRIVACY_NEW { get; set; }

        /// <summary>
        /// 보건관리전문 직책
        /// </summary>
        public string WORKER_ROLE { get; set; }

        /// <summary>
        /// 중점관리대상 여부(보건대행)
        /// </summary>
        public string ISMANAGEOSHA { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WEBSEND { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WEBSENDDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string INBODYSENDYN { get; set; }

        /// <summary>
        /// 검진년도
        /// </summary>
        public string GJYEAR { get; set; }

        /// <summary>
        /// 접수(건강진단)일자
        /// </summary>
        public string JEPDATE { get; set; }


        /// <summary>
        /// 나이(검진일자기준 만 나이)
        /// </summary>
        public long AGE { get; set; }

        /// <summary>
        /// 건진종류 코드
        /// </summary>
        public string GJJONG { get; set; }

        /// <summary>
        /// 검진차수(1.1차 2.2차 3.기타검진)
        /// </summary>
        public string GJCHASU { get; set; }

        /// <summary>
        /// 검진분기(1.전반기 2.하반기 3.기타)
        /// </summary>
        public string GJBANGI { get; set; }

        /// <summary>
        /// 출장검진여부(Y.출장검진 N.내원검진)
        /// </summary>
        public string GBCHUL { get; set; }

        /// <summary>
        /// 작업상태(아래참조)
        /// </summary>
        public string GBSTS { get; set; }

        /// <summary>
        /// 인원통계구분
        /// </summary>
        public string GBINWON { get; set; }

        /// <summary>
        /// 접수취소일자
        /// </summary>
        public string DELDATE { get; set; }

        

        

        /// <summary>
        /// 부담율 코드(아래참조)
        /// </summary>
        public string BURATE { get; set; }
        

        /// <summary>
        /// 유해인자(여러개가 입력 가능:컴마로 분리)
        /// </summary>
        public string UCODES { get; set; }

        /// <summary>
        /// 선택검사 코드
        /// </summary>
        public string SEXAMS { get; set; }

        /// <summary>
        /// 전직종코드
        /// </summary>
        public string OLDJIKJONG { get; set; }



        /// <summary>
        /// 전직종 근무 시작일
        /// </summary>
        public string OLDSDATE { get; set; }

        /// <summary>
        /// 전직종 근무 종료일
        /// </summary>
        public string OLDEDATE { get; set; }


        /// <summary>
        /// 발급년도
        /// </summary>
        public string BALYEAR { get; set; }

        /// <summary>
        /// 발급년도별 일련번호
        /// </summary>
        public long BALSEQ { get; set; }

        /// <summary>
        /// 바코드발행여부(Y/N or NULL)
        /// </summary>
        public string GBEXAM { get; set; }
        
        /// <summary>
        /// 접수구분(1.정상접수 2.가접수)
        /// </summary>
        public string JEPSUGBN { get; set; }


        /// <summary>
        /// 청구서번호
        /// </summary>
        public string JEPNO { get; set; }

        /// <summary>
        /// 특수검진 문진입력 여부(Y:입력 N.미입력)
        /// </summary>
        public string GBSPCMUNJIN { get; set; }

        /// <summary>
        /// 직원추천사번
        /// </summary>
        public long GBSABUN { get; set; }

        /// <summary>
        /// 일반진찰상담여부(Y:진찰 NULL:미진찰)
        /// </summary>
        public string GBJINCHAL { get; set; }

        /// <summary>
        /// 건강보험 문진 상태(NULL:비대상 N:미입력 Y:입력완료)
        /// </summary>
        public string GBMUNJIN1 { get; set; }

        /// <summary>
        /// 구강검사 문진 상태(NULL:비대상 N:미입력 Y:입력완료)
        /// </summary>
        public string GBMUNJIN2 { get; set; }

        /// <summary>
        /// 특수검진 문진 상태(NULL:비대상 N:미입력 Y:입력완료)
        /// </summary>
        public string GBMUNJIN3 { get; set; }

        /// <summary>
        /// 구강검사 여부(Y/N): 구강검사(ZD00)
        /// </summary>
        public string GBDENTAL { get; set; }

        /// <summary>
        /// 2차검진대상 구분 (NULL:미정 N:비대상 Y:대상)
        /// </summary>
        public string SECOND_FLAG { get; set; }

        /// <summary>
        /// 2차검진일(2차검진일이 없을 경우 NULL)
        /// </summary>
        public string SECOND_DATE { get; set; }

        /// <summary>
        /// 2차검진대상 연락일자
        /// </summary>
        public string SECOND_TONGBO { get; set; }

        /// <summary>
        /// 2차검진 검사항목(ex,1,2,L,...)
        /// </summary>
        public string SECOND_EXAMS { get; set; }

        /// <summary>
        /// 2차검진 사유(고혈압의심,직촬재촬,...)
        /// </summary>
        public string SECOND_SAYU { get; set; }

        /// <summary>
        /// 청구구분(Y.청구, " " 미청구)
        /// </summary>
        public string CHUNGGUYN { get; set; }

        /// <summary>
        /// 구강검사청구(Y.청구, " " 미청구)
        /// </summary>
        public string DENTCHUNGGUYN { get; set; }

        /// <summary>
        /// 암검사청구(Y.청구, " " 미청구)
        /// </summary>
        public string CANCERCHUNGGUYN { get; set; }

        /// <summary>
        /// 금액
        /// </summary>
        public long MAMT { get; set; }

        /// <summary>
        /// 금액
        /// </summary>
        public long FAMT { get; set; }

        /// <summary>
        /// 마일리지암 구분
        /// </summary>
        public string MILEAGEAM { get; set; }

        /// <summary>
        /// 무료암대상 여부
        /// </summary>
        public string MURYOAM { get; set; }

        /// <summary>
        /// 종검접수여부
        /// </summary>
        public string JONGGUMYN { get; set; }

        /// <summary>
        /// 자료전송 여부
        /// </summary>
        public string SEND { get; set; }

        /// <summary>
        /// 마일리지암구분
        /// </summary>
        public string MILEAGEAMGBN { get; set; }

        /// <summary>
        /// 무료암구분
        /// </summary>
        public string MURYOGBN { get; set; }

        /// <summary>
        /// 회사청구 유무
        /// </summary>
        public string NEGODATE { get; set; }

        /// <summary>
        /// 작업자 사번
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// 최종 작업 시각
        /// </summary>
        public string ENTTIME { get; set; }

        /// <summary>
        /// 2차검진 미실시 사유
        /// </summary>
        public string SECOND_MISAYU { get; set; }

        /// <summary>
        /// 건강보험1,2차 청구번호
        /// </summary>
        public long MIRNO1 { get; set; }

        /// <summary>
        /// 구강검진 청구번호
        /// </summary>
        public long MIRNO2 { get; set; }

        /// <summary>
        /// 암검진   청구번호
        /// </summary>
        public long MIRNO3 { get; set; }

        /// <summary>
        /// 안전공단 특수검진 전송 일련번호
        /// </summary>
        public long OHMSNO { get; set; }

        /// <summary>
        /// 회사청구 미수번호
        /// </summary>
        public long MISUNO1 { get; set; }

        /// <summary>
        /// 2차검사인 경우 1차판정 의사 면허번호
        /// </summary>
        public long FIRSTPANDRNO { get; set; }

        /// <summary>
        /// 급한 환자 표시
        /// </summary>
        public string ERFLAG { get; set; }

        /// <summary>
        /// 암검진 보건소청구번호
        /// </summary>
        public long MIRNO4 { get; set; }

        /// <summary>
        /// 청구 제외 사유
        /// </summary>
        public string MIRSAYU { get; set; }

        /// <summary>
        /// 급한 환자 통보일자
        /// </summary>
        public string ERTONGBO { get; set; }

        /// <summary>
        /// 공단청구 미수번호
        /// </summary>
        public long MISUNO2 { get; set; }

        /// <summary>
        /// 의료급여암 대상 : 2005년
        /// </summary>
        public string GUBDAESANG { get; set; }

        /// <summary>
        /// 암검진 의료급여암청구번호
        /// </summary>
        public long MIRNO5 { get; set; }

        /// <summary>
        /// 공단청구 구강미수번호
        /// </summary>
        public long MISUNO3 { get; set; }

        /// <summary>
        /// 방사선촬영번호()2005.07.13추가,2009년부터 직촬외래번호통합됨
        /// </summary>
        public string XRAYNO { get; set; }

        /// <summary>
        /// 신용카드 승인일련번호(1~9999999999)
        /// </summary>
        public long CARDSEQNO { get; set; }

        /// <summary>
        /// 69종 추가판정여부
        /// </summary>
        public string GBADDPAN { get; set; }

        /// <summary>
        /// 암검진구분 예)1,1,0,1,1,0 (위UGI,위GFS,대장암,간암,유방,자궁)
        /// </summary>
        public string GBAM { get; set; }

        /// <summary>
        /// 결과발송일자
        /// </summary>
        public string BALDATE { get; set; }

        /// <summary>
        /// 수진등록여부 (Y.자동등록)
        /// </summary>
        public string GBSUJIN_SET { get; set; }

        /// <summary>
        /// 가내원여부(Y:출장을 가내원처리 N or Null: 해당사항없음)
        /// </summary>
        public string GBCHUL2 { get; set; }

        /// <summary>
        /// 구강진찰상담여부(Y:진찰 NULL:미진찰)
        /// </summary>
        public string GBJINCHAL2 { get; set; }

        /// <summary>
        /// 상담의사 사번
        /// </summary>
        public long SANGDAMDRNO { get; set; }

        /// <summary>
        /// 상담일자
        /// </summary>
        public string SANGDAMDATE { get; set; }

        /// <summary>
        /// 원무과 전달 메세지
        /// </summary>
        public string SENDMSG { get; set; }

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
        /// HEMS Data 생성번호
        /// </summary>
        public long HEMSNO { get; set; }

        /// <summary>
        /// HEMS DATA 제외 사유
        /// </summary>
        public string HEMSMIRSAYU { get; set; }

        /// <summary>
        /// 자동접수 구분 (Y/N or Null)
        /// </summary>
        public string AUTOJEP { get; set; }

        /// <summary>
        /// 보건소청구 미수번호
        /// </summary>
        public long MISUNO4 { get; set; }

        /// <summary>
        /// 포스코 청구서 고유번호
        /// </summary>
        public long P_WRTNO { get; set; }

        /// <summary>
        /// 판정일자(2014-08-02)
        /// </summary>
        public string PANJENGDATE { get; set; }

        /// <summary>
        /// 2차 정밀청력 검사 대상자(Y/N)
        /// </summary>
        public string GBAUDIO2EXAM { get; set; }

        /// <summary>
        /// 문진표 인쇄구분(1.인터넷 2.작성제출 3.인쇄 NULL:미인쇄)
        /// </summary>
        public string GBMUNJINPRINT { get; set; }

        /// <summary>
        /// 대기순번관련 전달사항
        /// </summary>
        public string WAITREMARK { get; set; }

        /// <summary>
        /// 휴대폰 결과 조회용 PDF 파일명
        /// </summary>
        public string PDFPATH { get; set; }

        /// <summary>
        /// 낙상주의 여부(Y/N)
        /// </summary>
        public string GBNAKSANG { get; set; }

        /// <summary>
        /// 1차없이 구강검진만 실시여부(Y/N)
        /// </summary>
        public string GBDENTONLY { get; set; }

        /// <summary>
        /// 자동판정구분(Y:정상A자동판정,N:대상아님,NULL:미점검)
        /// </summary>
        public string GBAUTOPAN { get; set; }

        /// <summary>
        /// 인터넷문진번호
        /// </summary>
        public long IEMUNNO { get; set; }

        /// <summary>
        /// 웹결과지인쇄 신청일시(NULL:미신청)
        /// </summary>
        public string WEBPRINTREQ { get; set; }

        /// <summary>
        /// 공단 수진등록 확인 여부(Y/N)
        /// </summary>
        public string GBSUJIN_CHK { get; set; }

        /// <summary>
        /// 웹결과지인쇄 전송일시(NULL:미전송)
        /// </summary>
        public string WEBPRINTSEND { get; set; }

        /// <summary>
        /// 구강공단청구금액
        /// </summary>
        public long DENTAMT { get; set; }

        /// <summary>
        /// 휴일가산여부(Y/N)
        /// </summary>
        public string GBHUADD { get; set; }

        /// <summary>
        /// 액팅화면 참고사항
        /// </summary>
        public string EXAMREMARK { get; set; }

        /// <summary>
        /// 종검내시경실 여부(Y/N)
        /// </summary>
        public string GBHEAENDO { get; set; }

        /// <summary>
        /// 자격정보1(01:사업장 02:공무원 03:성인병, 04:의료급여)
        /// </summary>
        public string GBCHK1 { get; set; }

        /// <summary>
        /// 자격정보2(01:생애의료급여)
        /// </summary>
        public string GBCHK2 { get; set; }

        /// <summary>
        /// 임시
        /// </summary>
        public string GBCHK3 { get; set; }

        /// <summary>
        /// 생활습관접수
        /// </summary>
        public string GBLIFE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBJUSO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long GWRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string JEPBUN { get; set; }

        public string JUSO { get; set; }
        public string TONGBODATE2 { get; set; }

    }
}

