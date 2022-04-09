using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{
    /// <summary>
    /// 간호사 상태보고서 업무수행내용
    /// </summary>
    public class NursePerformContentJson : BaseDto
    {
        /// <summary>
        /// 보건관리 계획수립 실시지도
        /// </summary>
        public string IsOshaPlan { get; set; }
        /// <summary>
        /// 일반환경위생관리지도
        /// </summary>
        public string IsOshaGeneral { get; set; }
        /// <summary>
        /// MSDS 게시교육실시지도
        /// </summary>
        public string IsMsds { get; set; }
        /// <summary>
        /// 안전보건표지공고표지설치지도
        /// </summary>
        public string IsMsdsInstall { get; set; }
        /// <summary>
        /// 노출근로자특수건강진단의뢰
        /// </summary>
        public string IsMsdsSpeacial { get; set; }

        /// <summary>
        /// 건강진단계획수립실시지도
        /// </summary>
        public string IsHcPlan { get; set; }
        /// <summary>
        /// 배치전후 건강진단관리
        /// </summary>
        public string IsHcManage { get; set; }
        /// <summary>
        /// 건강진단결과 설명
        /// </summary>
        public string IsHcExplain { get; set; }
        /// <summary>
        /// 건강이상자 질병유소견자 관리
        /// </summary>
        public string IsDesease { get; set; }
        /// <summary>
        /// 만성질환자관리
        /// </summary>
        public string IsDesease2 { get; set; }
        /// <summary>
        /// 산업재해근로자 건강확인지도
        /// </summary>
        public string IsDeseaseCheck{ get; set; }
        /// <summary>
        /// 외래진료임상검사 안내지도
        /// </summary>
        public string IsDeseaseCheck2 { get; set; }

        /// <summary>
        /// 응급의료체계 구축활요
        /// </summary>
        public string IsEmergency { get; set; }
        /// <summary>
        /// 응급처치 물품유지 및 관리지도
        /// </summary>
        public string IsEmergencyManage { get; set; }

        /// <summary>
        /// 직업병 예방 지도
        /// </summary>
        public string IsJobManage { get; set; }
        /// <summary>
        /// 직업병 유소견자 조치,건의
        /// </summary>
        public string IsJobManage2 { get; set; }
        /// <summary>
        /// 산업안전보건위원회 운영지도
        /// </summary>
        public string IsCommite { get; set; }
        /// <summary>
        /// 보호구 적격품 선정지도
        /// </summary>
        public string IsProtection1 { get; set; }
        /// <summary>
        /// 보호구지급 착용 관리방법지도
        /// </summary>
        public string IsProtection2 { get; set; }
        /// <summary>
        /// 안전보건교육실시 확인 지도
        /// </summary>
        public string IsEdu { get; set; }
        /// <summary>
        /// 보건교육실시(정기,기타)
        /// </summary>
        public string IsEduType { get; set; }

        /// <summary>
        /// 집체
        /// </summary>
        public string IsEduKind1 { get; set; }
        /// <summary>
        /// 소그룹
        /// </summary>
        public string IsEduKind2 { get; set; }
        /// <summary>
        /// 개별
        /// </summary>
        public string IsEduKind3 { get; set; }
        /// <summary>
        /// 강의식
        /// </summary>
        public string IsEduKind4 { get; set; }
        /// <summary>
        /// 상담식
        /// </summary>
        public string IsEduKind5 { get; set; }
        /// <summary>
        /// 실습식
        /// </summary>
        public string IsEduKind6 { get; set; }
        /// <summary>
        /// 참석자
        /// </summary>
        public string IsEduCount { get; set; }
        /// <summary>
        /// 주제
        /// </summary>
        public string IsEduTitle { get; set; }

        /// <summary>
        /// 작업장 순회 및 점검
        /// </summary>
        public string IsVisit { get; set; }

        /// <summary>
        /// 흡연음주비만 등 생활습관개선지도
        /// </summary>
        public string IsHealth1 { get; set; }
        /// <summary>
        /// 건강증활동 실시지도
        /// </summary>
        public string IsHealth2 { get; set; }

        /// <summary>
        /// 뇌심혈관질환 위험도 관리지도
        /// </summary>
        public string IsDe1 { get; set; }
        /// <summary>
        /// 직무스트레스 관리평가 지도
        /// </summary>
        public string IsDe2 { get; set; }
        /// <summary>
        /// 근골격계질환 관리지도
        /// </summary>
        public string IsDe3 { get; set; }

        /// <summary>
        /// 청력보존프로그램 운영지도
        /// </summary>
        public string IsHe1 { get; set; }
        /// <summary>
        /// 호흡기보호프로그램 운영지도
        /// </summary>
        public string IsHe2 { get; set; }
        /// <summary>
        /// 밀폐공간보건작업프로그램 지도
        /// </summary>
        public string IsHe3 { get; set; }

        /// <summary>
        /// 총상담 인원
        /// </summary>
        public int SangdamCount { get; set; }
        public string IsSangdam1 { get; set; } //고혈압
        public string IsSangdam2 { get; set; } // 당뇨
        public string IsSangdam3 { get; set; } // 기타
        public string IsSangdam4 { get; set; } // 소음
        public string IsSangdam5 { get; set; } // 분진
        public string IsSangdam6 { get; set; } // 화학적인자

        public int CheckCount1 { get; set; } //혈압  
        public int CheckCount2 { get; set; } // 혈당
        public int CheckCount3 { get; set; } // 소변
        public int CheckCount4 { get; set; } //체지방

        /// <summary>
        /// 보건정보자료보급
        /// </summary>
        public string OshaData { get; set; } 
    }
}
