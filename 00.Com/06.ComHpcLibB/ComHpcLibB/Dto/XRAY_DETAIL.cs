namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class XRAY_DETAIL : BaseDto
    {

        /// <summary>
        /// 외래 SLIP / 입원 ENTDATE
        /// </summary>
        public DateTime? ENTERDATE { get; set; }

        /// <summary>
        /// 입외구분(I.입원 O.외래)
        /// </summary>
        public string IPDOPD { get; set; }

        /// <summary>
        /// 예약여부
        /// </summary>
        public string GBRESERVED { get; set; }

        /// <summary>
        /// 촬영/예약일자(yy-mm-dd hh24:mi)
        /// </summary>
        public DateTime? SEEKDATE { get; set; }

        /// <summary>
        /// 병록번호
        /// </summary>
        public string PANO { get; set; }

        /// <summary>
        /// 수진자명
        /// </summary>
        public string SNAME { get; set; }

        /// <summary>
        /// 성별(M.남자  F.여자)
        /// </summary>
        public string SEX { get; set; }

        /// <summary>
        /// 나이
        /// </summary>
        public long AGE { get; set; }

        /// <summary>
        /// 과목
        /// </summary>
        public string DEPTCODE { get; set; }

        /// <summary>
        /// 의사코드
        /// </summary>
        public string DRCODE { get; set; }

        /// <summary>
        /// 병동
        /// </summary>
        public string WARDCODE { get; set; }

        /// <summary>
        /// 병실
        /// </summary>
        public string ROOMCODE { get; set; }

        /// <summary>
        /// 검사종류(아래참조)
        /// </summary>
        public string XJONG { get; set; }

        /// <summary>
        /// 치료부위(SubClass 코드)
        /// </summary>
        public string XSUBCODE { get; set; }

        /// <summary>
        /// 방사선 검사 코드
        /// </summary>
        public string XCODE { get; set; }

        /// <summary>
        /// 판독여부(판독결과지번호:WRTNO 미판독은 0,NULL)
        /// </summary>
        public long EXINFO { get; set; }

        /// <summary>
        /// 수량
        /// </summary>
        public long QTY { get; set; }

        /// <summary>
        /// 추가촬영정보(사용않함)
        /// </summary>
        public string EXMORE { get; set; }

        /// <summary>
        /// 촬영자 ID(촬영기사 사번)
        /// </summary>
        public long EXID { get; set; }

        /// <summary>
        /// 촬영완료구분(1:촬영완료):XuWork에서 등록함
        /// </summary>
        public string GBEND { get; set; }

        /// <summary>
        /// 관리번호(실소모량의 Index)
        /// </summary>
        public long MGRNO { get; set; }

        /// <summary>
        /// 여부 (Portable),Y / N
        /// </summary>
        public string GBPORTABLE { get; set; }

        /// <summary>
        /// 촬영방법(PA,AP등)
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 촬영실(촬영실코드 참조)
        /// </summary>
        public string XRAYROOM { get; set; }

        /// <summary>
        /// 당직(Null:주간 1.야간)
        /// </summary>
        public string GBNGT { get; set; }

        /// <summary>
        /// OCS 의사 컴멘트
        /// </summary>
        public string DRREMARK { get; set; }

        /// <summary>
        /// OCS 오더번호
        /// </summary>
        public long ORDERNO { get; set; }

        /// <summary>
        /// OCS 오드코드
        /// </summary>
        public string ORDERCODE { get; set; }

        /// <summary>
        /// PACS 고유번호(YYYYMMDD-nnnn)
        /// </summary>
        public string PACSNO { get; set; }

        /// <summary>
        /// 오더명칭
        /// </summary>
        public string ORDERNAME { get; set; }

        /// <summary>
        /// PACS의 영상 UID
        /// </summary>
        public string PACSSTUDYID { get; set; }

        /// <summary>
        /// PACS WorkList의 촬영완료(Y.완료)
        /// </summary>
        public string PACS_END { get; set; }

        /// <summary>
        /// 판독요청 여부(Y.판독요청)
        /// </summary>
        public string GBREAD { get; set; }

        /// <summary>
        /// 외부판독 의뢰일자 및 시각
        /// </summary>
        public DateTime? READ_SEND { get; set; }

        /// <summary>
        /// 외부판독 결과지 도착일자 및 시각
        /// </summary>
        public DateTime? READ_RECEIVE { get; set; }

        /// <summary>
        /// 외부판독 응급판독여부(Y or NULL)
        /// </summary>
        public string READ_FLAG { get; set; }

        /// <summary>
        /// 촬영 동의서
        /// </summary>
        public string AGREE { get; set; }

        /// <summary>
        /// 오더시간(2005/08/25부터)
        /// </summary>
        public DateTime? ORDERDATE { get; set; }

        /// <summary>
        /// 방사선접수시간(2005/08/25부터):HL7 전송 시각
        /// </summary>
        public DateTime? SENDDATE { get; set; }

        /// <summary>
        /// 영상전송시간(2005/08/25부터):촬영완료시간
        /// </summary>
        public DateTime? XSENDDATE { get; set; }

        /// <summary>
        /// PC에 백업일자(폴더명)
        /// </summary>
        public string PC_BACKDATE { get; set; }

        /// <summary>
        /// 판독의뢰 인쇄여부
        /// </summary>
        public string GBPRINT { get; set; }

        /// <summary>
        /// 예약검사 WRTNO (KOSMOS_PMPA.ETC_EXAM_RESERVED_MST)
        /// </summary>
        public long EXAM_WRTNO { get; set; }

        /// <summary>
        /// 처방의 판독지 출력일자
        /// </summary>
        public DateTime? DRDATE { get; set; }

        /// <summary>
        /// 처방의 판독번호
        /// </summary>
        public long DRWRTNO { get; set; }

        /// <summary>
        /// 자료이전 여부(NULL:이전안함, NOT NULL:이전완료)
        /// </summary>
        public long STUDY_REF { get; set; }

        /// <summary>
        /// 자료이전 일자 및 시각
        /// </summary>
        public DateTime? IMAGE_BDATE { get; set; }

        /// <summary>
        /// 사용안함
        /// </summary>
        public string GBHIC { get; set; }

        /// <summary>
        /// 검진 접수번호(WRTNO)/종검은 강제로 1 세팅
        /// </summary>
        public long HIC_WRTNO { get; set; }

        /// <summary>
        /// 검진 검사코드
        /// </summary>
        public string HIC_CODE { get; set; }

        /// <summary>
        /// 자격(2009/07/29 부터사용)
        /// </summary>
        public string BI { get; set; }

        /// <summary>
        /// 카덱스 간호사 수동작업 (삭제 : D, 수동확인 : 1 =>하단 확인일자에 추가)
        /// </summary>
        public string CADEX_DEL { get; set; }

        /// <summary>
        /// 처방일자(2010/09/03 부터사용)
        /// </summary>
        public string BDATE { get; set; }

        /// <summary>
        /// 근전도 결과(KOSMOS_OCS.ETC_RSEULT 의 WRTNO)
        /// </summary>
        public long EMGWRTNO { get; set; }

        /// <summary>
        /// 선택진료구분(1,0)
        /// </summary>
        public string GBSPC { get; set; }

/// <summary>
        /// 가예약일자 시간(단순)
        /// </summary>
        public DateTime? RDATE { get; set; }

        /// <summary>
        /// 상태 아래참조
        /// </summary>
        public string GBSTS { get; set; }

        /// <summary>
        /// 확인시간,자동시간
        /// </summary>
        public DateTime? CDATE { get; set; }

        /// <summary>
        /// 수납취소시간
        /// </summary>
        public DateTime? DELDATE { get; set; }

        /// <summary>
        /// 확인자사번
        /// </summary>
        public string CSABUN { get; set; }

        /// <summary>
        /// 확인 참고사항
        /// </summary>
        public string CREMARK { get; set; }

        /// <summary>
        /// 간호사전달구분(1.보류 2.오더취소(변경예정) 3.다음날촬영 4.촬영가능)
        /// </summary>
        public string N_STS { get; set; }

        /// <summary>
        /// 간호사전달참고사항
        /// </summary>
        public string N_REMARK { get; set; }

        /// <summary>
        /// 간호사전달시간
        /// </summary>
        public DateTime? N_ENTDATE { get; set; }

        /// <summary>
        /// 수동접수구분 Y 2013-04-22
        /// </summary>
        public string GB_MANUAL { get; set; }

        /// <summary>
        /// PICKUP 참고사항( CT, SONO에만 사용:금식관련)
        /// </summary>
        public string PICKUPREMARK { get; set; }

        /// <summary>
        /// 근전도 확인일자 2013-12-10
        /// </summary>
        public DateTime? CON_DATE { get; set; }

        /// <summary>
        /// 미시행수검자
        /// </summary>
        public DateTime? GDATE { get; set; }

        /// <summary>
        /// 미시행수검자사번
        /// </summary>
        public long GSABUN { get; set; }

        /// <summary>
        /// 오더 gbinfo - 2014-05-13
        /// </summary>
        public string GBINFO { get; set; }

        /// <summary>
        /// CVR대상표시 2015-03-16 (Y:대상>S:문자전송됨)
        /// </summary>
        public string CVR { get; set; }

        /// <summary>
        /// CVR대상 등록일자
        /// </summary>
        public DateTime? CVR_DATE { get; set; }

        /// <summary>
        /// CVR구분(1:촬영자, 2:판독자)
        /// </summary>
        public string CVR_GUBUN { get; set; }

        /// <summary>
        /// CVR 의사사번
        /// </summary>
        public long CVR_DRSABUN { get; set; }

        /// <summary>
        /// CVR 문자전송시간
        /// </summary>
        public DateTime? CVR_SEND { get; set; }

        /// <summary>
        /// 응급여부(E:응급)
        /// </summary>
        public string GBER { get; set; }

        /// <summary>
        /// 영상본시각(CVR용)
        /// </summary>
        public DateTime? CVR_CDATE { get; set; }

        /// <summary>
        /// 미국마취 신체등급 BAS_BCODE GUBUN=`마취_신체등급(ASA)`
        /// </summary>
        public string ASA { get; set; }

        /// <summary>
        /// 입력자
        /// </summary>
        public string INPS { get; set; }

        /// <summary>
        /// 최초시간
        /// </summary>
        public DateTime? INPT_DT { get; set; }

        /// <summary>
        /// 변경자
        /// </summary>
        public string UPPS { get; set; }

        /// <summary>
        /// 변경시간
        /// </summary>
        public DateTime? UP_DT { get; set; }

        public string RID { get; set; }
        
        /// <summary>
        /// 방사선 판독결과 자료
        /// </summary>
        public XRAY_DETAIL()
        {
        }
    }
}
