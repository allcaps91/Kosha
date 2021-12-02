using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{
    /// <summary>
    /// 작업환경점검 (  밀폐공간, 사무실오염, 근골격계부담작업, 관리대상유해물질 ) 
    /// </summary>
    public class ENVCHECKJSON2 : BaseDto
    {
        public string ENVCHECK2_1 { get; set; } //밀폐공간보건프로그램 수립시행
        public string ENVCHECK2_2 { get; set; } //환기상태
        public string ENVCHECK2_3 { get; set; } //보호구지급 및 관리
        public string ENVCHECK2_4 { get; set; } //대피용기구의비치
        public string ENVCHECK2_5 { get; set; } //사고시대피등
        public string ENVCHECK2_6 { get; set; } //소화설비등
        public string ENVCHECK2_7 { get; set; } //넹징ㅅ;ㄷ,ㅇㅇ,;직압
        public string ENVCHECK2_8 { get; set; } //출입구의 임의잠김방지
        public string ENVCHECK2_9 { get; set; } //안전한작업방법등의주지
        public string ENVCHECK2_10 { get; set; } //산소농도등의측정

        /// <summary>
        /// 사무실오렴
        /// </summary>
        public string ENVCHECK2_11 { get; set; } //공기정화설비등의 가동ㅇ
        public string ENVCHECK2_12 { get; set; } //공기정화설비등의 관리
        public string ENVCHECK2_13 { get; set; } //미생물오염관리
        public string ENVCHECK2_14 { get; set; } //건물개보수시공기오염관리
        public string ENVCHECK2_15 { get; set; } //사무실의청결
        public string ENVCHECK2_16 { get; set; } //보호구 지급 및 관리
        public string ENVCHECK2_17 { get; set; } //유해성등의 주지

        /// <summary>
        /// 소음
        /// </summary>
        public string ENVCHECK2_100 { get; set; } 
        public string ENVCHECK2_101 { get; set; } 
        
        public string ENVCHECK2_103 { get; set; } 
        public string ENVCHECK2_104 { get; set; } 
        public string ENVCHECK2_105 { get; set; }
        public string ENVCHECK2_106 { get; set; }
        public string ENVCHECK2_107 { get; set; }

        /// <summary>
        /// 근골격계부담작업
        /// </summary>
        public string ENVCHECK2_18 { get; set; } //위해요인조사
        public string ENVCHECK2_19 { get; set; } //작업환경
        public string ENVCHECK2_20 { get; set; } //통치 및 사후조치
        public string ENVCHECK2_21 { get; set; } //유해성주지
        public string ENVCHECK2_22 { get; set; } //예방프로그램 주지
        public string ENVCHECK2_23 { get; set; } //중량물 ㅍ시
        public string ENVCHECK2_24 { get; set; } //작업자세
        public string ENVCHECK2_25 { get; set; } //수지조사여부

        /// <summary>
        /// 관리대상유해물질
        /// </summary>
        public string ENVCHECK2_26 { get; set; } //관계되는설비
        public string ENVCHECK2_27 { get; set; } //국소배기 및 전체환기
        public string ENVCHECK2_28 { get; set; } //긴급차단장치의 설치
        public string ENVCHECK2_29 { get; set; } //작업방법, 사고시대피
        public string ENVCHECK2_30 { get; set; } //발암물질의 취급
        public string ENVCHECK2_31 { get; set; } //사용전 점검
        public string ENVCHECK2_32 { get; set; } //명칭등의 게시
        public string ENVCHECK2_33 { get; set; } //관리대상 유해물질의 저장
        public string ENVCHECK2_34 { get; set; } //빈용기등의 관리
        public string ENVCHECK2_35 { get; set; } //유해성등의 주지
        public string ENVCHECK2_36 { get; set; } //보호구지급및관리
        public string ENVCHECK2_37 { get; set; } //출입금지등
        public string ENVCHECK2_38 { get; set; } //흡연등의 금지
        public string ENVCHECK2_39 { get; set; } //세척시설등
        

    }
}
