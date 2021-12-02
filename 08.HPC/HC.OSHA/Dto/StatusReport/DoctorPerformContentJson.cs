using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto.StatusReport
{
    public class DoctorPerformContentJson 
    {
        public string mainContent { get; set; } //보건담당자 업무지도 
        public string sDate { get; set; } // 산업안전보건위원회 개최예쩡일
        public string sSupport { get; set; }// 산업안전보건위원회 운영지도
        
        public string hCcount { get; set; }//건강상담 총 건수NumSangdamCount
        public string noiseD1 { get; set; } // D1 직업병 소음
        public string noiseC1 { get; set; } // C1 직업병 소음
        public string bunD1 { get; set; } // D1 직업병 분진
        public string bunC1 { get; set; } // C1 직업병 분진
        public string mD1 { get; set; } // D1 직업병 화확물질
        public string mC1 { get; set; } // C1 직업병 화학물질

        public string goldD1 { get; set; } // D1 직업병 금속류
        public string goldC1 { get; set; } // C1 직업병 금속류

        public string etcD1 { get; set; } // D1 직업병 기타
        public string etcC1 { get; set; } // C1 직업병 기타

        public string etcDN { get; set; } // DN 직업병 기타
        public string etcCN { get; set; } // CN 직업병 기타

        public string aD1 { get; set; } // D1 일반질병 고혈압
        public string aC1 { get; set; } // C1 일반질병 고혈압
        public string bD1 { get; set; } // D1 일반질병 당뇨
        public string bC1 { get; set; } // C1 일반질병 당뇨
        public string cD1 { get; set; } // D1 일반질병 이상지질
        public string cC1 { get; set; } // C1 일반질병 이상지질

        public string dD1 { get; set; } // D1 일반질병 간장질환
        public string dC1 { get; set; } // C1 일반질병 간장질환

        public string eD1 { get; set; } // D1 일반질병 기타
        public string eC1 { get; set; } // C1 일반질병 기타

        /// <summary>
        /// 혈압측정 간이검사
        /// </summary>
        public string ppCount { get; set; } // 혈압측정 총건수
        public string ppa { get; set; } // 혈압측정 혈압건수
        public string ppb { get; set; } // 혈압측정 혈당
        public string ppc { get; set; } // 혈압측정 단백뇨
        public string ppd { get; set; } // 혈압측정 체지방

        public string qqCount { get; set; } // 외래진ㄹ,검사의로 총건수
        public string qqContent { get; set; } // 어ㅣ래진료 내용

        public string chk1 { get; set; } // 조치지도 근무중치료
        public string chk2 { get; set; } // 조치지도 추적검사
        public string chk3 { get; set; } // 조치지도 보호구착용
        public string chk4 { get; set; } // 조치지도 조건부근무
        public string chk4Content { get; set; } // 조치지도 조건부근무 내용

        public string chk5 { get; set; } // 조치지도 근로시간단축
        public string chk6 { get; set; } // 조치지도 작업전환
        public string chk7 { get; set; } // 조치지도 근로제한
        public string chk8 { get; set; } // 조치지도 기타
        public string chk8Content { get; set; } // 조치지도 기타 내용
        public string chkContent { get; set; } // 이행지도 이행확인결과


        public string chk10 { get; set; } // 건강증진지도 흡연
        public string chk11 { get; set; } // 건강증진지도 음주
        public string chk12 { get; set; } // 건강증진지도 운동
        public string chk13 { get; set; } // 건강증진지도 영양
        public string chk14 { get; set; } // 건강증진지도 비만
        public string chk15 { get; set; } // 건강증진지도 직업관련성질환예방
        public string chk15Content { get; set; } // 건강증진지도 직업관련성질환예방 내용

        public string chk20 { get; set; } // 응급처치관리 응급의료체계구축활용
        public string chk21 { get; set; } // 응급처치관리 응급처치지도

        public string chk31 { get; set; } // 직업병관리 직업병예방지도
        public string chk32 { get; set; } // 응급처치관리 원인조사
        public string chk33 { get; set; } // 응급처치관리 재발방지지도
        public string chk34 { get; set; } // 응급처치관리  사후조치결과확인

        public string chk41 { get; set; } // 작업환경관리 미만
        public string chk42 { get; set; } // 작업환경관리 초과
        public string chk42Content { get; set; } // 작업환경관리 초과 내용

        public string chk43 { get; set; } // 작업환경관리 일반환경위생관리
        public string chk44 { get; set; } // 작업환경관리 유해환경관리
        public string chk45 { get; set; } // 작업환경관리 ㅂ호구관리
        public string chk46 { get; set; } // 작업장순회점검지도
        public string chk46Content { get; set; } // 작업장순회점검지도내용

        public string bogunRadio { get; set; } // 보건교육 정기, 기타
        
        public string bogun3 { get; set; } // 보건교육 참석자
        public string bogun4 { get; set; } // 보건교육 장소
        public string bogun5 { get; set; } // 보건교육 교육자료
        public string bogun6 { get; set; } // 보건교육 교육주제
        public string bogunchk1{ get; set; } // 보건교육 개별
        public string bogunchk2 { get; set; } // 보건교육 소그룹
        public string bogunchk3 { get; set; } // 보건교육 집체교육
        public string bogunchk4 { get; set; } // 보건교육 강의식
        public string bogunchk5 { get; set; } // 보건교육 상담식
        public string bogunchk6 { get; set; } // 보건교육 기타
        public string bogunchk6Content { get; set; } // 보건교육 기타내용
        public string bogun7 { get; set; } // 보건정보자료보급 내용




    }
}

