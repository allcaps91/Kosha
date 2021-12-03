using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{
    /// <summary>
    /// 작업환경점검 (  조도, 분진, 온습도 ) 
    /// </summary>
    public class ENVCHECKJSON1 : BaseDto
    {
        public string ENVCHECK1 { get; set; } //작업관련조명상태
        /// <summary>
        /// //조명기구 등의 관리상태
        /// </summary>
        public string ENVCHECK2 { get; set; } 
        public string ENVCHECK3 { get; set; } //국소배기장치 설비
        public string ENVCHECK4 { get; set; } //청소실시여부
        public string ENVCHECK5 { get; set; } //유해성 주지여부
        public string ENVCHECK6 { get; set; } //호흡기보호프로그램 시행
        public string ENVCHECK7 { get; set; } //호흡용보호구 지급 및 관리
        public string ENVCHECK8 { get; set; } //온습도조절
        public string ENVCHECK9 { get; set; } //환기장치설치등
        public string ENVCHECK10 { get; set; } //고열장해예방조치
        public string ENVCHECK11 { get; set; } //한랭장해예방조치
        public string ENVCHECK12 { get; set; } //다습장해예방조치
        public string ENVCHECK13 { get; set; } //휴게시설등
        public string ENVCHECK14 { get; set; } //출입금지등
        public string ENVCHECK15 { get; set; } //소금과 음료수 등의 비치
        public string ENVCHECK16 { get; set; } //보호구 지급 및 관리


    }
}
