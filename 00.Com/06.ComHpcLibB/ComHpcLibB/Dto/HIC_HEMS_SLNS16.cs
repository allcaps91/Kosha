namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// 
    public class HIC_HEMS_SLNS16 : BaseDto
    {

        /// <summary>
        /// 특검사업년도
        /// </summary>
        public string SLNS_YEAR { get; set; }

        /// <summary>
        /// 검진측정기관코드
        /// </summary>
        public string HOS_CODE { get; set; }

        /// <summary>
        /// 산재번호
        /// </summary>
        public string INDDIS_NO { get; set; }

        /// <summary>
        /// 개시번호
        /// </summary>
        public string INDOPEN_NO { get; set; }

        /// <summary>
        /// 순번
        /// </summary>
        public long INDDIS_NO_SEQ { get; set; }

        /// <summary>
        /// 1차 검진일
        /// </summary>
        public string FRST_SLNS_DT { get; set; }

        /// <summary>
        /// 주민번호
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// 1번(과거력) 뇌졸중(중풍) 진단여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1011 { get; set; }

        /// <summary>
        /// 1번(과거력) 뇌졸중(중풍) 약물치료여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1012 { get; set; }

        /// <summary>
        /// 1번(과거력) 심근경색/협심증 진단여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1021 { get; set; }

        /// <summary>
        /// 1번(과거력) 심근경색/협심증 약물치료여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1022 { get; set; }

        /// <summary>
        /// 1번(과거력) 고혈압 진단여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1031 { get; set; }

        /// <summary>
        /// 1번(과거력) 고혈압 약물치료여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1032 { get; set; }

        /// <summary>
        /// 1번(과거력) 당뇨병 진단여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1041 { get; set; }

        /// <summary>
        /// 1번(과거력) 당뇨병 약물치료여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1042 { get; set; }

        /// <summary>
        /// 1번(과거력) 이상지질혈증 진단여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1051 { get; set; }

        /// <summary>
        /// 1번(과거력) 이상지질혈증 약물치료여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1052 { get; set; }

        /// <summary>
        /// 1번(과거력) 폐결핵 진단여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1061 { get; set; }

        /// <summary>
        /// 1번(과거력) 폐결핵 약물치료여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1062 { get; set; }

        /// <summary>
        /// 1번(과거력) 기타(암포함) 진단여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1071 { get; set; }

        /// <summary>
        /// 1번(과거력) 기타(암포함) 약물치료여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1072 { get; set; }

        /// <summary>
        /// 2번(가족력) 뇌졸중(중풍) (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_2010 { get; set; }

        /// <summary>
        /// 2번(가족력) 심근경색/협심증 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_2020 { get; set; }

        /// <summary>
        /// 2번(가족력) 고혈압 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_2030 { get; set; }

        /// <summary>
        /// 2번(가족력) 당뇨병 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_2040 { get; set; }

        /// <summary>
        /// 2번(가족력) 기타(암포함) (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_2070 { get; set; }

        /// <summary>
        /// 3번(B형간염) 바이러스 보유 여부 (1.예 2.아니오 3.모름)
        /// </summary>
        public string MDCXM_ITRVW_3010 { get; set; }

        /// <summary>
        /// 4번(흡연) 평생 총5갑(100개비) 흡연여부 (1.아니오, 2.예, 지금은 끊었음,
        /// </summary>
        public string MDCXM_ITRVW_4010 { get; set; }

        /// <summary>
        /// 4-1번(흡연) 과거흡연년수(총년)
        /// </summary>
        public string MDCXM_ITRVW_4021 { get; set; }

        /// <summary>
        /// 4-1번(흡연) 과거흡연량(하루개피)
        /// </summary>
        public string MDCXM_ITRVW_4022 { get; set; }

        /// <summary>
        /// 4-2번(흡연) 현재흡연년수(총년)
        /// </summary>
        public string MDCXM_ITRVW_4031 { get; set; }

        /// <summary>
        /// 4-2번(흡연) 현재흡연량(하루개피)
        /// </summary>
        public string MDCXM_ITRVW_4032 { get; set; }

        /// <summary>
        /// 5번(전자담배) 사용 경험 여부 (1.예, 2.아니오)
        /// </summary>
        public string MDCXM_ITRVW_5010 { get; set; }

        /// <summary>
        /// 5-1번(전자담배) 최근 한달 사용 경험 여부 (1.아니오, 2.월 1-2일, 3.월 3
        /// </summary>
        public string MDCXM_ITRVW_5020 { get; set; }

        /// <summary>
        /// 6번(음주) 지난 1년간  술을 마시는 횟수(주기) (1.일주일에, 2.한달에, 3.
        /// </summary>
        public string MDCXM_ITRVW_6010 { get; set; }

        /// <summary>
        /// 6번(음주) 횟수
        /// </summary>
        public string MDCXM_ITRVW_6011 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (소주-잔)
        /// </summary>
        public string MDCXM_ITRVW_6111 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (소주-병)
        /// </summary>
        public string MDCXM_ITRVW_6112 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (소주-캔)
        /// </summary>
        public string MDCXM_ITRVW_6113 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (소주-cc)
        /// </summary>
        public string MDCXM_ITRVW_6114 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (맥주-잔)
        /// </summary>
        public string MDCXM_ITRVW_6121 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (맥주-병)
        /// </summary>
        public string MDCXM_ITRVW_6122 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (맥주-캔)
        /// </summary>
        public string MDCXM_ITRVW_6123 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (맥주-cc)
        /// </summary>
        public string MDCXM_ITRVW_6124 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (양주-잔)
        /// </summary>
        public string MDCXM_ITRVW_6131 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (양주-병)
        /// </summary>
        public string MDCXM_ITRVW_6132 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (양주-캔)
        /// </summary>
        public string MDCXM_ITRVW_6133 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (양주-cc)
        /// </summary>
        public string MDCXM_ITRVW_6134 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (막걸리-잔)
        /// </summary>
        public string MDCXM_ITRVW_6141 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (막걸리-병)
        /// </summary>
        public string MDCXM_ITRVW_6142 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (막걸리-캔)
        /// </summary>
        public string MDCXM_ITRVW_6143 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (막걸리-cc)
        /// </summary>
        public string MDCXM_ITRVW_6144 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (와인-잔)
        /// </summary>
        public string MDCXM_ITRVW_6151 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (와인-병)
        /// </summary>
        public string MDCXM_ITRVW_6152 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (와인-캔)
        /// </summary>
        public string MDCXM_ITRVW_6153 { get; set; }

        /// <summary>
        /// 6-1번(음주) 술을 마시는 날은 보통 어느정도 마십니까? (와인-cc)
        /// </summary>
        public string MDCXM_ITRVW_6154 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (소
        /// </summary>
        public string MDCXM_ITRVW_6211 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (소
        /// </summary>
        public string MDCXM_ITRVW_6212 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (소
        /// </summary>
        public string MDCXM_ITRVW_6213 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (소
        /// </summary>
        public string MDCXM_ITRVW_6214 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (맥
        /// </summary>
        public string MDCXM_ITRVW_6221 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (맥
        /// </summary>
        public string MDCXM_ITRVW_6222 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (맥
        /// </summary>
        public string MDCXM_ITRVW_6223 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (맥
        /// </summary>
        public string MDCXM_ITRVW_6224 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (양
        /// </summary>
        public string MDCXM_ITRVW_6231 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (양
        /// </summary>
        public string MDCXM_ITRVW_6232 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (양
        /// </summary>
        public string MDCXM_ITRVW_6233 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (양
        /// </summary>
        public string MDCXM_ITRVW_6234 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (막
        /// </summary>
        public string MDCXM_ITRVW_6241 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (막
        /// </summary>
        public string MDCXM_ITRVW_6242 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (막
        /// </summary>
        public string MDCXM_ITRVW_6243 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (막
        /// </summary>
        public string MDCXM_ITRVW_6244 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (와
        /// </summary>
        public string MDCXM_ITRVW_6251 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (와
        /// </summary>
        public string MDCXM_ITRVW_6252 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (와
        /// </summary>
        public string MDCXM_ITRVW_6253 { get; set; }

        /// <summary>
        /// 6-2번(음주) 가장 많이 마셨던 하루 음주량은 보통 어느정도 마십니까? (와
        /// </summary>
        public string MDCXM_ITRVW_6254 { get; set; }

        /// <summary>
        /// 7-1번(신체활동/고강도) 평소1주일간, 고강도 신체활동을 며칠 하십니까?
        /// </summary>
        public string MDCXM_ITRVW_7010 { get; set; }

        /// <summary>
        /// 7-2.1번(신체활동/고강도) 평소 하루에, 고강도 신체활동 몇시간 하십니까?
        /// </summary>
        public string MDCXM_ITRVW_7021 { get; set; }

        /// <summary>
        /// 7-2.2번(신체활동/고강도) 평소 하루에, 고강도 신체활동 몇분 하십니까?
        /// </summary>
        public string MDCXM_ITRVW_7022 { get; set; }

        /// <summary>
        /// 8-1번(신체활동/중강도) 평소1주일간, 중강도 신체활동을 며칠 하십니까?
        /// </summary>
        public string MDCXM_ITRVW_8010 { get; set; }

        /// <summary>
        /// 8-2.1번(신체활동/중강도) 평소 하루에, 중강도 신체활동 몇시간 하십니까?
        /// </summary>
        public string MDCXM_ITRVW_8021 { get; set; }

        /// <summary>
        /// 8-2.2번(신체활동/중강도) 평소 하루에, 중강도 신체활동 몇분 하십니까?
        /// </summary>
        public string MDCXM_ITRVW_8022 { get; set; }

        /// <summary>
        /// 9번(신체활동) 최근 1주일간, 근력운동을 한날이 며칠입니까? 주당 ( )일
        /// </summary>
        public string MDCXM_ITRVW_9010 { get; set; }

        /// <summary>
        /// 10번(증상) 일반-식욕 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A011 { get; set; }

        /// <summary>
        /// 10번(증상) 일반-피로감 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A012 { get; set; }

        /// <summary>
        /// 10번(증상) 일반-덩어리 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A013 { get; set; }

        /// <summary>
        /// 10번(증상) 피부-염증 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A021 { get; set; }

        /// <summary>
        /// 10번(증상) 피부-반점 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A022 { get; set; }

        /// <summary>
        /// 10번(증상) 피부-체모,손톱,발톱 변화 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A023 { get; set; }

        /// <summary>
        /// 10번 피부 거칠어짐 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A024 { get; set; }

        /// <summary>
        /// 10번(증상) 눈-눈물 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A031 { get; set; }

        /// <summary>
        /// 10번(증상) 눈-시력 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A032 { get; set; }

        /// <summary>
        /// 10번(증상) 눈-충혈 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A033 { get; set; }

        /// <summary>
        /// 10번(증상) 귀-난청 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A041 { get; set; }

        /// <summary>
        /// 10번(증상) 귀-이명 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A042 { get; set; }

        /// <summary>
        /// 10번(증상) 코-코피 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A051 { get; set; }

        /// <summary>
        /// 10번(증상) 코-콧물 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A052 { get; set; }

        /// <summary>
        /// 10번(증상) 코-후각이상 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A053 { get; set; }

        /// <summary>
        /// 10번(증상) 입-잇몸 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A061 { get; set; }

        /// <summary>
        /// 10번(증상) 입-맛 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A062 { get; set; }

        /// <summary>
        /// 10번(증상) 소화기-통증 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A071 { get; set; }

        /// <summary>
        /// 10번(증상) 소화기-입맛 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A072 { get; set; }

        /// <summary>
        /// 10번(증상) 소화기-변비 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A073 { get; set; }

        /// <summary>
        /// 10번(증상) 심혈관/호흡기-가슴 두근거림 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A081 { get; set; }

        /// <summary>
        /// 10번(증상) 심혈관/호흡기-기침 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A082 { get; set; }

        /// <summary>
        /// 10번(증상) 심혈관/호흡기-답답 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A083 { get; set; }

        /// <summary>
        /// 10번(증상) 심혈관/호흡기-아침기침 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A084 { get; set; }

        /// <summary>
        /// 10번(증상) 심혈관/호흡기-휴일이후기침 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A085 { get; set; }

        /// <summary>
        /// 10번(증상) 척추/사지-쑤심 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A091 { get; set; }

        /// <summary>
        /// 10번(증상) 척추/사지-손발떨림 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A092 { get; set; }

        /// <summary>
        /// 10번(증상) 척추/사지-손발감각 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A093 { get; set; }

        /// <summary>
        /// 10번(증상) 척추/사지-추울때 손가락 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A094 { get; set; }

        /// <summary>
        /// 10번(증상) 척추/사지-허리통증 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A095 { get; set; }

        /// <summary>
        /// 10번(증상) 정신/신경-두통 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A101 { get; set; }

        /// <summary>
        /// 10번(증상) 정신/신경-어지럼증 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A102 { get; set; }

        /// <summary>
        /// 10번(증상) 정신/신경-기억력 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A103 { get; set; }

        /// <summary>
        /// 10번(증상) 정신/신경-불안초조 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A104 { get; set; }

        /// <summary>
        /// 10번(증상) 정신/신경-정신이멍 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A105 { get; set; }

        /// <summary>
        /// 10번(증상) 정신/신경-정신집중 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A106 { get; set; }

        /// <summary>
        /// 10번(증상) 비뇨/생식-소변 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A111 { get; set; }

        /// <summary>
        /// 10번(증상) 비뇨/생식-붓기 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A112 { get; set; }

        /// <summary>
        /// 10번(증상) 비뇨/생식-생리 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A113 { get; set; }

        /// <summary>
        /// 10번(증상) 비뇨/생식-자연유산 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_A114 { get; set; }

        /// <summary>
        /// 10번(증상) 그외 다른 증상
        /// </summary>
        public string MDCXM_ITRVW_A120 { get; set; }

        /// <summary>
        /// 10번(증상) 작업중 이상 (1.Yes, 2.No)
        /// </summary>
        public string MDCXM_ITRVW_A130 { get; set; }

        /// <summary>
        /// 10번(증상) 작업중 취급물질로 이상 (1.Yes, 2.No)
        /// </summary>
        public string MDCXM_ITRVW_A140 { get; set; }

        /// <summary>
        /// 10번(증상) 의사소견
        /// </summary>
        public string MDCXM_ITRVW_A150 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long HEMSNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HIC_HEMS_SLNS16()
        {
        }
    }
}
