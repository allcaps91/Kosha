using ComBase; //기본 클래스
using ComDbB; //DB연결
using ComSupLibB.Com;
using System;
using System.Collections.Generic;
using System.Data;

namespace ComSupLibB.SupInfc
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : clsInFcSQL.cs
    /// Description     : 감염관리 
    /// Author          : 김홍록
    /// Create Date     : 2018-05-17
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "신규" />
    public class clsInFc : clsMethod
    {
        public string READ_ERROR(string arg, string argPTNO, string ArgSname, string argRDATE, string argDDATE, string argSPECNO)
        {
            string strMsg = "";
            string strCode = "";

            int nCode = arg.IndexOf("<code_dt>");

            if (nCode > -1 )
            {
                strCode = arg.Substring(nCode, 4);

                switch (strCode)
                {
                    case "2001":
                        break;
                    case "3001" : strMsg = "인증정보 '사용자(기관)  인증정보는 필수 입력사항입니다'";
                        break;
                    case "3002" : strMsg = "요양기관 아이디 '의뢰기관코드(8자리)는  필수 입력사항입니다'";
                        break;
                    case "3003" : strMsg = "의뢰기관 담당자(주치의)  성명   '의뢰기관담당자명은  필수 입력사항입니다'";
                        break;
                    case "3004" : strMsg = "환자성명 '환자성명은 필수  입력사항입니다'";
                        break;
                    case "3005" : strMsg = "환자성별 '환자성별코드는  필수 입력사항입니다'";
                        break;
                    case "3006" : strMsg = "환자생년월일 '환자생년월인(YYYYMMDD)은  필수 입력사항입니다'";
                        break;
                    case "3007" : strMsg = "환자등록번호 '환자등록번호는  필수 입력사항입니다'";
                        break;
                    case "3008" : strMsg = "검체종류(유형)  '검체종류코드는  필수 입력사항입니다'";
                        break;
                    case "3009" : strMsg = "검사방법(유형)  '검사방법코드는  필수 입력사항입니다'";
                        break;
                    case "3010" : strMsg = "병원체코드 '병원체코드는  필수 입력사항입니다'";
                        break;
                    case "3011" : strMsg = "의뢰일자 '의뢰일(YYYYMMDD)은  필수 입력사항입니다'";
                        break;
                    case "3012" : strMsg = "진단일자 '진단일(YYYYMMDD)은  필수 입력사항입니다'";
                        break;
                    case "3013" : strMsg = "검사기관 담당자(진단의,  검사자) 성명   '검사기관담당자성명은  필수 입력사항입니다'";
                        break;
                    case "4001" : strMsg = "중복 전송 여부  '중복 전송된  병원체 검사결과 신고입니다'";
                        break;
                    case "4002" : strMsg = "요양기관 아이디 '의뢰기관코드에  해당하는 정보를 찾을 수 없습니다, 질병보건통합관리시스템을 통해 확인해 주세요'";
                        break;
                    case "4003" : strMsg = "의뢰기관 담당자(주치의)  성명   '의뢰기관담당자명은  최대 30자리로 입력하세요'";
                        break;
                    case "4004" : strMsg = "환자성명 '환자성명은 최대  30자리로 입력하세요'";
                        break;
                    case "4005" : strMsg = "환자성별 '환자성별코드는  1 또는 2 여야합니다'";
                        break;
                    case "4006" : strMsg = "환자생년월일 '환자생년월일은  8자리(YYYYMMDD)로 입력하세요'";
                        break;
                    case "4007" : strMsg = "환자생년월일 '환자생년월일은  숫자로 입력하세요'";
                        break;
                    case "4008" : strMsg = "검체종류(유형)  '검체종류는  01, 02, 03, 04, 05, 99 숫자와 ','만 입력해야 합니다'";
                        break;
                    case "4009" : strMsg = "검체종류(유형)  '검체종류  기타정보 값이 없습니다' - 검체종류 코드값이 99일때";
                        break;
                    case "4010" : strMsg = "검사방법(유형)  '검사방법은  01, 02, 03, 04, 99 숫자와 ','만 입력해야 합니다'";
                        break;
                    case "4011" : strMsg = "검사방법(유형)  '검사방법  기타정보 값이 없습니다.' - 검사방법 코드값이 99일때";
                        break;
                    case "4012" : strMsg = "병원체코드 '병원체 코드값이  유효하지 않습니다, 배포가이드 문서를 참조하세요'";
                        break;
                    case "4013" : strMsg = "의뢰일자 '의뢰일은  8자리(YYYYMMDD)입니다'";
                        break;
                    case "4014" : strMsg = "의뢰일자 '의뢰일은 숫자로  입력하세요'";
                        break;
                    case "4015" : strMsg = "의뢰일자(의뢰일<=진단일)    '의뢰일은 진단일과 같거나  빨라야합니다.'";
                        break;
                    case "4016" : strMsg = "진단일자 '진단일은  8자리(YYYYMMDD)입니다'";
                        break;
                    case "4017" : strMsg = "진단일자 '진단일은 숫자로  입력하세요'";
                        break;
                    case "4018" : strMsg = "검사기관 담당자(진단의,  검사자) 성명   '검사기관담당자명은  최대 30자리로 입력하세요.'";
                        break;
                    case "5001" : strMsg = "인증정보 '사용자(기관)  인증정보가 웹신고시스템에 등록된 인증정보와 같지 않습니다'";
                        break;
                    default:
                        strMsg = "전송 에러 코드 : " + strCode + " 전산정보팀 연락 바랍니다.";
                            break;
                }
            }

            return strMsg;
        }

        // INFECTIOUS_SENDDATA
        public string OGCR { set; get; }                            // 사용자(기관) 인증정보
        public string PATNT_NM { set; get; }                        // 환자성명
        public string PATNT_IHIDNUM { set; get; }                   // 환자주민등록번호
        public string PATNT_REGIST_NO { set; get; }                 // 환자등록번호
        public string PRTCTOR_NM { set; get; }                      // 보호자성명
        public string PATNT_SEXDSTN_CD { set; get; }                // 환자성별코드
        public string PATNT_TELNO { set; get; }                     // 환자전화번호
        public string PATNT_MBTLNUM { set; get; }                   // 환자이동전화번호
        public string PATNT_RN_ZIP { set; get; }                    // 환자우편번호
        public string PATNT_RDNMADR { set; get; }                   // 환자도로명주소
        public string PATNT_RDNMADR_DTL { set; get; }               // 환자도로명주소상세
        public string RESDNC_INDSTNCT_AT { set; get; }              // 거주지불명여부
        public string IDNTY_UKNWN_AT { set; get; }                  // 신원미상여부
        public string PATNT_OCCP_CD { set; get; }                   // 환자직업코드
        public string OCCP_DTL_INFO { set; get; }                   // 직업상세정보
        public string ICDGRP_CD { set; get; }                       // 감염병군코드
        public string ICD_CD { set; get; }                          // 감염병코드
        public string EIDS_SYMPTMS { set; get; }                    // 신종감염병증후군증상
        public string ATFSS_DE { set; get; }                        // 발병일자
        public string DGNSS_DE { set; get; }                        // 진단일자
        public string STTEMNT_DE { set; get; }                      // 신고일자
        public string DSNDGNSS_INSPCT_RESULT_TY_CD { set; get; }    // 확진검사결과유형코드
        public string HSPTLZ_TY_CD { set; get; }                    // 입원유형코드
        public string PATNT_CL_CD { set; get; }                     // 환자분류코드
        public string DEATH_AT_CD { set; get; }                     // 사망여부코드
        public string MDLCNST_KCN_INSTT_ID { set; get; }            // 요양기관기호
        public string DOCTR_NM { set; get; }                        // 의사성명
        public string HSPTL_SWBSER { set; get; }                    // 병원소프트웨어개발사
        public string HSPTL_SWKND { set; get; }                     // 병원소프트웨어종류
        public string DPLCT_AT { set; get; }                        // 중복여부
        public string RSPNS_MSSAGE_TY { set; get; }                 // 응답메시지유형
        public string PARATYPHOID_GERM_INFO { set; get; }           // 파라티푸스균주정보
        public string DYSENTERY_GERM_INFO { set; get; }             // 세균성이질균주정보
        public string ENTGERM_GERM_INFO { set; get; }               // 장출혈성대장균감염증_균주정보
        public string ENTGERM_GERM_ETC_INFO { set; get; }           // 장출혈성대장균감염증균주기타정보
        public string SCRBTYPH_INSPCT_MTH { set; get; }             // 쯔쯔가무시증검사방법
        public string SCRBTYPH_INSPCT_RATE { set; get; }            // 쯔쯔가무시증검사비율
        public string SCRBTYPH_INSPCT_RATE_ETC { set; get; }        // 쯔쯔가무시증검사비율기타
        public string SFTS_TICK_BITE { set; get; }                  // 중증열성혈소판감소증후군진드기교상
        public string SFTS_SYMPTMS { set; get; }                    // 중증열성혈소판감소증후군증상
        public string SFTS_HSPTLZ_INFO { set; get; }                // 중증열성혈소판감소증후군입원정보
        public string RABIES_BITE_INFO { set; get; }                // 공수병교상정보
        public string RABIES_HSPTLZ_INFO { set; get; }              // 공수병입원정보
        public string RABIES_TRTMNT { set; get; }                   // 공수병처치
        public string RM_INFO { set; get; }                   // 기타 특이 사항

    }
}
