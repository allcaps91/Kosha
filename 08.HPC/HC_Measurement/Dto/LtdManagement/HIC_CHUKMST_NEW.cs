namespace HC_Measurement.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HIC_CHUKMST_NEW : BaseDto
    {

        /// <summary>
        /// 일련번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 작업년도
        /// </summary>
        public string CHKYEAR { get; set; }

        /// <summary>
        /// 우편번호
        /// </summary>
        public string MAILCODE { get; set; }

        /// <summary>
        /// 사업장주소
        /// </summary>
        public string JUSO { get; set; }

        /// <summary>
        /// 반기
        /// </summary>
        public string BANGI { get; set; }

        /// <summary>
        /// 사업장코드
        /// </summary>
        public long LTDCODE { get; set; }

        /// <summary>
        /// 사업장명
        /// </summary>
        public string LTDNAME { get; set; }

        /// <summary>
        /// 상호명
        /// </summary>
        public string SANGHO { get; set; }

        /// <summary>
        /// 측정일 Fr
        /// </summary>
        public string SDATE { get; set; }

        /// <summary>
        /// 측정일 To
        /// </summary>
        public string EDATE { get; set; }

        /// <summary>
        /// 작성일자
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 작성자
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 작성자명
        /// </summary>
        public string ENTNAME { get; set; }

        /// <summary>
        /// 지정한계치
        /// </summary>
        public long T_LIMIT { get; set; }

        /// <summary>
        /// 5인이상 지정한계치 => 국고대상 누적
        /// </summary>
        public long T5_LIMIT { get; set; }

        /// <summary>
        /// 총누적
        /// </summary>
        public long TO_ACCUM { get; set; }

        /// <summary>
        /// 5인이상 누적
        /// </summary>
        public long T5_ACCUM { get; set; }

        /// <summary>
        /// 담당자
        /// </summary>
        public string LTD_MANAGER { get; set; }

        /// <summary>
        /// 직위
        /// </summary>
        public string LTD_GRADE { get; set; }

        /// <summary>
        /// 휴대전화
        /// </summary>
        public string LTD_HPHONE { get; set; }

        /// <summary>
        /// EMAIL
        /// </summary>
        public string LTD_EMAIL { get; set; }

        /// <summary>
        /// 국고지원여부
        /// </summary>
        public string GBSUPPORT { get; set; }

        /// <summary>
        /// 진행상황
        /// </summary>
        public string GBSTS { get; set; }

        /// <summary>
        /// 진행상황 명칭
        /// </summary>
        public string GBSTS_NM { get; set; }

        /// <summary>
        /// 진행상황
        /// </summary>
        public string GBSTSNAME { get; set; }

        /// <summary>
        /// Rowid
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 삭제일자
        /// </summary>
        public DateTime? DELDATE { get; set; }

        /// <summary>
        /// 삭제사번
        /// </summary>
        public long DELSABUN { get; set; }

        /// <summary>
        /// 삭제자
        /// </summary>
        public string DELNAME { get; set; }

        /// <summary>
        /// 대표자
        /// </summary>
        public string DAEPYO { get; set; }

        /// <summary>
        /// 사업장 전화번호
        /// </summary>
        public string TEL { get; set; }

        /// <summary>
        /// 담당자 팩스
        /// </summary>
        public string LTD_FAX { get; set; }

        /// <summary>
        /// 기존, 신규 구분
        /// </summary>
        public string GBNEW { get; set; }

        /// <summary>
        /// 방문, 유선 구분
        /// </summary>
        public string GBWAY { get; set; }

        /// <summary>
        /// 예상측정일수
        /// </summary>
        public long ILSU { get; set; }

        /// <summary>
        /// 예비조사일
        /// </summary>
        public string BDATE { get; set; }

        /// <summary>
        /// 총인원
        /// </summary>
        public long INWON { get; set; }

        /// <summary>
        /// 사무직 인원
        /// </summary>
        public long INWON_S { get; set; }

        /// <summary>
        /// 현장직 인원
        /// </summary>
        public long INWON_H { get; set; }

        /// <summary>
        /// 근로형태 주간여부
        /// </summary>
        public string GBDAY { get; set; }

        /// <summary>
        /// 근로형태 교대여부
        /// </summary>
        public string GBSHIFT { get; set; }

        /// <summary>
        /// 주간 근무시간
        /// </summary>
        public long DAYTIME { get; set; }

        /// <summary>
        /// 교대근무 조
        /// </summary>
        public long SHIFTGRPCNT { get; set; }

        /// <summary>
        /// 교대근무 교대
        /// </summary>
        public long SHIFTQUARTER { get; set; }

        /// <summary>
        /// 교대근무 시간
        /// </summary>
        public long SHIFTTIME { get; set; }

        /// <summary>
        /// 근로시간 1
        /// </summary>
        public string WORKTIME1 { get; set; }

        /// <summary>
        /// 근로시간 2
        /// </summary>
        public string WORKTIME2 { get; set; }

        /// <summary>
        /// 근로시간 3
        /// </summary>
        public string WORKTIME3 { get; set; }

        /// <summary>
        /// 근로시간 4
        /// </summary>
        public string WORKTIME4 { get; set; }

        /// <summary>
        /// 식사시간 1
        /// </summary>
        public string MEALTIME1 { get; set; }

        /// <summary>
        /// 식사시간 2
        /// </summary>
        public string MEALTIME2 { get; set; }

        /// <summary>
        /// 근로시간 1
        /// </summary>
        public string WORKTIME11 { get; set; }

        /// <summary>
        /// 근로시간 2
        /// </summary>
        public string WORKTIME22 { get; set; }

        /// <summary>
        /// 근로시간 3
        /// </summary>
        public string WORKTIME33 { get; set; }

        /// <summary>
        /// 근로시간 4
        /// </summary>
        public string WORKTIME44 { get; set; }

        /// <summary>
        /// 식사시간 1
        /// </summary>
        public string MEALTIME11 { get; set; }

        /// <summary>
        /// 식사시간 2
        /// </summary>
        public string MEALTIME22 { get; set; }

        /// <summary>
        /// 잔업유무
        /// </summary>
        public string GBOVERTIME { get; set; }

        /// <summary>
        /// 잔업시간
        /// </summary>
        public string OVERTIME { get; set; }

        /// <summary>
        /// 견적서요청 여부
        /// </summary>
        public string GBESTIMATE { get; set; }

        /// <summary>
        /// 노출기준보정 여부
        /// </summary>
        public string GBCORRECT { get; set; }

        /// <summary>
        /// 지역시료 여부
        /// </summary>
        public string GBSAMPLE { get; set; }

        /// <summary>
        /// 과산화수소 여부
        /// </summary>
        public string GBUCODE1 { get; set; }

        /// <summary>
        /// 시안화수소 여부
        /// </summary>
        public string GBUCODE2 { get; set; }

        /// <summary>
        /// 무수프탈산 여부
        /// </summary>
        public string GBUCODE3 { get; set; }

        /// <summary>
        /// 무수말레인 여부
        /// </summary>
        public string GBUCODE4 { get; set; }

        /// <summary>
        /// TDI, MDI 여부
        /// </summary>
        public string GBUCODE5 { get; set; }

        /// <summary>
        /// 6가크롬(불) 여부
        /// </summary>
        public string GBUCODE6 { get; set; }

        /// <summary>
        /// 작업내용 요약
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 6가크롬(불) 여부
        /// </summary>
        public string GBCHROMIUM { get; set; }

        /// <summary>
        /// 견적서등록 여부
        /// </summary>
        public string GBEST { get; set; }

        /// <summary>
        /// 최종 견적서등록 일시
        /// </summary>
        public DateTime? EST_DATE { get; set; }

        /// <summary>
        /// 최종 견적서등록 사번
        /// </summary>
        public long EST_SABUN { get; set; }

        /// <summary>
        /// 최종 견적서등록자 성명
        /// </summary>
        public string EST_JOBNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string IsDelete { get; set; }

        /// <summary>
        /// 수시측정 여부
        /// </summary>
        public string GBTEMP { get; set; }

        /// <summary>
        /// 사업장내 관리순번
        /// </summary>
        public long LTDSEQNO { get; set; }

        /// <summary>
        /// 사업장내 공사 및 공장명
        /// </summary>
        public string LTDGONGNAME { get; set; }

        /// <summary>
        /// 측정주기_공정신규변경_여부 (없음: 0, 있음: 1)
        /// </summary>
        public string CYCLE_PROCS_NEW_CHANGE_YN { get; set; }

        /// <summary>
        /// 측정주기_공정신규변경_일자
        /// </summary>
        public DateTime? CYCLE_PROCS_NEW_CHANGE_DATE { get; set; }

        /// <summary>
        /// 측정주기_전공정측정결과 (2년초과:0,1회초과:1,1회미만:2,2회미만:3)
        /// </summary>
        public string CYCLE_PROCS_WEM_RESULT { get; set; }

        /// <summary>
        /// 측정주기_발암성물질_노출초과여부 (없음: 0, 있음: 1)
        /// </summary>
        public string CYCLE_CRNGN_RDMTR_OVER_YN { get; set; }

        /// <summary>
        /// 측정주기_화학적인자_노출초과여부 (없음: 0, 있음: 1)
        /// </summary>
        public string CYCLE_CHMCLS_RDMTR_OVER_YN { get; set; }

        /// <summary>
        /// 측정주기_향후주기 (3월: 0, 6월: 1, 1년: 2)
        /// </summary>
        public string CYCLE_FUTR_WEM_CYCLE { get; set; }

        /// <summary>
        /// 측정주기_향후측정예상일자
        /// </summary>
        public DateTime? CYCLE_FUTR_WEM_PLAN_DATE { get; set; }

        /// <summary>
        /// 건강검진 자료사전 Table
        /// </summary>
        public HIC_CHUKMST_NEW()
        {
        }
    }
}
