namespace ComHpcLibB.Model
{

    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HIC_RES_DENTAL_JEPSU_PATEINT : BaseDto
    {

        /// <summary>
        /// 접수번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 우식증:우식증유무
        /// </summary>
        public string USIK1 { get; set; }

        /// <summary>
        /// 우식증:우식증유(상)
        /// </summary>
        public string USIK2 { get; set; }

        /// <summary>
        /// 우식증:우식증유(하)
        /// </summary>
        public string USIK3 { get; set; }

        /// <summary>
        /// 우식증:우식증발생차아위험유무
        /// </summary>
        public string USIK4 { get; set; }

        /// <summary>
        /// 우식증:우식증발생차아위험유(상)
        /// </summary>
        public string USIK5 { get; set; }

        /// <summary>
        /// 우식증:우식증발생차아위험유(하)
        /// </summary>
        public string USIK6 { get; set; }

        /// <summary>
        /// 결손치:결손치 유무
        /// </summary>
        public string GYELSON1 { get; set; }

        /// <summary>
        /// 결손치:결손치(상)
        /// </summary>
        public string GYELSON2 { get; set; }

        /// <summary>
        /// 결손치:결손치(하)
        /// </summary>
        public string GYELSON3 { get; set; }

        /// <summary>
        /// 치주:치주질환유무
        /// </summary>
        public string CHIJU1 { get; set; }

        /// <summary>
        /// 치주:치은비대
        /// </summary>
        public string CHIJU2 { get; set; }

        /// <summary>
        /// 치주:치석
        /// </summary>
        public string CHIJU3 { get; set; }

        /// <summary>
        /// 치주:치주낭
        /// </summary>
        public string CHIJU4 { get; set; }

        /// <summary>
        /// 치주:기타
        /// </summary>
        public string CHIJU5 { get; set; }

        /// <summary>
        /// 치주:차아 마모증
        /// </summary>
        public string CHIJU6 { get; set; }

        /// <summary>
        /// 치주:제3대구치(사랑니)유무
        /// </summary>
        public string CHIJU7 { get; set; }

        /// <summary>
        /// 치주:제3대구치(사랑니)이상
        /// </summary>
        public string CHIJU8 { get; set; }

        /// <summary>
        /// 치주:구내염및 구강연조직질환
        /// </summary>
        public string CHIJU9 { get; set; }

        /// <summary>
        /// 치주:악관절이상
        /// </summary>
        public string CHIJU10 { get; set; }

        /// <summary>
        /// 의치:의치상태필요유무
        /// </summary>
        public string BOCHUL1 { get; set; }

        /// <summary>
        /// 의치:의치상태필요(상)
        /// </summary>
        public string BOCHUL2 { get; set; }

        /// <summary>
        /// 의치:의치상태필요(하)
        /// </summary>
        public string BOCHUL3 { get; set; }

        /// <summary>
        /// 의치:의치상태유(상)
        /// </summary>
        public string BOCHUL4 { get; set; }

        /// <summary>
        /// 의치:의치상태유(하)
        /// </summary>
        public string BOCHUL5 { get; set; }

        /// <summary>
        /// 기타:ANUG
        /// </summary>
        public string BOCHUL6 { get; set; }

        /// <summary>
        /// 기타:지도설
        /// </summary>
        public string BOCHUL7 { get; set; }

        /// <summary>
        /// 기타:과잉치
        /// </summary>
        public string BOCHUL8 { get; set; }

        /// <summary>
        /// 기타:유치잔존
        /// </summary>
        public string BOCHUL9 { get; set; }

        /// <summary>
        /// 기타:선천결손치
        /// </summary>
        public string BOCHUL10 { get; set; }

        /// <summary>
        /// 기타:부정교합
        /// </summary>
        public string BOCHUL11 { get; set; }

        /// <summary>
        /// 기타:기타
        /// </summary>
        public string BOCHUL12 { get; set; }

        /// <summary>
        /// 지난 1년간 치과병(의)원 방문여부
        /// </summary>
        public string OPDDNT { get; set; }

        /// <summary>
        /// 지난 1년간 치석제거
        /// </summary>
        public string SCALING { get; set; }

        /// <summary>
        /// 현재의 자신의 구강상태
        /// </summary>
        public string DNTSTATUS { get; set; }

        /// <summary>
        /// 식습관:단음식, 청량음료
        /// </summary>
        public string FOOD1 { get; set; }

        /// <summary>
        /// 식습관:질기고 딱딱한 음식
        /// </summary>
        public string FOOD2 { get; set; }

        /// <summary>
        /// 식습관:간식
        /// </summary>
        public string FOOD3 { get; set; }

        /// <summary>
        /// 하루중 이를 닦을 때(아침식사전)
        /// </summary>
        public string BRUSH11 { get; set; }

        /// <summary>
        /// 하루중 이를 닦을 때(아침식사후)
        /// </summary>
        public string BRUSH12 { get; set; }

        /// <summary>
        /// 하루중 이를 닦을 때(점심식사후)
        /// </summary>
        public string BRUSH13 { get; set; }

        /// <summary>
        /// 하루중 이를 닦을 때(저녁식사후)
        /// </summary>
        public string BRUSH14 { get; set; }

        /// <summary>
        /// 하루중 이를 닦을 때(잠자기직전)
        /// </summary>
        public string BRUSH15 { get; set; }

        /// <summary>
        /// 하루중 이를 닦을 때(간식섭취후)
        /// </summary>
        public string BRUSH16 { get; set; }

        /// <summary>
        /// 잇솔질습관:어떻게 닦으십니까?
        /// </summary>
        public string BRUSH21 { get; set; }

        /// <summary>
        /// 구강증상및증후: 찬음식
        /// </summary>
        public string JUNGSANG1 { get; set; }

        /// <summary>
        /// 구강증상및증후: 잇솔질
        /// </summary>
        public string JUNGSANG2 { get; set; }

        /// <summary>
        /// 구강증상및증후: 잇몸
        /// </summary>
        public string JUNGSANG3 { get; set; }

        /// <summary>
        /// 구강증상및증후: 턱관절소리
        /// </summary>
        public string JUNGSANG4 { get; set; }

        /// <summary>
        /// 구강증상및증후: 턱관절통증
        /// </summary>
        public string JUNGSANG5 { get; set; }

        /// <summary>
        /// 구강증상및증후: 입냄새
        /// </summary>
        public string JUNGSANG6 { get; set; }

        /// <summary>
        /// 구강증상및증후: 이가 부서진다
        /// </summary>
        public string JUNGSANG7 { get; set; }

        /// <summary>
        /// 의사에게 하고 싶은 말
        /// </summary>
        public string MUNJINETC { get; set; }

        /// <summary>
        /// 종합소견:1.정상
        /// </summary>
        public string PANJENG1 { get; set; }

        /// <summary>
        /// 종합소견:2.치아우식증(충치) 치료가 필요합니다.
        /// </summary>
        public string PANJENG2 { get; set; }

        /// <summary>
        /// 종합소견:3.이를 해 넣어야 합니다.
        /// </summary>
        public string PANJENG3 { get; set; }

        /// <summary>
        /// 종합소견:4.치석제거(스케일링)가 필요합니다.
        /// </summary>
        public string PANJENG4 { get; set; }

        /// <summary>
        /// 종합소견:5.잇몸수술이 필요합니다.
        /// </summary>
        public string PANJENG5 { get; set; }

        /// <summary>
        /// 종합소견:6.이르 빼야 합니다.(사랑나,뿌리만 남은 이)
        /// </summary>
        public string PANJENG6 { get; set; }

        /// <summary>
        /// 종합소견:7.구강연조직 질환을 치료해야 합니다.
        /// </summary>
        public string PANJENG7 { get; set; }

        /// <summary>
        /// 종합소견:8.악관절 이상을 치료해야 합니다.
        /// </summary>
        public string PANJENG8 { get; set; }

        /// <summary>
        /// 종합소견:9.틀니를 해 넣어야 합니다.
        /// </summary>
        public string PANJENG9 { get; set; }

        /// <summary>
        /// 종합소견:10.응급구강진료가 필요합니다.
        /// </summary>
        public string PANJENG10 { get; set; }

        /// <summary>
        /// 종합소견:11.치료했던 치아를 다시 검사,치료
        /// </summary>
        public string PANJENG11 { get; set; }

        /// <summary>
        /// 종합소견:12.기타
        /// </summary>
        public string PANJENG12 { get; set; }

        /// <summary>
        /// 판정일자
        /// </summary>
        public string PANJENGDATE { get; set; }

        /// <summary>
        /// 통보방법(1.사업장 2.주소지 3.내원)
        /// </summary>
        public string TONGBOGBN { get; set; }

        /// <summary>
        /// 결과통보일
        /// </summary>
        public string TONGBODATE { get; set; }

        /// <summary>
        /// 판정의사 면허번호
        /// </summary>
        public long PANJENGDRNO { get; set; }

        /// <summary>
        /// 청구번호
        /// </summary>
        public long MIRNO { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string PANJENG13 { get; set; }

        /// <summary>
        /// 청구여부
        /// </summary>
        public string MIRYN { get; set; }

        /// <summary>
        /// 담배를 피우십시니까? 2009년
        /// </summary>
        public string T_HABIT1 { get; set; }

        /// <summary>
        /// 양치횟수 2009년
        /// </summary>
        public long T_HABIT2 { get; set; }

        /// <summary>
        /// 잠자기전 칫솔질 2009년
        /// </summary>
        public string T_HABIT3 { get; set; }

        /// <summary>
        /// 치실,치간솔 사용 2009년
        /// </summary>
        public string T_HABIT4 { get; set; }

        /// <summary>
        /// 치아가 아픈적? 2009년
        /// </summary>
        public string T_STAT1 { get; set; }

        /// <summary>
        /// 잇몸아프거나 피 2009년
        /// </summary>
        public string T_STAT2 { get; set; }

        /// <summary>
        /// 혀,입안쪽 아픈적? 2009년
        /// </summary>
        public string T_STAT3 { get; set; }

        /// <summary>
        /// 입냄새? 2009년
        /// </summary>
        public string T_STAT4 { get; set; }

        /// <summary>
        /// 치아시린적? 2009년
        /// </summary>
        public string T_STAT5 { get; set; }

        /// <summary>
        /// 턱관절소리,아픔? 2009년
        /// </summary>
        public string T_STAT6 { get; set; }

        /// <summary>
        /// 음식을 씹는데 불편감? 2009년
        /// </summary>
        public string T_FUNCTION1 { get; set; }

        /// <summary>
        /// 발음하는데 불편감? 2009년
        /// </summary>
        public string T_FUNCTION2 { get; set; }

        /// <summary>
        /// 틀니상태-틀니가 덜거덕거림 2009년
        /// </summary>
        public string T_FUNCTION3 { get; set; }

        /// <summary>
        /// 틀니상태-입안아픔 2009년
        /// </summary>
        public string T_FUNCTION4 { get; set; }

        /// <summary>
        /// 틀니상태-빼놓고 자는지? 2009년
        /// </summary>
        public string T_FUNCTION5 { get; set; }

        /// <summary>
        /// 현재당뇨병? 2009년
        /// </summary>
        public string T_JILBYUNG1 { get; set; }

        /// <summary>
        /// 치아검사-우식증-치관 2009년
        /// </summary>
        public string T_PAN1 { get; set; }

        /// <summary>
        /// 치아검사-우식증-치근 2009년
        /// </summary>
        public string T_PAN2 { get; set; }

        /// <summary>
        /// 치아검사-결손치 2009년
        /// </summary>
        public string T_PAN3 { get; set; }

        /// <summary>
        /// 치아검사-마모증 2009년
        /// </summary>
        public string T_PAN4 { get; set; }

        /// <summary>
        /// 치아검사-사랑니 2009년
        /// </summary>
        public string T_PAN5 { get; set; }

        /// <summary>
        /// 치주조직-치은염증 2009년
        /// </summary>
        public string T_PAN6 { get; set; }

        /// <summary>
        /// 치주조직-치은연상(치석) 2010년
        /// </summary>
        public string T_PAN7 { get; set; }

        /// <summary>
        /// 의치보철-고정성보철물 2009년
        /// </summary>
        public string T_PAN8 { get; set; }

        /// <summary>
        /// 의치보철-틀니상태 2009년
        /// </summary>
        public string T_PAN9 { get; set; }

        /// <summary>
        /// 구강연조직-구강연조직이상 2009년
        /// </summary>
        public string T_PAN10 { get; set; }

        /// <summary>
        /// 기타소견
        /// </summary>
        public string T_PAN_ETC { get; set; }

        /// <summary>
        /// 치면세균막(만40세)-상악우측부(15번) 2009년
        /// </summary>
        public long T40_PAN1 { get; set; }

        /// <summary>
        /// 치면세균막(만40세)-상악중앙부(13번) 2009년
        /// </summary>
        public long T40_PAN2 { get; set; }

        /// <summary>
        /// 치면세균막(만40세)-상악좌측부(26번) 2009년
        /// </summary>
        public long T40_PAN3 { get; set; }

        /// <summary>
        /// 치면세균막(만40세)-하악좌측부(36번) 2009년
        /// </summary>
        public long T40_PAN4 { get; set; }

        /// <summary>
        /// 치면세균막(만40세)-하악중앙부(32번) 2009년
        /// </summary>
        public long T40_PAN5 { get; set; }

        /// <summary>
        /// 치면세균막(만40세)-하악우측부(44번) 2009년
        /// </summary>
        public long T40_PAN6 { get; set; }

        /// <summary>
        /// 구강종합소견1
        /// </summary>
        public string T_PANJENG1 { get; set; }

        /// <summary>
        /// 구강종합소견2
        /// </summary>
        public string T_PANJENG2 { get; set; }

        /// <summary>
        /// 구강종합소견3
        /// </summary>
        public string T_PANJENG3 { get; set; }

        /// <summary>
        /// 구강종합소견4
        /// </summary>
        public string T_PANJENG4 { get; set; }

        /// <summary>
        /// 구강종합소견5
        /// </summary>
        public string T_PANJENG5 { get; set; }

        /// <summary>
        /// 구강종합소견6
        /// </summary>
        public string T_PANJENG6 { get; set; }

        /// <summary>
        /// 구강종합소견7
        /// </summary>
        public string T_PANJENG7 { get; set; }

        /// <summary>
        /// 구강종합소견8
        /// </summary>
        public string T_PANJENG8 { get; set; }

        /// <summary>
        /// 구강종합소견9
        /// </summary>
        public string T_PANJENG9 { get; set; }

        /// <summary>
        /// 구강종합소견10
        /// </summary>
        public string T_PANJENG10 { get; set; }

        /// <summary>
        /// 구강종합판정-기타소견
        /// </summary>
        public string T_PANJENG_ETC { get; set; }

        /// <summary>
        /// 구강종합소견 추가조치
        /// </summary>
        public string T_PANJENG_SOGEN { get; set; }

        /// <summary>
        /// 결과지인쇄 Y
        /// </summary>
        public string GBPRINT { get; set; }

        /// <summary>
        /// 구강상담내역
        /// </summary>
        public string SANGDAM { get; set; }

        /// <summary>
        /// 부식증-법랑질표면부(E1)
        /// </summary>
        public string BUSIK1 { get; set; }

        /// <summary>
        /// 부식증-법랑질파괴부(E2)
        /// </summary>
        public string BUSIK2 { get; set; }

        /// <summary>
        /// 부식증-상아질표면부(E3)
        /// </summary>
        public string BUSIK3 { get; set; }

        /// <summary>
        /// 부식증-2차상아질파괴부(E4)
        /// </summary>
        public string BUSIK4 { get; set; }

        /// <summary>
        /// 부식증-치주노출부식(E5)
        /// </summary>
        public string BUSIK5 { get; set; }

        /// <summary>
        /// 교모증-법랑질파괴부(T1)
        /// </summary>
        public string GYOMO1 { get; set; }

        /// <summary>
        /// 교모증-상아질파괴부(T2)
        /// </summary>
        public string GYOMO2 { get; set; }

        /// <summary>
        /// 교모증-교두의완전파괴(T3)
        /// </summary>
        public string GYOMO3 { get; set; }

        /// <summary>
        /// 교모증-치관치근경계부까지파괴(T4)
        /// </summary>
        public string GYOMO4 { get; set; }

        /// <summary>
        /// 치주조직검사결과
        /// </summary>
        public string CHIJURESULT { get; set; }

        /// <summary>
        /// 치주상태-출혈
        /// </summary>
        public string CHIJUSTAT1 { get; set; }

        /// <summary>
        /// 치주상태-치석형성
        /// </summary>
        public string CHIJUSTAT2 { get; set; }

        /// <summary>
        /// 치주상태-전치주낭형성
        /// </summary>
        public string CHIJUSTAT3 { get; set; }

        /// <summary>
        /// 치주상태-심치주낭형성
        /// </summary>
        public string CHIJUSTAT4 { get; set; }

        /// <summary>
        /// 치주상태-기타
        /// </summary>
        public string CHIJUSTATETC { get; set; }

        /// <summary>
        /// 부식증-정상(E0)
        /// </summary>
        public string BUSIK0 { get; set; }

        /// <summary>
        /// 교모증-정상(T0)
        /// </summary>
        public string GYOMO0 { get; set; }

        /// <summary>
        /// 치주조직-치은연상(치석) 2010년
        /// </summary>
        public string T_PAN11 { get; set; }

        /// <summary>
        /// 치아 닦는 방법 치과에서 배운적 2014년
        /// </summary>
        public string T_HABIT5 { get; set; }

        /// <summary>
        /// 지난 일주일 동안 잠자기전 칫솔질 2014년
        /// </summary>
        public string T_HABIT6 { get; set; }

        /// <summary>
        /// 치약에 불소가 들어있습니까? 2014년도
        /// </summary>
        public string T_HABIT7 { get; set; }

        /// <summary>
        /// 하루에 과자 등 끈끈한 간식 2014년
        /// </summary>
        public string T_HABIT8 { get; set; }

        /// <summary>
        /// 하루에 탄산,청량음료는 얼마나 2014년도
        /// </summary>
        public string T_HABIT9 { get; set; }

        /// <summary>
        /// 심혈관질환 여부 2014년
        /// </summary>
        public string T_JILBYUNG2 { get; set; }

        /// <summary>
        /// 문진표평가 2014년
        /// </summary>
        public string RES_MUNJIN { get; set; }

        /// <summary>
        /// 조치사항 2014년
        /// </summary>
        public string RES_JOCHI { get; set; }

        /// <summary>7
        /// 구강검사결과 2014년
        /// </summary>
        public string RES_RESULT { get; set; }

        /// <summary>
        /// 2019년 치면세균막(만40세) - 상악우측 제 1대구치(16번) 세균막
        /// </summary>
        public long T40_PAN1_NEW { get; set; }

        /// <summary>
        /// 2019년 치면세균막(만40세) - 상아우측 중절치    (11번) 세균막
        /// </summary>
        public long T40_PAN2_NEW { get; set; }

        /// <summary>
        /// 2019년 치면세균막(만40세) - 상아좌측 제 1대구치(26번) 세균막
        /// </summary>
        public long T40_PAN3_NEW { get; set; }

        /// <summary>
        /// 2019년 치면세균막(만40세) - 하악좌측 제 1대구치(36번) 세균막
        /// </summary>
        public long T40_PAN4_NEW { get; set; }

        /// <summary>
        /// 2019년 치면세균막(만40세) - 하악좌측 중절치    (31번) 세균막
        /// </summary>
        public long T40_PAN5_NEW { get; set; }

        /// <summary>
        /// 2019년 치면세균막(만40세) - 하악우측 제 1대구치(46번) 세균막
        /// </summary>
        public long T40_PAN6_NEW { get; set; }


        public string LTDCODE { get; set; }
        public string JEPDATE { get; set; }
        public string GJYEAR { get; set; }
        public string SNAME { get; set; }
        public string GJJONG { get; set; }
        public string JUMIN { get; set; }
        public string BUSENAME { get; set; }
        public string UCODES { get; set; }
        public string TONGBODATE2 { get; set; }
        public string BALDATE { get; set; }
        public string HPHONE { get; set; }
        public string EMAIL { get; set; }
        public string JUSO1 { get; set; }
        public string JUSO2 { get; set; }
        public string MAILCODE { get; set; }
    }
}

