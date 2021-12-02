namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// HEMS 전송 문진표
    /// </summary>
    /// 
    public class HIC_HEMS_SLNS15 : BaseDto
    {
        /// <summary>
        /// 특검사업년도
        /// </summary>
        public string SLNS_YEAR { get; set; }

        /// <summary>
        /// 검진기관코드
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
        /// 최초_검진일
        /// </summary>
        public string FRST_SLNS_DT { get; set; }

        /// <summary>
        /// 인적키(주민번호)
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// 1번(기존질병) 뇌졸중 진단여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1011 { get; set; }

        /// <summary>
        /// 1번(기존질병) 뇌졸중 치료여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1012 { get; set; }

        /// <summary>
        /// 1번(기존질병) 심장병 진단여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1021 { get; set; }

        /// <summary>
        /// 1번(기존질병) 심장병 치료여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1022 { get; set; }

        /// <summary>
        /// 1번(기존질병) 고혈압 진단여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1031 { get; set; }

        /// <summary>
        /// 1번(기존질병) 고혈압 치료여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1032 { get; set; }

        /// <summary>
        /// 1번(기존질병) 당뇨병 진단여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1041 { get; set; }

        /// <summary>
        /// 1번(기존질병) 당뇨병 치료여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1042 { get; set; }

        /// <summary>
        /// 1번(기존질병) 고지혈증 진단여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1051 { get; set; }

        /// <summary>
        /// 1번(기존질병) 고지혈증 치료여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1052 { get; set; }

        /// <summary>
        /// 1번(기존질병) 폐결핵 진단여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1061 { get; set; }

        /// <summary>
        /// 1번(기존질병) 폐결핵 치료여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1062 { get; set; }

        /// <summary>
        /// 1번(기존질병) 기타 진단여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1071 { get; set; }

        /// <summary>
        /// 1번(기존질병) 기타 치료여부 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_1072 { get; set; }

        /// <summary>
        /// 2번(가족력) 뇌졸중 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_2010 { get; set; }

        /// <summary>
        /// 2번(가족력) 심장병 (0.미해당 1.해당)
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
        /// 2번(가족력) 고지혈증 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_2050 { get; set; }

        /// <summary>
        /// 2번(가족력) 폐결핵 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_2060 { get; set; }

        /// <summary>
        /// 2번(가족력) 기타 (0.미해당 1.해당)
        /// </summary>
        public string MDCXM_ITRVW_2070 { get; set; }

        /// <summary>
        /// 3번(B형간염) B형간염여부 (1.예 2.아니오 3.모름)
        /// </summary>
        public string MDCXM_ITRVW_3010 { get; set; }

        /// <summary>
        /// 4-1번(흡연) 흡연여부(1.아니오, 2.예, 지금은 끊었음, 3.예, 현재도 흡연
        /// </summary>
        public string MDCXM_ITRVW_4010 { get; set; }

        /// <summary>
        /// 4-2번(흡연) 과거흡연년수(총년)
        /// </summary>
        public string MDCXM_ITRVW_4021 { get; set; }

        /// <summary>
        /// 4-2번(흡연) 과거흡연량(하루개피)
        /// </summary>
        public string MDCXM_ITRVW_4022 { get; set; }

        /// <summary>
        /// 4-3번(흡연) 현재흡연년수(총년)
        /// </summary>
        public string MDCXM_ITRVW_4031 { get; set; }

        /// <summary>
        /// 4-3번(흡연) 현재흡연량(하루개피)
        /// </summary>
        public string MDCXM_ITRVW_4032 { get; set; }

        /// <summary>
        /// 5-1번(음주) 1주 평균 몇일 (1.0일, 2.1일, 3.2일 ,4.3일, 5.4일, 6.5일, 7
        /// </summary>
        public string MDCXM_ITRVW_5010 { get; set; }

        /// <summary>
        /// 5-2번(음주) 하루음주잔수(잔)
        /// </summary>
        public string MDCXM_ITRVW_5020 { get; set; }

        /// <summary>
        /// 6번(신체활동) 격렬한운동 하루20분이상 시행일수 (1.0일, 2.1일, 3.2일 ,4
        /// </summary>
        public string MDCXM_ITRVW_6010 { get; set; }

        /// <summary>
        /// 6번(신체활동) 중간운동 하루30분이상 시행일수 (1.0일, 2.1일, 3.2일 ,4.3
        /// </summary>
        public string MDCXM_ITRVW_6020 { get; set; }

        /// <summary>
        /// 6번(신체활동) 가벼운운동 하루총30분 이상 걸은날 (1.0일, 2.1일, 3.2일 ,
        /// </summary>
        public string MDCXM_ITRVW_6030 { get; set; }

        /// <summary>
        /// 7번(증상) 일반-식욕 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7011 { get; set; }

        /// <summary>
        /// 7번(증상) 일반-피로감 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7012 { get; set; }

        /// <summary>
        /// 7번(증상) 일반-덩어리 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7013 { get; set; }

        /// <summary>
        /// 7번(증상) 피부-염증 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7021 { get; set; }

        /// <summary>
        /// 7번(증상) 피부-반점 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7022 { get; set; }

        /// <summary>
        /// 7번(증상) 피부-체모,손톱,발톱 변화 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7023 { get; set; }

        /// <summary>
        /// 7번(증상) 눈-눈물 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7031 { get; set; }

        /// <summary>
        /// 7번(증상) 눈-시력 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7032 { get; set; }

        /// <summary>
        /// 7번(증상) 눈-충혈 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7033 { get; set; }

        /// <summary>
        /// 7번(증상) 귀-난청 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7041 { get; set; }

        /// <summary>
        /// 7번(증상) 귀-이명 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7042 { get; set; }

        /// <summary>
        /// 7번(증상) 코-코피 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7051 { get; set; }

        /// <summary>
        /// 7번(증상) 코-콧물 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7052 { get; set; }

        /// <summary>
        /// 7번(증상) 코-후각이상 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7053 { get; set; }

        /// <summary>
        /// 7번(증상) 입-잇몸 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7061 { get; set; }

        /// <summary>
        /// 7번(증상) 입-맛 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7062 { get; set; }

        /// <summary>
        /// 7번(증상) 소화기-통증 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7071 { get; set; }

        /// <summary>
        /// 7번(증상) 소화기-입맛 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7072 { get; set; }

        /// <summary>
        /// 7번(증상) 소화기-변비 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7073 { get; set; }

        /// <summary>
        /// 7번(증상) 심혈관/호흡기-가슴 두근거림 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7081 { get; set; }

        /// <summary>
        /// 7번(증상) 심혈관/호흡기-기침 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7082 { get; set; }

        /// <summary>
        /// 7번(증상) 심혈관/호흡기-답답 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7083 { get; set; }

        /// <summary>
        /// 7번(증상) 심혈관/호흡기-아침기침 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7084 { get; set; }

        /// <summary>
        /// 7번(증상) 심혈관/호흡기-휴일이후기침 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7085 { get; set; }

        /// <summary>
        /// 7번(증상) 척추/사지-쑤심 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7091 { get; set; }

        /// <summary>
        /// 7번(증상) 척추/사지-손발떨림 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7092 { get; set; }

        /// <summary>
        /// 7번(증상) 척추/사지-손발감각 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7093 { get; set; }

        /// <summary>
        /// 7번(증상) 척추/사지-추울때 손가락 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7094 { get; set; }

        /// <summary>
        /// 7번(증상) 척추/사지-허리통증 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7095 { get; set; }

        /// <summary>
        /// 7번(증상) 정신/신경-두통 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7101 { get; set; }

        /// <summary>
        /// 7번(증상) 정신/신경-어지럼증 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7102 { get; set; }

        /// <summary>
        /// 7번(증상) 정신/신경-기억력 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7103 { get; set; }

        /// <summary>
        /// 7번(증상) 정신/신경-불안초조 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7104 { get; set; }

        /// <summary>
        /// 7번(증상) 정신/신경-정신이멍 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7105 { get; set; }

        /// <summary>
        /// 7번(증상) 정신/신경-정신집중 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7106 { get; set; }

        /// <summary>
        /// 7번(증상) 비뇨/생식-소변 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7111 { get; set; }

        /// <summary>
        /// 7번(증상) 비뇨/생식-붓기 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7112 { get; set; }

        /// <summary>
        /// 7번(증상) 비뇨/생식-생리 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7113 { get; set; }

        /// <summary>
        /// 7번(증상) 비뇨/생식-자연유산 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7114 { get; set; }

        /// <summary>
        /// 7번(증상) 그외 다른 증상
        /// </summary>
        public string MDCXM_ITRVW_7120 { get; set; }

        /// <summary>
        /// 7번(증상) 작업중 이상 (1.Yes, 2.No)
        /// </summary>
        public string MDCXM_ITRVW_7130 { get; set; }

        /// <summary>
        /// 7번(증상) 작업중 취급물질로 이상 (1.Yes, 2.No)
        /// </summary>
        public string MDCXM_ITRVW_7140 { get; set; }

        /// <summary>
        /// 7번(증상) 의사소견
        /// </summary>
        public string MDCXM_ITRVW_7150 { get; set; }

        /// <summary>
        /// HEMS 번호
        /// </summary>
        public long HEMSNO { get; set; }

        /// <summary>
        /// 7번 피부 거칠어짐 (1.심함, 2.약간, 3.없음)
        /// </summary>
        public string MDCXM_ITRVW_7024 { get; set; }

        /// <summary>
        /// HEMS 전송 문진표
        /// </summary>
        public HIC_HEMS_SLNS15()
        {
        }
    }
}
