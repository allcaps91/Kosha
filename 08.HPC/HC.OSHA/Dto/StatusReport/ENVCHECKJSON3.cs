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
    public class ENVCHECKJSON3 : BaseDto
    {
        /// <summary>
        /// 특별관리대상유해물질
        /// </summary>
        public string ENVCHECK3_1 { get; set; } //국소배기장치의 설치 성능
        public string ENVCHECK3_2 { get; set; } //사용전점검등
        public string ENVCHECK3_3 { get; set; } //출입금지 등의금지
        public string ENVCHECK3_4 { get; set; } //흡연등의금지
        public string ENVCHECK3_5 { get; set; } //명칭등의 게시
        public string ENVCHECK3_6 { get; set; } //유해성 주지
        public string ENVCHECK3_7 { get; set; } //작업수칙
        public string ENVCHECK3_8 { get; set; } //잠금장치 등
        public string ENVCHECK3_9 { get; set; } //목욕설비 등
        public string ENVCHECK3_10 { get; set; } //긴급세척시설 등
        public string ENVCHECK3_11 { get; set; } //기록의 보존
        public string ENVCHECK3_12 { get; set; } //보호구 지급 및 관리

        /// <summary>
        /// 산업재해
        /// </summary>
        public string ENVCHECK3_13 { get; set; } //산업재해 발생일시
        public string ENVCHECK3_14 { get; set; } //산업재해 발생경위
        public string ENVCHECK3_15 { get; set; } //산재구분
        public string ENVCHECK3_16 { get; set; } //이름
        public string ENVCHECK3_17 { get; set; } //진단명
        public string ChkAccNone { get; set; } //해당없음
        

        /// <summary>
        /// 금지유해물질
        /// </summary>
        public string ENVCHECK3_18 { get; set; } //국소배기장치 성능 등
        public string ENVCHECK3_19 { get; set; } //바닥등
        public string ENVCHECK3_20 { get; set; } //유해성등의 주지
        public string ENVCHECK3_21 { get; set; } //용기 및 보관
        public string ENVCHECK3_22 { get; set; } //출입금지 등
        public string ENVCHECK3_23 { get; set; } //흡연 및 누출조치 등
        public string ENVCHECK3_24 { get; set; } //세인설비 등
        public string ENVCHECK3_25 { get; set; } //기록보관
        public string ENVCHECK3_26 { get; set; } //보호구 지급 및 관리

        /// <summary>
        /// 그 밖의 유해인자
        /// </summary>
        public string ENVCHECK3_27 { get; set; } //컴퓨터 단말기 조작업무
        public string ENVCHECK3_28 { get; set; } //직무스트레스 관련
        public string ENVCHECK3_29 { get; set; } //작업장 청결 등
        public string ENVCHECK3_30 { get; set; } //오염된 바닥의 세척등
        public string ENVCHECK3_31 { get; set; } //휴게시설
        public string ENVCHECK3_32 { get; set; } //의자비치 
        public string ENVCHECK3_33 { get; set; } //수면장소등의 설치
        public string ENVCHECK3_34 { get; set; } //구급용구


    }
}
